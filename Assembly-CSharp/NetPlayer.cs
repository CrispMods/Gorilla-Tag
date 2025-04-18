﻿using System;
using System.Collections.Generic;
using Fusion;
using GorillaTag;
using Photon.Realtime;
using UnityEngine;

// Token: 0x0200027E RID: 638
[Serializable]
public abstract class NetPlayer : ObjectPoolEvents
{
	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06000EEA RID: 3818
	public abstract bool IsValid { get; }

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x06000EEB RID: 3819
	public abstract int ActorNumber { get; }

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x06000EEC RID: 3820
	public abstract string UserId { get; }

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x06000EED RID: 3821
	public abstract bool IsMasterClient { get; }

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x06000EEE RID: 3822
	public abstract bool IsLocal { get; }

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x06000EEF RID: 3823
	public abstract bool IsNull { get; }

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x06000EF0 RID: 3824
	public abstract string NickName { get; }

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0004BE32 File Offset: 0x0004A032
	// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x0004BE3A File Offset: 0x0004A03A
	public virtual string SanitizedNickName { get; set; } = string.Empty;

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06000EF3 RID: 3827
	public abstract string DefaultName { get; }

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x06000EF4 RID: 3828
	public abstract bool InRoom { get; }

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0004BE43 File Offset: 0x0004A043
	// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x0004BE4B File Offset: 0x0004A04B
	public virtual float JoinedTime { get; private set; }

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x0004BE54 File Offset: 0x0004A054
	// (set) Token: 0x06000EF8 RID: 3832 RVA: 0x0004BE5C File Offset: 0x0004A05C
	public virtual float LeftTime { get; private set; }

	// Token: 0x06000EF9 RID: 3833
	public abstract bool Equals(NetPlayer myPlayer, NetPlayer other);

	// Token: 0x06000EFA RID: 3834 RVA: 0x0004BE65 File Offset: 0x0004A065
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

	// Token: 0x06000EFB RID: 3835 RVA: 0x0004BE8E File Offset: 0x0004A08E
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

	// Token: 0x06000EFC RID: 3836 RVA: 0x0004BEAB File Offset: 0x0004A0AB
	public virtual bool CheckSingleCallRPC(NetPlayer.SingleCallRPC RPCType)
	{
		return this.SingleCallRPCStatus.Contains((int)RPCType);
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x0004BEB9 File Offset: 0x0004A0B9
	public virtual void ReceivedSingleCallRPC(NetPlayer.SingleCallRPC RPCType)
	{
		this.SingleCallRPCStatus.Add((int)RPCType);
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x0004BEC8 File Offset: 0x0004A0C8
	public Player GetPlayerRef()
	{
		return (this as PunNetPlayer).PlayerRef;
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x0004BED5 File Offset: 0x0004A0D5
	public string ToStringFull()
	{
		return string.Format("#{0: 0:00} '{1}', Not sure what to do with inactive yet, Or custom props?", this.ActorNumber, this.NickName);
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x0004BEF2 File Offset: 0x0004A0F2
	public static implicit operator NetPlayer(Player player)
	{
		Utils.Log("Using an implicit cast from Player to NetPlayer. Please make sure this was intended as this has potential to cause errors when switching between network backends");
		NetworkSystem instance = NetworkSystem.Instance;
		return ((instance != null) ? instance.GetPlayer(player) : null) ?? null;
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x0004BF15 File Offset: 0x0004A115
	public static implicit operator NetPlayer(PlayerRef player)
	{
		Utils.Log("Using an implicit cast from PlayerRef to NetPlayer. Please make sure this was intended as this has potential to cause errors when switching between network backends");
		NetworkSystem instance = NetworkSystem.Instance;
		return ((instance != null) ? instance.GetPlayer(player) : null) ?? null;
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x0004BF38 File Offset: 0x0004A138
	public static NetPlayer Get(Player player)
	{
		NetworkSystem instance = NetworkSystem.Instance;
		return ((instance != null) ? instance.GetPlayer(player) : null) ?? null;
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x0004BF51 File Offset: 0x0004A151
	public static NetPlayer Get(PlayerRef player)
	{
		NetworkSystem instance = NetworkSystem.Instance;
		return ((instance != null) ? instance.GetPlayer(player) : null) ?? null;
	}

	// Token: 0x040011B4 RID: 4532
	private HashSet<int> SingleCallRPCStatus = new HashSet<int>(3);

	// Token: 0x0200027F RID: 639
	public enum SingleCallRPC
	{
		// Token: 0x040011B6 RID: 4534
		CMS_RequestRoomInitialization,
		// Token: 0x040011B7 RID: 4535
		CMS_RequestTriggerHistory,
		// Token: 0x040011B8 RID: 4536
		CMS_RequestQuestScore,
		// Token: 0x040011B9 RID: 4537
		Count
	}
}
