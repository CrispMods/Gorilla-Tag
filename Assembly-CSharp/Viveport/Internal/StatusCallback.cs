using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x0200093F RID: 2367
	// (Invoke) Token: 0x06003939 RID: 14649
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void StatusCallback(int nResult);
}
