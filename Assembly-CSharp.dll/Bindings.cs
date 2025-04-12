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
	// Token: 0x06002D15 RID: 11541 RVA: 0x001246B8 File Offset: 0x001228B8
	public unsafe static void GameObjectBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Bindings.LuauGameObject>("GameObject").AddField("position", "Position").AddField("rotation", "Rotation").AddField("scale", "Scale").AddStaticFunction("findGameObject", new lua_CFunction(Bindings.GameObjectFunctions.FindGameObject)).AddFunction("setCollision", new lua_CFunction(Bindings.GameObjectFunctions.SetCollision)).AddFunction("setVisibility", new lua_CFunction(Bindings.GameObjectFunctions.SetVisibility)).Build(L, true));
	}

	// Token: 0x06002D16 RID: 11542 RVA: 0x00124750 File Offset: 0x00122950
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

	// Token: 0x06002D17 RID: 11543 RVA: 0x00124804 File Offset: 0x00122A04
	public unsafe static void Vec3Builder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Vector3>("Vec3").AddField("x", null).AddField("y", null).AddField("z", null).AddStaticFunction("new", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.New))).AddFunction("__add", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Add))).AddFunction("__sub", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Sub))).AddFunction("__mul", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Mul))).AddFunction("__div", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Div))).AddFunction("__unm", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Unm))).AddFunction("__eq", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Eq))).AddFunction("__tostring", new lua_CFunction(Bindings.Vec3Functions.ToSring)).AddFunction("toString", new lua_CFunction(Bindings.Vec3Functions.ToSring)).AddFunction("dot", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Dot))).AddFunction("cross", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Cross))).AddFunction("projectOnTo", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Project))).AddFunction("length", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Length))).AddFunction("normalize", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Normalize))).AddFunction("getSafeNormal", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.SafeNormal))).AddStaticFunction("rotate", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Rotate))).AddFunction("rotate", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Rotate))).AddStaticFunction("distance", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Distance))).AddFunction("distance", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Distance))).AddStaticFunction("lerp", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Lerp))).AddFunction("lerp", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.Lerp))).AddProperty("zeroVector", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.ZeroVector))).AddProperty("oneVector", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.Vec3Functions.OneVector))).Build(L, true));
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x00124AB4 File Offset: 0x00122CB4
	public unsafe static void QuatBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Quaternion>("Quat").AddField("x", null).AddField("y", null).AddField("z", null).AddField("w", null).AddStaticFunction("new", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.New))).AddFunction("__mul", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.Mul))).AddFunction("__eq", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.Eq))).AddFunction("__tostring", new lua_CFunction(Bindings.QuatFunctions.ToString)).AddFunction("toString", new lua_CFunction(Bindings.QuatFunctions.ToString)).AddStaticFunction("fromEuler", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.FromEuler))).AddStaticFunction("fromDirection", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.FromDirection))).AddFunction("getUpVector", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.GetUpVector))).AddFunction("euler", BurstCompiler.CompileFunctionPointer<lua_CFunction>(new lua_CFunction(Bindings.QuatFunctions.Euler))).Build(L, true));
	}

	// Token: 0x06002D19 RID: 11545 RVA: 0x00124BF4 File Offset: 0x00122DF4
	public unsafe static void PlayerBuilder(lua_State* L)
	{
		LuauVm.ClassBuilders.Append(new LuauClassBuilder<Bindings.LuauPlayer>("Player").AddField("playerID", "PlayerID").AddField("playerName", "PlayerName").AddField("playerMaterial", "PlayerMaterial").AddField("isMasterClient", "IsMasterClient").AddField("bodyPosition", "BodyPosition").AddField("leftHandPosition", "LeftHandPosition").AddField("rightHandPosition", "RightHandPosition").AddField("headRotation", "HeadRotation").AddField("leftHandRotation", "LeftHandRotation").AddField("rightHandRotation", "RightHandRotation").AddStaticFunction("getPlayerByID", new lua_CFunction(Bindings.PlayerFunctions.GetPlayerByID)).Build(L, true));
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x00124CCC File Offset: 0x00122ECC
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int LuaStartVibration(lua_State* L)
	{
		bool forLeftController = Luau.lua_toboolean(L, 1) == 1;
		float amplitude = (float)Luau.luaL_checknumber(L, 2);
		float duration = (float)Luau.luaL_checknumber(L, 3);
		GorillaTagger.Instance.StartVibration(forLeftController, amplitude, duration);
		return 0;
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x00124D04 File Offset: 0x00122F04
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
		// Token: 0x06002D1D RID: 11549 RVA: 0x00124D64 File Offset: 0x00122F64
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
		// Token: 0x06002D1F RID: 11551 RVA: 0x00124FF4 File Offset: 0x001231F4
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

		// Token: 0x06002D20 RID: 11552 RVA: 0x00125058 File Offset: 0x00123258
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

		// Token: 0x06002D21 RID: 11553 RVA: 0x0012511C File Offset: 0x0012331C
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SetCollision(lua_State* L)
		{
			Bindings.LuauGameObject* data = Luau.lua_class_get<Bindings.LuauGameObject>(L, 1, "GameObject");
			Bindings.LuauGameObjectList.FirstOrDefault((KeyValuePair<GameObject, IntPtr> g) => g.Value == (IntPtr)((void*)data)).Key.GetComponent<Collider>().enabled = (Luau.lua_toboolean(L, 2) == 1);
			return 0;
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x0012517C File Offset: 0x0012337C
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
		// Token: 0x06002D27 RID: 11559 RVA: 0x001251DC File Offset: 0x001233DC
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

		// Token: 0x06002D28 RID: 11560 RVA: 0x00125340 File Offset: 0x00123540
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
		// Token: 0x06002D29 RID: 11561 RVA: 0x0004DC78 File Offset: 0x0004BE78
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int New(lua_State* L)
		{
			return Bindings.Vec3Functions.New_00002D29$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x0004DC80 File Offset: 0x0004BE80
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Add(lua_State* L)
		{
			return Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x0004DC88 File Offset: 0x0004BE88
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Sub(lua_State* L)
		{
			return Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x0004DC90 File Offset: 0x0004BE90
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Mul(lua_State* L)
		{
			return Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x0004DC98 File Offset: 0x0004BE98
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Div(lua_State* L)
		{
			return Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x0004DCA0 File Offset: 0x0004BEA0
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Unm(lua_State* L)
		{
			return Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x0004DCA8 File Offset: 0x0004BEA8
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Eq(lua_State* L)
		{
			return Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x001253B4 File Offset: 0x001235B4
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int ToSring(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Luau.lua_pushstring(L, vector.ToString());
			return 1;
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x0004DCB0 File Offset: 0x0004BEB0
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Dot(lua_State* L)
		{
			return Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x0004DCB8 File Offset: 0x0004BEB8
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Cross(lua_State* L)
		{
			return Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x0004DCC0 File Offset: 0x0004BEC0
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Project(lua_State* L)
		{
			return Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x0004DCC8 File Offset: 0x0004BEC8
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Length(lua_State* L)
		{
			return Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x0004DCD0 File Offset: 0x0004BED0
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Normalize(lua_State* L)
		{
			return Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x0004DCD8 File Offset: 0x0004BED8
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int SafeNormal(lua_State* L)
		{
			return Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x0004DCE0 File Offset: 0x0004BEE0
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Distance(lua_State* L)
		{
			return Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x0004DCE8 File Offset: 0x0004BEE8
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Lerp(lua_State* L)
		{
			return Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x0004DCF0 File Offset: 0x0004BEF0
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Rotate(lua_State* L)
		{
			return Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x0004DCF8 File Offset: 0x0004BEF8
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int ZeroVector(lua_State* L)
		{
			return Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x0004DD00 File Offset: 0x0004BF00
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int OneVector(lua_State* L)
		{
			return Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x001253F0 File Offset: 0x001235F0
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

		// Token: 0x06002D3D RID: 11581 RVA: 0x00125454 File Offset: 0x00123654
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Add$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a + b;
			return 1;
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x001254AC File Offset: 0x001236AC
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Sub$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a - b;
			return 1;
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x00125504 File Offset: 0x00123704
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Mul$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			float d = (float)Luau.luaL_checknumber(L, 2);
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a * d;
			return 1;
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x00125550 File Offset: 0x00123750
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Div$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			float d = (float)Luau.luaL_checknumber(L, 2);
			*Luau.lua_class_push<Vector3>(L, "Vec3") = a / d;
			return 1;
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x0012559C File Offset: 0x0012379C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Unm$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = -a;
			return 1;
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x001255DC File Offset: 0x001237DC
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

		// Token: 0x06002D43 RID: 11587 RVA: 0x0012562C File Offset: 0x0012382C
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

		// Token: 0x06002D44 RID: 11588 RVA: 0x00125678 File Offset: 0x00123878
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Cross$BurstManaged(lua_State* L)
		{
			Vector3 lhs = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 rhs = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = Vector3.Cross(lhs, rhs);
			return 1;
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x001256D0 File Offset: 0x001238D0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Project$BurstManaged(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 onNormal = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = Vector3.Project(vector, onNormal);
			return 1;
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x00125728 File Offset: 0x00123928
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Length$BurstManaged(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Luau.lua_pushnumber(L, (double)Vector3.Magnitude(vector));
			return 1;
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x0004DD08 File Offset: 0x0004BF08
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Normalize$BurstManaged(lua_State* L)
		{
			Luau.lua_class_get<Vector3>(L, 1, "Vec3")->Normalize();
			return 0;
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x0012575C File Offset: 0x0012395C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int SafeNormal$BurstManaged(lua_State* L)
		{
			Vector3 vector = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = vector.normalized;
			return 1;
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x001257A0 File Offset: 0x001239A0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Distance$BurstManaged(lua_State* L)
		{
			Vector3 a = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Vector3 b = *Luau.lua_class_get<Vector3>(L, 2, "Vec3");
			Luau.lua_pushnumber(L, (double)Vector3.Distance(a, b));
			return 1;
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x001257EC File Offset: 0x001239EC
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

		// Token: 0x06002D4B RID: 11595 RVA: 0x00125850 File Offset: 0x00123A50
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Rotate$BurstManaged(lua_State* L)
		{
			Vector3 point = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Quaternion rotation = *Luau.lua_class_get<Quaternion>(L, 2, "Quat");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = rotation * point;
			return 1;
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x0004DD21 File Offset: 0x0004BF21
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

		// Token: 0x06002D4D RID: 11597 RVA: 0x0004DD54 File Offset: 0x0004BF54
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
			// Token: 0x06002D52 RID: 11602 RVA: 0x0004DD87 File Offset: 0x0004BF87
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.New_00002D29$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.New_00002D29$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.New_00002D29$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.New$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.New_00002D29$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.New_00002D29$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D53 RID: 11603 RVA: 0x001258A8 File Offset: 0x00123AA8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.New_00002D29$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D54 RID: 11604 RVA: 0x0004DDB3 File Offset: 0x0004BFB3
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.New_00002D29$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.New(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D55 RID: 11605 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002D56 RID: 11606 RVA: 0x0004DDC4 File Offset: 0x0004BFC4
			// Note: this type is marked as 'beforefieldinit'.
			static New_00002D29$BurstDirectCall()
			{
				Bindings.Vec3Functions.New_00002D29$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D57 RID: 11607 RVA: 0x001258C0 File Offset: 0x00123AC0
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
			// Token: 0x06002D5C RID: 11612 RVA: 0x0004DDCB File Offset: 0x0004BFCB
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Add$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Add_00002D2A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D5D RID: 11613 RVA: 0x001258F4 File Offset: 0x00123AF4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D5E RID: 11614 RVA: 0x0004DDF7 File Offset: 0x0004BFF7
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Add(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D5F RID: 11615 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002D60 RID: 11616 RVA: 0x0004DE08 File Offset: 0x0004C008
			// Note: this type is marked as 'beforefieldinit'.
			static Add_00002D2A$BurstDirectCall()
			{
				Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D61 RID: 11617 RVA: 0x0012590C File Offset: 0x00123B0C
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
			// Token: 0x06002D66 RID: 11622 RVA: 0x0004DE0F File Offset: 0x0004C00F
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Sub$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Sub_00002D2B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D67 RID: 11623 RVA: 0x00125940 File Offset: 0x00123B40
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D68 RID: 11624 RVA: 0x0004DE3B File Offset: 0x0004C03B
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Sub(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D69 RID: 11625 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002D6A RID: 11626 RVA: 0x0004DE4C File Offset: 0x0004C04C
			// Note: this type is marked as 'beforefieldinit'.
			static Sub_00002D2B$BurstDirectCall()
			{
				Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D6B RID: 11627 RVA: 0x00125958 File Offset: 0x00123B58
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
			// Token: 0x06002D70 RID: 11632 RVA: 0x0004DE53 File Offset: 0x0004C053
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Mul$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Mul_00002D2C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D71 RID: 11633 RVA: 0x0012598C File Offset: 0x00123B8C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D72 RID: 11634 RVA: 0x0004DE7F File Offset: 0x0004C07F
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Mul(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D73 RID: 11635 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002D74 RID: 11636 RVA: 0x0004DE90 File Offset: 0x0004C090
			// Note: this type is marked as 'beforefieldinit'.
			static Mul_00002D2C$BurstDirectCall()
			{
				Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D75 RID: 11637 RVA: 0x001259A4 File Offset: 0x00123BA4
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
			// Token: 0x06002D7A RID: 11642 RVA: 0x0004DE97 File Offset: 0x0004C097
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Div$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Div_00002D2D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D7B RID: 11643 RVA: 0x001259D8 File Offset: 0x00123BD8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D7C RID: 11644 RVA: 0x0004DEC3 File Offset: 0x0004C0C3
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Div(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D7D RID: 11645 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002D7E RID: 11646 RVA: 0x0004DED4 File Offset: 0x0004C0D4
			// Note: this type is marked as 'beforefieldinit'.
			static Div_00002D2D$BurstDirectCall()
			{
				Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D7F RID: 11647 RVA: 0x001259F0 File Offset: 0x00123BF0
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
			// Token: 0x06002D84 RID: 11652 RVA: 0x0004DEDB File Offset: 0x0004C0DB
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Unm$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Unm_00002D2E$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D85 RID: 11653 RVA: 0x00125A24 File Offset: 0x00123C24
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D86 RID: 11654 RVA: 0x0004DF07 File Offset: 0x0004C107
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Unm(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D87 RID: 11655 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002D88 RID: 11656 RVA: 0x0004DF18 File Offset: 0x0004C118
			// Note: this type is marked as 'beforefieldinit'.
			static Unm_00002D2E$BurstDirectCall()
			{
				Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D89 RID: 11657 RVA: 0x00125A3C File Offset: 0x00123C3C
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
			// Token: 0x06002D8E RID: 11662 RVA: 0x0004DF1F File Offset: 0x0004C11F
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Eq$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Eq_00002D2F$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D8F RID: 11663 RVA: 0x00125A70 File Offset: 0x00123C70
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D90 RID: 11664 RVA: 0x0004DF4B File Offset: 0x0004C14B
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Eq(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D91 RID: 11665 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002D92 RID: 11666 RVA: 0x0004DF5C File Offset: 0x0004C15C
			// Note: this type is marked as 'beforefieldinit'.
			static Eq_00002D2F$BurstDirectCall()
			{
				Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D93 RID: 11667 RVA: 0x00125A88 File Offset: 0x00123C88
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
			// Token: 0x06002D98 RID: 11672 RVA: 0x0004DF63 File Offset: 0x0004C163
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Dot$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Dot_00002D31$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.Pointer;
			}

			// Token: 0x06002D99 RID: 11673 RVA: 0x00125ABC File Offset: 0x00123CBC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002D9A RID: 11674 RVA: 0x0004DF8F File Offset: 0x0004C18F
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Dot(lua_State*)).MethodHandle);
			}

			// Token: 0x06002D9B RID: 11675 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002D9C RID: 11676 RVA: 0x0004DFA0 File Offset: 0x0004C1A0
			// Note: this type is marked as 'beforefieldinit'.
			static Dot_00002D31$BurstDirectCall()
			{
				Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.Constructor();
			}

			// Token: 0x06002D9D RID: 11677 RVA: 0x00125AD4 File Offset: 0x00123CD4
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
			// Token: 0x06002DA2 RID: 11682 RVA: 0x0004DFA7 File Offset: 0x0004C1A7
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Cross$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Cross_00002D32$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DA3 RID: 11683 RVA: 0x00125B08 File Offset: 0x00123D08
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DA4 RID: 11684 RVA: 0x0004DFD3 File Offset: 0x0004C1D3
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Cross(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DA5 RID: 11685 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002DA6 RID: 11686 RVA: 0x0004DFE4 File Offset: 0x0004C1E4
			// Note: this type is marked as 'beforefieldinit'.
			static Cross_00002D32$BurstDirectCall()
			{
				Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DA7 RID: 11687 RVA: 0x00125B20 File Offset: 0x00123D20
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
			// Token: 0x06002DAC RID: 11692 RVA: 0x0004DFEB File Offset: 0x0004C1EB
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Project$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Project_00002D33$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DAD RID: 11693 RVA: 0x00125B54 File Offset: 0x00123D54
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DAE RID: 11694 RVA: 0x0004E017 File Offset: 0x0004C217
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Project(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DAF RID: 11695 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002DB0 RID: 11696 RVA: 0x0004E028 File Offset: 0x0004C228
			// Note: this type is marked as 'beforefieldinit'.
			static Project_00002D33$BurstDirectCall()
			{
				Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DB1 RID: 11697 RVA: 0x00125B6C File Offset: 0x00123D6C
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
			// Token: 0x06002DB6 RID: 11702 RVA: 0x0004E02F File Offset: 0x0004C22F
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Length$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Length_00002D34$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DB7 RID: 11703 RVA: 0x00125BA0 File Offset: 0x00123DA0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DB8 RID: 11704 RVA: 0x0004E05B File Offset: 0x0004C25B
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Length(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DB9 RID: 11705 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002DBA RID: 11706 RVA: 0x0004E06C File Offset: 0x0004C26C
			// Note: this type is marked as 'beforefieldinit'.
			static Length_00002D34$BurstDirectCall()
			{
				Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DBB RID: 11707 RVA: 0x00125BB8 File Offset: 0x00123DB8
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
			// Token: 0x06002DC0 RID: 11712 RVA: 0x0004E073 File Offset: 0x0004C273
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Normalize$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Normalize_00002D35$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DC1 RID: 11713 RVA: 0x00125BEC File Offset: 0x00123DEC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DC2 RID: 11714 RVA: 0x0004E09F File Offset: 0x0004C29F
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Normalize(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DC3 RID: 11715 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002DC4 RID: 11716 RVA: 0x0004E0B0 File Offset: 0x0004C2B0
			// Note: this type is marked as 'beforefieldinit'.
			static Normalize_00002D35$BurstDirectCall()
			{
				Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DC5 RID: 11717 RVA: 0x00125C04 File Offset: 0x00123E04
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
			// Token: 0x06002DCA RID: 11722 RVA: 0x0004E0B7 File Offset: 0x0004C2B7
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.SafeNormal$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.SafeNormal_00002D36$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DCB RID: 11723 RVA: 0x00125C38 File Offset: 0x00123E38
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DCC RID: 11724 RVA: 0x0004E0E3 File Offset: 0x0004C2E3
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.SafeNormal(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DCD RID: 11725 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002DCE RID: 11726 RVA: 0x0004E0F4 File Offset: 0x0004C2F4
			// Note: this type is marked as 'beforefieldinit'.
			static SafeNormal_00002D36$BurstDirectCall()
			{
				Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DCF RID: 11727 RVA: 0x00125C50 File Offset: 0x00123E50
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
			// Token: 0x06002DD4 RID: 11732 RVA: 0x0004E0FB File Offset: 0x0004C2FB
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Distance$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Distance_00002D37$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DD5 RID: 11733 RVA: 0x00125C84 File Offset: 0x00123E84
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DD6 RID: 11734 RVA: 0x0004E127 File Offset: 0x0004C327
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Distance(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DD7 RID: 11735 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002DD8 RID: 11736 RVA: 0x0004E138 File Offset: 0x0004C338
			// Note: this type is marked as 'beforefieldinit'.
			static Distance_00002D37$BurstDirectCall()
			{
				Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DD9 RID: 11737 RVA: 0x00125C9C File Offset: 0x00123E9C
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
			// Token: 0x06002DDE RID: 11742 RVA: 0x0004E13F File Offset: 0x0004C33F
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Lerp$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Lerp_00002D38$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DDF RID: 11743 RVA: 0x00125CD0 File Offset: 0x00123ED0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DE0 RID: 11744 RVA: 0x0004E16B File Offset: 0x0004C36B
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Lerp(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DE1 RID: 11745 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002DE2 RID: 11746 RVA: 0x0004E17C File Offset: 0x0004C37C
			// Note: this type is marked as 'beforefieldinit'.
			static Lerp_00002D38$BurstDirectCall()
			{
				Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DE3 RID: 11747 RVA: 0x00125CE8 File Offset: 0x00123EE8
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
			// Token: 0x06002DE8 RID: 11752 RVA: 0x0004E183 File Offset: 0x0004C383
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.Rotate$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.Rotate_00002D39$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DE9 RID: 11753 RVA: 0x00125D1C File Offset: 0x00123F1C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DEA RID: 11754 RVA: 0x0004E1AF File Offset: 0x0004C3AF
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.Rotate(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DEB RID: 11755 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002DEC RID: 11756 RVA: 0x0004E1C0 File Offset: 0x0004C3C0
			// Note: this type is marked as 'beforefieldinit'.
			static Rotate_00002D39$BurstDirectCall()
			{
				Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DED RID: 11757 RVA: 0x00125D34 File Offset: 0x00123F34
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
			// Token: 0x06002DF2 RID: 11762 RVA: 0x0004E1C7 File Offset: 0x0004C3C7
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.ZeroVector$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.ZeroVector_00002D3A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DF3 RID: 11763 RVA: 0x00125D68 File Offset: 0x00123F68
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DF4 RID: 11764 RVA: 0x0004E1F3 File Offset: 0x0004C3F3
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.ZeroVector(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DF5 RID: 11765 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002DF6 RID: 11766 RVA: 0x0004E204 File Offset: 0x0004C404
			// Note: this type is marked as 'beforefieldinit'.
			static ZeroVector_00002D3A$BurstDirectCall()
			{
				Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.Constructor();
			}

			// Token: 0x06002DF7 RID: 11767 RVA: 0x00125D80 File Offset: 0x00123F80
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
			// Token: 0x06002DFC RID: 11772 RVA: 0x0004E20B File Offset: 0x0004C40B
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.Pointer == 0)
				{
					Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.DeferredCompilation, methodof(Bindings.Vec3Functions.OneVector$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.Vec3Functions.OneVector_00002D3B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.Pointer;
			}

			// Token: 0x06002DFD RID: 11773 RVA: 0x00125DB4 File Offset: 0x00123FB4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002DFE RID: 11774 RVA: 0x0004E237 File Offset: 0x0004C437
			public unsafe static void Constructor()
			{
				Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.Vec3Functions.OneVector(lua_State*)).MethodHandle);
			}

			// Token: 0x06002DFF RID: 11775 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002E00 RID: 11776 RVA: 0x0004E248 File Offset: 0x0004C448
			// Note: this type is marked as 'beforefieldinit'.
			static OneVector_00002D3B$BurstDirectCall()
			{
				Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E01 RID: 11777 RVA: 0x00125DCC File Offset: 0x00123FCC
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
		// Token: 0x06002E02 RID: 11778 RVA: 0x0004E24F File Offset: 0x0004C44F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int New(lua_State* L)
		{
			return Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x0004E257 File Offset: 0x0004C457
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Mul(lua_State* L)
		{
			return Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x0004E25F File Offset: 0x0004C45F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Eq(lua_State* L)
		{
			return Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x00125E00 File Offset: 0x00124000
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int ToString(lua_State* L)
		{
			Quaternion quaternion = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			Luau.lua_pushstring(L, quaternion.ToString());
			return 1;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x0004E267 File Offset: 0x0004C467
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int FromEuler(lua_State* L)
		{
			return Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x0004E26F File Offset: 0x0004C46F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int FromDirection(lua_State* L)
		{
			return Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x0004E277 File Offset: 0x0004C477
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int GetUpVector(lua_State* L)
		{
			return Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x0004E27F File Offset: 0x0004C47F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(lua_CFunction))]
		public unsafe static int Euler(lua_State* L)
		{
			return Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.Invoke(L);
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x00125E3C File Offset: 0x0012403C
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

		// Token: 0x06002E0B RID: 11787 RVA: 0x00125EB8 File Offset: 0x001240B8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Mul$BurstManaged(lua_State* L)
		{
			Quaternion lhs = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			Quaternion rhs = *Luau.lua_class_get<Quaternion>(L, 2, "Quat");
			*Luau.lua_class_push<Quaternion>(L, "Quat") = lhs * rhs;
			return 1;
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x00125F10 File Offset: 0x00124110
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

		// Token: 0x06002E0D RID: 11789 RVA: 0x00125F60 File Offset: 0x00124160
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

		// Token: 0x06002E0E RID: 11790 RVA: 0x00125FC4 File Offset: 0x001241C4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int FromDirection$BurstManaged(lua_State* L)
		{
			Vector3 lookRotation = *Luau.lua_class_get<Vector3>(L, 1, "Vec3");
			Luau.lua_class_push<Quaternion>(L, "Quat")->SetLookRotation(lookRotation);
			return 1;
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x00126000 File Offset: 0x00124200
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int GetUpVector$BurstManaged(lua_State* L)
		{
			Quaternion rotation = *Luau.lua_class_get<Quaternion>(L, 1, "Quat");
			*Luau.lua_class_push<Vector3>(L, "Vec3") = rotation * Vector3.up;
			return 1;
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x00126048 File Offset: 0x00124248
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
			// Token: 0x06002E15 RID: 11797 RVA: 0x0004E287 File Offset: 0x0004C487
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.New$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.New_00002D3C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E16 RID: 11798 RVA: 0x0012608C File Offset: 0x0012428C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E17 RID: 11799 RVA: 0x0004E2B3 File Offset: 0x0004C4B3
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.New(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E18 RID: 11800 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002E19 RID: 11801 RVA: 0x0004E2C4 File Offset: 0x0004C4C4
			// Note: this type is marked as 'beforefieldinit'.
			static New_00002D3C$BurstDirectCall()
			{
				Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E1A RID: 11802 RVA: 0x001260A4 File Offset: 0x001242A4
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
			// Token: 0x06002E1F RID: 11807 RVA: 0x0004E2CB File Offset: 0x0004C4CB
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.Mul$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.Mul_00002D3D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E20 RID: 11808 RVA: 0x001260D8 File Offset: 0x001242D8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E21 RID: 11809 RVA: 0x0004E2F7 File Offset: 0x0004C4F7
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.Mul(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E22 RID: 11810 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002E23 RID: 11811 RVA: 0x0004E308 File Offset: 0x0004C508
			// Note: this type is marked as 'beforefieldinit'.
			static Mul_00002D3D$BurstDirectCall()
			{
				Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E24 RID: 11812 RVA: 0x001260F0 File Offset: 0x001242F0
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
			// Token: 0x06002E29 RID: 11817 RVA: 0x0004E30F File Offset: 0x0004C50F
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.Eq$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.Eq_00002D3E$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E2A RID: 11818 RVA: 0x00126124 File Offset: 0x00124324
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E2B RID: 11819 RVA: 0x0004E33B File Offset: 0x0004C53B
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.Eq(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E2C RID: 11820 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002E2D RID: 11821 RVA: 0x0004E34C File Offset: 0x0004C54C
			// Note: this type is marked as 'beforefieldinit'.
			static Eq_00002D3E$BurstDirectCall()
			{
				Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E2E RID: 11822 RVA: 0x0012613C File Offset: 0x0012433C
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
			// Token: 0x06002E33 RID: 11827 RVA: 0x0004E353 File Offset: 0x0004C553
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.FromEuler$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.FromEuler_00002D40$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E34 RID: 11828 RVA: 0x00126170 File Offset: 0x00124370
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E35 RID: 11829 RVA: 0x0004E37F File Offset: 0x0004C57F
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.FromEuler(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E36 RID: 11830 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002E37 RID: 11831 RVA: 0x0004E390 File Offset: 0x0004C590
			// Note: this type is marked as 'beforefieldinit'.
			static FromEuler_00002D40$BurstDirectCall()
			{
				Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E38 RID: 11832 RVA: 0x00126188 File Offset: 0x00124388
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
			// Token: 0x06002E3D RID: 11837 RVA: 0x0004E397 File Offset: 0x0004C597
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.FromDirection$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.FromDirection_00002D41$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E3E RID: 11838 RVA: 0x001261BC File Offset: 0x001243BC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E3F RID: 11839 RVA: 0x0004E3C3 File Offset: 0x0004C5C3
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.FromDirection(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E40 RID: 11840 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002E41 RID: 11841 RVA: 0x0004E3D4 File Offset: 0x0004C5D4
			// Note: this type is marked as 'beforefieldinit'.
			static FromDirection_00002D41$BurstDirectCall()
			{
				Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E42 RID: 11842 RVA: 0x001261D4 File Offset: 0x001243D4
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
			// Token: 0x06002E47 RID: 11847 RVA: 0x0004E3DB File Offset: 0x0004C5DB
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.GetUpVector$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.GetUpVector_00002D42$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E48 RID: 11848 RVA: 0x00126208 File Offset: 0x00124408
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E49 RID: 11849 RVA: 0x0004E407 File Offset: 0x0004C607
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.GetUpVector(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E4A RID: 11850 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002E4B RID: 11851 RVA: 0x0004E418 File Offset: 0x0004C618
			// Note: this type is marked as 'beforefieldinit'.
			static GetUpVector_00002D42$BurstDirectCall()
			{
				Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E4C RID: 11852 RVA: 0x00126220 File Offset: 0x00124420
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
			// Token: 0x06002E51 RID: 11857 RVA: 0x0004E41F File Offset: 0x0004C61F
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.Pointer == 0)
				{
					Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.DeferredCompilation, methodof(Bindings.QuatFunctions.Euler$BurstManaged(lua_State*)).MethodHandle, typeof(Bindings.QuatFunctions.Euler_00002D43$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.Pointer;
			}

			// Token: 0x06002E52 RID: 11858 RVA: 0x00126254 File Offset: 0x00124454
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06002E53 RID: 11859 RVA: 0x0004E44B File Offset: 0x0004C64B
			public unsafe static void Constructor()
			{
				Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Bindings.QuatFunctions.Euler(lua_State*)).MethodHandle);
			}

			// Token: 0x06002E54 RID: 11860 RVA: 0x0002F75F File Offset: 0x0002D95F
			public static void Initialize()
			{
			}

			// Token: 0x06002E55 RID: 11861 RVA: 0x0004E45C File Offset: 0x0004C65C
			// Note: this type is marked as 'beforefieldinit'.
			static Euler_00002D43$BurstDirectCall()
			{
				Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.Constructor();
			}

			// Token: 0x06002E56 RID: 11862 RVA: 0x0012626C File Offset: 0x0012446C
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
