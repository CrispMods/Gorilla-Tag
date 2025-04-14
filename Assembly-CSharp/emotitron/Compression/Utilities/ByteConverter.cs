using System;
using System.Runtime.InteropServices;

namespace emotitron.Compression.Utilities
{
	// Token: 0x02000C73 RID: 3187
	[StructLayout(LayoutKind.Explicit)]
	public struct ByteConverter
	{
		// Token: 0x17000824 RID: 2084
		public byte this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.byte0;
				case 1:
					return this.byte1;
				case 2:
					return this.byte2;
				case 3:
					return this.byte3;
				case 4:
					return this.byte4;
				case 5:
					return this.byte5;
				case 6:
					return this.byte6;
				case 7:
					return this.byte7;
				default:
					return 0;
				}
			}
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x00186DE8 File Offset: 0x00184FE8
		public static implicit operator ByteConverter(byte[] bytes)
		{
			ByteConverter result = default(ByteConverter);
			int num = bytes.Length;
			result.byte0 = bytes[0];
			if (num > 0)
			{
				result.byte1 = bytes[1];
			}
			if (num > 1)
			{
				result.byte2 = bytes[2];
			}
			if (num > 2)
			{
				result.byte3 = bytes[3];
			}
			if (num > 3)
			{
				result.byte4 = bytes[4];
			}
			if (num > 4)
			{
				result.byte5 = bytes[5];
			}
			if (num > 5)
			{
				result.byte6 = bytes[3];
			}
			if (num > 6)
			{
				result.byte7 = bytes[7];
			}
			return result;
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x00186E6C File Offset: 0x0018506C
		public static implicit operator ByteConverter(byte val)
		{
			return new ByteConverter
			{
				byte0 = val
			};
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x00186E8C File Offset: 0x0018508C
		public static implicit operator ByteConverter(sbyte val)
		{
			return new ByteConverter
			{
				int8 = val
			};
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x00186EAC File Offset: 0x001850AC
		public static implicit operator ByteConverter(char val)
		{
			return new ByteConverter
			{
				character = val
			};
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x00186ECC File Offset: 0x001850CC
		public static implicit operator ByteConverter(uint val)
		{
			return new ByteConverter
			{
				uint32 = val
			};
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x00186EEC File Offset: 0x001850EC
		public static implicit operator ByteConverter(int val)
		{
			return new ByteConverter
			{
				int32 = val
			};
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x00186F0C File Offset: 0x0018510C
		public static implicit operator ByteConverter(ulong val)
		{
			return new ByteConverter
			{
				uint64 = val
			};
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x00186F2C File Offset: 0x0018512C
		public static implicit operator ByteConverter(long val)
		{
			return new ByteConverter
			{
				int64 = val
			};
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x00186F4C File Offset: 0x0018514C
		public static implicit operator ByteConverter(float val)
		{
			return new ByteConverter
			{
				float32 = val
			};
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x00186F6C File Offset: 0x0018516C
		public static implicit operator ByteConverter(double val)
		{
			return new ByteConverter
			{
				float64 = val
			};
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x00186F8C File Offset: 0x0018518C
		public static implicit operator ByteConverter(bool val)
		{
			return new ByteConverter
			{
				int32 = (val ? 1 : 0)
			};
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x00186FB0 File Offset: 0x001851B0
		public void ExtractByteArray(byte[] targetArray)
		{
			int num = targetArray.Length;
			targetArray[0] = this.byte0;
			if (num > 0)
			{
				targetArray[1] = this.byte1;
			}
			if (num > 1)
			{
				targetArray[2] = this.byte2;
			}
			if (num > 2)
			{
				targetArray[3] = this.byte3;
			}
			if (num > 3)
			{
				targetArray[4] = this.byte4;
			}
			if (num > 4)
			{
				targetArray[5] = this.byte5;
			}
			if (num > 5)
			{
				targetArray[6] = this.byte6;
			}
			if (num > 6)
			{
				targetArray[7] = this.byte7;
			}
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x00187023 File Offset: 0x00185223
		public static implicit operator byte(ByteConverter bc)
		{
			return bc.byte0;
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x0018702B File Offset: 0x0018522B
		public static implicit operator sbyte(ByteConverter bc)
		{
			return bc.int8;
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x00187033 File Offset: 0x00185233
		public static implicit operator char(ByteConverter bc)
		{
			return bc.character;
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x0018703B File Offset: 0x0018523B
		public static implicit operator ushort(ByteConverter bc)
		{
			return bc.uint16;
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x00187043 File Offset: 0x00185243
		public static implicit operator short(ByteConverter bc)
		{
			return bc.int16;
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x0018704B File Offset: 0x0018524B
		public static implicit operator uint(ByteConverter bc)
		{
			return bc.uint32;
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x00187053 File Offset: 0x00185253
		public static implicit operator int(ByteConverter bc)
		{
			return bc.int32;
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x0018705B File Offset: 0x0018525B
		public static implicit operator ulong(ByteConverter bc)
		{
			return bc.uint64;
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x00187063 File Offset: 0x00185263
		public static implicit operator long(ByteConverter bc)
		{
			return bc.int64;
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x0018706B File Offset: 0x0018526B
		public static implicit operator float(ByteConverter bc)
		{
			return bc.float32;
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x00187073 File Offset: 0x00185273
		public static implicit operator double(ByteConverter bc)
		{
			return bc.float64;
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x0018707B File Offset: 0x0018527B
		public static implicit operator bool(ByteConverter bc)
		{
			return bc.int32 != 0;
		}

		// Token: 0x040052E9 RID: 21225
		[FieldOffset(0)]
		public float float32;

		// Token: 0x040052EA RID: 21226
		[FieldOffset(0)]
		public double float64;

		// Token: 0x040052EB RID: 21227
		[FieldOffset(0)]
		public sbyte int8;

		// Token: 0x040052EC RID: 21228
		[FieldOffset(0)]
		public short int16;

		// Token: 0x040052ED RID: 21229
		[FieldOffset(0)]
		public ushort uint16;

		// Token: 0x040052EE RID: 21230
		[FieldOffset(0)]
		public char character;

		// Token: 0x040052EF RID: 21231
		[FieldOffset(0)]
		public int int32;

		// Token: 0x040052F0 RID: 21232
		[FieldOffset(0)]
		public uint uint32;

		// Token: 0x040052F1 RID: 21233
		[FieldOffset(0)]
		public long int64;

		// Token: 0x040052F2 RID: 21234
		[FieldOffset(0)]
		public ulong uint64;

		// Token: 0x040052F3 RID: 21235
		[FieldOffset(0)]
		public byte byte0;

		// Token: 0x040052F4 RID: 21236
		[FieldOffset(1)]
		public byte byte1;

		// Token: 0x040052F5 RID: 21237
		[FieldOffset(2)]
		public byte byte2;

		// Token: 0x040052F6 RID: 21238
		[FieldOffset(3)]
		public byte byte3;

		// Token: 0x040052F7 RID: 21239
		[FieldOffset(4)]
		public byte byte4;

		// Token: 0x040052F8 RID: 21240
		[FieldOffset(5)]
		public byte byte5;

		// Token: 0x040052F9 RID: 21241
		[FieldOffset(6)]
		public byte byte6;

		// Token: 0x040052FA RID: 21242
		[FieldOffset(7)]
		public byte byte7;

		// Token: 0x040052FB RID: 21243
		[FieldOffset(4)]
		public uint uint16_B;
	}
}
