using System;

namespace GTMathUtil
{
	// Token: 0x02000973 RID: 2419
	internal class CriticalSpringDamper
	{
		// Token: 0x06003AFF RID: 15103 RVA: 0x0010DA8A File Offset: 0x0010BC8A
		private static float halflife_to_damping(float halflife, float eps = 1E-05f)
		{
			return 2.7725887f / (halflife + eps);
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x0010D7B2 File Offset: 0x0010B9B2
		private static float fast_negexp(float x)
		{
			return 1f / (1f + x + 0.48f * x * x + 0.235f * x * x * x);
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x0010F9E0 File Offset: 0x0010DBE0
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

		// Token: 0x04003C0A RID: 15370
		public float x;

		// Token: 0x04003C0B RID: 15371
		public float xGoal;

		// Token: 0x04003C0C RID: 15372
		public float halfLife = 0.1f;

		// Token: 0x04003C0D RID: 15373
		private float curVel;
	}
}
