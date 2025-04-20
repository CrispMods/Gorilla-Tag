using System;
using UnityEngine;

// Token: 0x02000808 RID: 2056
[CreateAssetMenu(menuName = "ScriptableObjects/FXSystemSettings", order = 2)]
public class FXSystemSettings : ScriptableObject
{
	// Token: 0x060032CE RID: 13006 RVA: 0x00138B48 File Offset: 0x00136D48
	public void Awake()
	{
		int num = (this.callLimits != null) ? this.callLimits.Length : 0;
		int num2 = (this.CallLimitsCooldown != null) ? this.CallLimitsCooldown.Length : 0;
		for (int i = 0; i < num; i++)
		{
			FXType key = this.callLimits[i].Key;
			int num3 = (int)key;
			if (this.callSettings[num3] != null)
			{
				Debug.Log("call setting for " + key.ToString() + " already exists, skipping");
			}
			else
			{
				this.callSettings[num3] = this.callLimits[i];
			}
		}
		for (int i = 0; i < num2; i++)
		{
			FXType key = this.CallLimitsCooldown[i].Key;
			int num3 = (int)key;
			if (this.callSettings[num3] != null)
			{
				Debug.Log("call setting for " + key.ToString() + " already exists, skipping");
			}
			else
			{
				this.callSettings[num3] = this.CallLimitsCooldown[i];
			}
		}
		for (int i = 0; i < this.callSettings.Length; i++)
		{
			if (this.callSettings[i] == null)
			{
				this.callSettings[i] = new LimiterType
				{
					CallLimitSettings = new CallLimiter(0, 0f, 0f),
					Key = (FXType)i
				};
			}
		}
	}

	// Token: 0x0400365C RID: 13916
	[SerializeField]
	private LimiterType[] callLimits;

	// Token: 0x0400365D RID: 13917
	[SerializeField]
	private CooldownType[] CallLimitsCooldown;

	// Token: 0x0400365E RID: 13918
	[NonSerialized]
	public bool forLocalRig;

	// Token: 0x0400365F RID: 13919
	[NonSerialized]
	public CallLimitType<CallLimiter>[] callSettings = new CallLimitType<CallLimiter>[20];
}
