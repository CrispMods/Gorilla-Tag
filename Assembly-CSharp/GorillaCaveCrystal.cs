using System;
using UnityEngine;

// Token: 0x0200054F RID: 1359
public class GorillaCaveCrystal : Tappable
{
	// Token: 0x06002143 RID: 8515 RVA: 0x000A5AB6 File Offset: 0x000A3CB6
	private void Awake()
	{
		if (this.tapScript == null)
		{
			this.tapScript = base.GetComponent<TapInnerGlow>();
		}
	}

	// Token: 0x06002144 RID: 8516 RVA: 0x000A5AD2 File Offset: 0x000A3CD2
	public override void OnTapLocal(float tapStrength, float tapTime, PhotonMessageInfoWrapped info)
	{
		this._tapStrength = tapStrength;
		this.AnimateCrystal();
	}

	// Token: 0x06002145 RID: 8517 RVA: 0x000A5AE1 File Offset: 0x000A3CE1
	private void AnimateCrystal()
	{
		if (this.tapScript)
		{
			this.tapScript.Tap();
		}
	}

	// Token: 0x040024E8 RID: 9448
	public bool overrideSoundAndMaterial;

	// Token: 0x040024E9 RID: 9449
	public CrystalOctave octave;

	// Token: 0x040024EA RID: 9450
	public CrystalNote note;

	// Token: 0x040024EB RID: 9451
	[SerializeField]
	private MeshRenderer _crystalRenderer;

	// Token: 0x040024EC RID: 9452
	public TapInnerGlow tapScript;

	// Token: 0x040024ED RID: 9453
	[HideInInspector]
	public GorillaCaveCrystalVisuals visuals;

	// Token: 0x040024EE RID: 9454
	[HideInInspector]
	[SerializeField]
	private AnimationCurve _lerpInCurve = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x040024EF RID: 9455
	[HideInInspector]
	[SerializeField]
	private AnimationCurve _lerpOutCurve = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x040024F0 RID: 9456
	[HideInInspector]
	[SerializeField]
	private bool _animating;

	// Token: 0x040024F1 RID: 9457
	[HideInInspector]
	[SerializeField]
	[Range(0f, 1f)]
	private float _tapStrength = 1f;

	// Token: 0x040024F2 RID: 9458
	[NonSerialized]
	private TimeSince _timeSinceLastTap;
}
