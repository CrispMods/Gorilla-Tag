using System;

namespace TagEffects
{
	// Token: 0x02000B32 RID: 2866
	[Serializable]
	public class TagEffectsCombo : IEquatable<TagEffectsCombo>
	{
		// Token: 0x0600476A RID: 18282 RVA: 0x00153FB8 File Offset: 0x001521B8
		bool IEquatable<TagEffectsCombo>.Equals(TagEffectsCombo other)
		{
			return (other.inputA == this.inputA && other.inputB == this.inputB) || (other.inputA == this.inputB && other.inputB == this.inputA);
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x00154013 File Offset: 0x00152213
		public override bool Equals(object obj)
		{
			return this.Equals((TagEffectsCombo)obj);
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x00154021 File Offset: 0x00152221
		public override int GetHashCode()
		{
			return this.inputA.GetHashCode() * this.inputB.GetHashCode();
		}

		// Token: 0x04004909 RID: 18697
		public TagEffectPack inputA;

		// Token: 0x0400490A RID: 18698
		public TagEffectPack inputB;
	}
}
