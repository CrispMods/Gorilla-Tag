using System;
using UnityEngine;

// Token: 0x02000554 RID: 1364
public class GorillaColorizableParticle : GorillaColorizableBase
{
	// Token: 0x0600215D RID: 8541 RVA: 0x000A6260 File Offset: 0x000A4460
	public override void SetColor(Color color)
	{
		ParticleSystem.MainModule main = this.particleSystem.main;
		Color color2 = new Color(Mathf.Pow(color.r, this.gradientColorPower), Mathf.Pow(color.g, this.gradientColorPower), Mathf.Pow(color.b, this.gradientColorPower), color.a);
		main.startColor = new ParticleSystem.MinMaxGradient(this.useLinearColor ? color.linear : color, this.useLinearColor ? color2.linear : color2);
	}

	// Token: 0x04002507 RID: 9479
	public ParticleSystem particleSystem;

	// Token: 0x04002508 RID: 9480
	public float gradientColorPower = 2f;

	// Token: 0x04002509 RID: 9481
	public bool useLinearColor = true;
}
