using System;
using UnityEngine;

namespace emotitron.Compression
{
	// Token: 0x02000CA2 RID: 3234
	[Serializable]
	public class LiteIntCrusher : LiteCrusher<int>
	{
		// Token: 0x0600519B RID: 20891 RVA: 0x001BE0A0 File Offset: 0x001BC2A0
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

		// Token: 0x0600519C RID: 20892 RVA: 0x00064EBE File Offset: 0x000630BE
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

		// Token: 0x0600519D RID: 20893 RVA: 0x001BE0F8 File Offset: 0x001BC2F8
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

		// Token: 0x0600519E RID: 20894 RVA: 0x001BE164 File Offset: 0x001BC364
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

		// Token: 0x0600519F RID: 20895 RVA: 0x001BE1B4 File Offset: 0x001BC3B4
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

		// Token: 0x060051A0 RID: 20896 RVA: 0x00064EFD File Offset: 0x000630FD
		public override ulong Encode(int value)
		{
			value = ((value > this.biggest) ? this.biggest : ((value < this.smallest) ? this.smallest : value));
			return (ulong)((long)(value - this.smallest));
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x00064F2D File Offset: 0x0006312D
		public override int Decode(uint cvalue)
		{
			return (int)((ulong)cvalue + (ulong)((long)this.smallest));
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x001BE20C File Offset: 0x001BC40C
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

		// Token: 0x060051A3 RID: 20899 RVA: 0x001BE23C File Offset: 0x001BC43C
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

		// Token: 0x040053F0 RID: 21488
		[SerializeField]
		public LiteIntCompressType compressType;

		// Token: 0x040053F1 RID: 21489
		[SerializeField]
		protected int min;

		// Token: 0x040053F2 RID: 21490
		[SerializeField]
		protected int max;

		// Token: 0x040053F3 RID: 21491
		[SerializeField]
		private int smallest;

		// Token: 0x040053F4 RID: 21492
		[SerializeField]
		private int biggest;
	}
}
