using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B84 RID: 2948
	public class GTTime
	{
		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06004AA3 RID: 19107 RVA: 0x00169D08 File Offset: 0x00167F08
		// (set) Token: 0x06004AA4 RID: 19108 RVA: 0x00169D0F File Offset: 0x00167F0F
		public static bool usingServerTime { get; private set; }

		// Token: 0x06004AA5 RID: 19109 RVA: 0x00169D17 File Offset: 0x00167F17
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long GetServerStartupTimeAsMilliseconds()
		{
			return GorillaComputer.instance.startupMillis;
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x00169D28 File Offset: 0x00167F28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long GetDeviceStartupTimeAsMilliseconds()
		{
			return (long)(TimeSpan.FromTicks(DateTime.UtcNow.Ticks).TotalMilliseconds - Time.realtimeSinceStartupAsDouble * 1000.0);
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x00169D60 File Offset: 0x00167F60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long GetStartupTimeAsMilliseconds()
		{
			GTTime.usingServerTime = true;
			long num = 0L;
			if (GorillaComputer.instance != null)
			{
				num = GTTime.GetServerStartupTimeAsMilliseconds();
			}
			if (num == 0L)
			{
				GTTime.usingServerTime = false;
				num = GTTime.GetDeviceStartupTimeAsMilliseconds();
			}
			return num;
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x00169D9B File Offset: 0x00167F9B
		public static long TimeAsMilliseconds()
		{
			return GTTime.GetStartupTimeAsMilliseconds() + (long)(Time.realtimeSinceStartupAsDouble * 1000.0);
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x00169DB3 File Offset: 0x00167FB3
		public static double TimeAsDouble()
		{
			return (double)GTTime.GetStartupTimeAsMilliseconds() / 1000.0 + Time.realtimeSinceStartupAsDouble;
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x00169DCC File Offset: 0x00167FCC
		public static DateTime GetAAxiomDateTime()
		{
			DateTime result;
			try
			{
				TimeZoneInfo destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
				result = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, destinationTimeZone);
			}
			catch
			{
				try
				{
					TimeZoneInfo destinationTimeZone2 = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
					result = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, destinationTimeZone2);
				}
				catch
				{
					result = DateTime.UtcNow;
				}
			}
			return result;
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x00169E34 File Offset: 0x00168034
		public static string GetAAxiomDateTimeAsStringForDisplay()
		{
			return GTTime.GetAAxiomDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x00169E54 File Offset: 0x00168054
		public static string GetAAxiomDateTimeAsStringForFilename()
		{
			return GTTime.GetAAxiomDateTime().ToString("yyyy-MM-dd_HH-mm-ss-fff");
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x00169E74 File Offset: 0x00168074
		public static long GetAAxiomDateTimeAsHumanReadableLong()
		{
			return long.Parse(GTTime.GetAAxiomDateTime().ToString("yyyyMMddHHmmssfff00"));
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x00169E98 File Offset: 0x00168098
		public static DateTime ConvertDateTimeHumanReadableLongToDateTime(long humanReadableLong)
		{
			return DateTime.ParseExact(humanReadableLong.ToString(), "yyyyMMddHHmmssfff'00'", CultureInfo.InvariantCulture);
		}
	}
}
