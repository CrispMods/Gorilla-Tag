using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Viveport.Internal
{
	// Token: 0x02000955 RID: 2389
	internal class Deeplink
	{
		// Token: 0x060039C9 RID: 14793 RVA: 0x00055B1E File Offset: 0x00053D1E
		static Deeplink()
		{
			Api.LoadLibraryManually("viveport_api");
		}

		// Token: 0x060039CA RID: 14794
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_IsReady")]
		internal static extern void IsReady(StatusCallback IsReadyCallback);

		// Token: 0x060039CB RID: 14795
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_IsReady")]
		internal static extern void IsReady_64(StatusCallback IsReadyCallback);

		// Token: 0x060039CC RID: 14796
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToApp")]
		internal static extern void GoToApp(StatusCallback2 GoToAppCallback, string ViveportId, string LaunchData);

		// Token: 0x060039CD RID: 14797
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToApp")]
		internal static extern void GoToApp_64(StatusCallback2 GoToAppCallback, string ViveportId, string LaunchData);

		// Token: 0x060039CE RID: 14798
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToAppWithBranchName")]
		internal static extern void GoToApp(StatusCallback2 GoToAppCallback, string ViveportId, string LaunchData, string branchName);

		// Token: 0x060039CF RID: 14799
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToAppWithBranchName")]
		internal static extern void GoToApp_64(StatusCallback2 GoToAppCallback, string ViveportId, string LaunchData, string branchName);

		// Token: 0x060039D0 RID: 14800
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToStore")]
		internal static extern void GoToStore(StatusCallback2 GetSessionTokenCallback, string ViveportId);

		// Token: 0x060039D1 RID: 14801
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToStore")]
		internal static extern void GoToStore_64(StatusCallback2 GetSessionTokenCallback, string ViveportId);

		// Token: 0x060039D2 RID: 14802
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToAppOrGoToStore")]
		internal static extern void GoToAppOrGoToStore(StatusCallback2 GoToAppCallback, string ViveportId, string LaunchData);

		// Token: 0x060039D3 RID: 14803
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GoToAppOrGoToStore")]
		internal static extern void GoToAppOrGoToStore_64(StatusCallback2 GoToAppCallback, string ViveportId, string LaunchData);

		// Token: 0x060039D4 RID: 14804
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GetAppLaunchData")]
		internal static extern int GetAppLaunchData(StringBuilder userId, int size);

		// Token: 0x060039D5 RID: 14805
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportDeeplink_GetAppLaunchData")]
		internal static extern int GetAppLaunchData_64(StringBuilder userId, int size);
	}
}
