using System;
using GorillaLocomotion.Swimming;
using UnityEngine;

// Token: 0x02000218 RID: 536
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class WaterRippleEffect : MonoBehaviour
{
	// Token: 0x06000C83 RID: 3203 RVA: 0x00038C35 File Offset: 0x00036E35
	private void Awake()
	{
		this.animator = base.GetComponent<Animator>();
		this.renderer = base.GetComponent<SpriteRenderer>();
		this.ripplePlaybackSpeedHash = Animator.StringToHash(this.ripplePlaybackSpeedName);
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x00038C60 File Offset: 0x00036E60
	public void Destroy()
	{
		this.waterVolume = null;
		ObjectPools.instance.Destroy(base.gameObject);
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x0009F7CC File Offset: 0x0009D9CC
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

	// Token: 0x06000C86 RID: 3206 RVA: 0x0009F864 File Offset: 0x0009DA64
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

	// Token: 0x04000FB7 RID: 4023
	[SerializeField]
	private float ripplePlaybackSpeed = 1f;

	// Token: 0x04000FB8 RID: 4024
	[SerializeField]
	private float fadeOutDelay = 0.5f;

	// Token: 0x04000FB9 RID: 4025
	[SerializeField]
	private float fadeOutTime = 1f;

	// Token: 0x04000FBA RID: 4026
	private string ripplePlaybackSpeedName = "RipplePlaybackSpeed";

	// Token: 0x04000FBB RID: 4027
	private int ripplePlaybackSpeedHash;

	// Token: 0x04000FBC RID: 4028
	private float rippleStartTime = -1f;

	// Token: 0x04000FBD RID: 4029
	private Animator animator;

	// Token: 0x04000FBE RID: 4030
	private SpriteRenderer renderer;

	// Token: 0x04000FBF RID: 4031
	private WaterVolume waterVolume;
}
