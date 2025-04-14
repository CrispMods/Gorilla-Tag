using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal.Arcade
{
	// Token: 0x02000939 RID: 2361
	// (Invoke) Token: 0x06003907 RID: 14599
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void SessionCallback(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
}
