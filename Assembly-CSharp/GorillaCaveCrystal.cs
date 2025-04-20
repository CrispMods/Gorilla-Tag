using System;
using UnityEngine;

// Token: 0x0200055D RID: 1373
public class GorillaCaveCrystal : Tappable
{
	// Token: 0x060021A1 RID: 8609 RVA: 0x00046E9A File Offset: 0x0004509A
	private void Awake()
	{
		if (this.tapScript == null)
		{
			this.tapScript = base.GetComponent<TapInnerGlow>();
		}
	}

	// Token: 0x060021A2 RID: 8610 RVA: 0x00046EB6 File Offset: 0x000450B6
	public override void OnTapLocal(float tapStrength, float tapTime, PhotonMessageInfoWrapped info)
	{
		this._tapStrength = tapStrength;
		this.AnimateCrystal();
	}

	// Token: 0x060021A3 RID: 8611 RVA: 0x00046EC5 File Offset: 0x000450C5
	private void AnimateCrystal()
	{
		if (this.tapScript)
		{
			this.tapScript.Tap();
		}
	}

	// Token: 0x04002540 RID: 9536
	public bool overrideSoundAndMaterial;

	// Token: 0x04002541 RID: 9537
	public CrystalOctave octave;

	// Token: 0x04002542 RID: 9538
	public CrystalNote note;

	// Token: 0x04002543 RID: 9539
	[SerializeField]
	private MeshRenderer _crystalRenderer;

	// Token: 0x04002544 RID: 9540
	public TapInnerGlow tapScript;

	// Token: 0x04002545 RID: 9541
	[HideInInspector]
	public GorillaCaveCrystalVisuals visuals;

	// Token: 0x04002546 RID: 9542
	[HideInInspector]
	[SerializeField]
	private AnimationCurve _lerpInCurve = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x04002547 RID: 9543
	[HideInInspector]
	[SerializeField]
	private AnimationCurve _lerpOutCurve = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x04002548 RID: 9544
	[HideInInspector]
	[SerializeField]
	private bool _animating;

	// Token: 0x04002549 RID: 9545
	[HideInInspector]
	[SerializeField]
	[Range(0f, 1f)]
	private float _tapStrength = 1f;

	// Token: 0x0400254A RID: 9546
	[NonSerialized]
	private TimeSince _timeSinceLastTap;
}
