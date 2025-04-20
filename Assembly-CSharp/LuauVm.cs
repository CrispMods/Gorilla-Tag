using System;
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

// Token: 0x0200078F RID: 1935
public class LuauVm : MonoBehaviourPunCallbacks, IOnEventCallback
{
	// Token: 0x06002F92 RID: 12178 RVA: 0x0012C570 File Offset: 0x0012A770
	private void Update()
	{
		foreach (LuauScriptRunner luauScriptRunner in LuauScriptRunner.ScriptRunners)
		{
			if (!luauScriptRunner.Tick(Time.deltaTime))
			{
				LuauHud.Instance.LuauLog(luauScriptRunner.ScriptName + " errored out");
				LuauScriptRunner.ScriptRunners.Remove(luauScriptRunner);
				break;
			}
		}
	}

	// Token: 0x06002F93 RID: 12179 RVA: 0x00030607 File Offset: 0x0002E807
	private void Start()
	{
	}

	// Token: 0x06002F94 RID: 12180 RVA: 0x00030607 File Offset: 0x0002E807
	private void Awake()
	{
	}

	// Token: 0x06002F95 RID: 12181 RVA: 0x0012C5F0 File Offset: 0x0012A7F0
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
								if (!luauScriptRunner.ShouldTick)
								{
									return;
								}
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
								else if (obj.IsType<Player>())
								{
									int actorNumber = ((Player)obj).ActorNumber;
									IntPtr ptr;
									if (Bindings.LuauPlayerList.TryGetValue(actorNumber, out ptr))
									{
										Luau.lua_class_push(l, "Player", ptr);
										Luau.lua_rawseti(l, -2, i);
									}
									else
									{
										NetPlayer netPlayer = (NetPlayer)obj;
										if (netPlayer == null)
										{
											Luau.lua_pushnil(l);
											Luau.lua_rawseti(l, -2, i);
										}
										else
										{
											Bindings.LuauPlayer* ptr2 = Luau.lua_class_push<Bindings.LuauPlayer>(l);
											ptr2->PlayerID = netPlayer.ActorNumber;
											ptr2->PlayerName = netPlayer.SanitizedNickName;
											ptr2->PlayerMaterial = 0;
											ptr2->IsMasterClient = netPlayer.IsMasterClient;
											RigContainer rigContainer;
											VRRigCache.Instance.TryGetVrrig(netPlayer, out rigContainer);
											VRRig rig = rigContainer.Rig;
											Bindings.LuauVRRigList[netPlayer.ActorNumber] = rig;
											Bindings.PlayerFunctions.UpdatePlayer(l, rig, ptr2);
											Bindings.LuauPlayerList[netPlayer.ActorNumber] = (IntPtr)((void*)ptr2);
											Luau.lua_rawseti(l, -2, i);
										}
									}
								}
								else if (obj == null)
								{
									Luau.lua_pushnil(l);
									Luau.lua_rawseti(l, -2, i);
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

	// Token: 0x06002F96 RID: 12182 RVA: 0x0012CA10 File Offset: 0x0012AC10
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

	// Token: 0x040033BB RID: 13243
	public static List<object> ClassBuilders = new List<object>();

	// Token: 0x040033BC RID: 13244
	public static List<GCHandle> Handles = new List<GCHandle>();

	// Token: 0x040033BD RID: 13245
	private static Dictionary<int, float> callTimers = new Dictionary<int, float>();

	// Token: 0x040033BE RID: 13246
	private static float callCount = 25f;
}
