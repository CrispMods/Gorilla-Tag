using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class TempMask : MonoBehaviour
{
	// Token: 0x0600047B RID: 1147 RVA: 0x0007D32C File Offset: 0x0007B52C
	private void Awake()
	{
		this.dayOn = new DateTime(this.year, this.month, this.day);
		this.myRig = base.GetComponentInParent<VRRig>();
		if (this.myRig != null && this.myRig.netView.IsMine && !this.myRig.isOfflineVRRig)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x00033627 File Offset: 0x00031827
	private void OnEnable()
	{
		base.StartCoroutine(this.MaskOnDuringDate());
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x00033636 File Offset: 0x00031836
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0003363E File Offset: 0x0003183E
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

	// Token: 0x04000534 RID: 1332
	public int year;

	// Token: 0x04000535 RID: 1333
	public int month;

	// Token: 0x04000536 RID: 1334
	public int day;

	// Token: 0x04000537 RID: 1335
	public DateTime dayOn;

	// Token: 0x04000538 RID: 1336
	public MeshRenderer myRenderer;

	// Token: 0x04000539 RID: 1337
	private DateTime myDate;

	// Token: 0x0400053A RID: 1338
	private VRRig myRig;
}
