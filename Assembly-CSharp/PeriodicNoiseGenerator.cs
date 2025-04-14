using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class PeriodicNoiseGenerator : MonoBehaviour
{
	// Token: 0x060002EE RID: 750 RVA: 0x000123C5 File Offset: 0x000105C5
	private void Awake()
	{
		this.noiseActor = base.GetComponentInParent<CrittersLoudNoise>();
		this.lastTime = Time.time;
		this.mR = base.GetComponentInChildren<MeshRenderer>();
	}

	// Token: 0x060002EF RID: 751 RVA: 0x000123EC File Offset: 0x000105EC
	private void Update()
	{
		if (!CrittersManager.instance.LocalAuthority())
		{
			return;
		}
		if (Time.time > this.lastTime + this.sleepDuration)
		{
			this.lastTime = Time.time + this.randomDuration * Random.value;
			this.noiseActor.SetTimeEnabled();
			this.noiseActor.soundEnabled = true;
			this.mR.sharedMaterial = this.solid;
		}
		if (!this.noiseActor.soundEnabled && this.mR.sharedMaterial != this.transparent)
		{
			this.mR.sharedMaterial = this.transparent;
		}
	}

	// Token: 0x04000386 RID: 902
	public float sleepDuration;

	// Token: 0x04000387 RID: 903
	public float randomDuration;

	// Token: 0x04000388 RID: 904
	public float lastTime;

	// Token: 0x04000389 RID: 905
	private CrittersLoudNoise noiseActor;

	// Token: 0x0400038A RID: 906
	public Material transparent;

	// Token: 0x0400038B RID: 907
	public Material solid;

	// Token: 0x0400038C RID: 908
	private MeshRenderer mR;
}
