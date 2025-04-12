using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B89 RID: 2953
	public static class GTAppState
	{
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06004AB4 RID: 19124 RVA: 0x000604A7 File Offset: 0x0005E6A7
		// (set) Token: 0x06004AB5 RID: 19125 RVA: 0x000604AE File Offset: 0x0005E6AE
		[OnEnterPlay_Set(false)]
		public static bool isQuitting { get; private set; }

		// Token: 0x06004AB6 RID: 19126 RVA: 0x0019C200 File Offset: 0x0019A400
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

		// Token: 0x06004AB7 RID: 19127 RVA: 0x0002F75F File Offset: 0x0002D95F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void HandleOnAfterSceneLoad()
		{
		}
	}
}
