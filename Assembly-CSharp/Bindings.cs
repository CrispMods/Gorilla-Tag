using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using ExitGames.Client.Photon;
using GorillaLocomotion;
using Photon.Pun;
using Photon.Realtime;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

// Token: 0x0200071D RID: 1821
[BurstCompile]
public static class Bindings
{
	// Token: 0x06002D0D RID: 11533 RVA: 0x000DEE30 File Offset: 0x000DD030
	public unsafe static void GameObjectBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Bindings.LuauGameObject>("GameObject").AddField("position", "Position").AddField("rotation", "Rotation").AddField("scale", "Scale").AddStaticFunction("findGameObject", new lua_CFunction(Bindings.GameObjectFunctions.FindGameObject)).AddFunction("setCollision", new lua_CFunction(Bindings.GameObjectFunctions.SetCollision)).AddFunction("setVisibility", new lua_CFunction(Bindings.GameObjectFunctions.SetVisibility)).Build(L, true));
	}

	// Token: 0x06002D0E RID: 11534 RVA: 0x000DEEC8 File Offset: 0x000DD0C8
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

	// Token: 0x06002D0F RID: 11535 RVA: 0x000DEF7C File Offset: 0x000DD17C
	public unsafe static void Vec3Builder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Vector3>("Vec3").AddField("x", null).AddField("y", null).AddField("z", null).AddStaticFunction("new", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.New))).AddFunction("__add", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Add))).AddFunction("__sub", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Sub))).AddFunction("__mul", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Mul))).AddFunction("__div", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Div))).AddFunction("__unm", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Unm))).AddFunction("__eq", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Eq))).AddFunction("__tostring", new lua_CFunction(Bindings.Vec3Functions.ToSring)).AddFunction("toString", new lua_CFunction(Bindings.Vec3Functions.ToSring)).AddFunction("dot", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Dot))).AddFunction("cross", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Cross))).AddFunction("projectOnTo", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Project))).AddFunction("length", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Length))).AddFunction("normalize", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Normalize))).AddFunction("getSafeNormal", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.SafeNormal))).AddStaticFunction("rotate", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Rotate))).AddFunction("rotate", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Rotate))).AddStaticFunction("distance", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Distance))).AddFunction("distance", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Distance))).AddStaticFunction("lerp", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Lerp))).AddFunction("lerp", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Lerp))).AddProperty("zeroVector", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.ZeroVector))).AddProperty("oneVector", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.OneVector))).Build(L, true));
	}

	// Token: 0x06002D10 RID: 11536 RVA: 0x000DF22C File Offset: 0x000DD42C
	public unsafe static void QuatBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Quaternion>("Quat").AddField("x", null).AddField("y", null).AddField("z", null).AddField("w", null).AddStaticFunction("new", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.New))).AddFunction("__mul", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.Mul))).AddFunction("__eq", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.Eq))).AddFunction("__tostring", new lua_CFunction(Bindings.QuatFunctions.ToString)).AddFunction("toString", new lua_CFunction(Bindings.QuatFunctions.ToString)).AddStaticFunction("fromEuler", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.FromEuler))).AddStaticFunction("fromDirection", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.FromDirection))).AddFunction("getUpVector", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.GetUpVector))).AddFunction("euler", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.Euler))).Build(L, true));
	}

	// Token: 0x06002D11 RID: 11537 RVA: 0x000DF36C File Offset: 0x000DD56C
	public unsafe static void PlayerBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Bindings.LuauPlayer>("Player").AddField("playerID", "PlayerID").AddField("playerName", "PlayerName").AddField("playerMaterial", "PlayerMaterial").AddField("isMasterClient", "IsMasterClient").AddField("bodyPosition", "BodyPosition").AddField("leftHandPosition", "LeftHandPosition").AddField("rightHandPosition", "RightHandPosition").AddField("headRotation", "HeadRotation").AddField("leftHandRotation", "LeftHandRotation").AddField("rightHandRotation", "RightHandRotation").AddStaticFunction("getPlayerByID", new lua_CFunction(Bindings.PlayerFunctions.GetPlayerByID)).Build(L, true));
	}

	// Token: 0x06002D12 RID: 11538 RVA: 0x000DF444 File Offset: 0x000DD644
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int LuaStartVibration(lua_State* L)
	{
		bool forLeftController = Luau.lua_toboolean(L, 1) == 1;
		float amplitude = (float)Luau.luaL_checknumber(L, 2);
		float duration = (float)Luau.luaL_checknumber(L, 3);
		GorillaTagger.Instance.StartVibration(forLeftController, amplitude, duration);
		return 0;
	}

	// Token: 0x06002D13 RID: 11539 RVA: 0x000DF47C File Offset: 0x000DD67C
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

	// Token: 0x04003276 RID: 12918
	public static Dictionary<GameObject, IntPtr> LuauGameObjectList = new Dictionary<GameObject, IntPtr>();

	// Token: 0x04003277 RID: 12919
	public static Dictionary<int, IntPtr> LuauPlayerList = new Dictionary<int, IntPtr>();

	// Token: 0x04003278 RID: 12920
	public static Dictionary<int, VRRig> LuauVRRigList = new Dictionary<int, VRRig>();

	// Token: 0x04003279 RID: 12921
	public unsafe static Bindings.GorillaLocomotionSettings* LocomotionSettings;

	// Token: 0x0200071E RID: 1822
	public static class LuaEmit
	{
		// Token: 0x06002D15 RID: 11541 RVA: 0x000DF4FC File Offset: 0x000DD6FC
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
				Debug.Log("Emit rate limit reached, event not sent");
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
				if (lua_Types != Luau.lua_Types.LUA_TBOOLEAN)
				{
					if (lua_Types != Luau.lua_Types.LUA_TNUMBER)
					{
						if (lua_Types != Luau.lua_Types.LUA_TUSERDATA)
						{
							FixedString32Bytes fixedString32Bytes = "\"Unknown Type in table\"";
							Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes) + 2));
							return 0;
						}
						Luau.lua_getmetatable(L, -1);
						Luau.lua_getfield(L, -1, "metahash");
						BurstClassInfo.ClassInfo classInfo;
						if (!BurstClassInfo.ClassList.InfoFields.Data.TryGetValue((int)Luau.luaL_checknumber(L, -1), out classInfo))
						{
							FixedString32Bytes fixedString32Bytes2 = "\"Internal Class Info Error\"";
							Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes2) + 2));
							return 0;
						}
						FixedString32Bytes fixedString32Bytes3 = "Vec3";
						if (classInfo.Name == fixedString32Bytes3)
						{
							list.Add(*Luau.lua_class_get<Vector3>(L, -3));
							Luau.lua_pop(L, 3);
						}
						else
						{
							fixedString32Bytes3 = "Quat";
							if (classInfo.Name == fixedString32Bytes3)
							{
								list.Add(*Luau.lua_class_get<Quaternion>(L, -3));
								Luau.lua_pop(L, 3);
							}
							else
							{
								fixedString32Bytes3 = "Player";
								if (classInfo.Name == fixedString32Bytes3)
								{
									int playerID = Luau.lua_class_get<Bindings.LuauPlayer>(L, -3)->PlayerID;
									list.Add(playerID);
									Luau.lua_pop(L, 3);
								}
								else
								{
									FixedString32Bytes fixedString32Bytes4 = "\"Unknown Type in table\"";
									Luau.luaL_errorL(L, (sbyte*)((byte*)UnsafeUtility.AddressOf<FixedString32Bytes>(ref fixedString32Bytes4) + 2));
								}
							}
						}
					}
					else
					{
						list.Add(Luau.luaL_checknumber(L, -1));
						Luau.lua_pop(L, 1);
					}
				}
				else
				{
					list.Add(Luau.luaL_checknumber(L, -1) == 1.0);
					Luau.lua_pop(L, 1);
				}
			}
			if (PhotonNetwork.InRoom)
			{
				PhotonNetwork.RaiseEvent(180, list.ToArray(), raiseEventOptions, SendOptions.SendReliable);
			}
			return 0;
		}

		// Token: 0x0400327A RID: 12922
		private static float callTime = 0f;

		// Token: 0x0400327B RID: 12923
		private static float callCount = 20f;
	}

	// Token: 0x0200071F RID: 1823
	[BurstCompile]
	public struct LuauGameObject
	{
		// Token: 0x0400327C RID: 12924
		public Vector3 Position;

		// Token: 0x0400327D RID: 12925
		public Quaternion Rotation;

		// Token: 0x0400327E RID: 12926
		public Vector3 Scale;
	}

	// Token: 0x02000720 RID: 1824
	[BurstCompile]
	public static class GameObjectFunctions
	{
		// Token: 0x06002D17 RID: 11543 RVA: 0x000DF7A0 File Offset: 0x000DD9A0
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

		// Token: 0x06002D18 RID: 11544 RVA: 0x000DF804 File Offset: 0x000DDA04
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int FindGameObject(lua_State* L)
		{
			GameObject gameObject = GameObject.Find(new string((sbyte*)Luau.luaL_checkstring(L, 1)));
			if (!(gameObject != null))
			{
				return 0;
			}
			if (!ModIOMapLoader.IsCustomScene(gameObject.scene.name))
			{
				return 0;
			}
			IntPtr value;
			if (Bindings.LuauGameObjectList.TryGetValue(gameObject, out value))
			{
				Luau.lua_pushlightuserdata(L, (void*)value);
				Luau.luaL_getmetatable(L, "GameObject");
				Luau.lua_setmetatable(L, -2);
			}
			else
			{
				Bindings.LuauGameObject* ptr = Luau.lua_class_push<Bindings.LuauGameObject>(L);
				ptr->Position = gameObject.transform.position;
				ptr->Rotation = gameObject.transform.rotation;
				ptr->Scale = gameObject.transform.localScale;
				Bindings.LuauGameObjectList.TryAdd(gameObject, (IntPtr)((void*)ptr));
			}
			return 1;
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000DF8C8 File Offset: 0x000DDAC8
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SetCollision(lua_State* L)
		{
			Bindings.LuauGameObject* data = Luau.lua_class_get<Bindings.LuauGameObject>(L, 1, "GameObject");
			Bindings.LuauGameObjectList.FirstOrDefault((KeyValuePair<GameObject, IntPtr> g) => g.Value == (IntPtr)((void*)data)).Key.GetComponent<Collider>().enabled = (Luau.lua_toboolean(L, 2) == 1);
			return 0;
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000DF928 File Offset: 0x000DDB28
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SetVisibility(lua_State* L)
		{
			Bindings.LuauGameObject* data = Luau.lua_class_get<Bindings.LuauGameObject>(L, 1, "GameObject");
			Bindings.LuauGameObjectList.FirstOrDefault((KeyValuePair<GameObject, IntPtr> g) => g.Value == (IntPtr)((void*)data)).Key.GetComponent<MeshRenderer>().enabled = (Luau.lua_toboolean(L, 2) == 1);
			return 0;
		}
	}

	// Token: 0x02000723 RID: 1827
	[BurstCompile]
	public struct LuauPlayer
	{
		// Token: 0x04003281 RID: 12929
		public int PlayerID;

		// Token: 0x04003282 RID: 12930
		public FixedString32Bytes PlayerName;

		// Token: 0x04003283 RID: 12931
		public int PlayerMaterial;

		// Token: 0x04003284 RID: 12932
		public bool IsMasterClient;

		// Token: 0x04003285 RID: 12933
		public Vector3 BodyPosition;

		// Token: 0x04003286 RID: 12934
		public Vector3 LeftHandPosition;

		// Token: 0x04003287 RID: 12935
		public Vector3 RightHandPosition;

		// Token: 0x04003288 RID: 12936
		public Quaternion HeadRotation;

		// Token: 0x04003289 RID: 12937
		public Quaternion LeftHandRotation;

		// Token: 0x0400328A RID: 12938
		public Quaternion RightHandRotation;
	}

	// Token: 0x02000724 RID: 1828
	[BurstCompile]
	public static class PlayerFunctions
	{
		// Token: 0x06002D1F RID: 11551 RVA: 0x000DF9B8 File Offset: 0x000DDBB8
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int GetPlayerByID(lua_State* L)
		{
			int num = (int)Luau.luaL_checknumber(L, 1);
			foreach (NetPlayer netPlayer in RoomSystem.PlayersInRoom)
			{
				if (netPlayer.ActorNumber == num)
				{
					IntPtr value;
					if (Bindings.LuauPlayerList.TryGetValue(netPlayer.ActorNumber, out value))
					{
						Luau.lua_pushlightuserdata(L, (void*)value);
						Luau.luaL_getmetatable(L, "Player");
						Luau.lua_setmetatable(L, -2);
					}
					else
					{
						Bindings.LuauPlayer* ptr = Luau.lua_class_push<Bindings.LuauPlayer>(L);
						ptr->PlayerID = netPlayer.ActorNumber;
						ptr->PlayerName = netPlayer.SanitizedNickName;
						ptr->PlayerMaterial = 0;
						ptr->IsMasterClient = netPlayer.IsMasterClient;
						Bindings.LuauPlayerList[netPlayer.ActorNumber] = (IntPtr)((void*)ptr);
						GorillaGameManager instance = GorillaGameManager.instance;
						VRRig vrrig = (instance != null) ? instance.FindPlayerVRRig(netPlayer) : null;
						if (vrrig != null)
						{
							ptr->PlayerName = vrrig.playerNameVisible;
							Bindings.LuauVRRigList[netPlayer.ActorNumber] = vrrig;
							Bindings.PlayerFunctions.UpdatePlayer(L, vrrig, ptr);
							Bindings.LuauPlayerList[netPlayer.ActorNumber] = (IntPtr)((void*)ptr);
						}
					}
				}
			}
			return 0;
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000DFB1C File Offset: 0x000DDD1C
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

	// Token: 0x02000725 RID: 1829
	[BurstCompile]
	public static class Vec3Functions
	{
		// Token: 0x06002D21 RID: 11553 RVA: 0x000DFB8F File Offset: 0x000DDD8F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int New(lua_State* L)
		{
			return Bindings.Vec3Functions.New_00002D21$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x000DFB97 File Offset: 0x000DDD97
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Add(lua_State* L)
		{
			return Bindings.Vec3Functions.Add_00002D22$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x000DFB9F File Offset: 0x000DDD9F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Sub(lua_State* L)
		{
			return Bindings.Vec3Functions.Sub_00002D23$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x000DFBA7 File Offset: 0x000DDDA7
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Mul(lua_State* L)
		{
			return Bindings.Vec3Functions.Mul_00002D24$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x000DFBAF File Offset: 0x000DDDAF
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Div(lua_State* L)
		{
			return Bindings.Vec3Functions.Div_00002D25$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x000DFBB7 File Offset: 0x000DDDB7
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Unm(lua_State* L)
		{
			return Bindings.Vec3Functions.Unm_00002D26$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x000DFBBF File Offset: 0x000DDDBF
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Eq(lua_State* L)
		{
			return Bindings.Vec3Functions.Eq_00002D27$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x000DFBC8 File Offset: 0x000DDDC8
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int ToSring(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Luau.lua_pushstring(L, vector.ToString());
			return 1;
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x000DFC01 File Offset: 0x000DDE01
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Dot(lua_State* L)
		{
			return Bindings.Vec3Functions.Dot_00002D29$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x000DFC09 File Offset: 0x000DDE09
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Cross(lua_State* L)
		{
			return Bindings.Vec3Functions.Cross_00002D2A$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x000DFC11 File Offset: 0x000DDE11
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Project(lua_State* L)
		{
			return Bindings.Vec3Functions.Project_00002D2B$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x000DFC19 File Offset: 0x000DDE19
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Length(lua_State* L)
		{
			return Bindings.Vec3Functions.Length_00002D2C$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x000DFC21 File Offset: 0x000DDE21
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Normalize(lua_State* L)
		{
			return Bindings.Vec3Functions.Normalize_00002D2D$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x000DFC29 File Offset: 0x000DDE29
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SafeNormal(lua_State* L)
		{
			return Bindings.Vec3Functions.SafeNormal_00002D2E$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x000DFC31 File Offset: 0x000DDE31
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Distance(lua_State* L)
		{
			return Bindings.Vec3Functions.Distance_00002D2F$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000DFC39 File Offset: 0x000DDE39
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Lerp(lua_State* L)
		{
			return Bindings.Vec3Functions.Lerp_00002D30$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x000DFC41 File Offset: 0x000DDE41
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Rotate(lua_State* L)
		{
			return Bindings.Vec3Functions.Rotate_00002D31$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x000DFC49 File Offset: 0x000DDE49
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int ZeroVector(lua_State* L)
		{
			return Bindings.Vec3Functions.ZeroVector_00002D32$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x000DFC51 File Offset: 0x000DDE51
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int OneVector(lua_State* L)
		{
			return Bindings.Vec3Functions.OneVector_00002D33$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000DFC5C File Offset: 0x000DDE5C
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

		// Token: 0x06002D35 RID: 11573 RVA: 0x000DFCC0 File Offset: 0x000DDEC0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Add$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a + b;
			return 1;
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000DFD18 File Offset: 0x000DDF18
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Sub$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a - b;
			return 1;
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x000DFD70 File Offset: 0x000DDF70
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Mul$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			float d = (float)Luau.luaL_checknumber(L, 2);
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a * d;
			return 1;
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000DFDBC File Offset: 0x000DDFBC
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Div$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			float d = (float)Luau.luaL_checknumber(L, 2);
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a / d;
			return 1;
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x000DFE08 File Offset: 0x000DE008
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Unm$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = -a;
			return 1;
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000DFE48 File Offset: 0x000DE048
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

		// Token: 0x06002D3B RID: 11579 RVA: 0x000DFE98 File Offset: 0x000DE098
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

		// Token: 0x06002D3C RID: 11580 RVA: 0x000DFEE4 File Offset: 0x000DE0E4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Cross$BurstManaged(lua_State* L)
		{
			Vector3 lhs = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 rhs = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = Vector3.Cross(lhs, rhs);
			return 1;
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000DFF3C File Offset: 0x000DE13C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Project$BurstManaged(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 onNormal = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = Vector3.Project(vector, onNormal);
			return 1;
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000DFF94 File Offset: 0x000DE194
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Length$BurstManaged(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Luau.lua_pushnumber(L, (double)Vector3.Magnitude(vector));
			return 1;
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x000DFFC6 File Offset: 0x000DE1C6
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Normalize$BurstManaged(lua_State* L)
		{
			Luau.lua_class_get<Vector3>(L, 1, "Vec3")->Normalize();
			return 0;
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x000DFFE0 File Offset: 0x000DE1E0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int SafeNormal$BurstManaged(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = vector.normalized;
			return 1;
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x000E0024 File Offset: 0x000DE224
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Distance$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			Luau.lua_pushnumber(L, (double)Vector3.Distance(a, b));
			return 1;
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000E0070 File Offset: 0x000DE270
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

		// Token: 0x06002D43 RID: 11587 RVA: 0x000E00D4 File Offset: 0x000DE2D4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Rotate$BurstManaged(lua_State* L)
		{
			Vector3 point = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Quaternion rotation = *Luau.lua_class_get<Quaternion>(L, 2, "Quat");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = rotation * point;
			return 1;
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000E012C File Offset: 0x000DE32C
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

		// Token: 0x06002D45 RID: 11589 RVA: 0x000E015F File Offset: 0x000DE35F
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

		// Token: 0x02000726 RID: 1830
		// (Invoke) Token: 0x06002D47 RID: 11591
		public unsafe delegate int New_00002D21$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000727 RID: 1831
		internal static class New_00002D21$BurstDirectCall
		{
			// Token: 0x06002D4A RID: 11594 RVA: 0x000E0192 File Offset: 0x000DE392
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.New_00002D21$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.New_00002D21$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.New_00002D21$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.New$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.New_00002D21$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.New_00002D21$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D4B RID: 11595 RVA: 0x000E01C0 File Offset: 0x000DE3C0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.New_00002D21$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D4C RID: 11596 RVA: 0x000E01D8 File Offset: 0x000DE3D8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.New_00002D21$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.New(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D4D RID: 11597 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D4E RID: 11598 RVA: 0x000E01E9 File Offset: 0x000DE3E9
			// Note: this type is marked as 'beforefieldinit'.
			static New_00002D21$BurstDirectCall()
			{
				Bindings.Vec3Functions.New_00002D21$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D4F RID: 11599 RVA: 0x000E01F0 File Offset: 0x000DE3F0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.New_00002D21$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.New$BurstManaged(L);
			}

			// Token: 0x0400328B RID: 12939
			private static IntPtr Pointer;

			// Token: 0x0400328C RID: 12940
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000728 RID: 1832
		// (Invoke) Token: 0x06002D51 RID: 11601
		public unsafe delegate int Add_00002D22$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000729 RID: 1833
		internal static class Add_00002D22$BurstDirectCall
		{
			// Token: 0x06002D54 RID: 11604 RVA: 0x000E0221 File Offset: 0x000DE421
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Add_00002D22$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Add_00002D22$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Add_00002D22$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Add$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Add_00002D22$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Add_00002D22$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D55 RID: 11605 RVA: 0x000E0250 File Offset: 0x000DE450
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Add_00002D22$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D56 RID: 11606 RVA: 0x000E0268 File Offset: 0x000DE468
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Add_00002D22$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Add(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D57 RID: 11607 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D58 RID: 11608 RVA: 0x000E0279 File Offset: 0x000DE479
			// Note: this type is marked as 'beforefieldinit'.
			static Add_00002D22$BurstDirectCall()
			{
				Bindings.Vec3Functions.Add_00002D22$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D59 RID: 11609 RVA: 0x000E0280 File Offset: 0x000DE480
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Add_00002D22$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Add$BurstManaged(L);
			}

			// Token: 0x0400328D RID: 12941
			private static IntPtr Pointer;

			// Token: 0x0400328E RID: 12942
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200072A RID: 1834
		// (Invoke) Token: 0x06002D5B RID: 11611
		public unsafe delegate int Sub_00002D23$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200072B RID: 1835
		internal static class Sub_00002D23$BurstDirectCall
		{
			// Token: 0x06002D5E RID: 11614 RVA: 0x000E02B1 File Offset: 0x000DE4B1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Sub_00002D23$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Sub_00002D23$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Sub_00002D23$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Sub$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Sub_00002D23$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Sub_00002D23$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D5F RID: 11615 RVA: 0x000E02E0 File Offset: 0x000DE4E0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Sub_00002D23$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D60 RID: 11616 RVA: 0x000E02F8 File Offset: 0x000DE4F8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Sub_00002D23$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Sub(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D61 RID: 11617 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D62 RID: 11618 RVA: 0x000E0309 File Offset: 0x000DE509
			// Note: this type is marked as 'beforefieldinit'.
			static Sub_00002D23$BurstDirectCall()
			{
				Bindings.Vec3Functions.Sub_00002D23$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D63 RID: 11619 RVA: 0x000E0310 File Offset: 0x000DE510
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Sub_00002D23$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Sub$BurstManaged(L);
			}

			// Token: 0x0400328F RID: 12943
			private static IntPtr Pointer;

			// Token: 0x04003290 RID: 12944
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200072C RID: 1836
		// (Invoke) Token: 0x06002D65 RID: 11621
		public unsafe delegate int Mul_00002D24$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200072D RID: 1837
		internal static class Mul_00002D24$BurstDirectCall
		{
			// Token: 0x06002D68 RID: 11624 RVA: 0x000E0341 File Offset: 0x000DE541
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Mul_00002D24$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Mul_00002D24$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Mul_00002D24$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Mul$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Mul_00002D24$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Mul_00002D24$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D69 RID: 11625 RVA: 0x000E0370 File Offset: 0x000DE570
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Mul_00002D24$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D6A RID: 11626 RVA: 0x000E0388 File Offset: 0x000DE588
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Mul_00002D24$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Mul(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D6B RID: 11627 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D6C RID: 11628 RVA: 0x000E0399 File Offset: 0x000DE599
			// Note: this type is marked as 'beforefieldinit'.
			static Mul_00002D24$BurstDirectCall()
			{
				Bindings.Vec3Functions.Mul_00002D24$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D6D RID: 11629 RVA: 0x000E03A0 File Offset: 0x000DE5A0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Mul_00002D24$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Mul$BurstManaged(L);
			}

			// Token: 0x04003291 RID: 12945
			private static IntPtr Pointer;

			// Token: 0x04003292 RID: 12946
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200072E RID: 1838
		// (Invoke) Token: 0x06002D6F RID: 11631
		public unsafe delegate int Div_00002D25$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200072F RID: 1839
		internal static class Div_00002D25$BurstDirectCall
		{
			// Token: 0x06002D72 RID: 11634 RVA: 0x000E03D1 File Offset: 0x000DE5D1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Div_00002D25$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Div_00002D25$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Div_00002D25$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Div$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Div_00002D25$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Div_00002D25$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D73 RID: 11635 RVA: 0x000E0400 File Offset: 0x000DE600
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Div_00002D25$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D74 RID: 11636 RVA: 0x000E0418 File Offset: 0x000DE618
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Div_00002D25$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Div(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D75 RID: 11637 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D76 RID: 11638 RVA: 0x000E0429 File Offset: 0x000DE629
			// Note: this type is marked as 'beforefieldinit'.
			static Div_00002D25$BurstDirectCall()
			{
				Bindings.Vec3Functions.Div_00002D25$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D77 RID: 11639 RVA: 0x000E0430 File Offset: 0x000DE630
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Div_00002D25$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Div$BurstManaged(L);
			}

			// Token: 0x04003293 RID: 12947
			private static IntPtr Pointer;

			// Token: 0x04003294 RID: 12948
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000730 RID: 1840
		// (Invoke) Token: 0x06002D79 RID: 11641
		public unsafe delegate int Unm_00002D26$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000731 RID: 1841
		internal static class Unm_00002D26$BurstDirectCall
		{
			// Token: 0x06002D7C RID: 11644 RVA: 0x000E0461 File Offset: 0x000DE661
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Unm_00002D26$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Unm_00002D26$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Unm_00002D26$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Unm$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Unm_00002D26$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Unm_00002D26$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D7D RID: 11645 RVA: 0x000E0490 File Offset: 0x000DE690
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Unm_00002D26$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D7E RID: 11646 RVA: 0x000E04A8 File Offset: 0x000DE6A8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Unm_00002D26$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Unm(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D7F RID: 11647 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D80 RID: 11648 RVA: 0x000E04B9 File Offset: 0x000DE6B9
			// Note: this type is marked as 'beforefieldinit'.
			static Unm_00002D26$BurstDirectCall()
			{
				Bindings.Vec3Functions.Unm_00002D26$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D81 RID: 11649 RVA: 0x000E04C0 File Offset: 0x000DE6C0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Unm_00002D26$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Unm$BurstManaged(L);
			}

			// Token: 0x04003295 RID: 12949
			private static IntPtr Pointer;

			// Token: 0x04003296 RID: 12950
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000732 RID: 1842
		// (Invoke) Token: 0x06002D83 RID: 11651
		public unsafe delegate int Eq_00002D27$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000733 RID: 1843
		internal static class Eq_00002D27$BurstDirectCall
		{
			// Token: 0x06002D86 RID: 11654 RVA: 0x000E04F1 File Offset: 0x000DE6F1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Eq_00002D27$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Eq_00002D27$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Eq_00002D27$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Eq$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Eq_00002D27$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Eq_00002D27$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D87 RID: 11655 RVA: 0x000E0520 File Offset: 0x000DE720
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Eq_00002D27$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D88 RID: 11656 RVA: 0x000E0538 File Offset: 0x000DE738
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Eq_00002D27$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Eq(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D89 RID: 11657 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D8A RID: 11658 RVA: 0x000E0549 File Offset: 0x000DE749
			// Note: this type is marked as 'beforefieldinit'.
			static Eq_00002D27$BurstDirectCall()
			{
				Bindings.Vec3Functions.Eq_00002D27$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D8B RID: 11659 RVA: 0x000E0550 File Offset: 0x000DE750
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Eq_00002D27$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Eq$BurstManaged(L);
			}

			// Token: 0x04003297 RID: 12951
			private static IntPtr Pointer;

			// Token: 0x04003298 RID: 12952
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000734 RID: 1844
		// (Invoke) Token: 0x06002D8D RID: 11661
		public unsafe delegate int Dot_00002D29$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000735 RID: 1845
		internal static class Dot_00002D29$BurstDirectCall
		{
			// Token: 0x06002D90 RID: 11664 RVA: 0x000E0581 File Offset: 0x000DE781
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Dot_00002D29$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Dot_00002D29$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Dot_00002D29$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Dot$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Dot_00002D29$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Dot_00002D29$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D91 RID: 11665 RVA: 0x000E05B0 File Offset: 0x000DE7B0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Dot_00002D29$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D92 RID: 11666 RVA: 0x000E05C8 File Offset: 0x000DE7C8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Dot_00002D29$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Dot(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D93 RID: 11667 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D94 RID: 11668 RVA: 0x000E05D9 File Offset: 0x000DE7D9
			// Note: this type is marked as 'beforefieldinit'.
			static Dot_00002D29$BurstDirectCall()
			{
				Bindings.Vec3Functions.Dot_00002D29$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D95 RID: 11669 RVA: 0x000E05E0 File Offset: 0x000DE7E0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Dot_00002D29$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Dot$BurstManaged(L);
			}

			// Token: 0x04003299 RID: 12953
			private static IntPtr Pointer;

			// Token: 0x0400329A RID: 12954
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000736 RID: 1846
		// (Invoke) Token: 0x06002D97 RID: 11671
		public unsafe delegate int Cross_00002D2A$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000737 RID: 1847
		internal static class Cross_00002D2A$BurstDirectCall
		{
			// Token: 0x06002D9A RID: 11674 RVA: 0x000E0611 File Offset: 0x000DE811
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Cross_00002D2A$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Cross_00002D2A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Cross_00002D2A$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Cross$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Cross_00002D2A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Cross_00002D2A$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D9B RID: 11675 RVA: 0x000E0640 File Offset: 0x000DE840
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Cross_00002D2A$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D9C RID: 11676 RVA: 0x000E0658 File Offset: 0x000DE858
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Cross_00002D2A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Cross(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D9D RID: 11677 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D9E RID: 11678 RVA: 0x000E0669 File Offset: 0x000DE869
			// Note: this type is marked as 'beforefieldinit'.
			static Cross_00002D2A$BurstDirectCall()
			{
				Bindings.Vec3Functions.Cross_00002D2A$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D9F RID: 11679 RVA: 0x000E0670 File Offset: 0x000DE870
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Cross_00002D2A$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Cross$BurstManaged(L);
			}

			// Token: 0x0400329B RID: 12955
			private static IntPtr Pointer;

			// Token: 0x0400329C RID: 12956
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000738 RID: 1848
		// (Invoke) Token: 0x06002DA1 RID: 11681
		public unsafe delegate int Project_00002D2B$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000739 RID: 1849
		internal static class Project_00002D2B$BurstDirectCall
		{
			// Token: 0x06002DA4 RID: 11684 RVA: 0x000E06A1 File Offset: 0x000DE8A1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Project_00002D2B$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Project_00002D2B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Project_00002D2B$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Project$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Project_00002D2B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Project_00002D2B$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DA5 RID: 11685 RVA: 0x000E06D0 File Offset: 0x000DE8D0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Project_00002D2B$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DA6 RID: 11686 RVA: 0x000E06E8 File Offset: 0x000DE8E8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Project_00002D2B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Project(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DA7 RID: 11687 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DA8 RID: 11688 RVA: 0x000E06F9 File Offset: 0x000DE8F9
			// Note: this type is marked as 'beforefieldinit'.
			static Project_00002D2B$BurstDirectCall()
			{
				Bindings.Vec3Functions.Project_00002D2B$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DA9 RID: 11689 RVA: 0x000E0700 File Offset: 0x000DE900
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Project_00002D2B$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Project$BurstManaged(L);
			}

			// Token: 0x0400329D RID: 12957
			private static IntPtr Pointer;

			// Token: 0x0400329E RID: 12958
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200073A RID: 1850
		// (Invoke) Token: 0x06002DAB RID: 11691
		public unsafe delegate int Length_00002D2C$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200073B RID: 1851
		internal static class Length_00002D2C$BurstDirectCall
		{
			// Token: 0x06002DAE RID: 11694 RVA: 0x000E0731 File Offset: 0x000DE931
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Length_00002D2C$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Length_00002D2C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Length_00002D2C$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Length$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Length_00002D2C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Length_00002D2C$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DAF RID: 11695 RVA: 0x000E0760 File Offset: 0x000DE960
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Length_00002D2C$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DB0 RID: 11696 RVA: 0x000E0778 File Offset: 0x000DE978
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Length_00002D2C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Length(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DB1 RID: 11697 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DB2 RID: 11698 RVA: 0x000E0789 File Offset: 0x000DE989
			// Note: this type is marked as 'beforefieldinit'.
			static Length_00002D2C$BurstDirectCall()
			{
				Bindings.Vec3Functions.Length_00002D2C$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DB3 RID: 11699 RVA: 0x000E0790 File Offset: 0x000DE990
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Length_00002D2C$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Length$BurstManaged(L);
			}

			// Token: 0x0400329F RID: 12959
			private static IntPtr Pointer;

			// Token: 0x040032A0 RID: 12960
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200073C RID: 1852
		// (Invoke) Token: 0x06002DB5 RID: 11701
		public unsafe delegate int Normalize_00002D2D$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200073D RID: 1853
		internal static class Normalize_00002D2D$BurstDirectCall
		{
			// Token: 0x06002DB8 RID: 11704 RVA: 0x000E07C1 File Offset: 0x000DE9C1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Normalize_00002D2D$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Normalize_00002D2D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Normalize_00002D2D$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Normalize$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Normalize_00002D2D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Normalize_00002D2D$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DB9 RID: 11705 RVA: 0x000E07F0 File Offset: 0x000DE9F0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Normalize_00002D2D$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DBA RID: 11706 RVA: 0x000E0808 File Offset: 0x000DEA08
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Normalize_00002D2D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Normalize(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DBB RID: 11707 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DBC RID: 11708 RVA: 0x000E0819 File Offset: 0x000DEA19
			// Note: this type is marked as 'beforefieldinit'.
			static Normalize_00002D2D$BurstDirectCall()
			{
				Bindings.Vec3Functions.Normalize_00002D2D$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DBD RID: 11709 RVA: 0x000E0820 File Offset: 0x000DEA20
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Normalize_00002D2D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Normalize$BurstManaged(L);
			}

			// Token: 0x040032A1 RID: 12961
			private static IntPtr Pointer;

			// Token: 0x040032A2 RID: 12962
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200073E RID: 1854
		// (Invoke) Token: 0x06002DBF RID: 11711
		public unsafe delegate int SafeNormal_00002D2E$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200073F RID: 1855
		internal static class SafeNormal_00002D2E$BurstDirectCall
		{
			// Token: 0x06002DC2 RID: 11714 RVA: 0x000E0851 File Offset: 0x000DEA51
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.SafeNormal_00002D2E$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.SafeNormal_00002D2E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.SafeNormal_00002D2E$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.SafeNormal$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.SafeNormal_00002D2E$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.SafeNormal_00002D2E$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DC3 RID: 11715 RVA: 0x000E0880 File Offset: 0x000DEA80
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.SafeNormal_00002D2E$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DC4 RID: 11716 RVA: 0x000E0898 File Offset: 0x000DEA98
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.SafeNormal_00002D2E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.SafeNormal(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DC5 RID: 11717 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DC6 RID: 11718 RVA: 0x000E08A9 File Offset: 0x000DEAA9
			// Note: this type is marked as 'beforefieldinit'.
			static SafeNormal_00002D2E$BurstDirectCall()
			{
				Bindings.Vec3Functions.SafeNormal_00002D2E$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DC7 RID: 11719 RVA: 0x000E08B0 File Offset: 0x000DEAB0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.SafeNormal_00002D2E$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.SafeNormal$BurstManaged(L);
			}

			// Token: 0x040032A3 RID: 12963
			private static IntPtr Pointer;

			// Token: 0x040032A4 RID: 12964
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000740 RID: 1856
		// (Invoke) Token: 0x06002DC9 RID: 11721
		public unsafe delegate int Distance_00002D2F$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000741 RID: 1857
		internal static class Distance_00002D2F$BurstDirectCall
		{
			// Token: 0x06002DCC RID: 11724 RVA: 0x000E08E1 File Offset: 0x000DEAE1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Distance_00002D2F$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Distance_00002D2F$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Distance_00002D2F$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Distance$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Distance_00002D2F$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Distance_00002D2F$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DCD RID: 11725 RVA: 0x000E0910 File Offset: 0x000DEB10
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Distance_00002D2F$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DCE RID: 11726 RVA: 0x000E0928 File Offset: 0x000DEB28
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Distance_00002D2F$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Distance(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DCF RID: 11727 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DD0 RID: 11728 RVA: 0x000E0939 File Offset: 0x000DEB39
			// Note: this type is marked as 'beforefieldinit'.
			static Distance_00002D2F$BurstDirectCall()
			{
				Bindings.Vec3Functions.Distance_00002D2F$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DD1 RID: 11729 RVA: 0x000E0940 File Offset: 0x000DEB40
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Distance_00002D2F$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Distance$BurstManaged(L);
			}

			// Token: 0x040032A5 RID: 12965
			private static IntPtr Pointer;

			// Token: 0x040032A6 RID: 12966
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000742 RID: 1858
		// (Invoke) Token: 0x06002DD3 RID: 11731
		public unsafe delegate int Lerp_00002D30$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000743 RID: 1859
		internal static class Lerp_00002D30$BurstDirectCall
		{
			// Token: 0x06002DD6 RID: 11734 RVA: 0x000E0971 File Offset: 0x000DEB71
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Lerp_00002D30$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Lerp_00002D30$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Lerp_00002D30$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Lerp$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Lerp_00002D30$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Lerp_00002D30$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DD7 RID: 11735 RVA: 0x000E09A0 File Offset: 0x000DEBA0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Lerp_00002D30$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DD8 RID: 11736 RVA: 0x000E09B8 File Offset: 0x000DEBB8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Lerp_00002D30$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Lerp(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DD9 RID: 11737 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DDA RID: 11738 RVA: 0x000E09C9 File Offset: 0x000DEBC9
			// Note: this type is marked as 'beforefieldinit'.
			static Lerp_00002D30$BurstDirectCall()
			{
				Bindings.Vec3Functions.Lerp_00002D30$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DDB RID: 11739 RVA: 0x000E09D0 File Offset: 0x000DEBD0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Lerp_00002D30$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Lerp$BurstManaged(L);
			}

			// Token: 0x040032A7 RID: 12967
			private static IntPtr Pointer;

			// Token: 0x040032A8 RID: 12968
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000744 RID: 1860
		// (Invoke) Token: 0x06002DDD RID: 11741
		public unsafe delegate int Rotate_00002D31$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000745 RID: 1861
		internal static class Rotate_00002D31$BurstDirectCall
		{
			// Token: 0x06002DE0 RID: 11744 RVA: 0x000E0A01 File Offset: 0x000DEC01
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Rotate_00002D31$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Rotate_00002D31$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Rotate_00002D31$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Rotate$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Rotate_00002D31$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Rotate_00002D31$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DE1 RID: 11745 RVA: 0x000E0A30 File Offset: 0x000DEC30
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Rotate_00002D31$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DE2 RID: 11746 RVA: 0x000E0A48 File Offset: 0x000DEC48
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Rotate_00002D31$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Rotate(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DE3 RID: 11747 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DE4 RID: 11748 RVA: 0x000E0A59 File Offset: 0x000DEC59
			// Note: this type is marked as 'beforefieldinit'.
			static Rotate_00002D31$BurstDirectCall()
			{
				Bindings.Vec3Functions.Rotate_00002D31$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DE5 RID: 11749 RVA: 0x000E0A60 File Offset: 0x000DEC60
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Rotate_00002D31$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Rotate$BurstManaged(L);
			}

			// Token: 0x040032A9 RID: 12969
			private static IntPtr Pointer;

			// Token: 0x040032AA RID: 12970
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000746 RID: 1862
		// (Invoke) Token: 0x06002DE7 RID: 11751
		public unsafe delegate int ZeroVector_00002D32$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000747 RID: 1863
		internal static class ZeroVector_00002D32$BurstDirectCall
		{
			// Token: 0x06002DEA RID: 11754 RVA: 0x000E0A91 File Offset: 0x000DEC91
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.ZeroVector_00002D32$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.ZeroVector_00002D32$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.ZeroVector_00002D32$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.ZeroVector$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.ZeroVector_00002D32$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.ZeroVector_00002D32$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DEB RID: 11755 RVA: 0x000E0AC0 File Offset: 0x000DECC0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.ZeroVector_00002D32$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DEC RID: 11756 RVA: 0x000E0AD8 File Offset: 0x000DECD8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.ZeroVector_00002D32$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.ZeroVector(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DED RID: 11757 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DEE RID: 11758 RVA: 0x000E0AE9 File Offset: 0x000DECE9
			// Note: this type is marked as 'beforefieldinit'.
			static ZeroVector_00002D32$BurstDirectCall()
			{
				Bindings.Vec3Functions.ZeroVector_00002D32$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DEF RID: 11759 RVA: 0x000E0AF0 File Offset: 0x000DECF0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.ZeroVector_00002D32$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.ZeroVector$BurstManaged(L);
			}

			// Token: 0x040032AB RID: 12971
			private static IntPtr Pointer;

			// Token: 0x040032AC RID: 12972
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000748 RID: 1864
		// (Invoke) Token: 0x06002DF1 RID: 11761
		public unsafe delegate int OneVector_00002D33$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000749 RID: 1865
		internal static class OneVector_00002D33$BurstDirectCall
		{
			// Token: 0x06002DF4 RID: 11764 RVA: 0x000E0B21 File Offset: 0x000DED21
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.OneVector_00002D33$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.OneVector_00002D33$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.OneVector_00002D33$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.OneVector$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.OneVector_00002D33$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.OneVector_00002D33$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DF5 RID: 11765 RVA: 0x000E0B50 File Offset: 0x000DED50
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.OneVector_00002D33$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DF6 RID: 11766 RVA: 0x000E0B68 File Offset: 0x000DED68
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.OneVector_00002D33$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.OneVector(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DF7 RID: 11767 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DF8 RID: 11768 RVA: 0x000E0B79 File Offset: 0x000DED79
			// Note: this type is marked as 'beforefieldinit'.
			static OneVector_00002D33$BurstDirectCall()
			{
				Bindings.Vec3Functions.OneVector_00002D33$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DF9 RID: 11769 RVA: 0x000E0B80 File Offset: 0x000DED80
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.OneVector_00002D33$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.OneVector$BurstManaged(L);
			}

			// Token: 0x040032AD RID: 12973
			private static IntPtr Pointer;

			// Token: 0x040032AE RID: 12974
			private static IntPtr DeferredCompilation;
		}
	}

	// Token: 0x0200074A RID: 1866
	[BurstCompile]
	public static class QuatFunctions
	{
		// Token: 0x06002DFA RID: 11770 RVA: 0x000E0BB1 File Offset: 0x000DEDB1
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int New(lua_State* L)
		{
			return Bindings.QuatFunctions.New_00002D34$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x000E0BB9 File Offset: 0x000DEDB9
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Mul(lua_State* L)
		{
			return Bindings.QuatFunctions.Mul_00002D35$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x000E0BC1 File Offset: 0x000DEDC1
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Eq(lua_State* L)
		{
			return Bindings.QuatFunctions.Eq_00002D36$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x000E0BCC File Offset: 0x000DEDCC
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int ToString(lua_State* L)
		{
			Quaternion quaternion = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			Luau.lua_pushstring(L, quaternion.ToString());
			return 1;
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x000E0C05 File Offset: 0x000DEE05
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int FromEuler(lua_State* L)
		{
			return Bindings.QuatFunctions.FromEuler_00002D38$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x000E0C0D File Offset: 0x000DEE0D
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int FromDirection(lua_State* L)
		{
			return Bindings.QuatFunctions.FromDirection_00002D39$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x000E0C15 File Offset: 0x000DEE15
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int GetUpVector(lua_State* L)
		{
			return Bindings.QuatFunctions.GetUpVector_00002D3A$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x000E0C1D File Offset: 0x000DEE1D
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Euler(lua_State* L)
		{
			return Bindings.QuatFunctions.Euler_00002D3B$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x000E0C28 File Offset: 0x000DEE28
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

		// Token: 0x06002E03 RID: 11779 RVA: 0x000E0CA4 File Offset: 0x000DEEA4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Mul$BurstManaged(lua_State* L)
		{
			Quaternion lhs = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			Quaternion rhs = *Luau.lua_class_get<Quaternion>(L, 2, "Quat");
			*Luau.lua_class_push<Quaternion>(L, "Quat") = lhs * rhs;
			return 1;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000E0CFC File Offset: 0x000DEEFC
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

		// Token: 0x06002E05 RID: 11781 RVA: 0x000E0D4C File Offset: 0x000DEF4C
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

		// Token: 0x06002E06 RID: 11782 RVA: 0x000E0DB0 File Offset: 0x000DEFB0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int FromDirection$BurstManaged(lua_State* L)
		{
			Vector3 lookRotation = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Luau.lua_class_push<Quaternion>(L, "Quat")->SetLookRotation(lookRotation);
			return 1;
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000E0DEC File Offset: 0x000DEFEC
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int GetUpVector$BurstManaged(lua_State* L)
		{
			Quaternion rotation = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = rotation * Vector3.up;
			return 1;
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000E0E34 File Offset: 0x000DF034
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Euler$BurstManaged(lua_State* L)
		{
			Quaternion quaternion = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = quaternion.eulerAngles;
			return 1;
		}

		// Token: 0x0200074B RID: 1867
		// (Invoke) Token: 0x06002E0A RID: 11786
		public unsafe delegate int New_00002D34$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200074C RID: 1868
		internal static class New_00002D34$BurstDirectCall
		{
			// Token: 0x06002E0D RID: 11789 RVA: 0x000E0E75 File Offset: 0x000DF075
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.New_00002D34$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.New_00002D34$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.New_00002D34$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.New$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.New_00002D34$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.New_00002D34$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E0E RID: 11790 RVA: 0x000E0EA4 File Offset: 0x000DF0A4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.New_00002D34$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E0F RID: 11791 RVA: 0x000E0EBC File Offset: 0x000DF0BC
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.New_00002D34$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.New(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E10 RID: 11792 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E11 RID: 11793 RVA: 0x000E0ECD File Offset: 0x000DF0CD
			// Note: this type is marked as 'beforefieldinit'.
			static New_00002D34$BurstDirectCall()
			{
				Bindings.QuatFunctions.New_00002D34$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E12 RID: 11794 RVA: 0x000E0ED4 File Offset: 0x000DF0D4
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.New_00002D34$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.New$BurstManaged(L);
			}

			// Token: 0x040032AF RID: 12975
			private static IntPtr Pointer;

			// Token: 0x040032B0 RID: 12976
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200074D RID: 1869
		// (Invoke) Token: 0x06002E14 RID: 11796
		public unsafe delegate int Mul_00002D35$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200074E RID: 1870
		internal static class Mul_00002D35$BurstDirectCall
		{
			// Token: 0x06002E17 RID: 11799 RVA: 0x000E0F05 File Offset: 0x000DF105
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.Mul_00002D35$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.Mul_00002D35$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.Mul_00002D35$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.Mul$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.Mul_00002D35$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.Mul_00002D35$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E18 RID: 11800 RVA: 0x000E0F34 File Offset: 0x000DF134
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.Mul_00002D35$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E19 RID: 11801 RVA: 0x000E0F4C File Offset: 0x000DF14C
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.Mul_00002D35$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.Mul(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E1A RID: 11802 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E1B RID: 11803 RVA: 0x000E0F5D File Offset: 0x000DF15D
			// Note: this type is marked as 'beforefieldinit'.
			static Mul_00002D35$BurstDirectCall()
			{
				Bindings.QuatFunctions.Mul_00002D35$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E1C RID: 11804 RVA: 0x000E0F64 File Offset: 0x000DF164
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.Mul_00002D35$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.Mul$BurstManaged(L);
			}

			// Token: 0x040032B1 RID: 12977
			private static IntPtr Pointer;

			// Token: 0x040032B2 RID: 12978
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200074F RID: 1871
		// (Invoke) Token: 0x06002E1E RID: 11806
		public unsafe delegate int Eq_00002D36$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000750 RID: 1872
		internal static class Eq_00002D36$BurstDirectCall
		{
			// Token: 0x06002E21 RID: 11809 RVA: 0x000E0F95 File Offset: 0x000DF195
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.Eq_00002D36$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.Eq_00002D36$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.Eq_00002D36$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.Eq$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.Eq_00002D36$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.Eq_00002D36$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E22 RID: 11810 RVA: 0x000E0FC4 File Offset: 0x000DF1C4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.Eq_00002D36$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E23 RID: 11811 RVA: 0x000E0FDC File Offset: 0x000DF1DC
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.Eq_00002D36$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.Eq(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E24 RID: 11812 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E25 RID: 11813 RVA: 0x000E0FED File Offset: 0x000DF1ED
			// Note: this type is marked as 'beforefieldinit'.
			static Eq_00002D36$BurstDirectCall()
			{
				Bindings.QuatFunctions.Eq_00002D36$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E26 RID: 11814 RVA: 0x000E0FF4 File Offset: 0x000DF1F4
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.Eq_00002D36$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.Eq$BurstManaged(L);
			}

			// Token: 0x040032B3 RID: 12979
			private static IntPtr Pointer;

			// Token: 0x040032B4 RID: 12980
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000751 RID: 1873
		// (Invoke) Token: 0x06002E28 RID: 11816
		public unsafe delegate int FromEuler_00002D38$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000752 RID: 1874
		internal static class FromEuler_00002D38$BurstDirectCall
		{
			// Token: 0x06002E2B RID: 11819 RVA: 0x000E1025 File Offset: 0x000DF225
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.FromEuler_00002D38$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.FromEuler_00002D38$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.FromEuler_00002D38$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.FromEuler$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.FromEuler_00002D38$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.FromEuler_00002D38$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E2C RID: 11820 RVA: 0x000E1054 File Offset: 0x000DF254
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.FromEuler_00002D38$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E2D RID: 11821 RVA: 0x000E106C File Offset: 0x000DF26C
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.FromEuler_00002D38$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.FromEuler(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E2E RID: 11822 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E2F RID: 11823 RVA: 0x000E107D File Offset: 0x000DF27D
			// Note: this type is marked as 'beforefieldinit'.
			static FromEuler_00002D38$BurstDirectCall()
			{
				Bindings.QuatFunctions.FromEuler_00002D38$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E30 RID: 11824 RVA: 0x000E1084 File Offset: 0x000DF284
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.FromEuler_00002D38$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.FromEuler$BurstManaged(L);
			}

			// Token: 0x040032B5 RID: 12981
			private static IntPtr Pointer;

			// Token: 0x040032B6 RID: 12982
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000753 RID: 1875
		// (Invoke) Token: 0x06002E32 RID: 11826
		public unsafe delegate int FromDirection_00002D39$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000754 RID: 1876
		internal static class FromDirection_00002D39$BurstDirectCall
		{
			// Token: 0x06002E35 RID: 11829 RVA: 0x000E10B5 File Offset: 0x000DF2B5
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.FromDirection_00002D39$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.FromDirection_00002D39$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.FromDirection_00002D39$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.FromDirection$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.FromDirection_00002D39$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.FromDirection_00002D39$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E36 RID: 11830 RVA: 0x000E10E4 File Offset: 0x000DF2E4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.FromDirection_00002D39$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E37 RID: 11831 RVA: 0x000E10FC File Offset: 0x000DF2FC
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.FromDirection_00002D39$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.FromDirection(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E38 RID: 11832 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E39 RID: 11833 RVA: 0x000E110D File Offset: 0x000DF30D
			// Note: this type is marked as 'beforefieldinit'.
			static FromDirection_00002D39$BurstDirectCall()
			{
				Bindings.QuatFunctions.FromDirection_00002D39$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E3A RID: 11834 RVA: 0x000E1114 File Offset: 0x000DF314
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.FromDirection_00002D39$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.FromDirection$BurstManaged(L);
			}

			// Token: 0x040032B7 RID: 12983
			private static IntPtr Pointer;

			// Token: 0x040032B8 RID: 12984
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000755 RID: 1877
		// (Invoke) Token: 0x06002E3C RID: 11836
		public unsafe delegate int GetUpVector_00002D3A$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000756 RID: 1878
		internal static class GetUpVector_00002D3A$BurstDirectCall
		{
			// Token: 0x06002E3F RID: 11839 RVA: 0x000E1145 File Offset: 0x000DF345
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.GetUpVector_00002D3A$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.GetUpVector_00002D3A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.GetUpVector_00002D3A$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.GetUpVector$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.GetUpVector_00002D3A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.GetUpVector_00002D3A$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E40 RID: 11840 RVA: 0x000E1174 File Offset: 0x000DF374
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.GetUpVector_00002D3A$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E41 RID: 11841 RVA: 0x000E118C File Offset: 0x000DF38C
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.GetUpVector_00002D3A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.GetUpVector(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E42 RID: 11842 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E43 RID: 11843 RVA: 0x000E119D File Offset: 0x000DF39D
			// Note: this type is marked as 'beforefieldinit'.
			static GetUpVector_00002D3A$BurstDirectCall()
			{
				Bindings.QuatFunctions.GetUpVector_00002D3A$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E44 RID: 11844 RVA: 0x000E11A4 File Offset: 0x000DF3A4
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.GetUpVector_00002D3A$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.GetUpVector$BurstManaged(L);
			}

			// Token: 0x040032B9 RID: 12985
			private static IntPtr Pointer;

			// Token: 0x040032BA RID: 12986
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000757 RID: 1879
		// (Invoke) Token: 0x06002E46 RID: 11846
		public unsafe delegate int Euler_00002D3B$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000758 RID: 1880
		internal static class Euler_00002D3B$BurstDirectCall
		{
			// Token: 0x06002E49 RID: 11849 RVA: 0x000E11D5 File Offset: 0x000DF3D5
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.Euler_00002D3B$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.Euler_00002D3B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.Euler_00002D3B$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.Euler$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.Euler_00002D3B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.Euler_00002D3B$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E4A RID: 11850 RVA: 0x000E1204 File Offset: 0x000DF404
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.Euler_00002D3B$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E4B RID: 11851 RVA: 0x000E121C File Offset: 0x000DF41C
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.Euler_00002D3B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.Euler(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E4C RID: 11852 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E4D RID: 11853 RVA: 0x000E122D File Offset: 0x000DF42D
			// Note: this type is marked as 'beforefieldinit'.
			static Euler_00002D3B$BurstDirectCall()
			{
				Bindings.QuatFunctions.Euler_00002D3B$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E4E RID: 11854 RVA: 0x000E1234 File Offset: 0x000DF434
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.Euler_00002D3B$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.Euler$BurstManaged(L);
			}

			// Token: 0x040032BB RID: 12987
			private static IntPtr Pointer;

			// Token: 0x040032BC RID: 12988
			private static IntPtr DeferredCompilation;
		}
	}

	// Token: 0x02000759 RID: 1881
	public struct GorillaLocomotionSettings
	{
		// Token: 0x040032BD RID: 12989
		public float velocityLimit;

		// Token: 0x040032BE RID: 12990
		public float slideVelocityLimit;

		// Token: 0x040032BF RID: 12991
		public float maxJumpSpeed;

		// Token: 0x040032C0 RID: 12992
		public float jumpMultiplier;
	}
}
