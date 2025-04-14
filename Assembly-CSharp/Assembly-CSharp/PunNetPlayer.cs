using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020002AD RID: 685
[Serializable]
public class PunNetPlayer : NetPlayer
{
	// Token: 0x170001DA RID: 474
	// (get) Token: 0x060010A3 RID: 4259 RVA: 0x000510D3 File Offset: 0x0004F2D3
	// (set) Token: 0x060010A4 RID: 4260 RVA: 0x000510DB File Offset: 0x0004F2DB
	public Player PlayerRef { get; private set; }

	// Token: 0x060010A6 RID: 4262 RVA: 0x000510EC File Offset: 0x0004F2EC
	public void InitPlayer(Player playerRef)
	{
		this.PlayerRef = playerRef;
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x060010A7 RID: 4263 RVA: 0x000510F5 File Offset: 0x0004F2F5
	public override bool IsValid
	{
		get
		{
			return !this.PlayerRef.IsInactive;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x060010A8 RID: 4264 RVA: 0x00051105 File Offset: 0x0004F305
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
	// (get) Token: 0x060010A9 RID: 4265 RVA: 0x00051118 File Offset: 0x0004F318
	public override string UserId
	{
		get
		{
			return this.PlayerRef.UserId;
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x060010AA RID: 4266 RVA: 0x00051125 File Offset: 0x0004F325
	public override bool IsMasterClient
	{
		get
		{
			return this.PlayerRef.IsMasterClient;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x060010AB RID: 4267 RVA: 0x00051132 File Offset: 0x0004F332
	public override bool IsLocal
	{
		get
		{
			return this.PlayerRef == PhotonNetwork.LocalPlayer;
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x060010AC RID: 4268 RVA: 0x00051141 File Offset: 0x0004F341
	public override bool IsNull
	{
		get
		{
			return this.PlayerRef == null;
		}
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x060010AD RID: 4269 RVA: 0x0005114C File Offset: 0x0004F34C
	public override string NickName
	{
		get
		{
			return this.PlayerRef.NickName;
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x060010AE RID: 4270 RVA: 0x00051159 File Offset: 0x0004F359
	public override string DefaultName
	{
		get
		{
			return this.PlayerRef.DefaultName;
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x060010AF RID: 4271 RVA: 0x00051166 File Offset: 0x0004F366
	public override bool InRoom
	{
		get
		{
			Room currentRoom = PhotonNetwork.CurrentRoom;
			return currentRoom != null && currentRoom.Players.ContainsValue(this.PlayerRef);
		}
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x00051183 File Offset: 0x0004F383
	public override bool Equals(NetPlayer myPlayer, NetPlayer other)
	{
		return myPlayer != null && other != null && ((PunNetPlayer)myPlayer).PlayerRef.Equals(((PunNetPlayer)other).PlayerRef);
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x000511A8 File Offset: 0x0004F3A8
	public override void OnReturned()
	{
		base.OnReturned();
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x000511B0 File Offset: 0x0004F3B0
	public override void OnTaken()
	{
		base.OnTaken();
		this.PlayerRef = null;
	}
}
