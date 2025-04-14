using System;
using System.Runtime.InteropServices;

// Token: 0x0200076C RID: 1900
// (Invoke) Token: 0x06002E87 RID: 11911
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate int lua_CFunction(lua_State* L);
