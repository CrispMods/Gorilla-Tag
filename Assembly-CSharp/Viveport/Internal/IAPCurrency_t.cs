using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x0200094C RID: 2380
	internal struct IAPCurrency_t
	{
		// Token: 0x04003BB9 RID: 15289
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string m_pName;

		// Token: 0x04003BBA RID: 15290
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string m_pSymbol;
	}
}
