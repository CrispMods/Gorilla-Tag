using System;
using UnityEngine;

// Token: 0x020008C7 RID: 2247
public class ThrowableBugBeacon : MonoBehaviour
{
	// Token: 0x14000068 RID: 104
	// (add) Token: 0x0600363F RID: 13887 RVA: 0x001411C8 File Offset: 0x0013F3C8
	// (remove) Token: 0x06003640 RID: 13888 RVA: 0x001411FC File Offset: 0x0013F3FC
	public static event ThrowableBugBeacon.ThrowableBugBeaconEvent OnCall;

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x06003641 RID: 13889 RVA: 0x00141230 File Offset: 0x0013F430
	// (remove) Token: 0x06003642 RID: 13890 RVA: 0x00141264 File Offset: 0x0013F464
	public static event ThrowableBugBeacon.ThrowableBugBeaconEvent OnDismiss;

	// Token: 0x1400006A RID: 106
	// (add) Token: 0x06003643 RID: 13891 RVA: 0x00141298 File Offset: 0x0013F498
	// (remove) Token: 0x06003644 RID: 13892 RVA: 0x001412CC File Offset: 0x0013F4CC
	public static event ThrowableBugBeacon.ThrowableBugBeaconEvent OnLock;

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x06003645 RID: 13893 RVA: 0x00141300 File Offset: 0x0013F500
	// (remove) Token: 0x06003646 RID: 13894 RVA: 0x00141334 File Offset: 0x0013F534
	public static event ThrowableBugBeacon.ThrowableBugBeaconEvent OnUnlock;

	// Token: 0x1400006C RID: 108
	// (add) Token: 0x06003647 RID: 13895 RVA: 0x00141368 File Offset: 0x0013F568
	// (remove) Token: 0x06003648 RID: 13896 RVA: 0x0014139C File Offset: 0x0013F59C
	public static event ThrowableBugBeacon.ThrowableBugBeaconFloatEvent OnChangeSpeedMultiplier;

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x06003649 RID: 13897 RVA: 0x00053014 File Offset: 0x00051214
	public ThrowableBug.BugName BugName
	{
		get
		{
			return this.bugName;
		}
	}

	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x0600364A RID: 13898 RVA: 0x0005301C File Offset: 0x0005121C
	public float Range
	{
		get
		{
			return this.range;
		}
	}

	// Token: 0x0600364B RID: 13899 RVA: 0x00053024 File Offset: 0x00051224
	public void Call()
	{
		if (ThrowableBugBeacon.OnCall != null)
		{
			ThrowableBugBeacon.OnCall(this);
		}
	}

	// Token: 0x0600364C RID: 13900 RVA: 0x00053038 File Offset: 0x00051238
	public void Dismiss()
	{
		if (ThrowableBugBeacon.OnDismiss != null)
		{
			ThrowableBugBeacon.OnDismiss(this);
		}
	}

	// Token: 0x0600364D RID: 13901 RVA: 0x0005304C File Offset: 0x0005124C
	public void Lock()
	{
		if (ThrowableBugBeacon.OnLock != null)
		{
			ThrowableBugBeacon.OnLock(this);
		}
	}

	// Token: 0x0600364E RID: 13902 RVA: 0x00053060 File Offset: 0x00051260
	public void Unlock()
	{
		if (ThrowableBugBeacon.OnUnlock != null)
		{
			ThrowableBugBeacon.OnUnlock(this);
		}
	}

	// Token: 0x0600364F RID: 13903 RVA: 0x00053074 File Offset: 0x00051274
	public void ChangeSpeedMultiplier(float f)
	{
		if (ThrowableBugBeacon.OnChangeSpeedMultiplier != null)
		{
			ThrowableBugBeacon.OnChangeSpeedMultiplier(this, f);
		}
	}

	// Token: 0x06003650 RID: 13904 RVA: 0x00053060 File Offset: 0x00051260
	private void OnDisable()
	{
		if (ThrowableBugBeacon.OnUnlock != null)
		{
			ThrowableBugBeacon.OnUnlock(this);
		}
	}

	// Token: 0x040038A8 RID: 14504
	[SerializeField]
	private float range;

	// Token: 0x040038A9 RID: 14505
	[SerializeField]
	private ThrowableBug.BugName bugName;

	// Token: 0x020008C8 RID: 2248
	// (Invoke) Token: 0x06003653 RID: 13907
	public delegate void ThrowableBugBeaconEvent(ThrowableBugBeacon tbb);

	// Token: 0x020008C9 RID: 2249
	// (Invoke) Token: 0x06003657 RID: 13911
	public delegate void ThrowableBugBeaconFloatEvent(ThrowableBugBeacon tbb, float f);
}
