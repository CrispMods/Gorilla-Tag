using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BAE RID: 2990
	public class GTTime
	{
		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06004BE2 RID: 19426 RVA: 0x00061E7A File Offset: 0x0006007A
		// (set) Token: 0x06004BE3 RID: 19427 RVA: 0x00061E81 File Offset: 0x00060081
		public static bool usingServerTime { get; private set; }

		// Token: 0x06004BE4 RID: 19428 RVA: 0x00061E89 File Offset: 0x00060089
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long GetServerStartupTimeAsMilliseconds()
		{
			return GorillaComputer.instance.startupMillis;
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x001A30D8 File Offset: 0x001A12D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long GetDeviceStartupTimeAsMilliseconds()
		{
			return (long)(TimeSpan.FromTicks(DateTime.UtcNow.Ticks).TotalMilliseconds - Time.realtimeSinceStartupAsDouble * 1000.0);
		}

		// Token: 0x06004BE6 RID: 19430 RVA: 0x001A3110 File Offset: 0x001A1310
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

		// Token: 0x06004BE7 RID: 19431 RVA: 0x00061E97 File Offset: 0x00060097
		public static long TimeAsMilliseconds()
		{
			return GTTime.GetStartupTimeAsMilliseconds() + (long)(Time.realtimeSinceStartupAsDouble * 1000.0);
		}

		// Token: 0x06004BE8 RID: 19432 RVA: 0x00061EAF File Offset: 0x000600AF
		public static double TimeAsDouble()
		{
			return (double)GTTime.GetStartupTimeAsMilliseconds() / 1000.0 + Time.realtimeSinceStartupAsDouble;
		}

		// Token: 0x06004BE9 RID: 19433 RVA: 0x001A314C File Offset: 0x001A134C
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

		// Token: 0x06004BEA RID: 19434 RVA: 0x001A31B4 File Offset: 0x001A13B4
		public static string GetAAxiomDateTimeAsStringForDisplay()
		{
			return GTTime.GetAAxiomDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x001A31D4 File Offset: 0x001A13D4
		public static string GetAAxiomDateTimeAsStringForFilename()
		{
			return GTTime.GetAAxiomDateTime().ToString("yyyy-MM-dd_HH-mm-ss-fff");
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x001A31F4 File Offset: 0x001A13F4
		public static long GetAAxiomDateTimeAsHumanReadableLong()
		{
			return long.Parse(GTTime.GetAAxiomDateTime().ToString("yyyyMMddHHmmssfff00"));
		}

		// Token: 0x06004BED RID: 19437 RVA: 0x00061EC7 File Offset: 0x000600C7
		public static DateTime ConvertDateTimeHumanReadableLongToDateTime(long humanReadableLong)
		{
			return DateTime.ParseExact(humanReadableLong.ToString(), "yyyyMMddHHmmssfff'00'", CultureInfo.InvariantCulture);
		}
	}
}
