using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02000778 RID: 1912
public class LuauScriptRunner
{
	// Token: 0x06002EE7 RID: 12007 RVA: 0x000E2BD0 File Offset: 0x000E0DD0
	public unsafe static bool ErrorCheck(lua_State* L, int status)
	{
		if (status == 2)
		{
			Debug.Log(new string(Luau.lua_tostring(L, -1)));
			sbyte* value = (sbyte*)Luau.lua_debugtrace(L);
			Debug.Log(new string(value));
			Luau.lua_close(L);
			return true;
		}
		return false;
	}

	// Token: 0x06002EE8 RID: 12008 RVA: 0x000E2C10 File Offset: 0x000E0E10
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

	// Token: 0x06002EE9 RID: 12009 RVA: 0x000E2CC0 File Offset: 0x000E0EC0
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

	// Token: 0x06002EEA RID: 12010 RVA: 0x000E2DA7 File Offset: 0x000E0FA7
	public LuauScriptRunner FromFile(string filePath, [CanBeNull] lua_CFunction bindings = null, [CanBeNull] lua_CFunction tick = null)
	{
		return new LuauScriptRunner(File.ReadAllText(Path.Join(Application.persistentDataPath, "Scripts", filePath)), filePath, bindings, tick, null);
	}

	// Token: 0x06002EEB RID: 12011 RVA: 0x000E2DD8 File Offset: 0x000E0FD8
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

	// Token: 0x04003315 RID: 13077
	public static List<LuauScriptRunner> ScriptRunners = new List<LuauScriptRunner>();

	// Token: 0x04003316 RID: 13078
	public bool ShouldTick;

	// Token: 0x04003317 RID: 13079
	private lua_CFunction postTickCallback;

	// Token: 0x04003318 RID: 13080
	private lua_CFunction preTickCallback;

	// Token: 0x04003319 RID: 13081
	public string ScriptName;

	// Token: 0x0400331A RID: 13082
	public string Script;

	// Token: 0x0400331B RID: 13083
	public unsafe lua_State* L;
}
