using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000937 RID: 2359
	internal class Token
	{
		// Token: 0x060038F2 RID: 14578 RVA: 0x00108133 File Offset: 0x00106333
		static Token()
		{
			Api.LoadLibraryManually("viveport_api");
		}

		// Token: 0x060038F3 RID: 14579
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportToken_IsReady")]
		internal static extern int IsReady(StatusCallback IsReadyCallback);

		// Token: 0x060038F4 RID: 14580
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportToken_IsReady")]
		internal static extern int IsReady_64(StatusCallback IsReadyCallback);

		// Token: 0x060038F5 RID: 14581
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportToken_GetSessionToken")]
		internal static extern int GetSessionToken(StatusCallback2 GetSessionTokenCallback);

		// Token: 0x060038F6 RID: 14582
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportToken_GetSessionToken")]
		internal static extern int GetSessionToken_64(StatusCallback2 GetSessionTokenCallback);
	}
}
