using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x02000805 RID: 2053
[CreateAssetMenu(menuName = "ScriptableObjects/RoomSystemSettings", order = 2)]
internal class RoomSystemSettings : ScriptableObject
{
	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x06003297 RID: 12951 RVA: 0x000F35BE File Offset: 0x000F17BE
	public ExpectedUsersDecayTimer ExpectedUsersTimer
	{
		get
		{
			return this.expectedUsersTimer;
		}
	}

	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x06003298 RID: 12952 RVA: 0x000F35C6 File Offset: 0x000F17C6
	public CallLimiterWithCooldown StatusEffectLimiter
	{
		get
		{
			return this.statusEffectLimiter;
		}
	}

	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x06003299 RID: 12953 RVA: 0x000F35CE File Offset: 0x000F17CE
	public CallLimiterWithCooldown SoundEffectLimiter
	{
		get
		{
			return this.soundEffectLimiter;
		}
	}

	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x0600329A RID: 12954 RVA: 0x000F35D6 File Offset: 0x000F17D6
	public CallLimiterWithCooldown SoundEffectOtherLimiter
	{
		get
		{
			return this.soundEffectOtherLimiter;
		}
	}

	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x0600329B RID: 12955 RVA: 0x000F35DE File Offset: 0x000F17DE
	public CallLimiterWithCooldown PlayerEffectLimiter
	{
		get
		{
			return this.playerEffectLimiter;
		}
	}

	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x0600329C RID: 12956 RVA: 0x000F35E6 File Offset: 0x000F17E6
	public GameObject PlayerImpactEffect
	{
		get
		{
			return this.playerImpactEffect;
		}
	}

	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x0600329D RID: 12957 RVA: 0x000F35EE File Offset: 0x000F17EE
	public List<RoomSystem.PlayerEffectConfig> PlayerEffects
	{
		get
		{
			return this.playerEffects;
		}
	}

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x0600329E RID: 12958 RVA: 0x000F35F6 File Offset: 0x000F17F6
	public int PausedDCTimer
	{
		get
		{
			return this.pausedDCTimer;
		}
	}

	// Token: 0x0400361E RID: 13854
	[SerializeField]
	private ExpectedUsersDecayTimer expectedUsersTimer;

	// Token: 0x0400361F RID: 13855
	[SerializeField]
	private CallLimiterWithCooldown statusEffectLimiter;

	// Token: 0x04003620 RID: 13856
	[SerializeField]
	private CallLimiterWithCooldown soundEffectLimiter;

	// Token: 0x04003621 RID: 13857
	[SerializeField]
	private CallLimiterWithCooldown soundEffectOtherLimiter;

	// Token: 0x04003622 RID: 13858
	[SerializeField]
	private CallLimiterWithCooldown playerEffectLimiter;

	// Token: 0x04003623 RID: 13859
	[SerializeField]
	private GameObject playerImpactEffect;

	// Token: 0x04003624 RID: 13860
	[SerializeField]
	private List<RoomSystem.PlayerEffectConfig> playerEffects = new List<RoomSystem.PlayerEffectConfig>();

	// Token: 0x04003625 RID: 13861
	[SerializeField]
	private int pausedDCTimer;
}
