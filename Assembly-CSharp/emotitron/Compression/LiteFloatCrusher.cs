using System;
using emotitron.Compression.HalfFloat;
using UnityEngine;

namespace emotitron.Compression
{
	// Token: 0x02000C6F RID: 3183
	[Serializable]
	public class LiteFloatCrusher : LiteCrusher<float>
	{
		// Token: 0x06005032 RID: 20530 RVA: 0x001866C8 File Offset: 0x001848C8
		public LiteFloatCrusher()
		{
			this.compressType = LiteFloatCompressType.Half16;
			this.min = 0f;
			this.max = 1f;
			this.accurateCenter = true;
			LiteFloatCrusher.Recalculate(this.compressType, this.min, this.max, this.accurateCenter, ref this.bits, ref this.encoder, ref this.decoder, ref this.maxCVal);
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x00186744 File Offset: 0x00184944
		public LiteFloatCrusher(LiteFloatCompressType compressType, float min, float max, bool accurateCenter)
		{
			this.compressType = compressType;
			this.min = min;
			this.max = max;
			this.accurateCenter = accurateCenter;
			LiteFloatCrusher.Recalculate(compressType, min, max, accurateCenter, ref this.bits, ref this.encoder, ref this.decoder, ref this.maxCVal);
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x001867A8 File Offset: 0x001849A8
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

		// Token: 0x06005035 RID: 20533 RVA: 0x00186810 File Offset: 0x00184A10
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

		// Token: 0x06005036 RID: 20534 RVA: 0x0018687C File Offset: 0x00184A7C
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

		// Token: 0x06005037 RID: 20535 RVA: 0x001868E0 File Offset: 0x00184AE0
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

		// Token: 0x06005038 RID: 20536 RVA: 0x00186945 File Offset: 0x00184B45
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

		// Token: 0x06005039 RID: 20537 RVA: 0x00186984 File Offset: 0x00184B84
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

		// Token: 0x0600503A RID: 20538 RVA: 0x001869E0 File Offset: 0x00184BE0
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

		// Token: 0x040052D9 RID: 21209
		[SerializeField]
		protected float min;

		// Token: 0x040052DA RID: 21210
		[SerializeField]
		protected float max;

		// Token: 0x040052DB RID: 21211
		[SerializeField]
		public LiteFloatCompressType compressType = LiteFloatCompressType.Half16;

		// Token: 0x040052DC RID: 21212
		[SerializeField]
		private bool accurateCenter = true;

		// Token: 0x040052DD RID: 21213
		[SerializeField]
		private float encoder;

		// Token: 0x040052DE RID: 21214
		[SerializeField]
		private float decoder;

		// Token: 0x040052DF RID: 21215
		[SerializeField]
		private ulong maxCVal;
	}
}
