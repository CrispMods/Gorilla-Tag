using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000769 RID: 1897
public class LuauClassBuilder<[IsUnmanaged] T> where T : struct, ValueType
{
	// Token: 0x06002E7D RID: 11901 RVA: 0x000E1CAC File Offset: 0x000DFEAC
	public LuauClassBuilder(string className)
	{
		this._className = className;
		this._classType = typeof(T);
	}

	// Token: 0x06002E7E RID: 11902 RVA: 0x000E1D24 File Offset: 0x000DFF24
	public LuauClassBuilder<T> AddField(string luaName, string fieldName = null)
	{
		if (fieldName == null)
		{
			fieldName = luaName;
		}
		FieldInfo field = typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
		if (field == null)
		{
			throw new ArgumentException(string.Concat(new string[]
			{
				"Property ",
				fieldName,
				" does not exist on type ",
				typeof(T).Name,
				"."
			}));
		}
		this._classFields.TryAdd(LuaHashing.ByteHash(luaName), field);
		return this;
	}

	// Token: 0x06002E7F RID: 11903 RVA: 0x000E1DA6 File Offset: 0x000DFFA6
	public LuauClassBuilder<T> AddStaticFunction(string luaName, lua_CFunction function)
	{
		this._staticFunctions.TryAdd(luaName, function);
		return this;
	}

	// Token: 0x06002E80 RID: 11904 RVA: 0x000E1DB7 File Offset: 0x000DFFB7
	public LuauClassBuilder<T> AddStaticFunction(string luaName, FunctionPointer<lua_CFunction> function)
	{
		this._staticFunctionPtrs.TryAdd(luaName, function);
		return this;
	}

	// Token: 0x06002E81 RID: 11905 RVA: 0x000E1DC8 File Offset: 0x000DFFC8
	public LuauClassBuilder<T> AddProperty(string luaName, lua_CFunction function)
	{
		this._properties.TryAdd(luaName, function);
		return this;
	}

	// Token: 0x06002E82 RID: 11906 RVA: 0x000E1DD9 File Offset: 0x000DFFD9
	public LuauClassBuilder<T> AddProperty(string luaName, FunctionPointer<lua_CFunction> function)
	{
		this._propertyPtrs.TryAdd(luaName, function);
		return this;
	}

	// Token: 0x06002E83 RID: 11907 RVA: 0x000E1DEA File Offset: 0x000DFFEA
	public LuauClassBuilder<T> AddFunction(string luaName, lua_CFunction function)
	{
		if (luaName.StartsWith("__"))
		{
			this._staticFunctions.TryAdd(luaName, function);
		}
		this._functions.TryAdd(LuaHashing.ByteHash(luaName), function);
		return this;
	}

	// Token: 0x06002E84 RID: 11908 RVA: 0x000E1E1B File Offset: 0x000E001B
	public LuauClassBuilder<T> AddFunction(string luaName, FunctionPointer<lua_CFunction> function)
	{
		if (luaName.StartsWith("__"))
		{
			this._staticFunctionPtrs.TryAdd(luaName, function);
		}
		this._functionPtrs.TryAdd(LuaHashing.ByteHash(luaName), function);
		return this;
	}

	// Token: 0x06002E85 RID: 11909 RVA: 0x000E1E4C File Offset: 0x000E004C
	public unsafe LuauClassBuilder<T> Build(lua_State* L, bool global)
	{
		BurstClassInfo.NewClass<T>(this._className, this._classFields, this._functions, this._functionPtrs);
		Luau.luaL_newmetatable(L, this._className);
		FunctionPointer<lua_CFunction> fn = BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(BurstClassInfo.Index));
		Luau.lua_pushcfunction(L, fn, null);
		Luau.lua_setfield(L, -2, "__index");
		FunctionPointer<lua_CFunction> fn2 = BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(BurstClassInfo.NameCall));
		Luau.lua_pushcfunction(L, fn2, null);
		Luau.lua_setfield(L, -2, "__namecall");
		FunctionPointer<lua_CFunction> fn3 = BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(BurstClassInfo.NewIndex));
		Luau.lua_pushcfunction(L, fn3, null);
		Luau.lua_setfield(L, -2, "__newindex");
		foreach (KeyValuePair<string, lua_CFunction> keyValuePair in this._staticFunctions)
		{
			Luau.lua_pushcfunction(L, keyValuePair.Value, keyValuePair.Key);
			Luau.lua_setfield(L, -2, keyValuePair.Key);
		}
		foreach (KeyValuePair<string, FunctionPointer<lua_CFunction>> keyValuePair2 in this._staticFunctionPtrs)
		{
			Luau.lua_pushcfunction(L, keyValuePair2.Value, keyValuePair2.Key);
			Luau.lua_setfield(L, -2, keyValuePair2.Key);
		}
		FixedString32Bytes fixedString32Bytes = "metahash";
		byte* k = (byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2;
		Luau.lua_pushnumber(L, (double)LuaHashing.ByteHash(this._className));
		Luau.lua_setfield(L, -2, k);
		Luau.lua_setreadonly(L, -1, 1);
		Luau.lua_pop(L, 1);
		if (global)
		{
			Luau.lua_createtable(L, 0, 0);
			foreach (KeyValuePair<string, lua_CFunction> keyValuePair3 in this._staticFunctions)
			{
				Luau.lua_pushcfunction(L, keyValuePair3.Value, keyValuePair3.Key);
				Luau.lua_setfield(L, -2, keyValuePair3.Key);
			}
			foreach (KeyValuePair<string, FunctionPointer<lua_CFunction>> keyValuePair4 in this._staticFunctionPtrs)
			{
				Luau.lua_pushcfunction(L, keyValuePair4.Value, keyValuePair4.Key);
				Luau.lua_setfield(L, -2, keyValuePair4.Key);
			}
			Luau.lua_pushnumber(L, (double)LuaHashing.ByteHash(this._className));
			Luau.lua_setfield(L, -2, k);
			Luau.luaL_getmetatable(L, this._className);
			Luau.lua_setmetatable(L, -2);
			Luau.lua_setglobal(L, this._className);
		}
		return this;
	}

	// Token: 0x040032DE RID: 13022
	private string _className;

	// Token: 0x040032DF RID: 13023
	private Type _classType;

	// Token: 0x040032E0 RID: 13024
	private Dictionary<string, lua_CFunction> _staticFunctions = new Dictionary<string, lua_CFunction>();

	// Token: 0x040032E1 RID: 13025
	private Dictionary<string, FunctionPointer<lua_CFunction>> _staticFunctionPtrs = new Dictionary<string, FunctionPointer<lua_CFunction>>();

	// Token: 0x040032E2 RID: 13026
	private Dictionary<int, FieldInfo> _classFields = new Dictionary<int, FieldInfo>();

	// Token: 0x040032E3 RID: 13027
	private Dictionary<string, lua_CFunction> _properties = new Dictionary<string, lua_CFunction>();

	// Token: 0x040032E4 RID: 13028
	private Dictionary<string, FunctionPointer<lua_CFunction>> _propertyPtrs = new Dictionary<string, FunctionPointer<lua_CFunction>>();

	// Token: 0x040032E5 RID: 13029
	private Dictionary<int, lua_CFunction> _functions = new Dictionary<int, lua_CFunction>();

	// Token: 0x040032E6 RID: 13030
	private Dictionary<int, FunctionPointer<lua_CFunction>> _functionPtrs = new Dictionary<int, FunctionPointer<lua_CFunction>>();
}
