using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000456 RID: 1110
public class MonkeCandle : RubberDuck
{
	// Token: 0x06001B59 RID: 7001 RVA: 0x000D9CF8 File Offset: 0x000D7EF8
	protected override void Start()
	{
		base.Start();
		if (!this.IsMyItem())
		{
			this.movingFxAudio.volume = this.movingFxAudio.volume * 0.5f;
			this.fxExplodeAudio.volume = this.fxExplodeAudio.volume * 0.5f;
		}
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x000D9D4C File Offset: 0x000D7F4C
	public override void TriggeredLateUpdate()
	{
		base.TriggeredLateUpdate();
		if (!this.particleFX.isPlaying)
		{
			return;
		}
		int particles = this.particleFX.GetParticles(this.fxParticleArray);
		if (particles <= 0)
		{
			this.movingFxAudio.GTStop();
			if (this.currentParticles.Count == 0)
			{
				return;
			}
		}
		for (int i = 0; i < particles; i++)
		{
			if (this.currentParticles.Contains(this.fxParticleArray[i].randomSeed))
			{
				this.currentParticles.Remove(this.fxParticleArray[i].randomSeed);
			}
		}
		foreach (uint key in this.currentParticles)
		{
			if (this.particleInfoDict.TryGetValue(key, out this.outPosition))
			{
				this.fxExplodeAudio.transform.position = this.outPosition;
				this.fxExplodeAudio.GTPlayOneShot(this.fxExplodeAudio.clip, 1f);
				this.particleInfoDict.Remove(key);
			}
		}
		this.currentParticles.Clear();
		for (int j = 0; j < particles; j++)
		{
			if (j == 0)
			{
				this.movingFxAudio.transform.position = this.fxParticleArray[j].position;
			}
			if (this.particleInfoDict.TryGetValue(this.fxParticleArray[j].randomSeed, out this.outPosition))
			{
				this.particleInfoDict[this.fxParticleArray[j].randomSeed] = this.fxParticleArray[j].position;
			}
			else
			{
				this.particleInfoDict.Add(this.fxParticleArray[j].randomSeed, this.fxParticleArray[j].position);
				if (j == 0 && !this.movingFxAudio.isPlaying)
				{
					this.movingFxAudio.GTPlay();
				}
			}
			this.currentParticles.Add(this.fxParticleArray[j].randomSeed);
		}
	}

	// Token: 0x04001E3F RID: 7743
	private ParticleSystem.Particle[] fxParticleArray = new ParticleSystem.Particle[20];

	// Token: 0x04001E40 RID: 7744
	public AudioSource movingFxAudio;

	// Token: 0x04001E41 RID: 7745
	public AudioSource fxExplodeAudio;

	// Token: 0x04001E42 RID: 7746
	private List<uint> currentParticles = new List<uint>();

	// Token: 0x04001E43 RID: 7747
	private Dictionary<uint, Vector3> particleInfoDict = new Dictionary<uint, Vector3>();

	// Token: 0x04001E44 RID: 7748
	private Vector3 outPosition;
}
