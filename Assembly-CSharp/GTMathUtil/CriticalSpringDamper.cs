using System;

namespace GTMathUtil
{
	// Token: 0x02000996 RID: 2454
	internal class CriticalSpringDamper
	{
		// Token: 0x06003C0B RID: 15371 RVA: 0x00056EAF File Offset: 0x000550AF
		private static float halflife_to_damping(float halflife, float eps = 1E-05f)
		{
			return 2.7725887f / (halflife + eps);
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x00056E42 File Offset: 0x00055042
		private static float fast_negexp(float x)
		{
			return 1f / (1f + x + 0.48f * x * x + 0.235f * x * x * x);
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x00152BA0 File Offset: 0x00150DA0
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

		// Token: 0x04003CD2 RID: 15570
		public float x;

		// Token: 0x04003CD3 RID: 15571
		public float xGoal;

		// Token: 0x04003CD4 RID: 15572
		public float halfLife = 0.1f;

		// Token: 0x04003CD5 RID: 15573
		private float curVel;
	}
}
