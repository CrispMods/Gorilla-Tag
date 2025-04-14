using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000925 RID: 2341
	// (Invoke) Token: 0x06003874 RID: 14452
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void StatusCallback(int nResult);
}
