using System;
using UnityEngine;

namespace emotitron.Compression
{
	// Token: 0x02000C74 RID: 3188
	[Serializable]
	public class LiteIntCrusher : LiteCrusher<int>
	{
		// Token: 0x06005047 RID: 20551 RVA: 0x001B5FBC File Offset: 0x001B41BC
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

		// Token: 0x06005048 RID: 20552 RVA: 0x00063499 File Offset: 0x00061699
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

		// Token: 0x06005049 RID: 20553 RVA: 0x001B6014 File Offset: 0x001B4214
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

		// Token: 0x0600504A RID: 20554 RVA: 0x001B6080 File Offset: 0x001B4280
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

		// Token: 0x0600504B RID: 20555 RVA: 0x001B60D0 File Offset: 0x001B42D0
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

		// Token: 0x0600504C RID: 20556 RVA: 0x000634D8 File Offset: 0x000616D8
		public override ulong Encode(int value)
		{
			value = ((value > this.biggest) ? this.biggest : ((value < this.smallest) ? this.smallest : value));
			return (ulong)((long)(value - this.smallest));
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x00063508 File Offset: 0x00061708
		public override int Decode(uint cvalue)
		{
			return (int)((ulong)cvalue + (ulong)((long)this.smallest));
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x001B6128 File Offset: 0x001B4328
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

		// Token: 0x0600504F RID: 20559 RVA: 0x001B6158 File Offset: 0x001B4358
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

		// Token: 0x040052F6 RID: 21238
		[SerializeField]
		public LiteIntCompressType compressType;

		// Token: 0x040052F7 RID: 21239
		[SerializeField]
		protected int min;

		// Token: 0x040052F8 RID: 21240
		[SerializeField]
		protected int max;

		// Token: 0x040052F9 RID: 21241
		[SerializeField]
		private int smallest;

		// Token: 0x040052FA RID: 21242
		[SerializeField]
		private int biggest;
	}
}
