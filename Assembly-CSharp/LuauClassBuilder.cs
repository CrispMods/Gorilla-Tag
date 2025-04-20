using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000780 RID: 1920
public class LuauClassBuilder<[IsUnmanaged] T> where T : struct, ValueType
{
	// Token: 0x06002F1A RID: 12058 RVA: 0x0012B930 File Offset: 0x00129B30
	public LuauClassBuilder(string className)
	{
		this._className = className;
		this._classType = typeof(T);
	}

	// Token: 0x06002F1B RID: 12059 RVA: 0x0012B9A8 File Offset: 0x00129BA8
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

	// Token: 0x06002F1C RID: 12060 RVA: 0x0004F8FA File Offset: 0x0004DAFA
	public LuauClassBuilder<T> AddStaticFunction(string luaName, lua_CFunction function)
	{
		this._staticFunctions.TryAdd(luaName, function);
		return this;
	}

	// Token: 0x06002F1D RID: 12061 RVA: 0x0004F90B File Offset: 0x0004DB0B
	public LuauClassBuilder<T> AddStaticFunction(string luaName, FunctionPointer<lua_CFunction> function)
	{
		this._staticFunctionPtrs.TryAdd(luaName, function);
		return this;
	}

	// Token: 0x06002F1E RID: 12062 RVA: 0x0004F91C File Offset: 0x0004DB1C
	public LuauClassBuilder<T> AddProperty(string luaName, lua_CFunction function)
	{
		this._properties.TryAdd(luaName, function);
		return this;
	}

	// Token: 0x06002F1F RID: 12063 RVA: 0x0004F92D File Offset: 0x0004DB2D
	public LuauClassBuilder<T> AddProperty(string luaName, FunctionPointer<lua_CFunction> function)
	{
		this._propertyPtrs.TryAdd(luaName, function);
		return this;
	}

	// Token: 0x06002F20 RID: 12064 RVA: 0x0004F93E File Offset: 0x0004DB3E
	public LuauClassBuilder<T> AddFunction(string luaName, lua_CFunction function)
	{
		if (luaName.StartsWith("__"))
		{
			this._staticFunctions.TryAdd(luaName, function);
		}
		this._functions.TryAdd(LuaHashing.ByteHash(luaName), function);
		return this;
	}

	// Token: 0x06002F21 RID: 12065 RVA: 0x0004F96F File Offset: 0x0004DB6F
	public LuauClassBuilder<T> AddFunction(string luaName, FunctionPointer<lua_CFunction> function)
	{
		if (luaName.StartsWith("__"))
		{
			this._staticFunctionPtrs.TryAdd(luaName, function);
		}
		this._functionPtrs.TryAdd(LuaHashing.ByteHash(luaName), function);
		return this;
	}

	// Token: 0x06002F22 RID: 12066 RVA: 0x0012BA2C File Offset: 0x00129C2C
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

	// Token: 0x0400337D RID: 13181
	private string _className;

	// Token: 0x0400337E RID: 13182
	private Type _classType;

	// Token: 0x0400337F RID: 13183
	private Dictionary<string, lua_CFunction> _staticFunctions = new Dictionary<string, lua_CFunction>();

	// Token: 0x04003380 RID: 13184
	private Dictionary<string, FunctionPointer<lua_CFunction>> _staticFunctionPtrs = new Dictionary<string, FunctionPointer<lua_CFunction>>();

	// Token: 0x04003381 RID: 13185
	private Dictionary<int, FieldInfo> _classFields = new Dictionary<int, FieldInfo>();

	// Token: 0x04003382 RID: 13186
	private Dictionary<string, lua_CFunction> _properties = new Dictionary<string, lua_CFunction>();

	// Token: 0x04003383 RID: 13187
	private Dictionary<string, FunctionPointer<lua_CFunction>> _propertyPtrs = new Dictionary<string, FunctionPointer<lua_CFunction>>();

	// Token: 0x04003384 RID: 13188
	private Dictionary<int, lua_CFunction> _functions = new Dictionary<int, lua_CFunction>();

	// Token: 0x04003385 RID: 13189
	private Dictionary<int, FunctionPointer<lua_CFunction>> _functionPtrs = new Dictionary<int, FunctionPointer<lua_CFunction>>();
}
