using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorillaTag;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020008B3 RID: 2227
public static class Utils
{
	// Token: 0x060035F4 RID: 13812 RVA: 0x00143D44 File Offset: 0x00141F44
	public static void Disable(this GameObject target)
	{
		if (!target.activeSelf)
		{
			return;
		}
		PooledList<IPreDisable> pooledList = Utils.g_listPool.Take();
		List<IPreDisable> list = pooledList.List;
		target.GetComponents<IPreDisable>(list);
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			try
			{
				list[i].PreDisable();
			}
			catch (Exception)
			{
			}
		}
		target.SetActive(false);
		Utils.g_listPool.Return(pooledList);
	}

	// Token: 0x060035F5 RID: 13813 RVA: 0x000536EE File Offset: 0x000518EE
	public static void AddIfNew<T>(this List<T> list, T item)
	{
		if (!list.Contains(item))
		{
			list.Add(item);
		}
	}

	// Token: 0x060035F6 RID: 13814 RVA: 0x00053700 File Offset: 0x00051900
	public static bool InRoom(this NetPlayer player)
	{
		return NetworkSystem.Instance.InRoom && NetworkSystem.Instance.AllNetPlayers.Contains(player);
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x00143DBC File Offset: 0x00141FBC
	public static bool PlayerInRoom(int actorNumber)
	{
		if (NetworkSystem.Instance.InRoom)
		{
			NetPlayer[] allNetPlayers = NetworkSystem.Instance.AllNetPlayers;
			for (int i = 0; i < allNetPlayers.Length; i++)
			{
				if (allNetPlayers[i].ActorNumber == actorNumber)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060035F8 RID: 13816 RVA: 0x00053720 File Offset: 0x00051920
	public static bool PlayerInRoom(int actorNumer, out Player photonPlayer)
	{
		photonPlayer = null;
		return PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.Players.TryGetValue(actorNumer, out photonPlayer);
	}

	// Token: 0x060035F9 RID: 13817 RVA: 0x0005373F File Offset: 0x0005193F
	public static bool PlayerInRoom(int actorNumber, out NetPlayer player)
	{
		if (NetworkSystem.Instance == null)
		{
			player = null;
			return false;
		}
		player = NetworkSystem.Instance.GetPlayer(actorNumber);
		return NetworkSystem.Instance.InRoom && player != null;
	}

	// Token: 0x060035FA RID: 13818 RVA: 0x0013CF18 File Offset: 0x0013B118
	public static long PackVector3ToLong(Vector3 vector)
	{
		long num = (long)Mathf.Clamp(Mathf.RoundToInt(vector.x * 1024f) + 1048576, 0, 2097151);
		long num2 = (long)Mathf.Clamp(Mathf.RoundToInt(vector.y * 1024f) + 1048576, 0, 2097151);
		long num3 = (long)Mathf.Clamp(Mathf.RoundToInt(vector.z * 1024f) + 1048576, 0, 2097151);
		return num + (num2 << 21) + (num3 << 42);
	}

	// Token: 0x060035FB RID: 13819 RVA: 0x0013CF9C File Offset: 0x0013B19C
	public static Vector3 UnpackVector3FromLong(long data)
	{
		float num = (float)(data & 2097151L);
		long num2 = data >> 21 & 2097151L;
		long num3 = data >> 42 & 2097151L;
		return new Vector3((float)((long)num - 1048576L) * 0.0009765625f, (float)(num2 - 1048576L) * 0.0009765625f, (float)(num3 - 1048576L) * 0.0009765625f);
	}

	// Token: 0x060035FC RID: 13820 RVA: 0x00053773 File Offset: 0x00051973
	public static bool IsASCIILetterOrDigit(char c)
	{
		return (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || (c >= 'a' && c <= 'z');
	}

	// Token: 0x060035FD RID: 13821 RVA: 0x00030607 File Offset: 0x0002E807
	public static void Log(object message)
	{
	}

	// Token: 0x060035FE RID: 13822 RVA: 0x00030607 File Offset: 0x0002E807
	public static void Log(object message, UnityEngine.Object context)
	{
	}

	// Token: 0x060035FF RID: 13823 RVA: 0x00143DFC File Offset: 0x00141FFC
	public static bool ValidateServerTime(double time, double maximumLatency)
	{
		double simTime = NetworkSystem.Instance.SimTime;
		double num = 4294967.295 - maximumLatency;
		double num2;
		if (simTime > maximumLatency || time < maximumLatency)
		{
			if (time > simTime)
			{
				return false;
			}
			num2 = simTime - time;
		}
		else
		{
			double num3 = num + simTime;
			if (time > simTime && time < num3)
			{
				return false;
			}
			num2 = simTime + (4294967.295 - time);
		}
		return num2 <= maximumLatency;
	}

	// Token: 0x0400386D RID: 14445
	private static ObjectPool<PooledList<IPreDisable>> g_listPool = new ObjectPool<PooledList<IPreDisable>>(2, 10);

	// Token: 0x0400386E RID: 14446
	private static StringBuilder reusableSB = new StringBuilder();
}
