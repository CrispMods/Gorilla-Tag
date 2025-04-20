using System;
using GorillaTag;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class LocalActivateOnDateRange : MonoBehaviour
{
	// Token: 0x06000AE8 RID: 2792 RVA: 0x00099048 File Offset: 0x00097248
	private void Awake()
	{
		GameObject[] array = this.gameObjectsToActivate;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x00037B08 File Offset: 0x00035D08
	private void OnEnable()
	{
		this.InitActiveTimes();
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x00099074 File Offset: 0x00097274
	private void InitActiveTimes()
	{
		this.activationTime = new DateTime(this.activationYear, this.activationMonth, this.activationDay, this.activationHour, this.activationMinute, this.activationSecond, DateTimeKind.Utc);
		this.deactivationTime = new DateTime(this.deactivationYear, this.deactivationMonth, this.deactivationDay, this.deactivationHour, this.deactivationMinute, this.deactivationSecond, DateTimeKind.Utc);
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x000990E4 File Offset: 0x000972E4
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

	// Token: 0x04000D4C RID: 3404
	[Header("Activation Date and Time (UTC)")]
	public int activationYear = 2023;

	// Token: 0x04000D4D RID: 3405
	public int activationMonth = 4;

	// Token: 0x04000D4E RID: 3406
	public int activationDay = 1;

	// Token: 0x04000D4F RID: 3407
	public int activationHour = 7;

	// Token: 0x04000D50 RID: 3408
	public int activationMinute;

	// Token: 0x04000D51 RID: 3409
	public int activationSecond;

	// Token: 0x04000D52 RID: 3410
	[Header("Deactivation Date and Time (UTC)")]
	public int deactivationYear = 2023;

	// Token: 0x04000D53 RID: 3411
	public int deactivationMonth = 4;

	// Token: 0x04000D54 RID: 3412
	public int deactivationDay = 2;

	// Token: 0x04000D55 RID: 3413
	public int deactivationHour = 7;

	// Token: 0x04000D56 RID: 3414
	public int deactivationMinute;

	// Token: 0x04000D57 RID: 3415
	public int deactivationSecond;

	// Token: 0x04000D58 RID: 3416
	public GameObject[] gameObjectsToActivate;

	// Token: 0x04000D59 RID: 3417
	private bool isActive;

	// Token: 0x04000D5A RID: 3418
	private DateTime activationTime;

	// Token: 0x04000D5B RID: 3419
	private DateTime deactivationTime;

	// Token: 0x04000D5C RID: 3420
	[DebugReadout]
	public double dbgTimeUntilActivation;

	// Token: 0x04000D5D RID: 3421
	[DebugReadout]
	public double dbgTimeUntilDeactivation;
}
