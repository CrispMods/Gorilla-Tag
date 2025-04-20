using System;
using UnityEngine;

// Token: 0x020008E0 RID: 2272
public class ThrowableBugBeacon : MonoBehaviour
{
	// Token: 0x1400006C RID: 108
	// (add) Token: 0x060036FB RID: 14075 RVA: 0x00146788 File Offset: 0x00144988
	// (remove) Token: 0x060036FC RID: 14076 RVA: 0x001467BC File Offset: 0x001449BC
	public static event ThrowableBugBeacon.ThrowableBugBeaconEvent OnCall;

	// Token: 0x1400006D RID: 109
	// (add) Token: 0x060036FD RID: 14077 RVA: 0x001467F0 File Offset: 0x001449F0
	// (remove) Token: 0x060036FE RID: 14078 RVA: 0x00146824 File Offset: 0x00144A24
	public static event ThrowableBugBeacon.ThrowableBugBeaconEvent OnDismiss;

	// Token: 0x1400006E RID: 110
	// (add) Token: 0x060036FF RID: 14079 RVA: 0x00146858 File Offset: 0x00144A58
	// (remove) Token: 0x06003700 RID: 14080 RVA: 0x0014688C File Offset: 0x00144A8C
	public static event ThrowableBugBeacon.ThrowableBugBeaconEvent OnLock;

	// Token: 0x1400006F RID: 111
	// (add) Token: 0x06003701 RID: 14081 RVA: 0x001468C0 File Offset: 0x00144AC0
	// (remove) Token: 0x06003702 RID: 14082 RVA: 0x001468F4 File Offset: 0x00144AF4
	public static event ThrowableBugBeacon.ThrowableBugBeaconEvent OnUnlock;

	// Token: 0x14000070 RID: 112
	// (add) Token: 0x06003703 RID: 14083 RVA: 0x00146928 File Offset: 0x00144B28
	// (remove) Token: 0x06003704 RID: 14084 RVA: 0x0014695C File Offset: 0x00144B5C
	public static event ThrowableBugBeacon.ThrowableBugBeaconFloatEvent OnChangeSpeedMultiplier;

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x06003705 RID: 14085 RVA: 0x00054531 File Offset: 0x00052731
	public ThrowableBug.BugName BugName
	{
		get
		{
			return this.bugName;
		}
	}

	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x06003706 RID: 14086 RVA: 0x00054539 File Offset: 0x00052739
	public float Range
	{
		get
		{
			return this.range;
		}
	}

	// Token: 0x06003707 RID: 14087 RVA: 0x00054541 File Offset: 0x00052741
	public void Call()
	{
		if (ThrowableBugBeacon.OnCall != null)
		{
			ThrowableBugBeacon.OnCall(this);
		}
	}

	// Token: 0x06003708 RID: 14088 RVA: 0x00054555 File Offset: 0x00052755
	public void Dismiss()
	{
		if (ThrowableBugBeacon.OnDismiss != null)
		{
			ThrowableBugBeacon.OnDismiss(this);
		}
	}

	// Token: 0x06003709 RID: 14089 RVA: 0x00054569 File Offset: 0x00052769
	public void Lock()
	{
		if (ThrowableBugBeacon.OnLock != null)
		{
			ThrowableBugBeacon.OnLock(this);
		}
	}

	// Token: 0x0600370A RID: 14090 RVA: 0x0005457D File Offset: 0x0005277D
	public void Unlock()
	{
		if (ThrowableBugBeacon.OnUnlock != null)
		{
			ThrowableBugBeacon.OnUnlock(this);
		}
	}

	// Token: 0x0600370B RID: 14091 RVA: 0x00054591 File Offset: 0x00052791
	public void ChangeSpeedMultiplier(float f)
	{
		if (ThrowableBugBeacon.OnChangeSpeedMultiplier != null)
		{
			ThrowableBugBeacon.OnChangeSpeedMultiplier(this, f);
		}
	}

	// Token: 0x0600370C RID: 14092 RVA: 0x0005457D File Offset: 0x0005277D
	private void OnDisable()
	{
		if (ThrowableBugBeacon.OnUnlock != null)
		{
			ThrowableBugBeacon.OnUnlock(this);
		}
	}

	// Token: 0x04003957 RID: 14679
	[SerializeField]
	private float range;

	// Token: 0x04003958 RID: 14680
	[SerializeField]
	private ThrowableBug.BugName bugName;

	// Token: 0x020008E1 RID: 2273
	// (Invoke) Token: 0x0600370F RID: 14095
	public delegate void ThrowableBugBeaconEvent(ThrowableBugBeacon tbb);

	// Token: 0x020008E2 RID: 2274
	// (Invoke) Token: 0x06003713 RID: 14099
	public delegate void ThrowableBugBeaconFloatEvent(ThrowableBugBeacon tbb, float f);
}
