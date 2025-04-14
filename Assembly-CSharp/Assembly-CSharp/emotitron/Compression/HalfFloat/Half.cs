using System;
using System.Globalization;

namespace emotitron.Compression.HalfFloat
{
	// Token: 0x02000C77 RID: 3191
	[Serializable]
	public struct Half : IConvertible, IComparable, IComparable<Half>, IEquatable<Half>, IFormattable
	{
		// Token: 0x06005071 RID: 20593 RVA: 0x0018764E File Offset: 0x0018584E
		public Half(float value)
		{
			this.value = HalfUtilities.Pack(value);
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06005072 RID: 20594 RVA: 0x0018765C File Offset: 0x0018585C
		public ushort RawValue
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06005073 RID: 20595 RVA: 0x00187664 File Offset: 0x00185864
		public static float[] ConvertToFloat(Half[] values)
		{
			float[] array = new float[values.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = HalfUtilities.Unpack(values[i].RawValue);
			}
			return array;
		}

		// Token: 0x06005074 RID: 20596 RVA: 0x001876A0 File Offset: 0x001858A0
		public static Half[] ConvertToHalf(float[] values)
		{
			Half[] array = new Half[values.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Half(values[i]);
			}
			return array;
		}

		// Token: 0x06005075 RID: 20597 RVA: 0x001876D4 File Offset: 0x001858D4
		public static bool IsInfinity(Half half)
		{
			return half == Half.PositiveInfinity || half == Half.NegativeInfinity;
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x001876F0 File Offset: 0x001858F0
		public static bool IsNaN(Half half)
		{
			return half == Half.NaN;
		}

		// Token: 0x06005077 RID: 20599 RVA: 0x001876FD File Offset: 0x001858FD
		public static bool IsNegativeInfinity(Half half)
		{
			return half == Half.NegativeInfinity;
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x0018770A File Offset: 0x0018590A
		public static bool IsPositiveInfinity(Half half)
		{
			return half == Half.PositiveInfinity;
		}

		// Token: 0x06005079 RID: 20601 RVA: 0x00187717 File Offset: 0x00185917
		public static bool operator <(Half left, Half right)
		{
			return left < right;
		}

		// Token: 0x0600507A RID: 20602 RVA: 0x00187729 File Offset: 0x00185929
		public static bool operator >(Half left, Half right)
		{
			return left > right;
		}

		// Token: 0x0600507B RID: 20603 RVA: 0x0018773B File Offset: 0x0018593B
		public static bool operator <=(Half left, Half right)
		{
			return left <= right;
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x00187750 File Offset: 0x00185950
		public static bool operator >=(Half left, Half right)
		{
			return left >= right;
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x00187765 File Offset: 0x00185965
		public static bool operator ==(Half left, Half right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600507E RID: 20606 RVA: 0x0018776F File Offset: 0x0018596F
		public static bool operator !=(Half left, Half right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0600507F RID: 20607 RVA: 0x0018777C File Offset: 0x0018597C
		public static explicit operator Half(float value)
		{
			return new Half(value);
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x00187784 File Offset: 0x00185984
		public static implicit operator float(Half value)
		{
			return HalfUtilities.Unpack(value.value);
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x00187794 File Offset: 0x00185994
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, this.ToString(), Array.Empty<object>());
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x001877C4 File Offset: 0x001859C4
		public string ToString(string format)
		{
			if (format == null)
			{
				return this.ToString();
			}
			return string.Format(CultureInfo.CurrentCulture, this.ToString(format, CultureInfo.CurrentCulture), Array.Empty<object>());
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x0018780C File Offset: 0x00185A0C
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, this.ToString(), Array.Empty<object>());
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x00187838 File Offset: 0x00185A38
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (format == null)
			{
				this.ToString(formatProvider);
			}
			return string.Format(formatProvider, this.ToString(format, formatProvider), Array.Empty<object>());
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x00187871 File Offset: 0x00185A71
		public override int GetHashCode()
		{
			return (int)(this.value * 3 / 2 ^ this.value);
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x00187884 File Offset: 0x00185A84
		public int CompareTo(Half value)
		{
			if (this < value)
			{
				return -1;
			}
			if (this > value)
			{
				return 1;
			}
			if (this != value)
			{
				if (!Half.IsNaN(this))
				{
					return 1;
				}
				if (!Half.IsNaN(value))
				{
					return -1;
				}
			}
			return 0;
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x001878DC File Offset: 0x00185ADC
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is Half))
			{
				throw new ArgumentException("The argument value must be a SlimMath.Half.");
			}
			Half half = (Half)value;
			if (this < half)
			{
				return -1;
			}
			if (this > half)
			{
				return 1;
			}
			if (this != half)
			{
				if (!Half.IsNaN(this))
				{
					return 1;
				}
				if (!Half.IsNaN(half))
				{
					return -1;
				}
			}
			return 0;
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x00187950 File Offset: 0x00185B50
		public static bool Equals(ref Half value1, ref Half value2)
		{
			return value1.value == value2.value;
		}

		// Token: 0x06005089 RID: 20617 RVA: 0x00187960 File Offset: 0x00185B60
		public bool Equals(Half other)
		{
			return other.value == this.value;
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x00187970 File Offset: 0x00185B70
		public override bool Equals(object obj)
		{
			return obj != null && !(obj.GetType() != base.GetType()) && this.Equals((Half)obj);
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x001879A2 File Offset: 0x00185BA2
		public TypeCode GetTypeCode()
		{
			return Type.GetTypeCode(typeof(Half));
		}

		// Token: 0x0600508C RID: 20620 RVA: 0x001879B3 File Offset: 0x00185BB3
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x001879C5 File Offset: 0x00185BC5
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x001879D7 File Offset: 0x00185BD7
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException("Invalid cast from SlimMath.Half to System.Char.");
		}

		// Token: 0x0600508F RID: 20623 RVA: 0x001879E3 File Offset: 0x00185BE3
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException("Invalid cast from SlimMath.Half to System.DateTime.");
		}

		// Token: 0x06005090 RID: 20624 RVA: 0x001879EF File Offset: 0x00185BEF
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06005091 RID: 20625 RVA: 0x00187A01 File Offset: 0x00185C01
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x00187A13 File Offset: 0x00185C13
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x00187A25 File Offset: 0x00185C25
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x00187A37 File Offset: 0x00185C37
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x00187A49 File Offset: 0x00185C49
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x00187A5B File Offset: 0x00185C5B
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x00187A68 File Offset: 0x00185C68
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return ((IConvertible)this).ToType(type, provider);
		}

		// Token: 0x06005098 RID: 20632 RVA: 0x00187A82 File Offset: 0x00185C82
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x00187A94 File Offset: 0x00185C94
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x00187AA6 File Offset: 0x00185CA6
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x0400530E RID: 21262
		private ushort value;

		// Token: 0x0400530F RID: 21263
		public const int PrecisionDigits = 3;

		// Token: 0x04005310 RID: 21264
		public const int MantissaBits = 11;

		// Token: 0x04005311 RID: 21265
		public const int MaximumDecimalExponent = 4;

		// Token: 0x04005312 RID: 21266
		public const int MaximumBinaryExponent = 15;

		// Token: 0x04005313 RID: 21267
		public const int MinimumDecimalExponent = -4;

		// Token: 0x04005314 RID: 21268
		public const int MinimumBinaryExponent = -14;

		// Token: 0x04005315 RID: 21269
		public const int ExponentRadix = 2;

		// Token: 0x04005316 RID: 21270
		public const int AdditionRounding = 1;

		// Token: 0x04005317 RID: 21271
		public static readonly Half Epsilon = new Half(0.0004887581f);

		// Token: 0x04005318 RID: 21272
		public static readonly Half MaxValue = new Half(65504f);

		// Token: 0x04005319 RID: 21273
		public static readonly Half MinValue = new Half(6.103516E-05f);

		// Token: 0x0400531A RID: 21274
		public static readonly Half NaN = new Half(float.NaN);

		// Token: 0x0400531B RID: 21275
		public static readonly Half NegativeInfinity = new Half(float.NegativeInfinity);

		// Token: 0x0400531C RID: 21276
		public static readonly Half PositiveInfinity = new Half(float.PositiveInfinity);
	}
}
