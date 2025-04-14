using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000931 RID: 2353
	// (Invoke) Token: 0x06003880 RID: 14464
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void IAPurchaseCallback(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
}
