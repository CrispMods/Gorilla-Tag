using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000954 RID: 2388
	internal class Token
	{
		// Token: 0x060039C3 RID: 14787 RVA: 0x00055B1E File Offset: 0x00053D1E
		static Token()
		{
			Api.LoadLibraryManually("viveport_api");
		}

		// Token: 0x060039C4 RID: 14788
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportToken_IsReady")]
		internal static extern int IsReady(StatusCallback IsReadyCallback);

		// Token: 0x060039C5 RID: 14789
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportToken_IsReady")]
		internal static extern int IsReady_64(StatusCallback IsReadyCallback);

		// Token: 0x060039C6 RID: 14790
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportToken_GetSessionToken")]
		internal static extern int GetSessionToken(StatusCallback2 GetSessionTokenCallback);

		// Token: 0x060039C7 RID: 14791
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportToken_GetSessionToken")]
		internal static extern int GetSessionToken_64(StatusCallback2 GetSessionTokenCallback);
	}
}
