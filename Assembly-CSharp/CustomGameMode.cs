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

// Token: 0x02000731 RID: 1841
public class CustomGameMode : GorillaGameManager
{
	// Token: 0x06002D8B RID: 11659 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06002D8C RID: 11660 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnSerializeRead(object obj)
	{
	}

	// Token: 0x06002D8D RID: 11661 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06002D8E RID: 11662 RVA: 0x0003924B File Offset: 0x0003744B
	public override object OnSerializeWrite()
	{
		return null;
	}

	// Token: 0x06002D8F RID: 11663 RVA: 0x00030607 File Offset: 0x0002E807
	public override void AddFusionDataBehaviour(NetworkObject obj)
	{
	}

	// Token: 0x06002D90 RID: 11664 RVA: 0x0004EED8 File Offset: 0x0004D0D8
	public override GameModeType GameType()
	{
		return GameModeType.Custom;
	}

	// Token: 0x06002D91 RID: 11665 RVA: 0x0004EEDB File Offset: 0x0004D0DB
	public override string GameModeName()
	{
		return "CUSTOM";
	}

	// Token: 0x06002D92 RID: 11666 RVA: 0x001287B4 File Offset: 0x001269B4
	public unsafe override int MyMatIndex(NetPlayer forPlayer)
	{
		IntPtr value;
		if (Bindings.LuauPlayerList.TryGetValue(forPlayer.ActorNumber, out value))
		{
			return ((Bindings.LuauPlayer*)((void*)value))->PlayerMaterial;
		}
		return 0;
	}

	// Token: 0x06002D93 RID: 11667 RVA: 0x001287E4 File Offset: 0x001269E4
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
						ptr->PlayerName = vrrig.playerNameVisible;
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

	// Token: 0x06002D94 RID: 11668 RVA: 0x00128920 File Offset: 0x00126B20
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
					Luau.lua_getglobal(l, "Players");
					int num = Luau.lua_objlen(l, -1);
					for (int i = 1; i <= num; i++)
					{
						Luau.lua_rawgeti(l, -1, i);
						Bindings.LuauPlayer* ptr = (Bindings.LuauPlayer*)Luau.lua_touserdata(l, -1);
						Luau.lua_pop(l, 1);
						if (ptr != null && ptr->PlayerID == player.ActorNumber)
						{
							for (int j = i; j < num; j++)
							{
								Luau.lua_rawgeti(l, -1, j + 1);
								Luau.lua_rawseti(l, -2, j);
							}
							Luau.lua_pushnil(l);
							Luau.lua_rawseti(l, -2, num);
							break;
						}
					}
					Luau.lua_pop(l, 1);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.ToString());
		}
	}

	// Token: 0x06002D95 RID: 11669 RVA: 0x00128A0C File Offset: 0x00126C0C
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

	// Token: 0x06002D96 RID: 11670 RVA: 0x00128AC4 File Offset: 0x00126CC4
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

	// Token: 0x06002D97 RID: 11671 RVA: 0x00128B18 File Offset: 0x00126D18
	public unsafe static void LuaStart()
	{
		if (CustomGameMode.LuaScript == "")
		{
			return;
		}
		CustomGameMode.RunGamemodeScript(CustomGameMode.LuaScript);
		if (CustomGameMode.gameScriptRunner.ShouldTick)
		{
			CustomGameMode.GameModeInitialized = true;
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
					ptr->PlayerName = rig.playerNameVisible;
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
		}
	}

	// Token: 0x06002D98 RID: 11672 RVA: 0x00128C9C File Offset: 0x00126E9C
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

	// Token: 0x06002D99 RID: 11673 RVA: 0x00128CE0 File Offset: 0x00126EE0
	public static void StopScript()
	{
		CustomGameMode.GameModeInitialized = false;
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

	// Token: 0x06002D9A RID: 11674 RVA: 0x00128D84 File Offset: 0x00126F84
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
			IntPtr ptr;
			if (Bindings.LuauPlayerList.TryGetValue(touchedPlayer.ActorNumber, out ptr))
			{
				Luau.lua_pushstring(l, "touchedPlayer");
				Luau.lua_class_push(l, "Player", ptr);
				Luau.lua_pcall(l, 2, 0, 0);
				return;
			}
		}
		else
		{
			Luau.lua_pop(l, 1);
		}
	}

	// Token: 0x06002D9B RID: 11675 RVA: 0x00128E10 File Offset: 0x00127010
	public unsafe static void TaggedByEnvironment()
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
			Luau.lua_pushstring(l, "taggedByEnvironment");
			Luau.lua_pushnil(l);
			Luau.lua_pcall(l, 2, 0, 0);
			return;
		}
		Luau.lua_pop(l, 1);
	}

	// Token: 0x06002D9C RID: 11676 RVA: 0x00128E7C File Offset: 0x0012707C
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

	// Token: 0x06002D9D RID: 11677 RVA: 0x00128EF8 File Offset: 0x001270F8
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

	// Token: 0x06002D9E RID: 11678 RVA: 0x00128F88 File Offset: 0x00127188
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int AfterTickGamemode(lua_State* L)
	{
		try
		{
			foreach (KeyValuePair<GameObject, IntPtr> keyValuePair in Bindings.LuauGameObjectList)
			{
				GameObject key = keyValuePair.Key;
				if (key.IsNotNull())
				{
					Transform transform = key.transform;
					Bindings.LuauGameObject* ptr = (Bindings.LuauGameObject*)((void*)keyValuePair.Value);
					Vector3 position = ptr->Position;
					position = new Vector3((float)Math.Round((double)position.x, 4), (float)Math.Round((double)position.y, 4), (float)Math.Round((double)position.z, 4));
					transform.SetPositionAndRotation(position, ptr->Rotation);
					transform.localScale = ptr->Scale;
				}
			}
		}
		catch (Exception)
		{
		}
		return 0;
	}

	// Token: 0x06002D9F RID: 11679 RVA: 0x00129064 File Offset: 0x00127264
	[MonoPInvokeCallback(typeof(lua_CFunction))]
	public unsafe static int PreTickGamemode(lua_State* L)
	{
		try
		{
			Luau.lua_pushboolean(L, (PhotonNetwork.InRoom && CustomGameMode.WasInRoom) ? 1 : 0);
			Luau.lua_setglobal(L, "InRoom");
			foreach (KeyValuePair<int, IntPtr> keyValuePair in Bindings.LuauPlayerList)
			{
				Bindings.LuauPlayer* ptr = (Bindings.LuauPlayer*)((void*)keyValuePair.Value);
				VRRig vrrig;
				Bindings.LuauVRRigList.TryGetValue(keyValuePair.Key, out vrrig);
				if (!vrrig.IsNotNull())
				{
					LuauHud.Instance.LuauLog("Unknown Rig for player");
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
		}
		catch (Exception)
		{
		}
		return 0;
	}

	// Token: 0x06002DA0 RID: 11680 RVA: 0x0004EEE2 File Offset: 0x0004D0E2
	private static void RunGamemodeScript(string script)
	{
		CustomGameMode.gameScriptRunner = new LuauScriptRunner(script, "GameMode", new lua_CFunction(CustomGameMode.GameModeBindings), new lua_CFunction(CustomGameMode.PreTickGamemode), new lua_CFunction(CustomGameMode.AfterTickGamemode));
	}

	// Token: 0x06002DA1 RID: 11681 RVA: 0x0004EF18 File Offset: 0x0004D118
	private static void RunGamemodeScriptFromFile(string filename)
	{
		CustomGameMode.RunGamemodeScript(File.ReadAllText(Path.Join(Application.persistentDataPath, "Scripts", filename)));
	}

	// Token: 0x0400330F RID: 13071
	public static LuauScriptRunner gameScriptRunner;

	// Token: 0x04003310 RID: 13072
	public static string LuaScript = "";

	// Token: 0x04003311 RID: 13073
	private static bool WasInRoom = false;

	// Token: 0x04003312 RID: 13074
	public static bool GameModeInitialized;
}
