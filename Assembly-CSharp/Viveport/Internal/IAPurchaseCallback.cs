using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x0200092E RID: 2350
	// (Invoke) Token: 0x06003874 RID: 14452
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void IAPurchaseCallback(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
}
