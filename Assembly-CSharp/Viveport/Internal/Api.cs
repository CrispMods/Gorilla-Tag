using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x0200094E RID: 2382
	internal class Api
	{
		// Token: 0x06003958 RID: 14680 RVA: 0x00055B1E File Offset: 0x00053D1E
		static Api()
		{
			Api.LoadLibraryManually("viveport_api");
		}

		// Token: 0x06003959 RID: 14681
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_GetLicense")]
		internal static extern void GetLicense(GetLicenseCallback callback, string appId, string appKey);

		// Token: 0x0600395A RID: 14682
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_GetLicense")]
		internal static extern void GetLicense_64(GetLicenseCallback callback, string appId, string appKey);

		// Token: 0x0600395B RID: 14683
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_Init")]
		internal static extern int Init(StatusCallback initCallback, string appId);

		// Token: 0x0600395C RID: 14684
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_Init")]
		internal static extern int Init_64(StatusCallback initCallback, string appId);

		// Token: 0x0600395D RID: 14685
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_Shutdown")]
		internal static extern int Shutdown(StatusCallback initCallback);

		// Token: 0x0600395E RID: 14686
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_Shutdown")]
		internal static extern int Shutdown_64(StatusCallback initCallback);

		// Token: 0x0600395F RID: 14687
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_Version")]
		internal static extern IntPtr Version();

		// Token: 0x06003960 RID: 14688
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_Version")]
		internal static extern IntPtr Version_64();

		// Token: 0x06003961 RID: 14689
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_QueryRuntimeMode")]
		internal static extern void QueryRuntimeMode(QueryRuntimeModeCallback queryRunTimeCallback);

		// Token: 0x06003962 RID: 14690
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_QueryRuntimeMode")]
		internal static extern void QueryRuntimeMode_64(QueryRuntimeModeCallback queryRunTimeCallback);

		// Token: 0x06003963 RID: 14691
		[DllImport("kernel32.dll")]
		internal static extern IntPtr LoadLibrary(string dllToLoad);

		// Token: 0x06003964 RID: 14692 RVA: 0x0014C434 File Offset: 0x0014A634
		internal static void LoadLibraryManually(string dllName)
		{
			if (string.IsNullOrEmpty(dllName))
			{
				return;
			}
			if (IntPtr.Size == 8)
			{
				Api.LoadLibrary("x64/" + dllName + "64.dll");
				return;
			}
			Api.LoadLibrary("x86/" + dllName + ".dll");
		}
	}
}
