using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal.Arcade
{
	// Token: 0x02000956 RID: 2390
	// (Invoke) Token: 0x060039D8 RID: 14808
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void SessionCallback(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
}
