using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020002AD RID: 685
[Serializable]
public class PunNetPlayer : NetPlayer
{
	// Token: 0x170001DA RID: 474
	// (get) Token: 0x060010A0 RID: 4256 RVA: 0x00050D4F File Offset: 0x0004EF4F
	// (set) Token: 0x060010A1 RID: 4257 RVA: 0x00050D57 File Offset: 0x0004EF57
	public Player PlayerRef { get; private set; }

	// Token: 0x060010A3 RID: 4259 RVA: 0x00050D68 File Offset: 0x0004EF68
	public void InitPlayer(Player playerRef)
	{
		this.PlayerRef = playerRef;
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x060010A4 RID: 4260 RVA: 0x00050D71 File Offset: 0x0004EF71
	public override bool IsValid
	{
		get
		{
			return !this.PlayerRef.IsInactive;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x060010A5 RID: 4261 RVA: 0x00050D81 File Offset: 0x0004EF81
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
	// (get) Token: 0x060010A6 RID: 4262 RVA: 0x00050D94 File Offset: 0x0004EF94
	public override string UserId
	{
		get
		{
			return this.PlayerRef.UserId;
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x060010A7 RID: 4263 RVA: 0x00050DA1 File Offset: 0x0004EFA1
	public override bool IsMasterClient
	{
		get
		{
			return this.PlayerRef.IsMasterClient;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x060010A8 RID: 4264 RVA: 0x00050DAE File Offset: 0x0004EFAE
	public override bool IsLocal
	{
		get
		{
			return this.PlayerRef == PhotonNetwork.LocalPlayer;
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x060010A9 RID: 4265 RVA: 0x00050DBD File Offset: 0x0004EFBD
	public override bool IsNull
	{
		get
		{
			return this.PlayerRef == null;
		}
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x060010AA RID: 4266 RVA: 0x00050DC8 File Offset: 0x0004EFC8
	public override string NickName
	{
		get
		{
			return this.PlayerRef.NickName;
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x060010AB RID: 4267 RVA: 0x00050DD5 File Offset: 0x0004EFD5
	public override string DefaultName
	{
		get
		{
			return this.PlayerRef.DefaultName;
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x060010AC RID: 4268 RVA: 0x00050DE2 File Offset: 0x0004EFE2
	public override bool InRoom
	{
		get
		{
			Room currentRoom = PhotonNetwork.CurrentRoom;
			return currentRoom != null && currentRoom.Players.ContainsValue(this.PlayerRef);
		}
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x00050DFF File Offset: 0x0004EFFF
	public override bool Equals(NetPlayer myPlayer, NetPlayer other)
	{
		return myPlayer != null && other != null && ((PunNetPlayer)myPlayer).PlayerRef.Equals(((PunNetPlayer)other).PlayerRef);
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x00050E24 File Offset: 0x0004F024
	public override void OnReturned()
	{
		base.OnReturned();
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x00050E2C File Offset: 0x0004F02C
	public override void OnTaken()
	{
		base.OnTaken();
		this.PlayerRef = null;
	}
}
