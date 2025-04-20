using System;
using UnityEngine;

// Token: 0x0200017F RID: 383
public class PartyInABox : MonoBehaviour
{
	// Token: 0x06000998 RID: 2456 RVA: 0x00036C99 File Offset: 0x00034E99
	private void Awake()
	{
		this.Reset();
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x00036C99 File Offset: 0x00034E99
	private void OnEnable()
	{
		this.Reset();
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x00036CA1 File Offset: 0x00034EA1
	public void Cranked_ReleaseParty()
	{
		if (!this.parentHoldable.IsLocalObject())
		{
			return;
		}
		this.ReleaseParty();
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x00092218 File Offset: 0x00090418
	public void ReleaseParty()
	{
		if (this.isReleased)
		{
			return;
		}
		if (this.parentHoldable.IsLocalObject())
		{
			this.parentHoldable.itemState |= TransferrableObject.ItemStates.State0;
			GorillaTagger.Instance.StartVibration(true, this.partyHapticStrength, this.partyHapticDuration);
			GorillaTagger.Instance.StartVibration(false, this.partyHapticStrength, this.partyHapticDuration);
		}
		this.isReleased = true;
		this.spring.enabled = true;
		this.anim.Play();
		this.particles.Play();
		this.partyAudio.Play();
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x000922B4 File Offset: 0x000904B4
	private void Update()
	{
		if (this.parentHoldable.IsLocalObject())
		{
			return;
		}
		if (this.parentHoldable.itemState.HasFlag(TransferrableObject.ItemStates.State0))
		{
			if (!this.isReleased)
			{
				this.ReleaseParty();
				return;
			}
		}
		else if (this.isReleased)
		{
			this.Reset();
		}
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x0009230C File Offset: 0x0009050C
	public void Reset()
	{
		this.isReleased = false;
		this.parentHoldable.itemState &= (TransferrableObject.ItemStates)(-2);
		this.spring.enabled = false;
		this.anim.Stop();
		foreach (PartyInABox.ForceTransform forceTransform in this.forceTransforms)
		{
			forceTransform.Apply();
		}
	}

	// Token: 0x04000B87 RID: 2951
	[SerializeField]
	private TransferrableObject parentHoldable;

	// Token: 0x04000B88 RID: 2952
	[SerializeField]
	private ParticleSystem particles;

	// Token: 0x04000B89 RID: 2953
	[SerializeField]
	private Animation anim;

	// Token: 0x04000B8A RID: 2954
	[SerializeField]
	private SpringyWobbler spring;

	// Token: 0x04000B8B RID: 2955
	[SerializeField]
	private AudioSource partyAudio;

	// Token: 0x04000B8C RID: 2956
	[SerializeField]
	private float partyHapticStrength;

	// Token: 0x04000B8D RID: 2957
	[SerializeField]
	private float partyHapticDuration;

	// Token: 0x04000B8E RID: 2958
	private bool isReleased;

	// Token: 0x04000B8F RID: 2959
	[SerializeField]
	private PartyInABox.ForceTransform[] forceTransforms;

	// Token: 0x02000180 RID: 384
	[Serializable]
	private struct ForceTransform
	{
		// Token: 0x0600099F RID: 2463 RVA: 0x00036CB7 File Offset: 0x00034EB7
		public void Apply()
		{
			this.transform.localPosition = this.localPosition;
			this.transform.localRotation = this.localRotation;
		}

		// Token: 0x04000B90 RID: 2960
		public Transform transform;

		// Token: 0x04000B91 RID: 2961
		public Vector3 localPosition;

		// Token: 0x04000B92 RID: 2962
		public Quaternion localRotation;
	}
}
