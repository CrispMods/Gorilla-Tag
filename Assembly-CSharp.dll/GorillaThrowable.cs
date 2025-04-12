using System;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200068F RID: 1679
public class GorillaThrowable : MonoBehaviourPun, IPunObservable, IPhotonViewCallback
{
	// Token: 0x060029C5 RID: 10693 RVA: 0x00116EA4 File Offset: 0x001150A4
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

	// Token: 0x060029C6 RID: 10694 RVA: 0x00116FC4 File Offset: 0x001151C4
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

	// Token: 0x060029C7 RID: 10695 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void IsHandPushing(XRNode node)
	{
	}

	// Token: 0x060029C8 RID: 10696 RVA: 0x00117130 File Offset: 0x00115330
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

	// Token: 0x060029C9 RID: 10697 RVA: 0x0011735C File Offset: 0x0011555C
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

	// Token: 0x060029CA RID: 10698 RVA: 0x001173D4 File Offset: 0x001155D4
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

	// Token: 0x060029CB RID: 10699 RVA: 0x00117570 File Offset: 0x00115770
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

	// Token: 0x060029CC RID: 10700 RVA: 0x00117600 File Offset: 0x00115800
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

	// Token: 0x060029CD RID: 10701 RVA: 0x0011767C File Offset: 0x0011587C
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

	// Token: 0x060029CE RID: 10702 RVA: 0x00117718 File Offset: 0x00115918
	public float InterpolateVolume()
	{
		return (Mathf.Clamp(this.rigidbody.velocity.magnitude, this.minVelocity, this.maxVelocity) - this.minVelocity) / (this.maxVelocity - this.minVelocity) * (this.maxVolume - this.minVolume) + this.minVolume;
	}

	// Token: 0x04002F10 RID: 12048
	public int trackingHistorySize;

	// Token: 0x04002F11 RID: 12049
	public float throwMultiplier;

	// Token: 0x04002F12 RID: 12050
	public float throwMagnitudeLimit;

	// Token: 0x04002F13 RID: 12051
	private Vector3[] velocityHistory;

	// Token: 0x04002F14 RID: 12052
	private Vector3[] headsetVelocityHistory;

	// Token: 0x04002F15 RID: 12053
	private Vector3[] positionHistory;

	// Token: 0x04002F16 RID: 12054
	private Vector3[] headsetPositionHistory;

	// Token: 0x04002F17 RID: 12055
	private Vector3[] rotationHistory;

	// Token: 0x04002F18 RID: 12056
	private Vector3[] rotationalVelocityHistory;

	// Token: 0x04002F19 RID: 12057
	private Vector3 previousPosition;

	// Token: 0x04002F1A RID: 12058
	private Vector3 previousRotation;

	// Token: 0x04002F1B RID: 12059
	private Vector3 previousHeadsetPosition;

	// Token: 0x04002F1C RID: 12060
	private int currentIndex;

	// Token: 0x04002F1D RID: 12061
	private Vector3 currentVelocity;

	// Token: 0x04002F1E RID: 12062
	private Vector3 currentHeadsetVelocity;

	// Token: 0x04002F1F RID: 12063
	private Vector3 currentRotationalVelocity;

	// Token: 0x04002F20 RID: 12064
	public Vector3 denormalizedVelocityAverage;

	// Token: 0x04002F21 RID: 12065
	private Vector3 denormalizedHeadsetVelocityAverage;

	// Token: 0x04002F22 RID: 12066
	private Vector3 denormalizedRotationalVelocityAverage;

	// Token: 0x04002F23 RID: 12067
	private Transform headsetTransform;

	// Token: 0x04002F24 RID: 12068
	private Vector3 targetPosition;

	// Token: 0x04002F25 RID: 12069
	private Quaternion targetRotation;

	// Token: 0x04002F26 RID: 12070
	public bool initialLerp;

	// Token: 0x04002F27 RID: 12071
	public float lerpValue = 0.4f;

	// Token: 0x04002F28 RID: 12072
	public float lerpDistanceLimit = 0.01f;

	// Token: 0x04002F29 RID: 12073
	public bool isHeld;

	// Token: 0x04002F2A RID: 12074
	public Rigidbody rigidbody;

	// Token: 0x04002F2B RID: 12075
	private int loopIndex;

	// Token: 0x04002F2C RID: 12076
	private Transform transformToFollow;

	// Token: 0x04002F2D RID: 12077
	private Vector3 offset;

	// Token: 0x04002F2E RID: 12078
	private Quaternion offsetRotation;

	// Token: 0x04002F2F RID: 12079
	public AudioSource audioSource;

	// Token: 0x04002F30 RID: 12080
	public int timeLastReceived;

	// Token: 0x04002F31 RID: 12081
	public bool synchThrow;

	// Token: 0x04002F32 RID: 12082
	public float tempFloat;

	// Token: 0x04002F33 RID: 12083
	public Transform grabbingTransform;

	// Token: 0x04002F34 RID: 12084
	public float pickupLerp;

	// Token: 0x04002F35 RID: 12085
	public float minVelocity;

	// Token: 0x04002F36 RID: 12086
	public float maxVelocity;

	// Token: 0x04002F37 RID: 12087
	public float minVolume;

	// Token: 0x04002F38 RID: 12088
	public float maxVolume;

	// Token: 0x04002F39 RID: 12089
	public bool isLinear;

	// Token: 0x04002F3A RID: 12090
	public float linearMax;

	// Token: 0x04002F3B RID: 12091
	public float exponThrowMultMax;

	// Token: 0x04002F3C RID: 12092
	public int bounceAudioClip;
}
