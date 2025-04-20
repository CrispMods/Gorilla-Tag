using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020002B8 RID: 696
[Serializable]
public class PunNetPlayer : NetPlayer
{
	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x060010EC RID: 4332 RVA: 0x0003B8AC File Offset: 0x00039AAC
	// (set) Token: 0x060010ED RID: 4333 RVA: 0x0003B8B4 File Offset: 0x00039AB4
	public Player PlayerRef { get; private set; }

	// Token: 0x060010EF RID: 4335 RVA: 0x0003B8C5 File Offset: 0x00039AC5
	public void InitPlayer(Player playerRef)
	{
		this.PlayerRef = playerRef;
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x060010F0 RID: 4336 RVA: 0x0003B8CE File Offset: 0x00039ACE
	public override bool IsValid
	{
		get
		{
			return !this.PlayerRef.IsInactive;
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x060010F1 RID: 4337 RVA: 0x0003B8DE File Offset: 0x00039ADE
	public override int ActorNumber
	{
		get
		{
			Player playerRef = this.PlayerRef;
			if (playerRef == null)
			{
				return -1;
			}
			return playerRef.ActorNumber;
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x060010F2 RID: 4338 RVA: 0x0003B8F1 File Offset: 0x00039AF1
	public override string UserId
	{
		get
		{
			return this.PlayerRef.UserId;
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x060010F3 RID: 4339 RVA: 0x0003B8FE File Offset: 0x00039AFE
	public override bool IsMasterClient
	{
		get
		{
			return this.PlayerRef.IsMasterClient;
		}
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x060010F4 RID: 4340 RVA: 0x0003B90B File Offset: 0x00039B0B
	public override bool IsLocal
	{
		get
		{
			return this.PlayerRef == PhotonNetwork.LocalPlayer;
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x060010F5 RID: 4341 RVA: 0x0003B91A File Offset: 0x00039B1A
	public override bool IsNull
	{
		get
		{
			return this.PlayerRef == null;
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x060010F6 RID: 4342 RVA: 0x0003B925 File Offset: 0x00039B25
	public override string NickName
	{
		get
		{
			return this.PlayerRef.NickName;
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x060010F7 RID: 4343 RVA: 0x0003B932 File Offset: 0x00039B32
	public override string DefaultName
	{
		get
		{
			return this.PlayerRef.DefaultName;
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x060010F8 RID: 4344 RVA: 0x0003B93F File Offset: 0x00039B3F
	public override bool InRoom
	{
		get
		{
			Room currentRoom = PhotonNetwork.CurrentRoom;
			return currentRoom != null && currentRoom.Players.ContainsValue(this.PlayerRef);
		}
	}

	// Token: 0x060010F9 RID: 4345 RVA: 0x0003B95C File Offset: 0x00039B5C
	public override bool Equals(NetPlayer myPlayer, NetPlayer other)
	{
		return myPlayer != null && other != null && ((PunNetPlayer)myPlayer).PlayerRef.Equals(((PunNetPlayer)other).PlayerRef);
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x0003B981 File Offset: 0x00039B81
	public override void OnReturned()
	{
		base.OnReturned();
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x0003B989 File Offset: 0x00039B89
	public override void OnTaken()
	{
		base.OnTaken();
		this.PlayerRef = null;
	}
}
