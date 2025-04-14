using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public class TempMask : MonoBehaviour
{
	// Token: 0x06000441 RID: 1089 RVA: 0x00019BDC File Offset: 0x00017DDC
	private void Awake()
	{
		this.dayOn = new DateTime(this.year, this.month, this.day);
		this.myRig = base.GetComponentInParent<VRRig>();
		if (this.myRig != null && this.myRig.netView.IsMine && !this.myRig.isOfflineVRRig)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x00019C4A File Offset: 0x00017E4A
	private void OnEnable()
	{
		base.StartCoroutine(this.MaskOnDuringDate());
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x00019C59 File Offset: 0x00017E59
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x00019C61 File Offset: 0x00017E61
	private IEnumerator MaskOnDuringDate()
	{
		for (;;)
		{
			if (GorillaComputer.instance != null && GorillaComputer.instance.startupMillis != 0L)
			{
				this.myDate = new DateTime(GorillaComputer.instance.startupMillis * 10000L + (long)(Time.realtimeSinceStartup * 1000f * 10000f)).Subtract(TimeSpan.FromHours(7.0));
				if (this.myDate.DayOfYear == this.dayOn.DayOfYear)
				{
					if (!this.myRenderer.enabled)
					{
						this.myRenderer.enabled = true;
					}
				}
				else if (this.myRenderer.enabled)
				{
					this.myRenderer.enabled = false;
				}
			}
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	// Token: 0x040004F5 RID: 1269
	public int year;

	// Token: 0x040004F6 RID: 1270
	public int month;

	// Token: 0x040004F7 RID: 1271
	public int day;

	// Token: 0x040004F8 RID: 1272
	public DateTime dayOn;

	// Token: 0x040004F9 RID: 1273
	public MeshRenderer myRenderer;

	// Token: 0x040004FA RID: 1274
	private DateTime myDate;

	// Token: 0x040004FB RID: 1275
	private VRRig myRig;
}
