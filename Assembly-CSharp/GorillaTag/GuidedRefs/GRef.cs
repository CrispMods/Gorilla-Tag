using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BFE RID: 3070
	public static class GRef
	{
		// Token: 0x06004D98 RID: 19864 RVA: 0x00062DC5 File Offset: 0x00060FC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ShouldResolveNow(GRef.EResolveModes mode)
		{
			return Application.isPlaying && (mode & GRef.EResolveModes.Runtime) == GRef.EResolveModes.Runtime;
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x00062DD6 File Offset: 0x00060FD6
		public static bool IsAnyResolveModeOn(GRef.EResolveModes mode)
		{
			return mode > GRef.EResolveModes.None;
		}

		// Token: 0x02000BFF RID: 3071
		[Flags]
		public enum EResolveModes
		{
			// Token: 0x04004F27 RID: 20263
			None = 0,
			// Token: 0x04004F28 RID: 20264
			Runtime = 1,
			// Token: 0x04004F29 RID: 20265
			SceneProcessing = 2
		}
	}
}
