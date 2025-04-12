using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020008D1 RID: 2257
public class TransformOscilation : MonoBehaviour
{
	// Token: 0x06003689 RID: 13961 RVA: 0x001415B8 File Offset: 0x0013F7B8
	private void Start()
	{
		this.rootPos = base.transform.localPosition;
		this.rootRot = base.transform.localRotation.eulerAngles;
	}

	// Token: 0x0600368A RID: 13962 RVA: 0x001415F0 File Offset: 0x0013F7F0
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

	// Token: 0x040038C7 RID: 14535
	[SerializeField]
	private Vector3 PosAmp;

	// Token: 0x040038C8 RID: 14536
	[SerializeField]
	private Vector3 PosFreq;

	// Token: 0x040038C9 RID: 14537
	[SerializeField]
	private Vector3 RotAmp;

	// Token: 0x040038CA RID: 14538
	[SerializeField]
	private Vector3 RotFreq;

	// Token: 0x040038CB RID: 14539
	private Vector3 rootPos;

	// Token: 0x040038CC RID: 14540
	private Vector3 rootRot;

	// Token: 0x040038CD RID: 14541
	private Vector3 offsPos = Vector3.zero;

	// Token: 0x040038CE RID: 14542
	private Vector3 offsRot = Vector3.zero;

	// Token: 0x040038CF RID: 14543
	private DateTime dt;

	// Token: 0x040038D0 RID: 14544
	[SerializeField]
	private bool useServerTime;
}
