using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

// Token: 0x02000267 RID: 615
public class FusionNetPlayer : NetPlayer
{
	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000E50 RID: 3664 RVA: 0x0003A32F File Offset: 0x0003852F
	// (set) Token: 0x06000E51 RID: 3665 RVA: 0x0003A337 File Offset: 0x00038537
	public PlayerRef PlayerRef { get; private set; }

	// Token: 0x06000E52 RID: 3666 RVA: 0x000A424C File Offset: 0x000A244C
	public FusionNetPlayer()
	{
		this.PlayerRef = default(PlayerRef);
	}

	// Token: 0x06000E53 RID: 3667 RVA: 0x0003A340 File Offset: 0x00038540
	public FusionNetPlayer(PlayerRef playerRef)
	{
		this.PlayerRef = playerRef;
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000E54 RID: 3668 RVA: 0x0003A34F File Offset: 0x0003854F
	private NetworkRunner runner
	{
		get
		{
			return ((NetworkSystemFusion)NetworkSystem.Instance).runner;
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000E55 RID: 3669 RVA: 0x000A4270 File Offset: 0x000A2470
	public override bool IsValid
	{
		get
		{
			return this.validPlayer && this.PlayerRef.IsRealPlayer;
		}
	}

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06000E56 RID: 3670 RVA: 0x000A4298 File Offset: 0x000A2498
	public override int ActorNumber
	{
		get
		{
			return this.PlayerRef.PlayerId;
		}
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06000E57 RID: 3671 RVA: 0x000A42B4 File Offset: 0x000A24B4
	public override string UserId
	{
		get
		{
			return NetworkSystem.Instance.GetUserID(this.PlayerRef.PlayerId);
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000E58 RID: 3672 RVA: 0x000A42DC File Offset: 0x000A24DC
	public override bool IsMasterClient
	{
		get
		{
			if (!(this.runner == null))
			{
				return (this.IsLocal && this.runner.IsSharedModeMasterClient) || NetworkSystem.Instance.MasterClient == this;
			}
			return this.PlayerRef == default(PlayerRef);
		}
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06000E59 RID: 3673 RVA: 0x000A4330 File Offset: 0x000A2530
	public override bool IsLocal
	{
		get
		{
			if (!(this.runner == null))
			{
				return this.PlayerRef == this.runner.LocalPlayer;
			}
			return this.PlayerRef == default(PlayerRef);
		}
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06000E5A RID: 3674 RVA: 0x0003A360 File Offset: 0x00038560
	public override bool IsNull
	{
		get
		{
			PlayerRef playerRef = this.PlayerRef;
			return false;
		}
	}

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06000E5B RID: 3675 RVA: 0x0003A36A File Offset: 0x0003856A
	public override string NickName
	{
		get
		{
			return NetworkSystem.Instance.GetNickName(this);
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06000E5C RID: 3676 RVA: 0x000A4378 File Offset: 0x000A2578
	public override string DefaultName
	{
		get
		{
			if (string.IsNullOrEmpty(this._defaultName))
			{
				this._defaultName = "gorilla" + UnityEngine.Random.Range(0, 9999).ToString().PadLeft(4, '0');
			}
			return this._defaultName;
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06000E5D RID: 3677 RVA: 0x000A43C4 File Offset: 0x000A25C4
	public override bool InRoom
	{
		get
		{
			using (IEnumerator<PlayerRef> enumerator = this.runner.ActivePlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == this.PlayerRef)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x000A4424 File Offset: 0x000A2624
	public override bool Equals(NetPlayer myPlayer, NetPlayer other)
	{
		return myPlayer != null && other != null && ((FusionNetPlayer)myPlayer).PlayerRef.Equals(((FusionNetPlayer)other).PlayerRef);
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x0003A377 File Offset: 0x00038577
	public void InitPlayer(PlayerRef player)
	{
		this.PlayerRef = player;
		this.validPlayer = true;
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x000A4458 File Offset: 0x000A2658
	public override void OnReturned()
	{
		base.OnReturned();
		this.PlayerRef = default(PlayerRef);
		if (this.PlayerRef.PlayerId != -1)
		{
			Debug.LogError("Returned Player to pool but isnt -1, broken");
		}
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x0003A387 File Offset: 0x00038587
	public override void OnTaken()
	{
		base.OnTaken();
	}

	// Token: 0x04001130 RID: 4400
	private string _defaultName;

	// Token: 0x04001131 RID: 4401
	private bool validPlayer;
}
