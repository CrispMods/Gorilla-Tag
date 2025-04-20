using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02000790 RID: 1936
public class LuauScriptRunner
{
	// Token: 0x06002F99 RID: 12185 RVA: 0x0012CB10 File Offset: 0x0012AD10
	public unsafe static bool ErrorCheck(lua_State* L, int status)
	{
		if (status == 2)
		{
			sbyte* value = Luau.lua_tostring(L, -1);
			LuauHud.Instance.LuauLog(new string(value));
			sbyte* value2 = (sbyte*)Luau.lua_debugtrace(L);
			LuauHud.Instance.LuauLog(new string(value2));
			Luau.lua_close(L);
			return true;
		}
		return false;
	}

	// Token: 0x06002F9A RID: 12186 RVA: 0x0012CB5C File Offset: 0x0012AD5C
	public bool Tick(float deltaTime)
	{
		if (!this.ShouldTick)
		{
			return false;
		}
		Luau.lua_getfield(this.L, -10002, "tick");
		if (Luau.lua_type(this.L, -1) == 7)
		{
			this.preTickCallback(this.L);
			Luau.lua_pushnumber(this.L, (double)deltaTime);
			int status = Luau.lua_pcall(this.L, 1, 0, 0);
			this.ShouldTick = !LuauScriptRunner.ErrorCheck(this.L, status);
			if (this.ShouldTick)
			{
				this.postTickCallback(this.L);
			}
			return this.ShouldTick;
		}
		Luau.lua_pop(this.L, 1);
		return false;
	}

	// Token: 0x06002F9B RID: 12187 RVA: 0x0012CC0C File Offset: 0x0012AE0C
	public unsafe LuauScriptRunner(string script, string name, [CanBeNull] lua_CFunction bindings = null, [CanBeNull] lua_CFunction preTick = null, [CanBeNull] lua_CFunction postTick = null)
	{
		this.Script = script;
		this.ScriptName = name;
		this.L = Luau.luaL_newstate();
		LuauScriptRunner.ScriptRunners.Add(this);
		Luau.luaL_openlibs(this.L);
		Bindings.Vec3Builder(this.L);
		Bindings.QuatBuilder(this.L);
		if (bindings != null)
		{
			bindings(this.L);
		}
		this.postTickCallback = postTick;
		this.preTickCallback = preTick;
		UIntPtr size = (UIntPtr)((IntPtr)0);
		Luau.lua_register(this.L, new lua_CFunction(Luau.lua_print), "print");
		byte[] bytes = Encoding.UTF8.GetBytes(script);
		sbyte* data = Luau.luau_compile(script, (UIntPtr)((IntPtr)bytes.Length), null, &size);
		Luau.luau_load(this.L, name, data, size, 0);
		int status = Luau.lua_resume(this.L, null, 0);
		this.ShouldTick = !LuauScriptRunner.ErrorCheck(this.L, status);
	}

	// Token: 0x06002F9C RID: 12188 RVA: 0x0004FB21 File Offset: 0x0004DD21
	public LuauScriptRunner FromFile(string filePath, [CanBeNull] lua_CFunction bindings = null, [CanBeNull] lua_CFunction tick = null)
	{
		return new LuauScriptRunner(File.ReadAllText(Path.Join(Application.persistentDataPath, "Scripts", filePath)), filePath, bindings, tick, null);
	}

	// Token: 0x06002F9D RID: 12189 RVA: 0x0012CCF4 File Offset: 0x0012AEF4
	~LuauScriptRunner()
	{
		LuauVm.ClassBuilders.Clear();
		Bindings.LuauPlayerList.Clear();
		Bindings.LuauGameObjectList.Clear();
		Bindings.LuauVRRigList.Clear();
		ReflectionMetaNames.ReflectedNames.Clear();
		if (BurstClassInfo.ClassList.InfoFields.Data.IsCreated)
		{
			BurstClassInfo.ClassList.InfoFields.Data.Clear();
		}
	}

	// Token: 0x040033BF RID: 13247
	public static List<LuauScriptRunner> ScriptRunners = new List<LuauScriptRunner>();

	// Token: 0x040033C0 RID: 13248
	public bool ShouldTick;

	// Token: 0x040033C1 RID: 13249
	private lua_CFunction postTickCallback;

	// Token: 0x040033C2 RID: 13250
	private lua_CFunction preTickCallback;

	// Token: 0x040033C3 RID: 13251
	public string ScriptName;

	// Token: 0x040033C4 RID: 13252
	public string Script;

	// Token: 0x040033C5 RID: 13253
	public unsafe lua_State* L;
}
