using System;
using UnityEngine;

// Token: 0x020001E9 RID: 489
public static class ShaderPlatformSetter
{
	// Token: 0x06000B65 RID: 2917 RVA: 0x000371B2 File Offset: 0x000353B2
	[RuntimeInitializeOnLoadMethod]
	public static void HandleRuntimeInitializeOnLoad()
	{
		Shader.DisableKeyword("PLATFORM_IS_ANDROID");
		Shader.DisableKeyword("QATESTING");
	}
}
