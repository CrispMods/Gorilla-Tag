using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorillaTag;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000897 RID: 2199
public static class Utils
{
	// Token: 0x06003528 RID: 13608 RVA: 0x000FD5EC File Offset: 0x000FB7EC
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

	// Token: 0x06003529 RID: 13609 RVA: 0x000FD664 File Offset: 0x000FB864
	public static void AddIfNew<T>(this List<T> list, T item)
	{
		if (!list.Contains(item))
		{
			list.Add(item);
		}
	}

	// Token: 0x0600352A RID: 13610 RVA: 0x000FD676 File Offset: 0x000FB876
	public static bool InRoom(this NetPlayer player)
	{
		return NetworkSystem.Instance.InRoom && NetworkSystem.Instance.AllNetPlayers.Contains(player);
	}

	// Token: 0x0600352B RID: 13611 RVA: 0x000FD698 File Offset: 0x000FB898
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

	// Token: 0x0600352C RID: 13612 RVA: 0x000FD6D8 File Offset: 0x000FB8D8
	public static bool PlayerInRoom(int actorNumer, out Player photonPlayer)
	{
		photonPlayer = null;
		return PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.Players.TryGetValue(actorNumer, out photonPlayer);
	}

	// Token: 0x0600352D RID: 13613 RVA: 0x000FD6F7 File Offset: 0x000FB8F7
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

	// Token: 0x0600352E RID: 13614 RVA: 0x000FD72C File Offset: 0x000FB92C
	public static long PackVector3ToLong(Vector3 vector)
	{
		long num = (long)Mathf.Clamp(Mathf.RoundToInt(vector.x * 1024f) + 1048576, 0, 2097151);
		long num2 = (long)Mathf.Clamp(Mathf.RoundToInt(vector.y * 1024f) + 1048576, 0, 2097151);
		long num3 = (long)Mathf.Clamp(Mathf.RoundToInt(vector.z * 1024f) + 1048576, 0, 2097151);
		return num + (num2 << 21) + (num3 << 42);
	}

	// Token: 0x0600352F RID: 13615 RVA: 0x000FD7B0 File Offset: 0x000FB9B0
	public static Vector3 UnpackVector3FromLong(long data)
	{
		float num = (float)(data & 2097151L);
		long num2 = data >> 21 & 2097151L;
		long num3 = data >> 42 & 2097151L;
		return new Vector3((float)((long)num - 1048576L) * 0.0009765625f, (float)(num2 - 1048576L) * 0.0009765625f, (float)(num3 - 1048576L) * 0.0009765625f);
	}

	// Token: 0x06003530 RID: 13616 RVA: 0x000FD80E File Offset: 0x000FBA0E
	public static bool IsASCIILetterOrDigit(char c)
	{
		return (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || (c >= 'a' && c <= 'z');
	}

	// Token: 0x06003531 RID: 13617 RVA: 0x000023F4 File Offset: 0x000005F4
	public static void Log(object message)
	{
	}

	// Token: 0x06003532 RID: 13618 RVA: 0x000023F4 File Offset: 0x000005F4
	public static void Log(object message, Object context)
	{
	}

	// Token: 0x040037AD RID: 14253
	private static ObjectPool<PooledList<IPreDisable>> g_listPool = new ObjectPool<PooledList<IPreDisable>>(2);

	// Token: 0x040037AE RID: 14254
	private static StringBuilder reusableSB = new StringBuilder();
}
