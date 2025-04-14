using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000922 RID: 2338
	// (Invoke) Token: 0x06003868 RID: 14440
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void StatusCallback(int nResult);
}
