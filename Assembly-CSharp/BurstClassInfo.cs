using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000772 RID: 1906
[BurstCompile]
public static class BurstClassInfo
{
	// Token: 0x06002EED RID: 12013 RVA: 0x0012AF64 File Offset: 0x00129164
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

	// Token: 0x06002EEE RID: 12014 RVA: 0x0004F7E6 File Offset: 0x0004D9E6
	[BurstCompile]
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int Index(lua_State* L)
	{
		return BurstClassInfo.Index_00002DDB$BurstDirectCall.Invoke(L);
	}

	// Token: 0x06002EEF RID: 12015 RVA: 0x0004F7EE File Offset: 0x0004D9EE
	[BurstCompile]
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int NewIndex(lua_State* L)
	{
		return BurstClassInfo.NewIndex_00002DDC$BurstDirectCall.Invoke(L);
	}

	// Token: 0x06002EF0 RID: 12016 RVA: 0x0004F7F6 File Offset: 0x0004D9F6
	[BurstCompile]
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int NameCall(lua_State* L)
	{
		return BurstClassInfo.NameCall_00002DDD$BurstDirectCall.Invoke(L);
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x0012B298 File Offset: 0x00129498
	[BurstCompile]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int Index$BurstManaged(lua_State* L)
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
		Luau.lua_pop(L, 1);
		byte* tname = (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref classInfo.Name) + 2;
		IntPtr pointer = IntPtr.Zero;
		Luau.lua_Types lua_Types = (Luau.lua_Types)Luau.lua_type(L, 1);
		if (lua_Types == Luau.lua_Types.LUA_TUSERDATA)
		{
			pointer = (IntPtr)Luau.luaL_checkudata(L, 1, tname);
		}
		else
		{
			if (lua_Types != Luau.lua_Types.LUA_TTABLE)
			{
				FixedString32Bytes fixedString32Bytes2 = "\"Unknown type for __index\"";
				Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes2) + 2));
				return 0;
			}
			pointer = Luau.lua_light_ptr(L, 1);
		}
		int len = Luau.lua_objlen(L, 2);
		int key = LuaHashing.ByteHash(Luau.luaL_checkstring(L, 2), len);
		BurstClassInfo.BurstFieldInfo burstFieldInfo;
		if (classInfo.FieldList.TryGetValue(key, out burstFieldInfo))
		{
			IntPtr intPtr = pointer + burstFieldInfo.Offset;
			switch (burstFieldInfo.FieldType)
			{
			case BurstClassInfo.EFieldTypes.Float:
				Luau.lua_pushnumber(L, (double)(*(float*)((void*)intPtr)));
				return 1;
			case BurstClassInfo.EFieldTypes.Int:
				Luau.lua_pushnumber(L, (double)(*(int*)((void*)intPtr)));
				return 1;
			case BurstClassInfo.EFieldTypes.Double:
				Luau.lua_pushnumber(L, *(double*)((void*)intPtr));
				return 1;
			case BurstClassInfo.EFieldTypes.Bool:
				Luau.lua_pushboolean(L, (*(byte*)((void*)intPtr) != 0) ? 1 : 0);
				return 1;
			case BurstClassInfo.EFieldTypes.String:
				Luau.lua_pushstring(L, (byte*)((void*)intPtr) + 2);
				return 1;
			case BurstClassInfo.EFieldTypes.LightUserData:
				Luau.lua_class_push(L, burstFieldInfo.MetatableName, intPtr);
				return 1;
			}
		}
		IntPtr ptr;
		if (classInfo.FunctionList.TryGetValue(key, out ptr))
		{
			FunctionPointer<lua_CFunction> fn = new FunctionPointer<lua_CFunction>(ptr);
			FixedString32Bytes fixedString32Bytes3 = "";
			Luau.lua_pushcclosurek(L, fn, (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes3) + 2, 0, null);
			return 1;
		}
		FixedString32Bytes fixedString32Bytes4 = "\"Unknown Type?\"";
		Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes4) + 2));
		return 0;
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x0012B48C File Offset: 0x0012968C
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
		Luau.lua_pop(L, 1);
		byte* tname = (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref classInfo.Name) + 2;
		IntPtr pointer = IntPtr.Zero;
		Luau.lua_Types lua_Types = (Luau.lua_Types)Luau.lua_type(L, 1);
		if (lua_Types == Luau.lua_Types.LUA_TUSERDATA)
		{
			pointer = (IntPtr)Luau.luaL_checkudata(L, 1, tname);
		}
		else
		{
			if (lua_Types != Luau.lua_Types.LUA_TTABLE)
			{
				FixedString32Bytes fixedString32Bytes2 = "\"Unknown type for __newindex\"";
				Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes2) + 2));
				return 0;
			}
			pointer = Luau.lua_light_ptr(L, 1);
		}
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
				Buffer.MemoryCopy((void*)((IntPtr)((void*)Luau.lua_class_get(L, 3, burstFieldInfo.MetatableName))), (void*)value, (long)burstFieldInfo.Size, (long)burstFieldInfo.Size);
				return 0;
			}
		}
		FixedString32Bytes fixedString32Bytes3 = "\"Unknown Type\"";
		Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes3) + 2));
		return 0;
	}

	// Token: 0x06002EF4 RID: 12020 RVA: 0x0012B65C File Offset: 0x0012985C
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
		Luau.lua_pop(L, 1);
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

	// Token: 0x04003361 RID: 13153
	private static readonly FixedString32Bytes _k_metatableLookup = "metahash";

	// Token: 0x02000773 RID: 1907
	public enum EFieldTypes
	{
		// Token: 0x04003363 RID: 13155
		Float,
		// Token: 0x04003364 RID: 13156
		Int,
		// Token: 0x04003365 RID: 13157
		Double,
		// Token: 0x04003366 RID: 13158
		Bool,
		// Token: 0x04003367 RID: 13159
		String,
		// Token: 0x04003368 RID: 13160
		LightUserData
	}

	// Token: 0x02000774 RID: 1908
	[BurstCompile]
	public struct BurstFieldInfo
	{
		// Token: 0x04003369 RID: 13161
		public int NameHash;

		// Token: 0x0400336A RID: 13162
		public FixedString32Bytes Name;

		// Token: 0x0400336B RID: 13163
		public FixedString32Bytes MetatableName;

		// Token: 0x0400336C RID: 13164
		public int Offset;

		// Token: 0x0400336D RID: 13165
		public BurstClassInfo.EFieldTypes FieldType;

		// Token: 0x0400336E RID: 13166
		public int Size;
	}

	// Token: 0x02000775 RID: 1909
	[BurstCompile]
	public struct ClassInfo
	{
		// Token: 0x0400336F RID: 13167
		public int NameHash;

		// Token: 0x04003370 RID: 13168
		public FixedString32Bytes Name;

		// Token: 0x04003371 RID: 13169
		public NativeHashMap<int, BurstClassInfo.BurstFieldInfo> FieldList;

		// Token: 0x04003372 RID: 13170
		public NativeHashMap<int, IntPtr> FunctionList;
	}

	// Token: 0x02000776 RID: 1910
	public abstract class ClassList
	{
		// Token: 0x04003373 RID: 13171
		public static readonly SharedStatic<NativeHashMap<int, BurstClassInfo.ClassInfo>> InfoFields = SharedStatic<NativeHashMap<int, BurstClassInfo.ClassInfo>>.GetOrCreateUnsafe(0U, -7258312696341931442L, -7445903157129162016L);

		// Token: 0x02000777 RID: 1911
		private class FieldKey
		{
		}

		// Token: 0x02000778 RID: 1912
		public static class MetatableNames<T>
		{
			// Token: 0x04003374 RID: 13172
			public static FixedString32Bytes Name;
		}
	}

	// Token: 0x02000779 RID: 1913
	// (Invoke) Token: 0x06002EF9 RID: 12025
	public unsafe delegate int Index_00002DDB$PostfixBurstDelegate(lua_State* L);

	// Token: 0x0200077A RID: 1914
	internal static class Index_00002DDB$BurstDirectCall
	{
		// Token: 0x06002EFC RID: 12028 RVA: 0x0004F82E File Offset: 0x0004DA2E
		[BurstDiscard]
		private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
		{
			if (BurstClassInfo.Index_00002DDB$BurstDirectCall.Pointer == 0)
			{
				BurstClassInfo.Index_00002DDB$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BurstClassInfo.Index_00002DDB$BurstDirectCall.DeferredCompilation, methodof(BurstClassInfo.Index$BurstManaged(lua_State*)).MethodHandle, typeof(BurstClassInfo.Index_00002DDB$PostfixBurstDelegate).TypeHandle);
			}
			A_0 = BurstClassInfo.Index_00002DDB$BurstDirectCall.Pointer;
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x0012B714 File Offset: 0x00129914
		private static IntPtr GetFunctionPointer()
		{
			IntPtr result = (IntPtr)0;
			BurstClassInfo.Index_00002DDB$BurstDirectCall.GetFunctionPointerDiscard(ref result);
			return result;
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x0004F85A File Offset: 0x0004DA5A
		public unsafe static void Constructor()
		{
			BurstClassInfo.Index_00002DDB$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BurstClassInfo.Index(lua_State*)).MethodHandle);
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x00030607 File Offset: 0x0002E807
		public static void Initialize()
		{
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x0004F86B File Offset: 0x0004DA6B
		// Note: this type is marked as 'beforefieldinit'.
		static Index_00002DDB$BurstDirectCall()
		{
			BurstClassInfo.Index_00002DDB$BurstDirectCall.Constructor();
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x0012B72C File Offset: 0x0012992C
		public unsafe static int Invoke(lua_State* L)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = BurstClassInfo.Index_00002DDB$BurstDirectCall.GetFunctionPointer();
				if (functionPointer != 0)
				{
					return calli(System.Int32(lua_State*), L, functionPointer);
				}
			}
			return BurstClassInfo.Index$BurstManaged(L);
		}

		// Token: 0x04003375 RID: 13173
		private static IntPtr Pointer;

		// Token: 0x04003376 RID: 13174
		private static IntPtr DeferredCompilation;
	}

	// Token: 0x0200077B RID: 1915
	// (Invoke) Token: 0x06002F03 RID: 12035
	public unsafe delegate int NewIndex_00002DDC$PostfixBurstDelegate(lua_State* L);

	// Token: 0x0200077C RID: 1916
	internal static class NewIndex_00002DDC$BurstDirectCall
	{
		// Token: 0x06002F06 RID: 12038 RVA: 0x0004F872 File Offset: 0x0004DA72
		[BurstDiscard]
		private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
		{
			if (BurstClassInfo.NewIndex_00002DDC$BurstDirectCall.Pointer == 0)
			{
				BurstClassInfo.NewIndex_00002DDC$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BurstClassInfo.NewIndex_00002DDC$BurstDirectCall.DeferredCompilation, methodof(BurstClassInfo.NewIndex$BurstManaged(lua_State*)).MethodHandle, typeof(BurstClassInfo.NewIndex_00002DDC$PostfixBurstDelegate).TypeHandle);
			}
			A_0 = BurstClassInfo.NewIndex_00002DDC$BurstDirectCall.Pointer;
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x0012B760 File Offset: 0x00129960
		private static IntPtr GetFunctionPointer()
		{
			IntPtr result = (IntPtr)0;
			BurstClassInfo.NewIndex_00002DDC$BurstDirectCall.GetFunctionPointerDiscard(ref result);
			return result;
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x0004F89E File Offset: 0x0004DA9E
		public unsafe static void Constructor()
		{
			BurstClassInfo.NewIndex_00002DDC$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BurstClassInfo.NewIndex(lua_State*)).MethodHandle);
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x00030607 File Offset: 0x0002E807
		public static void Initialize()
		{
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x0004F8AF File Offset: 0x0004DAAF
		// Note: this type is marked as 'beforefieldinit'.
		static NewIndex_00002DDC$BurstDirectCall()
		{
			BurstClassInfo.NewIndex_00002DDC$BurstDirectCall.Constructor();
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x0012B778 File Offset: 0x00129978
		public unsafe static int Invoke(lua_State* L)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = BurstClassInfo.NewIndex_00002DDC$BurstDirectCall.GetFunctionPointer();
				if (functionPointer != 0)
				{
					return calli(System.Int32(lua_State*), L, functionPointer);
				}
			}
			return BurstClassInfo.NewIndex$BurstManaged(L);
		}

		// Token: 0x04003377 RID: 13175
		private static IntPtr Pointer;

		// Token: 0x04003378 RID: 13176
		private static IntPtr DeferredCompilation;
	}

	// Token: 0x0200077D RID: 1917
	// (Invoke) Token: 0x06002F0D RID: 12045
	public unsafe delegate int NameCall_00002DDD$PostfixBurstDelegate(lua_State* L);

	// Token: 0x0200077E RID: 1918
	internal static class NameCall_00002DDD$BurstDirectCall
	{
		// Token: 0x06002F10 RID: 12048 RVA: 0x0004F8B6 File Offset: 0x0004DAB6
		[BurstDiscard]
		private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
		{
			if (BurstClassInfo.NameCall_00002DDD$BurstDirectCall.Pointer == 0)
			{
				BurstClassInfo.NameCall_00002DDD$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BurstClassInfo.NameCall_00002DDD$BurstDirectCall.DeferredCompilation, methodof(BurstClassInfo.NameCall$BurstManaged(lua_State*)).MethodHandle, typeof(BurstClassInfo.NameCall_00002DDD$PostfixBurstDelegate).TypeHandle);
			}
			A_0 = BurstClassInfo.NameCall_00002DDD$BurstDirectCall.Pointer;
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x0012B7AC File Offset: 0x001299AC
		private static IntPtr GetFunctionPointer()
		{
			IntPtr result = (IntPtr)0;
			BurstClassInfo.NameCall_00002DDD$BurstDirectCall.GetFunctionPointerDiscard(ref result);
			return result;
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x0004F8E2 File Offset: 0x0004DAE2
		public unsafe static void Constructor()
		{
			BurstClassInfo.NameCall_00002DDD$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BurstClassInfo.NameCall(lua_State*)).MethodHandle);
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x00030607 File Offset: 0x0002E807
		public static void Initialize()
		{
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x0004F8F3 File Offset: 0x0004DAF3
		// Note: this type is marked as 'beforefieldinit'.
		static NameCall_00002DDD$BurstDirectCall()
		{
			BurstClassInfo.NameCall_00002DDD$BurstDirectCall.Constructor();
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x0012B7C4 File Offset: 0x001299C4
		public unsafe static int Invoke(lua_State* L)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = BurstClassInfo.NameCall_00002DDD$BurstDirectCall.GetFunctionPointer();
				if (functionPointer != 0)
				{
					return calli(System.Int32(lua_State*), L, functionPointer);
				}
			}
			return BurstClassInfo.NameCall$BurstManaged(L);
		}

		// Token: 0x04003379 RID: 13177
		private static IntPtr Pointer;

		// Token: 0x0400337A RID: 13178
		private static IntPtr DeferredCompilation;
	}
}
