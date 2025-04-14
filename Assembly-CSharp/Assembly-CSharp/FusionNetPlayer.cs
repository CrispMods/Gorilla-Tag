using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

// Token: 0x0200025C RID: 604
public class FusionNetPlayer : NetPlayer
{
	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000E07 RID: 3591 RVA: 0x00047744 File Offset: 0x00045944
	// (set) Token: 0x06000E08 RID: 3592 RVA: 0x0004774C File Offset: 0x0004594C
	public PlayerRef PlayerRef { get; private set; }

	// Token: 0x06000E09 RID: 3593 RVA: 0x00047758 File Offset: 0x00045958
	public FusionNetPlayer()
	{
		this.PlayerRef = default(PlayerRef);
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x0004777A File Offset: 0x0004597A
	public FusionNetPlayer(PlayerRef playerRef)
	{
		this.PlayerRef = playerRef;
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000E0B RID: 3595 RVA: 0x00047789 File Offset: 0x00045989
	private NetworkRunner runner
	{
		get
		{
			return ((NetworkSystemFusion)NetworkSystem.Instance).runner;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0004779C File Offset: 0x0004599C
	public override bool IsValid
	{
		get
		{
			return this.validPlayer && this.PlayerRef.IsRealPlayer;
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000E0D RID: 3597 RVA: 0x000477C4 File Offset: 0x000459C4
	public override int ActorNumber
	{
		get
		{
			return this.PlayerRef.PlayerId;
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000E0E RID: 3598 RVA: 0x000477E0 File Offset: 0x000459E0
	public override string UserId
	{
		get
		{
			return NetworkSystem.Instance.GetUserID(this.PlayerRef.PlayerId);
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000E0F RID: 3599 RVA: 0x00047808 File Offset: 0x00045A08
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
	// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0004785C File Offset: 0x00045A5C
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
	// (get) Token: 0x06000E11 RID: 3601 RVA: 0x000478A2 File Offset: 0x00045AA2
	public override bool IsNull
	{
		get
		{
			PlayerRef playerRef = this.PlayerRef;
			return false;
		}
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000E12 RID: 3602 RVA: 0x000478AC File Offset: 0x00045AAC
	public override string NickName
	{
		get
		{
			return NetworkSystem.Instance.GetNickName(this);
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000E13 RID: 3603 RVA: 0x000478BC File Offset: 0x00045ABC
	public override string DefaultName
	{
		get
		{
			if (string.IsNullOrEmpty(this._defaultName))
			{
				this._defaultName = "gorilla" + Random.Range(0, 9999).ToString().PadLeft(4, '0');
			}
			return this._defaultName;
		}
	}

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06000E14 RID: 3604 RVA: 0x00047908 File Offset: 0x00045B08
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

	// Token: 0x06000E15 RID: 3605 RVA: 0x00047968 File Offset: 0x00045B68
	public override bool Equals(NetPlayer myPlayer, NetPlayer other)
	{
		return myPlayer != null && other != null && ((FusionNetPlayer)myPlayer).PlayerRef.Equals(((FusionNetPlayer)other).PlayerRef);
	}

	// Token: 0x06000E16 RID: 3606 RVA: 0x0004799B File Offset: 0x00045B9B
	public void InitPlayer(PlayerRef player)
	{
		this.PlayerRef = player;
		this.validPlayer = true;
	}

	// Token: 0x06000E17 RID: 3607 RVA: 0x000479AC File Offset: 0x00045BAC
	public override void OnReturned()
	{
		base.OnReturned();
		this.PlayerRef = default(PlayerRef);
		if (this.PlayerRef.PlayerId != -1)
		{
			Debug.LogError("Returned Player to pool but isnt -1, broken");
		}
	}

	// Token: 0x06000E18 RID: 3608 RVA: 0x000479E9 File Offset: 0x00045BE9
	public override void OnTaken()
	{
		base.OnTaken();
	}

	// Token: 0x040010EB RID: 4331
	private string _defaultName;

	// Token: 0x040010EC RID: 4332
	private bool validPlayer;
}
