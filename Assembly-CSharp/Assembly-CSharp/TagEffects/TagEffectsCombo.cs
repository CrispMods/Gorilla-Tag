﻿using System;

namespace TagEffects
{
	// Token: 0x02000B35 RID: 2869
	[Serializable]
	public class TagEffectsCombo : IEquatable<TagEffectsCombo>
	{
		// Token: 0x06004776 RID: 18294 RVA: 0x00154580 File Offset: 0x00152780
		bool IEquatable<TagEffectsCombo>.Equals(TagEffectsCombo other)
		{
			return (other.inputA == this.inputA && other.inputB == this.inputB) || (other.inputA == this.inputB && other.inputB == this.inputA);
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x001545DB File Offset: 0x001527DB
		public override bool Equals(object obj)
		{
			return this.Equals((TagEffectsCombo)obj);
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x001545E9 File Offset: 0x001527E9
		public override int GetHashCode()
		{
			return this.inputA.GetHashCode() * this.inputB.GetHashCode();
		}

		// Token: 0x0400491B RID: 18715
		public TagEffectPack inputA;

		// Token: 0x0400491C RID: 18716
		public TagEffectPack inputB;
	}
}
