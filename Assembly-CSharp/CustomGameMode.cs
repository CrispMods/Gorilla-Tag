using System;
using System.Collections.Generic;
using System.IO;
using AOT;
using Fusion;
using GorillaExtensions;
using GorillaGameModes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x0200071C RID: 1820
public class CustomGameMode : GorillaGameManager
{
	// Token: 0x06002CF5 RID: 11509 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06002CF6 RID: 11510 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnSerializeRead(object obj)
	{
	}

	// Token: 0x06002CF7 RID: 11511 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06002CF8 RID: 11512 RVA: 0x00042E31 File Offset: 0x00041031
	public override object OnSerializeWrite()
	{
		return null;
	}

	// Token: 0x06002CF9 RID: 11513 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void AddFusionDataBehaviour(NetworkObject obj)
	{
	}

	// Token: 0x06002CFA RID: 11514 RVA: 0x000DE2F6 File Offset: 0x000DC4F6
	public override GameModeType GameType()
	{
		return GameModeType.Custom;
	}

	// Token: 0x06002CFB RID: 11515 RVA: 0x000DE2F9 File Offset: 0x000DC4F9
	public override string GameModeName()
	{
		return "CUSTOM";
	}

	// Token: 0x06002CFC RID: 11516 RVA: 0x000DE300 File Offset: 0x000DC500
	public unsafe override int MyMatIndex(NetPlayer forPlayer)
	{
		IntPtr value;
		if (Bindings.LuauPlayerList.TryGetValue(forPlayer.ActorNumber, out value))
		{
			return ((Bindings.LuauPlayer*)((void*)value))->PlayerMaterial;
		}
		return 0;
	}

	// Token: 0x06002CFD RID: 11517 RVA: 0x000DE330 File Offset: 0x000DC530
	public unsafe override void OnPlayerEnteredRoom(NetPlayer player)
	{
		try
		{
			if (CustomGameMode.gameScriptRunner != null)
			{
				if (CustomGameMode.gameScriptRunner.ShouldTick)
				{
					if (!Bindings.LuauPlayerList.ContainsKey(player.ActorNumber))
					{
						lua_State* l = CustomGameMode.gameScriptRunner.L;
						Luau.lua_getglobal(l, "Players");
						int num = Luau.lua_objlen(l, -1);
						Bindings.LuauPlayer* ptr = Luau.lua_class_push<Bindings.LuauPlayer>(l);
						ptr->PlayerID = player.ActorNumber;
						ptr->PlayerMaterial = 0;
						ptr->IsMasterClient = player.IsMasterClient;
						VRRig vrrig = this.FindPlayerVRRig(player);
						ptr->PlayerName = vrrig.playerNameVisible;
						Bindings.LuauVRRigList[player.ActorNumber] = vrrig;
						Bindings.PlayerFunctions.UpdatePlayer(l, vrrig, ptr);
						Bindings.LuauPlayerList[player.ActorNumber] = (IntPtr)((void*)ptr);
						Luau.lua_rawseti(CustomGameMode.gameScriptRunner.L, -2, num + 1);
						if (player.IsLocal)
						{
							Luau.lua_rawgeti(l, -1, num + 1);
							Luau.lua_setglobal(l, "LocalPlayer");
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.ToString());
		}
	}

	// Token: 0x06002CFE RID: 11518 RVA: 0x000DE44C File Offset: 0x000DC64C
	public unsafe override void OnPlayerLeftRoom(NetPlayer player)
	{
		try
		{
			if (CustomGameMode.gameScriptRunner != null)
			{
				if (CustomGameMode.gameScriptRunner.ShouldTick)
				{
					lua_State* l = CustomGameMode.gameScriptRunner.L;
					Bindings.LuauPlayerList.Remove(player.ActorNumber);
					int num = 1;
					foreach (KeyValuePair<int, IntPtr> keyValuePair in Bindings.LuauPlayerList)
					{
						Bindings.LuauPlayer luauPlayer = *(Bindings.LuauPlayer*)((void*)keyValuePair.Value);
						*Luau.lua_class_push<Bindings.LuauPlayer>(l) = luauPlayer;
						Luau.lua_rawseti(l, -2, num++);
					}
					for (int i = num; i <= 10; i++)
					{
						Luau.lua_pushnil(CustomGameMode.gameScriptRunner.L);
						Luau.lua_rawseti(CustomGameMode.gameScriptRunner.L, -2, i);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.ToString());
		}
	}

	// Token: 0x06002CFF RID: 11519 RVA: 0x000DE54C File Offset: 0x000DC74C
	public unsafe override void OnMasterClientSwitched(NetPlayer newMasterClient)
	{
		try
		{
			if (CustomGameMode.gameScriptRunner != null)
			{
				if (CustomGameMode.gameScriptRunner.ShouldTick)
				{
					foreach (KeyValuePair<int, IntPtr> keyValuePair in Bindings.LuauPlayerList)
					{
						Bindings.LuauPlayer* ptr = (Bindings.LuauPlayer*)((void*)keyValuePair.Value);
						ptr->IsMasterClient = false;
					}
					IntPtr value;
					Bindings.LuauPlayerList.TryGetValue(newMasterClient.ActorNumber, out value);
					Bindings.LuauPlayer* ptr2 = (Bindings.LuauPlayer*)((void*)value);
					ptr2->IsMasterClient = true;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.ToString());
		}
	}

	// Token: 0x06002D00 RID: 11520 RVA: 0x000DE604 File Offset: 0x000DC804
	public override void StartPlaying()
	{
		base.StartPlaying();
		try
		{
			PhotonNetwork.AddCallbackTarget(this);
			CustomGameMode.GameModeInitialized = true;
			if (CustomGameMode.LuaScript != "")
			{
				CustomGameMode.LuaStart();
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.ToString());
		}
	}

	// Token: 0x06002D01 RID: 11521 RVA: 0x000DE658 File Offset: 0x000DC858
	public unsafe static void LuaStart()
	{
		if (CustomGameMode.LuaScript == "")
		{
			return;
		}
		CustomGameMode.RunGamemodeScript(CustomGameMode.LuaScript);
		if (CustomGameMode.gameScriptRunner.ShouldTick)
		{
			lua_State* l = CustomGameMode.gameScriptRunner.L;
			Bindings.LuauPlayerList.Clear();
			Luau.lua_getglobal(l, "Players");
			Player[] playerList = PhotonNetwork.PlayerList;
			for (int i = 0; i < playerList.Length; i++)
			{
				NetPlayer netPlayer = playerList[i];
				if (netPlayer != null)
				{
					Bindings.LuauPlayer* ptr = Luau.lua_class_push<Bindings.LuauPlayer>(l);
					ptr->PlayerID = netPlayer.ActorNumber;
					ptr->PlayerMaterial = 0;
					ptr->IsMasterClient = netPlayer.IsMasterClient;
					Bindings.LuauPlayerList[netPlayer.ActorNumber] = (IntPtr)((void*)ptr);
					RigContainer rigContainer;
					VRRigCache.Instance.TryGetVrrig(netPlayer, out rigContainer);
					VRRig rig = rigContainer.Rig;
					ptr->PlayerName = rig.playerNameVisible;
					Bindings.LuauVRRigList[netPlayer.ActorNumber] = rig;
					Bindings.PlayerFunctions.UpdatePlayer(l, rig, ptr);
					Luau.lua_rawseti(l, -2, i + 1);
					if (netPlayer.IsLocal)
					{
						Luau.lua_rawgeti(l, -1, i + 1);
						Luau.lua_setglobal(l, "LocalPlayer");
					}
				}
				else
				{
					Luau.lua_pushnil(l);
					Luau.lua_rawseti(l, -2, i + 1);
				}
			}
			for (int j = playerList.Length; j <= 10; j++)
			{
				Luau.lua_pushnil(l);
				Luau.lua_rawseti(l, -2, j + 1);
			}
			if (!Bindings.LuauPlayerList.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
			{
				NetPlayer netPlayer2 = PhotonNetwork.LocalPlayer;
				Bindings.LuauPlayer* ptr2 = Luau.lua_class_push<Bindings.LuauPlayer>(l);
				ptr2->PlayerID = netPlayer2.ActorNumber;
				ptr2->PlayerMaterial = 0;
				ptr2->IsMasterClient = netPlayer2.IsMasterClient;
				Bindings.LuauPlayerList[netPlayer2.ActorNumber] = (IntPtr)((void*)ptr2);
				RigContainer rigContainer2;
				VRRigCache.Instance.TryGetVrrig(netPlayer2, out rigContainer2);
				VRRig rig2 = rigContainer2.Rig;
				ptr2->PlayerName = rig2.playerNameVisible;
				Bindings.LuauVRRigList[netPlayer2.ActorNumber] = rig2;
				Bindings.PlayerFunctions.UpdatePlayer(l, rig2, ptr2);
				Luau.lua_setglobal(l, "LocalPlayer");
			}
		}
	}

	// Token: 0x06002D02 RID: 11522 RVA: 0x000DE87C File Offset: 0x000DCA7C
	public override void StopPlaying()
	{
		base.StopPlaying();
		try
		{
			CustomGameMode.GameModeInitialized = false;
			if (CustomGameMode.gameScriptRunner != null)
			{
				CustomGameMode.StopScript();
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.ToString());
		}
	}

	// Token: 0x06002D03 RID: 11523 RVA: 0x000DE8C0 File Offset: 0x000DCAC0
	public static void StopScript()
	{
		if (CustomGameMode.gameScriptRunner.ShouldTick)
		{
			Luau.lua_close(CustomGameMode.gameScriptRunner.L);
		}
		LuauScriptRunner.ScriptRunners.Remove(CustomGameMode.gameScriptRunner);
		CustomGameMode.gameScriptRunner.ShouldTick = false;
		CustomGameMode.gameScriptRunner = null;
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

	// Token: 0x06002D04 RID: 11524 RVA: 0x000DE95C File Offset: 0x000DCB5C
	public unsafe static void TouchPlayer(NetPlayer touchedPlayer)
	{
		if (CustomGameMode.gameScriptRunner == null)
		{
			return;
		}
		if (!CustomGameMode.gameScriptRunner.ShouldTick)
		{
			return;
		}
		lua_State* l = CustomGameMode.gameScriptRunner.L;
		Luau.lua_getfield(l, -10002, "onEvent");
		if (Luau.lua_type(l, -1) == 7)
		{
			Luau.lua_pushstring(l, "touchedPlayer");
			IntPtr value;
			if (Bindings.LuauPlayerList.TryGetValue(touchedPlayer.ActorNumber, out value))
			{
				Luau.lua_pushlightuserdata(l, (void*)value);
				Luau.luaL_getmetatable(l, "Player");
				Luau.lua_setmetatable(l, -2);
			}
			else
			{
				Bindings.LuauPlayer* ptr = Luau.lua_class_push<Bindings.LuauPlayer>(l);
				ptr->PlayerID = touchedPlayer.ActorNumber;
				ptr->PlayerMaterial = 0;
				ptr->IsMasterClient = touchedPlayer.IsMasterClient;
				RigContainer rigContainer;
				VRRigCache.Instance.TryGetVrrig(touchedPlayer, out rigContainer);
				VRRig rig = rigContainer.Rig;
				ptr->PlayerName = rig.playerNameVisible;
				Bindings.LuauVRRigList[touchedPlayer.ActorNumber] = rig;
				Bindings.PlayerFunctions.UpdatePlayer(l, rig, ptr);
				Bindings.LuauPlayerList[touchedPlayer.ActorNumber] = (IntPtr)((void*)ptr);
			}
			Luau.lua_pcall(l, 2, 0, 0);
			return;
		}
		Luau.lua_pop(l, 1);
	}

	// Token: 0x06002D05 RID: 11525 RVA: 0x000DEA7C File Offset: 0x000DCC7C
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int GameModeBindings(lua_State* L)
	{
		Bindings.GorillaLocomotionSettingsBuilder(L);
		Bindings.PlayerBuilder(L);
		Bindings.GameObjectBuilder(L);
		Luau.lua_createtable(L, 10, 0);
		Luau.lua_setglobal(L, "Players");
		Luau.lua_register(L, new lua_CFunction(Bindings.LuaEmit.Emit), "emitEvent");
		Luau.lua_register(L, new lua_CFunction(Bindings.LuaStartVibration), "startVibration");
		Luau.lua_register(L, new lua_CFunction(Bindings.LuaPlaySound), "playSound");
		return 0;
	}

	// Token: 0x06002D06 RID: 11526 RVA: 0x000DEAF8 File Offset: 0x000DCCF8
	public unsafe override float[] LocalPlayerSpeed()
	{
		if (Bindings.LocomotionSettings == null || CustomGameMode.gameScriptRunner == null || !CustomGameMode.gameScriptRunner.ShouldTick)
		{
			this.playerSpeed[0] = 6.5f;
			this.playerSpeed[1] = 1.1f;
		}
		else
		{
			this.playerSpeed[0] = Bindings.LocomotionSettings->maxJumpSpeed.ClampSafe(0f, 100f);
			this.playerSpeed[1] = Bindings.LocomotionSettings->jumpMultiplier.ClampSafe(0f, 100f);
		}
		return this.playerSpeed;
	}

	// Token: 0x06002D07 RID: 11527 RVA: 0x000DEB88 File Offset: 0x000DCD88
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int AfterTickGamemode(lua_State* L)
	{
		foreach (KeyValuePair<GameObject, IntPtr> keyValuePair in Bindings.LuauGameObjectList)
		{
			GameObject key = keyValuePair.Key;
			if (key.IsNotNull())
			{
				Transform transform = key.transform;
				Bindings.LuauGameObject* ptr = (Bindings.LuauGameObject*)((void*)keyValuePair.Value);
				transform.SetPositionAndRotation(ptr->Position, ptr->Rotation);
				transform.localScale = ptr->Scale;
			}
		}
		return 0;
	}

	// Token: 0x06002D08 RID: 11528 RVA: 0x000DEC14 File Offset: 0x000DCE14
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int PreTickGamemode(lua_State* L)
	{
		Luau.lua_pushboolean(L, (PhotonNetwork.InRoom && CustomGameMode.WasInRoom) ? 1 : 0);
		Luau.lua_setglobal(L, "InRoom");
		foreach (KeyValuePair<int, IntPtr> keyValuePair in Bindings.LuauPlayerList)
		{
			Bindings.LuauPlayer* ptr = (Bindings.LuauPlayer*)((void*)keyValuePair.Value);
			VRRig vrrig;
			Bindings.LuauVRRigList.TryGetValue(keyValuePair.Key, out vrrig);
			if (vrrig == null)
			{
				Debug.LogError("Unknown Rig for player");
			}
			else
			{
				if (keyValuePair.Key == PhotonNetwork.LocalPlayer.ActorNumber)
				{
					ptr->IsMasterClient = PhotonNetwork.LocalPlayer.IsMasterClient;
				}
				Bindings.PlayerFunctions.UpdatePlayer(L, vrrig, ptr);
			}
		}
		foreach (KeyValuePair<GameObject, IntPtr> keyValuePair2 in Bindings.LuauGameObjectList)
		{
			GameObject key = keyValuePair2.Key;
			if (key.IsNotNull())
			{
				Transform transform = key.transform;
				Bindings.LuauGameObject* ptr2 = (Bindings.LuauGameObject*)((void*)keyValuePair2.Value);
				Vector3 position = transform.position;
				position = new Vector3((float)Math.Round((double)position.x, 4), (float)Math.Round((double)position.y, 4), (float)Math.Round((double)position.z, 4));
				ptr2->Position = position;
				ptr2->Rotation = transform.rotation;
				ptr2->Scale = transform.localScale;
			}
		}
		CustomGameMode.WasInRoom = PhotonNetwork.InRoom;
		return 0;
	}

	// Token: 0x06002D09 RID: 11529 RVA: 0x000DEDBC File Offset: 0x000DCFBC
	private static void RunGamemodeScript(string script)
	{
		CustomGameMode.gameScriptRunner = new LuauScriptRunner(script, "GameMode", new lua_CFunction(CustomGameMode.GameModeBindings), new lua_CFunction(CustomGameMode.PreTickGamemode), new lua_CFunction(CustomGameMode.AfterTickGamemode));
	}

	// Token: 0x06002D0A RID: 11530 RVA: 0x000DEDF2 File Offset: 0x000DCFF2
	private static void RunGamemodeScriptFromFile(string filename)
	{
		CustomGameMode.RunGamemodeScript(File.ReadAllText(Path.Join(Application.persistentDataPath, "Scripts", filename)));
	}

	// Token: 0x04003272 RID: 12914
	public static LuauScriptRunner gameScriptRunner;

	// Token: 0x04003273 RID: 12915
	public static string LuaScript = "";

	// Token: 0x04003274 RID: 12916
	private static bool WasInRoom = false;

	// Token: 0x04003275 RID: 12917
	public static bool GameModeInitialized;
}
