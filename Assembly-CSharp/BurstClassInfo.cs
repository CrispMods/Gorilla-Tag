using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x0200075B RID: 1883
[BurstCompile]
public static class BurstClassInfo
{
	// Token: 0x06002E50 RID: 11856 RVA: 0x000E1274 File Offset: 0x000DF474
	public unsafe static void NewClass<T>(string className, Dictionary<int, FieldInfo> fieldList, Dictionary<int, lua_CFunction> functionList, Dictionary<int, FunctionPointer<lua_CFunction>> functionPtrList)
	{
		if (!BurstClassInfo.ClassList.InfoFields.Data.IsCreated)
		{
			*BurstClassInfo.ClassList.InfoFields.Data = new NativeHashMap<int, BurstClassInfo.ClassInfo>(20, Allocator.Persistent);
		}
		BurstClassInfo.ClassList.MetatableNames<T>.Name = className;
		ReflectionMetaNames.ReflectedNames.TryAdd(typeof(T), className);
		BurstClassInfo.ClassInfo classInfo = default(BurstClassInfo.ClassInfo);
		classInfo.NameHash = LuaHashing.ByteHash(className);
		if (className.Length > 30)
		{
			throw new Exception("Name to long");
		}
		classInfo.Name = className;
		classInfo.FieldList = new NativeHashMap<int, BurstClassInfo.BurstFieldInfo>(fieldList.Count, Allocator.Persistent);
		foreach (KeyValuePair<int, FieldInfo> keyValuePair in fieldList)
		{
			BurstClassInfo.BurstFieldInfo item = default(BurstClassInfo.BurstFieldInfo);
			item.NameHash = keyValuePair.Key;
			item.Name = keyValuePair.Value.Name;
			item.Offset = (int)Marshal.OffsetOf<T>(keyValuePair.Value.Name);
			Type fieldType = keyValuePair.Value.FieldType;
			if (fieldType == typeof(float))
			{
				item.FieldType = BurstClassInfo.EFieldTypes.Float;
			}
			else if (fieldType == typeof(int))
			{
				item.FieldType = BurstClassInfo.EFieldTypes.Int;
			}
			else if (fieldType == typeof(double))
			{
				item.FieldType = BurstClassInfo.EFieldTypes.Double;
			}
			else if (fieldType == typeof(bool))
			{
				item.FieldType = BurstClassInfo.EFieldTypes.Bool;
			}
			else if (fieldType == typeof(FixedString32Bytes))
			{
				item.FieldType = BurstClassInfo.EFieldTypes.String;
			}
			else if (!fieldType.IsPrimitive)
			{
				item.FieldType = BurstClassInfo.EFieldTypes.LightUserData;
				ReflectionMetaNames.ReflectedNames.TryGetValue(fieldType, out item.MetatableName);
			}
			item.Size = Marshal.SizeOf(fieldType);
			classInfo.FieldList.TryAdd(keyValuePair.Key, item);
		}
		classInfo.FunctionList = new NativeHashMap<int, IntPtr>(functionList.Count + functionPtrList.Count, Allocator.Persistent);
		foreach (KeyValuePair<int, lua_CFunction> keyValuePair2 in functionList)
		{
			classInfo.FunctionList.TryAdd(keyValuePair2.Key, Marshal.GetFunctionPointerForDelegate<lua_CFunction>(keyValuePair2.Value));
		}
		foreach (KeyValuePair<int, FunctionPointer<lua_CFunction>> keyValuePair3 in functionPtrList)
		{
			classInfo.FunctionList.TryAdd(keyValuePair3.Key, keyValuePair3.Value.Value);
		}
		BurstClassInfo.ClassList.InfoFields.Data.Add(classInfo.NameHash, classInfo);
	}

	// Token: 0x06002E51 RID: 11857 RVA: 0x000E15A8 File Offset: 0x000DF7A8
	[BurstCompile]
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int Index(lua_State* L)
	{
		return BurstClassInfo.Index_00002D3E$BurstDirectCall.Invoke(L);
	}

	// Token: 0x06002E52 RID: 11858 RVA: 0x000E15B0 File Offset: 0x000DF7B0
	[BurstCompile]
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int NewIndex(lua_State* L)
	{
		return BurstClassInfo.NewIndex_00002D3F$BurstDirectCall.Invoke(L);
	}

	// Token: 0x06002E53 RID: 11859 RVA: 0x000E15B8 File Offset: 0x000DF7B8
	[BurstCompile]
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int NameCall(lua_State* L)
	{
		return BurstClassInfo.NameCall_00002D40$BurstDirectCall.Invoke(L);
	}

	// Token: 0x06002E55 RID: 11861 RVA: 0x000E15D4 File Offset: 0x000DF7D4
	[BurstCompile]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int Index$BurstManaged(lua_State* L)
	{
		FixedString32Bytes k_metatableLookup = BurstClassInfo._k_metatableLookup;
		byte* k = (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref k_metatableLookup) + 2;
		Luau.lua_getmetatable(L, 1);
		Luau.lua_getfield(L, -1, k);
		BurstClassInfo.ClassInfo classInfo;
		if (!BurstClassInfo.ClassList.InfoFields.Data.TryGetValue((int)Luau.luaL_checknumber(L, -1), out classInfo))
		{
			FixedString32Bytes fixedString32Bytes = "\"Internal Class Info Error\"";
			Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2));
			return 0;
		}
		byte* tname = (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref classInfo.Name) + 2;
		IntPtr pointer = (IntPtr)Luau.luaL_checkudata(L, 1, tname);
		int len = Luau.lua_objlen(L, 2);
		int key = LuaHashing.ByteHash(Luau.luaL_checkstring(L, 2), len);
		BurstClassInfo.BurstFieldInfo burstFieldInfo;
		if (classInfo.FieldList.TryGetValue(key, out burstFieldInfo))
		{
			IntPtr value = pointer + burstFieldInfo.Offset;
			switch (burstFieldInfo.FieldType)
			{
			case BurstClassInfo.EFieldTypes.Float:
				Luau.lua_pushnumber(L, (double)(*(float*)((void*)value)));
				return 1;
			case BurstClassInfo.EFieldTypes.Int:
				Luau.lua_pushnumber(L, (double)(*(int*)((void*)value)));
				return 1;
			case BurstClassInfo.EFieldTypes.Double:
				Luau.lua_pushnumber(L, *(double*)((void*)value));
				return 1;
			case BurstClassInfo.EFieldTypes.Bool:
				Luau.lua_pushboolean(L, (*(byte*)((void*)value) != 0) ? 1 : 0);
				return 1;
			case BurstClassInfo.EFieldTypes.String:
				Luau.lua_pushstring(L, (byte*)((void*)value) + 2);
				return 1;
			case BurstClassInfo.EFieldTypes.LightUserData:
				Luau.lua_pushlightuserdata(L, (void*)value);
				Luau.luaL_getmetatable(L, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref burstFieldInfo.MetatableName) + 2);
				Luau.lua_setmetatable(L, -2);
				return 1;
			}
		}
		FixedString32Bytes fixedString32Bytes2 = "\"Unknown Type?\"";
		Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes2) + 2));
		return 0;
	}

	// Token: 0x06002E56 RID: 11862 RVA: 0x000E1768 File Offset: 0x000DF968
	[BurstCompile]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int NewIndex$BurstManaged(lua_State* L)
	{
		FixedString32Bytes k_metatableLookup = BurstClassInfo._k_metatableLookup;
		byte* k = (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref k_metatableLookup) + 2;
		Luau.luaL_getmetafield(L, 1, k);
		BurstClassInfo.ClassInfo classInfo;
		if (!BurstClassInfo.ClassList.InfoFields.Data.TryGetValue((int)Luau.luaL_checknumber(L, -1), out classInfo))
		{
			FixedString32Bytes fixedString32Bytes = "\"Internal Class Info Error\"";
			Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2));
			return 0;
		}
		byte* tname = (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref classInfo.Name) + 2;
		IntPtr pointer = (IntPtr)Luau.luaL_checkudata(L, 1, tname);
		int len = Luau.lua_objlen(L, 2);
		int key = LuaHashing.ByteHash(Luau.luaL_checkstring(L, 2), len);
		BurstClassInfo.BurstFieldInfo burstFieldInfo;
		if (classInfo.FieldList.TryGetValue(key, out burstFieldInfo))
		{
			IntPtr value = pointer + burstFieldInfo.Offset;
			switch (burstFieldInfo.FieldType)
			{
			case BurstClassInfo.EFieldTypes.Float:
				*(float*)((void*)value) = (float)Luau.luaL_checknumber(L, 3);
				return 0;
			case BurstClassInfo.EFieldTypes.Int:
				*(int*)((void*)value) = (int)Luau.luaL_checknumber(L, 3);
				return 0;
			case BurstClassInfo.EFieldTypes.Double:
				*(double*)((void*)value) = Luau.luaL_checknumber(L, 3);
				return 0;
			case BurstClassInfo.EFieldTypes.Bool:
				*(byte*)((void*)value) = ((Luau.lua_toboolean(L, 3) != 0) ? 1 : 0);
				return 0;
			case BurstClassInfo.EFieldTypes.LightUserData:
				Buffer.MemoryCopy((void*)((IntPtr)Luau.luaL_checkudata(L, 3, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref burstFieldInfo.MetatableName) + 2)), (void*)value, (long)burstFieldInfo.Size, (long)burstFieldInfo.Size);
				return 0;
			}
		}
		FixedString32Bytes fixedString32Bytes2 = "\"Unknown Type\"";
		Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes2) + 2));
		return 0;
	}

	// Token: 0x06002E57 RID: 11863 RVA: 0x000E18F4 File Offset: 0x000DFAF4
	[BurstCompile]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int NameCall$BurstManaged(lua_State* L)
	{
		FixedString32Bytes k_metatableLookup = BurstClassInfo._k_metatableLookup;
		byte* k = (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref k_metatableLookup) + 2;
		Luau.luaL_getmetafield(L, 1, k);
		BurstClassInfo.ClassInfo classInfo;
		if (!BurstClassInfo.ClassList.InfoFields.Data.TryGetValue((int)Luau.luaL_checknumber(L, -1), out classInfo))
		{
			FixedString32Bytes fixedString32Bytes = "\"Internal Class Info Error\"";
			Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2));
			return 0;
		}
		int key = LuaHashing.ByteHash(Luau.lua_namecallatom(L, null));
		IntPtr ptr;
		if (classInfo.FunctionList.TryGetValue(key, out ptr))
		{
			FunctionPointer<lua_CFunction> functionPointer = new FunctionPointer<lua_CFunction>(ptr);
			return functionPointer.Invoke(L);
		}
		FixedString32Bytes fixedString32Bytes2 = "\"Function not found in function list\"";
		Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes2) + 2));
		return 0;
	}

	// Token: 0x040032C2 RID: 12994
	private static readonly FixedString32Bytes _k_metatableLookup = "metahash";

	// Token: 0x0200075C RID: 1884
	public enum EFieldTypes
	{
		// Token: 0x040032C4 RID: 12996
		Float,
		// Token: 0x040032C5 RID: 12997
		Int,
		// Token: 0x040032C6 RID: 12998
		Double,
		// Token: 0x040032C7 RID: 12999
		Bool,
		// Token: 0x040032C8 RID: 13000
		String,
		// Token: 0x040032C9 RID: 13001
		LightUserData
	}

	// Token: 0x0200075D RID: 1885
	[BurstCompile]
	public struct BurstFieldInfo
	{
		// Token: 0x040032CA RID: 13002
		public int NameHash;

		// Token: 0x040032CB RID: 13003
		public FixedString32Bytes Name;

		// Token: 0x040032CC RID: 13004
		public FixedString32Bytes MetatableName;

		// Token: 0x040032CD RID: 13005
		public int Offset;

		// Token: 0x040032CE RID: 13006
		public BurstClassInfo.EFieldTypes FieldType;

		// Token: 0x040032CF RID: 13007
		public int Size;
	}

	// Token: 0x0200075E RID: 1886
	[BurstCompile]
	public struct ClassInfo
	{
		// Token: 0x040032D0 RID: 13008
		public int NameHash;

		// Token: 0x040032D1 RID: 13009
		public FixedString32Bytes Name;

		// Token: 0x040032D2 RID: 13010
		public NativeHashMap<int, BurstClassInfo.BurstFieldInfo> FieldList;

		// Token: 0x040032D3 RID: 13011
		public NativeHashMap<int, IntPtr> FunctionList;
	}

	// Token: 0x0200075F RID: 1887
	public abstract class ClassList
	{
		// Token: 0x040032D4 RID: 13012
		public static readonly SharedStatic<NativeHashMap<int, BurstClassInfo.ClassInfo>> InfoFields = SharedStatic<NativeHashMap<int, BurstClassInfo.ClassInfo>>.GetOrCreateUnsafe(0U, -7258312696341931442L, -7445903157129162016L);

		// Token: 0x02000760 RID: 1888
		private class FieldKey
		{
		}

		// Token: 0x02000761 RID: 1889
		public static class MetatableNames<T>
		{
			// Token: 0x040032D5 RID: 13013
			public static FixedString32Bytes Name;
		}
	}

	// Token: 0x02000762 RID: 1890
	// (Invoke) Token: 0x06002E5C RID: 11868
	public unsafe delegate int Index_00002D3E$PostfixBurstDelegate(lua_State* L);

	// Token: 0x02000763 RID: 1891
	internal static class Index_00002D3E$BurstDirectCall
	{
		// Token: 0x06002E5F RID: 11871 RVA: 0x000E19C3 File Offset: 0x000DFBC3
		[BurstDiscard]
		private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
		{
			if (BurstClassInfo.Index_00002D3E$BurstDirectCall.Pointer == 0)
			{
				BurstClassInfo.Index_00002D3E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BurstClassInfo.Index_00002D3E$BurstDirectCall.DeferredCompilation, methodof(BurstClassInfo.Index$BurstManaged(lua_State*)).MethodHandle, typeof(BurstClassInfo.Index_00002D3E$PostfixBurstDelegate).TypeHandle);
			}
			A_0 = BurstClassInfo.Index_00002D3E$BurstDirectCall.Pointer;
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x000E19F0 File Offset: 0x000DFBF0
		private static IntPtr GetFunctionPointer()
		{
			IntPtr result = (IntPtr)0;
			BurstClassInfo.Index_00002D3E$BurstDirectCall.GetFunctionPointerDiscard(ref result);
			return result;
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x000E1A08 File Offset: 0x000DFC08
		public unsafe static void Constructor()
		{
			BurstClassInfo.Index_00002D3E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BurstClassInfo.Index(lua_State*)).MethodHandle);
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x000023F4 File Offset: 0x000005F4
		public static void Initialize()
		{
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x000E1A19 File Offset: 0x000DFC19
		// Note: this type is marked as 'beforefieldinit'.
		static Index_00002D3E$BurstDirectCall()
		{
			BurstClassInfo.Index_00002D3E$BurstDirectCall.Constructor();
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x000E1A20 File Offset: 0x000DFC20
		public unsafe static int Invoke(lua_State* L)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = BurstClassInfo.Index_00002D3E$BurstDirectCall.GetFunctionPointer();
				if (functionPointer != 0)
				{
					return calli(System.Int32(lua_State*), L, functionPointer);
				}
			}
			return BurstClassInfo.Index$BurstManaged(L);
		}

		// Token: 0x040032D6 RID: 13014
		private static IntPtr Pointer;

		// Token: 0x040032D7 RID: 13015
		private static IntPtr DeferredCompilation;
	}

	// Token: 0x02000764 RID: 1892
	// (Invoke) Token: 0x06002E66 RID: 11878
	public unsafe delegate int NewIndex_00002D3F$PostfixBurstDelegate(lua_State* L);

	// Token: 0x02000765 RID: 1893
	internal static class NewIndex_00002D3F$BurstDirectCall
	{
		// Token: 0x06002E69 RID: 11881 RVA: 0x000E1A51 File Offset: 0x000DFC51
		[BurstDiscard]
		private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
		{
			if (BurstClassInfo.NewIndex_00002D3F$BurstDirectCall.Pointer == 0)
			{
				BurstClassInfo.NewIndex_00002D3F$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BurstClassInfo.NewIndex_00002D3F$BurstDirectCall.DeferredCompilation, methodof(BurstClassInfo.NewIndex$BurstManaged(lua_State*)).MethodHandle, typeof(BurstClassInfo.NewIndex_00002D3F$PostfixBurstDelegate).TypeHandle);
			}
			A_0 = BurstClassInfo.NewIndex_00002D3F$BurstDirectCall.Pointer;
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x000E1A80 File Offset: 0x000DFC80
		private static IntPtr GetFunctionPointer()
		{
			IntPtr result = (IntPtr)0;
			BurstClassInfo.NewIndex_00002D3F$BurstDirectCall.GetFunctionPointerDiscard(ref result);
			return result;
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x000E1A98 File Offset: 0x000DFC98
		public unsafe static void Constructor()
		{
			BurstClassInfo.NewIndex_00002D3F$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BurstClassInfo.NewIndex(lua_State*)).MethodHandle);
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x000023F4 File Offset: 0x000005F4
		public static void Initialize()
		{
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x000E1AA9 File Offset: 0x000DFCA9
		// Note: this type is marked as 'beforefieldinit'.
		static NewIndex_00002D3F$BurstDirectCall()
		{
			BurstClassInfo.NewIndex_00002D3F$BurstDirectCall.Constructor();
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000E1AB0 File Offset: 0x000DFCB0
		public unsafe static int Invoke(lua_State* L)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = BurstClassInfo.NewIndex_00002D3F$BurstDirectCall.GetFunctionPointer();
				if (functionPointer != 0)
				{
					return calli(System.Int32(lua_State*), L, functionPointer);
				}
			}
			return BurstClassInfo.NewIndex$BurstManaged(L);
		}

		// Token: 0x040032D8 RID: 13016
		private static IntPtr Pointer;

		// Token: 0x040032D9 RID: 13017
		private static IntPtr DeferredCompilation;
	}

	// Token: 0x02000766 RID: 1894
	// (Invoke) Token: 0x06002E70 RID: 11888
	public unsafe delegate int NameCall_00002D40$PostfixBurstDelegate(lua_State* L);

	// Token: 0x02000767 RID: 1895
	internal static class NameCall_00002D40$BurstDirectCall
	{
		// Token: 0x06002E73 RID: 11891 RVA: 0x000E1AE1 File Offset: 0x000DFCE1
		[BurstDiscard]
		private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
		{
			if (BurstClassInfo.NameCall_00002D40$BurstDirectCall.Pointer == 0)
			{
				BurstClassInfo.NameCall_00002D40$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BurstClassInfo.NameCall_00002D40$BurstDirectCall.DeferredCompilation, methodof(BurstClassInfo.NameCall$BurstManaged(lua_State*)).MethodHandle, typeof(BurstClassInfo.NameCall_00002D40$PostfixBurstDelegate).TypeHandle);
			}
			A_0 = BurstClassInfo.NameCall_00002D40$BurstDirectCall.Pointer;
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x000E1B10 File Offset: 0x000DFD10
		private static IntPtr GetFunctionPointer()
		{
			IntPtr result = (IntPtr)0;
			BurstClassInfo.NameCall_00002D40$BurstDirectCall.GetFunctionPointerDiscard(ref result);
			return result;
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000E1B28 File Offset: 0x000DFD28
		public unsafe static void Constructor()
		{
			BurstClassInfo.NameCall_00002D40$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BurstClassInfo.NameCall(lua_State*)).MethodHandle);
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000023F4 File Offset: 0x000005F4
		public static void Initialize()
		{
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000E1B39 File Offset: 0x000DFD39
		// Note: this type is marked as 'beforefieldinit'.
		static NameCall_00002D40$BurstDirectCall()
		{
			BurstClassInfo.NameCall_00002D40$BurstDirectCall.Constructor();
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x000E1B40 File Offset: 0x000DFD40
		public unsafe static int Invoke(lua_State* L)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = BurstClassInfo.NameCall_00002D40$BurstDirectCall.GetFunctionPointer();
				if (functionPointer != 0)
				{
					return calli(System.Int32(lua_State*), L, functionPointer);
				}
			}
			return BurstClassInfo.NameCall$BurstManaged(L);
		}

		// Token: 0x040032DA RID: 13018
		private static IntPtr Pointer;

		// Token: 0x040032DB RID: 13019
		private static IntPtr DeferredCompilation;
	}
}
