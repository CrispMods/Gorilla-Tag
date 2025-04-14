using System;
using GorillaLocomotion;
using GT_CustomMapSupportRuntime;
using UnityEngine;

// Token: 0x02000779 RID: 1913
public class ForceVolume : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06002EED RID: 12013 RVA: 0x000E2E5C File Offset: 0x000E105C
	private void Awake()
	{
		this.volume = base.GetComponent<Collider>();
		this.audioState = ForceVolume.AudioState.None;
	}

	// Token: 0x06002EEE RID: 12014 RVA: 0x0000F862 File Offset: 0x0000DA62
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06002EEF RID: 12015 RVA: 0x0000F86B File Offset: 0x0000DA6B
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06002EF0 RID: 12016 RVA: 0x000E2E74 File Offset: 0x000E1074
	public void SliceUpdate()
	{
		if (this.audioSource && this.audioSource != null && !this.audioSource.isPlaying && this.audioSource.enabled)
		{
			this.audioSource.enabled = false;
		}
	}

	// Token: 0x06002EF1 RID: 12017 RVA: 0x000E2EC4 File Offset: 0x000E10C4
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

	// Token: 0x06002EF2 RID: 12018 RVA: 0x000E2F24 File Offset: 0x000E1124
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

	// Token: 0x06002EF3 RID: 12019 RVA: 0x000E2F94 File Offset: 0x000E1194
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

	// Token: 0x06002EF4 RID: 12020 RVA: 0x000E2FEC File Offset: 0x000E11EC
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

	// Token: 0x06002EF5 RID: 12021 RVA: 0x000E3404 File Offset: 0x000E1604
	public void OnDrawGizmosSelected()
	{
		base.GetComponents<Collider>();
		Gizmos.color = Color.magenta;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.pullTOCenterMinDistance / base.transform.lossyScale.x, 1f, this.pullTOCenterMinDistance / base.transform.lossyScale.z));
	}

	// Token: 0x06002EF6 RID: 12022 RVA: 0x000E3474 File Offset: 0x000E1674
	public void SetPropertiesFromPlaceholder(ForceVolumeProperties properties)
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
	}

	// Token: 0x06002EF8 RID: 12024 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x0400331C RID: 13084
	[SerializeField]
	public bool scaleWithSize = true;

	// Token: 0x0400331D RID: 13085
	[SerializeField]
	private float accel;

	// Token: 0x0400331E RID: 13086
	[SerializeField]
	private float maxDepth = -1f;

	// Token: 0x0400331F RID: 13087
	[SerializeField]
	private float maxSpeed;

	// Token: 0x04003320 RID: 13088
	[SerializeField]
	private bool disableGrip;

	// Token: 0x04003321 RID: 13089
	[SerializeField]
	private bool dampenLateralVelocity = true;

	// Token: 0x04003322 RID: 13090
	[SerializeField]
	private float dampenXVelPerc;

	// Token: 0x04003323 RID: 13091
	[SerializeField]
	private float dampenZVelPerc;

	// Token: 0x04003324 RID: 13092
	[SerializeField]
	private bool applyPullToCenterAcceleration = true;

	// Token: 0x04003325 RID: 13093
	[SerializeField]
	private float pullToCenterAccel;

	// Token: 0x04003326 RID: 13094
	[SerializeField]
	private float pullToCenterMaxSpeed;

	// Token: 0x04003327 RID: 13095
	[SerializeField]
	private float pullTOCenterMinDistance = 0.1f;

	// Token: 0x04003328 RID: 13096
	private Collider volume;

	// Token: 0x04003329 RID: 13097
	public AudioClip enterClip;

	// Token: 0x0400332A RID: 13098
	public AudioClip exitClip;

	// Token: 0x0400332B RID: 13099
	public AudioClip loopClip;

	// Token: 0x0400332C RID: 13100
	public AudioClip loopCresendoClip;

	// Token: 0x0400332D RID: 13101
	public AudioSource audioSource;

	// Token: 0x0400332E RID: 13102
	private Vector3 enterPos;

	// Token: 0x0400332F RID: 13103
	private ForceVolume.AudioState audioState;

	// Token: 0x0200077A RID: 1914
	private enum AudioState
	{
		// Token: 0x04003331 RID: 13105
		None,
		// Token: 0x04003332 RID: 13106
		Enter,
		// Token: 0x04003333 RID: 13107
		Crescendo,
		// Token: 0x04003334 RID: 13108
		Loop,
		// Token: 0x04003335 RID: 13109
		Exit
	}
}
