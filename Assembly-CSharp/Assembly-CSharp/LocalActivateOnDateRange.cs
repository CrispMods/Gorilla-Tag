using System;
using GorillaTag;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class LocalActivateOnDateRange : MonoBehaviour
{
	// Token: 0x06000A9E RID: 2718 RVA: 0x00039CA8 File Offset: 0x00037EA8
	private void Awake()
	{
		GameObject[] array = this.gameObjectsToActivate;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x00039CD3 File Offset: 0x00037ED3
	private void OnEnable()
	{
		this.InitActiveTimes();
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x00039CDC File Offset: 0x00037EDC
	private void InitActiveTimes()
	{
		this.activationTime = new DateTime(this.activationYear, this.activationMonth, this.activationDay, this.activationHour, this.activationMinute, this.activationSecond, DateTimeKind.Utc);
		this.deactivationTime = new DateTime(this.deactivationYear, this.deactivationMonth, this.deactivationDay, this.deactivationHour, this.deactivationMinute, this.deactivationSecond, DateTimeKind.Utc);
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00039D4C File Offset: 0x00037F4C
	private void LateUpdate()
	{
		DateTime utcNow = DateTime.UtcNow;
		this.dbgTimeUntilActivation = (this.activationTime - utcNow).TotalSeconds;
		this.dbgTimeUntilDeactivation = (this.deactivationTime - utcNow).TotalSeconds;
		bool flag = utcNow >= this.activationTime && utcNow <= this.deactivationTime;
		if (flag != this.isActive)
		{
			GameObject[] array = this.gameObjectsToActivate;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(flag);
			}
			this.isActive = flag;
		}
	}

	// Token: 0x04000D07 RID: 3335
	[Header("Activation Date and Time (UTC)")]
	public int activationYear = 2023;

	// Token: 0x04000D08 RID: 3336
	public int activationMonth = 4;

	// Token: 0x04000D09 RID: 3337
	public int activationDay = 1;

	// Token: 0x04000D0A RID: 3338
	public int activationHour = 7;

	// Token: 0x04000D0B RID: 3339
	public int activationMinute;

	// Token: 0x04000D0C RID: 3340
	public int activationSecond;

	// Token: 0x04000D0D RID: 3341
	[Header("Deactivation Date and Time (UTC)")]
	public int deactivationYear = 2023;

	// Token: 0x04000D0E RID: 3342
	public int deactivationMonth = 4;

	// Token: 0x04000D0F RID: 3343
	public int deactivationDay = 2;

	// Token: 0x04000D10 RID: 3344
	public int deactivationHour = 7;

	// Token: 0x04000D11 RID: 3345
	public int deactivationMinute;

	// Token: 0x04000D12 RID: 3346
	public int deactivationSecond;

	// Token: 0x04000D13 RID: 3347
	public GameObject[] gameObjectsToActivate;

	// Token: 0x04000D14 RID: 3348
	private bool isActive;

	// Token: 0x04000D15 RID: 3349
	private DateTime activationTime;

	// Token: 0x04000D16 RID: 3350
	private DateTime deactivationTime;

	// Token: 0x04000D17 RID: 3351
	[DebugReadout]
	public double dbgTimeUntilActivation;

	// Token: 0x04000D18 RID: 3352
	[DebugReadout]
	public double dbgTimeUntilDeactivation;
}
