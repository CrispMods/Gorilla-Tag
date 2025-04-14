using System;
using UnityEngine;

// Token: 0x020007EE RID: 2030
[CreateAssetMenu(menuName = "ScriptableObjects/FXSystemSettings", order = 2)]
public class FXSystemSettings : ScriptableObject
{
	// Token: 0x06003218 RID: 12824 RVA: 0x000F0840 File Offset: 0x000EEA40
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

	// Token: 0x040035A1 RID: 13729
	[SerializeField]
	private LimiterType[] callLimits;

	// Token: 0x040035A2 RID: 13730
	[SerializeField]
	private CooldownType[] CallLimitsCooldown;

	// Token: 0x040035A3 RID: 13731
	[NonSerialized]
	public bool forLocalRig;

	// Token: 0x040035A4 RID: 13732
	[NonSerialized]
	public CallLimitType<CallLimiter>[] callSettings = new CallLimitType<CallLimiter>[15];
}
