using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class PeriodicNoiseGenerator : MonoBehaviour
{
	// Token: 0x060002F0 RID: 752 RVA: 0x00031533 File Offset: 0x0002F733
	private void Awake()
	{
		this.noiseActor = base.GetComponentInParent<CrittersLoudNoise>();
		this.lastTime = Time.time;
		this.mR = base.GetComponentInChildren<MeshRenderer>();
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x000744C8 File Offset: 0x000726C8
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

	// Token: 0x04000387 RID: 903
	public float sleepDuration;

	// Token: 0x04000388 RID: 904
	public float randomDuration;

	// Token: 0x04000389 RID: 905
	public float lastTime;

	// Token: 0x0400038A RID: 906
	private CrittersLoudNoise noiseActor;

	// Token: 0x0400038B RID: 907
	public Material transparent;

	// Token: 0x0400038C RID: 908
	public Material solid;

	// Token: 0x0400038D RID: 909
	private MeshRenderer mR;
}
