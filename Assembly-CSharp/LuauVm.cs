﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using GorillaExtensions;
using GorillaGameModes;
using Photon.Pun;
using Photon.Realtime;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000777 RID: 1911
public class LuauVm : MonoBehaviourPunCallbacks, IOnEventCallback
{
	// Token: 0x06002EE0 RID: 12000 RVA: 0x000E25CC File Offset: 0x000E07CC
	private void Update()
	{
		foreach (LuauScriptRunner luauScriptRunner in LuauScriptRunner.ScriptRunners)
		{
			if (!luauScriptRunner.Tick(Time.deltaTime))
			{
				Debug.LogWarning(luauScriptRunner.ScriptName + " errored out");
				LuauScriptRunner.ScriptRunners.Remove(luauScriptRunner);
				break;
			}
		}
	}

	// Token: 0x06002EE1 RID: 12001 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Start()
	{
	}

	// Token: 0x06002EE2 RID: 12002 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Awake()
	{
	}

	// Token: 0x06002EE3 RID: 12003 RVA: 0x000E2648 File Offset: 0x000E0848
	public unsafe void OnEvent(EventData eventData)
	{
		if (eventData.Code != 180)
		{
			return;
		}
		float num = 0f;
		LuauVm.callTimers.TryGetValue(eventData.Sender, out num);
		if (num < Time.time - 1f)
		{
			num = Time.time - 1f;
		}
		num += 1f / LuauVm.callCount;
		LuauVm.callTimers[eventData.Sender] = num;
		if (num > Time.time)
		{
			return;
		}
		try
		{
			if (GorillaGameManager.instance.GameType() == GameModeType.Custom)
			{
				foreach (LuauScriptRunner luauScriptRunner in LuauScriptRunner.ScriptRunners)
				{
					if (luauScriptRunner.ShouldTick)
					{
						lua_State* l = luauScriptRunner.L;
						Luau.lua_getfield(l, -10002, "onEvent");
						if (Luau.lua_type(l, -1) == 7)
						{
							object[] array = (object[])eventData.CustomData;
							if (array.Length > 20)
							{
								Luau.lua_pop(l, 1);
								break;
							}
							string text = array[0] as string;
							if (text == null)
							{
								Luau.lua_pop(l, 1);
								break;
							}
							if (string.IsNullOrEmpty(text))
							{
								Luau.lua_pop(l, 1);
								break;
							}
							if (text.Length > 30)
							{
								Luau.lua_pop(l, 1);
								break;
							}
							Luau.lua_pushstring(l, (string)array[0]);
							Luau.lua_createtable(l, array.Length, 0);
							for (int i = 1; i < array.Length; i++)
							{
								object obj = array[i];
								if (obj.IsType<double>())
								{
									if (double.IsFinite((double)obj))
									{
										Luau.lua_pushnumber(l, (double)obj);
										Luau.lua_rawseti(l, -2, i);
									}
								}
								else if (obj.IsType<bool>())
								{
									Luau.lua_pushboolean(l, (int)obj);
									Luau.lua_rawseti(l, -2, i);
								}
								else if (obj.IsType<Vector3>())
								{
									Vector3 vector = (Vector3)obj;
									vector.ClampMagnitudeSafe(10000000f);
									*Luau.lua_class_push<Vector3>(l, "Vec3") = vector;
									Luau.lua_rawseti(l, -2, i);
								}
								else if (obj.IsType<Quaternion>())
								{
									Quaternion quaternion = (Quaternion)obj;
									if (float.IsFinite(quaternion.x) && float.IsFinite(quaternion.y) && float.IsFinite(quaternion.z) && float.IsFinite(quaternion.w))
									{
										*Luau.lua_class_push<Quaternion>(l, "Quat") = quaternion;
										Luau.lua_rawseti(l, -2, i);
									}
								}
								else if (obj.IsType<int>())
								{
									IntPtr value;
									if (Bindings.LuauPlayerList.TryGetValue((int)obj, out value))
									{
										Luau.lua_pushlightuserdata(l, (void*)value);
										Luau.luaL_getmetatable(l, "Player");
										Luau.lua_setmetatable(l, -2);
										Luau.lua_rawseti(l, -2, i);
									}
									else
									{
										NetPlayer netPlayer = null;
										foreach (NetPlayer netPlayer2 in RoomSystem.PlayersInRoom)
										{
											if (netPlayer2.ActorNumber == (int)obj)
											{
												netPlayer = netPlayer2;
												break;
											}
										}
										if (netPlayer == null)
										{
											Luau.lua_pushnil(l);
											Luau.lua_rawseti(l, -2, i);
										}
										else
										{
											Bindings.LuauPlayer* ptr = Luau.lua_class_push<Bindings.LuauPlayer>(l);
											ptr->PlayerID = netPlayer.ActorNumber;
											ptr->PlayerName = netPlayer.SanitizedNickName;
											ptr->PlayerMaterial = 0;
											ptr->IsMasterClient = netPlayer.IsMasterClient;
											RigContainer rigContainer;
											VRRigCache.Instance.TryGetVrrig(netPlayer, out rigContainer);
											VRRig rig = rigContainer.Rig;
											Bindings.LuauVRRigList[netPlayer.ActorNumber] = rig;
											Bindings.PlayerFunctions.UpdatePlayer(l, rig, ptr);
											Bindings.LuauPlayerList[netPlayer.ActorNumber] = (IntPtr)((void*)ptr);
											Luau.lua_rawseti(l, -2, i);
										}
									}
								}
							}
							Luau.lua_pcall(l, 2, 0, 0);
						}
						else
						{
							Luau.lua_pop(l, 1);
						}
					}
				}
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x06002EE4 RID: 12004 RVA: 0x000E2AA4 File Offset: 0x000E0CA4
	protected override void Finalize()
	{
		try
		{
			foreach (GCHandle gchandle in LuauVm.Handles)
			{
				gchandle.Free();
			}
			if (BurstClassInfo.ClassList.InfoFields.Data.IsCreated)
			{
				foreach (KVPair<int, BurstClassInfo.ClassInfo> kvpair in BurstClassInfo.ClassList.InfoFields.Data)
				{
					if (kvpair.Value.FieldList.IsCreated)
					{
						kvpair.Value.FieldList.Dispose();
					}
				}
				BurstClassInfo.ClassList.InfoFields.Data.Dispose();
			}
		}
		catch (ObjectDisposedException message)
		{
			Debug.Log(message);
		}
		finally
		{
			base.Finalize();
		}
	}

	// Token: 0x04003311 RID: 13073
	public static List<object> ClassBuilders = new List<object>();

	// Token: 0x04003312 RID: 13074
	public static List<GCHandle> Handles = new List<GCHandle>();

	// Token: 0x04003313 RID: 13075
	private static Dictionary<int, float> callTimers = new Dictionary<int, float>();

	// Token: 0x04003314 RID: 13076
	private static float callCount = 25f;
}
