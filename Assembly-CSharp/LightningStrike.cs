using System;
using UnityEngine;

// Token: 0x020008B8 RID: 2232
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class LightningStrike : MonoBehaviour
{
	// Token: 0x060035FA RID: 13818 RVA: 0x000FF868 File Offset: 0x000FDA68
	private void Initialize()
	{
		this.ps = base.GetComponent<ParticleSystem>();
		this.psMain = this.ps.main;
		this.psMain.playOnAwake = true;
		this.psMain.stopAction = ParticleSystemStopAction.Disable;
		this.psShape = this.ps.shape;
		this.psTrails = this.ps.trails;
		this.audioSource = base.GetComponent<AudioSource>();
		this.audioSource.playOnAwake = true;
	}

	// Token: 0x060035FB RID: 13819 RVA: 0x000FF8E4 File Offset: 0x000FDAE4
	public void Play(Vector3 p1, Vector3 p2, float beamWidthMultiplier, float audioVolume)
	{
		if (this.ps == null)
		{
			this.Initialize();
		}
		base.transform.position = p1;
		base.transform.rotation = Quaternion.LookRotation(p1 - p2);
		this.psShape.radius = Vector3.Distance(p1, p2) * 0.5f;
		this.psShape.position = new Vector3(0f, 0f, -this.psShape.radius);
		this.psShape.randomPositionAmount = Mathf.Clamp(this.psShape.radius / 50f, 0f, 1f);
		this.psTrails.widthOverTrail = new ParticleSystem.MinMaxCurve(beamWidthMultiplier * 0.1f, beamWidthMultiplier);
		this.psMain.duration = LightningStrike.rand.NextFloat(0.05f, 0.12f);
		this.audioSource.volume = Mathf.Clamp(this.psShape.radius / 5f, 0f, 1f) * audioVolume;
		base.gameObject.SetActive(true);
	}

	// Token: 0x04003821 RID: 14369
	private static SRand rand = new SRand("LightningStrike");

	// Token: 0x04003822 RID: 14370
	private ParticleSystem ps;

	// Token: 0x04003823 RID: 14371
	private ParticleSystem.MainModule psMain;

	// Token: 0x04003824 RID: 14372
	private ParticleSystem.ShapeModule psShape;

	// Token: 0x04003825 RID: 14373
	private ParticleSystem.TrailModule psTrails;

	// Token: 0x04003826 RID: 14374
	private AudioSource audioSource;
}
