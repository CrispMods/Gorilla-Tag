using System;
using System.Runtime.InteropServices;

// Token: 0x02000783 RID: 1923
// (Invoke) Token: 0x06002F24 RID: 12068
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate int lua_CFunction(lua_State* L);
