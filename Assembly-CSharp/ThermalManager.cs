using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001FD RID: 509
[DefaultExecutionOrder(-100)]
public class ThermalManager : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06000BEC RID: 3052 RVA: 0x0003F254 File Offset: 0x0003D454
	public void OnEnable()
	{
		if (ThermalManager.instance != null)
		{
			Debug.LogError("ThermalManager already exists!");
			return;
		}
		ThermalManager.instance = this;
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.lastTime = Time.time;
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x0000F86B File Offset: 0x0000DA6B
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x0003F288 File Offset: 0x0003D488
	public void SliceUpdate()
	{
		float num = Time.time - this.lastTime;
		this.lastTime = Time.time;
		for (int i = 0; i < ThermalManager.receivers.Count; i++)
		{
			ThermalReceiver thermalReceiver = ThermalManager.receivers[i];
			Transform transform = thermalReceiver.transform;
			Vector3 position = transform.position;
			float x = transform.lossyScale.x;
			float num2 = 20f;
			for (int j = 0; j < ThermalManager.sources.Count; j++)
			{
				ThermalSourceVolume thermalSourceVolume = ThermalManager.sources[j];
				Transform transform2 = thermalSourceVolume.transform;
				float x2 = transform2.lossyScale.x;
				float num3 = Vector3.Distance(transform2.position, position);
				float num4 = 1f - Mathf.InverseLerp(thermalSourceVolume.innerRadius * x2, thermalSourceVolume.outerRadius * x2, num3 - thermalReceiver.radius * x);
				num2 += thermalSourceVolume.celsius * num4;
			}
			thermalReceiver.celsius = Mathf.Lerp(thermalReceiver.celsius, num2, num * thermalReceiver.conductivity);
		}
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x0003F393 File Offset: 0x0003D593
	public static void Register(ThermalSourceVolume source)
	{
		ThermalManager.sources.Add(source);
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x0003F3A0 File Offset: 0x0003D5A0
	public static void Unregister(ThermalSourceVolume source)
	{
		ThermalManager.sources.Remove(source);
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x0003F3AE File Offset: 0x0003D5AE
	public static void Register(ThermalReceiver receiver)
	{
		ThermalManager.receivers.Add(receiver);
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x0003F3BB File Offset: 0x0003D5BB
	public static void Unregister(ThermalReceiver receiver)
	{
		ThermalManager.receivers.Remove(receiver);
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000E46 RID: 3654
	public static readonly List<ThermalSourceVolume> sources = new List<ThermalSourceVolume>(256);

	// Token: 0x04000E47 RID: 3655
	public static readonly List<ThermalReceiver> receivers = new List<ThermalReceiver>(256);

	// Token: 0x04000E48 RID: 3656
	[NonSerialized]
	public static ThermalManager instance;

	// Token: 0x04000E49 RID: 3657
	private float lastTime;
}
