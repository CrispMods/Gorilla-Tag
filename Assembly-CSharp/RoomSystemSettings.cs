using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x0200081F RID: 2079
[CreateAssetMenu(menuName = "ScriptableObjects/RoomSystemSettings", order = 2)]
internal class RoomSystemSettings : ScriptableObject
{
	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x06003352 RID: 13138 RVA: 0x00051DC4 File Offset: 0x0004FFC4
	public ExpectedUsersDecayTimer ExpectedUsersTimer
	{
		get
		{
			return this.expectedUsersTimer;
		}
	}

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x06003353 RID: 13139 RVA: 0x00051DCC File Offset: 0x0004FFCC
	public CallLimiterWithCooldown StatusEffectLimiter
	{
		get
		{
			return this.statusEffectLimiter;
		}
	}

	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x06003354 RID: 13140 RVA: 0x00051DD4 File Offset: 0x0004FFD4
	public CallLimiterWithCooldown SoundEffectLimiter
	{
		get
		{
			return this.soundEffectLimiter;
		}
	}

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x06003355 RID: 13141 RVA: 0x00051DDC File Offset: 0x0004FFDC
	public CallLimiterWithCooldown SoundEffectOtherLimiter
	{
		get
		{
			return this.soundEffectOtherLimiter;
		}
	}

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06003356 RID: 13142 RVA: 0x00051DE4 File Offset: 0x0004FFE4
	public CallLimiterWithCooldown PlayerEffectLimiter
	{
		get
		{
			return this.playerEffectLimiter;
		}
	}

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06003357 RID: 13143 RVA: 0x00051DEC File Offset: 0x0004FFEC
	public GameObject PlayerImpactEffect
	{
		get
		{
			return this.playerImpactEffect;
		}
	}

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x06003358 RID: 13144 RVA: 0x00051DF4 File Offset: 0x0004FFF4
	public List<RoomSystem.PlayerEffectConfig> PlayerEffects
	{
		get
		{
			return this.playerEffects;
		}
	}

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x06003359 RID: 13145 RVA: 0x00051DFC File Offset: 0x0004FFFC
	public int PausedDCTimer
	{
		get
		{
			return this.pausedDCTimer;
		}
	}

	// Token: 0x040036DA RID: 14042
	[SerializeField]
	private ExpectedUsersDecayTimer expectedUsersTimer;

	// Token: 0x040036DB RID: 14043
	[SerializeField]
	private CallLimiterWithCooldown statusEffectLimiter;

	// Token: 0x040036DC RID: 14044
	[SerializeField]
	private CallLimiterWithCooldown soundEffectLimiter;

	// Token: 0x040036DD RID: 14045
	[SerializeField]
	private CallLimiterWithCooldown soundEffectOtherLimiter;

	// Token: 0x040036DE RID: 14046
	[SerializeField]
	private CallLimiterWithCooldown playerEffectLimiter;

	// Token: 0x040036DF RID: 14047
	[SerializeField]
	private GameObject playerImpactEffect;

	// Token: 0x040036E0 RID: 14048
	[SerializeField]
	private List<RoomSystem.PlayerEffectConfig> playerEffects = new List<RoomSystem.PlayerEffectConfig>();

	// Token: 0x040036E1 RID: 14049
	[SerializeField]
	private int pausedDCTimer;
}
