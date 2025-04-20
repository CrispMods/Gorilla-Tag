using System;
using UnityEngine;

// Token: 0x020008D4 RID: 2260
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class LightningStrike : MonoBehaviour
{
	// Token: 0x060036C2 RID: 14018 RVA: 0x00145388 File Offset: 0x00143588
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

	// Token: 0x060036C3 RID: 14019 RVA: 0x00145404 File Offset: 0x00143604
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

	// Token: 0x040038E2 RID: 14562
	private static SRand rand = new SRand("LightningStrike");

	// Token: 0x040038E3 RID: 14563
	private ParticleSystem ps;

	// Token: 0x040038E4 RID: 14564
	private ParticleSystem.MainModule psMain;

	// Token: 0x040038E5 RID: 14565
	private ParticleSystem.ShapeModule psShape;

	// Token: 0x040038E6 RID: 14566
	private ParticleSystem.TrailModule psTrails;

	// Token: 0x040038E7 RID: 14567
	private AudioSource audioSource;
}
