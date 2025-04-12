using System;
using GorillaLocomotion.Swimming;
using UnityEngine;

// Token: 0x0200020D RID: 525
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class WaterRippleEffect : MonoBehaviour
{
	// Token: 0x06000C3A RID: 3130 RVA: 0x00037975 File Offset: 0x00035B75
	private void Awake()
	{
		this.animator = base.GetComponent<Animator>();
		this.renderer = base.GetComponent<SpriteRenderer>();
		this.ripplePlaybackSpeedHash = Animator.StringToHash(this.ripplePlaybackSpeedName);
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x000379A0 File Offset: 0x00035BA0
	public void Destroy()
	{
		this.waterVolume = null;
		ObjectPools.instance.Destroy(base.gameObject);
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x0009CF40 File Offset: 0x0009B140
	public void PlayEffect(WaterVolume volume = null)
	{
		this.waterVolume = volume;
		this.rippleStartTime = Time.time;
		this.animator.SetFloat(this.ripplePlaybackSpeedHash, this.ripplePlaybackSpeed);
		if (this.waterVolume != null && this.waterVolume.Parameters != null)
		{
			this.renderer.color = this.waterVolume.Parameters.rippleSpriteColor;
		}
		Color color = this.renderer.color;
		color.a = 1f;
		this.renderer.color = color;
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x0009CFD8 File Offset: 0x0009B1D8
	private void Update()
	{
		if (this.waterVolume != null && !this.waterVolume.isStationary && this.waterVolume.surfacePlane != null)
		{
			Vector3 b = Vector3.Dot(base.transform.position - this.waterVolume.surfacePlane.position, this.waterVolume.surfacePlane.up) * this.waterVolume.surfacePlane.up;
			base.transform.position = base.transform.position - b;
		}
		float num = Mathf.Clamp01((Time.time - this.rippleStartTime - this.fadeOutDelay) / this.fadeOutTime);
		Color color = this.renderer.color;
		color.a = 1f - num;
		this.renderer.color = color;
		if (num >= 1f - Mathf.Epsilon)
		{
			this.Destroy();
			return;
		}
	}

	// Token: 0x04000F72 RID: 3954
	[SerializeField]
	private float ripplePlaybackSpeed = 1f;

	// Token: 0x04000F73 RID: 3955
	[SerializeField]
	private float fadeOutDelay = 0.5f;

	// Token: 0x04000F74 RID: 3956
	[SerializeField]
	private float fadeOutTime = 1f;

	// Token: 0x04000F75 RID: 3957
	private string ripplePlaybackSpeedName = "RipplePlaybackSpeed";

	// Token: 0x04000F76 RID: 3958
	private int ripplePlaybackSpeedHash;

	// Token: 0x04000F77 RID: 3959
	private float rippleStartTime = -1f;

	// Token: 0x04000F78 RID: 3960
	private Animator animator;

	// Token: 0x04000F79 RID: 3961
	private SpriteRenderer renderer;

	// Token: 0x04000F7A RID: 3962
	private WaterVolume waterVolume;
}
