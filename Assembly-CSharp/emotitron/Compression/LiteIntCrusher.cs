using System;
using UnityEngine;

namespace emotitron.Compression
{
	// Token: 0x02000C71 RID: 3185
	[Serializable]
	public class LiteIntCrusher : LiteCrusher<int>
	{
		// Token: 0x0600503B RID: 20539 RVA: 0x00186A80 File Offset: 0x00184C80
		public LiteIntCrusher()
		{
			this.compressType = LiteIntCompressType.PackSigned;
			this.min = -128;
			this.max = 127;
			if (this.compressType == LiteIntCompressType.Range)
			{
				LiteIntCrusher.Recalculate(this.min, this.max, ref this.smallest, ref this.biggest, ref this.bits);
			}
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x00186AD6 File Offset: 0x00184CD6
		public LiteIntCrusher(LiteIntCompressType comType = LiteIntCompressType.PackSigned, int min = -128, int max = 127)
		{
			this.compressType = comType;
			this.min = min;
			this.max = max;
			if (this.compressType == LiteIntCompressType.Range)
			{
				LiteIntCrusher.Recalculate(min, max, ref this.smallest, ref this.biggest, ref this.bits);
			}
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x00186B18 File Offset: 0x00184D18
		public override ulong WriteValue(int val, byte[] buffer, ref int bitposition)
		{
			switch (this.compressType)
			{
			case LiteIntCompressType.PackSigned:
			{
				uint num = (uint)(val << 1 ^ val >> 31);
				buffer.WritePackedBytes((ulong)num, ref bitposition, 32);
				return (ulong)num;
			}
			case LiteIntCompressType.PackUnsigned:
				buffer.WritePackedBytes((ulong)val, ref bitposition, 32);
				return (ulong)val;
			case LiteIntCompressType.Range:
			{
				ulong num2 = this.Encode(val);
				buffer.Write(num2, ref bitposition, this.bits);
				return num2;
			}
			default:
				return 0UL;
			}
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x00186B84 File Offset: 0x00184D84
		public override void WriteCValue(uint cval, byte[] buffer, ref int bitposition)
		{
			switch (this.compressType)
			{
			case LiteIntCompressType.PackSigned:
				buffer.WritePackedBytes((ulong)cval, ref bitposition, 32);
				return;
			case LiteIntCompressType.PackUnsigned:
				buffer.WritePackedBytes((ulong)cval, ref bitposition, 32);
				return;
			case LiteIntCompressType.Range:
				buffer.Write((ulong)cval, ref bitposition, this.bits);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x00186BD4 File Offset: 0x00184DD4
		public override int ReadValue(byte[] buffer, ref int bitposition)
		{
			switch (this.compressType)
			{
			case LiteIntCompressType.PackSigned:
				return buffer.ReadSignedPackedBytes(ref bitposition, 32);
			case LiteIntCompressType.PackUnsigned:
				return (int)buffer.ReadPackedBytes(ref bitposition, 32);
			case LiteIntCompressType.Range:
			{
				uint val = (uint)buffer.Read(ref bitposition, this.bits);
				return this.Decode(val);
			}
			default:
				return 0;
			}
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x00186C29 File Offset: 0x00184E29
		public override ulong Encode(int value)
		{
			value = ((value > this.biggest) ? this.biggest : ((value < this.smallest) ? this.smallest : value));
			return (ulong)((long)(value - this.smallest));
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x00186C59 File Offset: 0x00184E59
		public override int Decode(uint cvalue)
		{
			return (int)((ulong)cvalue + (ulong)((long)this.smallest));
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x00186C68 File Offset: 0x00184E68
		public static void Recalculate(int min, int max, ref int smallest, ref int biggest, ref int bits)
		{
			if (min < max)
			{
				smallest = min;
				biggest = max;
			}
			else
			{
				smallest = max;
				biggest = min;
			}
			int maxvalue = biggest - smallest;
			bits = LiteCrusher.GetBitsForMaxValue((uint)maxvalue);
		}

		// Token: 0x06005043 RID: 20547 RVA: 0x00186C98 File Offset: 0x00184E98
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				base.GetType().Name,
				" ",
				this.compressType.ToString(),
				" mn: ",
				this.min.ToString(),
				" mx: ",
				this.max.ToString(),
				" sm: ",
				this.smallest.ToString()
			});
		}

		// Token: 0x040052E4 RID: 21220
		[SerializeField]
		public LiteIntCompressType compressType;

		// Token: 0x040052E5 RID: 21221
		[SerializeField]
		protected int min;

		// Token: 0x040052E6 RID: 21222
		[SerializeField]
		protected int max;

		// Token: 0x040052E7 RID: 21223
		[SerializeField]
		private int smallest;

		// Token: 0x040052E8 RID: 21224
		[SerializeField]
		private int biggest;
	}
}
