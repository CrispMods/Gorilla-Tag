using System;
using System.Runtime.InteropServices;

namespace emotitron.Compression.Utilities
{
	// Token: 0x02000CA4 RID: 3236
	[StructLayout(LayoutKind.Explicit)]
	public struct ByteConverter
	{
		// Token: 0x17000842 RID: 2114
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

		// Token: 0x060051AD RID: 20909 RVA: 0x001BE334 File Offset: 0x001BC534
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

		// Token: 0x060051AE RID: 20910 RVA: 0x001BE3B8 File Offset: 0x001BC5B8
		public static implicit operator ByteConverter(byte val)
		{
			return new ByteConverter
			{
				byte0 = val
			};
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x001BE3D8 File Offset: 0x001BC5D8
		public static implicit operator ByteConverter(sbyte val)
		{
			return new ByteConverter
			{
				int8 = val
			};
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x001BE3F8 File Offset: 0x001BC5F8
		public static implicit operator ByteConverter(char val)
		{
			return new ByteConverter
			{
				character = val
			};
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x001BE418 File Offset: 0x001BC618
		public static implicit operator ByteConverter(uint val)
		{
			return new ByteConverter
			{
				uint32 = val
			};
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x001BE438 File Offset: 0x001BC638
		public static implicit operator ByteConverter(int val)
		{
			return new ByteConverter
			{
				int32 = val
			};
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x001BE458 File Offset: 0x001BC658
		public static implicit operator ByteConverter(ulong val)
		{
			return new ByteConverter
			{
				uint64 = val
			};
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x001BE478 File Offset: 0x001BC678
		public static implicit operator ByteConverter(long val)
		{
			return new ByteConverter
			{
				int64 = val
			};
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x001BE498 File Offset: 0x001BC698
		public static implicit operator ByteConverter(float val)
		{
			return new ByteConverter
			{
				float32 = val
			};
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x001BE4B8 File Offset: 0x001BC6B8
		public static implicit operator ByteConverter(double val)
		{
			return new ByteConverter
			{
				float64 = val
			};
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x001BE4D8 File Offset: 0x001BC6D8
		public static implicit operator ByteConverter(bool val)
		{
			return new ByteConverter
			{
				int32 = (val ? 1 : 0)
			};
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x001BE4FC File Offset: 0x001BC6FC
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

		// Token: 0x060051B9 RID: 20921 RVA: 0x00064F93 File Offset: 0x00063193
		public static implicit operator byte(ByteConverter bc)
		{
			return bc.byte0;
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x00064F9B File Offset: 0x0006319B
		public static implicit operator sbyte(ByteConverter bc)
		{
			return bc.int8;
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x00064FA3 File Offset: 0x000631A3
		public static implicit operator char(ByteConverter bc)
		{
			return bc.character;
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x00064FAB File Offset: 0x000631AB
		public static implicit operator ushort(ByteConverter bc)
		{
			return bc.uint16;
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x00064FB3 File Offset: 0x000631B3
		public static implicit operator short(ByteConverter bc)
		{
			return bc.int16;
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x00064FBB File Offset: 0x000631BB
		public static implicit operator uint(ByteConverter bc)
		{
			return bc.uint32;
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x00064FC3 File Offset: 0x000631C3
		public static implicit operator int(ByteConverter bc)
		{
			return bc.int32;
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x00064FCB File Offset: 0x000631CB
		public static implicit operator ulong(ByteConverter bc)
		{
			return bc.uint64;
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x00064FD3 File Offset: 0x000631D3
		public static implicit operator long(ByteConverter bc)
		{
			return bc.int64;
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x00064FDB File Offset: 0x000631DB
		public static implicit operator float(ByteConverter bc)
		{
			return bc.float32;
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x00064FE3 File Offset: 0x000631E3
		public static implicit operator double(ByteConverter bc)
		{
			return bc.float64;
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x00064FEB File Offset: 0x000631EB
		public static implicit operator bool(ByteConverter bc)
		{
			return bc.int32 != 0;
		}

		// Token: 0x040053F5 RID: 21493
		[FieldOffset(0)]
		public float float32;

		// Token: 0x040053F6 RID: 21494
		[FieldOffset(0)]
		public double float64;

		// Token: 0x040053F7 RID: 21495
		[FieldOffset(0)]
		public sbyte int8;

		// Token: 0x040053F8 RID: 21496
		[FieldOffset(0)]
		public short int16;

		// Token: 0x040053F9 RID: 21497
		[FieldOffset(0)]
		public ushort uint16;

		// Token: 0x040053FA RID: 21498
		[FieldOffset(0)]
		public char character;

		// Token: 0x040053FB RID: 21499
		[FieldOffset(0)]
		public int int32;

		// Token: 0x040053FC RID: 21500
		[FieldOffset(0)]
		public uint uint32;

		// Token: 0x040053FD RID: 21501
		[FieldOffset(0)]
		public long int64;

		// Token: 0x040053FE RID: 21502
		[FieldOffset(0)]
		public ulong uint64;

		// Token: 0x040053FF RID: 21503
		[FieldOffset(0)]
		public byte byte0;

		// Token: 0x04005400 RID: 21504
		[FieldOffset(1)]
		public byte byte1;

		// Token: 0x04005401 RID: 21505
		[FieldOffset(2)]
		public byte byte2;

		// Token: 0x04005402 RID: 21506
		[FieldOffset(3)]
		public byte byte3;

		// Token: 0x04005403 RID: 21507
		[FieldOffset(4)]
		public byte byte4;

		// Token: 0x04005404 RID: 21508
		[FieldOffset(5)]
		public byte byte5;

		// Token: 0x04005405 RID: 21509
		[FieldOffset(6)]
		public byte byte6;

		// Token: 0x04005406 RID: 21510
		[FieldOffset(7)]
		public byte byte7;

		// Token: 0x04005407 RID: 21511
		[FieldOffset(4)]
		public uint uint16_B;
	}
}
