using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x0200093E RID: 2366
	// (Invoke) Token: 0x06003935 RID: 14645
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void GetLicenseCallback([MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string signature);
}
