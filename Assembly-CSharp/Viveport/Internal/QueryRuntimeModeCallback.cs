using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000941 RID: 2369
	// (Invoke) Token: 0x06003941 RID: 14657
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void QueryRuntimeModeCallback(int nResult, int nMode);
}
