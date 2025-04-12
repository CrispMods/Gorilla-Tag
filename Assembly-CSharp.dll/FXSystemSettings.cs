using System;
using UnityEngine;

// Token: 0x020007F1 RID: 2033
[CreateAssetMenu(menuName = "ScriptableObjects/FXSystemSettings", order = 2)]
public class FXSystemSettings : ScriptableObject
{
	// Token: 0x06003224 RID: 12836 RVA: 0x00133928 File Offset: 0x00131B28
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

	// Token: 0x040035B3 RID: 13747
	[SerializeField]
	private LimiterType[] callLimits;

	// Token: 0x040035B4 RID: 13748
	[SerializeField]
	private CooldownType[] CallLimitsCooldown;

	// Token: 0x040035B5 RID: 13749
	[NonSerialized]
	public bool forLocalRig;

	// Token: 0x040035B6 RID: 13750
	[NonSerialized]
	public CallLimitType<CallLimiter>[] callSettings = new CallLimitType<CallLimiter>[15];
}
