using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000926 RID: 2342
	// (Invoke) Token: 0x06003878 RID: 14456
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void StatusCallback2(int nResult, [MarshalAs(UnmanagedType.LPStr)] string message);
}
