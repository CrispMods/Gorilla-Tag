using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B89 RID: 2953
	public static class GTAppState
	{
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06004AB4 RID: 19124 RVA: 0x00169EB0 File Offset: 0x001680B0
		// (set) Token: 0x06004AB5 RID: 19125 RVA: 0x00169EB7 File Offset: 0x001680B7
		[OnEnterPlay_Set(false)]
		public static bool isQuitting { get; private set; }

		// Token: 0x06004AB6 RID: 19126 RVA: 0x00169EC0 File Offset: 0x001680C0
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

		// Token: 0x06004AB7 RID: 19127 RVA: 0x000023F4 File Offset: 0x000005F4
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void HandleOnAfterSceneLoad()
		{
		}
	}
}
