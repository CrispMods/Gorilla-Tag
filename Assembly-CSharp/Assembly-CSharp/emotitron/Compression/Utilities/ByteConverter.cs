using System;
using System.Runtime.InteropServices;

namespace emotitron.Compression.Utilities
{
	// Token: 0x02000C76 RID: 3190
	[StructLayout(LayoutKind.Explicit)]
	public struct ByteConverter
	{
		// Token: 0x17000825 RID: 2085
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

		// Token: 0x06005059 RID: 20569 RVA: 0x001873B0 File Offset: 0x001855B0
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

		// Token: 0x0600505A RID: 20570 RVA: 0x00187434 File Offset: 0x00185634
		public static implicit operator ByteConverter(byte val)
		{
			return new ByteConverter
			{
				byte0 = val
			};
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x00187454 File Offset: 0x00185654
		public static implicit operator ByteConverter(sbyte val)
		{
			return new ByteConverter
			{
				int8 = val
			};
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x00187474 File Offset: 0x00185674
		public static implicit operator ByteConverter(char val)
		{
			return new ByteConverter
			{
				character = val
			};
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x00187494 File Offset: 0x00185694
		public static implicit operator ByteConverter(uint val)
		{
			return new ByteConverter
			{
				uint32 = val
			};
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x001874B4 File Offset: 0x001856B4
		public static implicit operator ByteConverter(int val)
		{
			return new ByteConverter
			{
				int32 = val
			};
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x001874D4 File Offset: 0x001856D4
		public static implicit operator ByteConverter(ulong val)
		{
			return new ByteConverter
			{
				uint64 = val
			};
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x001874F4 File Offset: 0x001856F4
		public static implicit operator ByteConverter(long val)
		{
			return new ByteConverter
			{
				int64 = val
			};
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x00187514 File Offset: 0x00185714
		public static implicit operator ByteConverter(float val)
		{
			return new ByteConverter
			{
				float32 = val
			};
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x00187534 File Offset: 0x00185734
		public static implicit operator ByteConverter(double val)
		{
			return new ByteConverter
			{
				float64 = val
			};
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x00187554 File Offset: 0x00185754
		public static implicit operator ByteConverter(bool val)
		{
			return new ByteConverter
			{
				int32 = (val ? 1 : 0)
			};
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x00187578 File Offset: 0x00185778
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

		// Token: 0x06005065 RID: 20581 RVA: 0x001875EB File Offset: 0x001857EB
		public static implicit operator byte(ByteConverter bc)
		{
			return bc.byte0;
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x001875F3 File Offset: 0x001857F3
		public static implicit operator sbyte(ByteConverter bc)
		{
			return bc.int8;
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x001875FB File Offset: 0x001857FB
		public static implicit operator char(ByteConverter bc)
		{
			return bc.character;
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x00187603 File Offset: 0x00185803
		public static implicit operator ushort(ByteConverter bc)
		{
			return bc.uint16;
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x0018760B File Offset: 0x0018580B
		public static implicit operator short(ByteConverter bc)
		{
			return bc.int16;
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x00187613 File Offset: 0x00185813
		public static implicit operator uint(ByteConverter bc)
		{
			return bc.uint32;
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x0018761B File Offset: 0x0018581B
		public static implicit operator int(ByteConverter bc)
		{
			return bc.int32;
		}

		// Token: 0x0600506C RID: 20588 RVA: 0x00187623 File Offset: 0x00185823
		public static implicit operator ulong(ByteConverter bc)
		{
			return bc.uint64;
		}

		// Token: 0x0600506D RID: 20589 RVA: 0x0018762B File Offset: 0x0018582B
		public static implicit operator long(ByteConverter bc)
		{
			return bc.int64;
		}

		// Token: 0x0600506E RID: 20590 RVA: 0x00187633 File Offset: 0x00185833
		public static implicit operator float(ByteConverter bc)
		{
			return bc.float32;
		}

		// Token: 0x0600506F RID: 20591 RVA: 0x0018763B File Offset: 0x0018583B
		public static implicit operator double(ByteConverter bc)
		{
			return bc.float64;
		}

		// Token: 0x06005070 RID: 20592 RVA: 0x00187643 File Offset: 0x00185843
		public static implicit operator bool(ByteConverter bc)
		{
			return bc.int32 != 0;
		}

		// Token: 0x040052FB RID: 21243
		[FieldOffset(0)]
		public float float32;

		// Token: 0x040052FC RID: 21244
		[FieldOffset(0)]
		public double float64;

		// Token: 0x040052FD RID: 21245
		[FieldOffset(0)]
		public sbyte int8;

		// Token: 0x040052FE RID: 21246
		[FieldOffset(0)]
		public short int16;

		// Token: 0x040052FF RID: 21247
		[FieldOffset(0)]
		public ushort uint16;

		// Token: 0x04005300 RID: 21248
		[FieldOffset(0)]
		public char character;

		// Token: 0x04005301 RID: 21249
		[FieldOffset(0)]
		public int int32;

		// Token: 0x04005302 RID: 21250
		[FieldOffset(0)]
		public uint uint32;

		// Token: 0x04005303 RID: 21251
		[FieldOffset(0)]
		public long int64;

		// Token: 0x04005304 RID: 21252
		[FieldOffset(0)]
		public ulong uint64;

		// Token: 0x04005305 RID: 21253
		[FieldOffset(0)]
		public byte byte0;

		// Token: 0x04005306 RID: 21254
		[FieldOffset(1)]
		public byte byte1;

		// Token: 0x04005307 RID: 21255
		[FieldOffset(2)]
		public byte byte2;

		// Token: 0x04005308 RID: 21256
		[FieldOffset(3)]
		public byte byte3;

		// Token: 0x04005309 RID: 21257
		[FieldOffset(4)]
		public byte byte4;

		// Token: 0x0400530A RID: 21258
		[FieldOffset(5)]
		public byte byte5;

		// Token: 0x0400530B RID: 21259
		[FieldOffset(6)]
		public byte byte6;

		// Token: 0x0400530C RID: 21260
		[FieldOffset(7)]
		public byte byte7;

		// Token: 0x0400530D RID: 21261
		[FieldOffset(4)]
		public uint uint16_B;
	}
}
