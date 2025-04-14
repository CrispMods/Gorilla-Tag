using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200044A RID: 1098
public class MonkeCandle : RubberDuck
{
	// Token: 0x06001B05 RID: 6917 RVA: 0x00085170 File Offset: 0x00083370
	protected override void Start()
	{
		base.Start();
		if (!this.IsMyItem())
		{
			this.movingFxAudio.volume = this.movingFxAudio.volume * 0.5f;
			this.fxExplodeAudio.volume = this.fxExplodeAudio.volume * 0.5f;
		}
	}

	// Token: 0x06001B06 RID: 6918 RVA: 0x000851C4 File Offset: 0x000833C4
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

	// Token: 0x04001DF0 RID: 7664
	private ParticleSystem.Particle[] fxParticleArray = new ParticleSystem.Particle[20];

	// Token: 0x04001DF1 RID: 7665
	public AudioSource movingFxAudio;

	// Token: 0x04001DF2 RID: 7666
	public AudioSource fxExplodeAudio;

	// Token: 0x04001DF3 RID: 7667
	private List<uint> currentParticles = new List<uint>();

	// Token: 0x04001DF4 RID: 7668
	private Dictionary<uint, Vector3> particleInfoDict = new Dictionary<uint, Vector3>();

	// Token: 0x04001DF5 RID: 7669
	private Vector3 outPosition;
}
