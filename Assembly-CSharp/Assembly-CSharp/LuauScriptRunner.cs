using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02000779 RID: 1913
public class LuauScriptRunner
{
	// Token: 0x06002EEF RID: 12015 RVA: 0x000E3050 File Offset: 0x000E1250
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

	// Token: 0x06002EF0 RID: 12016 RVA: 0x000E3090 File Offset: 0x000E1290
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

	// Token: 0x06002EF1 RID: 12017 RVA: 0x000E3140 File Offset: 0x000E1340
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

	// Token: 0x06002EF2 RID: 12018 RVA: 0x000E3227 File Offset: 0x000E1427
	public LuauScriptRunner FromFile(string filePath, [CanBeNull] lua_CFunction bindings = null, [CanBeNull] lua_CFunction tick = null)
	{
		return new LuauScriptRunner(File.ReadAllText(Path.Join(Application.persistentDataPath, "Scripts", filePath)), filePath, bindings, tick, null);
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x000E3258 File Offset: 0x000E1458
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

	// Token: 0x0400331B RID: 13083
	public static List<LuauScriptRunner> ScriptRunners = new List<LuauScriptRunner>();

	// Token: 0x0400331C RID: 13084
	public bool ShouldTick;

	// Token: 0x0400331D RID: 13085
	private lua_CFunction postTickCallback;

	// Token: 0x0400331E RID: 13086
	private lua_CFunction preTickCallback;

	// Token: 0x0400331F RID: 13087
	public string ScriptName;

	// Token: 0x04003320 RID: 13088
	public string Script;

	// Token: 0x04003321 RID: 13089
	public unsafe lua_State* L;
}
