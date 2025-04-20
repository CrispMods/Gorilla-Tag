using System;
using UnityEngine;

// Token: 0x02000561 RID: 1377
public class GorillaColorizableParticle : GorillaColorizableBase
{
	// Token: 0x060021B3 RID: 8627 RVA: 0x000F650C File Offset: 0x000F470C
	public override void SetColor(Color color)
	{
		ParticleSystem.MainModule main = this.particleSystem.main;
		Color color2 = new Color(Mathf.Pow(color.r, this.gradientColorPower), Mathf.Pow(color.g, this.gradientColorPower), Mathf.Pow(color.b, this.gradientColorPower), color.a);
		main.startColor = new ParticleSystem.MinMaxGradient(this.useLinearColor ? color.linear : color, this.useLinearColor ? color2.linear : color2);
	}

	// Token: 0x04002559 RID: 9561
	public ParticleSystem particleSystem;

	// Token: 0x0400255A RID: 9562
	public float gradientColorPower = 2f;

	// Token: 0x0400255B RID: 9563
	public bool useLinearColor = true;
}
