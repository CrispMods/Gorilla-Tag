using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000155 RID: 341
public class EqualizerAnim : MonoBehaviour
{
	// Token: 0x060008AF RID: 2223 RVA: 0x0002F705 File Offset: 0x0002D905
	private void Start()
	{
		this.inputColorHash = this.inputColorProperty;
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x0002F718 File Offset: 0x0002D918
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

	// Token: 0x04000A64 RID: 2660
	[SerializeField]
	private AnimationCurve redCurve;

	// Token: 0x04000A65 RID: 2661
	[SerializeField]
	private AnimationCurve greenCurve;

	// Token: 0x04000A66 RID: 2662
	[SerializeField]
	private AnimationCurve blueCurve;

	// Token: 0x04000A67 RID: 2663
	[SerializeField]
	private float loopDuration;

	// Token: 0x04000A68 RID: 2664
	[SerializeField]
	private Material material;

	// Token: 0x04000A69 RID: 2665
	[SerializeField]
	private string inputColorProperty;

	// Token: 0x04000A6A RID: 2666
	private ShaderHashId inputColorHash;

	// Token: 0x04000A6B RID: 2667
	private static int thisFrame;

	// Token: 0x04000A6C RID: 2668
	private static HashSet<Material> materialsUpdatedThisFrame = new HashSet<Material>();
}
