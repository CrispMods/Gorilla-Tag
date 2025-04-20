using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using ExitGames.Client.Photon;
using GorillaExtensions;
using GorillaLocomotion;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

// Token: 0x02000732 RID: 1842
[BurstCompile]
public static class Bindings
{
	// Token: 0x06002DA4 RID: 11684 RVA: 0x00129244 File Offset: 0x00127444
	public unsafe static void GameObjectBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Bindings.LuauGameObject>("GameObject").AddField("position", "Position").AddField("rotation", "Rotation").AddField("scale", "Scale").AddStaticFunction("findGameObject", new lua_CFunction(Bindings.GameObjectFunctions.FindGameObject)).AddFunction("setCollision", new lua_CFunction(Bindings.GameObjectFunctions.SetCollision)).AddFunction("setVisibility", new lua_CFunction(Bindings.GameObjectFunctions.SetVisibility)).AddFunction("setActive", new lua_CFunction(Bindings.GameObjectFunctions.SetActive)).AddFunction("setText", new lua_CFunction(Bindings.GameObjectFunctions.SetText)).Build(L, true));
	}

	// Token: 0x06002DA5 RID: 11685 RVA: 0x00129308 File Offset: 0x00127508
	public unsafe static void GorillaLocomotionSettingsBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Bindings.GorillaLocomotionSettings>("PSettings").AddField("velocityLimit", null).AddField("slideVelocityLimit", null).AddField("maxJumpSpeed", null).AddField("jumpMultiplier", null).Build(L, false));
		Bindings.LocomotionSettings = Luau.lua_class_push<Bindings.GorillaLocomotionSettings>(L);
		Bindings.LocomotionSettings->velocityLimit = GTPlayer.Instance.velocityLimit;
		Bindings.LocomotionSettings->slideVelocityLimit = GTPlayer.Instance.slideVelocityLimit;
		Bindings.LocomotionSettings->maxJumpSpeed = 6.5f;
		Bindings.LocomotionSettings->jumpMultiplier = 1.1f;
		Luau.lua_setglobal(L, "PlayerSettings");
	}

	// Token: 0x06002DA6 RID: 11686 RVA: 0x001293BC File Offset: 0x001275BC
	public unsafe static void Vec3Builder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Vector3>("Vec3").AddField("x", null).AddField("y", null).AddField("z", null).AddStaticFunction("new", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.New))).AddFunction("__add", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Add))).AddFunction("__sub", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Sub))).AddFunction("__mul", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Mul))).AddFunction("__div", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Div))).AddFunction("__unm", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Unm))).AddFunction("__eq", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Eq))).AddFunction("__tostring", new lua_CFunction(Bindings.Vec3Functions.ToSring)).AddFunction("toString", new lua_CFunction(Bindings.Vec3Functions.ToSring)).AddFunction("dot", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Dot))).AddFunction("cross", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Cross))).AddFunction("projectOnTo", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Project))).AddFunction("length", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Length))).AddFunction("normalize", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Normalize))).AddFunction("getSafeNormal", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.SafeNormal))).AddStaticFunction("rotate", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Rotate))).AddFunction("rotate", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Rotate))).AddStaticFunction("distance", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Distance))).AddFunction("distance", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Distance))).AddStaticFunction("lerp", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Lerp))).AddFunction("lerp", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Lerp))).AddProperty("zeroVector", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.ZeroVector))).AddProperty("oneVector", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.OneVector))).Build(L, true));
	}

	// Token: 0x06002DA7 RID: 11687 RVA: 0x0012966C File Offset: 0x0012786C
	public unsafe static void QuatBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Quaternion>("Quat").AddField("x", null).AddField("y", null).AddField("z", null).AddField("w", null).AddStaticFunction("new", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.New))).AddFunction("__mul", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.Mul))).AddFunction("__eq", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.Eq))).AddFunction("__tostring", new lua_CFunction(Bindings.QuatFunctions.ToString)).AddFunction("toString", new lua_CFunction(Bindings.QuatFunctions.ToString)).AddStaticFunction("fromEuler", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.FromEuler))).AddStaticFunction("fromDirection", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.FromDirection))).AddFunction("getUpVector", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.GetUpVector))).AddFunction("euler", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.Euler))).Build(L, true));
	}

	// Token: 0x06002DA8 RID: 11688 RVA: 0x001297AC File Offset: 0x001279AC
	public unsafe static void PlayerBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Bindings.LuauPlayer>("Player").AddField("playerID", "PlayerID").AddField("playerName", "PlayerName").AddField("playerMaterial", "PlayerMaterial").AddField("isMasterClient", "IsMasterClient").AddField("bodyPosition", "BodyPosition").AddField("leftHandPosition", "LeftHandPosition").AddField("rightHandPosition", "RightHandPosition").AddField("headRotation", "HeadRotation").AddField("leftHandRotation", "LeftHandRotation").AddField("rightHandRotation", "RightHandRotation").AddStaticFunction("getPlayerByID", new lua_CFunction(Bindings.PlayerFunctions.GetPlayerByID)).Build(L, true));
	}

	// Token: 0x06002DA9 RID: 11689 RVA: 0x00129884 File Offset: 0x00127A84
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int LuaStartVibration(lua_State* L)
	{
		bool forLeftController = Luau.lua_toboolean(L, 1) == 1;
		float amplitude = (float)Luau.luaL_checknumber(L, 2);
		float duration = (float)Luau.luaL_checknumber(L, 3);
		GorillaTagger.Instance.StartVibration(forLeftController, amplitude, duration);
		return 0;
	}

	// Token: 0x06002DAA RID: 11690 RVA: 0x001298BC File Offset: 0x00127ABC
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int LuaPlaySound(lua_State* L)
	{
		int num = (int)Luau.luaL_checknumber(L, 1);
		Vector3 position = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
		float volume = (float)Luau.luaL_checknumber(L, 3);
		if (num < 0 || num >= VRRig.LocalRig.clipToPlay.Length)
		{
			return 0;
		}
		AudioSource.PlayClipAtPoint(VRRig.LocalRig.clipToPlay[num], position, volume);
		return 0;
	}

	// Token: 0x04003313 RID: 13075
	public static Dictionary<GameObject, IntPtr> LuauGameObjectList = new Dictionary<GameObject, IntPtr>();

	// Token: 0x04003314 RID: 13076
	public static Dictionary<int, IntPtr> LuauPlayerList = new Dictionary<int, IntPtr>();

	// Token: 0x04003315 RID: 13077
	public static Dictionary<int, VRRig> LuauVRRigList = new Dictionary<int, VRRig>();

	// Token: 0x04003316 RID: 13078
	public unsafe static Bindings.GorillaLocomotionSettings* LocomotionSettings;

	// Token: 0x02000733 RID: 1843
	public static class LuaEmit
	{
		// Token: 0x06002DAC RID: 11692 RVA: 0x0012991C File Offset: 0x00127B1C
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Emit(lua_State* L)
		{
			if (Bindings.LuaEmit.callTime < Time.time - 1f)
			{
				Bindings.LuaEmit.callTime = Time.time - 1f;
			}
			Bindings.LuaEmit.callTime += 1f / Bindings.LuaEmit.callCount;
			if (Bindings.LuaEmit.callTime > Time.time)
			{
				LuauHud.Instance.LuauLog("Emit rate limit reached, event not sent");
				return 0;
			}
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.Others
			};
			if (Luau.lua_type(L, 2) != 6)
			{
				Luau.luaL_errorL(L, "Argument 2 must be a table", Array.Empty<string>());
				return 0;
			}
			Luau.lua_pushnil(L);
			int num = 0;
			List<object> list = new List<object>();
			list.Add(Marshal.PtrToStringAnsi((IntPtr)((void*)Luau.luaL_checkstring(L, 1))));
			while (Luau.lua_next(L, 2) != 0 && num++ < 10)
			{
				Luau.lua_Types lua_Types = (Luau.lua_Types)Luau.lua_type(L, -1);
				if (lua_Types <= Luau.lua_Types.LUA_TNUMBER)
				{
					if (lua_Types == Luau.lua_Types.LUA_TBOOLEAN)
					{
						list.Add(Luau.lua_toboolean(L, -1) == 1);
						Luau.lua_pop(L, 1);
						continue;
					}
					if (lua_Types == Luau.lua_Types.LUA_TNUMBER)
					{
						list.Add(Luau.luaL_checknumber(L, -1));
						Luau.lua_pop(L, 1);
						continue;
					}
				}
				else if (lua_Types == Luau.lua_Types.LUA_TTABLE || lua_Types == Luau.lua_Types.LUA_TUSERDATA)
				{
					Luau.luaL_getmetafield(L, -1, "metahash");
					BurstClassInfo.ClassInfo classInfo;
					if (!BurstClassInfo.ClassList.InfoFields.Data.TryGetValue((int)Luau.luaL_checknumber(L, -1), out classInfo))
					{
						FixedString64Bytes fixedString64Bytes = "\"Internal Class Info Error No Metatable Found\"";
						Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString64Bytes>(ref fixedString64Bytes) + 2));
						return 0;
					}
					Luau.lua_pop(L, 1);
					FixedString32Bytes fixedString32Bytes = "Vec3";
					if (classInfo.Name == fixedString32Bytes)
					{
						list.Add(*Luau.lua_class_get<Vector3>(L, -1));
						Luau.lua_pop(L, 1);
						continue;
					}
					fixedString32Bytes = "Quat";
					if (classInfo.Name == fixedString32Bytes)
					{
						list.Add(*Luau.lua_class_get<Quaternion>(L, -1));
						Luau.lua_pop(L, 1);
						continue;
					}
					fixedString32Bytes = "Player";
					if (classInfo.Name == fixedString32Bytes)
					{
						int playerID = Luau.lua_class_get<Bindings.LuauPlayer>(L, -1)->PlayerID;
						NetPlayer netPlayer = null;
						foreach (NetPlayer netPlayer2 in RoomSystem.PlayersInRoom)
						{
							if (netPlayer2.ActorNumber == playerID)
							{
								netPlayer = netPlayer2;
							}
						}
						if (netPlayer == null)
						{
							list.Add(null);
						}
						else
						{
							list.Add(netPlayer.GetPlayerRef());
						}
						Luau.lua_pop(L, 1);
						continue;
					}
					FixedString32Bytes fixedString32Bytes2 = "\"Unknown Type in table\"";
					Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes2) + 2));
					continue;
				}
				FixedString32Bytes fixedString32Bytes3 = "\"Unknown Type in table\"";
				Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes3) + 2));
				return 0;
			}
			if (PhotonNetwork.InRoom)
			{
				PhotonNetwork.RaiseEvent(180, list.ToArray(), raiseEventOptions, SendOptions.SendReliable);
			}
			return 0;
		}

		// Token: 0x04003317 RID: 13079
		private static float callTime = 0f;

		// Token: 0x04003318 RID: 13080
		private static float callCount = 20f;
	}

	// Token: 0x02000734 RID: 1844
	[BurstCompile]
	public struct LuauGameObject
	{
		// Token: 0x04003319 RID: 13081
		public Vector3 Position;

		// Token: 0x0400331A RID: 13082
		public Quaternion Rotation;

		// Token: 0x0400331B RID: 13083
		public Vector3 Scale;
	}

	// Token: 0x02000735 RID: 1845
	[BurstCompile]
	public static class GameObjectFunctions
	{
		// Token: 0x06002DAE RID: 11694 RVA: 0x00129C10 File Offset: 0x00127E10
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int New(lua_State* L)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Bindings.LuauGameObject* ptr = Luau.lua_class_push<Bindings.LuauGameObject>(L);
			ptr->Position = gameObject.transform.position;
			ptr->Rotation = gameObject.transform.rotation;
			ptr->Scale = gameObject.transform.localScale;
			Bindings.LuauGameObjectList.TryAdd(gameObject, (IntPtr)((void*)ptr));
			return 1;
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x00129C74 File Offset: 0x00127E74
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int FindGameObject(lua_State* L)
		{
			GameObject gameObject = GameObject.Find(new string((sbyte*)Luau.luaL_checkstring(L, 1)));
			if (!(gameObject != null))
			{
				return 0;
			}
			if (!CustomMapLoader.IsCustomScene(gameObject.scene.name))
			{
				return 0;
			}
			IntPtr ptr;
			if (Bindings.LuauGameObjectList.TryGetValue(gameObject, out ptr))
			{
				Luau.lua_class_push(L, "GameObject", ptr);
			}
			else
			{
				Bindings.LuauGameObject* ptr2 = Luau.lua_class_push<Bindings.LuauGameObject>(L);
				ptr2->Position = gameObject.transform.position;
				ptr2->Rotation = gameObject.transform.rotation;
				ptr2->Scale = gameObject.transform.localScale;
				Bindings.LuauGameObjectList.TryAdd(gameObject, (IntPtr)((void*)ptr2));
			}
			return 1;
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x00129D28 File Offset: 0x00127F28
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SetCollision(lua_State* L)
		{
			Bindings.LuauGameObject* data = Luau.lua_class_get<Bindings.LuauGameObject>(L, 1, "GameObject");
			Bindings.LuauGameObjectList.FirstOrDefault((KeyValuePair<GameObject, IntPtr> g) => g.Value == (IntPtr)((void*)data)).Key.GetComponent<Collider>().enabled = (Luau.lua_toboolean(L, 2) == 1);
			return 0;
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x00129D88 File Offset: 0x00127F88
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SetVisibility(lua_State* L)
		{
			Bindings.LuauGameObject* data = Luau.lua_class_get<Bindings.LuauGameObject>(L, 1, "GameObject");
			Bindings.LuauGameObjectList.FirstOrDefault((KeyValuePair<GameObject, IntPtr> g) => g.Value == (IntPtr)((void*)data)).Key.GetComponent<MeshRenderer>().enabled = (Luau.lua_toboolean(L, 2) == 1);
			return 0;
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x00129DE8 File Offset: 0x00127FE8
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SetActive(lua_State* L)
		{
			Bindings.LuauGameObject* data = Luau.lua_class_get<Bindings.LuauGameObject>(L, 1, "GameObject");
			Bindings.LuauGameObjectList.FirstOrDefault((KeyValuePair<GameObject, IntPtr> g) => g.Value == (IntPtr)((void*)data)).Key.SetActive(Luau.lua_toboolean(L, 2) == 1);
			return 0;
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x00129E40 File Offset: 0x00128040
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SetText(lua_State* L)
		{
			Bindings.LuauGameObject* data = Luau.lua_class_get<Bindings.LuauGameObject>(L, 1, "GameObject");
			GameObject key = Bindings.LuauGameObjectList.FirstOrDefault((KeyValuePair<GameObject, IntPtr> g) => g.Value == (IntPtr)((void*)data)).Key;
			string text = new string(Luau.lua_tostring(L, 2));
			TextMeshPro component = key.GetComponent<TextMeshPro>();
			if (component.IsNotNull())
			{
				component.text = text;
			}
			else
			{
				TextMesh component2 = key.GetComponent<TextMesh>();
				if (component2.IsNotNull())
				{
					component2.text = text;
				}
			}
			return 0;
		}
	}

	// Token: 0x0200073A RID: 1850
	[BurstCompile]
	public struct LuauPlayer
	{
		// Token: 0x04003320 RID: 13088
		public int PlayerID;

		// Token: 0x04003321 RID: 13089
		public FixedString32Bytes PlayerName;

		// Token: 0x04003322 RID: 13090
		public int PlayerMaterial;

		// Token: 0x04003323 RID: 13091
		public bool IsMasterClient;

		// Token: 0x04003324 RID: 13092
		public Vector3 BodyPosition;

		// Token: 0x04003325 RID: 13093
		public Vector3 LeftHandPosition;

		// Token: 0x04003326 RID: 13094
		public Vector3 RightHandPosition;

		// Token: 0x04003327 RID: 13095
		public Quaternion HeadRotation;

		// Token: 0x04003328 RID: 13096
		public Quaternion LeftHandRotation;

		// Token: 0x04003329 RID: 13097
		public Quaternion RightHandRotation;
	}

	// Token: 0x0200073B RID: 1851
	[BurstCompile]
	public static class PlayerFunctions
	{
		// Token: 0x06002DBC RID: 11708 RVA: 0x00129ECC File Offset: 0x001280CC
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int GetPlayerByID(lua_State* L)
		{
			int num = (int)Luau.luaL_checknumber(L, 1);
			foreach (NetPlayer netPlayer in RoomSystem.PlayersInRoom)
			{
				if (netPlayer.ActorNumber == num)
				{
					IntPtr ptr;
					if (Bindings.LuauPlayerList.TryGetValue(netPlayer.ActorNumber, out ptr))
					{
						Luau.lua_class_push(L, "Player", ptr);
					}
					else
					{
						Bindings.LuauPlayer* ptr2 = Luau.lua_class_push<Bindings.LuauPlayer>(L);
						ptr2->PlayerID = netPlayer.ActorNumber;
						ptr2->PlayerMaterial = 0;
						ptr2->IsMasterClient = netPlayer.IsMasterClient;
						Bindings.LuauPlayerList[netPlayer.ActorNumber] = (IntPtr)((void*)ptr2);
						GorillaGameManager instance = GorillaGameManager.instance;
						VRRig vrrig = (instance != null) ? instance.FindPlayerVRRig(netPlayer) : null;
						if (vrrig != null)
						{
							ptr2->PlayerName = vrrig.playerNameVisible;
							Bindings.LuauVRRigList[netPlayer.ActorNumber] = vrrig;
							Bindings.PlayerFunctions.UpdatePlayer(L, vrrig, ptr2);
							Bindings.LuauPlayerList[netPlayer.ActorNumber] = (IntPtr)((void*)ptr2);
						}
					}
				}
			}
			return 0;
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x0012A004 File Offset: 0x00128204
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static void UpdatePlayer(lua_State* L, VRRig p, Bindings.LuauPlayer* data)
		{
			data->BodyPosition = p.transform.position;
			data->LeftHandPosition = p.leftHandTransform.position;
			data->RightHandPosition = p.rightHandTransform.position;
			data->HeadRotation = p.transform.rotation;
			data->LeftHandRotation = p.leftHandTransform.rotation;
			data->RightHandRotation = p.rightHandTransform.rotation;
		}
	}

	// Token: 0x0200073C RID: 1852
	[BurstCompile]
	public static class Vec3Functions
	{
		// Token: 0x06002DBE RID: 11710 RVA: 0x0004EFEF File Offset: 0x0004D1EF
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int New(lua_State* L)
		{
			return Bindings.Vec3Functions.New_00002DBE$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x0004EFF7 File Offset: 0x0004D1F7
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Add(lua_State* L)
		{
			return Bindings.Vec3Functions.Add_00002DBF$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x0004EFFF File Offset: 0x0004D1FF
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Sub(lua_State* L)
		{
			return Bindings.Vec3Functions.Sub_00002DC0$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x0004F007 File Offset: 0x0004D207
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Mul(lua_State* L)
		{
			return Bindings.Vec3Functions.Mul_00002DC1$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x0004F00F File Offset: 0x0004D20F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Div(lua_State* L)
		{
			return Bindings.Vec3Functions.Div_00002DC2$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x0004F017 File Offset: 0x0004D217
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Unm(lua_State* L)
		{
			return Bindings.Vec3Functions.Unm_00002DC3$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x0004F01F File Offset: 0x0004D21F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Eq(lua_State* L)
		{
			return Bindings.Vec3Functions.Eq_00002DC4$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x0012A078 File Offset: 0x00128278
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int ToSring(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Luau.lua_pushstring(L, vector.ToString());
			return 1;
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x0004F027 File Offset: 0x0004D227
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Dot(lua_State* L)
		{
			return Bindings.Vec3Functions.Dot_00002DC6$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x0004F02F File Offset: 0x0004D22F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Cross(lua_State* L)
		{
			return Bindings.Vec3Functions.Cross_00002DC7$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x0004F037 File Offset: 0x0004D237
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Project(lua_State* L)
		{
			return Bindings.Vec3Functions.Project_00002DC8$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x0004F03F File Offset: 0x0004D23F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Length(lua_State* L)
		{
			return Bindings.Vec3Functions.Length_00002DC9$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x0004F047 File Offset: 0x0004D247
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Normalize(lua_State* L)
		{
			return Bindings.Vec3Functions.Normalize_00002DCA$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x0004F04F File Offset: 0x0004D24F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SafeNormal(lua_State* L)
		{
			return Bindings.Vec3Functions.SafeNormal_00002DCB$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x0004F057 File Offset: 0x0004D257
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Distance(lua_State* L)
		{
			return Bindings.Vec3Functions.Distance_00002DCC$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x0004F05F File Offset: 0x0004D25F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Lerp(lua_State* L)
		{
			return Bindings.Vec3Functions.Lerp_00002DCD$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x0004F067 File Offset: 0x0004D267
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Rotate(lua_State* L)
		{
			return Bindings.Vec3Functions.Rotate_00002DCE$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x0004F06F File Offset: 0x0004D26F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int ZeroVector(lua_State* L)
		{
			return Bindings.Vec3Functions.ZeroVector_00002DCF$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x0004F077 File Offset: 0x0004D277
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int OneVector(lua_State* L)
		{
			return Bindings.Vec3Functions.OneVector_00002DD0$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x0012A0B4 File Offset: 0x001282B4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int New$BurstManaged(lua_State* L)
		{
			Vector3* ptr = Luau.lua_class_push<Vector3>(L, "Vec3");
			ptr->x = (float)Luau.luaL_optnumber(L, 1, 0.0);
			ptr->y = (float)Luau.luaL_optnumber(L, 2, 0.0);
			ptr->z = (float)Luau.luaL_optnumber(L, 3, 0.0);
			return 1;
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x0012A118 File Offset: 0x00128318
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Add$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a + b;
			return 1;
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x0012A170 File Offset: 0x00128370
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Sub$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a - b;
			return 1;
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x0012A1C8 File Offset: 0x001283C8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Mul$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			float d = (float)Luau.luaL_checknumber(L, 2);
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a * d;
			return 1;
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x0012A214 File Offset: 0x00128414
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Div$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			float d = (float)Luau.luaL_checknumber(L, 2);
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a / d;
			return 1;
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x0012A260 File Offset: 0x00128460
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Unm$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = -a;
			return 1;
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x0012A2A0 File Offset: 0x001284A0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Eq$BurstManaged(lua_State* L)
		{
			Vector3 lhs = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 rhs = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			int num = (lhs == rhs) ? 1 : 0;
			Luau.lua_pushnumber(L, (double)num);
			return 1;
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x0012A2F0 File Offset: 0x001284F0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Dot$BurstManaged(lua_State* L)
		{
			Vector3 lhs = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 rhs = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			double n = (double)Vector3.Dot(lhs, rhs);
			Luau.lua_pushnumber(L, n);
			return 1;
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x0012A33C File Offset: 0x0012853C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Cross$BurstManaged(lua_State* L)
		{
			Vector3 lhs = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 rhs = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = Vector3.Cross(lhs, rhs);
			return 1;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x0012A394 File Offset: 0x00128594
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Project$BurstManaged(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 onNormal = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = Vector3.Project(vector, onNormal);
			return 1;
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x0012A3EC File Offset: 0x001285EC
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Length$BurstManaged(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Luau.lua_pushnumber(L, (double)Vector3.Magnitude(vector));
			return 1;
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x0004F07F File Offset: 0x0004D27F
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Normalize$BurstManaged(lua_State* L)
		{
			Luau.lua_class_get<Vector3>(L, 1, "Vec3")->Normalize();
			return 0;
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x0012A420 File Offset: 0x00128620
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int SafeNormal$BurstManaged(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = vector.normalized;
			return 1;
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x0012A464 File Offset: 0x00128664
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Distance$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			Luau.lua_pushnumber(L, (double)Vector3.Distance(a, b));
			return 1;
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x0012A4B0 File Offset: 0x001286B0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Lerp$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			double num = Luau.luaL_checknumber(L, 3);
			*Luau.lua_class_push<Vector3>(L, "Vec3") = Vector3.Lerp(a, b, (float)num);
			return 1;
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x0012A514 File Offset: 0x00128714
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Rotate$BurstManaged(lua_State* L)
		{
			Vector3 point = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Quaternion rotation = *Luau.lua_class_get<Quaternion>(L, 2, "Quat");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = rotation * point;
			return 1;
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x0004F098 File Offset: 0x0004D298
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int ZeroVector$BurstManaged(lua_State* L)
		{
			Vector3* ptr = Luau.lua_class_push<Vector3>(L, "Vec3");
			ptr->x = 0f;
			ptr->y = 0f;
			ptr->z = 0f;
			return 1;
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x0004F0CB File Offset: 0x0004D2CB
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int OneVector$BurstManaged(lua_State* L)
		{
			Vector3* ptr = Luau.lua_class_push<Vector3>(L, "Vec3");
			ptr->x = 1f;
			ptr->y = 1f;
			ptr->z = 1f;
			return 1;
		}

		// Token: 0x0200073D RID: 1853
		// (Invoke) Token: 0x06002DE4 RID: 11748
		public unsafe delegate int New_00002DBE$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200073E RID: 1854
		internal static class New_00002DBE$BurstDirectCall
		{
			// Token: 0x06002DE7 RID: 11751 RVA: 0x0004F0FE File Offset: 0x0004D2FE
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.New_00002DBE$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.New_00002DBE$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.New_00002DBE$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.New$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.New_00002DBE$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.New_00002DBE$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DE8 RID: 11752 RVA: 0x0012A56C File Offset: 0x0012876C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.New_00002DBE$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DE9 RID: 11753 RVA: 0x0004F12A File Offset: 0x0004D32A
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.New_00002DBE$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.New(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DEA RID: 11754 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002DEB RID: 11755 RVA: 0x0004F13B File Offset: 0x0004D33B
			// Note: this type is marked as 'beforefieldinit'.
			static New_00002DBE$BurstDirectCall()
			{
				Bindings.Vec3Functions.New_00002DBE$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DEC RID: 11756 RVA: 0x0012A584 File Offset: 0x00128784
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.New_00002DBE$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.New$BurstManaged(L);
			}

			// Token: 0x0400332A RID: 13098
			private static IntPtr Pointer;

			// Token: 0x0400332B RID: 13099
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200073F RID: 1855
		// (Invoke) Token: 0x06002DEE RID: 11758
		public unsafe delegate int Add_00002DBF$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000740 RID: 1856
		internal static class Add_00002DBF$BurstDirectCall
		{
			// Token: 0x06002DF1 RID: 11761 RVA: 0x0004F142 File Offset: 0x0004D342
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Add_00002DBF$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Add_00002DBF$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Add_00002DBF$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Add$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Add_00002DBF$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Add_00002DBF$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DF2 RID: 11762 RVA: 0x0012A5B8 File Offset: 0x001287B8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Add_00002DBF$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DF3 RID: 11763 RVA: 0x0004F16E File Offset: 0x0004D36E
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Add_00002DBF$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Add(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DF4 RID: 11764 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002DF5 RID: 11765 RVA: 0x0004F17F File Offset: 0x0004D37F
			// Note: this type is marked as 'beforefieldinit'.
			static Add_00002DBF$BurstDirectCall()
			{
				Bindings.Vec3Functions.Add_00002DBF$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DF6 RID: 11766 RVA: 0x0012A5D0 File Offset: 0x001287D0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Add_00002DBF$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Add$BurstManaged(L);
			}

			// Token: 0x0400332C RID: 13100
			private static IntPtr Pointer;

			// Token: 0x0400332D RID: 13101
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000741 RID: 1857
		// (Invoke) Token: 0x06002DF8 RID: 11768
		public unsafe delegate int Sub_00002DC0$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000742 RID: 1858
		internal static class Sub_00002DC0$BurstDirectCall
		{
			// Token: 0x06002DFB RID: 11771 RVA: 0x0004F186 File Offset: 0x0004D386
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Sub_00002DC0$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Sub_00002DC0$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Sub_00002DC0$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Sub$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Sub_00002DC0$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Sub_00002DC0$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DFC RID: 11772 RVA: 0x0012A604 File Offset: 0x00128804
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Sub_00002DC0$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DFD RID: 11773 RVA: 0x0004F1B2 File Offset: 0x0004D3B2
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Sub_00002DC0$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Sub(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DFE RID: 11774 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002DFF RID: 11775 RVA: 0x0004F1C3 File Offset: 0x0004D3C3
			// Note: this type is marked as 'beforefieldinit'.
			static Sub_00002DC0$BurstDirectCall()
			{
				Bindings.Vec3Functions.Sub_00002DC0$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E00 RID: 11776 RVA: 0x0012A61C File Offset: 0x0012881C
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Sub_00002DC0$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Sub$BurstManaged(L);
			}

			// Token: 0x0400332E RID: 13102
			private static IntPtr Pointer;

			// Token: 0x0400332F RID: 13103
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000743 RID: 1859
		// (Invoke) Token: 0x06002E02 RID: 11778
		public unsafe delegate int Mul_00002DC1$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000744 RID: 1860
		internal static class Mul_00002DC1$BurstDirectCall
		{
			// Token: 0x06002E05 RID: 11781 RVA: 0x0004F1CA File Offset: 0x0004D3CA
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Mul_00002DC1$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Mul_00002DC1$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Mul_00002DC1$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Mul$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Mul_00002DC1$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Mul_00002DC1$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E06 RID: 11782 RVA: 0x0012A650 File Offset: 0x00128850
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Mul_00002DC1$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E07 RID: 11783 RVA: 0x0004F1F6 File Offset: 0x0004D3F6
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Mul_00002DC1$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Mul(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E08 RID: 11784 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E09 RID: 11785 RVA: 0x0004F207 File Offset: 0x0004D407
			// Note: this type is marked as 'beforefieldinit'.
			static Mul_00002DC1$BurstDirectCall()
			{
				Bindings.Vec3Functions.Mul_00002DC1$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E0A RID: 11786 RVA: 0x0012A668 File Offset: 0x00128868
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Mul_00002DC1$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Mul$BurstManaged(L);
			}

			// Token: 0x04003330 RID: 13104
			private static IntPtr Pointer;

			// Token: 0x04003331 RID: 13105
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000745 RID: 1861
		// (Invoke) Token: 0x06002E0C RID: 11788
		public unsafe delegate int Div_00002DC2$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000746 RID: 1862
		internal static class Div_00002DC2$BurstDirectCall
		{
			// Token: 0x06002E0F RID: 11791 RVA: 0x0004F20E File Offset: 0x0004D40E
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Div_00002DC2$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Div_00002DC2$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Div_00002DC2$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Div$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Div_00002DC2$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Div_00002DC2$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E10 RID: 11792 RVA: 0x0012A69C File Offset: 0x0012889C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Div_00002DC2$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E11 RID: 11793 RVA: 0x0004F23A File Offset: 0x0004D43A
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Div_00002DC2$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Div(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E12 RID: 11794 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E13 RID: 11795 RVA: 0x0004F24B File Offset: 0x0004D44B
			// Note: this type is marked as 'beforefieldinit'.
			static Div_00002DC2$BurstDirectCall()
			{
				Bindings.Vec3Functions.Div_00002DC2$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E14 RID: 11796 RVA: 0x0012A6B4 File Offset: 0x001288B4
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Div_00002DC2$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Div$BurstManaged(L);
			}

			// Token: 0x04003332 RID: 13106
			private static IntPtr Pointer;

			// Token: 0x04003333 RID: 13107
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000747 RID: 1863
		// (Invoke) Token: 0x06002E16 RID: 11798
		public unsafe delegate int Unm_00002DC3$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000748 RID: 1864
		internal static class Unm_00002DC3$BurstDirectCall
		{
			// Token: 0x06002E19 RID: 11801 RVA: 0x0004F252 File Offset: 0x0004D452
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Unm_00002DC3$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Unm_00002DC3$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Unm_00002DC3$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Unm$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Unm_00002DC3$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Unm_00002DC3$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E1A RID: 11802 RVA: 0x0012A6E8 File Offset: 0x001288E8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Unm_00002DC3$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E1B RID: 11803 RVA: 0x0004F27E File Offset: 0x0004D47E
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Unm_00002DC3$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Unm(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E1C RID: 11804 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E1D RID: 11805 RVA: 0x0004F28F File Offset: 0x0004D48F
			// Note: this type is marked as 'beforefieldinit'.
			static Unm_00002DC3$BurstDirectCall()
			{
				Bindings.Vec3Functions.Unm_00002DC3$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E1E RID: 11806 RVA: 0x0012A700 File Offset: 0x00128900
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Unm_00002DC3$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Unm$BurstManaged(L);
			}

			// Token: 0x04003334 RID: 13108
			private static IntPtr Pointer;

			// Token: 0x04003335 RID: 13109
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000749 RID: 1865
		// (Invoke) Token: 0x06002E20 RID: 11808
		public unsafe delegate int Eq_00002DC4$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200074A RID: 1866
		internal static class Eq_00002DC4$BurstDirectCall
		{
			// Token: 0x06002E23 RID: 11811 RVA: 0x0004F296 File Offset: 0x0004D496
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Eq_00002DC4$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Eq_00002DC4$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Eq_00002DC4$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Eq$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Eq_00002DC4$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Eq_00002DC4$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E24 RID: 11812 RVA: 0x0012A734 File Offset: 0x00128934
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Eq_00002DC4$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E25 RID: 11813 RVA: 0x0004F2C2 File Offset: 0x0004D4C2
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Eq_00002DC4$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Eq(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E26 RID: 11814 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E27 RID: 11815 RVA: 0x0004F2D3 File Offset: 0x0004D4D3
			// Note: this type is marked as 'beforefieldinit'.
			static Eq_00002DC4$BurstDirectCall()
			{
				Bindings.Vec3Functions.Eq_00002DC4$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E28 RID: 11816 RVA: 0x0012A74C File Offset: 0x0012894C
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Eq_00002DC4$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Eq$BurstManaged(L);
			}

			// Token: 0x04003336 RID: 13110
			private static IntPtr Pointer;

			// Token: 0x04003337 RID: 13111
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200074B RID: 1867
		// (Invoke) Token: 0x06002E2A RID: 11818
		public unsafe delegate int Dot_00002DC6$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200074C RID: 1868
		internal static class Dot_00002DC6$BurstDirectCall
		{
			// Token: 0x06002E2D RID: 11821 RVA: 0x0004F2DA File Offset: 0x0004D4DA
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Dot_00002DC6$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Dot_00002DC6$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Dot_00002DC6$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Dot$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Dot_00002DC6$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Dot_00002DC6$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E2E RID: 11822 RVA: 0x0012A780 File Offset: 0x00128980
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Dot_00002DC6$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E2F RID: 11823 RVA: 0x0004F306 File Offset: 0x0004D506
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Dot_00002DC6$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Dot(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E30 RID: 11824 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E31 RID: 11825 RVA: 0x0004F317 File Offset: 0x0004D517
			// Note: this type is marked as 'beforefieldinit'.
			static Dot_00002DC6$BurstDirectCall()
			{
				Bindings.Vec3Functions.Dot_00002DC6$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E32 RID: 11826 RVA: 0x0012A798 File Offset: 0x00128998
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Dot_00002DC6$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Dot$BurstManaged(L);
			}

			// Token: 0x04003338 RID: 13112
			private static IntPtr Pointer;

			// Token: 0x04003339 RID: 13113
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200074D RID: 1869
		// (Invoke) Token: 0x06002E34 RID: 11828
		public unsafe delegate int Cross_00002DC7$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200074E RID: 1870
		internal static class Cross_00002DC7$BurstDirectCall
		{
			// Token: 0x06002E37 RID: 11831 RVA: 0x0004F31E File Offset: 0x0004D51E
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Cross_00002DC7$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Cross_00002DC7$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Cross_00002DC7$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Cross$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Cross_00002DC7$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Cross_00002DC7$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E38 RID: 11832 RVA: 0x0012A7CC File Offset: 0x001289CC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Cross_00002DC7$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E39 RID: 11833 RVA: 0x0004F34A File Offset: 0x0004D54A
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Cross_00002DC7$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Cross(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E3A RID: 11834 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E3B RID: 11835 RVA: 0x0004F35B File Offset: 0x0004D55B
			// Note: this type is marked as 'beforefieldinit'.
			static Cross_00002DC7$BurstDirectCall()
			{
				Bindings.Vec3Functions.Cross_00002DC7$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E3C RID: 11836 RVA: 0x0012A7E4 File Offset: 0x001289E4
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Cross_00002DC7$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Cross$BurstManaged(L);
			}

			// Token: 0x0400333A RID: 13114
			private static IntPtr Pointer;

			// Token: 0x0400333B RID: 13115
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200074F RID: 1871
		// (Invoke) Token: 0x06002E3E RID: 11838
		public unsafe delegate int Project_00002DC8$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000750 RID: 1872
		internal static class Project_00002DC8$BurstDirectCall
		{
			// Token: 0x06002E41 RID: 11841 RVA: 0x0004F362 File Offset: 0x0004D562
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Project_00002DC8$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Project_00002DC8$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Project_00002DC8$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Project$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Project_00002DC8$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Project_00002DC8$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E42 RID: 11842 RVA: 0x0012A818 File Offset: 0x00128A18
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Project_00002DC8$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E43 RID: 11843 RVA: 0x0004F38E File Offset: 0x0004D58E
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Project_00002DC8$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Project(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E44 RID: 11844 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E45 RID: 11845 RVA: 0x0004F39F File Offset: 0x0004D59F
			// Note: this type is marked as 'beforefieldinit'.
			static Project_00002DC8$BurstDirectCall()
			{
				Bindings.Vec3Functions.Project_00002DC8$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E46 RID: 11846 RVA: 0x0012A830 File Offset: 0x00128A30
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Project_00002DC8$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Project$BurstManaged(L);
			}

			// Token: 0x0400333C RID: 13116
			private static IntPtr Pointer;

			// Token: 0x0400333D RID: 13117
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000751 RID: 1873
		// (Invoke) Token: 0x06002E48 RID: 11848
		public unsafe delegate int Length_00002DC9$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000752 RID: 1874
		internal static class Length_00002DC9$BurstDirectCall
		{
			// Token: 0x06002E4B RID: 11851 RVA: 0x0004F3A6 File Offset: 0x0004D5A6
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Length_00002DC9$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Length_00002DC9$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Length_00002DC9$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Length$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Length_00002DC9$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Length_00002DC9$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E4C RID: 11852 RVA: 0x0012A864 File Offset: 0x00128A64
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Length_00002DC9$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E4D RID: 11853 RVA: 0x0004F3D2 File Offset: 0x0004D5D2
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Length_00002DC9$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Length(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E4E RID: 11854 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E4F RID: 11855 RVA: 0x0004F3E3 File Offset: 0x0004D5E3
			// Note: this type is marked as 'beforefieldinit'.
			static Length_00002DC9$BurstDirectCall()
			{
				Bindings.Vec3Functions.Length_00002DC9$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E50 RID: 11856 RVA: 0x0012A87C File Offset: 0x00128A7C
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Length_00002DC9$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Length$BurstManaged(L);
			}

			// Token: 0x0400333E RID: 13118
			private static IntPtr Pointer;

			// Token: 0x0400333F RID: 13119
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000753 RID: 1875
		// (Invoke) Token: 0x06002E52 RID: 11858
		public unsafe delegate int Normalize_00002DCA$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000754 RID: 1876
		internal static class Normalize_00002DCA$BurstDirectCall
		{
			// Token: 0x06002E55 RID: 11861 RVA: 0x0004F3EA File Offset: 0x0004D5EA
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Normalize_00002DCA$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Normalize_00002DCA$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Normalize_00002DCA$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Normalize$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Normalize_00002DCA$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Normalize_00002DCA$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E56 RID: 11862 RVA: 0x0012A8B0 File Offset: 0x00128AB0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Normalize_00002DCA$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E57 RID: 11863 RVA: 0x0004F416 File Offset: 0x0004D616
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Normalize_00002DCA$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Normalize(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E58 RID: 11864 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E59 RID: 11865 RVA: 0x0004F427 File Offset: 0x0004D627
			// Note: this type is marked as 'beforefieldinit'.
			static Normalize_00002DCA$BurstDirectCall()
			{
				Bindings.Vec3Functions.Normalize_00002DCA$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E5A RID: 11866 RVA: 0x0012A8C8 File Offset: 0x00128AC8
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Normalize_00002DCA$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Normalize$BurstManaged(L);
			}

			// Token: 0x04003340 RID: 13120
			private static IntPtr Pointer;

			// Token: 0x04003341 RID: 13121
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000755 RID: 1877
		// (Invoke) Token: 0x06002E5C RID: 11868
		public unsafe delegate int SafeNormal_00002DCB$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000756 RID: 1878
		internal static class SafeNormal_00002DCB$BurstDirectCall
		{
			// Token: 0x06002E5F RID: 11871 RVA: 0x0004F42E File Offset: 0x0004D62E
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.SafeNormal_00002DCB$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.SafeNormal_00002DCB$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.SafeNormal_00002DCB$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.SafeNormal$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.SafeNormal_00002DCB$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.SafeNormal_00002DCB$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E60 RID: 11872 RVA: 0x0012A8FC File Offset: 0x00128AFC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.SafeNormal_00002DCB$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E61 RID: 11873 RVA: 0x0004F45A File Offset: 0x0004D65A
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.SafeNormal_00002DCB$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.SafeNormal(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E62 RID: 11874 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E63 RID: 11875 RVA: 0x0004F46B File Offset: 0x0004D66B
			// Note: this type is marked as 'beforefieldinit'.
			static SafeNormal_00002DCB$BurstDirectCall()
			{
				Bindings.Vec3Functions.SafeNormal_00002DCB$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E64 RID: 11876 RVA: 0x0012A914 File Offset: 0x00128B14
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.SafeNormal_00002DCB$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.SafeNormal$BurstManaged(L);
			}

			// Token: 0x04003342 RID: 13122
			private static IntPtr Pointer;

			// Token: 0x04003343 RID: 13123
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000757 RID: 1879
		// (Invoke) Token: 0x06002E66 RID: 11878
		public unsafe delegate int Distance_00002DCC$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000758 RID: 1880
		internal static class Distance_00002DCC$BurstDirectCall
		{
			// Token: 0x06002E69 RID: 11881 RVA: 0x0004F472 File Offset: 0x0004D672
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Distance_00002DCC$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Distance_00002DCC$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Distance_00002DCC$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Distance$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Distance_00002DCC$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Distance_00002DCC$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E6A RID: 11882 RVA: 0x0012A948 File Offset: 0x00128B48
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Distance_00002DCC$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E6B RID: 11883 RVA: 0x0004F49E File Offset: 0x0004D69E
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Distance_00002DCC$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Distance(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E6C RID: 11884 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E6D RID: 11885 RVA: 0x0004F4AF File Offset: 0x0004D6AF
			// Note: this type is marked as 'beforefieldinit'.
			static Distance_00002DCC$BurstDirectCall()
			{
				Bindings.Vec3Functions.Distance_00002DCC$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E6E RID: 11886 RVA: 0x0012A960 File Offset: 0x00128B60
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Distance_00002DCC$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Distance$BurstManaged(L);
			}

			// Token: 0x04003344 RID: 13124
			private static IntPtr Pointer;

			// Token: 0x04003345 RID: 13125
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000759 RID: 1881
		// (Invoke) Token: 0x06002E70 RID: 11888
		public unsafe delegate int Lerp_00002DCD$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200075A RID: 1882
		internal static class Lerp_00002DCD$BurstDirectCall
		{
			// Token: 0x06002E73 RID: 11891 RVA: 0x0004F4B6 File Offset: 0x0004D6B6
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Lerp_00002DCD$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Lerp_00002DCD$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Lerp_00002DCD$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Lerp$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Lerp_00002DCD$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Lerp_00002DCD$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E74 RID: 11892 RVA: 0x0012A994 File Offset: 0x00128B94
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Lerp_00002DCD$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E75 RID: 11893 RVA: 0x0004F4E2 File Offset: 0x0004D6E2
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Lerp_00002DCD$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Lerp(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E76 RID: 11894 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E77 RID: 11895 RVA: 0x0004F4F3 File Offset: 0x0004D6F3
			// Note: this type is marked as 'beforefieldinit'.
			static Lerp_00002DCD$BurstDirectCall()
			{
				Bindings.Vec3Functions.Lerp_00002DCD$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E78 RID: 11896 RVA: 0x0012A9AC File Offset: 0x00128BAC
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Lerp_00002DCD$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Lerp$BurstManaged(L);
			}

			// Token: 0x04003346 RID: 13126
			private static IntPtr Pointer;

			// Token: 0x04003347 RID: 13127
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200075B RID: 1883
		// (Invoke) Token: 0x06002E7A RID: 11898
		public unsafe delegate int Rotate_00002DCE$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200075C RID: 1884
		internal static class Rotate_00002DCE$BurstDirectCall
		{
			// Token: 0x06002E7D RID: 11901 RVA: 0x0004F4FA File Offset: 0x0004D6FA
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Rotate_00002DCE$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Rotate_00002DCE$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Rotate_00002DCE$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Rotate$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Rotate_00002DCE$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Rotate_00002DCE$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E7E RID: 11902 RVA: 0x0012A9E0 File Offset: 0x00128BE0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Rotate_00002DCE$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E7F RID: 11903 RVA: 0x0004F526 File Offset: 0x0004D726
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Rotate_00002DCE$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Rotate(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E80 RID: 11904 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E81 RID: 11905 RVA: 0x0004F537 File Offset: 0x0004D737
			// Note: this type is marked as 'beforefieldinit'.
			static Rotate_00002DCE$BurstDirectCall()
			{
				Bindings.Vec3Functions.Rotate_00002DCE$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E82 RID: 11906 RVA: 0x0012A9F8 File Offset: 0x00128BF8
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Rotate_00002DCE$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Rotate$BurstManaged(L);
			}

			// Token: 0x04003348 RID: 13128
			private static IntPtr Pointer;

			// Token: 0x04003349 RID: 13129
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200075D RID: 1885
		// (Invoke) Token: 0x06002E84 RID: 11908
		public unsafe delegate int ZeroVector_00002DCF$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200075E RID: 1886
		internal static class ZeroVector_00002DCF$BurstDirectCall
		{
			// Token: 0x06002E87 RID: 11911 RVA: 0x0004F53E File Offset: 0x0004D73E
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.ZeroVector_00002DCF$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.ZeroVector_00002DCF$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.ZeroVector_00002DCF$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.ZeroVector$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.ZeroVector_00002DCF$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.ZeroVector_00002DCF$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E88 RID: 11912 RVA: 0x0012AA2C File Offset: 0x00128C2C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.ZeroVector_00002DCF$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E89 RID: 11913 RVA: 0x0004F56A File Offset: 0x0004D76A
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.ZeroVector_00002DCF$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.ZeroVector(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E8A RID: 11914 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E8B RID: 11915 RVA: 0x0004F57B File Offset: 0x0004D77B
			// Note: this type is marked as 'beforefieldinit'.
			static ZeroVector_00002DCF$BurstDirectCall()
			{
				Bindings.Vec3Functions.ZeroVector_00002DCF$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E8C RID: 11916 RVA: 0x0012AA44 File Offset: 0x00128C44
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.ZeroVector_00002DCF$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.ZeroVector$BurstManaged(L);
			}

			// Token: 0x0400334A RID: 13130
			private static IntPtr Pointer;

			// Token: 0x0400334B RID: 13131
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200075F RID: 1887
		// (Invoke) Token: 0x06002E8E RID: 11918
		public unsafe delegate int OneVector_00002DD0$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000760 RID: 1888
		internal static class OneVector_00002DD0$BurstDirectCall
		{
			// Token: 0x06002E91 RID: 11921 RVA: 0x0004F582 File Offset: 0x0004D782
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.OneVector_00002DD0$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.OneVector_00002DD0$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.OneVector_00002DD0$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.OneVector$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.OneVector_00002DD0$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.OneVector_00002DD0$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E92 RID: 11922 RVA: 0x0012AA78 File Offset: 0x00128C78
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.OneVector_00002DD0$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E93 RID: 11923 RVA: 0x0004F5AE File Offset: 0x0004D7AE
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.OneVector_00002DD0$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.OneVector(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E94 RID: 11924 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002E95 RID: 11925 RVA: 0x0004F5BF File Offset: 0x0004D7BF
			// Note: this type is marked as 'beforefieldinit'.
			static OneVector_00002DD0$BurstDirectCall()
			{
				Bindings.Vec3Functions.OneVector_00002DD0$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E96 RID: 11926 RVA: 0x0012AA90 File Offset: 0x00128C90
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.OneVector_00002DD0$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.OneVector$BurstManaged(L);
			}

			// Token: 0x0400334C RID: 13132
			private static IntPtr Pointer;

			// Token: 0x0400334D RID: 13133
			private static IntPtr DeferredCompilation;
		}
	}

	// Token: 0x02000761 RID: 1889
	[BurstCompile]
	public static class QuatFunctions
	{
		// Token: 0x06002E97 RID: 11927 RVA: 0x0004F5C6 File Offset: 0x0004D7C6
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int New(lua_State* L)
		{
			return Bindings.QuatFunctions.New_00002DD1$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x0004F5CE File Offset: 0x0004D7CE
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Mul(lua_State* L)
		{
			return Bindings.QuatFunctions.Mul_00002DD2$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x0004F5D6 File Offset: 0x0004D7D6
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Eq(lua_State* L)
		{
			return Bindings.QuatFunctions.Eq_00002DD3$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x0012AAC4 File Offset: 0x00128CC4
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int ToString(lua_State* L)
		{
			Quaternion quaternion = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			Luau.lua_pushstring(L, quaternion.ToString());
			return 1;
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x0004F5DE File Offset: 0x0004D7DE
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int FromEuler(lua_State* L)
		{
			return Bindings.QuatFunctions.FromEuler_00002DD5$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x0004F5E6 File Offset: 0x0004D7E6
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int FromDirection(lua_State* L)
		{
			return Bindings.QuatFunctions.FromDirection_00002DD6$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x0004F5EE File Offset: 0x0004D7EE
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int GetUpVector(lua_State* L)
		{
			return Bindings.QuatFunctions.GetUpVector_00002DD7$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x0004F5F6 File Offset: 0x0004D7F6
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Euler(lua_State* L)
		{
			return Bindings.QuatFunctions.Euler_00002DD8$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x0012AB00 File Offset: 0x00128D00
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int New$BurstManaged(lua_State* L)
		{
			Quaternion* ptr = Luau.lua_class_push<Quaternion>(L, "Quat");
			ptr->x = (float)Luau.luaL_optnumber(L, 1, 0.0);
			ptr->y = (float)Luau.luaL_optnumber(L, 2, 0.0);
			ptr->z = (float)Luau.luaL_optnumber(L, 3, 0.0);
			ptr->w = (float)Luau.luaL_optnumber(L, 4, 0.0);
			return 1;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x0012AB7C File Offset: 0x00128D7C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Mul$BurstManaged(lua_State* L)
		{
			Quaternion lhs = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			Quaternion rhs = *Luau.lua_class_get<Quaternion>(L, 2, "Quat");
			*Luau.lua_class_push<Quaternion>(L, "Quat") = lhs * rhs;
			return 1;
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x0012ABD4 File Offset: 0x00128DD4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Eq$BurstManaged(lua_State* L)
		{
			Quaternion lhs = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			Quaternion rhs = *Luau.lua_class_get<Quaternion>(L, 2, "Quat");
			int num = (lhs == rhs) ? 1 : 0;
			Luau.lua_pushnumber(L, (double)num);
			return 1;
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x0012AC24 File Offset: 0x00128E24
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int FromEuler$BurstManaged(lua_State* L)
		{
			float x = (float)Luau.luaL_optnumber(L, 1, 0.0);
			float y = (float)Luau.luaL_optnumber(L, 2, 0.0);
			float z = (float)Luau.luaL_optnumber(L, 3, 0.0);
			Luau.lua_class_push<Quaternion>(L, "Quat")->eulerAngles = new Vector3(x, y, z);
			return 1;
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x0012AC88 File Offset: 0x00128E88
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int FromDirection$BurstManaged(lua_State* L)
		{
			Vector3 lookRotation = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Luau.lua_class_push<Quaternion>(L, "Quat")->SetLookRotation(lookRotation);
			return 1;
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x0012ACC4 File Offset: 0x00128EC4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int GetUpVector$BurstManaged(lua_State* L)
		{
			Quaternion rotation = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = rotation * Vector3.up;
			return 1;
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x0012AD0C File Offset: 0x00128F0C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Euler$BurstManaged(lua_State* L)
		{
			Quaternion quaternion = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = quaternion.eulerAngles;
			return 1;
		}

		// Token: 0x02000762 RID: 1890
		// (Invoke) Token: 0x06002EA7 RID: 11943
		public unsafe delegate int New_00002DD1$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000763 RID: 1891
		internal static class New_00002DD1$BurstDirectCall
		{
			// Token: 0x06002EAA RID: 11946 RVA: 0x0004F5FE File Offset: 0x0004D7FE
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.New_00002DD1$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.New_00002DD1$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.New_00002DD1$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.New$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.New_00002DD1$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.New_00002DD1$BurstDirectCall.Pointer;
			}

			// Token: 0x06002EAB RID: 11947 RVA: 0x0012AD50 File Offset: 0x00128F50
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.New_00002DD1$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002EAC RID: 11948 RVA: 0x0004F62A File Offset: 0x0004D82A
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.New_00002DD1$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.New(lua_State*)).MethodHandle);
			}

			// Token: 0x06002EAD RID: 11949 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002EAE RID: 11950 RVA: 0x0004F63B File Offset: 0x0004D83B
			// Note: this type is marked as 'beforefieldinit'.
			static New_00002DD1$BurstDirectCall()
			{
				Bindings.QuatFunctions.New_00002DD1$BurstDirectCall.Constructor();
			}

			// Token: 0x06002EAF RID: 11951 RVA: 0x0012AD68 File Offset: 0x00128F68
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.New_00002DD1$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.New$BurstManaged(L);
			}

			// Token: 0x0400334E RID: 13134
			private static IntPtr Pointer;

			// Token: 0x0400334F RID: 13135
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000764 RID: 1892
		// (Invoke) Token: 0x06002EB1 RID: 11953
		public unsafe delegate int Mul_00002DD2$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000765 RID: 1893
		internal static class Mul_00002DD2$BurstDirectCall
		{
			// Token: 0x06002EB4 RID: 11956 RVA: 0x0004F642 File Offset: 0x0004D842
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.Mul_00002DD2$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.Mul_00002DD2$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.Mul_00002DD2$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.Mul$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.Mul_00002DD2$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.Mul_00002DD2$BurstDirectCall.Pointer;
			}

			// Token: 0x06002EB5 RID: 11957 RVA: 0x0012AD9C File Offset: 0x00128F9C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.Mul_00002DD2$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002EB6 RID: 11958 RVA: 0x0004F66E File Offset: 0x0004D86E
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.Mul_00002DD2$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.Mul(lua_State*)).MethodHandle);
			}

			// Token: 0x06002EB7 RID: 11959 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002EB8 RID: 11960 RVA: 0x0004F67F File Offset: 0x0004D87F
			// Note: this type is marked as 'beforefieldinit'.
			static Mul_00002DD2$BurstDirectCall()
			{
				Bindings.QuatFunctions.Mul_00002DD2$BurstDirectCall.Constructor();
			}

			// Token: 0x06002EB9 RID: 11961 RVA: 0x0012ADB4 File Offset: 0x00128FB4
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.Mul_00002DD2$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.Mul$BurstManaged(L);
			}

			// Token: 0x04003350 RID: 13136
			private static IntPtr Pointer;

			// Token: 0x04003351 RID: 13137
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000766 RID: 1894
		// (Invoke) Token: 0x06002EBB RID: 11963
		public unsafe delegate int Eq_00002DD3$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000767 RID: 1895
		internal static class Eq_00002DD3$BurstDirectCall
		{
			// Token: 0x06002EBE RID: 11966 RVA: 0x0004F686 File Offset: 0x0004D886
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.Eq_00002DD3$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.Eq_00002DD3$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.Eq_00002DD3$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.Eq$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.Eq_00002DD3$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.Eq_00002DD3$BurstDirectCall.Pointer;
			}

			// Token: 0x06002EBF RID: 11967 RVA: 0x0012ADE8 File Offset: 0x00128FE8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.Eq_00002DD3$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002EC0 RID: 11968 RVA: 0x0004F6B2 File Offset: 0x0004D8B2
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.Eq_00002DD3$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.Eq(lua_State*)).MethodHandle);
			}

			// Token: 0x06002EC1 RID: 11969 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002EC2 RID: 11970 RVA: 0x0004F6C3 File Offset: 0x0004D8C3
			// Note: this type is marked as 'beforefieldinit'.
			static Eq_00002DD3$BurstDirectCall()
			{
				Bindings.QuatFunctions.Eq_00002DD3$BurstDirectCall.Constructor();
			}

			// Token: 0x06002EC3 RID: 11971 RVA: 0x0012AE00 File Offset: 0x00129000
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.Eq_00002DD3$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.Eq$BurstManaged(L);
			}

			// Token: 0x04003352 RID: 13138
			private static IntPtr Pointer;

			// Token: 0x04003353 RID: 13139
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000768 RID: 1896
		// (Invoke) Token: 0x06002EC5 RID: 11973
		public unsafe delegate int FromEuler_00002DD5$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000769 RID: 1897
		internal static class FromEuler_00002DD5$BurstDirectCall
		{
			// Token: 0x06002EC8 RID: 11976 RVA: 0x0004F6CA File Offset: 0x0004D8CA
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.FromEuler_00002DD5$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.FromEuler_00002DD5$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.FromEuler_00002DD5$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.FromEuler$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.FromEuler_00002DD5$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.FromEuler_00002DD5$BurstDirectCall.Pointer;
			}

			// Token: 0x06002EC9 RID: 11977 RVA: 0x0012AE34 File Offset: 0x00129034
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.FromEuler_00002DD5$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002ECA RID: 11978 RVA: 0x0004F6F6 File Offset: 0x0004D8F6
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.FromEuler_00002DD5$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.FromEuler(lua_State*)).MethodHandle);
			}

			// Token: 0x06002ECB RID: 11979 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002ECC RID: 11980 RVA: 0x0004F707 File Offset: 0x0004D907
			// Note: this type is marked as 'beforefieldinit'.
			static FromEuler_00002DD5$BurstDirectCall()
			{
				Bindings.QuatFunctions.FromEuler_00002DD5$BurstDirectCall.Constructor();
			}

			// Token: 0x06002ECD RID: 11981 RVA: 0x0012AE4C File Offset: 0x0012904C
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.FromEuler_00002DD5$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.FromEuler$BurstManaged(L);
			}

			// Token: 0x04003354 RID: 13140
			private static IntPtr Pointer;

			// Token: 0x04003355 RID: 13141
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200076A RID: 1898
		// (Invoke) Token: 0x06002ECF RID: 11983
		public unsafe delegate int FromDirection_00002DD6$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200076B RID: 1899
		internal static class FromDirection_00002DD6$BurstDirectCall
		{
			// Token: 0x06002ED2 RID: 11986 RVA: 0x0004F70E File Offset: 0x0004D90E
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.FromDirection_00002DD6$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.FromDirection_00002DD6$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.FromDirection_00002DD6$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.FromDirection$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.FromDirection_00002DD6$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.FromDirection_00002DD6$BurstDirectCall.Pointer;
			}

			// Token: 0x06002ED3 RID: 11987 RVA: 0x0012AE80 File Offset: 0x00129080
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.FromDirection_00002DD6$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002ED4 RID: 11988 RVA: 0x0004F73A File Offset: 0x0004D93A
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.FromDirection_00002DD6$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.FromDirection(lua_State*)).MethodHandle);
			}

			// Token: 0x06002ED5 RID: 11989 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002ED6 RID: 11990 RVA: 0x0004F74B File Offset: 0x0004D94B
			// Note: this type is marked as 'beforefieldinit'.
			static FromDirection_00002DD6$BurstDirectCall()
			{
				Bindings.QuatFunctions.FromDirection_00002DD6$BurstDirectCall.Constructor();
			}

			// Token: 0x06002ED7 RID: 11991 RVA: 0x0012AE98 File Offset: 0x00129098
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.FromDirection_00002DD6$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.FromDirection$BurstManaged(L);
			}

			// Token: 0x04003356 RID: 13142
			private static IntPtr Pointer;

			// Token: 0x04003357 RID: 13143
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200076C RID: 1900
		// (Invoke) Token: 0x06002ED9 RID: 11993
		public unsafe delegate int GetUpVector_00002DD7$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200076D RID: 1901
		internal static class GetUpVector_00002DD7$BurstDirectCall
		{
			// Token: 0x06002EDC RID: 11996 RVA: 0x0004F752 File Offset: 0x0004D952
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.GetUpVector_00002DD7$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.GetUpVector_00002DD7$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.GetUpVector_00002DD7$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.GetUpVector$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.GetUpVector_00002DD7$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.GetUpVector_00002DD7$BurstDirectCall.Pointer;
			}

			// Token: 0x06002EDD RID: 11997 RVA: 0x0012AECC File Offset: 0x001290CC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.GetUpVector_00002DD7$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002EDE RID: 11998 RVA: 0x0004F77E File Offset: 0x0004D97E
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.GetUpVector_00002DD7$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.GetUpVector(lua_State*)).MethodHandle);
			}

			// Token: 0x06002EDF RID: 11999 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002EE0 RID: 12000 RVA: 0x0004F78F File Offset: 0x0004D98F
			// Note: this type is marked as 'beforefieldinit'.
			static GetUpVector_00002DD7$BurstDirectCall()
			{
				Bindings.QuatFunctions.GetUpVector_00002DD7$BurstDirectCall.Constructor();
			}

			// Token: 0x06002EE1 RID: 12001 RVA: 0x0012AEE4 File Offset: 0x001290E4
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.GetUpVector_00002DD7$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.GetUpVector$BurstManaged(L);
			}

			// Token: 0x04003358 RID: 13144
			private static IntPtr Pointer;

			// Token: 0x04003359 RID: 13145
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200076E RID: 1902
		// (Invoke) Token: 0x06002EE3 RID: 12003
		public unsafe delegate int Euler_00002DD8$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200076F RID: 1903
		internal static class Euler_00002DD8$BurstDirectCall
		{
			// Token: 0x06002EE6 RID: 12006 RVA: 0x0004F796 File Offset: 0x0004D996
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.Euler_00002DD8$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.Euler_00002DD8$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.Euler_00002DD8$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.Euler$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.Euler_00002DD8$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.Euler_00002DD8$BurstDirectCall.Pointer;
			}

			// Token: 0x06002EE7 RID: 12007 RVA: 0x0012AF18 File Offset: 0x00129118
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.Euler_00002DD8$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002EE8 RID: 12008 RVA: 0x0004F7C2 File Offset: 0x0004D9C2
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.Euler_00002DD8$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.Euler(lua_State*)).MethodHandle);
			}

			// Token: 0x06002EE9 RID: 12009 RVA: 0x00030607 File Offset: 0x0002E807
			public static void Initialize()
			{
			}

			// Token: 0x06002EEA RID: 12010 RVA: 0x0004F7D3 File Offset: 0x0004D9D3
			// Note: this type is marked as 'beforefieldinit'.
			static Euler_00002DD8$BurstDirectCall()
			{
				Bindings.QuatFunctions.Euler_00002DD8$BurstDirectCall.Constructor();
			}

			// Token: 0x06002EEB RID: 12011 RVA: 0x0012AF30 File Offset: 0x00129130
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.Euler_00002DD8$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.Euler$BurstManaged(L);
			}

			// Token: 0x0400335A RID: 13146
			private static IntPtr Pointer;

			// Token: 0x0400335B RID: 13147
			private static IntPtr DeferredCompilation;
		}
	}

	// Token: 0x02000770 RID: 1904
	public struct GorillaLocomotionSettings
	{
		// Token: 0x0400335C RID: 13148
		public float velocityLimit;

		// Token: 0x0400335D RID: 13149
		public float slideVelocityLimit;

		// Token: 0x0400335E RID: 13150
		public float maxJumpSpeed;

		// Token: 0x0400335F RID: 13151
		public float jumpMultiplier;
	}
}
