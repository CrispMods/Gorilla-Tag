using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BD3 RID: 3027
	public static class GRef
	{
		// Token: 0x06004C58 RID: 19544 RVA: 0x00173B94 File Offset: 0x00171D94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ShouldResolveNow(GRef.EResolveModes mode)
		{
			return Application.isPlaying && (mode & GRef.EResolveModes.Runtime) == GRef.EResolveModes.Runtime;
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x00173BA5 File Offset: 0x00171DA5
		public static bool IsAnyResolveModeOn(GRef.EResolveModes mode)
		{
			return mode > GRef.EResolveModes.None;
		}

		// Token: 0x02000BD4 RID: 3028
		[Flags]
		public enum EResolveModes
		{
			// Token: 0x04004E43 RID: 20035
			None = 0,
			// Token: 0x04004E44 RID: 20036
			Runtime = 1,
			// Token: 0x04004E45 RID: 20037
			SceneProcessing = 2
		}
	}
}
