using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000923 RID: 2339
	// (Invoke) Token: 0x0600386C RID: 14444
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void StatusCallback2(int nResult, [MarshalAs(UnmanagedType.LPStr)] string message);
}
