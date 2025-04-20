using System;
using System.Collections.Generic;
using GorillaLocomotion.Swimming;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020007D6 RID: 2006
public class WaterInteractionEvents : MonoBehaviour
{
	// Token: 0x0600318F RID: 12687 RVA: 0x00136028 File Offset: 0x00134228
	private void Update()
	{
		if (this.overlappingWaterVolumes.Count < 1)
		{
			if (this.inWater)
			{
				this.onExitWater.Invoke();
			}
			this.inWater = false;
			base.enabled = false;
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.overlappingWaterVolumes.Count; i++)
		{
			WaterVolume.SurfaceQuery surfaceQuery;
			if (this.overlappingWaterVolumes[i].GetSurfaceQueryForPoint(this.waterContactSphere.transform.position, out surfaceQuery, false))
			{
				float num = Vector3.Dot(surfaceQuery.surfacePoint - this.waterContactSphere.transform.position, surfaceQuery.surfaceNormal);
				float num2 = Vector3.Dot(surfaceQuery.surfacePoint - Vector3.up * surfaceQuery.maxDepth - base.transform.position, surfaceQuery.surfaceNormal);
				if (num > -this.waterContactSphere.radius && num2 < this.waterContactSphere.radius)
				{
					flag = true;
				}
			}
		}
		bool flag2 = this.inWater;
		this.inWater = flag;
		if (!flag2 && this.inWater)
		{
			this.onEnterWater.Invoke();
			return;
		}
		if (flag2 && !this.inWater)
		{
			this.onExitWater.Invoke();
		}
	}

	// Token: 0x06003190 RID: 12688 RVA: 0x00136164 File Offset: 0x00134364
	protected void OnTriggerEnter(Collider other)
	{
		WaterVolume component = other.GetComponent<WaterVolume>();
		if (component != null && !this.overlappingWaterVolumes.Contains(component))
		{
			this.overlappingWaterVolumes.Add(component);
			base.enabled = true;
		}
	}

	// Token: 0x06003191 RID: 12689 RVA: 0x001361A4 File Offset: 0x001343A4
	protected void OnTriggerExit(Collider other)
	{
		WaterVolume component = other.GetComponent<WaterVolume>();
		if (component != null && this.overlappingWaterVolumes.Contains(component))
		{
			this.overlappingWaterVolumes.Remove(component);
		}
	}

	// Token: 0x040035CC RID: 13772
	public UnityEvent onEnterWater = new UnityEvent();

	// Token: 0x040035CD RID: 13773
	public UnityEvent onExitWater = new UnityEvent();

	// Token: 0x040035CE RID: 13774
	[SerializeField]
	private SphereCollider waterContactSphere;

	// Token: 0x040035CF RID: 13775
	private List<WaterVolume> overlappingWaterVolumes = new List<WaterVolume>();

	// Token: 0x040035D0 RID: 13776
	private bool inWater;
}
