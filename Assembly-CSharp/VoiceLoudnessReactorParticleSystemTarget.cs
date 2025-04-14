using System;
using UnityEngine;

// Token: 0x020008E8 RID: 2280
[Serializable]
public class VoiceLoudnessReactorParticleSystemTarget
{
	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x060036C9 RID: 14025 RVA: 0x00103C57 File Offset: 0x00101E57
	// (set) Token: 0x060036CA RID: 14026 RVA: 0x00103C5F File Offset: 0x00101E5F
	public float InitialSpeed
	{
		get
		{
			return this.initialSpeed;
		}
		set
		{
			this.initialSpeed = value;
		}
	}

	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x060036CB RID: 14027 RVA: 0x00103C68 File Offset: 0x00101E68
	// (set) Token: 0x060036CC RID: 14028 RVA: 0x00103C70 File Offset: 0x00101E70
	public float InitialRate
	{
		get
		{
			return this.initialRate;
		}
		set
		{
			this.initialRate = value;
		}
	}

	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x060036CD RID: 14029 RVA: 0x00103C79 File Offset: 0x00101E79
	// (set) Token: 0x060036CE RID: 14030 RVA: 0x00103C81 File Offset: 0x00101E81
	public float InitialSize
	{
		get
		{
			return this.initialSize;
		}
		set
		{
			this.initialSize = value;
		}
	}

	// Token: 0x040039C0 RID: 14784
	public ParticleSystem particleSystem;

	// Token: 0x040039C1 RID: 14785
	public bool UseSmoothedLoudness;

	// Token: 0x040039C2 RID: 14786
	public float Scale = 1f;

	// Token: 0x040039C3 RID: 14787
	private float initialSpeed;

	// Token: 0x040039C4 RID: 14788
	private float initialRate;

	// Token: 0x040039C5 RID: 14789
	private float initialSize;

	// Token: 0x040039C6 RID: 14790
	public AnimationCurve speed;

	// Token: 0x040039C7 RID: 14791
	public AnimationCurve rate;

	// Token: 0x040039C8 RID: 14792
	public AnimationCurve size;

	// Token: 0x040039C9 RID: 14793
	[HideInInspector]
	public ParticleSystem.MainModule Main;

	// Token: 0x040039CA RID: 14794
	[HideInInspector]
	public ParticleSystem.EmissionModule Emission;
}
