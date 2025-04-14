using System;
using UnityEngine;

// Token: 0x02000286 RID: 646
[Serializable]
public struct NetworkSystemConfig
{
	// Token: 0x17000197 RID: 407
	// (get) Token: 0x06000F36 RID: 3894 RVA: 0x0004C541 File Offset: 0x0004A741
	public static string AppVersion
	{
		get
		{
			return NetworkSystemConfig.prependCode + "." + NetworkSystemConfig.AppVersionStripped;
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x06000F37 RID: 3895 RVA: 0x0004C558 File Offset: 0x0004A758
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

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0004C5B8 File Offset: 0x0004A7B8
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

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06000F39 RID: 3897 RVA: 0x0004C607 File Offset: 0x0004A807
	public static string GameVersionType
	{
		get
		{
			return NetworkSystemConfig.gameVersionType;
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000F3A RID: 3898 RVA: 0x0004C60E File Offset: 0x0004A80E
	public static int GameMajorVersion
	{
		get
		{
			return NetworkSystemConfig.majorVersion;
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06000F3B RID: 3899 RVA: 0x0004C615 File Offset: 0x0004A815
	public static int GameMinorVersion
	{
		get
		{
			return NetworkSystemConfig.minorVersion;
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06000F3C RID: 3900 RVA: 0x0004C61C File Offset: 0x0004A81C
	public static int GameMinorVersion2
	{
		get
		{
			return NetworkSystemConfig.minorVersion2;
		}
	}

	// Token: 0x040011CD RID: 4557
	[HideInInspector]
	public int MaxPlayerCount;

	// Token: 0x040011CE RID: 4558
	private static string gameVersionType = "live1";

	// Token: 0x040011CF RID: 4559
	public static string prependCode = "1Capture2HotfixPrepend34";

	// Token: 0x040011D0 RID: 4560
	public static int majorVersion = 1;

	// Token: 0x040011D1 RID: 4561
	public static int minorVersion = 1;

	// Token: 0x040011D2 RID: 4562
	public static int minorVersion2 = 105;
}
