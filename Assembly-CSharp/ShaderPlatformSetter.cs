using System;
using UnityEngine;

// Token: 0x020001E9 RID: 489
public static class ShaderPlatformSetter
{
	// Token: 0x06000B63 RID: 2915 RVA: 0x0003D0C5 File Offset: 0x0003B2C5
	[RuntimeInitializeOnLoadMethod]
	public static void HandleRuntimeInitializeOnLoad()
	{
		Shader.DisableKeyword("PLATFORM_IS_ANDROID");
		Shader.DisableKeyword("QATESTING");
	}
}
