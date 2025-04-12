using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Viveport.Internal
{
	// Token: 0x02000938 RID: 2360
	internal class DLC
	{
		// Token: 0x060038EA RID: 14570 RVA: 0x0005457C File Offset: 0x0005277C
		static DLC()
		{
			Api.LoadLibraryManually("viveport_api");
		}

		// Token: 0x060038EB RID: 14571
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_IsReady")]
		internal static extern int IsReady(StatusCallback callback);

		// Token: 0x060038EC RID: 14572
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_IsReady")]
		internal static extern int IsReady_64(StatusCallback callback);

		// Token: 0x060038ED RID: 14573
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_GetCount")]
		internal static extern int GetCount();

		// Token: 0x060038EE RID: 14574
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_GetCount")]
		internal static extern int GetCount_64();

		// Token: 0x060038EF RID: 14575
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_GetIsAvailable")]
		internal static extern bool GetIsAvailable(int index, StringBuilder appId, out bool isAvailable);

		// Token: 0x060038F0 RID: 14576
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_GetIsAvailable")]
		internal static extern bool GetIsAvailable_64(int index, StringBuilder appId, out bool isAvailable);

		// Token: 0x060038F1 RID: 14577
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_IsSubscribed")]
		internal static extern int IsSubscribed();

		// Token: 0x060038F2 RID: 14578
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDlc_IsSubscribed")]
		internal static extern int IsSubscribed_64();
	}
}
