using System;
using UnityEngine;

// Token: 0x02000174 RID: 372
public class PartyInABox : MonoBehaviour
{
	// Token: 0x0600094B RID: 2379 RVA: 0x00031C4A File Offset: 0x0002FE4A
	private void Awake()
	{
		this.Reset();
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x00031C4A File Offset: 0x0002FE4A
	private void OnEnable()
	{
		this.Reset();
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x00031C52 File Offset: 0x0002FE52
	public void Cranked_ReleaseParty()
	{
		if (!this.parentHoldable.IsLocalObject())
		{
			return;
		}
		this.ReleaseParty();
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x00031C68 File Offset: 0x0002FE68
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

	// Token: 0x0600094F RID: 2383 RVA: 0x00031D04 File Offset: 0x0002FF04
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

	// Token: 0x06000950 RID: 2384 RVA: 0x00031D5C File Offset: 0x0002FF5C
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

	// Token: 0x04000B40 RID: 2880
	[SerializeField]
	private TransferrableObject parentHoldable;

	// Token: 0x04000B41 RID: 2881
	[SerializeField]
	private ParticleSystem particles;

	// Token: 0x04000B42 RID: 2882
	[SerializeField]
	private Animation anim;

	// Token: 0x04000B43 RID: 2883
	[SerializeField]
	private SpringyWobbler spring;

	// Token: 0x04000B44 RID: 2884
	[SerializeField]
	private AudioSource partyAudio;

	// Token: 0x04000B45 RID: 2885
	[SerializeField]
	private float partyHapticStrength;

	// Token: 0x04000B46 RID: 2886
	[SerializeField]
	private float partyHapticDuration;

	// Token: 0x04000B47 RID: 2887
	private bool isReleased;

	// Token: 0x04000B48 RID: 2888
	[SerializeField]
	private PartyInABox.ForceTransform[] forceTransforms;

	// Token: 0x02000175 RID: 373
	[Serializable]
	private struct ForceTransform
	{
		// Token: 0x06000952 RID: 2386 RVA: 0x00031DBF File Offset: 0x0002FFBF
		public void Apply()
		{
			this.transform.localPosition = this.localPosition;
			this.transform.localRotation = this.localRotation;
		}

		// Token: 0x04000B49 RID: 2889
		public Transform transform;

		// Token: 0x04000B4A RID: 2890
		public Vector3 localPosition;

		// Token: 0x04000B4B RID: 2891
		public Quaternion localRotation;
	}
}
