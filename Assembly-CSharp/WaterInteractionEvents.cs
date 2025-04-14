﻿using System;
using System.Collections.Generic;
using GorillaLocomotion.Swimming;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020007BE RID: 1982
public class WaterInteractionEvents : MonoBehaviour
{
	// Token: 0x060030DD RID: 12509 RVA: 0x000ED340 File Offset: 0x000EB540
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

	// Token: 0x060030DE RID: 12510 RVA: 0x000ED47C File Offset: 0x000EB67C
	protected void OnTriggerEnter(Collider other)
	{
		WaterVolume component = other.GetComponent<WaterVolume>();
		if (component != null && !this.overlappingWaterVolumes.Contains(component))
		{
			this.overlappingWaterVolumes.Add(component);
			base.enabled = true;
		}
	}

	// Token: 0x060030DF RID: 12511 RVA: 0x000ED4BC File Offset: 0x000EB6BC
	protected void OnTriggerExit(Collider other)
	{
		WaterVolume component = other.GetComponent<WaterVolume>();
		if (component != null && this.overlappingWaterVolumes.Contains(component))
		{
			this.overlappingWaterVolumes.Remove(component);
		}
	}

	// Token: 0x04003522 RID: 13602
	public UnityEvent onEnterWater = new UnityEvent();

	// Token: 0x04003523 RID: 13603
	public UnityEvent onExitWater = new UnityEvent();

	// Token: 0x04003524 RID: 13604
	[SerializeField]
	private SphereCollider waterContactSphere;

	// Token: 0x04003525 RID: 13605
	private List<WaterVolume> overlappingWaterVolumes = new List<WaterVolume>();

	// Token: 0x04003526 RID: 13606
	private bool inWater;
}
