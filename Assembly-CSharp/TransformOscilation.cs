using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020008CE RID: 2254
public class TransformOscilation : MonoBehaviour
{
	// Token: 0x0600367D RID: 13949 RVA: 0x00101678 File Offset: 0x000FF878
	private void Start()
	{
		this.rootPos = base.transform.localPosition;
		this.rootRot = base.transform.localRotation.eulerAngles;
	}

	// Token: 0x0600367E RID: 13950 RVA: 0x001016B0 File Offset: 0x000FF8B0
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

	// Token: 0x040038B5 RID: 14517
	[SerializeField]
	private Vector3 PosAmp;

	// Token: 0x040038B6 RID: 14518
	[SerializeField]
	private Vector3 PosFreq;

	// Token: 0x040038B7 RID: 14519
	[SerializeField]
	private Vector3 RotAmp;

	// Token: 0x040038B8 RID: 14520
	[SerializeField]
	private Vector3 RotFreq;

	// Token: 0x040038B9 RID: 14521
	private Vector3 rootPos;

	// Token: 0x040038BA RID: 14522
	private Vector3 rootRot;

	// Token: 0x040038BB RID: 14523
	private Vector3 offsPos = Vector3.zero;

	// Token: 0x040038BC RID: 14524
	private Vector3 offsRot = Vector3.zero;

	// Token: 0x040038BD RID: 14525
	private DateTime dt;

	// Token: 0x040038BE RID: 14526
	[SerializeField]
	private bool useServerTime;
}
