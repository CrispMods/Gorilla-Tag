using System;
using System.Collections.Generic;
using Fusion;
using GorillaTag;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000289 RID: 649
[Serializable]
public abstract class NetPlayer : ObjectPoolEvents
{
	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06000F35 RID: 3893
	public abstract bool IsValid { get; }

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06000F36 RID: 3894
	public abstract int ActorNumber { get; }

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x06000F37 RID: 3895
	public abstract string UserId { get; }

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x06000F38 RID: 3896
	public abstract bool IsMasterClient { get; }

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06000F39 RID: 3897
	public abstract bool IsLocal { get; }

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06000F3A RID: 3898
	public abstract bool IsNull { get; }

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06000F3B RID: 3899
	public abstract string NickName { get; }

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06000F3C RID: 3900 RVA: 0x0003AAE1 File Offset: 0x00038CE1
	// (set) Token: 0x06000F3D RID: 3901 RVA: 0x0003AAE9 File Offset: 0x00038CE9
	public virtual string SanitizedNickName { get; set; } = string.Empty;

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06000F3E RID: 3902
	public abstract string DefaultName { get; }

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x06000F3F RID: 3903
	public abstract bool InRoom { get; }

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x06000F40 RID: 3904 RVA: 0x0003AAF2 File Offset: 0x00038CF2
	// (set) Token: 0x06000F41 RID: 3905 RVA: 0x0003AAFA File Offset: 0x00038CFA
	public virtual float JoinedTime { get; private set; }

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x06000F42 RID: 3906 RVA: 0x0003AB03 File Offset: 0x00038D03
	// (set) Token: 0x06000F43 RID: 3907 RVA: 0x0003AB0B File Offset: 0x00038D0B
	public virtual float LeftTime { get; private set; }

	// Token: 0x06000F44 RID: 3908
	public abstract bool Equals(NetPlayer myPlayer, NetPlayer other);

	// Token: 0x06000F45 RID: 3909 RVA: 0x0003AB14 File Offset: 0x00038D14
	public virtual void OnReturned()
	{
		this.LeftTime = Time.time;
		HashSet<int> singleCallRPCStatus = this.SingleCallRPCStatus;
		if (singleCallRPCStatus != null)
		{
			singleCallRPCStatus.Clear();
		}
		this.SanitizedNickName = string.Empty;
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x0003AB3D File Offset: 0x00038D3D
	public virtual void OnTaken()
	{
		this.JoinedTime = Time.time;
		HashSet<int> singleCallRPCStatus = this.SingleCallRPCStatus;
		if (singleCallRPCStatus == null)
		{
			return;
		}
		singleCallRPCStatus.Clear();
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x0003AB5A File Offset: 0x00038D5A
	public virtual bool CheckSingleCallRPC(NetPlayer.SingleCallRPC RPCType)
	{
		return this.SingleCallRPCStatus.Contains((int)RPCType);
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x0003AB68 File Offset: 0x00038D68
	public virtual void ReceivedSingleCallRPC(NetPlayer.SingleCallRPC RPCType)
	{
		this.SingleCallRPCStatus.Add((int)RPCType);
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x0003AB77 File Offset: 0x00038D77
	public Player GetPlayerRef()
	{
		return (this as PunNetPlayer).PlayerRef;
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x0003AB84 File Offset: 0x00038D84
	public string ToStringFull()
	{
		return string.Format("#{0: 0:00} '{1}', Not sure what to do with inactive yet, Or custom props?", this.ActorNumber, this.NickName);
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x0003ABA1 File Offset: 0x00038DA1
	public static implicit operator NetPlayer(Player player)
	{
		Utils.Log("Using an implicit cast from Player to NetPlayer. Please make sure this was intended as this has potential to cause errors when switching between network backends");
		NetworkSystem instance = NetworkSystem.Instance;
		return ((instance != null) ? instance.GetPlayer(player) : null) ?? null;
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x0003ABC4 File Offset: 0x00038DC4
	public static implicit operator NetPlayer(PlayerRef player)
	{
		Utils.Log("Using an implicit cast from PlayerRef to NetPlayer. Please make sure this was intended as this has potential to cause errors when switching between network backends");
		NetworkSystem instance = NetworkSystem.Instance;
		return ((instance != null) ? instance.GetPlayer(player) : null) ?? null;
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x0003ABE7 File Offset: 0x00038DE7
	public static NetPlayer Get(Player player)
	{
		NetworkSystem instance = NetworkSystem.Instance;
		return ((instance != null) ? instance.GetPlayer(player) : null) ?? null;
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x0003AC00 File Offset: 0x00038E00
	public static NetPlayer Get(PlayerRef player)
	{
		NetworkSystem instance = NetworkSystem.Instance;
		return ((instance != null) ? instance.GetPlayer(player) : null) ?? null;
	}

	// Token: 0x040011FA RID: 4602
	private HashSet<int> SingleCallRPCStatus = new HashSet<int>(5);

	// Token: 0x0200028A RID: 650
	public enum SingleCallRPC
	{
		// Token: 0x040011FC RID: 4604
		CMS_RequestRoomInitialization,
		// Token: 0x040011FD RID: 4605
		CMS_RequestTriggerHistory,
		// Token: 0x040011FE RID: 4606
		CMS_SyncTriggerHistory,
		// Token: 0x040011FF RID: 4607
		CMS_SyncTriggerCounts,
		// Token: 0x04001200 RID: 4608
		RequestQuestScore,
		// Token: 0x04001201 RID: 4609
		Count
	}
}
