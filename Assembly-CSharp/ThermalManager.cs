using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000208 RID: 520
[DefaultExecutionOrder(-100)]
public class ThermalManager : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06000C37 RID: 3127 RVA: 0x0003887D File Offset: 0x00036A7D
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

	// Token: 0x06000C38 RID: 3128 RVA: 0x000320C8 File Offset: 0x000302C8
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x0009DB64 File Offset: 0x0009BD64
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

	// Token: 0x06000C3A RID: 3130 RVA: 0x000388AF File Offset: 0x00036AAF
	public static void Register(ThermalSourceVolume source)
	{
		ThermalManager.sources.Add(source);
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x000388BC File Offset: 0x00036ABC
	public static void Unregister(ThermalSourceVolume source)
	{
		ThermalManager.sources.Remove(source);
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x000388CA File Offset: 0x00036ACA
	public static void Register(ThermalReceiver receiver)
	{
		ThermalManager.receivers.Add(receiver);
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x000388D7 File Offset: 0x00036AD7
	public static void Unregister(ThermalReceiver receiver)
	{
		ThermalManager.receivers.Remove(receiver);
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000E8C RID: 3724
	public static readonly List<ThermalSourceVolume> sources = new List<ThermalSourceVolume>(256);

	// Token: 0x04000E8D RID: 3725
	public static readonly List<ThermalReceiver> receivers = new List<ThermalReceiver>(256);

	// Token: 0x04000E8E RID: 3726
	[NonSerialized]
	public static ThermalManager instance;

	// Token: 0x04000E8F RID: 3727
	private float lastTime;
}
