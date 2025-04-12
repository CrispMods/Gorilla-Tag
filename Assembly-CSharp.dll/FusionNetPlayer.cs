using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

// Token: 0x0200025C RID: 604
public class FusionNetPlayer : NetPlayer
{
	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000E07 RID: 3591 RVA: 0x0003906F File Offset: 0x0003726F
	// (set) Token: 0x06000E08 RID: 3592 RVA: 0x00039077 File Offset: 0x00037277
	public PlayerRef PlayerRef { get; private set; }

	// Token: 0x06000E09 RID: 3593 RVA: 0x000A19C0 File Offset: 0x0009FBC0
	public FusionNetPlayer()
	{
		this.PlayerRef = default(PlayerRef);
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x00039080 File Offset: 0x00037280
	public FusionNetPlayer(PlayerRef playerRef)
	{
		this.PlayerRef = playerRef;
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000E0B RID: 3595 RVA: 0x0003908F File Offset: 0x0003728F
	private NetworkRunner runner
	{
		get
		{
			return ((NetworkSystemFusion)NetworkSystem.Instance).runner;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000E0C RID: 3596 RVA: 0x000A19E4 File Offset: 0x0009FBE4
	public override bool IsValid
	{
		get
		{
			return this.validPlayer && this.PlayerRef.IsRealPlayer;
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000E0D RID: 3597 RVA: 0x000A1A0C File Offset: 0x0009FC0C
	public override int ActorNumber
	{
		get
		{
			return this.PlayerRef.PlayerId;
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000E0E RID: 3598 RVA: 0x000A1A28 File Offset: 0x0009FC28
	public override string UserId
	{
		get
		{
			return NetworkSystem.Instance.GetUserID(this.PlayerRef.PlayerId);
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000E0F RID: 3599 RVA: 0x000A1A50 File Offset: 0x0009FC50
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

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000E10 RID: 3600 RVA: 0x000A1AA4 File Offset: 0x0009FCA4
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

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000E11 RID: 3601 RVA: 0x000390A0 File Offset: 0x000372A0
	public override bool IsNull
	{
		get
		{
			PlayerRef playerRef = this.PlayerRef;
			return false;
		}
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000E12 RID: 3602 RVA: 0x000390AA File Offset: 0x000372AA
	public override string NickName
	{
		get
		{
			return NetworkSystem.Instance.GetNickName(this);
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000E13 RID: 3603 RVA: 0x000A1AEC File Offset: 0x0009FCEC
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

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06000E14 RID: 3604 RVA: 0x000A1B38 File Offset: 0x0009FD38
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

	// Token: 0x06000E15 RID: 3605 RVA: 0x000A1B98 File Offset: 0x0009FD98
	public override bool Equals(NetPlayer myPlayer, NetPlayer other)
	{
		return myPlayer != null && other != null && ((FusionNetPlayer)myPlayer).PlayerRef.Equals(((FusionNetPlayer)other).PlayerRef);
	}

	// Token: 0x06000E16 RID: 3606 RVA: 0x000390B7 File Offset: 0x000372B7
	public void InitPlayer(PlayerRef player)
	{
		this.PlayerRef = player;
		this.validPlayer = true;
	}

	// Token: 0x06000E17 RID: 3607 RVA: 0x000A1BCC File Offset: 0x0009FDCC
	public override void OnReturned()
	{
		base.OnReturned();
		this.PlayerRef = default(PlayerRef);
		if (this.PlayerRef.PlayerId != -1)
		{
			Debug.LogError("Returned Player to pool but isnt -1, broken");
		}
	}

	// Token: 0x06000E18 RID: 3608 RVA: 0x000390C7 File Offset: 0x000372C7
	public override void OnTaken()
	{
		base.OnTaken();
	}

	// Token: 0x040010EB RID: 4331
	private string _defaultName;

	// Token: 0x040010EC RID: 4332
	private bool validPlayer;
}
