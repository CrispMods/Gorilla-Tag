using System;
using System.Collections.Generic;
using GorillaLocomotion.Swimming;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020007BF RID: 1983
public class WaterInteractionEvents : MonoBehaviour
{
	// Token: 0x060030E5 RID: 12517 RVA: 0x00130E08 File Offset: 0x0012F008
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

	// Token: 0x060030E6 RID: 12518 RVA: 0x00130F44 File Offset: 0x0012F144
	protected void OnTriggerEnter(Collider other)
	{
		WaterVolume component = other.GetComponent<WaterVolume>();
		if (component != null && !this.overlappingWaterVolumes.Contains(component))
		{
			this.overlappingWaterVolumes.Add(component);
			base.enabled = true;
		}
	}

	// Token: 0x060030E7 RID: 12519 RVA: 0x00130F84 File Offset: 0x0012F184
	protected void OnTriggerExit(Collider other)
	{
		WaterVolume component = other.GetComponent<WaterVolume>();
		if (component != null && this.overlappingWaterVolumes.Contains(component))
		{
			this.overlappingWaterVolumes.Remove(component);
		}
	}

	// Token: 0x04003528 RID: 13608
	public UnityEvent onEnterWater = new UnityEvent();

	// Token: 0x04003529 RID: 13609
	public UnityEvent onExitWater = new UnityEvent();

	// Token: 0x0400352A RID: 13610
	[SerializeField]
	private SphereCollider waterContactSphere;

	// Token: 0x0400352B RID: 13611
	private List<WaterVolume> overlappingWaterVolumes = new List<WaterVolume>();

	// Token: 0x0400352C RID: 13612
	private bool inWater;
}
