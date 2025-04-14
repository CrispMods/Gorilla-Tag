using System;
using UnityEngine;

// Token: 0x02000553 RID: 1363
public class GorillaColorizableParticle : GorillaColorizableBase
{
	// Token: 0x06002155 RID: 8533 RVA: 0x000A5DE0 File Offset: 0x000A3FE0
	public override void SetColor(Color color)
	{
		ParticleSystem.MainModule main = this.particleSystem.main;
		Color color2 = new Color(Mathf.Pow(color.r, this.gradientColorPower), Mathf.Pow(color.g, this.gradientColorPower), Mathf.Pow(color.b, this.gradientColorPower), color.a);
		main.startColor = new ParticleSystem.MinMaxGradient(this.useLinearColor ? color.linear : color, this.useLinearColor ? color2.linear : color2);
	}

	// Token: 0x04002501 RID: 9473
	public ParticleSystem particleSystem;

	// Token: 0x04002502 RID: 9474
	public float gradientColorPower = 2f;

	// Token: 0x04002503 RID: 9475
	public bool useLinearColor = true;
}
