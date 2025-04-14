using System;

namespace GTMathUtil
{
	// Token: 0x02000970 RID: 2416
	internal class CriticalSpringDamper
	{
		// Token: 0x06003AF3 RID: 15091 RVA: 0x0010D4C2 File Offset: 0x0010B6C2
		private static float halflife_to_damping(float halflife, float eps = 1E-05f)
		{
			return 2.7725887f / (halflife + eps);
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x0010D1EA File Offset: 0x0010B3EA
		private static float fast_negexp(float x)
		{
			return 1f / (1f + x + 0.48f * x * x + 0.235f * x * x * x);
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x0010F418 File Offset: 0x0010D618
		public float Update(float dt)
		{
			float num = CriticalSpringDamper.halflife_to_damping(this.halfLife, 1E-05f) / 2f;
			float num2 = this.x - this.xGoal;
			float num3 = this.curVel + num2 * num;
			float num4 = CriticalSpringDamper.fast_negexp(num * dt);
			this.x = num4 * (num2 + num3 * dt) + this.xGoal;
			this.curVel = num4 * (this.curVel - num3 * num * dt);
			return this.x;
		}

		// Token: 0x04003BF8 RID: 15352
		public float x;

		// Token: 0x04003BF9 RID: 15353
		public float xGoal;

		// Token: 0x04003BFA RID: 15354
		public float halfLife = 0.1f;

		// Token: 0x04003BFB RID: 15355
		private float curVel;
	}
}
