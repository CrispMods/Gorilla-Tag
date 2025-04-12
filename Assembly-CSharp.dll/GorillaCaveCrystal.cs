using System;
using UnityEngine;

// Token: 0x02000550 RID: 1360
public class GorillaCaveCrystal : Tappable
{
	// Token: 0x0600214B RID: 8523 RVA: 0x00045AF5 File Offset: 0x00043CF5
	private void Awake()
	{
		if (this.tapScript == null)
		{
			this.tapScript = base.GetComponent<TapInnerGlow>();
		}
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x00045B11 File Offset: 0x00043D11
	public override void OnTapLocal(float tapStrength, float tapTime, PhotonMessageInfoWrapped info)
	{
		this._tapStrength = tapStrength;
		this.AnimateCrystal();
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x00045B20 File Offset: 0x00043D20
	private void AnimateCrystal()
	{
		if (this.tapScript)
		{
			this.tapScript.Tap();
		}
	}

	// Token: 0x040024EE RID: 9454
	public bool overrideSoundAndMaterial;

	// Token: 0x040024EF RID: 9455
	public CrystalOctave octave;

	// Token: 0x040024F0 RID: 9456
	public CrystalNote note;

	// Token: 0x040024F1 RID: 9457
	[SerializeField]
	private MeshRenderer _crystalRenderer;

	// Token: 0x040024F2 RID: 9458
	public TapInnerGlow tapScript;

	// Token: 0x040024F3 RID: 9459
	[HideInInspector]
	public GorillaCaveCrystalVisuals visuals;

	// Token: 0x040024F4 RID: 9460
	[HideInInspector]
	[SerializeField]
	private AnimationCurve _lerpInCurve = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x040024F5 RID: 9461
	[HideInInspector]
	[SerializeField]
	private AnimationCurve _lerpOutCurve = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x040024F6 RID: 9462
	[HideInInspector]
	[SerializeField]
	private bool _animating;

	// Token: 0x040024F7 RID: 9463
	[HideInInspector]
	[SerializeField]
	[Range(0f, 1f)]
	private float _tapStrength = 1f;

	// Token: 0x040024F8 RID: 9464
	[NonSerialized]
	private TimeSince _timeSinceLastTap;
}
