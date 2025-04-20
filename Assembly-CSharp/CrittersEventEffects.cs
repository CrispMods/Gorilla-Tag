using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class CrittersEventEffects : MonoBehaviour
{
	// Token: 0x0600016B RID: 363 RVA: 0x0006E3EC File Offset: 0x0006C5EC
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

	// Token: 0x0600016C RID: 364 RVA: 0x0006E484 File Offset: 0x0006C684
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

	// Token: 0x040001C4 RID: 452
	public CrittersManager manager;

	// Token: 0x040001C5 RID: 453
	public CrittersEventEffects.CrittersEventResponse[] eventEffects;

	// Token: 0x040001C6 RID: 454
	private Dictionary<CrittersManager.CritterEvent, GameObject> effectResponse;

	// Token: 0x0200004A RID: 74
	[Serializable]
	public class CrittersEventResponse
	{
		// Token: 0x040001C7 RID: 455
		public CrittersManager.CritterEvent eventType;

		// Token: 0x040001C8 RID: 456
		public GameObject effect;
	}
}
