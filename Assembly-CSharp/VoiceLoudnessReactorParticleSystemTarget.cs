using System;
using UnityEngine;

// Token: 0x02000904 RID: 2308
[Serializable]
public class VoiceLoudnessReactorParticleSystemTarget
{
	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x06003791 RID: 14225 RVA: 0x00054B40 File Offset: 0x00052D40
	// (set) Token: 0x06003792 RID: 14226 RVA: 0x00054B48 File Offset: 0x00052D48
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

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x06003793 RID: 14227 RVA: 0x00054B51 File Offset: 0x00052D51
	// (set) Token: 0x06003794 RID: 14228 RVA: 0x00054B59 File Offset: 0x00052D59
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

	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x06003795 RID: 14229 RVA: 0x00054B62 File Offset: 0x00052D62
	// (set) Token: 0x06003796 RID: 14230 RVA: 0x00054B6A File Offset: 0x00052D6A
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

	// Token: 0x04003A81 RID: 14977
	public ParticleSystem particleSystem;

	// Token: 0x04003A82 RID: 14978
	public bool UseSmoothedLoudness;

	// Token: 0x04003A83 RID: 14979
	public float Scale = 1f;

	// Token: 0x04003A84 RID: 14980
	private float initialSpeed;

	// Token: 0x04003A85 RID: 14981
	private float initialRate;

	// Token: 0x04003A86 RID: 14982
	private float initialSize;

	// Token: 0x04003A87 RID: 14983
	public AnimationCurve speed;

	// Token: 0x04003A88 RID: 14984
	public AnimationCurve rate;

	// Token: 0x04003A89 RID: 14985
	public AnimationCurve size;

	// Token: 0x04003A8A RID: 14986
	[HideInInspector]
	public ParticleSystem.MainModule Main;

	// Token: 0x04003A8B RID: 14987
	[HideInInspector]
	public ParticleSystem.EmissionModule Emission;
}
