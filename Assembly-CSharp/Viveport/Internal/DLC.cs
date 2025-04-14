using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Viveport.Internal
{
	// Token: 0x02000935 RID: 2357
	internal class DLC
	{
		// Token: 0x060038DE RID: 14558 RVA: 0x00108133 File Offset: 0x00106333
		static DLC()
		{
			Api.LoadLibraryManually("viveport_api");
		}

		// Token: 0x060038DF RID: 14559
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_IsReady")]
		internal static extern int IsReady(StatusCallback callback);

		// Token: 0x060038E0 RID: 14560
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_IsReady")]
		internal static extern int IsReady_64(StatusCallback callback);

		// Token: 0x060038E1 RID: 14561
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_GetCount")]
		internal static extern int GetCount();

		// Token: 0x060038E2 RID: 14562
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_GetCount")]
		internal static extern int GetCount_64();

		// Token: 0x060038E3 RID: 14563
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_GetIsAvailable")]
		internal static extern bool GetIsAvailable(int index, StringBuilder appId, out bool isAvailable);

		// Token: 0x060038E4 RID: 14564
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_GetIsAvailable")]
		internal static extern bool GetIsAvailable_64(int index, StringBuilder appId, out bool isAvailable);

		// Token: 0x060038E5 RID: 14565
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_IsSubscribed")]
		internal static extern int IsSubscribed();

		// Token: 0x060038E6 RID: 14566
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_IsSubscribed")]
		internal static extern int IsSubscribed_64();
	}
}
