using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class EqualizerAnim : MonoBehaviour
{
	// Token: 0x060008F3 RID: 2291 RVA: 0x000365DF File Offset: 0x000347DF
	private void Start()
	{
		this.inputColorHash = this.inputColorProperty;
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x00090340 File Offset: 0x0008E540
	private void Update()
	{
		if (EqualizerAnim.thisFrame == Time.frameCount)
		{
			if (EqualizerAnim.materialsUpdatedThisFrame.Contains(this.material))
			{
				return;
			}
		}
		else
		{
			EqualizerAnim.thisFrame = Time.frameCount;
			EqualizerAnim.materialsUpdatedThisFrame.Clear();
		}
		float time = Time.time % this.loopDuration;
		this.material.SetColor(this.inputColorHash, new Color(this.redCurve.Evaluate(time), this.greenCurve.Evaluate(time), this.blueCurve.Evaluate(time)));
		EqualizerAnim.materialsUpdatedThisFrame.Add(this.material);
	}

	// Token: 0x04000AA7 RID: 2727
	[SerializeField]
	private AnimationCurve redCurve;

	// Token: 0x04000AA8 RID: 2728
	[SerializeField]
	private AnimationCurve greenCurve;

	// Token: 0x04000AA9 RID: 2729
	[SerializeField]
	private AnimationCurve blueCurve;

	// Token: 0x04000AAA RID: 2730
	[SerializeField]
	private float loopDuration;

	// Token: 0x04000AAB RID: 2731
	[SerializeField]
	private Material material;

	// Token: 0x04000AAC RID: 2732
	[SerializeField]
	private string inputColorProperty;

	// Token: 0x04000AAD RID: 2733
	private ShaderHashId inputColorHash;

	// Token: 0x04000AAE RID: 2734
	private static int thisFrame;

	// Token: 0x04000AAF RID: 2735
	private static HashSet<Material> materialsUpdatedThisFrame = new HashSet<Material>();
}
