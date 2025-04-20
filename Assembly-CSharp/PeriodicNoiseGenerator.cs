using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class PeriodicNoiseGenerator : MonoBehaviour
{
	// Token: 0x0600031D RID: 797 RVA: 0x00032666 File Offset: 0x00030866
	private void Awake()
	{
		this.noiseActor = base.GetComponentInParent<CrittersLoudNoise>();
		this.lastTime = Time.time;
		this.mR = base.GetComponentInChildren<MeshRenderer>();
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00076BF0 File Offset: 0x00074DF0
	private void Update()
	{
		if (!CrittersManager.instance.LocalAuthority())
		{
			return;
		}
		if (Time.time > this.lastTime + this.sleepDuration)
		{
			this.lastTime = Time.time + this.randomDuration * UnityEngine.Random.value;
			this.noiseActor.SetTimeEnabled();
			this.noiseActor.soundEnabled = true;
			this.mR.sharedMaterial = this.solid;
		}
		if (!this.noiseActor.soundEnabled && this.mR.sharedMaterial != this.transparent)
		{
			this.mR.sharedMaterial = this.transparent;
		}
	}

	// Token: 0x040003B8 RID: 952
	public float sleepDuration;

	// Token: 0x040003B9 RID: 953
	public float randomDuration;

	// Token: 0x040003BA RID: 954
	public float lastTime;

	// Token: 0x040003BB RID: 955
	private CrittersLoudNoise noiseActor;

	// Token: 0x040003BC RID: 956
	public Material transparent;

	// Token: 0x040003BD RID: 957
	public Material solid;

	// Token: 0x040003BE RID: 958
	private MeshRenderer mR;
}
