using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Viveport.Internal
{
	// Token: 0x0200093B RID: 2363
	internal class Deeplink
	{
		// Token: 0x06003904 RID: 14596 RVA: 0x0005457C File Offset: 0x0005277C
		static Deeplink()
		{
			Api.LoadLibraryManually("viveport_api");
		}

		// Token: 0x06003905 RID: 14597
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_IsReady")]
		internal static extern void IsReady(StatusCallback IsReadyCallback);

		// Token: 0x06003906 RID: 14598
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_IsReady")]
		internal static extern void IsReady_64(StatusCallback IsReadyCallback);

		// Token: 0x06003907 RID: 14599
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToApp")]
		internal static extern void GoToApp(StatusCallback2 GoToAppCallback, string ViveportId, string LaunchData);

		// Token: 0x06003908 RID: 14600
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToApp")]
		internal static extern void GoToApp_64(StatusCallback2 GoToAppCallback, string ViveportId, string LaunchData);

		// Token: 0x06003909 RID: 14601
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToAppWithBranchName")]
		internal static extern void GoToApp(StatusCallback2 GoToAppCallback, string ViveportId, string LaunchData, string branchName);

		// Token: 0x0600390A RID: 14602
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToAppWithBranchName")]
		internal static extern void GoToApp_64(StatusCallback2 GoToAppCallback, string ViveportId, string LaunchData, string branchName);

		// Token: 0x0600390B RID: 14603
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToStore")]
		internal static extern void GoToStore(StatusCallback2 GetSessionTokenCallback, string ViveportId);

		// Token: 0x0600390C RID: 14604
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToStore")]
		internal static extern void GoToStore_64(StatusCallback2 GetSessionTokenCallback, string ViveportId);

		// Token: 0x0600390D RID: 14605
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToAppOrGoToStore")]
		internal static extern void GoToAppOrGoToStore(StatusCallback2 GoToAppCallback, string ViveportId, string LaunchData);

		// Token: 0x0600390E RID: 14606
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToAppOrGoToStore")]
		internal static extern void GoToAppOrGoToStore_64(StatusCallback2 GoToAppCallback, string ViveportId, string LaunchData);

		// Token: 0x0600390F RID: 14607
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GetAppLaunchData")]
		internal static extern int GetAppLaunchData(StringBuilder userId, int size);

		// Token: 0x06003910 RID: 14608
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GetAppLaunchData")]
		internal static extern int GetAppLaunchData_64(StringBuilder userId, int size);
	}
}
