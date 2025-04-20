using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000785 RID: 1925
public class Luau
{
	// Token: 0x06002F2B RID: 12075
	[DllImport("luau")]
	public unsafe static extern lua_State* luaL_newstate();

	// Token: 0x06002F2C RID: 12076
	[DllImport("luau")]
	public unsafe static extern void luaL_openlibs(lua_State* L);

	// Token: 0x06002F2D RID: 12077
	[DllImport("luau")]
	public unsafe static extern sbyte* luau_compile([MarshalAs(UnmanagedType.LPStr)] string source, [NativeInteger] UIntPtr size, lua_CompileOptions* options, [NativeInteger] UIntPtr* outsize);

	// Token: 0x06002F2E RID: 12078
	[DllImport("luau")]
	public unsafe static extern int luau_load(lua_State* L, [MarshalAs(UnmanagedType.LPStr)] string chunkname, sbyte* data, [NativeInteger] UIntPtr size, int env);

	// Token: 0x06002F2F RID: 12079
	[DllImport("luau")]
	public unsafe static extern void lua_pushvalue(lua_State* L, int idx);

	// Token: 0x06002F30 RID: 12080
	[DllImport("luau")]
	public unsafe static extern void lua_pushcclosurek(lua_State* L, lua_CFunction fn, [MarshalAs(UnmanagedType.LPStr)] string debugname, int nup, lua_Continuation cont);

	// Token: 0x06002F31 RID: 12081
	[DllImport("luau")]
	public unsafe static extern void lua_pushcclosurek(lua_State* L, FunctionPointer<lua_CFunction> fn, [MarshalAs(UnmanagedType.LPStr)] string debugname, int nup, lua_Continuation cont);

	// Token: 0x06002F32 RID: 12082
	[DllImport("luau")]
	public unsafe static extern void lua_pushcclosurek(lua_State* L, FunctionPointer<lua_CFunction> fn, byte* debugname, int nup, int* cont);

	// Token: 0x06002F33 RID: 12083 RVA: 0x0004F9A0 File Offset: 0x0004DBA0
	public unsafe static void lua_pushcfunction(lua_State* L, FunctionPointer<lua_CFunction> fn, [MarshalAs(UnmanagedType.LPStr)] string debugname)
	{
		Luau.lua_pushcclosurek(L, fn, debugname, 0, null);
	}

	// Token: 0x06002F34 RID: 12084 RVA: 0x0004F9AC File Offset: 0x0004DBAC
	public unsafe static void lua_pushcfunction(lua_State* L, lua_CFunction fn, [MarshalAs(UnmanagedType.LPStr)] string debugname)
	{
		Luau.lua_pushcclosurek(L, fn, debugname, 0, null);
	}

	// Token: 0x06002F35 RID: 12085
	[DllImport("luau")]
	public unsafe static extern void lua_settop(lua_State* L, int idx);

	// Token: 0x06002F36 RID: 12086
	[DllImport("luau")]
	public unsafe static extern int lua_gettop(lua_State* L);

	// Token: 0x06002F37 RID: 12087
	[DllImport("luau")]
	public unsafe static extern sbyte* lua_tolstring(lua_State* L, int idx, int* len);

	// Token: 0x06002F38 RID: 12088
	[DllImport("luau")]
	public unsafe static extern int lua_resume(lua_State* L, lua_State* from, int nargs);

	// Token: 0x06002F39 RID: 12089
	[DllImport("luau")]
	public unsafe static extern void lua_setfield(lua_State* L, int index, [MarshalAs(UnmanagedType.LPStr)] string k);

	// Token: 0x06002F3A RID: 12090
	[DllImport("luau")]
	public unsafe static extern void lua_setfield(lua_State* L, int index, byte* k);

	// Token: 0x06002F3B RID: 12091 RVA: 0x0004F9B8 File Offset: 0x0004DBB8
	public unsafe static void lua_setglobal(lua_State* L, string s)
	{
		Luau.lua_setfield(L, -10002, s);
	}

	// Token: 0x06002F3C RID: 12092 RVA: 0x0012BCEC File Offset: 0x00129EEC
	public unsafe static void lua_register(lua_State* L, lua_CFunction f, string n)
	{
		lua_Continuation cont = null;
		Luau.lua_pushcclosurek(L, f, n, 0, cont);
		Luau.lua_setglobal(L, n);
	}

	// Token: 0x06002F3D RID: 12093 RVA: 0x0004F9C6 File Offset: 0x0004DBC6
	public unsafe static void lua_pop(lua_State* L, int n)
	{
		Luau.lua_settop(L, -n - 1);
	}

	// Token: 0x06002F3E RID: 12094 RVA: 0x0004F9D2 File Offset: 0x0004DBD2
	public unsafe static sbyte* lua_tostring(lua_State* L, int idx)
	{
		return Luau.lua_tolstring(L, idx, null);
	}

	// Token: 0x06002F3F RID: 12095
	[DllImport("luau")]
	public unsafe static extern int lua_isstring(lua_State* L, int index);

	// Token: 0x06002F40 RID: 12096
	[DllImport("luau")]
	public unsafe static extern int lua_type(lua_State* L, int index);

	// Token: 0x06002F41 RID: 12097
	[DllImport("luau")]
	public unsafe static extern int lua_pushstring(lua_State* L, [MarshalAs(UnmanagedType.LPStr)] string s);

	// Token: 0x06002F42 RID: 12098
	[DllImport("luau")]
	public unsafe static extern int lua_pushstring(lua_State* L, byte* s);

	// Token: 0x06002F43 RID: 12099
	[DllImport("luau")]
	public unsafe static extern int lua_error(lua_State* L);

	// Token: 0x06002F44 RID: 12100
	[DllImport("luau")]
	public unsafe static extern void luaL_errorL(lua_State* L, [MarshalAs(UnmanagedType.LPStr)] string fmt, [MarshalAs(UnmanagedType.LPStr)] params string[] a);

	// Token: 0x06002F45 RID: 12101
	[DllImport("luau")]
	public unsafe static extern void luaL_errorL(lua_State* L, sbyte* fmt);

	// Token: 0x06002F46 RID: 12102
	[DllImport("luau")]
	public unsafe static extern int lua_toboolean(lua_State* L, int index);

	// Token: 0x06002F47 RID: 12103
	[DllImport("luau")]
	public unsafe static extern byte* lua_debugtrace(lua_State* L);

	// Token: 0x06002F48 RID: 12104
	[DllImport("luau")]
	public unsafe static extern void lua_close(lua_State* L);

	// Token: 0x06002F49 RID: 12105
	[DllImport("luau")]
	public unsafe static extern void* lua_touserdatatagged(lua_State* L, int idx, int tag);

	// Token: 0x06002F4A RID: 12106
	[DllImport("luau")]
	public unsafe static extern void* lua_touserdata(lua_State* L, int index);

	// Token: 0x06002F4B RID: 12107
	[DllImport("luau")]
	public unsafe static extern void* lua_newuserdatatagged(lua_State* L, int sz, int tag);

	// Token: 0x06002F4C RID: 12108
	[DllImport("luau")]
	public unsafe static extern void lua_getuserdatametatable(lua_State* L, int tag);

	// Token: 0x06002F4D RID: 12109
	[DllImport("luau")]
	public unsafe static extern void lua_setuserdatametatable(lua_State* L, int tag, int idx);

	// Token: 0x06002F4E RID: 12110
	[DllImport("luau")]
	public unsafe static extern int lua_setmetatable(lua_State* L, int objindex);

	// Token: 0x06002F4F RID: 12111
	[DllImport("luau")]
	public unsafe static extern int luaL_newmetatable(lua_State* L, [MarshalAs(UnmanagedType.LPStr)] string tname);

	// Token: 0x06002F50 RID: 12112
	[DllImport("luau")]
	public unsafe static extern int lua_getfield(lua_State* L, int idx, [MarshalAs(UnmanagedType.LPStr)] string k);

	// Token: 0x06002F51 RID: 12113
	[DllImport("luau")]
	public unsafe static extern int lua_getfield(lua_State* L, int idx, byte* k);

	// Token: 0x06002F52 RID: 12114
	[DllImport("luau")]
	public unsafe static extern int luaL_getmetafield(lua_State* L, int idx, byte* k);

	// Token: 0x06002F53 RID: 12115
	[DllImport("luau")]
	public unsafe static extern int luaL_getmetafield(lua_State* L, int idx, [MarshalAs(UnmanagedType.LPStr)] string k);

	// Token: 0x06002F54 RID: 12116 RVA: 0x0004F9DD File Offset: 0x0004DBDD
	public unsafe static void luaL_getmetatable(lua_State* L, string n)
	{
		Luau.lua_getfield(L, -10000, n);
	}

	// Token: 0x06002F55 RID: 12117 RVA: 0x0004F9EC File Offset: 0x0004DBEC
	public unsafe static void luaL_getmetatable(lua_State* L, byte* n)
	{
		Luau.lua_getfield(L, -10000, n);
	}

	// Token: 0x06002F56 RID: 12118 RVA: 0x0004F9FB File Offset: 0x0004DBFB
	public unsafe static void lua_getglobal(lua_State* L, string n)
	{
		Luau.lua_getfield(L, -10002, n);
	}

	// Token: 0x06002F57 RID: 12119
	[DllImport("luau")]
	public unsafe static extern int lua_getmetatable(lua_State* L, int objindex);

	// Token: 0x06002F58 RID: 12120
	[DllImport("luau")]
	public unsafe static extern byte* lua_namecallatom(lua_State* L, int* atom);

	// Token: 0x06002F59 RID: 12121
	[DllImport("luau")]
	public unsafe static extern byte* luaL_checklstring(lua_State* L, int numArg, int* l);

	// Token: 0x06002F5A RID: 12122 RVA: 0x0004FA0A File Offset: 0x0004DC0A
	public unsafe static byte* luaL_checkstring(lua_State* L, int n)
	{
		return Luau.luaL_checklstring(L, n, null);
	}

	// Token: 0x06002F5B RID: 12123
	[DllImport("luau")]
	public unsafe static extern void lua_pushnumber(lua_State* L, double n);

	// Token: 0x06002F5C RID: 12124
	[DllImport("luau")]
	public unsafe static extern double luaL_checknumber(lua_State* L, int numArg);

	// Token: 0x06002F5D RID: 12125
	[DllImport("luau")]
	public unsafe static extern void lua_setreadonly(lua_State* L, int idx, int enabled);

	// Token: 0x06002F5E RID: 12126
	[DllImport("luau")]
	public unsafe static extern double lua_tonumberx(lua_State* L, int index, int* isnum);

	// Token: 0x06002F5F RID: 12127
	[DllImport("luau")]
	public unsafe static extern int lua_gc(lua_State* L, int what, int data);

	// Token: 0x06002F60 RID: 12128
	[DllImport("luau")]
	public unsafe static extern void lua_call(lua_State* L, int nargs, int nresults);

	// Token: 0x06002F61 RID: 12129
	[DllImport("luau")]
	public unsafe static extern int lua_pcall(lua_State* L, int nargs, int nresults, int fn);

	// Token: 0x06002F62 RID: 12130
	[DllImport("luau")]
	public unsafe static extern int lua_status(lua_State* L);

	// Token: 0x06002F63 RID: 12131
	[DllImport("luau")]
	public unsafe static extern void* luaL_checkudata(lua_State* L, int arg, [MarshalAs(UnmanagedType.LPStr)] string tname);

	// Token: 0x06002F64 RID: 12132
	[DllImport("luau")]
	public unsafe static extern void* luaL_checkudata(lua_State* L, int arg, byte* tname);

	// Token: 0x06002F65 RID: 12133
	[DllImport("luau")]
	public unsafe static extern int lua_objlen(lua_State* L, int index);

	// Token: 0x06002F66 RID: 12134
	[DllImport("luau")]
	public unsafe static extern double luaL_optnumber(lua_State* L, int narg, double d);

	// Token: 0x06002F67 RID: 12135
	[DllImport("luau")]
	public unsafe static extern void lua_createtable(lua_State* L, int narr, int nrec);

	// Token: 0x06002F68 RID: 12136
	[DllImport("luau")]
	public unsafe static extern void lua_pushlightuserdatatagged(lua_State* L, void* p, int tag);

	// Token: 0x06002F69 RID: 12137
	[DllImport("luau")]
	public unsafe static extern void lua_pushnil(lua_State* L);

	// Token: 0x06002F6A RID: 12138
	[DllImport("luau")]
	public unsafe static extern int lua_next(lua_State* L, int index);

	// Token: 0x06002F6B RID: 12139 RVA: 0x0004FA15 File Offset: 0x0004DC15
	public unsafe static void lua_pushlightuserdata(lua_State* L, void* p)
	{
		Luau.lua_pushlightuserdatatagged(L, p, 0);
	}

	// Token: 0x06002F6C RID: 12140
	[DllImport("luau")]
	public unsafe static extern void lua_rawseti(lua_State* L, int idx, int n);

	// Token: 0x06002F6D RID: 12141
	[DllImport("luau")]
	public unsafe static extern void lua_rawgeti(lua_State* L, int index, int n);

	// Token: 0x06002F6E RID: 12142
	[DllImport("luau")]
	public unsafe static extern void lua_rawget(lua_State* L, int index);

	// Token: 0x06002F6F RID: 12143
	[DllImport("luau")]
	public unsafe static extern void lua_remove(lua_State* L, int index);

	// Token: 0x06002F70 RID: 12144
	[DllImport("luau")]
	public unsafe static extern void lua_pushboolean(lua_State* L, int b);

	// Token: 0x06002F71 RID: 12145
	[DllImport("luau")]
	public unsafe static extern int lua_rawequal(lua_State* L, int a, int b);

	// Token: 0x06002F72 RID: 12146 RVA: 0x0004FA1F File Offset: 0x0004DC1F
	public unsafe static void* lua_newuserdata(lua_State* L, int size)
	{
		return Luau.lua_newuserdatatagged(L, size, 0);
	}

	// Token: 0x06002F73 RID: 12147 RVA: 0x0004FA29 File Offset: 0x0004DC29
	public unsafe static double lua_tonumber(lua_State* L, int index)
	{
		return Luau.lua_tonumberx(L, index, null);
	}

	// Token: 0x06002F74 RID: 12148 RVA: 0x0012BD0C File Offset: 0x00129F0C
	public unsafe static T* lua_class_push<[IsUnmanaged] T>(lua_State* L) where T : struct, ValueType
	{
		T* result = (T*)Luau.lua_newuserdata(L, sizeof(T));
		FixedString32Bytes name = BurstClassInfo.ClassList.MetatableNames<T>.Name;
		Luau.luaL_getmetatable(L, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref name) + 2);
		Luau.lua_setmetatable(L, -2);
		return result;
	}

	// Token: 0x06002F75 RID: 12149 RVA: 0x0012BD48 File Offset: 0x00129F48
	public unsafe static T* lua_class_push<[IsUnmanaged] T>(lua_State* L, FixedString32Bytes name) where T : struct, ValueType
	{
		T* result = (T*)Luau.lua_newuserdata(L, sizeof(T));
		Luau.luaL_getmetatable(L, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref name) + 2);
		Luau.lua_setmetatable(L, -2);
		return result;
	}

	// Token: 0x06002F76 RID: 12150 RVA: 0x0012BD7C File Offset: 0x00129F7C
	public unsafe static void lua_class_push(lua_State* L, FixedString32Bytes name, IntPtr ptr)
	{
		FixedString32Bytes fixedString32Bytes = "__ptr";
		Luau.lua_createtable(L, 0, 0);
		Luau.lua_pushlightuserdata(L, (void*)ptr);
		Luau.lua_setfield(L, -2, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2);
		Luau.luaL_getmetatable(L, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref name) + 2);
		Luau.lua_setmetatable(L, -2);
	}

	// Token: 0x06002F77 RID: 12151 RVA: 0x0012BDD4 File Offset: 0x00129FD4
	public unsafe static void lua_class_push<[IsUnmanaged] T>(lua_State* L, T* ptr) where T : struct, ValueType
	{
		FixedString32Bytes fixedString32Bytes = "__ptr";
		FixedString32Bytes name = BurstClassInfo.ClassList.MetatableNames<T>.Name;
		Luau.lua_createtable(L, 0, 0);
		Luau.lua_pushlightuserdata(L, (void*)ptr);
		Luau.lua_setfield(L, -2, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2);
		Luau.luaL_getmetatable(L, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref name) + 2);
		Luau.lua_setmetatable(L, -2);
	}

	// Token: 0x06002F78 RID: 12152 RVA: 0x0012BE2C File Offset: 0x0012A02C
	public unsafe static T* lua_class_get<[IsUnmanaged] T>(lua_State* L, int idx) where T : struct, ValueType
	{
		int num = Luau.lua_type(L, idx);
		FixedString32Bytes name = BurstClassInfo.ClassList.MetatableNames<T>.Name;
		byte* ptr = (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref name) + 2;
		if (num == 8)
		{
			T* ptr2 = (T*)Luau.luaL_checkudata(L, idx, ptr);
			if (ptr2 != null)
			{
				return ptr2;
			}
		}
		if (num == 6)
		{
			Luau.lua_getmetatable(L, idx);
			Luau.luaL_getmetatable(L, ptr);
			bool flag = Luau.lua_rawequal(L, -1, -2) == 1;
			Luau.lua_pop(L, 2);
			if (flag)
			{
				Luau.lua_getfield(L, idx, "__ptr");
				if (Luau.lua_type(L, -1) == 2)
				{
					T* ptr3 = (T*)Luau.lua_touserdata(L, -1);
					Luau.lua_pop(L, 1);
					if (ptr3 != null)
					{
						return ptr3;
					}
				}
				Luau.lua_pop(L, 1);
			}
		}
		FixedString32Bytes fixedString32Bytes = "\"Invalid Type\"";
		Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2));
		return null;
	}

	// Token: 0x06002F79 RID: 12153 RVA: 0x0012BEE4 File Offset: 0x0012A0E4
	public unsafe static T* lua_class_get<[IsUnmanaged] T>(lua_State* L, int idx, FixedString32Bytes name) where T : struct, ValueType
	{
		int num = Luau.lua_type(L, idx);
		byte* ptr = (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref name) + 2;
		if (num == 8)
		{
			T* ptr2 = (T*)Luau.luaL_checkudata(L, idx, ptr);
			if (ptr2 != null)
			{
				return ptr2;
			}
		}
		if (num == 6)
		{
			Luau.lua_getmetatable(L, idx);
			Luau.luaL_getmetatable(L, ptr);
			bool flag = Luau.lua_rawequal(L, -1, -2) == 1;
			Luau.lua_pop(L, 1);
			if (flag)
			{
				FixedString32Bytes fixedString32Bytes = "__ptr";
				Luau.lua_getfield(L, idx, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2);
				if (Luau.lua_type(L, -1) == 2)
				{
					T* ptr3 = (T*)Luau.lua_touserdata(L, -1);
					Luau.lua_pop(L, 1);
					if (ptr3 != null)
					{
						return ptr3;
					}
				}
				Luau.lua_pop(L, 1);
			}
		}
		FixedString32Bytes fixedString32Bytes2 = "\"Invalid Type\"";
		Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes2) + 2));
		return null;
	}

	// Token: 0x06002F7A RID: 12154 RVA: 0x0012BFA4 File Offset: 0x0012A1A4
	public unsafe static byte* lua_class_get(lua_State* L, int idx, FixedString32Bytes name)
	{
		int num = Luau.lua_type(L, idx);
		byte* ptr = (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref name) + 2;
		if (num == 8)
		{
			byte* ptr2 = (byte*)Luau.luaL_checkudata(L, idx, ptr);
			if (ptr2 != null)
			{
				return ptr2;
			}
		}
		if (num == 6)
		{
			Luau.lua_getmetatable(L, idx);
			Luau.luaL_getmetatable(L, ptr);
			bool flag = Luau.lua_rawequal(L, -1, -2) == 1;
			Luau.lua_pop(L, 1);
			if (flag)
			{
				FixedString32Bytes fixedString32Bytes = "__ptr";
				Luau.lua_getfield(L, idx, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2);
				if (Luau.lua_type(L, -1) == 2)
				{
					byte* ptr3 = (byte*)Luau.lua_touserdata(L, -1);
					Luau.lua_pop(L, 1);
					if (ptr3 != null)
					{
						return ptr3;
					}
				}
				Luau.lua_pop(L, 1);
			}
		}
		FixedString32Bytes fixedString32Bytes2 = "\"Invalid Type\"";
		Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes2) + 2));
		return null;
	}

	// Token: 0x06002F7B RID: 12155 RVA: 0x0012C064 File Offset: 0x0012A264
	public unsafe static IntPtr lua_light_ptr(lua_State* L, int idx)
	{
		FixedString32Bytes fixedString32Bytes = "__ptr";
		Luau.lua_getfield(L, idx, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2);
		if (Luau.lua_type(L, -1) == 2)
		{
			void* ptr = Luau.lua_touserdata(L, -1);
			Luau.lua_pop(L, 1);
			if (ptr != null)
			{
				return (IntPtr)ptr;
			}
		}
		return IntPtr.Zero;
	}

	// Token: 0x06002F7C RID: 12156 RVA: 0x0004FA34 File Offset: 0x0004DC34
	public unsafe static bool lua_class_check<[IsUnmanaged] T>(lua_State* L, int idx) where T : struct, ValueType
	{
		return Luau.lua_objlen(L, idx) == sizeof(T);
	}

	// Token: 0x06002F7D RID: 12157 RVA: 0x0012C0B8 File Offset: 0x0012A2B8
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int lua_print(lua_State* L)
	{
		string text = "";
		int num = Luau.lua_gettop(L);
		for (int i = 1; i <= num; i++)
		{
			int num2 = Luau.lua_type(L, i);
			if (num2 == 5 || num2 == 3)
			{
				sbyte* value = Luau.lua_tostring(L, i);
				text += Marshal.PtrToStringAnsi((IntPtr)((void*)value));
			}
			else
			{
				if (num2 != 1)
				{
					Luau.luaL_errorL(L, "Invalid String", Array.Empty<string>());
					return 0;
				}
				int num3 = Luau.lua_toboolean(L, i);
				text += ((num3 == 1) ? "true" : "false");
			}
		}
		LuauHud.Instance.LuauLog(text);
		return 0;
	}

	// Token: 0x04003386 RID: 13190
	public const int LUA_GLOBALSINDEX = -10002;

	// Token: 0x04003387 RID: 13191
	public const int LUA_REGISTRYINDEX = -10000;

	// Token: 0x02000786 RID: 1926
	public enum lua_Types
	{
		// Token: 0x04003389 RID: 13193
		LUA_TNIL,
		// Token: 0x0400338A RID: 13194
		LUA_TBOOLEAN,
		// Token: 0x0400338B RID: 13195
		LUA_TLIGHTUSERDATA,
		// Token: 0x0400338C RID: 13196
		LUA_TNUMBER,
		// Token: 0x0400338D RID: 13197
		LUA_TVECTOR,
		// Token: 0x0400338E RID: 13198
		LUA_TSTRING,
		// Token: 0x0400338F RID: 13199
		LUA_TTABLE,
		// Token: 0x04003390 RID: 13200
		LUA_TFUNCTION,
		// Token: 0x04003391 RID: 13201
		LUA_TUSERDATA,
		// Token: 0x04003392 RID: 13202
		LUA_TTHREAD,
		// Token: 0x04003393 RID: 13203
		LUA_TBUFFER,
		// Token: 0x04003394 RID: 13204
		LUA_TPROTO,
		// Token: 0x04003395 RID: 13205
		LUA_TUPVAL,
		// Token: 0x04003396 RID: 13206
		LUA_TDEADKEY,
		// Token: 0x04003397 RID: 13207
		LUA_T_COUNT = 11
	}

	// Token: 0x02000787 RID: 1927
	public enum lua_Status
	{
		// Token: 0x04003399 RID: 13209
		LUA_OK,
		// Token: 0x0400339A RID: 13210
		LUA_YIELD,
		// Token: 0x0400339B RID: 13211
		LUA_ERRRUN,
		// Token: 0x0400339C RID: 13212
		LUA_ERRSYNTAX,
		// Token: 0x0400339D RID: 13213
		LUA_ERRMEM,
		// Token: 0x0400339E RID: 13214
		LUA_ERRERR,
		// Token: 0x0400339F RID: 13215
		LUA_BREAK
	}

	// Token: 0x02000788 RID: 1928
	public enum gc_status
	{
		// Token: 0x040033A1 RID: 13217
		LUA_GCSTOP,
		// Token: 0x040033A2 RID: 13218
		LUA_GCRESTART,
		// Token: 0x040033A3 RID: 13219
		LUA_GCCOLLECT,
		// Token: 0x040033A4 RID: 13220
		LUA_GCCOUNT,
		// Token: 0x040033A5 RID: 13221
		LUA_GCISRUNNING,
		// Token: 0x040033A6 RID: 13222
		LUA_GCSTEP,
		// Token: 0x040033A7 RID: 13223
		LUA_GCSETGOAL,
		// Token: 0x040033A8 RID: 13224
		LUA_GCSETSTEPMUL,
		// Token: 0x040033A9 RID: 13225
		LUA_GCSETSTEPSIZE
	}

	// Token: 0x02000789 RID: 1929
	public static class lua_TypeID
	{
		// Token: 0x06002F7F RID: 12159 RVA: 0x0012C154 File Offset: 0x0012A354
		public static string get(Type t)
		{
			string result;
			if (Luau.lua_TypeID.names.TryGetValue(t, out result))
			{
				return result;
			}
			return "";
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x0004FA45 File Offset: 0x0004DC45
		public static void push(Type t, string name)
		{
			Luau.lua_TypeID.names.TryAdd(t, name);
		}

		// Token: 0x040033AA RID: 13226
		private static Dictionary<Type, string> names = new Dictionary<Type, string>();
	}

	// Token: 0x0200078A RID: 1930
	public static class lua_ClassFields<T>
	{
		// Token: 0x06002F82 RID: 12162 RVA: 0x0012C178 File Offset: 0x0012A378
		public static FieldInfo Get(string name)
		{
			Dictionary<int, FieldInfo> dictionary;
			FieldInfo result;
			if (Luau.lua_ClassFields<T>.classDictionarys.TryGetValue(typeof(T).GetHashCode(), out dictionary) && dictionary.TryGetValue(name.GetHashCode(), out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x0012C1B8 File Offset: 0x0012A3B8
		public static void Add(string name, FieldInfo field)
		{
			Dictionary<int, FieldInfo> dictionary;
			if (Luau.lua_ClassFields<T>.classDictionarys.TryGetValue(typeof(T).GetHashCode(), out dictionary))
			{
				dictionary.TryAdd(name.GetHashCode(), field);
				return;
			}
			Dictionary<int, FieldInfo> dictionary2 = new Dictionary<int, FieldInfo>();
			dictionary2.TryAdd(name.GetHashCode(), field);
			Luau.lua_ClassFields<T>.classDictionarys.TryAdd(typeof(T).GetHashCode(), dictionary2);
		}

		// Token: 0x040033AB RID: 13227
		private static Dictionary<int, Dictionary<int, FieldInfo>> classDictionarys = new Dictionary<int, Dictionary<int, FieldInfo>>();
	}

	// Token: 0x0200078B RID: 1931
	public static class lua_ClassProperties<T>
	{
		// Token: 0x06002F85 RID: 12165 RVA: 0x0012C220 File Offset: 0x0012A420
		public static lua_CFunction Get(string name)
		{
			Dictionary<string, lua_CFunction> dictionary;
			lua_CFunction result;
			if (Luau.lua_ClassProperties<T>.classProperties.TryGetValue(typeof(T), out dictionary) && dictionary.TryGetValue(name, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x0012C254 File Offset: 0x0012A454
		public static void Add(string name, lua_CFunction field)
		{
			Dictionary<string, lua_CFunction> dictionary;
			if (Luau.lua_ClassProperties<T>.classProperties.TryGetValue(typeof(T), out dictionary))
			{
				dictionary.TryAdd(name, field);
				return;
			}
			Dictionary<string, lua_CFunction> dictionary2 = new Dictionary<string, lua_CFunction>();
			dictionary2.TryAdd(name, field);
			Luau.lua_ClassProperties<T>.classProperties.TryAdd(typeof(T), dictionary2);
		}

		// Token: 0x040033AC RID: 13228
		private static Dictionary<Type, Dictionary<string, lua_CFunction>> classProperties = new Dictionary<Type, Dictionary<string, lua_CFunction>>();
	}

	// Token: 0x0200078C RID: 1932
	public static class lua_ClassFunctions<T>
	{
		// Token: 0x06002F88 RID: 12168 RVA: 0x0012C2A8 File Offset: 0x0012A4A8
		public static lua_CFunction Get(string name)
		{
			Dictionary<string, lua_CFunction> dictionary;
			lua_CFunction result;
			if (Luau.lua_ClassFunctions<T>.classProperties.TryGetValue(typeof(T), out dictionary) && dictionary.TryGetValue(name, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x0012C2DC File Offset: 0x0012A4DC
		public static void Add(string name, lua_CFunction field)
		{
			Dictionary<string, lua_CFunction> dictionary;
			if (Luau.lua_ClassFunctions<T>.classProperties.TryGetValue(typeof(T), out dictionary))
			{
				dictionary.TryAdd(name, field);
				return;
			}
			Dictionary<string, lua_CFunction> dictionary2 = new Dictionary<string, lua_CFunction>();
			dictionary2.TryAdd(name, field);
			Luau.lua_ClassFunctions<T>.classProperties.TryAdd(typeof(T), dictionary2);
		}

		// Token: 0x040033AD RID: 13229
		private static Dictionary<Type, Dictionary<string, lua_CFunction>> classProperties = new Dictionary<Type, Dictionary<string, lua_CFunction>>();
	}
}
