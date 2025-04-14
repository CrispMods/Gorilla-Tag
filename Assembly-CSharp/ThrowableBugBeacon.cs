using System;
using UnityEngine;

// Token: 0x020008C4 RID: 2244
public class ThrowableBugBeacon : MonoBehaviour
{
	// Token: 0x14000068 RID: 104
	// (add) Token: 0x06003633 RID: 13875 RVA: 0x00100FA0 File Offset: 0x000FF1A0
	// (remove) Token: 0x06003634 RID: 13876 RVA: 0x00100FD4 File Offset: 0x000FF1D4
	public static event ThrowableBugBeacon.ThrowableBugBeaconEvent OnCall;

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x06003635 RID: 13877 RVA: 0x00101008 File Offset: 0x000FF208
	// (remove) Token: 0x06003636 RID: 13878 RVA: 0x0010103C File Offset: 0x000FF23C
	public static event ThrowableBugBeacon.ThrowableBugBeaconEvent OnDismiss;

	// Token: 0x1400006A RID: 106
	// (add) Token: 0x06003637 RID: 13879 RVA: 0x00101070 File Offset: 0x000FF270
	// (remove) Token: 0x06003638 RID: 13880 RVA: 0x001010A4 File Offset: 0x000FF2A4
	public static event ThrowableBugBeacon.ThrowableBugBeaconEvent OnLock;

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x06003639 RID: 13881 RVA: 0x001010D8 File Offset: 0x000FF2D8
	// (remove) Token: 0x0600363A RID: 13882 RVA: 0x0010110C File Offset: 0x000FF30C
	public static event ThrowableBugBeacon.ThrowableBugBeaconEvent OnUnlock;

	// Token: 0x1400006C RID: 108
	// (add) Token: 0x0600363B RID: 13883 RVA: 0x00101140 File Offset: 0x000FF340
	// (remove) Token: 0x0600363C RID: 13884 RVA: 0x00101174 File Offset: 0x000FF374
	public static event ThrowableBugBeacon.ThrowableBugBeaconFloatEvent OnChangeSpeedMultiplier;

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x0600363D RID: 13885 RVA: 0x001011A7 File Offset: 0x000FF3A7
	public ThrowableBug.BugName BugName
	{
		get
		{
			return this.bugName;
		}
	}

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x0600363E RID: 13886 RVA: 0x001011AF File Offset: 0x000FF3AF
	public float Range
	{
		get
		{
			return this.range;
		}
	}

	// Token: 0x0600363F RID: 13887 RVA: 0x001011B7 File Offset: 0x000FF3B7
	public void Call()
	{
		if (ThrowableBugBeacon.OnCall != null)
		{
			ThrowableBugBeacon.OnCall(this);
		}
	}

	// Token: 0x06003640 RID: 13888 RVA: 0x001011CB File Offset: 0x000FF3CB
	public void Dismiss()
	{
		if (ThrowableBugBeacon.OnDismiss != null)
		{
			ThrowableBugBeacon.OnDismiss(this);
		}
	}

	// Token: 0x06003641 RID: 13889 RVA: 0x001011DF File Offset: 0x000FF3DF
	public void Lock()
	{
		if (ThrowableBugBeacon.OnLock != null)
		{
			ThrowableBugBeacon.OnLock(this);
		}
	}

	// Token: 0x06003642 RID: 13890 RVA: 0x001011F3 File Offset: 0x000FF3F3
	public void Unlock()
	{
		if (ThrowableBugBeacon.OnUnlock != null)
		{
			ThrowableBugBeacon.OnUnlock(this);
		}
	}

	// Token: 0x06003643 RID: 13891 RVA: 0x00101207 File Offset: 0x000FF407
	public void ChangeSpeedMultiplier(float f)
	{
		if (ThrowableBugBeacon.OnChangeSpeedMultiplier != null)
		{
			ThrowableBugBeacon.OnChangeSpeedMultiplier(this, f);
		}
	}

	// Token: 0x06003644 RID: 13892 RVA: 0x001011F3 File Offset: 0x000FF3F3
	private void OnDisable()
	{
		if (ThrowableBugBeacon.OnUnlock != null)
		{
			ThrowableBugBeacon.OnUnlock(this);
		}
	}

	// Token: 0x04003896 RID: 14486
	[SerializeField]
	private float range;

	// Token: 0x04003897 RID: 14487
	[SerializeField]
	private ThrowableBug.BugName bugName;

	// Token: 0x020008C5 RID: 2245
	// (Invoke) Token: 0x06003647 RID: 13895
	public delegate void ThrowableBugBeaconEvent(ThrowableBugBeacon tbb);

	// Token: 0x020008C6 RID: 2246
	// (Invoke) Token: 0x0600364B RID: 13899
	public delegate void ThrowableBugBeaconFloatEvent(ThrowableBugBeacon tbb, float f);
}
