using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000921 RID: 2337
	// (Invoke) Token: 0x06003864 RID: 14436
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void GetLicenseCallback([MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string signature);
}
