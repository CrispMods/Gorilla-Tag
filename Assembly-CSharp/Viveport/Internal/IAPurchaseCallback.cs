using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x0200094B RID: 2379
	// (Invoke) Token: 0x06003945 RID: 14661
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void IAPurchaseCallback(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
}
