using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020002AD RID: 685
[Serializable]
public class PunNetPlayer : NetPlayer
{
	// Token: 0x170001DA RID: 474
	// (get) Token: 0x060010A3 RID: 4259 RVA: 0x0003A5EC File Offset: 0x000387EC
	// (set) Token: 0x060010A4 RID: 4260 RVA: 0x0003A5F4 File Offset: 0x000387F4
	public Player PlayerRef { get; private set; }

	// Token: 0x060010A6 RID: 4262 RVA: 0x0003A605 File Offset: 0x00038805
	public void InitPlayer(Player playerRef)
	{
		this.PlayerRef = playerRef;
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x060010A7 RID: 4263 RVA: 0x0003A60E File Offset: 0x0003880E
	public override bool IsValid
	{
		get
		{
			return !this.PlayerRef.IsInactive;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x060010A8 RID: 4264 RVA: 0x0003A61E File Offset: 0x0003881E
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

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x060010A9 RID: 4265 RVA: 0x0003A631 File Offset: 0x00038831
	public override string UserId
	{
		get
		{
			return this.PlayerRef.UserId;
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x060010AA RID: 4266 RVA: 0x0003A63E File Offset: 0x0003883E
	public override bool IsMasterClient
	{
		get
		{
			return this.PlayerRef.IsMasterClient;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x060010AB RID: 4267 RVA: 0x0003A64B File Offset: 0x0003884B
	public override bool IsLocal
	{
		get
		{
			return this.PlayerRef == PhotonNetwork.LocalPlayer;
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x060010AC RID: 4268 RVA: 0x0003A65A File Offset: 0x0003885A
	public override bool IsNull
	{
		get
		{
			return this.PlayerRef == null;
		}
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x060010AD RID: 4269 RVA: 0x0003A665 File Offset: 0x00038865
	public override string NickName
	{
		get
		{
			return this.PlayerRef.NickName;
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x060010AE RID: 4270 RVA: 0x0003A672 File Offset: 0x00038872
	public override string DefaultName
	{
		get
		{
			return this.PlayerRef.DefaultName;
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x060010AF RID: 4271 RVA: 0x0003A67F File Offset: 0x0003887F
	public override bool InRoom
	{
		get
		{
			Room currentRoom = PhotonNetwork.CurrentRoom;
			return currentRoom != null && currentRoom.Players.ContainsValue(this.PlayerRef);
		}
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x0003A69C File Offset: 0x0003889C
	public override bool Equals(NetPlayer myPlayer, NetPlayer other)
	{
		return myPlayer != null && other != null && ((PunNetPlayer)myPlayer).PlayerRef.Equals(((PunNetPlayer)other).PlayerRef);
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x0003A6C1 File Offset: 0x000388C1
	public override void OnReturned()
	{
		base.OnReturned();
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x0003A6C9 File Offset: 0x000388C9
	public override void OnTaken()
	{
		base.OnTaken();
		this.PlayerRef = null;
	}
}
