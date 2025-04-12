using System;
using UnityEngine;

// Token: 0x020008BB RID: 2235
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class LightningStrike : MonoBehaviour
{
	// Token: 0x06003606 RID: 13830 RVA: 0x0013FDC8 File Offset: 0x0013DFC8
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

	// Token: 0x06003607 RID: 13831 RVA: 0x0013FE44 File Offset: 0x0013E044
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

	// Token: 0x04003833 RID: 14387
	private static SRand rand = new SRand("LightningStrike");

	// Token: 0x04003834 RID: 14388
	private ParticleSystem ps;

	// Token: 0x04003835 RID: 14389
	private ParticleSystem.MainModule psMain;

	// Token: 0x04003836 RID: 14390
	private ParticleSystem.ShapeModule psShape;

	// Token: 0x04003837 RID: 14391
	private ParticleSystem.TrailModule psTrails;

	// Token: 0x04003838 RID: 14392
	private AudioSource audioSource;
}
