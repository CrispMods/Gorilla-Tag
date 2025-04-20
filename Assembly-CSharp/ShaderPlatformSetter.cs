using System;
using UnityEngine;

// Token: 0x020001F4 RID: 500
public static class ShaderPlatformSetter
{
	// Token: 0x06000BAF RID: 2991 RVA: 0x00038472 File Offset: 0x00036672
	[RuntimeInitializeOnLoadMethod]
	public static void HandleRuntimeInitializeOnLoad()
	{
		Shader.DisableKeyword("PLATFORM_IS_ANDROID");
		Shader.DisableKeyword("QATESTING");
	}
}
