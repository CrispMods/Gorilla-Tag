using System;
using System.Globalization;

namespace emotitron.Compression.HalfFloat
{
	// Token: 0x02000C74 RID: 3188
	[Serializable]
	public struct Half : IConvertible, IComparable, IComparable<Half>, IEquatable<Half>, IFormattable
	{
		// Token: 0x06005065 RID: 20581 RVA: 0x00187086 File Offset: 0x00185286
		public Half(float value)
		{
			this.value = HalfUtilities.Pack(value);
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06005066 RID: 20582 RVA: 0x00187094 File Offset: 0x00185294
		public ushort RawValue
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x0018709C File Offset: 0x0018529C
		public static float[] ConvertToFloat(Half[] values)
		{
			float[] array = new float[values.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = HalfUtilities.Unpack(values[i].RawValue);
			}
			return array;
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x001870D8 File Offset: 0x001852D8
		public static Half[] ConvertToHalf(float[] values)
		{
			Half[] array = new Half[values.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Half(values[i]);
			}
			return array;
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x0018710C File Offset: 0x0018530C
		public static bool IsInfinity(Half half)
		{
			return half == Half.PositiveInfinity || half == Half.NegativeInfinity;
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x00187128 File Offset: 0x00185328
		public static bool IsNaN(Half half)
		{
			return half == Half.NaN;
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x00187135 File Offset: 0x00185335
		public static bool IsNegativeInfinity(Half half)
		{
			return half == Half.NegativeInfinity;
		}

		// Token: 0x0600506C RID: 20588 RVA: 0x00187142 File Offset: 0x00185342
		public static bool IsPositiveInfinity(Half half)
		{
			return half == Half.PositiveInfinity;
		}

		// Token: 0x0600506D RID: 20589 RVA: 0x0018714F File Offset: 0x0018534F
		public static bool operator <(Half left, Half right)
		{
			return left < right;
		}

		// Token: 0x0600506E RID: 20590 RVA: 0x00187161 File Offset: 0x00185361
		public static bool operator >(Half left, Half right)
		{
			return left > right;
		}

		// Token: 0x0600506F RID: 20591 RVA: 0x00187173 File Offset: 0x00185373
		public static bool operator <=(Half left, Half right)
		{
			return left <= right;
		}

		// Token: 0x06005070 RID: 20592 RVA: 0x00187188 File Offset: 0x00185388
		public static bool operator >=(Half left, Half right)
		{
			return left >= right;
		}

		// Token: 0x06005071 RID: 20593 RVA: 0x0018719D File Offset: 0x0018539D
		public static bool operator ==(Half left, Half right)
		{
			return left.Equals(right);
		}

		// Token: 0x06005072 RID: 20594 RVA: 0x001871A7 File Offset: 0x001853A7
		public static bool operator !=(Half left, Half right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06005073 RID: 20595 RVA: 0x001871B4 File Offset: 0x001853B4
		public static explicit operator Half(float value)
		{
			return new Half(value);
		}

		// Token: 0x06005074 RID: 20596 RVA: 0x001871BC File Offset: 0x001853BC
		public static implicit operator float(Half value)
		{
			return HalfUtilities.Unpack(value.value);
		}

		// Token: 0x06005075 RID: 20597 RVA: 0x001871CC File Offset: 0x001853CC
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, this.ToString(), Array.Empty<object>());
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x001871FC File Offset: 0x001853FC
		public string ToString(string format)
		{
			if (format == null)
			{
				return this.ToString();
			}
			return string.Format(CultureInfo.CurrentCulture, this.ToString(format, CultureInfo.CurrentCulture), Array.Empty<object>());
		}

		// Token: 0x06005077 RID: 20599 RVA: 0x00187244 File Offset: 0x00185444
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, this.ToString(), Array.Empty<object>());
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x00187270 File Offset: 0x00185470
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (format == null)
			{
				this.ToString(formatProvider);
			}
			return string.Format(formatProvider, this.ToString(format, formatProvider), Array.Empty<object>());
		}

		// Token: 0x06005079 RID: 20601 RVA: 0x001872A9 File Offset: 0x001854A9
		public override int GetHashCode()
		{
			return (int)(this.value * 3 / 2 ^ this.value);
		}

		// Token: 0x0600507A RID: 20602 RVA: 0x001872BC File Offset: 0x001854BC
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

		// Token: 0x0600507B RID: 20603 RVA: 0x00187314 File Offset: 0x00185514
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

		// Token: 0x0600507C RID: 20604 RVA: 0x00187388 File Offset: 0x00185588
		public static bool Equals(ref Half value1, ref Half value2)
		{
			return value1.value == value2.value;
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x00187398 File Offset: 0x00185598
		public bool Equals(Half other)
		{
			return other.value == this.value;
		}

		// Token: 0x0600507E RID: 20606 RVA: 0x001873A8 File Offset: 0x001855A8
		public override bool Equals(object obj)
		{
			return obj != null && !(obj.GetType() != base.GetType()) && this.Equals((Half)obj);
		}

		// Token: 0x0600507F RID: 20607 RVA: 0x001873DA File Offset: 0x001855DA
		public TypeCode GetTypeCode()
		{
			return Type.GetTypeCode(typeof(Half));
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x001873EB File Offset: 0x001855EB
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x001873FD File Offset: 0x001855FD
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x0018740F File Offset: 0x0018560F
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException("Invalid cast from SlimMath.Half to System.Char.");
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x0018741B File Offset: 0x0018561B
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException("Invalid cast from SlimMath.Half to System.DateTime.");
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x00187427 File Offset: 0x00185627
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x00187439 File Offset: 0x00185639
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x0018744B File Offset: 0x0018564B
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x0018745D File Offset: 0x0018565D
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x0018746F File Offset: 0x0018566F
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06005089 RID: 20617 RVA: 0x00187481 File Offset: 0x00185681
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x00187493 File Offset: 0x00185693
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x001874A0 File Offset: 0x001856A0
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return ((IConvertible)this).ToType(type, provider);
		}

		// Token: 0x0600508C RID: 20620 RVA: 0x001874BA File Offset: 0x001856BA
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x001874CC File Offset: 0x001856CC
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x001874DE File Offset: 0x001856DE
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x040052FC RID: 21244
		private ushort value;

		// Token: 0x040052FD RID: 21245
		public const int PrecisionDigits = 3;

		// Token: 0x040052FE RID: 21246
		public const int MantissaBits = 11;

		// Token: 0x040052FF RID: 21247
		public const int MaximumDecimalExponent = 4;

		// Token: 0x04005300 RID: 21248
		public const int MaximumBinaryExponent = 15;

		// Token: 0x04005301 RID: 21249
		public const int MinimumDecimalExponent = -4;

		// Token: 0x04005302 RID: 21250
		public const int MinimumBinaryExponent = -14;

		// Token: 0x04005303 RID: 21251
		public const int ExponentRadix = 2;

		// Token: 0x04005304 RID: 21252
		public const int AdditionRounding = 1;

		// Token: 0x04005305 RID: 21253
		public static readonly Half Epsilon = new Half(0.0004887581f);

		// Token: 0x04005306 RID: 21254
		public static readonly Half MaxValue = new Half(65504f);

		// Token: 0x04005307 RID: 21255
		public static readonly Half MinValue = new Half(6.103516E-05f);

		// Token: 0x04005308 RID: 21256
		public static readonly Half NaN = new Half(float.NaN);

		// Token: 0x04005309 RID: 21257
		public static readonly Half NegativeInfinity = new Half(float.NegativeInfinity);

		// Token: 0x0400530A RID: 21258
		public static readonly Half PositiveInfinity = new Half(float.PositiveInfinity);
	}
}
