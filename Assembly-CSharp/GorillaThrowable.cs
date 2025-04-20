using System;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000658 RID: 1624
public class GorillaThrowable : MonoBehaviourPun, IPunObservable, IPhotonViewCallback
{
	// Token: 0x0600283C RID: 10300 RVA: 0x001102AC File Offset: 0x0010E4AC
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

	// Token: 0x0600283D RID: 10301 RVA: 0x001103CC File Offset: 0x0010E5CC
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

	// Token: 0x0600283E RID: 10302 RVA: 0x00030607 File Offset: 0x0002E807
	private void IsHandPushing(XRNode node)
	{
	}

	// Token: 0x0600283F RID: 10303 RVA: 0x00110538 File Offset: 0x0010E738
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

	// Token: 0x06002840 RID: 10304 RVA: 0x00110764 File Offset: 0x0010E964
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

	// Token: 0x06002841 RID: 10305 RVA: 0x001107DC File Offset: 0x0010E9DC
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

	// Token: 0x06002842 RID: 10306 RVA: 0x00110978 File Offset: 0x0010EB78
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

	// Token: 0x06002843 RID: 10307 RVA: 0x00110A08 File Offset: 0x0010EC08
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

	// Token: 0x06002844 RID: 10308 RVA: 0x00110A84 File Offset: 0x0010EC84
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

	// Token: 0x06002845 RID: 10309 RVA: 0x00110B20 File Offset: 0x0010ED20
	public float InterpolateVolume()
	{
		return (Mathf.Clamp(this.rigidbody.velocity.magnitude, this.minVelocity, this.maxVelocity) - this.minVelocity) / (this.maxVelocity - this.minVelocity) * (this.maxVolume - this.minVolume) + this.minVolume;
	}

	// Token: 0x04002D7A RID: 11642
	public int trackingHistorySize;

	// Token: 0x04002D7B RID: 11643
	public float throwMultiplier;

	// Token: 0x04002D7C RID: 11644
	public float throwMagnitudeLimit;

	// Token: 0x04002D7D RID: 11645
	private Vector3[] velocityHistory;

	// Token: 0x04002D7E RID: 11646
	private Vector3[] headsetVelocityHistory;

	// Token: 0x04002D7F RID: 11647
	private Vector3[] positionHistory;

	// Token: 0x04002D80 RID: 11648
	private Vector3[] headsetPositionHistory;

	// Token: 0x04002D81 RID: 11649
	private Vector3[] rotationHistory;

	// Token: 0x04002D82 RID: 11650
	private Vector3[] rotationalVelocityHistory;

	// Token: 0x04002D83 RID: 11651
	private Vector3 previousPosition;

	// Token: 0x04002D84 RID: 11652
	private Vector3 previousRotation;

	// Token: 0x04002D85 RID: 11653
	private Vector3 previousHeadsetPosition;

	// Token: 0x04002D86 RID: 11654
	private int currentIndex;

	// Token: 0x04002D87 RID: 11655
	private Vector3 currentVelocity;

	// Token: 0x04002D88 RID: 11656
	private Vector3 currentHeadsetVelocity;

	// Token: 0x04002D89 RID: 11657
	private Vector3 currentRotationalVelocity;

	// Token: 0x04002D8A RID: 11658
	public Vector3 denormalizedVelocityAverage;

	// Token: 0x04002D8B RID: 11659
	private Vector3 denormalizedHeadsetVelocityAverage;

	// Token: 0x04002D8C RID: 11660
	private Vector3 denormalizedRotationalVelocityAverage;

	// Token: 0x04002D8D RID: 11661
	private Transform headsetTransform;

	// Token: 0x04002D8E RID: 11662
	private Vector3 targetPosition;

	// Token: 0x04002D8F RID: 11663
	private Quaternion targetRotation;

	// Token: 0x04002D90 RID: 11664
	public bool initialLerp;

	// Token: 0x04002D91 RID: 11665
	public float lerpValue = 0.4f;

	// Token: 0x04002D92 RID: 11666
	public float lerpDistanceLimit = 0.01f;

	// Token: 0x04002D93 RID: 11667
	public bool isHeld;

	// Token: 0x04002D94 RID: 11668
	public Rigidbody rigidbody;

	// Token: 0x04002D95 RID: 11669
	private int loopIndex;

	// Token: 0x04002D96 RID: 11670
	private Transform transformToFollow;

	// Token: 0x04002D97 RID: 11671
	private Vector3 offset;

	// Token: 0x04002D98 RID: 11672
	private Quaternion offsetRotation;

	// Token: 0x04002D99 RID: 11673
	public AudioSource audioSource;

	// Token: 0x04002D9A RID: 11674
	public int timeLastReceived;

	// Token: 0x04002D9B RID: 11675
	public bool synchThrow;

	// Token: 0x04002D9C RID: 11676
	public float tempFloat;

	// Token: 0x04002D9D RID: 11677
	public Transform grabbingTransform;

	// Token: 0x04002D9E RID: 11678
	public float pickupLerp;

	// Token: 0x04002D9F RID: 11679
	public float minVelocity;

	// Token: 0x04002DA0 RID: 11680
	public float maxVelocity;

	// Token: 0x04002DA1 RID: 11681
	public float minVolume;

	// Token: 0x04002DA2 RID: 11682
	public float maxVolume;

	// Token: 0x04002DA3 RID: 11683
	public bool isLinear;

	// Token: 0x04002DA4 RID: 11684
	public float linearMax;

	// Token: 0x04002DA5 RID: 11685
	public float exponThrowMultMax;

	// Token: 0x04002DA6 RID: 11686
	public int bounceAudioClip;
}
