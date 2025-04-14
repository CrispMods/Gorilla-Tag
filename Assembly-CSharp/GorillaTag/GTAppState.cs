using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B86 RID: 2950
	public static class GTAppState
	{
		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06004AA8 RID: 19112 RVA: 0x001698E8 File Offset: 0x00167AE8
		// (set) Token: 0x06004AA9 RID: 19113 RVA: 0x001698EF File Offset: 0x00167AEF
		[OnEnterPlay_Set(false)]
		public static bool isQuitting { get; private set; }

		// Token: 0x06004AAA RID: 19114 RVA: 0x001698F8 File Offset: 0x00167AF8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void HandleOnSubsystemRegistration()
		{
			GTAppState.isQuitting = false;
			Application.quitting += delegate()
			{
				GTAppState.isQuitting = true;
			};
			Debug.Log(string.Concat(new string[]
			{
				"GTAppState:\n- SystemInfo.operatingSystem=",
				SystemInfo.operatingSystem,
				"\n- SystemInfo.maxTextureArraySlices=",
				SystemInfo.maxTextureArraySlices.ToString(),
				"\n"
			}));
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x000023F4 File Offset: 0x000005F4
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void HandleOnAfterSceneLoad()
		{
		}
	}
}
