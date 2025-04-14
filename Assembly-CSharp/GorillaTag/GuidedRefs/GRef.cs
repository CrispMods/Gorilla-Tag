using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BD0 RID: 3024
	public static class GRef
	{
		// Token: 0x06004C4C RID: 19532 RVA: 0x001735CC File Offset: 0x001717CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ShouldResolveNow(GRef.EResolveModes mode)
		{
			return Application.isPlaying && (mode & GRef.EResolveModes.Runtime) == GRef.EResolveModes.Runtime;
		}

		// Token: 0x06004C4D RID: 19533 RVA: 0x001735DD File Offset: 0x001717DD
		public static bool IsAnyResolveModeOn(GRef.EResolveModes mode)
		{
			return mode > GRef.EResolveModes.None;
		}

		// Token: 0x02000BD1 RID: 3025
		[Flags]
		public enum EResolveModes
		{
			// Token: 0x04004E31 RID: 20017
			None = 0,
			// Token: 0x04004E32 RID: 20018
			Runtime = 1,
			// Token: 0x04004E33 RID: 20019
			SceneProcessing = 2
		}
	}
}
