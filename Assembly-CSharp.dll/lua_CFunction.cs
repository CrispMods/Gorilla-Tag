using System;
using System.Runtime.InteropServices;

// Token: 0x0200076D RID: 1901
// (Invoke) Token: 0x06002E8F RID: 11919
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate int lua_CFunction(lua_State* L);
