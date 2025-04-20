using System;
using System.Globalization;

namespace emotitron.Compression.HalfFloat
{
	// Token: 0x02000CA5 RID: 3237
	[Serializable]
	public struct Half : IConvertible, IComparable, IComparable<Half>, IEquatable<Half>, IFormattable
	{
		// Token: 0x060051C5 RID: 20933 RVA: 0x00064FF6 File Offset: 0x000631F6
		public Half(float value)
		{
			this.value = HalfUtilities.Pack(value);
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x060051C6 RID: 20934 RVA: 0x00065004 File Offset: 0x00063204
		public ushort RawValue
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x001BE570 File Offset: 0x001BC770
		public static float[] ConvertToFloat(Half[] values)
		{
			float[] array = new float[values.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = HalfUtilities.Unpack(values[i].RawValue);
			}
			return array;
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x001BE5AC File Offset: 0x001BC7AC
		public static Half[] ConvertToHalf(float[] values)
		{
			Half[] array = new Half[values.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Half(values[i]);
			}
			return array;
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x0006500C File Offset: 0x0006320C
		public static bool IsInfinity(Half half)
		{
			return half == Half.PositiveInfinity || half == Half.NegativeInfinity;
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x00065028 File Offset: 0x00063228
		public static bool IsNaN(Half half)
		{
			return half == Half.NaN;
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x00065035 File Offset: 0x00063235
		public static bool IsNegativeInfinity(Half half)
		{
			return half == Half.NegativeInfinity;
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x00065042 File Offset: 0x00063242
		public static bool IsPositiveInfinity(Half half)
		{
			return half == Half.PositiveInfinity;
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x0006504F File Offset: 0x0006324F
		public static bool operator <(Half left, Half right)
		{
			return left < right;
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x00065061 File Offset: 0x00063261
		public static bool operator >(Half left, Half right)
		{
			return left > right;
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x00065073 File Offset: 0x00063273
		public static bool operator <=(Half left, Half right)
		{
			return left <= right;
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x00065088 File Offset: 0x00063288
		public static bool operator >=(Half left, Half right)
		{
			return left >= right;
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x0006509D File Offset: 0x0006329D
		public static bool operator ==(Half left, Half right)
		{
			return left.Equals(right);
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x000650A7 File Offset: 0x000632A7
		public static bool operator !=(Half left, Half right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x000650B4 File Offset: 0x000632B4
		public static explicit operator Half(float value)
		{
			return new Half(value);
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x000650BC File Offset: 0x000632BC
		public static implicit operator float(Half value)
		{
			return HalfUtilities.Unpack(value.value);
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x001BE5E0 File Offset: 0x001BC7E0
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, this.ToString(), Array.Empty<object>());
		}

		// Token: 0x060051D6 RID: 20950 RVA: 0x001BE610 File Offset: 0x001BC810
		public string ToString(string format)
		{
			if (format == null)
			{
				return this.ToString();
			}
			return string.Format(CultureInfo.CurrentCulture, this.ToString(format, CultureInfo.CurrentCulture), Array.Empty<object>());
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x001BE658 File Offset: 0x001BC858
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, this.ToString(), Array.Empty<object>());
		}

		// Token: 0x060051D8 RID: 20952 RVA: 0x001BE684 File Offset: 0x001BC884
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (format == null)
			{
				this.ToString(formatProvider);
			}
			return string.Format(formatProvider, this.ToString(format, formatProvider), Array.Empty<object>());
		}

		// Token: 0x060051D9 RID: 20953 RVA: 0x000650C9 File Offset: 0x000632C9
		public override int GetHashCode()
		{
			return (int)(this.value * 3 / 2 ^ this.value);
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x001BE6C0 File Offset: 0x001BC8C0
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

		// Token: 0x060051DB RID: 20955 RVA: 0x001BE718 File Offset: 0x001BC918
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

		// Token: 0x060051DC RID: 20956 RVA: 0x000650DC File Offset: 0x000632DC
		public static bool Equals(ref Half value1, ref Half value2)
		{
			return value1.value == value2.value;
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x000650EC File Offset: 0x000632EC
		public bool Equals(Half other)
		{
			return other.value == this.value;
		}

		// Token: 0x060051DE RID: 20958 RVA: 0x000650FC File Offset: 0x000632FC
		public override bool Equals(object obj)
		{
			return obj != null && !(obj.GetType() != base.GetType()) && this.Equals((Half)obj);
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x0006512E File Offset: 0x0006332E
		public TypeCode GetTypeCode()
		{
			return Type.GetTypeCode(typeof(Half));
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x0006513F File Offset: 0x0006333F
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x00065151 File Offset: 0x00063351
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x00065163 File Offset: 0x00063363
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException("Invalid cast from SlimMath.Half to System.Char.");
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x0006516F File Offset: 0x0006336F
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException("Invalid cast from SlimMath.Half to System.DateTime.");
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x0006517B File Offset: 0x0006337B
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x0006518D File Offset: 0x0006338D
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x0006519F File Offset: 0x0006339F
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x000651B1 File Offset: 0x000633B1
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x000651C3 File Offset: 0x000633C3
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x000651D5 File Offset: 0x000633D5
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x000651E7 File Offset: 0x000633E7
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x000651F4 File Offset: 0x000633F4
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return ((IConvertible)this).ToType(type, provider);
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x0006520E File Offset: 0x0006340E
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x00065220 File Offset: 0x00063420
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x00065232 File Offset: 0x00063432
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x04005408 RID: 21512
		private ushort value;

		// Token: 0x04005409 RID: 21513
		public const int PrecisionDigits = 3;

		// Token: 0x0400540A RID: 21514
		public const int MantissaBits = 11;

		// Token: 0x0400540B RID: 21515
		public const int MaximumDecimalExponent = 4;

		// Token: 0x0400540C RID: 21516
		public const int MaximumBinaryExponent = 15;

		// Token: 0x0400540D RID: 21517
		public const int MinimumDecimalExponent = -4;

		// Token: 0x0400540E RID: 21518
		public const int MinimumBinaryExponent = -14;

		// Token: 0x0400540F RID: 21519
		public const int ExponentRadix = 2;

		// Token: 0x04005410 RID: 21520
		public const int AdditionRounding = 1;

		// Token: 0x04005411 RID: 21521
		public static readonly Half Epsilon = new Half(0.0004887581f);

		// Token: 0x04005412 RID: 21522
		public static readonly Half MaxValue = new Half(65504f);

		// Token: 0x04005413 RID: 21523
		public static readonly Half MinValue = new Half(6.103516E-05f);

		// Token: 0x04005414 RID: 21524
		public static readonly Half NaN = new Half(float.NaN);

		// Token: 0x04005415 RID: 21525
		public static readonly Half NegativeInfinity = new Half(float.NegativeInfinity);

		// Token: 0x04005416 RID: 21526
		public static readonly Half PositiveInfinity = new Half(float.PositiveInfinity);
	}
}
