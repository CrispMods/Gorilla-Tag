using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000940 RID: 2368
	// (Invoke) Token: 0x0600393D RID: 14653
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void StatusCallback2(int nResult, [MarshalAs(UnmanagedType.LPStr)] string message);
}
