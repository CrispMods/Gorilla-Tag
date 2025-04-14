using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000924 RID: 2340
	// (Invoke) Token: 0x06003870 RID: 14448
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void GetLicenseCallback([MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string signature);
}
