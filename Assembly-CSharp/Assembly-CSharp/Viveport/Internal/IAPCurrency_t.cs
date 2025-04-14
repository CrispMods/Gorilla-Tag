using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000932 RID: 2354
	internal struct IAPCurrency_t
	{
		// Token: 0x04003B06 RID: 15110
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string m_pName;

		// Token: 0x04003B07 RID: 15111
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string m_pSymbol;
	}
}
