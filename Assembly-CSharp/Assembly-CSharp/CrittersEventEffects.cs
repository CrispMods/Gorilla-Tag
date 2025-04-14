using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class CrittersEventEffects : MonoBehaviour
{
	// Token: 0x06000153 RID: 339 RVA: 0x00009184 File Offset: 0x00007384
	private void Awake()
	{
		if (this.manager == null)
		{
			GTDev.LogError<string>("CrittersEventEffects missing reference to CrittersManager", null);
			return;
		}
		this.effectResponse = new Dictionary<CrittersManager.CritterEvent, GameObject>();
		for (int i = 0; i < this.eventEffects.Length; i++)
		{
			if (this.eventEffects[i].effect != null)
			{
				this.effectResponse.Add(this.eventEffects[i].eventType, this.eventEffects[i].effect);
			}
		}
		this.manager.OnCritterEventReceived += this.HandleReceivedEvent;
	}

	// Token: 0x06000154 RID: 340 RVA: 0x0000921C File Offset: 0x0000741C
	private void HandleReceivedEvent(CrittersManager.CritterEvent eventType, int sourceActor, Vector3 position, Quaternion rotation)
	{
		GameObject prefab;
		if (this.effectResponse.TryGetValue(eventType, out prefab))
		{
			GameObject pooled = CrittersPool.GetPooled(prefab);
			if (pooled.IsNotNull())
			{
				pooled.transform.position = position;
				pooled.transform.rotation = rotation;
			}
		}
	}

	// Token: 0x0400019F RID: 415
	public CrittersManager manager;

	// Token: 0x040001A0 RID: 416
	public CrittersEventEffects.CrittersEventResponse[] eventEffects;

	// Token: 0x040001A1 RID: 417
	private Dictionary<CrittersManager.CritterEvent, GameObject> effectResponse;

	// Token: 0x02000045 RID: 69
	[Serializable]
	public class CrittersEventResponse
	{
		// Token: 0x040001A2 RID: 418
		public CrittersManager.CritterEvent eventType;

		// Token: 0x040001A3 RID: 419
		public GameObject effect;
	}
}
