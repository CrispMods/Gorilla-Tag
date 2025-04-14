using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x0200075C RID: 1884
[BurstCompile]
public static class BurstClassInfo
{
	// Token: 0x06002E58 RID: 11864 RVA: 0x000E16F4 File Offset: 0x000DF8F4
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

	// Token: 0x06002E59 RID: 11865 RVA: 0x000E1A28 File Offset: 0x000DFC28
	[BurstCompile]
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int Index(lua_State* L)
	{
		return BurstClassInfo.Index_00002D46$BurstDirectCall.Invoke(L);
	}

	// Token: 0x06002E5A RID: 11866 RVA: 0x000E1A30 File Offset: 0x000DFC30
	[BurstCompile]
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int NewIndex(lua_State* L)
	{
		return BurstClassInfo.NewIndex_00002D47$BurstDirectCall.Invoke(L);
	}

	// Token: 0x06002E5B RID: 11867 RVA: 0x000E1A38 File Offset: 0x000DFC38
	[BurstCompile]
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int NameCall(lua_State* L)
	{
		return BurstClassInfo.NameCall_00002D48$BurstDirectCall.Invoke(L);
	}

	// Token: 0x06002E5D RID: 11869 RVA: 0x000E1A54 File Offset: 0x000DFC54
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

	// Token: 0x06002E5E RID: 11870 RVA: 0x000E1BE8 File Offset: 0x000DFDE8
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

	// Token: 0x06002E5F RID: 11871 RVA: 0x000E1D74 File Offset: 0x000DFF74
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

	// Token: 0x040032C8 RID: 13000
	private static readonly FixedString32Bytes _k_metatableLookup = "metahash";

	// Token: 0x0200075D RID: 1885
	public enum EFieldTypes
	{
		// Token: 0x040032CA RID: 13002
		Float,
		// Token: 0x040032CB RID: 13003
		Int,
		// Token: 0x040032CC RID: 13004
		Double,
		// Token: 0x040032CD RID: 13005
		Bool,
		// Token: 0x040032CE RID: 13006
		String,
		// Token: 0x040032CF RID: 13007
		LightUserData
	}

	// Token: 0x0200075E RID: 1886
	[BurstCompile]
	public struct BurstFieldInfo
	{
		// Token: 0x040032D0 RID: 13008
		public int NameHash;

		// Token: 0x040032D1 RID: 13009
		public FixedString32Bytes Name;

		// Token: 0x040032D2 RID: 13010
		public FixedString32Bytes MetatableName;

		// Token: 0x040032D3 RID: 13011
		public int Offset;

		// Token: 0x040032D4 RID: 13012
		public BurstClassInfo.EFieldTypes FieldType;

		// Token: 0x040032D5 RID: 13013
		public int Size;
	}

	// Token: 0x0200075F RID: 1887
	[BurstCompile]
	public struct ClassInfo
	{
		// Token: 0x040032D6 RID: 13014
		public int NameHash;

		// Token: 0x040032D7 RID: 13015
		public FixedString32Bytes Name;

		// Token: 0x040032D8 RID: 13016
		public NativeHashMap<int, BurstClassInfo.BurstFieldInfo> FieldList;

		// Token: 0x040032D9 RID: 13017
		public NativeHashMap<int, IntPtr> FunctionList;
	}

	// Token: 0x02000760 RID: 1888
	public abstract class ClassList
	{
		// Token: 0x040032DA RID: 13018
		public static readonly SharedStatic<NativeHashMap<int, BurstClassInfo.ClassInfo>> InfoFields = SharedStatic<NativeHashMap<int, BurstClassInfo.ClassInfo>>.GetOrCreateUnsafe(0U, -7258312696341931442L, -7445903157129162016L);

		// Token: 0x02000761 RID: 1889
		private class FieldKey
		{
		}

		// Token: 0x02000762 RID: 1890
		public static class MetatableNames<T>
		{
			// Token: 0x040032DB RID: 13019
			public static FixedString32Bytes Name;
		}
	}

	// Token: 0x02000763 RID: 1891
	// (Invoke) Token: 0x06002E64 RID: 11876
	public unsafe delegate int Index_00002D46$PostfixBurstDelegate(lua_State* L);

	// Token: 0x02000764 RID: 1892
	internal static class Index_00002D46$BurstDirectCall
	{
		// Token: 0x06002E67 RID: 11879 RVA: 0x000E1E43 File Offset: 0x000E0043
		[BurstDiscard]
		private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
		{
			if (BurstClassInfo.Index_00002D46$BurstDirectCall.Pointer == 0)
			{
				BurstClassInfo.Index_00002D46$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BurstClassInfo.Index_00002D46$BurstDirectCall.DeferredCompilation, methodof(BurstClassInfo.Index$BurstManaged(lua_State*)).MethodHandle, typeof(BurstClassInfo.Index_00002D46$PostfixBurstDelegate).TypeHandle);
			}
			A_0 = BurstClassInfo.Index_00002D46$BurstDirectCall.Pointer;
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x000E1E70 File Offset: 0x000E0070
		private static IntPtr GetFunctionPointer()
		{
			IntPtr result = (IntPtr)0;
			BurstClassInfo.Index_00002D46$BurstDirectCall.GetFunctionPointerDiscard(ref result);
			return result;
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x000E1E88 File Offset: 0x000E0088
		public unsafe static void Constructor()
		{
			BurstClassInfo.Index_00002D46$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BurstClassInfo.Index(lua_State*)).MethodHandle);
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x000023F4 File Offset: 0x000005F4
		public static void Initialize()
		{
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x000E1E99 File Offset: 0x000E0099
		// Note: this type is marked as 'beforefieldinit'.
		static Index_00002D46$BurstDirectCall()
		{
			BurstClassInfo.Index_00002D46$BurstDirectCall.Constructor();
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x000E1EA0 File Offset: 0x000E00A0
		public unsafe static int Invoke(lua_State* L)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = BurstClassInfo.Index_00002D46$BurstDirectCall.GetFunctionPointer();
				if (functionPointer != 0)
				{
					return calli(System.Int32(lua_State*), L, functionPointer);
				}
			}
			return BurstClassInfo.Index$BurstManaged(L);
		}

		// Token: 0x040032DC RID: 13020
		private static IntPtr Pointer;

		// Token: 0x040032DD RID: 13021
		private static IntPtr DeferredCompilation;
	}

	// Token: 0x02000765 RID: 1893
	// (Invoke) Token: 0x06002E6E RID: 11886
	public unsafe delegate int NewIndex_00002D47$PostfixBurstDelegate(lua_State* L);

	// Token: 0x02000766 RID: 1894
	internal static class NewIndex_00002D47$BurstDirectCall
	{
		// Token: 0x06002E71 RID: 11889 RVA: 0x000E1ED1 File Offset: 0x000E00D1
		[BurstDiscard]
		private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
		{
			if (BurstClassInfo.NewIndex_00002D47$BurstDirectCall.Pointer == 0)
			{
				BurstClassInfo.NewIndex_00002D47$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BurstClassInfo.NewIndex_00002D47$BurstDirectCall.DeferredCompilation, methodof(BurstClassInfo.NewIndex$BurstManaged(lua_State*)).MethodHandle, typeof(BurstClassInfo.NewIndex_00002D47$PostfixBurstDelegate).TypeHandle);
			}
			A_0 = BurstClassInfo.NewIndex_00002D47$BurstDirectCall.Pointer;
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x000E1F00 File Offset: 0x000E0100
		private static IntPtr GetFunctionPointer()
		{
			IntPtr result = (IntPtr)0;
			BurstClassInfo.NewIndex_00002D47$BurstDirectCall.GetFunctionPointerDiscard(ref result);
			return result;
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x000E1F18 File Offset: 0x000E0118
		public unsafe static void Constructor()
		{
			BurstClassInfo.NewIndex_00002D47$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BurstClassInfo.NewIndex(lua_State*)).MethodHandle);
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x000023F4 File Offset: 0x000005F4
		public static void Initialize()
		{
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000E1F29 File Offset: 0x000E0129
		// Note: this type is marked as 'beforefieldinit'.
		static NewIndex_00002D47$BurstDirectCall()
		{
			BurstClassInfo.NewIndex_00002D47$BurstDirectCall.Constructor();
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000E1F30 File Offset: 0x000E0130
		public unsafe static int Invoke(lua_State* L)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = BurstClassInfo.NewIndex_00002D47$BurstDirectCall.GetFunctionPointer();
				if (functionPointer != 0)
				{
					return calli(System.Int32(lua_State*), L, functionPointer);
				}
			}
			return BurstClassInfo.NewIndex$BurstManaged(L);
		}

		// Token: 0x040032DE RID: 13022
		private static IntPtr Pointer;

		// Token: 0x040032DF RID: 13023
		private static IntPtr DeferredCompilation;
	}

	// Token: 0x02000767 RID: 1895
	// (Invoke) Token: 0x06002E78 RID: 11896
	public unsafe delegate int NameCall_00002D48$PostfixBurstDelegate(lua_State* L);

	// Token: 0x02000768 RID: 1896
	internal static class NameCall_00002D48$BurstDirectCall
	{
		// Token: 0x06002E7B RID: 11899 RVA: 0x000E1F61 File Offset: 0x000E0161
		[BurstDiscard]
		private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
		{
			if (BurstClassInfo.NameCall_00002D48$BurstDirectCall.Pointer == 0)
			{
				BurstClassInfo.NameCall_00002D48$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BurstClassInfo.NameCall_00002D48$BurstDirectCall.DeferredCompilation, methodof(BurstClassInfo.NameCall$BurstManaged(lua_State*)).MethodHandle, typeof(BurstClassInfo.NameCall_00002D48$PostfixBurstDelegate).TypeHandle);
			}
			A_0 = BurstClassInfo.NameCall_00002D48$BurstDirectCall.Pointer;
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x000E1F90 File Offset: 0x000E0190
		private static IntPtr GetFunctionPointer()
		{
			IntPtr result = (IntPtr)0;
			BurstClassInfo.NameCall_00002D48$BurstDirectCall.GetFunctionPointerDiscard(ref result);
			return result;
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x000E1FA8 File Offset: 0x000E01A8
		public unsafe static void Constructor()
		{
			BurstClassInfo.NameCall_00002D48$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BurstClassInfo.NameCall(lua_State*)).MethodHandle);
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x000023F4 File Offset: 0x000005F4
		public static void Initialize()
		{
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000E1FB9 File Offset: 0x000E01B9
		// Note: this type is marked as 'beforefieldinit'.
		static NameCall_00002D48$BurstDirectCall()
		{
			BurstClassInfo.NameCall_00002D48$BurstDirectCall.Constructor();
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x000E1FC0 File Offset: 0x000E01C0
		public unsafe static int Invoke(lua_State* L)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = BurstClassInfo.NameCall_00002D48$BurstDirectCall.GetFunctionPointer();
				if (functionPointer != 0)
				{
					return calli(System.Int32(lua_State*), L, functionPointer);
				}
			}
			return BurstClassInfo.NameCall$BurstManaged(L);
		}

		// Token: 0x040032E0 RID: 13024
		private static IntPtr Pointer;

		// Token: 0x040032E1 RID: 13025
		private static IntPtr DeferredCompilation;
	}
}
