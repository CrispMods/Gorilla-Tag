using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000927 RID: 2343
	// (Invoke) Token: 0x0600387C RID: 14460
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void QueryRuntimeModeCallback(int nResult, int nMode);
}
