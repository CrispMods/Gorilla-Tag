using System;
using emotitron.Compression.HalfFloat;
using UnityEngine;

namespace emotitron.Compression
{
	// Token: 0x02000CA0 RID: 3232
	[Serializable]
	public class LiteFloatCrusher : LiteCrusher<float>
	{
		// Token: 0x06005192 RID: 20882 RVA: 0x001BDD24 File Offset: 0x001BBF24
		public LiteFloatCrusher()
		{
			this.compressType = LiteFloatCompressType.Half16;
			this.min = 0f;
			this.max = 1f;
			this.accurateCenter = true;
			LiteFloatCrusher.Recalculate(this.compressType, this.min, this.max, this.accurateCenter, ref this.bits, ref this.encoder, ref this.decoder, ref this.maxCVal);
		}

		// Token: 0x06005193 RID: 20883 RVA: 0x001BDDA0 File Offset: 0x001BBFA0
		public LiteFloatCrusher(LiteFloatCompressType compressType, float min, float max, bool accurateCenter)
		{
			this.compressType = compressType;
			this.min = min;
			this.max = max;
			this.accurateCenter = accurateCenter;
			LiteFloatCrusher.Recalculate(compressType, min, max, accurateCenter, ref this.bits, ref this.encoder, ref this.decoder, ref this.maxCVal);
		}

		// Token: 0x06005194 RID: 20884 RVA: 0x001BDE04 File Offset: 0x001BC004
		public static void Recalculate(LiteFloatCompressType compressType, float min, float max, bool accurateCenter, ref int bits, ref float encoder, ref float decoder, ref ulong maxCVal)
		{
			bits = (int)compressType;
			float num = max - min;
			ulong num2 = (bits == 64) ? ulong.MaxValue : ((1UL << bits) - 1UL);
			if (accurateCenter && num2 != 0UL)
			{
				num2 -= 1UL;
			}
			encoder = ((num == 0f) ? 0f : (num2 / num));
			decoder = ((num2 == 0UL) ? 0f : (num / num2));
			maxCVal = num2;
		}

		// Token: 0x06005195 RID: 20885 RVA: 0x001BDE6C File Offset: 0x001BC06C
		public override ulong Encode(float val)
		{
			if (this.compressType == LiteFloatCompressType.Half16)
			{
				return (ulong)HalfUtilities.Pack(val);
			}
			if (this.compressType == LiteFloatCompressType.Full32)
			{
				return (ulong)val.uint32;
			}
			float num = (val - this.min) * this.encoder + 0.5f;
			if (num < 0f)
			{
				return 0UL;
			}
			ulong num2 = (ulong)num;
			if (num2 <= this.maxCVal)
			{
				return num2;
			}
			return this.maxCVal;
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x001BDED8 File Offset: 0x001BC0D8
		public override float Decode(uint cval)
		{
			if (this.compressType == LiteFloatCompressType.Half16)
			{
				return HalfUtilities.Unpack((ushort)cval);
			}
			if (this.compressType == LiteFloatCompressType.Full32)
			{
				return cval.float32;
			}
			if (cval == 0U)
			{
				return this.min;
			}
			if ((ulong)cval == this.maxCVal)
			{
				return this.max;
			}
			return cval * this.decoder + this.min;
		}

		// Token: 0x06005197 RID: 20887 RVA: 0x001BDF3C File Offset: 0x001BC13C
		public override ulong WriteValue(float val, byte[] buffer, ref int bitposition)
		{
			if (this.compressType == LiteFloatCompressType.Half16)
			{
				ulong num = (ulong)HalfUtilities.Pack(val);
				buffer.Write(num, ref bitposition, 16);
				return num;
			}
			if (this.compressType == LiteFloatCompressType.Full32)
			{
				ulong num2 = (ulong)val.uint32;
				buffer.Write(num2, ref bitposition, 32);
				return num2;
			}
			ulong num3 = this.Encode(val);
			buffer.Write(num3, ref bitposition, (int)this.compressType);
			return num3;
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x00064E81 File Offset: 0x00063081
		public override void WriteCValue(uint cval, byte[] buffer, ref int bitposition)
		{
			if (this.compressType == LiteFloatCompressType.Half16)
			{
				buffer.Write((ulong)cval, ref bitposition, 16);
				return;
			}
			if (this.compressType == LiteFloatCompressType.Full32)
			{
				buffer.Write((ulong)cval, ref bitposition, 32);
				return;
			}
			buffer.Write((ulong)cval, ref bitposition, (int)this.compressType);
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x001BDFA4 File Offset: 0x001BC1A4
		public override float ReadValue(byte[] buffer, ref int bitposition)
		{
			if (this.compressType == LiteFloatCompressType.Half16)
			{
				return HalfUtilities.Unpack((ushort)buffer.Read(ref bitposition, 16));
			}
			if (this.compressType == LiteFloatCompressType.Full32)
			{
				return ((uint)buffer.Read(ref bitposition, 32)).float32;
			}
			uint val = (uint)buffer.Read(ref bitposition, (int)this.compressType);
			return this.Decode(val);
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x001BE000 File Offset: 0x001BC200
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
				" e: ",
				this.encoder.ToString(),
				" d: ",
				this.decoder.ToString()
			});
		}

		// Token: 0x040053E5 RID: 21477
		[SerializeField]
		protected float min;

		// Token: 0x040053E6 RID: 21478
		[SerializeField]
		protected float max;

		// Token: 0x040053E7 RID: 21479
		[SerializeField]
		public LiteFloatCompressType compressType = LiteFloatCompressType.Half16;

		// Token: 0x040053E8 RID: 21480
		[SerializeField]
		private bool accurateCenter = true;

		// Token: 0x040053E9 RID: 21481
		[SerializeField]
		private float encoder;

		// Token: 0x040053EA RID: 21482
		[SerializeField]
		private float decoder;

		// Token: 0x040053EB RID: 21483
		[SerializeField]
		private ulong maxCVal;
	}
}
