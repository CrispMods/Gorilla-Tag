using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020008EA RID: 2282
public class TransformOscilation : MonoBehaviour
{
	// Token: 0x06003745 RID: 14149 RVA: 0x00146B78 File Offset: 0x00144D78
	private void Start()
	{
		this.rootPos = base.transform.localPosition;
		this.rootRot = base.transform.localRotation.eulerAngles;
	}

	// Token: 0x06003746 RID: 14150 RVA: 0x00146BB0 File Offset: 0x00144DB0
	private void Update()
	{
		if (this.useServerTime && GorillaComputer.instance == null)
		{
			return;
		}
		float num = Time.timeSinceLevelLoad;
		if (this.useServerTime)
		{
			this.dt = GorillaComputer.instance.GetServerTime();
			num = (float)this.dt.Minute * 60f + (float)this.dt.Second + (float)this.dt.Millisecond / 1000f;
		}
		this.offsPos.x = this.PosAmp.x * Mathf.Sin(num * this.PosFreq.x);
		this.offsPos.y = this.PosAmp.y * Mathf.Sin(num * this.PosFreq.y);
		this.offsPos.z = this.PosAmp.z * Mathf.Sin(num * this.PosFreq.z);
		this.offsRot.x = this.RotAmp.x * Mathf.Sin(num * this.RotFreq.x);
		this.offsRot.y = this.RotAmp.y * Mathf.Sin(num * this.RotFreq.y);
		this.offsRot.z = this.RotAmp.z * Mathf.Sin(num * this.RotFreq.z);
		base.transform.localPosition = this.rootPos + this.offsPos;
		base.transform.localRotation = Quaternion.Euler(this.rootRot + this.offsRot);
	}

	// Token: 0x04003976 RID: 14710
	[SerializeField]
	private Vector3 PosAmp;

	// Token: 0x04003977 RID: 14711
	[SerializeField]
	private Vector3 PosFreq;

	// Token: 0x04003978 RID: 14712
	[SerializeField]
	private Vector3 RotAmp;

	// Token: 0x04003979 RID: 14713
	[SerializeField]
	private Vector3 RotFreq;

	// Token: 0x0400397A RID: 14714
	private Vector3 rootPos;

	// Token: 0x0400397B RID: 14715
	private Vector3 rootRot;

	// Token: 0x0400397C RID: 14716
	private Vector3 offsPos = Vector3.zero;

	// Token: 0x0400397D RID: 14717
	private Vector3 offsRot = Vector3.zero;

	// Token: 0x0400397E RID: 14718
	private DateTime dt;

	// Token: 0x0400397F RID: 14719
	[SerializeField]
	private bool useServerTime;
}
