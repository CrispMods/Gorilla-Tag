using System;
using GorillaExtensions;
using GorillaLocomotion;
using GT_CustomMapSupportRuntime;
using UnityEngine;

// Token: 0x02000791 RID: 1937
public class ForceVolume : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06002F9F RID: 12191 RVA: 0x0004FB5C File Offset: 0x0004DD5C
	private void Awake()
	{
		this.volume = base.GetComponent<Collider>();
		this.audioState = ForceVolume.AudioState.None;
	}

	// Token: 0x06002FA0 RID: 12192 RVA: 0x000320BF File Offset: 0x000302BF
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06002FA1 RID: 12193 RVA: 0x000320C8 File Offset: 0x000302C8
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06002FA2 RID: 12194 RVA: 0x0012CD6C File Offset: 0x0012AF6C
	public void SliceUpdate()
	{
		if (this.audioSource && this.audioSource != null && !this.audioSource.isPlaying && this.audioSource.enabled)
		{
			this.audioSource.enabled = false;
		}
	}

	// Token: 0x06002FA3 RID: 12195 RVA: 0x0012CDBC File Offset: 0x0012AFBC
	private bool TriggerFilter(Collider other, out Rigidbody rb, out Transform xf)
	{
		rb = null;
		xf = null;
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject)
		{
			rb = GorillaTagger.Instance.GetComponent<Rigidbody>();
			xf = GorillaTagger.Instance.headCollider.GetComponent<Transform>();
		}
		return rb != null && xf != null;
	}

	// Token: 0x06002FA4 RID: 12196 RVA: 0x0012CE1C File Offset: 0x0012B01C
	public void OnTriggerEnter(Collider other)
	{
		Rigidbody rigidbody = null;
		Transform transform = null;
		if (!this.TriggerFilter(other, out rigidbody, out transform))
		{
			return;
		}
		if (this.enterClip == null)
		{
			return;
		}
		if (this.audioSource)
		{
			this.audioSource.enabled = true;
			this.audioSource.GTPlayOneShot(this.enterClip, 1f);
			this.audioState = ForceVolume.AudioState.Enter;
		}
		this.enterPos = transform.position;
	}

	// Token: 0x06002FA5 RID: 12197 RVA: 0x0012CE8C File Offset: 0x0012B08C
	public void OnTriggerExit(Collider other)
	{
		Rigidbody rigidbody = null;
		Transform transform = null;
		if (!this.TriggerFilter(other, out rigidbody, out transform))
		{
			return;
		}
		if (this.audioSource)
		{
			this.audioSource.enabled = true;
			this.audioSource.GTPlayOneShot(this.exitClip, 1f);
			this.audioState = ForceVolume.AudioState.None;
		}
	}

	// Token: 0x06002FA6 RID: 12198 RVA: 0x0012CEE4 File Offset: 0x0012B0E4
	public void OnTriggerStay(Collider other)
	{
		Rigidbody rigidbody = null;
		Transform transform = null;
		if (!this.TriggerFilter(other, out rigidbody, out transform))
		{
			return;
		}
		if (this.audioSource && !this.audioSource.isPlaying)
		{
			ForceVolume.AudioState audioState = this.audioState;
			if (audioState != ForceVolume.AudioState.Enter)
			{
				if (audioState == ForceVolume.AudioState.Loop)
				{
					if (this.loopClip != null)
					{
						this.audioSource.enabled = true;
						this.audioSource.GTPlayOneShot(this.loopClip, 1f);
					}
					this.audioState = ForceVolume.AudioState.Loop;
				}
			}
			else
			{
				if (this.loopCresendoClip != null)
				{
					this.audioSource.enabled = true;
					this.audioSource.GTPlayOneShot(this.loopCresendoClip, 1f);
				}
				this.audioState = ForceVolume.AudioState.Crescendo;
			}
		}
		if (this.disableGrip)
		{
			GTPlayer.Instance.SetMaximumSlipThisFrame();
		}
		SizeManager sizeManager = null;
		if (this.scaleWithSize)
		{
			sizeManager = rigidbody.GetComponent<SizeManager>();
		}
		Vector3 vector = rigidbody.velocity;
		if (this.scaleWithSize && sizeManager)
		{
			vector /= sizeManager.currentScale;
		}
		Vector3 b = Vector3.Dot(transform.position - base.transform.position, base.transform.up) * base.transform.up;
		Vector3 a = base.transform.position + b - transform.position;
		float num = a.magnitude + 0.0001f;
		Vector3 vector2 = a / num;
		float num2 = Vector3.Dot(vector, vector2);
		float d = this.accel;
		if (this.maxDepth > -1f)
		{
			float num3 = Vector3.Dot(transform.position - this.enterPos, vector2);
			float num4 = this.maxDepth - num3;
			float b2 = 0f;
			if (num4 > 0.0001f)
			{
				b2 = num2 * num2 / num4;
			}
			d = Mathf.Max(this.accel, b2);
		}
		float deltaTime = Time.deltaTime;
		Vector3 b3 = base.transform.up * d * deltaTime;
		vector += b3;
		Vector3 a2 = Mathf.Min(Vector3.Dot(vector, base.transform.up), this.maxSpeed) * base.transform.up;
		Vector3 a3 = Vector3.Dot(vector, base.transform.right) * base.transform.right;
		Vector3 a4 = Vector3.Dot(vector, base.transform.forward) * base.transform.forward;
		float d2 = 1f;
		float d3 = 1f;
		if (this.dampenLateralVelocity)
		{
			d2 = 1f - this.dampenXVelPerc * 0.01f * deltaTime;
			d3 = 1f - this.dampenZVelPerc * 0.01f * deltaTime;
		}
		vector = a2 + d2 * a3 + d3 * a4;
		if (this.applyPullToCenterAcceleration && this.pullToCenterAccel > 0f && this.pullToCenterMaxSpeed > 0f)
		{
			vector -= num2 * vector2;
			if (num > this.pullTOCenterMinDistance)
			{
				num2 += this.pullToCenterAccel * deltaTime;
				float b4 = Mathf.Min(this.pullToCenterMaxSpeed, num / deltaTime);
				num2 = Mathf.Min(num2, b4);
			}
			else
			{
				num2 = 0f;
			}
			vector += num2 * vector2;
			if (vector.magnitude > 0.0001f)
			{
				Vector3 vector3 = Vector3.Cross(base.transform.up, vector2);
				float magnitude = vector3.magnitude;
				if (magnitude > 0.0001f)
				{
					vector3 /= magnitude;
					num2 = Vector3.Dot(vector, vector3);
					vector -= num2 * vector3;
					num2 -= this.pullToCenterAccel * deltaTime;
					num2 = Mathf.Max(0f, num2);
					vector += num2 * vector3;
				}
			}
		}
		if (this.scaleWithSize && sizeManager)
		{
			vector *= sizeManager.currentScale;
		}
		rigidbody.velocity = vector;
	}

	// Token: 0x06002FA7 RID: 12199 RVA: 0x0012D2FC File Offset: 0x0012B4FC
	public void OnDrawGizmosSelected()
	{
		base.GetComponents<Collider>();
		Gizmos.color = Color.magenta;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.pullTOCenterMinDistance / base.transform.lossyScale.x, 1f, this.pullTOCenterMinDistance / base.transform.lossyScale.z));
	}

	// Token: 0x06002FA8 RID: 12200 RVA: 0x0012D36C File Offset: 0x0012B56C
	public void SetPropertiesFromPlaceholder(ForceVolumeProperties properties, AudioSource volumeAudioSource, Collider colliderVolume)
	{
		this.accel = properties.accel;
		this.maxDepth = properties.maxDepth;
		this.maxSpeed = properties.maxSpeed;
		this.disableGrip = properties.disableGrip;
		this.dampenLateralVelocity = properties.dampenLateralVelocity;
		this.dampenXVelPerc = properties.dampenXVel;
		this.dampenZVelPerc = properties.dampenZVel;
		this.applyPullToCenterAcceleration = properties.applyPullToCenterAcceleration;
		this.pullToCenterAccel = properties.pullToCenterAccel;
		this.pullToCenterMaxSpeed = properties.pullToCenterMaxSpeed;
		this.pullTOCenterMinDistance = properties.pullToCenterMinDistance;
		this.enterClip = properties.enterClip;
		this.exitClip = properties.exitClip;
		this.loopClip = properties.loopClip;
		this.loopCresendoClip = properties.loopCrescendoClip;
		if (volumeAudioSource.IsNotNull())
		{
			this.audioSource = volumeAudioSource;
		}
		if (colliderVolume.IsNotNull())
		{
			this.volume = colliderVolume;
		}
	}

	// Token: 0x06002FAA RID: 12202 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040033C6 RID: 13254
	[SerializeField]
	public bool scaleWithSize = true;

	// Token: 0x040033C7 RID: 13255
	[SerializeField]
	private float accel;

	// Token: 0x040033C8 RID: 13256
	[SerializeField]
	private float maxDepth = -1f;

	// Token: 0x040033C9 RID: 13257
	[SerializeField]
	private float maxSpeed;

	// Token: 0x040033CA RID: 13258
	[SerializeField]
	private bool disableGrip;

	// Token: 0x040033CB RID: 13259
	[SerializeField]
	private bool dampenLateralVelocity = true;

	// Token: 0x040033CC RID: 13260
	[SerializeField]
	private float dampenXVelPerc;

	// Token: 0x040033CD RID: 13261
	[SerializeField]
	private float dampenZVelPerc;

	// Token: 0x040033CE RID: 13262
	[SerializeField]
	private bool applyPullToCenterAcceleration = true;

	// Token: 0x040033CF RID: 13263
	[SerializeField]
	private float pullToCenterAccel;

	// Token: 0x040033D0 RID: 13264
	[SerializeField]
	private float pullToCenterMaxSpeed;

	// Token: 0x040033D1 RID: 13265
	[SerializeField]
	private float pullTOCenterMinDistance = 0.1f;

	// Token: 0x040033D2 RID: 13266
	private Collider volume;

	// Token: 0x040033D3 RID: 13267
	public AudioClip enterClip;

	// Token: 0x040033D4 RID: 13268
	public AudioClip exitClip;

	// Token: 0x040033D5 RID: 13269
	public AudioClip loopClip;

	// Token: 0x040033D6 RID: 13270
	public AudioClip loopCresendoClip;

	// Token: 0x040033D7 RID: 13271
	public AudioSource audioSource;

	// Token: 0x040033D8 RID: 13272
	private Vector3 enterPos;

	// Token: 0x040033D9 RID: 13273
	private ForceVolume.AudioState audioState;

	// Token: 0x02000792 RID: 1938
	private enum AudioState
	{
		// Token: 0x040033DB RID: 13275
		None,
		// Token: 0x040033DC RID: 13276
		Enter,
		// Token: 0x040033DD RID: 13277
		Crescendo,
		// Token: 0x040033DE RID: 13278
		Loop,
		// Token: 0x040033DF RID: 13279
		Exit
	}
}
