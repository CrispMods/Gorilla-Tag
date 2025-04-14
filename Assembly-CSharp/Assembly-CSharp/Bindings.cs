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

// Token: 0x0200071E RID: 1822
[BurstCompile]
public static class Bindings
{
	// Token: 0x06002D15 RID: 11541 RVA: 0x000DF2B0 File Offset: 0x000DD4B0
	public unsafe static void GameObjectBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Bindings.LuauGameObject>("GameObject").AddField("position", "Position").AddField("rotation", "Rotation").AddField("scale", "Scale").AddStaticFunction("findGameObject", new lua_CFunction(Bindings.GameObjectFunctions.FindGameObject)).AddFunction("setCollision", new lua_CFunction(Bindings.GameObjectFunctions.SetCollision)).AddFunction("setVisibility", new lua_CFunction(Bindings.GameObjectFunctions.SetVisibility)).Build(L, true));
	}

	// Token: 0x06002D16 RID: 11542 RVA: 0x000DF348 File Offset: 0x000DD548
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

	// Token: 0x06002D17 RID: 11543 RVA: 0x000DF3FC File Offset: 0x000DD5FC
	public unsafe static void Vec3Builder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Vector3>("Vec3").AddField("x", null).AddField("y", null).AddField("z", null).AddStaticFunction("new", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.New))).AddFunction("__add", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Add))).AddFunction("__sub", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Sub))).AddFunction("__mul", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Mul))).AddFunction("__div", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Div))).AddFunction("__unm", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Unm))).AddFunction("__eq", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Eq))).AddFunction("__tostring", new lua_CFunction(Bindings.Vec3Functions.ToSring)).AddFunction("toString", new lua_CFunction(Bindings.Vec3Functions.ToSring)).AddFunction("dot", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Dot))).AddFunction("cross", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Cross))).AddFunction("projectOnTo", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Project))).AddFunction("length", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Length))).AddFunction("normalize", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Normalize))).AddFunction("getSafeNormal", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.SafeNormal))).AddStaticFunction("rotate", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Rotate))).AddFunction("rotate", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Rotate))).AddStaticFunction("distance", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Distance))).AddFunction("distance", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Distance))).AddStaticFunction("lerp", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Lerp))).AddFunction("lerp", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Lerp))).AddProperty("zeroVector", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.ZeroVector))).AddProperty("oneVector", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.OneVector))).Build(L, true));
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x000DF6AC File Offset: 0x000DD8AC
	public unsafe static void QuatBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Quaternion>("Quat").AddField("x", null).AddField("y", null).AddField("z", null).AddField("w", null).AddStaticFunction("new", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.New))).AddFunction("__mul", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.Mul))).AddFunction("__eq", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.Eq))).AddFunction("__tostring", new lua_CFunction(Bindings.QuatFunctions.ToString)).AddFunction("toString", new lua_CFunction(Bindings.QuatFunctions.ToString)).AddStaticFunction("fromEuler", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.FromEuler))).AddStaticFunction("fromDirection", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.FromDirection))).AddFunction("getUpVector", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.GetUpVector))).AddFunction("euler", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.Euler))).Build(L, true));
	}

	// Token: 0x06002D19 RID: 11545 RVA: 0x000DF7EC File Offset: 0x000DD9EC
	public unsafe static void PlayerBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Bindings.LuauPlayer>("Player").AddField("playerID", "PlayerID").AddField("playerName", "PlayerName").AddField("playerMaterial", "PlayerMaterial").AddField("isMasterClient", "IsMasterClient").AddField("bodyPosition", "BodyPosition").AddField("leftHandPosition", "LeftHandPosition").AddField("rightHandPosition", "RightHandPosition").AddField("headRotation", "HeadRotation").AddField("leftHandRotation", "LeftHandRotation").AddField("rightHandRotation", "RightHandRotation").AddStaticFunction("getPlayerByID", new lua_CFunction(Bindings.PlayerFunctions.GetPlayerByID)).Build(L, true));
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x000DF8C4 File Offset: 0x000DDAC4
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int LuaStartVibration(lua_State* L)
	{
		bool forLeftController = Luau.lua_toboolean(L, 1) == 1;
		float amplitude = (float)Luau.luaL_checknumber(L, 2);
		float duration = (float)Luau.luaL_checknumber(L, 3);
		GorillaTagger.Instance.StartVibration(forLeftController, amplitude, duration);
		return 0;
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x000DF8FC File Offset: 0x000DDAFC
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

	// Token: 0x0400327C RID: 12924
	public static Dictionary<GameObject, IntPtr> LuauGameObjectList = new Dictionary<GameObject, IntPtr>();

	// Token: 0x0400327D RID: 12925
	public static Dictionary<int, IntPtr> LuauPlayerList = new Dictionary<int, IntPtr>();

	// Token: 0x0400327E RID: 12926
	public static Dictionary<int, VRRig> LuauVRRigList = new Dictionary<int, VRRig>();

	// Token: 0x0400327F RID: 12927
	public unsafe static Bindings.GorillaLocomotionSettings* LocomotionSettings;

	// Token: 0x0200071F RID: 1823
	public static class LuaEmit
	{
		// Token: 0x06002D1D RID: 11549 RVA: 0x000DF97C File Offset: 0x000DDB7C
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

		// Token: 0x04003280 RID: 12928
		private static float callTime = 0f;

		// Token: 0x04003281 RID: 12929
		private static float callCount = 20f;
	}

	// Token: 0x02000720 RID: 1824
	[BurstCompile]
	public struct LuauGameObject
	{
		// Token: 0x04003282 RID: 12930
		public Vector3 Position;

		// Token: 0x04003283 RID: 12931
		public Quaternion Rotation;

		// Token: 0x04003284 RID: 12932
		public Vector3 Scale;
	}

	// Token: 0x02000721 RID: 1825
	[BurstCompile]
	public static class GameObjectFunctions
	{
		// Token: 0x06002D1F RID: 11551 RVA: 0x000DFC20 File Offset: 0x000DDE20
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

		// Token: 0x06002D20 RID: 11552 RVA: 0x000DFC84 File Offset: 0x000DDE84
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

		// Token: 0x06002D21 RID: 11553 RVA: 0x000DFD48 File Offset: 0x000DDF48
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SetCollision(lua_State* L)
		{
			Bindings.LuauGameObject* data = Luau.lua_class_get<Bindings.LuauGameObject>(L, 1, "GameObject");
			Bindings.LuauGameObjectList.FirstOrDefault((KeyValuePair<GameObject, IntPtr> g) => g.Value == (IntPtr)((void*)data)).Key.GetComponent<Collider>().enabled = (Luau.lua_toboolean(L, 2) == 1);
			return 0;
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x000DFDA8 File Offset: 0x000DDFA8
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SetVisibility(lua_State* L)
		{
			Bindings.LuauGameObject* data = Luau.lua_class_get<Bindings.LuauGameObject>(L, 1, "GameObject");
			Bindings.LuauGameObjectList.FirstOrDefault((KeyValuePair<GameObject, IntPtr> g) => g.Value == (IntPtr)((void*)data)).Key.GetComponent<MeshRenderer>().enabled = (Luau.lua_toboolean(L, 2) == 1);
			return 0;
		}
	}

	// Token: 0x02000724 RID: 1828
	[BurstCompile]
	public struct LuauPlayer
	{
		// Token: 0x04003287 RID: 12935
		public int PlayerID;

		// Token: 0x04003288 RID: 12936
		public FixedString32Bytes PlayerName;

		// Token: 0x04003289 RID: 12937
		public int PlayerMaterial;

		// Token: 0x0400328A RID: 12938
		public bool IsMasterClient;

		// Token: 0x0400328B RID: 12939
		public Vector3 BodyPosition;

		// Token: 0x0400328C RID: 12940
		public Vector3 LeftHandPosition;

		// Token: 0x0400328D RID: 12941
		public Vector3 RightHandPosition;

		// Token: 0x0400328E RID: 12942
		public Quaternion HeadRotation;

		// Token: 0x0400328F RID: 12943
		public Quaternion LeftHandRotation;

		// Token: 0x04003290 RID: 12944
		public Quaternion RightHandRotation;
	}

	// Token: 0x02000725 RID: 1829
	[BurstCompile]
	public static class PlayerFunctions
	{
		// Token: 0x06002D27 RID: 11559 RVA: 0x000DFE38 File Offset: 0x000DE038
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

		// Token: 0x06002D28 RID: 11560 RVA: 0x000DFF9C File Offset: 0x000DE19C
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

	// Token: 0x02000726 RID: 1830
	[BurstCompile]
	public static class Vec3Functions
	{
		// Token: 0x06002D29 RID: 11561 RVA: 0x000E000F File Offset: 0x000DE20F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int New(lua_State* L)
		{
			return Bindings.Vec3Functions.New_00002D29$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x000E0017 File Offset: 0x000DE217
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Add(lua_State* L)
		{
			return Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x000E001F File Offset: 0x000DE21F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Sub(lua_State* L)
		{
			return Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x000E0027 File Offset: 0x000DE227
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Mul(lua_State* L)
		{
			return Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x000E002F File Offset: 0x000DE22F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Div(lua_State* L)
		{
			return Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x000E0037 File Offset: 0x000DE237
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Unm(lua_State* L)
		{
			return Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x000E003F File Offset: 0x000DE23F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Eq(lua_State* L)
		{
			return Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000E0048 File Offset: 0x000DE248
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int ToSring(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Luau.lua_pushstring(L, vector.ToString());
			return 1;
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x000E0081 File Offset: 0x000DE281
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Dot(lua_State* L)
		{
			return Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x000E0089 File Offset: 0x000DE289
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Cross(lua_State* L)
		{
			return Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x000E0091 File Offset: 0x000DE291
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Project(lua_State* L)
		{
			return Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000E0099 File Offset: 0x000DE299
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Length(lua_State* L)
		{
			return Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000E00A1 File Offset: 0x000DE2A1
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Normalize(lua_State* L)
		{
			return Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000E00A9 File Offset: 0x000DE2A9
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SafeNormal(lua_State* L)
		{
			return Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x000E00B1 File Offset: 0x000DE2B1
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Distance(lua_State* L)
		{
			return Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000E00B9 File Offset: 0x000DE2B9
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Lerp(lua_State* L)
		{
			return Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x000E00C1 File Offset: 0x000DE2C1
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Rotate(lua_State* L)
		{
			return Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000E00C9 File Offset: 0x000DE2C9
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int ZeroVector(lua_State* L)
		{
			return Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000E00D1 File Offset: 0x000DE2D1
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int OneVector(lua_State* L)
		{
			return Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000E00DC File Offset: 0x000DE2DC
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

		// Token: 0x06002D3D RID: 11581 RVA: 0x000E0140 File Offset: 0x000DE340
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Add$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a + b;
			return 1;
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000E0198 File Offset: 0x000DE398
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Sub$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a - b;
			return 1;
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x000E01F0 File Offset: 0x000DE3F0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Mul$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			float d = (float)Luau.luaL_checknumber(L, 2);
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a * d;
			return 1;
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x000E023C File Offset: 0x000DE43C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Div$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			float d = (float)Luau.luaL_checknumber(L, 2);
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a / d;
			return 1;
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x000E0288 File Offset: 0x000DE488
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Unm$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = -a;
			return 1;
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000E02C8 File Offset: 0x000DE4C8
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

		// Token: 0x06002D43 RID: 11587 RVA: 0x000E0318 File Offset: 0x000DE518
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

		// Token: 0x06002D44 RID: 11588 RVA: 0x000E0364 File Offset: 0x000DE564
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Cross$BurstManaged(lua_State* L)
		{
			Vector3 lhs = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 rhs = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = Vector3.Cross(lhs, rhs);
			return 1;
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000E03BC File Offset: 0x000DE5BC
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Project$BurstManaged(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 onNormal = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = Vector3.Project(vector, onNormal);
			return 1;
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000E0414 File Offset: 0x000DE614
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Length$BurstManaged(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Luau.lua_pushnumber(L, (double)Vector3.Magnitude(vector));
			return 1;
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000E0446 File Offset: 0x000DE646
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Normalize$BurstManaged(lua_State* L)
		{
			Luau.lua_class_get<Vector3>(L, 1, "Vec3")->Normalize();
			return 0;
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000E0460 File Offset: 0x000DE660
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int SafeNormal$BurstManaged(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = vector.normalized;
			return 1;
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000E04A4 File Offset: 0x000DE6A4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Distance$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			Luau.lua_pushnumber(L, (double)Vector3.Distance(a, b));
			return 1;
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x000E04F0 File Offset: 0x000DE6F0
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

		// Token: 0x06002D4B RID: 11595 RVA: 0x000E0554 File Offset: 0x000DE754
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Rotate$BurstManaged(lua_State* L)
		{
			Vector3 point = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Quaternion rotation = *Luau.lua_class_get<Quaternion>(L, 2, "Quat");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = rotation * point;
			return 1;
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x000E05AC File Offset: 0x000DE7AC
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

		// Token: 0x06002D4D RID: 11597 RVA: 0x000E05DF File Offset: 0x000DE7DF
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

		// Token: 0x02000727 RID: 1831
		// (Invoke) Token: 0x06002D4F RID: 11599
		public unsafe delegate int New_00002D29$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000728 RID: 1832
		internal static class New_00002D29$BurstDirectCall
		{
			// Token: 0x06002D52 RID: 11602 RVA: 0x000E0612 File Offset: 0x000DE812
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.New_00002D29$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.New_00002D29$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.New_00002D29$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.New$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.New_00002D29$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.New_00002D29$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D53 RID: 11603 RVA: 0x000E0640 File Offset: 0x000DE840
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.New_00002D29$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D54 RID: 11604 RVA: 0x000E0658 File Offset: 0x000DE858
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.New_00002D29$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.New(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D55 RID: 11605 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D56 RID: 11606 RVA: 0x000E0669 File Offset: 0x000DE869
			// Note: this type is marked as 'beforefieldinit'.
			static New_00002D29$BurstDirectCall()
			{
				Bindings.Vec3Functions.New_00002D29$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D57 RID: 11607 RVA: 0x000E0670 File Offset: 0x000DE870
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.New_00002D29$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.New$BurstManaged(L);
			}

			// Token: 0x04003291 RID: 12945
			private static IntPtr Pointer;

			// Token: 0x04003292 RID: 12946
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000729 RID: 1833
		// (Invoke) Token: 0x06002D59 RID: 11609
		public unsafe delegate int Add_00002D2A$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200072A RID: 1834
		internal static class Add_00002D2A$BurstDirectCall
		{
			// Token: 0x06002D5C RID: 11612 RVA: 0x000E06A1 File Offset: 0x000DE8A1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Add$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Add_00002D2A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D5D RID: 11613 RVA: 0x000E06D0 File Offset: 0x000DE8D0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D5E RID: 11614 RVA: 0x000E06E8 File Offset: 0x000DE8E8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Add(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D5F RID: 11615 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D60 RID: 11616 RVA: 0x000E06F9 File Offset: 0x000DE8F9
			// Note: this type is marked as 'beforefieldinit'.
			static Add_00002D2A$BurstDirectCall()
			{
				Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D61 RID: 11617 RVA: 0x000E0700 File Offset: 0x000DE900
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Add$BurstManaged(L);
			}

			// Token: 0x04003293 RID: 12947
			private static IntPtr Pointer;

			// Token: 0x04003294 RID: 12948
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200072B RID: 1835
		// (Invoke) Token: 0x06002D63 RID: 11619
		public unsafe delegate int Sub_00002D2B$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200072C RID: 1836
		internal static class Sub_00002D2B$BurstDirectCall
		{
			// Token: 0x06002D66 RID: 11622 RVA: 0x000E0731 File Offset: 0x000DE931
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Sub$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Sub_00002D2B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D67 RID: 11623 RVA: 0x000E0760 File Offset: 0x000DE960
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D68 RID: 11624 RVA: 0x000E0778 File Offset: 0x000DE978
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Sub(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D69 RID: 11625 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D6A RID: 11626 RVA: 0x000E0789 File Offset: 0x000DE989
			// Note: this type is marked as 'beforefieldinit'.
			static Sub_00002D2B$BurstDirectCall()
			{
				Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D6B RID: 11627 RVA: 0x000E0790 File Offset: 0x000DE990
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Sub$BurstManaged(L);
			}

			// Token: 0x04003295 RID: 12949
			private static IntPtr Pointer;

			// Token: 0x04003296 RID: 12950
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200072D RID: 1837
		// (Invoke) Token: 0x06002D6D RID: 11629
		public unsafe delegate int Mul_00002D2C$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200072E RID: 1838
		internal static class Mul_00002D2C$BurstDirectCall
		{
			// Token: 0x06002D70 RID: 11632 RVA: 0x000E07C1 File Offset: 0x000DE9C1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Mul$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Mul_00002D2C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D71 RID: 11633 RVA: 0x000E07F0 File Offset: 0x000DE9F0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D72 RID: 11634 RVA: 0x000E0808 File Offset: 0x000DEA08
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Mul(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D73 RID: 11635 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D74 RID: 11636 RVA: 0x000E0819 File Offset: 0x000DEA19
			// Note: this type is marked as 'beforefieldinit'.
			static Mul_00002D2C$BurstDirectCall()
			{
				Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D75 RID: 11637 RVA: 0x000E0820 File Offset: 0x000DEA20
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Mul$BurstManaged(L);
			}

			// Token: 0x04003297 RID: 12951
			private static IntPtr Pointer;

			// Token: 0x04003298 RID: 12952
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200072F RID: 1839
		// (Invoke) Token: 0x06002D77 RID: 11639
		public unsafe delegate int Div_00002D2D$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000730 RID: 1840
		internal static class Div_00002D2D$BurstDirectCall
		{
			// Token: 0x06002D7A RID: 11642 RVA: 0x000E0851 File Offset: 0x000DEA51
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Div$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Div_00002D2D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D7B RID: 11643 RVA: 0x000E0880 File Offset: 0x000DEA80
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D7C RID: 11644 RVA: 0x000E0898 File Offset: 0x000DEA98
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Div(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D7D RID: 11645 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D7E RID: 11646 RVA: 0x000E08A9 File Offset: 0x000DEAA9
			// Note: this type is marked as 'beforefieldinit'.
			static Div_00002D2D$BurstDirectCall()
			{
				Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D7F RID: 11647 RVA: 0x000E08B0 File Offset: 0x000DEAB0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Div$BurstManaged(L);
			}

			// Token: 0x04003299 RID: 12953
			private static IntPtr Pointer;

			// Token: 0x0400329A RID: 12954
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000731 RID: 1841
		// (Invoke) Token: 0x06002D81 RID: 11649
		public unsafe delegate int Unm_00002D2E$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000732 RID: 1842
		internal static class Unm_00002D2E$BurstDirectCall
		{
			// Token: 0x06002D84 RID: 11652 RVA: 0x000E08E1 File Offset: 0x000DEAE1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Unm$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Unm_00002D2E$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D85 RID: 11653 RVA: 0x000E0910 File Offset: 0x000DEB10
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D86 RID: 11654 RVA: 0x000E0928 File Offset: 0x000DEB28
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Unm(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D87 RID: 11655 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D88 RID: 11656 RVA: 0x000E0939 File Offset: 0x000DEB39
			// Note: this type is marked as 'beforefieldinit'.
			static Unm_00002D2E$BurstDirectCall()
			{
				Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D89 RID: 11657 RVA: 0x000E0940 File Offset: 0x000DEB40
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Unm$BurstManaged(L);
			}

			// Token: 0x0400329B RID: 12955
			private static IntPtr Pointer;

			// Token: 0x0400329C RID: 12956
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000733 RID: 1843
		// (Invoke) Token: 0x06002D8B RID: 11659
		public unsafe delegate int Eq_00002D2F$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000734 RID: 1844
		internal static class Eq_00002D2F$BurstDirectCall
		{
			// Token: 0x06002D8E RID: 11662 RVA: 0x000E0971 File Offset: 0x000DEB71
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Eq$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Eq_00002D2F$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D8F RID: 11663 RVA: 0x000E09A0 File Offset: 0x000DEBA0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D90 RID: 11664 RVA: 0x000E09B8 File Offset: 0x000DEBB8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Eq(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D91 RID: 11665 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D92 RID: 11666 RVA: 0x000E09C9 File Offset: 0x000DEBC9
			// Note: this type is marked as 'beforefieldinit'.
			static Eq_00002D2F$BurstDirectCall()
			{
				Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D93 RID: 11667 RVA: 0x000E09D0 File Offset: 0x000DEBD0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Eq$BurstManaged(L);
			}

			// Token: 0x0400329D RID: 12957
			private static IntPtr Pointer;

			// Token: 0x0400329E RID: 12958
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000735 RID: 1845
		// (Invoke) Token: 0x06002D95 RID: 11669
		public unsafe delegate int Dot_00002D31$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000736 RID: 1846
		internal static class Dot_00002D31$BurstDirectCall
		{
			// Token: 0x06002D98 RID: 11672 RVA: 0x000E0A01 File Offset: 0x000DEC01
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Dot$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Dot_00002D31$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D99 RID: 11673 RVA: 0x000E0A30 File Offset: 0x000DEC30
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D9A RID: 11674 RVA: 0x000E0A48 File Offset: 0x000DEC48
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Dot(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D9B RID: 11675 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002D9C RID: 11676 RVA: 0x000E0A59 File Offset: 0x000DEC59
			// Note: this type is marked as 'beforefieldinit'.
			static Dot_00002D31$BurstDirectCall()
			{
				Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D9D RID: 11677 RVA: 0x000E0A60 File Offset: 0x000DEC60
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Dot$BurstManaged(L);
			}

			// Token: 0x0400329F RID: 12959
			private static IntPtr Pointer;

			// Token: 0x040032A0 RID: 12960
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000737 RID: 1847
		// (Invoke) Token: 0x06002D9F RID: 11679
		public unsafe delegate int Cross_00002D32$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000738 RID: 1848
		internal static class Cross_00002D32$BurstDirectCall
		{
			// Token: 0x06002DA2 RID: 11682 RVA: 0x000E0A91 File Offset: 0x000DEC91
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Cross$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Cross_00002D32$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DA3 RID: 11683 RVA: 0x000E0AC0 File Offset: 0x000DECC0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DA4 RID: 11684 RVA: 0x000E0AD8 File Offset: 0x000DECD8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Cross(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DA5 RID: 11685 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DA6 RID: 11686 RVA: 0x000E0AE9 File Offset: 0x000DECE9
			// Note: this type is marked as 'beforefieldinit'.
			static Cross_00002D32$BurstDirectCall()
			{
				Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DA7 RID: 11687 RVA: 0x000E0AF0 File Offset: 0x000DECF0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Cross$BurstManaged(L);
			}

			// Token: 0x040032A1 RID: 12961
			private static IntPtr Pointer;

			// Token: 0x040032A2 RID: 12962
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000739 RID: 1849
		// (Invoke) Token: 0x06002DA9 RID: 11689
		public unsafe delegate int Project_00002D33$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200073A RID: 1850
		internal static class Project_00002D33$BurstDirectCall
		{
			// Token: 0x06002DAC RID: 11692 RVA: 0x000E0B21 File Offset: 0x000DED21
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Project$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Project_00002D33$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DAD RID: 11693 RVA: 0x000E0B50 File Offset: 0x000DED50
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DAE RID: 11694 RVA: 0x000E0B68 File Offset: 0x000DED68
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Project(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DAF RID: 11695 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DB0 RID: 11696 RVA: 0x000E0B79 File Offset: 0x000DED79
			// Note: this type is marked as 'beforefieldinit'.
			static Project_00002D33$BurstDirectCall()
			{
				Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DB1 RID: 11697 RVA: 0x000E0B80 File Offset: 0x000DED80
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Project$BurstManaged(L);
			}

			// Token: 0x040032A3 RID: 12963
			private static IntPtr Pointer;

			// Token: 0x040032A4 RID: 12964
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200073B RID: 1851
		// (Invoke) Token: 0x06002DB3 RID: 11699
		public unsafe delegate int Length_00002D34$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200073C RID: 1852
		internal static class Length_00002D34$BurstDirectCall
		{
			// Token: 0x06002DB6 RID: 11702 RVA: 0x000E0BB1 File Offset: 0x000DEDB1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Length$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Length_00002D34$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DB7 RID: 11703 RVA: 0x000E0BE0 File Offset: 0x000DEDE0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DB8 RID: 11704 RVA: 0x000E0BF8 File Offset: 0x000DEDF8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Length(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DB9 RID: 11705 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DBA RID: 11706 RVA: 0x000E0C09 File Offset: 0x000DEE09
			// Note: this type is marked as 'beforefieldinit'.
			static Length_00002D34$BurstDirectCall()
			{
				Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DBB RID: 11707 RVA: 0x000E0C10 File Offset: 0x000DEE10
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Length$BurstManaged(L);
			}

			// Token: 0x040032A5 RID: 12965
			private static IntPtr Pointer;

			// Token: 0x040032A6 RID: 12966
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200073D RID: 1853
		// (Invoke) Token: 0x06002DBD RID: 11709
		public unsafe delegate int Normalize_00002D35$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200073E RID: 1854
		internal static class Normalize_00002D35$BurstDirectCall
		{
			// Token: 0x06002DC0 RID: 11712 RVA: 0x000E0C41 File Offset: 0x000DEE41
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Normalize$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Normalize_00002D35$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DC1 RID: 11713 RVA: 0x000E0C70 File Offset: 0x000DEE70
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DC2 RID: 11714 RVA: 0x000E0C88 File Offset: 0x000DEE88
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Normalize(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DC3 RID: 11715 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DC4 RID: 11716 RVA: 0x000E0C99 File Offset: 0x000DEE99
			// Note: this type is marked as 'beforefieldinit'.
			static Normalize_00002D35$BurstDirectCall()
			{
				Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DC5 RID: 11717 RVA: 0x000E0CA0 File Offset: 0x000DEEA0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Normalize$BurstManaged(L);
			}

			// Token: 0x040032A7 RID: 12967
			private static IntPtr Pointer;

			// Token: 0x040032A8 RID: 12968
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200073F RID: 1855
		// (Invoke) Token: 0x06002DC7 RID: 11719
		public unsafe delegate int SafeNormal_00002D36$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000740 RID: 1856
		internal static class SafeNormal_00002D36$BurstDirectCall
		{
			// Token: 0x06002DCA RID: 11722 RVA: 0x000E0CD1 File Offset: 0x000DEED1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.SafeNormal$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.SafeNormal_00002D36$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DCB RID: 11723 RVA: 0x000E0D00 File Offset: 0x000DEF00
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DCC RID: 11724 RVA: 0x000E0D18 File Offset: 0x000DEF18
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.SafeNormal(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DCD RID: 11725 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DCE RID: 11726 RVA: 0x000E0D29 File Offset: 0x000DEF29
			// Note: this type is marked as 'beforefieldinit'.
			static SafeNormal_00002D36$BurstDirectCall()
			{
				Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DCF RID: 11727 RVA: 0x000E0D30 File Offset: 0x000DEF30
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.SafeNormal$BurstManaged(L);
			}

			// Token: 0x040032A9 RID: 12969
			private static IntPtr Pointer;

			// Token: 0x040032AA RID: 12970
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000741 RID: 1857
		// (Invoke) Token: 0x06002DD1 RID: 11729
		public unsafe delegate int Distance_00002D37$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000742 RID: 1858
		internal static class Distance_00002D37$BurstDirectCall
		{
			// Token: 0x06002DD4 RID: 11732 RVA: 0x000E0D61 File Offset: 0x000DEF61
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Distance$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Distance_00002D37$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DD5 RID: 11733 RVA: 0x000E0D90 File Offset: 0x000DEF90
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DD6 RID: 11734 RVA: 0x000E0DA8 File Offset: 0x000DEFA8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Distance(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DD7 RID: 11735 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DD8 RID: 11736 RVA: 0x000E0DB9 File Offset: 0x000DEFB9
			// Note: this type is marked as 'beforefieldinit'.
			static Distance_00002D37$BurstDirectCall()
			{
				Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DD9 RID: 11737 RVA: 0x000E0DC0 File Offset: 0x000DEFC0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Distance$BurstManaged(L);
			}

			// Token: 0x040032AB RID: 12971
			private static IntPtr Pointer;

			// Token: 0x040032AC RID: 12972
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000743 RID: 1859
		// (Invoke) Token: 0x06002DDB RID: 11739
		public unsafe delegate int Lerp_00002D38$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000744 RID: 1860
		internal static class Lerp_00002D38$BurstDirectCall
		{
			// Token: 0x06002DDE RID: 11742 RVA: 0x000E0DF1 File Offset: 0x000DEFF1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Lerp$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Lerp_00002D38$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DDF RID: 11743 RVA: 0x000E0E20 File Offset: 0x000DF020
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DE0 RID: 11744 RVA: 0x000E0E38 File Offset: 0x000DF038
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Lerp(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DE1 RID: 11745 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DE2 RID: 11746 RVA: 0x000E0E49 File Offset: 0x000DF049
			// Note: this type is marked as 'beforefieldinit'.
			static Lerp_00002D38$BurstDirectCall()
			{
				Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DE3 RID: 11747 RVA: 0x000E0E50 File Offset: 0x000DF050
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Lerp$BurstManaged(L);
			}

			// Token: 0x040032AD RID: 12973
			private static IntPtr Pointer;

			// Token: 0x040032AE RID: 12974
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000745 RID: 1861
		// (Invoke) Token: 0x06002DE5 RID: 11749
		public unsafe delegate int Rotate_00002D39$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000746 RID: 1862
		internal static class Rotate_00002D39$BurstDirectCall
		{
			// Token: 0x06002DE8 RID: 11752 RVA: 0x000E0E81 File Offset: 0x000DF081
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Rotate$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Rotate_00002D39$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DE9 RID: 11753 RVA: 0x000E0EB0 File Offset: 0x000DF0B0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DEA RID: 11754 RVA: 0x000E0EC8 File Offset: 0x000DF0C8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Rotate(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DEB RID: 11755 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DEC RID: 11756 RVA: 0x000E0ED9 File Offset: 0x000DF0D9
			// Note: this type is marked as 'beforefieldinit'.
			static Rotate_00002D39$BurstDirectCall()
			{
				Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DED RID: 11757 RVA: 0x000E0EE0 File Offset: 0x000DF0E0
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.Rotate$BurstManaged(L);
			}

			// Token: 0x040032AF RID: 12975
			private static IntPtr Pointer;

			// Token: 0x040032B0 RID: 12976
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000747 RID: 1863
		// (Invoke) Token: 0x06002DEF RID: 11759
		public unsafe delegate int ZeroVector_00002D3A$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000748 RID: 1864
		internal static class ZeroVector_00002D3A$BurstDirectCall
		{
			// Token: 0x06002DF2 RID: 11762 RVA: 0x000E0F11 File Offset: 0x000DF111
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.ZeroVector$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.ZeroVector_00002D3A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DF3 RID: 11763 RVA: 0x000E0F40 File Offset: 0x000DF140
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DF4 RID: 11764 RVA: 0x000E0F58 File Offset: 0x000DF158
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.ZeroVector(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DF5 RID: 11765 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002DF6 RID: 11766 RVA: 0x000E0F69 File Offset: 0x000DF169
			// Note: this type is marked as 'beforefieldinit'.
			static ZeroVector_00002D3A$BurstDirectCall()
			{
				Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DF7 RID: 11767 RVA: 0x000E0F70 File Offset: 0x000DF170
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.ZeroVector$BurstManaged(L);
			}

			// Token: 0x040032B1 RID: 12977
			private static IntPtr Pointer;

			// Token: 0x040032B2 RID: 12978
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000749 RID: 1865
		// (Invoke) Token: 0x06002DF9 RID: 11769
		public unsafe delegate int OneVector_00002D3B$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200074A RID: 1866
		internal static class OneVector_00002D3B$BurstDirectCall
		{
			// Token: 0x06002DFC RID: 11772 RVA: 0x000E0FA1 File Offset: 0x000DF1A1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.OneVector$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.OneVector_00002D3B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DFD RID: 11773 RVA: 0x000E0FD0 File Offset: 0x000DF1D0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DFE RID: 11774 RVA: 0x000E0FE8 File Offset: 0x000DF1E8
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.OneVector(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DFF RID: 11775 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E00 RID: 11776 RVA: 0x000E0FF9 File Offset: 0x000DF1F9
			// Note: this type is marked as 'beforefieldinit'.
			static OneVector_00002D3B$BurstDirectCall()
			{
				Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E01 RID: 11777 RVA: 0x000E1000 File Offset: 0x000DF200
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.Vec3Functions.OneVector$BurstManaged(L);
			}

			// Token: 0x040032B3 RID: 12979
			private static IntPtr Pointer;

			// Token: 0x040032B4 RID: 12980
			private static IntPtr DeferredCompilation;
		}
	}

	// Token: 0x0200074B RID: 1867
	[BurstCompile]
	public static class QuatFunctions
	{
		// Token: 0x06002E02 RID: 11778 RVA: 0x000E1031 File Offset: 0x000DF231
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int New(lua_State* L)
		{
			return Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x000E1039 File Offset: 0x000DF239
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Mul(lua_State* L)
		{
			return Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000E1041 File Offset: 0x000DF241
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Eq(lua_State* L)
		{
			return Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000E104C File Offset: 0x000DF24C
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int ToString(lua_State* L)
		{
			Quaternion quaternion = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			Luau.lua_pushstring(L, quaternion.ToString());
			return 1;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000E1085 File Offset: 0x000DF285
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int FromEuler(lua_State* L)
		{
			return Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000E108D File Offset: 0x000DF28D
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int FromDirection(lua_State* L)
		{
			return Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000E1095 File Offset: 0x000DF295
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int GetUpVector(lua_State* L)
		{
			return Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000E109D File Offset: 0x000DF29D
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Euler(lua_State* L)
		{
			return Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x000E10A8 File Offset: 0x000DF2A8
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

		// Token: 0x06002E0B RID: 11787 RVA: 0x000E1124 File Offset: 0x000DF324
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Mul$BurstManaged(lua_State* L)
		{
			Quaternion lhs = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			Quaternion rhs = *Luau.lua_class_get<Quaternion>(L, 2, "Quat");
			*Luau.lua_class_push<Quaternion>(L, "Quat") = lhs * rhs;
			return 1;
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000E117C File Offset: 0x000DF37C
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

		// Token: 0x06002E0D RID: 11789 RVA: 0x000E11CC File Offset: 0x000DF3CC
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

		// Token: 0x06002E0E RID: 11790 RVA: 0x000E1230 File Offset: 0x000DF430
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int FromDirection$BurstManaged(lua_State* L)
		{
			Vector3 lookRotation = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Luau.lua_class_push<Quaternion>(L, "Quat")->SetLookRotation(lookRotation);
			return 1;
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x000E126C File Offset: 0x000DF46C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int GetUpVector$BurstManaged(lua_State* L)
		{
			Quaternion rotation = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = rotation * Vector3.up;
			return 1;
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x000E12B4 File Offset: 0x000DF4B4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Euler$BurstManaged(lua_State* L)
		{
			Quaternion quaternion = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = quaternion.eulerAngles;
			return 1;
		}

		// Token: 0x0200074C RID: 1868
		// (Invoke) Token: 0x06002E12 RID: 11794
		public unsafe delegate int New_00002D3C$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200074D RID: 1869
		internal static class New_00002D3C$BurstDirectCall
		{
			// Token: 0x06002E15 RID: 11797 RVA: 0x000E12F5 File Offset: 0x000DF4F5
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.New$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.New_00002D3C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E16 RID: 11798 RVA: 0x000E1324 File Offset: 0x000DF524
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E17 RID: 11799 RVA: 0x000E133C File Offset: 0x000DF53C
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.New(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E18 RID: 11800 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E19 RID: 11801 RVA: 0x000E134D File Offset: 0x000DF54D
			// Note: this type is marked as 'beforefieldinit'.
			static New_00002D3C$BurstDirectCall()
			{
				Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E1A RID: 11802 RVA: 0x000E1354 File Offset: 0x000DF554
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.New$BurstManaged(L);
			}

			// Token: 0x040032B5 RID: 12981
			private static IntPtr Pointer;

			// Token: 0x040032B6 RID: 12982
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200074E RID: 1870
		// (Invoke) Token: 0x06002E1C RID: 11804
		public unsafe delegate int Mul_00002D3D$PostfixBurstDelegate(lua_State* L);

		// Token: 0x0200074F RID: 1871
		internal static class Mul_00002D3D$BurstDirectCall
		{
			// Token: 0x06002E1F RID: 11807 RVA: 0x000E1385 File Offset: 0x000DF585
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.Mul$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.Mul_00002D3D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E20 RID: 11808 RVA: 0x000E13B4 File Offset: 0x000DF5B4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E21 RID: 11809 RVA: 0x000E13CC File Offset: 0x000DF5CC
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.Mul(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E22 RID: 11810 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E23 RID: 11811 RVA: 0x000E13DD File Offset: 0x000DF5DD
			// Note: this type is marked as 'beforefieldinit'.
			static Mul_00002D3D$BurstDirectCall()
			{
				Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E24 RID: 11812 RVA: 0x000E13E4 File Offset: 0x000DF5E4
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.Mul$BurstManaged(L);
			}

			// Token: 0x040032B7 RID: 12983
			private static IntPtr Pointer;

			// Token: 0x040032B8 RID: 12984
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000750 RID: 1872
		// (Invoke) Token: 0x06002E26 RID: 11814
		public unsafe delegate int Eq_00002D3E$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000751 RID: 1873
		internal static class Eq_00002D3E$BurstDirectCall
		{
			// Token: 0x06002E29 RID: 11817 RVA: 0x000E1415 File Offset: 0x000DF615
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.Eq$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.Eq_00002D3E$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E2A RID: 11818 RVA: 0x000E1444 File Offset: 0x000DF644
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E2B RID: 11819 RVA: 0x000E145C File Offset: 0x000DF65C
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.Eq(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E2C RID: 11820 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E2D RID: 11821 RVA: 0x000E146D File Offset: 0x000DF66D
			// Note: this type is marked as 'beforefieldinit'.
			static Eq_00002D3E$BurstDirectCall()
			{
				Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E2E RID: 11822 RVA: 0x000E1474 File Offset: 0x000DF674
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.Eq$BurstManaged(L);
			}

			// Token: 0x040032B9 RID: 12985
			private static IntPtr Pointer;

			// Token: 0x040032BA RID: 12986
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000752 RID: 1874
		// (Invoke) Token: 0x06002E30 RID: 11824
		public unsafe delegate int FromEuler_00002D40$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000753 RID: 1875
		internal static class FromEuler_00002D40$BurstDirectCall
		{
			// Token: 0x06002E33 RID: 11827 RVA: 0x000E14A5 File Offset: 0x000DF6A5
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.FromEuler$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.FromEuler_00002D40$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E34 RID: 11828 RVA: 0x000E14D4 File Offset: 0x000DF6D4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E35 RID: 11829 RVA: 0x000E14EC File Offset: 0x000DF6EC
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.FromEuler(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E36 RID: 11830 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E37 RID: 11831 RVA: 0x000E14FD File Offset: 0x000DF6FD
			// Note: this type is marked as 'beforefieldinit'.
			static FromEuler_00002D40$BurstDirectCall()
			{
				Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E38 RID: 11832 RVA: 0x000E1504 File Offset: 0x000DF704
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.FromEuler$BurstManaged(L);
			}

			// Token: 0x040032BB RID: 12987
			private static IntPtr Pointer;

			// Token: 0x040032BC RID: 12988
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000754 RID: 1876
		// (Invoke) Token: 0x06002E3A RID: 11834
		public unsafe delegate int FromDirection_00002D41$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000755 RID: 1877
		internal static class FromDirection_00002D41$BurstDirectCall
		{
			// Token: 0x06002E3D RID: 11837 RVA: 0x000E1535 File Offset: 0x000DF735
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.FromDirection$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.FromDirection_00002D41$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E3E RID: 11838 RVA: 0x000E1564 File Offset: 0x000DF764
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E3F RID: 11839 RVA: 0x000E157C File Offset: 0x000DF77C
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.FromDirection(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E40 RID: 11840 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E41 RID: 11841 RVA: 0x000E158D File Offset: 0x000DF78D
			// Note: this type is marked as 'beforefieldinit'.
			static FromDirection_00002D41$BurstDirectCall()
			{
				Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E42 RID: 11842 RVA: 0x000E1594 File Offset: 0x000DF794
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.FromDirection$BurstManaged(L);
			}

			// Token: 0x040032BD RID: 12989
			private static IntPtr Pointer;

			// Token: 0x040032BE RID: 12990
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000756 RID: 1878
		// (Invoke) Token: 0x06002E44 RID: 11844
		public unsafe delegate int GetUpVector_00002D42$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000757 RID: 1879
		internal static class GetUpVector_00002D42$BurstDirectCall
		{
			// Token: 0x06002E47 RID: 11847 RVA: 0x000E15C5 File Offset: 0x000DF7C5
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.GetUpVector$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.GetUpVector_00002D42$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E48 RID: 11848 RVA: 0x000E15F4 File Offset: 0x000DF7F4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E49 RID: 11849 RVA: 0x000E160C File Offset: 0x000DF80C
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.GetUpVector(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E4A RID: 11850 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E4B RID: 11851 RVA: 0x000E161D File Offset: 0x000DF81D
			// Note: this type is marked as 'beforefieldinit'.
			static GetUpVector_00002D42$BurstDirectCall()
			{
				Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E4C RID: 11852 RVA: 0x000E1624 File Offset: 0x000DF824
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.GetUpVector$BurstManaged(L);
			}

			// Token: 0x040032BF RID: 12991
			private static IntPtr Pointer;

			// Token: 0x040032C0 RID: 12992
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000758 RID: 1880
		// (Invoke) Token: 0x06002E4E RID: 11854
		public unsafe delegate int Euler_00002D43$PostfixBurstDelegate(lua_State* L);

		// Token: 0x02000759 RID: 1881
		internal static class Euler_00002D43$BurstDirectCall
		{
			// Token: 0x06002E51 RID: 11857 RVA: 0x000E1655 File Offset: 0x000DF855
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.Euler$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.Euler_00002D43$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E52 RID: 11858 RVA: 0x000E1684 File Offset: 0x000DF884
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E53 RID: 11859 RVA: 0x000E169C File Offset: 0x000DF89C
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.Euler(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E54 RID: 11860 RVA: 0x000023F4 File Offset: 0x000005F4
			public static void Initialize()
			{
			}

			// Token: 0x06002E55 RID: 11861 RVA: 0x000E16AD File Offset: 0x000DF8AD
			// Note: this type is marked as 'beforefieldinit'.
			static Euler_00002D43$BurstDirectCall()
			{
				Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E56 RID: 11862 RVA: 0x000E16B4 File Offset: 0x000DF8B4
			public unsafe static int Invoke(lua_State* L)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(lua_State*), L, functionPointer);
					}
				}
				return Bindings.QuatFunctions.Euler$BurstManaged(L);
			}

			// Token: 0x040032C1 RID: 12993
			private static IntPtr Pointer;

			// Token: 0x040032C2 RID: 12994
			private static IntPtr DeferredCompilation;
		}
	}

	// Token: 0x0200075A RID: 1882
	public struct GorillaLocomotionSettings
	{
		// Token: 0x040032C3 RID: 12995
		public float velocityLimit;

		// Token: 0x040032C4 RID: 12996
		public float slideVelocityLimit;

		// Token: 0x040032C5 RID: 12997
		public float maxJumpSpeed;

		// Token: 0x040032C6 RID: 12998
		public float jumpMultiplier;
	}
}
