using System;
using UnityEngine;

// Token: 0x02000286 RID: 646
[Serializable]
public struct NetworkSystemConfig
{
	// Token: 0x17000197 RID: 407
	// (get) Token: 0x06000F34 RID: 3892 RVA: 0x0004C1FD File Offset: 0x0004A3FD
	public static string AppVersion
	{
		get
		{
			return NetworkSystemConfig.prependCode + "." + NetworkSystemConfig.AppVersionStripped;
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x06000F35 RID: 3893 RVA: 0x0004C214 File Offset: 0x0004A414
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
	// (get) Token: 0x06000F36 RID: 3894 RVA: 0x0004C274 File Offset: 0x0004A474
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
	// (get) Token: 0x06000F37 RID: 3895 RVA: 0x0004C2C3 File Offset: 0x0004A4C3
	public static string GameVersionType
	{
		get
		{
			return NetworkSystemConfig.gameVersionType;
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0004C2CA File Offset: 0x0004A4CA
	public static int GameMajorVersion
	{
		get
		{
			return NetworkSystemConfig.majorVersion;
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06000F39 RID: 3897 RVA: 0x0004C2D1 File Offset: 0x0004A4D1
	public static int GameMinorVersion
	{
		get
		{
			return NetworkSystemConfig.minorVersion;
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06000F3A RID: 3898 RVA: 0x0004C2D8 File Offset: 0x0004A4D8
	public static int GameMinorVersion2
	{
		get
		{
			return NetworkSystemConfig.minorVersion2;
		}
	}

	// Token: 0x040011CC RID: 4556
	[HideInInspector]
	public int MaxPlayerCount;

	// Token: 0x040011CD RID: 4557
	private static string gameVersionType = "live1";

	// Token: 0x040011CE RID: 4558
	public static string prependCode = "CaptureEntirelyPrepend";

	// Token: 0x040011CF RID: 4559
	public static int majorVersion = 1;

	// Token: 0x040011D0 RID: 4560
	public static int minorVersion = 1;

	// Token: 0x040011D1 RID: 4561
	public static int minorVersion2 = 103;
}
