using System;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200068E RID: 1678
public class GorillaThrowable : MonoBehaviourPun, IPunObservable, IPhotonViewCallback
{
	// Token: 0x060029BD RID: 10685 RVA: 0x000CF0B0 File Offset: 0x000CD2B0
	public virtual void Start()
	{
		this.offset = Vector3.zero;
		this.headsetTransform = GTPlayer.Instance.headCollider.transform;
		this.velocityHistory = new Vector3[this.trackingHistorySize];
		this.positionHistory = new Vector3[this.trackingHistorySize];
		this.headsetPositionHistory = new Vector3[this.trackingHistorySize];
		this.rotationHistory = new Vector3[this.trackingHistorySize];
		this.rotationalVelocityHistory = new Vector3[this.trackingHistorySize];
		for (int i = 0; i < this.trackingHistorySize; i++)
		{
			this.velocityHistory[i] = Vector3.zero;
			this.positionHistory[i] = base.transform.position - this.headsetTransform.position;
			this.headsetPositionHistory[i] = this.headsetTransform.position;
			this.rotationHistory[i] = base.transform.eulerAngles;
			this.rotationalVelocityHistory[i] = Vector3.zero;
		}
		this.currentIndex = 0;
		this.rigidbody = base.GetComponentInChildren<Rigidbody>();
	}

	// Token: 0x060029BE RID: 10686 RVA: 0x000CF1D0 File Offset: 0x000CD3D0
	public virtual void LateUpdate()
	{
		if (this.isHeld && base.photonView.IsMine)
		{
			base.transform.rotation = this.transformToFollow.rotation * this.offsetRotation;
			if (!this.initialLerp && (base.transform.position - this.transformToFollow.position).magnitude > this.lerpDistanceLimit)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, this.transformToFollow.position + this.transformToFollow.rotation * this.offset, this.pickupLerp);
			}
			else
			{
				this.initialLerp = true;
				base.transform.position = this.transformToFollow.position + this.transformToFollow.rotation * this.offset;
			}
		}
		if (!base.photonView.IsMine)
		{
			this.rigidbody.isKinematic = true;
			base.transform.position = Vector3.Lerp(base.transform.position, this.targetPosition, this.lerpValue);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.targetRotation, this.lerpValue);
		}
		this.StoreHistories();
	}

	// Token: 0x060029BF RID: 10687 RVA: 0x000023F4 File Offset: 0x000005F4
	private void IsHandPushing(XRNode node)
	{
	}

	// Token: 0x060029C0 RID: 10688 RVA: 0x000CF33C File Offset: 0x000CD53C
	private void StoreHistories()
	{
		this.previousPosition = this.positionHistory[this.currentIndex];
		this.previousRotation = this.rotationHistory[this.currentIndex];
		this.previousHeadsetPosition = this.headsetPositionHistory[this.currentIndex];
		this.currentIndex = (this.currentIndex + 1) % this.trackingHistorySize;
		this.currentVelocity = (base.transform.position - this.headsetTransform.position - this.previousPosition) / Time.deltaTime;
		this.currentHeadsetVelocity = (this.headsetTransform.position - this.previousHeadsetPosition) / Time.deltaTime;
		this.currentRotationalVelocity = (base.transform.eulerAngles - this.previousRotation) / Time.deltaTime;
		this.denormalizedVelocityAverage = Vector3.zero;
		this.denormalizedRotationalVelocityAverage = Vector3.zero;
		this.loopIndex = 0;
		while (this.loopIndex < this.trackingHistorySize)
		{
			this.denormalizedVelocityAverage += this.velocityHistory[this.loopIndex];
			this.denormalizedRotationalVelocityAverage += this.rotationalVelocityHistory[this.loopIndex];
			this.loopIndex++;
		}
		this.denormalizedVelocityAverage /= (float)this.trackingHistorySize;
		this.denormalizedRotationalVelocityAverage /= (float)this.trackingHistorySize;
		this.velocityHistory[this.currentIndex] = this.currentVelocity;
		this.positionHistory[this.currentIndex] = base.transform.position - this.headsetTransform.position;
		this.headsetPositionHistory[this.currentIndex] = this.headsetTransform.position;
		this.rotationHistory[this.currentIndex] = base.transform.eulerAngles;
		this.rotationalVelocityHistory[this.currentIndex] = this.currentRotationalVelocity;
	}

	// Token: 0x060029C1 RID: 10689 RVA: 0x000CF568 File Offset: 0x000CD768
	public virtual void Grabbed(Transform grabTransform)
	{
		this.grabbingTransform = grabTransform;
		this.isHeld = true;
		this.transformToFollow = this.grabbingTransform;
		this.offsetRotation = base.transform.rotation * Quaternion.Inverse(this.transformToFollow.rotation);
		this.initialLerp = false;
		this.rigidbody.isKinematic = true;
		this.rigidbody.useGravity = false;
		base.photonView.RequestOwnership();
	}

	// Token: 0x060029C2 RID: 10690 RVA: 0x000CF5E0 File Offset: 0x000CD7E0
	public virtual void ThrowThisThingo()
	{
		this.transformToFollow = null;
		this.isHeld = false;
		this.synchThrow = true;
		this.rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		this.rigidbody.isKinematic = false;
		this.rigidbody.useGravity = true;
		if (this.isLinear || this.denormalizedVelocityAverage.magnitude < this.linearMax)
		{
			if (this.denormalizedVelocityAverage.magnitude * this.throwMultiplier < this.throwMagnitudeLimit)
			{
				this.rigidbody.velocity = this.denormalizedVelocityAverage * this.throwMultiplier + this.currentHeadsetVelocity;
			}
			else
			{
				this.rigidbody.velocity = this.denormalizedVelocityAverage.normalized * this.throwMagnitudeLimit + this.currentHeadsetVelocity;
			}
		}
		else
		{
			this.rigidbody.velocity = this.denormalizedVelocityAverage.normalized * Mathf.Max(Mathf.Min(Mathf.Pow(this.throwMultiplier * this.denormalizedVelocityAverage.magnitude / this.linearMax, this.exponThrowMultMax), 0.1f) * this.denormalizedHeadsetVelocityAverage.magnitude, this.throwMagnitudeLimit) + this.currentHeadsetVelocity;
		}
		this.rigidbody.angularVelocity = this.denormalizedRotationalVelocityAverage * 3.1415927f / 180f;
		this.rigidbody.MovePosition(this.rigidbody.transform.position + this.rigidbody.velocity * Time.deltaTime);
	}

	// Token: 0x060029C3 RID: 10691 RVA: 0x000CF77C File Offset: 0x000CD97C
	void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
			stream.SendNext(this.rigidbody.velocity);
			return;
		}
		this.targetPosition = (Vector3)stream.ReceiveNext();
		this.targetRotation = (Quaternion)stream.ReceiveNext();
		this.rigidbody.velocity = (Vector3)stream.ReceiveNext();
	}

	// Token: 0x060029C4 RID: 10692 RVA: 0x000CF80C File Offset: 0x000CDA0C
	public virtual void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.GetComponent<GorillaSurfaceOverride>() != null)
		{
			if (NetworkSystem.Instance.InRoom)
			{
				base.photonView.RPC("PlaySurfaceHit", RpcTarget.Others, new object[]
				{
					this.bounceAudioClip,
					this.InterpolateVolume()
				});
			}
			this.PlaySurfaceHit(collision.collider.GetComponent<GorillaSurfaceOverride>().overrideIndex, this.InterpolateVolume());
		}
	}

	// Token: 0x060029C5 RID: 10693 RVA: 0x000CF888 File Offset: 0x000CDA88
	[PunRPC]
	public void PlaySurfaceHit(int soundIndex, float tapVolume)
	{
		if (soundIndex > -1 && soundIndex < GTPlayer.Instance.materialData.Count)
		{
			this.audioSource.volume = tapVolume;
			this.audioSource.clip = (GTPlayer.Instance.materialData[soundIndex].overrideAudio ? GTPlayer.Instance.materialData[soundIndex].audio : GTPlayer.Instance.materialData[0].audio);
			this.audioSource.GTPlayOneShot(this.audioSource.clip, 1f);
		}
	}

	// Token: 0x060029C6 RID: 10694 RVA: 0x000CF924 File Offset: 0x000CDB24
	public float InterpolateVolume()
	{
		return (Mathf.Clamp(this.rigidbody.velocity.magnitude, this.minVelocity, this.maxVelocity) - this.minVelocity) / (this.maxVelocity - this.minVelocity) * (this.maxVolume - this.minVolume) + this.minVolume;
	}

	// Token: 0x04002F0A RID: 12042
	public int trackingHistorySize;

	// Token: 0x04002F0B RID: 12043
	public float throwMultiplier;

	// Token: 0x04002F0C RID: 12044
	public float throwMagnitudeLimit;

	// Token: 0x04002F0D RID: 12045
	private Vector3[] velocityHistory;

	// Token: 0x04002F0E RID: 12046
	private Vector3[] headsetVelocityHistory;

	// Token: 0x04002F0F RID: 12047
	private Vector3[] positionHistory;

	// Token: 0x04002F10 RID: 12048
	private Vector3[] headsetPositionHistory;

	// Token: 0x04002F11 RID: 12049
	private Vector3[] rotationHistory;

	// Token: 0x04002F12 RID: 12050
	private Vector3[] rotationalVelocityHistory;

	// Token: 0x04002F13 RID: 12051
	private Vector3 previousPosition;

	// Token: 0x04002F14 RID: 12052
	private Vector3 previousRotation;

	// Token: 0x04002F15 RID: 12053
	private Vector3 previousHeadsetPosition;

	// Token: 0x04002F16 RID: 12054
	private int currentIndex;

	// Token: 0x04002F17 RID: 12055
	private Vector3 currentVelocity;

	// Token: 0x04002F18 RID: 12056
	private Vector3 currentHeadsetVelocity;

	// Token: 0x04002F19 RID: 12057
	private Vector3 currentRotationalVelocity;

	// Token: 0x04002F1A RID: 12058
	public Vector3 denormalizedVelocityAverage;

	// Token: 0x04002F1B RID: 12059
	private Vector3 denormalizedHeadsetVelocityAverage;

	// Token: 0x04002F1C RID: 12060
	private Vector3 denormalizedRotationalVelocityAverage;

	// Token: 0x04002F1D RID: 12061
	private Transform headsetTransform;

	// Token: 0x04002F1E RID: 12062
	private Vector3 targetPosition;

	// Token: 0x04002F1F RID: 12063
	private Quaternion targetRotation;

	// Token: 0x04002F20 RID: 12064
	public bool initialLerp;

	// Token: 0x04002F21 RID: 12065
	public float lerpValue = 0.4f;

	// Token: 0x04002F22 RID: 12066
	public float lerpDistanceLimit = 0.01f;

	// Token: 0x04002F23 RID: 12067
	public bool isHeld;

	// Token: 0x04002F24 RID: 12068
	public Rigidbody rigidbody;

	// Token: 0x04002F25 RID: 12069
	private int loopIndex;

	// Token: 0x04002F26 RID: 12070
	private Transform transformToFollow;

	// Token: 0x04002F27 RID: 12071
	private Vector3 offset;

	// Token: 0x04002F28 RID: 12072
	private Quaternion offsetRotation;

	// Token: 0x04002F29 RID: 12073
	public AudioSource audioSource;

	// Token: 0x04002F2A RID: 12074
	public int timeLastReceived;

	// Token: 0x04002F2B RID: 12075
	public bool synchThrow;

	// Token: 0x04002F2C RID: 12076
	public float tempFloat;

	// Token: 0x04002F2D RID: 12077
	public Transform grabbingTransform;

	// Token: 0x04002F2E RID: 12078
	public float pickupLerp;

	// Token: 0x04002F2F RID: 12079
	public float minVelocity;

	// Token: 0x04002F30 RID: 12080
	public float maxVelocity;

	// Token: 0x04002F31 RID: 12081
	public float minVolume;

	// Token: 0x04002F32 RID: 12082
	public float maxVolume;

	// Token: 0x04002F33 RID: 12083
	public bool isLinear;

	// Token: 0x04002F34 RID: 12084
	public float linearMax;

	// Token: 0x04002F35 RID: 12085
	public float exponThrowMultMax;

	// Token: 0x04002F36 RID: 12086
	public int bounceAudioClip;
}
