using System;
using UnityEngine;

// Token: 0x02000291 RID: 657
[Serializable]
public struct NetworkSystemConfig
{
	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06000F7F RID: 3967 RVA: 0x0003ADC3 File Offset: 0x00038FC3
	public static string AppVersion
	{
		get
		{
			return NetworkSystemConfig.prependCode + "." + NetworkSystemConfig.AppVersionStripped;
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06000F80 RID: 3968 RVA: 0x000A858C File Offset: 0x000A678C
	public static string AppVersionStripped
	{
		get
		{
			return string.Concat(new string[]
			{
				NetworkSystemConfig.gameVersionType,
				".",
				NetworkSystemConfig.majorVersion.ToString(),
				".",
				NetworkSystemConfig.minorVersion.ToString(),
				".",
				NetworkSystemConfig.minorVersion2.ToString()
			});
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06000F81 RID: 3969 RVA: 0x000A85EC File Offset: 0x000A67EC
	public static string BundleVersion
	{
		get
		{
			return string.Concat(new string[]
			{
				NetworkSystemConfig.majorVersion.ToString(),
				".",
				NetworkSystemConfig.minorVersion.ToString(),
				".",
				NetworkSystemConfig.minorVersion2.ToString()
			});
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06000F82 RID: 3970 RVA: 0x0003ADD9 File Offset: 0x00038FD9
	public static string GameVersionType
	{
		get
		{
			return NetworkSystemConfig.gameVersionType;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06000F83 RID: 3971 RVA: 0x0003ADE0 File Offset: 0x00038FE0
	public static int GameMajorVersion
	{
		get
		{
			return NetworkSystemConfig.majorVersion;
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06000F84 RID: 3972 RVA: 0x0003ADE7 File Offset: 0x00038FE7
	public static int GameMinorVersion
	{
		get
		{
			return NetworkSystemConfig.minorVersion;
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000F85 RID: 3973 RVA: 0x0003ADEE File Offset: 0x00038FEE
	public static int GameMinorVersion2
	{
		get
		{
			return NetworkSystemConfig.minorVersion2;
		}
	}

	// Token: 0x04001214 RID: 4628
	[HideInInspector]
	public int MaxPlayerCount;

	// Token: 0x04001215 RID: 4629
	private static string gameVersionType = "live1";

	// Token: 0x04001216 RID: 4630
	public static string prependCode = "3ggCapturePrepend14";

	// Token: 0x04001217 RID: 4631
	public static int majorVersion = 1;

	// Token: 0x04001218 RID: 4632
	public static int minorVersion = 1;

	// Token: 0x04001219 RID: 4633
	public static int minorVersion2 = 106;
}
