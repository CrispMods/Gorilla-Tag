using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x0200092F RID: 2351
	internal struct IAPCurrency_t
	{
		// Token: 0x04003AF4 RID: 15092
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string m_pName;

		// Token: 0x04003AF5 RID: 15093
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string m_pSymbol;
	}
}
