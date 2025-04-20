using System;

namespace TagEffects
{
	// Token: 0x02000B5F RID: 2911
	[Serializable]
	public class TagEffectsCombo : IEquatable<TagEffectsCombo>
	{
		// Token: 0x060048B3 RID: 18611 RVA: 0x001902AC File Offset: 0x0018E4AC
		bool IEquatable<TagEffectsCombo>.Equals(TagEffectsCombo other)
		{
			return (other.inputA == this.inputA && other.inputB == this.inputB) || (other.inputA == this.inputB && other.inputB == this.inputA);
		}

		// Token: 0x060048B4 RID: 18612 RVA: 0x0005F50A File Offset: 0x0005D70A
		public override bool Equals(object obj)
		{
			return this.Equals((TagEffectsCombo)obj);
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x0005F518 File Offset: 0x0005D718
		public override int GetHashCode()
		{
			return this.inputA.GetHashCode() * this.inputB.GetHashCode();
		}

		// Token: 0x040049FE RID: 18942
		public TagEffectPack inputA;

		// Token: 0x040049FF RID: 18943
		public TagEffectPack inputB;
	}
}
