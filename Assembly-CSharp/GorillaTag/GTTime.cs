using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B81 RID: 2945
	public class GTTime
	{
		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06004A97 RID: 19095 RVA: 0x00169740 File Offset: 0x00167940
		// (set) Token: 0x06004A98 RID: 19096 RVA: 0x00169747 File Offset: 0x00167947
		public static bool usingServerTime { get; private set; }

		// Token: 0x06004A99 RID: 19097 RVA: 0x0016974F File Offset: 0x0016794F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long GetServerStartupTimeAsMilliseconds()
		{
			return GorillaComputer.instance.startupMillis;
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x00169760 File Offset: 0x00167960
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long GetDeviceStartupTimeAsMilliseconds()
		{
			return (long)(TimeSpan.FromTicks(DateTime.UtcNow.Ticks).TotalMilliseconds - Time.realtimeSinceStartupAsDouble * 1000.0);
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x00169798 File Offset: 0x00167998
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

		// Token: 0x06004A9C RID: 19100 RVA: 0x001697D3 File Offset: 0x001679D3
		public static long TimeAsMilliseconds()
		{
			return GTTime.GetStartupTimeAsMilliseconds() + (long)(Time.realtimeSinceStartupAsDouble * 1000.0);
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x001697EB File Offset: 0x001679EB
		public static double TimeAsDouble()
		{
			return (double)GTTime.GetStartupTimeAsMilliseconds() / 1000.0 + Time.realtimeSinceStartupAsDouble;
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x00169804 File Offset: 0x00167A04
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

		// Token: 0x06004A9F RID: 19103 RVA: 0x0016986C File Offset: 0x00167A6C
		public static string GetAAxiomDateTimeAsStringForDisplay()
		{
			return GTTime.GetAAxiomDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x0016988C File Offset: 0x00167A8C
		public static string GetAAxiomDateTimeAsStringForFilename()
		{
			return GTTime.GetAAxiomDateTime().ToString("yyyy-MM-dd_HH-mm-ss-fff");
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x001698AC File Offset: 0x00167AAC
		public static long GetAAxiomDateTimeAsHumanReadableLong()
		{
			return long.Parse(GTTime.GetAAxiomDateTime().ToString("yyyyMMddHHmmssfff00"));
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x001698D0 File Offset: 0x00167AD0
		public static DateTime ConvertDateTimeHumanReadableLongToDateTime(long humanReadableLong)
		{
			return DateTime.ParseExact(humanReadableLong.ToString(), "yyyyMMddHHmmssfff'00'", CultureInfo.InvariantCulture);
		}
	}
}
