using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x02000808 RID: 2056
[CreateAssetMenu(menuName = "ScriptableObjects/RoomSystemSettings", order = 2)]
internal class RoomSystemSettings : ScriptableObject
{
	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x060032A3 RID: 12963 RVA: 0x000509B6 File Offset: 0x0004EBB6
	public ExpectedUsersDecayTimer ExpectedUsersTimer
	{
		get
		{
			return this.expectedUsersTimer;
		}
	}

	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x060032A4 RID: 12964 RVA: 0x000509BE File Offset: 0x0004EBBE
	public CallLimiterWithCooldown StatusEffectLimiter
	{
		get
		{
			return this.statusEffectLimiter;
		}
	}

	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x060032A5 RID: 12965 RVA: 0x000509C6 File Offset: 0x0004EBC6
	public CallLimiterWithCooldown SoundEffectLimiter
	{
		get
		{
			return this.soundEffectLimiter;
		}
	}

	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x060032A6 RID: 12966 RVA: 0x000509CE File Offset: 0x0004EBCE
	public CallLimiterWithCooldown SoundEffectOtherLimiter
	{
		get
		{
			return this.soundEffectOtherLimiter;
		}
	}

	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x060032A7 RID: 12967 RVA: 0x000509D6 File Offset: 0x0004EBD6
	public CallLimiterWithCooldown PlayerEffectLimiter
	{
		get
		{
			return this.playerEffectLimiter;
		}
	}

	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x060032A8 RID: 12968 RVA: 0x000509DE File Offset: 0x0004EBDE
	public GameObject PlayerImpactEffect
	{
		get
		{
			return this.playerImpactEffect;
		}
	}

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x060032A9 RID: 12969 RVA: 0x000509E6 File Offset: 0x0004EBE6
	public List<RoomSystem.PlayerEffectConfig> PlayerEffects
	{
		get
		{
			return this.playerEffects;
		}
	}

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x060032AA RID: 12970 RVA: 0x000509EE File Offset: 0x0004EBEE
	public int PausedDCTimer
	{
		get
		{
			return this.pausedDCTimer;
		}
	}

	// Token: 0x04003630 RID: 13872
	[SerializeField]
	private ExpectedUsersDecayTimer expectedUsersTimer;

	// Token: 0x04003631 RID: 13873
	[SerializeField]
	private CallLimiterWithCooldown statusEffectLimiter;

	// Token: 0x04003632 RID: 13874
	[SerializeField]
	private CallLimiterWithCooldown soundEffectLimiter;

	// Token: 0x04003633 RID: 13875
	[SerializeField]
	private CallLimiterWithCooldown soundEffectOtherLimiter;

	// Token: 0x04003634 RID: 13876
	[SerializeField]
	private CallLimiterWithCooldown playerEffectLimiter;

	// Token: 0x04003635 RID: 13877
	[SerializeField]
	private GameObject playerImpactEffect;

	// Token: 0x04003636 RID: 13878
	[SerializeField]
	private List<RoomSystem.PlayerEffectConfig> playerEffects = new List<RoomSystem.PlayerEffectConfig>();

	// Token: 0x04003637 RID: 13879
	[SerializeField]
	private int pausedDCTimer;
}
