using System;
using UnityEngine;

// Token: 0x020008EB RID: 2283
[Serializable]
public class VoiceLoudnessReactorParticleSystemTarget
{
	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x060036D5 RID: 14037 RVA: 0x00053623 File Offset: 0x00051823
	// (set) Token: 0x060036D6 RID: 14038 RVA: 0x0005362B File Offset: 0x0005182B
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

	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x060036D7 RID: 14039 RVA: 0x00053634 File Offset: 0x00051834
	// (set) Token: 0x060036D8 RID: 14040 RVA: 0x0005363C File Offset: 0x0005183C
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

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x060036D9 RID: 14041 RVA: 0x00053645 File Offset: 0x00051845
	// (set) Token: 0x060036DA RID: 14042 RVA: 0x0005364D File Offset: 0x0005184D
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

	// Token: 0x040039D2 RID: 14802
	public ParticleSystem particleSystem;

	// Token: 0x040039D3 RID: 14803
	public bool UseSmoothedLoudness;

	// Token: 0x040039D4 RID: 14804
	public float Scale = 1f;

	// Token: 0x040039D5 RID: 14805
	private float initialSpeed;

	// Token: 0x040039D6 RID: 14806
	private float initialRate;

	// Token: 0x040039D7 RID: 14807
	private float initialSize;

	// Token: 0x040039D8 RID: 14808
	public AnimationCurve speed;

	// Token: 0x040039D9 RID: 14809
	public AnimationCurve rate;

	// Token: 0x040039DA RID: 14810
	public AnimationCurve size;

	// Token: 0x040039DB RID: 14811
	[HideInInspector]
	public ParticleSystem.MainModule Main;

	// Token: 0x040039DC RID: 14812
	[HideInInspector]
	public ParticleSystem.EmissionModule Emission;
}
