using System;

namespace UniLabs.Time
{
	// Token: 0x0200097E RID: 2430
	public static class TimeUnitExtensions
	{
		// Token: 0x06003B47 RID: 15175 RVA: 0x001504E8 File Offset: 0x0014E6E8
		public static string ToShortString(this TimeUnit timeUnit)
		{
			string result;
			switch (timeUnit)
			{
			case TimeUnit.None:
				result = "";
				break;
			case TimeUnit.Milliseconds:
				result = "ms";
				break;
			case TimeUnit.Seconds:
				result = "s";
				break;
			case TimeUnit.Minutes:
				result = "m";
				break;
			case TimeUnit.Hours:
				result = "h";
				break;
			case TimeUnit.Days:
				result = "D";
				break;
			default:
				throw new ArgumentOutOfRangeException("timeUnit", timeUnit, null);
			}
			return result;
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x00150558 File Offset: 0x0014E758
		public static string ToSeparatorString(this TimeUnit timeUnit)
		{
			string result;
			switch (timeUnit)
			{
			case TimeUnit.None:
				result = "";
				break;
			case TimeUnit.Milliseconds:
				result = "";
				break;
			case TimeUnit.Seconds:
				result = ".";
				break;
			case TimeUnit.Minutes:
				result = ":";
				break;
			case TimeUnit.Hours:
				result = ":";
				break;
			case TimeUnit.Days:
				result = ".";
				break;
			default:
				throw new ArgumentOutOfRangeException("timeUnit", timeUnit, null);
			}
			return result;
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x001505C8 File Offset: 0x0014E7C8
		public static double GetUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit)
		{
			int num;
			switch (timeUnit)
			{
			case TimeUnit.None:
				num = 0;
				break;
			case TimeUnit.Milliseconds:
				num = timeSpan.Milliseconds;
				break;
			case TimeUnit.Seconds:
				num = timeSpan.Seconds;
				break;
			case TimeUnit.Minutes:
				num = timeSpan.Minutes;
				break;
			case TimeUnit.Hours:
				num = timeSpan.Hours;
				break;
			case TimeUnit.Days:
				num = timeSpan.Days;
				break;
			default:
				throw new ArgumentOutOfRangeException("timeUnit", timeUnit, null);
			}
			return (double)num;
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x00150640 File Offset: 0x0014E840
		public static TimeSpan WithUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit, double value)
		{
			TimeSpan result;
			switch (timeUnit)
			{
			case TimeUnit.None:
				result = timeSpan;
				break;
			case TimeUnit.Milliseconds:
				result = timeSpan.Add(TimeSpan.FromMilliseconds(value - (double)timeSpan.Milliseconds));
				break;
			case TimeUnit.Seconds:
				result = timeSpan.Add(TimeSpan.FromSeconds(value - (double)timeSpan.Seconds));
				break;
			case TimeUnit.Minutes:
				result = timeSpan.Add(TimeSpan.FromMinutes(value - (double)timeSpan.Minutes));
				break;
			case TimeUnit.Hours:
				result = timeSpan.Add(TimeSpan.FromHours(value - (double)timeSpan.Hours));
				break;
			case TimeUnit.Days:
				result = timeSpan.Add(TimeSpan.FromDays(value - (double)timeSpan.Days));
				break;
			default:
				throw new ArgumentOutOfRangeException("timeUnit", timeUnit, null);
			}
			return result;
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x00150704 File Offset: 0x0014E904
		public static double GetLowestUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit)
		{
			double result;
			switch (timeUnit)
			{
			case TimeUnit.None:
				result = 0.0;
				break;
			case TimeUnit.Milliseconds:
				result = (double)timeSpan.Milliseconds;
				break;
			case TimeUnit.Seconds:
				result = new TimeSpan(0, 0, 0, timeSpan.Seconds, timeSpan.Milliseconds).TotalSeconds;
				break;
			case TimeUnit.Minutes:
				result = new TimeSpan(0, 0, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds).TotalMinutes;
				break;
			case TimeUnit.Hours:
				result = new TimeSpan(0, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds).TotalHours;
				break;
			case TimeUnit.Days:
				result = timeSpan.TotalDays;
				break;
			default:
				throw new ArgumentOutOfRangeException("timeUnit", timeUnit, null);
			}
			return result;
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x001507E0 File Offset: 0x0014E9E0
		public static TimeSpan WithLowestUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit, double value)
		{
			TimeSpan result;
			switch (timeUnit)
			{
			case TimeUnit.None:
				result = timeSpan;
				break;
			case TimeUnit.Milliseconds:
				result = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, (int)value);
				break;
			case TimeUnit.Seconds:
				result = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, 0).Add(TimeSpan.FromSeconds(value));
				break;
			case TimeUnit.Minutes:
				result = new TimeSpan(timeSpan.Days, timeSpan.Hours, 0, 0).Add(TimeSpan.FromMinutes(value));
				break;
			case TimeUnit.Hours:
				result = new TimeSpan(timeSpan.Days, 0, 0, 0).Add(TimeSpan.FromHours(value));
				break;
			case TimeUnit.Days:
				result = TimeSpan.FromDays(value);
				break;
			default:
				throw new ArgumentOutOfRangeException("timeUnit", timeUnit, null);
			}
			return result;
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x001508CC File Offset: 0x0014EACC
		public static double GetHighestUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit)
		{
			double result;
			switch (timeUnit)
			{
			case TimeUnit.None:
				result = 0.0;
				break;
			case TimeUnit.Milliseconds:
				result = timeSpan.TotalMilliseconds;
				break;
			case TimeUnit.Seconds:
				result = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds).TotalSeconds;
				break;
			case TimeUnit.Minutes:
				result = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, 0).TotalMinutes;
				break;
			case TimeUnit.Hours:
				result = new TimeSpan(timeSpan.Days, timeSpan.Hours, 0, 0).TotalHours;
				break;
			case TimeUnit.Days:
				result = (double)timeSpan.Days;
				break;
			default:
				throw new ArgumentOutOfRangeException("timeUnit", timeUnit, null);
			}
			return result;
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x001509A8 File Offset: 0x0014EBA8
		public static TimeSpan WithHighestUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit, double value)
		{
			TimeSpan result;
			switch (timeUnit)
			{
			case TimeUnit.None:
				result = timeSpan;
				break;
			case TimeUnit.Milliseconds:
				result = TimeSpan.FromMilliseconds(value);
				break;
			case TimeUnit.Seconds:
				result = new TimeSpan(0, 0, 0, 0, timeSpan.Milliseconds).Add(TimeSpan.FromSeconds(value));
				break;
			case TimeUnit.Minutes:
				result = new TimeSpan(0, 0, 0, timeSpan.Seconds, timeSpan.Milliseconds).Add(TimeSpan.FromMinutes(value));
				break;
			case TimeUnit.Hours:
				result = new TimeSpan(0, 0, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds).Add(TimeSpan.FromHours(value));
				break;
			case TimeUnit.Days:
				result = new TimeSpan(0, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds).Add(TimeSpan.FromDays(value));
				break;
			default:
				throw new ArgumentOutOfRangeException("timeUnit", timeUnit, null);
			}
			return result;
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x00150AA8 File Offset: 0x0014ECA8
		public static double GetSingleUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit)
		{
			double result;
			switch (timeUnit)
			{
			case TimeUnit.Milliseconds:
				result = timeSpan.TotalMilliseconds;
				break;
			case TimeUnit.Seconds:
				result = timeSpan.TotalSeconds;
				break;
			case TimeUnit.Minutes:
				result = timeSpan.TotalMinutes;
				break;
			case TimeUnit.Hours:
				result = timeSpan.TotalHours;
				break;
			case TimeUnit.Days:
				result = timeSpan.TotalDays;
				break;
			default:
				throw new ArgumentOutOfRangeException("timeUnit", timeUnit, null);
			}
			return result;
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x00150B18 File Offset: 0x0014ED18
		public static TimeSpan FromSingleUnitValue(this TimeSpan timeSpan, TimeUnit timeUnit, double value)
		{
			TimeSpan result;
			switch (timeUnit)
			{
			case TimeUnit.None:
				result = TimeSpan.Zero;
				break;
			case TimeUnit.Milliseconds:
				result = TimeSpan.FromMilliseconds(value);
				break;
			case TimeUnit.Seconds:
				result = TimeSpan.FromSeconds(value);
				break;
			case TimeUnit.Minutes:
				result = TimeSpan.FromMinutes(value);
				break;
			case TimeUnit.Hours:
				result = TimeSpan.FromHours(value);
				break;
			case TimeUnit.Days:
				result = TimeSpan.FromDays(value);
				break;
			default:
				throw new ArgumentOutOfRangeException("timeUnit", timeUnit, null);
			}
			return result;
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x00150B90 File Offset: 0x0014ED90
		public static TimeSpan SnapToUnit(this TimeSpan timeSpan, TimeUnit timeUnit)
		{
			TimeSpan result;
			switch (timeUnit)
			{
			case TimeUnit.None:
				result = timeSpan;
				break;
			case TimeUnit.Milliseconds:
				result = timeSpan;
				break;
			case TimeUnit.Seconds:
				result = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
				break;
			case TimeUnit.Minutes:
				result = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, 0);
				break;
			case TimeUnit.Hours:
				result = new TimeSpan(timeSpan.Days, timeSpan.Hours, 0, 0);
				break;
			case TimeUnit.Days:
				result = new TimeSpan(timeSpan.Days, 0, 0, 0);
				break;
			default:
				throw new ArgumentOutOfRangeException("timeUnit", timeUnit, null);
			}
			return result;
		}

		// Token: 0x0200097F RID: 2431
		// (Invoke) Token: 0x06003B53 RID: 15187
		public delegate TimeSpan WithUnitValueDelegate(TimeSpan timeSpan, TimeUnit timeUnit, double value);

		// Token: 0x02000980 RID: 2432
		// (Invoke) Token: 0x06003B57 RID: 15191
		public delegate double GetUnitValueDelegate(TimeSpan timeSpan, TimeUnit timeUnit);
	}
}
