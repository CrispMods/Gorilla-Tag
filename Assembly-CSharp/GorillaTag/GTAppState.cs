using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BB3 RID: 2995
	public static class GTAppState
	{
		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06004BF3 RID: 19443 RVA: 0x00061EDF File Offset: 0x000600DF
		// (set) Token: 0x06004BF4 RID: 19444 RVA: 0x00061EE6 File Offset: 0x000600E6
		[OnEnterPlay_Set(false)]
		public static bool isQuitting { get; private set; }

		// Token: 0x06004BF5 RID: 19445 RVA: 0x001A3218 File Offset: 0x001A1418
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

		// Token: 0x06004BF6 RID: 19446 RVA: 0x00030607 File Offset: 0x0002E807
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void HandleOnAfterSceneLoad()
		{
		}
	}
}
