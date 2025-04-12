using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

// Token: 0x0200076F RID: 1903
public class Luau
{
	// Token: 0x06002E92 RID: 11922
	[DllImport("luau")]
	public unsafe static extern lua_State* luaL_newstate();

	// Token: 0x06002E93 RID: 11923
	[DllImport("luau")]
	public unsafe static extern void luaL_openlibs(lua_State* L);

	// Token: 0x06002E94 RID: 11924
	[DllImport("luau")]
	public unsafe static extern sbyte* luau_compile([MarshalAs(UnmanagedType.LPStr)] string source, [NativeInteger] UIntPtr size, lua_CompileOptions* options, [NativeInteger] UIntPtr* outsize);

	// Token: 0x06002E95 RID: 11925
	[DllImport("luau")]
	public unsafe static extern int luau_load(lua_State* L, [MarshalAs(UnmanagedType.LPStr)] string chunkname, sbyte* data, [NativeInteger] UIntPtr size, int env);

	// Token: 0x06002E96 RID: 11926
	[DllImport("luau")]
	public unsafe static extern void lua_pushvalue(lua_State* L, int idx);

	// Token: 0x06002E97 RID: 11927
	[DllImport("luau")]
	public unsafe static extern void lua_pushcclosurek(lua_State* L, lua_CFunction fn, [MarshalAs(UnmanagedType.LPStr)] string debugname, int nup, lua_Continuation cont);

	// Token: 0x06002E98 RID: 11928
	[DllImport("luau")]
	public unsafe static extern void lua_pushcclosurek(lua_State* L, FunctionPointer<lua_CFunction> fn, [MarshalAs(UnmanagedType.LPStr)] string debugname, int nup, lua_Continuation cont);

	// Token: 0x06002E99 RID: 11929 RVA: 0x00126F7C File Offset: 0x0012517C
	public unsafe static void lua_pushcfunction(lua_State* L, FunctionPointer<lua_CFunction> fn, [MarshalAs(UnmanagedType.LPStr)] string debugname)
	{
		Luau.lua_pushcclosurek(L, fn, debugname, 0, default(lua_Continuation));
	}

	// Token: 0x06002E9A RID: 11930 RVA: 0x00126F9C File Offset: 0x0012519C
	public unsafe static void lua_pushcfunction(lua_State* L, lua_CFunction fn, [MarshalAs(UnmanagedType.LPStr)] string debugname)
	{
		Luau.lua_pushcclosurek(L, fn, debugname, 0, default(lua_Continuation));
	}

	// Token: 0x06002E9B RID: 11931
	[DllImport("luau")]
	public unsafe static extern void lua_settop(lua_State* L, int idx);

	// Token: 0x06002E9C RID: 11932
	[DllImport("luau")]
	public unsafe static extern int lua_gettop(lua_State* L);

	// Token: 0x06002E9D RID: 11933
	[DllImport("luau")]
	public unsafe static extern sbyte* lua_tolstring(lua_State* L, int idx, int* len);

	// Token: 0x06002E9E RID: 11934
	[DllImport("luau")]
	public unsafe static extern int lua_resume(lua_State* L, lua_State* from, int nargs);

	// Token: 0x06002E9F RID: 11935
	[DllImport("luau")]
	public unsafe static extern void lua_setfield(lua_State* L, int index, [MarshalAs(UnmanagedType.LPStr)] string k);

	// Token: 0x06002EA0 RID: 11936
	[DllImport("luau")]
	public unsafe static extern void lua_setfield(lua_State* L, int index, byte* k);

	// Token: 0x06002EA1 RID: 11937 RVA: 0x0004E629 File Offset: 0x0004C829
	public unsafe static void lua_setglobal(lua_State* L, string s)
	{
		Luau.lua_setfield(L, -10002, s);
	}

	// Token: 0x06002EA2 RID: 11938 RVA: 0x00126FBC File Offset: 0x001251BC
	public unsafe static void lua_register(lua_State* L, lua_CFunction f, string n)
	{
		Luau.lua_pushcclosurek(L, f, n, 0, default(lua_Continuation));
		Luau.lua_setglobal(L, n);
	}

	// Token: 0x06002EA3 RID: 11939 RVA: 0x0004E637 File Offset: 0x0004C837
	public unsafe static void lua_pop(lua_State* L, int n)
	{
		Luau.lua_settop(L, -n - 1);
	}

	// Token: 0x06002EA4 RID: 11940 RVA: 0x0004E643 File Offset: 0x0004C843
	public unsafe static sbyte* lua_tostring(lua_State* L, int idx)
	{
		return Luau.lua_tolstring(L, idx, null);
	}

	// Token: 0x06002EA5 RID: 11941
	[DllImport("luau")]
	public unsafe static extern int lua_isstring(lua_State* L, int index);

	// Token: 0x06002EA6 RID: 11942
	[DllImport("luau")]
	public unsafe static extern int lua_type(lua_State* L, int index);

	// Token: 0x06002EA7 RID: 11943
	[DllImport("luau")]
	public unsafe static extern int lua_pushstring(lua_State* L, [MarshalAs(UnmanagedType.LPStr)] string s);

	// Token: 0x06002EA8 RID: 11944
	[DllImport("luau")]
	public unsafe static extern int lua_pushstring(lua_State* L, byte* s);

	// Token: 0x06002EA9 RID: 11945
	[DllImport("luau")]
	public unsafe static extern int lua_error(lua_State* L);

	// Token: 0x06002EAA RID: 11946
	[DllImport("luau")]
	public unsafe static extern void luaL_errorL(lua_State* L, [MarshalAs(UnmanagedType.LPStr)] string fmt, [MarshalAs(UnmanagedType.LPStr)] params string[] a);

	// Token: 0x06002EAB RID: 11947
	[DllImport("luau")]
	public unsafe static extern void luaL_errorL(lua_State* L, sbyte* fmt);

	// Token: 0x06002EAC RID: 11948
	[DllImport("luau")]
	public unsafe static extern int lua_toboolean(lua_State* L, int index);

	// Token: 0x06002EAD RID: 11949
	[DllImport("luau")]
	public unsafe static extern byte* lua_debugtrace(lua_State* L);

	// Token: 0x06002EAE RID: 11950
	[DllImport("luau")]
	public unsafe static extern void lua_close(lua_State* L);

	// Token: 0x06002EAF RID: 11951
	[DllImport("luau")]
	public unsafe static extern void* lua_touserdatatagged(lua_State* L, int idx, int tag);

	// Token: 0x06002EB0 RID: 11952
	[DllImport("luau")]
	public unsafe static extern void* lua_newuserdatatagged(lua_State* L, int sz, int tag);

	// Token: 0x06002EB1 RID: 11953
	[DllImport("luau")]
	public unsafe static extern void lua_getuserdatametatable(lua_State* L, int tag);

	// Token: 0x06002EB2 RID: 11954
	[DllImport("luau")]
	public unsafe static extern void lua_setuserdatametatable(lua_State* L, int tag, int idx);

	// Token: 0x06002EB3 RID: 11955
	[DllImport("luau")]
	public unsafe static extern int lua_setmetatable(lua_State* L, int objindex);

	// Token: 0x06002EB4 RID: 11956
	[DllImport("luau")]
	public unsafe static extern int luaL_newmetatable(lua_State* L, [MarshalAs(UnmanagedType.LPStr)] string tname);

	// Token: 0x06002EB5 RID: 11957
	[DllImport("luau")]
	public unsafe static extern int lua_getfield(lua_State* L, int idx, [MarshalAs(UnmanagedType.LPStr)] string k);

	// Token: 0x06002EB6 RID: 11958
	[DllImport("luau")]
	public unsafe static extern int lua_getfield(lua_State* L, int idx, byte* k);

	// Token: 0x06002EB7 RID: 11959
	[DllImport("luau")]
	public unsafe static extern int luaL_getmetafield(lua_State* L, int idx, byte* k);

	// Token: 0x06002EB8 RID: 11960 RVA: 0x0004E64E File Offset: 0x0004C84E
	public unsafe static void luaL_getmetatable(lua_State* L, string n)
	{
		Luau.lua_getfield(L, -10000, n);
	}

	// Token: 0x06002EB9 RID: 11961 RVA: 0x0004E65D File Offset: 0x0004C85D
	public unsafe static void luaL_getmetatable(lua_State* L, byte* n)
	{
		Luau.lua_getfield(L, -10000, n);
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x0004E66C File Offset: 0x0004C86C
	public unsafe static void lua_getglobal(lua_State* L, string n)
	{
		Luau.lua_getfield(L, -10002, n);
	}

	// Token: 0x06002EBB RID: 11963
	[DllImport("luau")]
	public unsafe static extern int lua_getmetatable(lua_State* L, int objindex);

	// Token: 0x06002EBC RID: 11964
	[DllImport("luau")]
	public unsafe static extern byte* lua_namecallatom(lua_State* L, int* atom);

	// Token: 0x06002EBD RID: 11965
	[DllImport("luau")]
	public unsafe static extern byte* luaL_checklstring(lua_State* L, int numArg, int* l);

	// Token: 0x06002EBE RID: 11966 RVA: 0x0004E67B File Offset: 0x0004C87B
	public unsafe static byte* luaL_checkstring(lua_State* L, int n)
	{
		return Luau.luaL_checklstring(L, n, null);
	}

	// Token: 0x06002EBF RID: 11967
	[DllImport("luau")]
	public unsafe static extern void lua_pushnumber(lua_State* L, double n);

	// Token: 0x06002EC0 RID: 11968
	[DllImport("luau")]
	public unsafe static extern double luaL_checknumber(lua_State* L, int numArg);

	// Token: 0x06002EC1 RID: 11969
	[DllImport("luau")]
	public unsafe static extern void lua_setreadonly(lua_State* L, int idx, int enabled);

	// Token: 0x06002EC2 RID: 11970
	[DllImport("luau")]
	public unsafe static extern double lua_tonumberx(lua_State* L, int index, int* isnum);

	// Token: 0x06002EC3 RID: 11971
	[DllImport("luau")]
	public unsafe static extern int lua_gc(lua_State* L, int what, int data);

	// Token: 0x06002EC4 RID: 11972
	[DllImport("luau")]
	public unsafe static extern void lua_call(lua_State* L, int nargs, int nresults);

	// Token: 0x06002EC5 RID: 11973
	[DllImport("luau")]
	public unsafe static extern int lua_pcall(lua_State* L, int nargs, int nresults, int fn);

	// Token: 0x06002EC6 RID: 11974
	[DllImport("luau")]
	public unsafe static extern int lua_status(lua_State* L);

	// Token: 0x06002EC7 RID: 11975
	[DllImport("luau")]
	public unsafe static extern void* luaL_checkudata(lua_State* L, int arg, [MarshalAs(UnmanagedType.LPStr)] string tname);

	// Token: 0x06002EC8 RID: 11976
	[DllImport("luau")]
	public unsafe static extern void* luaL_checkudata(lua_State* L, int arg, byte* tname);

	// Token: 0x06002EC9 RID: 11977
	[DllImport("luau")]
	public unsafe static extern int lua_objlen(lua_State* L, int index);

	// Token: 0x06002ECA RID: 11978
	[DllImport("luau")]
	public unsafe static extern double luaL_optnumber(lua_State* L, int narg, double d);

	// Token: 0x06002ECB RID: 11979
	[DllImport("luau")]
	public unsafe static extern void lua_createtable(lua_State* L, int narr, int nrec);

	// Token: 0x06002ECC RID: 11980
	[DllImport("luau")]
	public unsafe static extern void lua_pushlightuserdatatagged(lua_State* L, void* p, int tag);

	// Token: 0x06002ECD RID: 11981
	[DllImport("luau")]
	public unsafe static extern void lua_pushnil(lua_State* L);

	// Token: 0x06002ECE RID: 11982
	[DllImport("luau")]
	public unsafe static extern int lua_next(lua_State* L, int index);

	// Token: 0x06002ECF RID: 11983 RVA: 0x0004E686 File Offset: 0x0004C886
	public unsafe static void lua_pushlightuserdata(lua_State* L, void* p)
	{
		Luau.lua_pushlightuserdatatagged(L, p, 0);
	}

	// Token: 0x06002ED0 RID: 11984
	[DllImport("luau")]
	public unsafe static extern void lua_rawseti(lua_State* L, int idx, int n);

	// Token: 0x06002ED1 RID: 11985
	[DllImport("luau")]
	public unsafe static extern void lua_rawgeti(lua_State* L, int index, int n);

	// Token: 0x06002ED2 RID: 11986
	[DllImport("luau")]
	public unsafe static extern void lua_pushboolean(lua_State* L, int b);

	// Token: 0x06002ED3 RID: 11987 RVA: 0x0004E690 File Offset: 0x0004C890
	public unsafe static void* lua_newuserdata(lua_State* L, int size)
	{
		return Luau.lua_newuserdatatagged(L, size, 0);
	}

	// Token: 0x06002ED4 RID: 11988 RVA: 0x0004E69A File Offset: 0x0004C89A
	public unsafe static double lua_tonumber(lua_State* L, int index)
	{
		return Luau.lua_tonumberx(L, index, null);
	}

	// Token: 0x06002ED5 RID: 11989 RVA: 0x00126FE4 File Offset: 0x001251E4
	public unsafe static T* lua_class_push<[IsUnmanaged] T>(lua_State* L) where T : struct, ValueType
	{
		T* result = (T*)Luau.lua_newuserdata(L, sizeof(T));
		FixedString32Bytes name = BurstClassInfo.ClassList.MetatableNames<T>.Name;
		Luau.luaL_getmetatable(L, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref name) + 2);
		Luau.lua_setmetatable(L, -2);
		return result;
	}

	// Token: 0x06002ED6 RID: 11990 RVA: 0x00127020 File Offset: 0x00125220
	public unsafe static T* lua_class_push<[IsUnmanaged] T>(lua_State* L, FixedString32Bytes name) where T : struct, ValueType
	{
		T* result = (T*)Luau.lua_newuserdata(L, sizeof(T));
		Luau.luaL_getmetatable(L, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref name) + 2);
		Luau.lua_setmetatable(L, -2);
		return result;
	}

	// Token: 0x06002ED7 RID: 11991 RVA: 0x00127054 File Offset: 0x00125254
	public unsafe static T* lua_class_get<[IsUnmanaged] T>(lua_State* L, int idx) where T : struct, ValueType
	{
		int num = Luau.lua_type(L, idx);
		if (num == 8 || num == 2)
		{
			FixedString32Bytes name = BurstClassInfo.ClassList.MetatableNames<T>.Name;
			T* ptr = (T*)Luau.luaL_checkudata(L, idx, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref name) + 2);
			if (ptr != null)
			{
				return ptr;
			}
			FixedString32Bytes fixedString32Bytes = "\"Failed to get class\"";
			Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2));
		}
		return null;
	}

	// Token: 0x06002ED8 RID: 11992 RVA: 0x001270AC File Offset: 0x001252AC
	public unsafe static T* lua_class_get<[IsUnmanaged] T>(lua_State* L, int idx, FixedString32Bytes name) where T : struct, ValueType
	{
		int num = Luau.lua_type(L, idx);
		if (num == 8 || num == 2)
		{
			T* ptr = (T*)Luau.luaL_checkudata(L, idx, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref name) + 2);
			if (ptr != null)
			{
				return ptr;
			}
			FixedString32Bytes fixedString32Bytes = "\"Failed to get class\"";
			Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2));
		}
		return null;
	}

	// Token: 0x06002ED9 RID: 11993 RVA: 0x0004E6A5 File Offset: 0x0004C8A5
	public unsafe static bool lua_class_check<[IsUnmanaged] T>(lua_State* L, int idx) where T : struct, ValueType
	{
		return Luau.lua_objlen(L, idx) == sizeof(T);
	}

	// Token: 0x06002EDA RID: 11994 RVA: 0x00127100 File Offset: 0x00125300
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
		Debug.Log(text);
		return 0;
	}

	// Token: 0x040032ED RID: 13037
	public const int LUA_GLOBALSINDEX = -10002;

	// Token: 0x040032EE RID: 13038
	public const int LUA_REGISTRYINDEX = -10000;

	// Token: 0x02000770 RID: 1904
	public enum lua_Types
	{
		// Token: 0x040032F0 RID: 13040
		LUA_TNIL,
		// Token: 0x040032F1 RID: 13041
		LUA_TBOOLEAN,
		// Token: 0x040032F2 RID: 13042
		LUA_TLIGHTUSERDATA,
		// Token: 0x040032F3 RID: 13043
		LUA_TNUMBER,
		// Token: 0x040032F4 RID: 13044
		LUA_TVECTOR,
		// Token: 0x040032F5 RID: 13045
		LUA_TSTRING,
		// Token: 0x040032F6 RID: 13046
		LUA_TTABLE,
		// Token: 0x040032F7 RID: 13047
		LUA_TFUNCTION,
		// Token: 0x040032F8 RID: 13048
		LUA_TUSERDATA,
		// Token: 0x040032F9 RID: 13049
		LUA_TTHREAD,
		// Token: 0x040032FA RID: 13050
		LUA_TBUFFER,
		// Token: 0x040032FB RID: 13051
		LUA_TPROTO,
		// Token: 0x040032FC RID: 13052
		LUA_TUPVAL,
		// Token: 0x040032FD RID: 13053
		LUA_TDEADKEY,
		// Token: 0x040032FE RID: 13054
		LUA_T_COUNT = 11
	}

	// Token: 0x02000771 RID: 1905
	public enum lua_Status
	{
		// Token: 0x04003300 RID: 13056
		LUA_OK,
		// Token: 0x04003301 RID: 13057
		LUA_YIELD,
		// Token: 0x04003302 RID: 13058
		LUA_ERRRUN,
		// Token: 0x04003303 RID: 13059
		LUA_ERRSYNTAX,
		// Token: 0x04003304 RID: 13060
		LUA_ERRMEM,
		// Token: 0x04003305 RID: 13061
		LUA_ERRERR,
		// Token: 0x04003306 RID: 13062
		LUA_BREAK
	}

	// Token: 0x02000772 RID: 1906
	public enum gc_status
	{
		// Token: 0x04003308 RID: 13064
		LUA_GCSTOP,
		// Token: 0x04003309 RID: 13065
		LUA_GCRESTART,
		// Token: 0x0400330A RID: 13066
		LUA_GCCOLLECT,
		// Token: 0x0400330B RID: 13067
		LUA_GCCOUNT,
		// Token: 0x0400330C RID: 13068
		LUA_GCISRUNNING,
		// Token: 0x0400330D RID: 13069
		LUA_GCSTEP,
		// Token: 0x0400330E RID: 13070
		LUA_GCSETGOAL,
		// Token: 0x0400330F RID: 13071
		LUA_GCSETSTEPMUL,
		// Token: 0x04003310 RID: 13072
		LUA_GCSETSTEPSIZE
	}

	// Token: 0x02000773 RID: 1907
	public static class lua_TypeID
	{
		// Token: 0x06002EDC RID: 11996 RVA: 0x00127194 File Offset: 0x00125394
		public static string get(Type t)
		{
			string result;
			if (Luau.lua_TypeID.names.TryGetValue(t, out result))
			{
				return result;
			}
			return "";
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x0004E6B6 File Offset: 0x0004C8B6
		public static void push(Type t, string name)
		{
			Luau.lua_TypeID.names.TryAdd(t, name);
		}

		// Token: 0x04003311 RID: 13073
		private static Dictionary<Type, string> names = new Dictionary<Type, string>();
	}

	// Token: 0x02000774 RID: 1908
	public static class lua_ClassFields<T>
	{
		// Token: 0x06002EDF RID: 11999 RVA: 0x001271B8 File Offset: 0x001253B8
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

		// Token: 0x06002EE0 RID: 12000 RVA: 0x001271F8 File Offset: 0x001253F8
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

		// Token: 0x04003312 RID: 13074
		private static Dictionary<int, Dictionary<int, FieldInfo>> classDictionarys = new Dictionary<int, Dictionary<int, FieldInfo>>();
	}

	// Token: 0x02000775 RID: 1909
	public static class lua_ClassProperties<T>
	{
		// Token: 0x06002EE2 RID: 12002 RVA: 0x00127260 File Offset: 0x00125460
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

		// Token: 0x06002EE3 RID: 12003 RVA: 0x00127294 File Offset: 0x00125494
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

		// Token: 0x04003313 RID: 13075
		private static Dictionary<Type, Dictionary<string, lua_CFunction>> classProperties = new Dictionary<Type, Dictionary<string, lua_CFunction>>();
	}

	// Token: 0x02000776 RID: 1910
	public static class lua_ClassFunctions<T>
	{
		// Token: 0x06002EE5 RID: 12005 RVA: 0x001272E8 File Offset: 0x001254E8
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

		// Token: 0x06002EE6 RID: 12006 RVA: 0x0012731C File Offset: 0x0012551C
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

		// Token: 0x04003314 RID: 13076
		private static Dictionary<Type, Dictionary<string, lua_CFunction>> classProperties = new Dictionary<Type, Dictionary<string, lua_CFunction>>();
	}
}
