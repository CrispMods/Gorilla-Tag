using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008EE RID: 2286
public class ZoneBasedGameObjectActivator : MonoBehaviour
{
	// Token: 0x060036DE RID: 14046 RVA: 0x00053696 File Offset: 0x00051896
	private void OnEnable()
	{
		ZoneManagement.OnZoneChange += this.ZoneManagement_OnZoneChange;
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x000536A9 File Offset: 0x000518A9
	private void OnDisable()
	{
		ZoneManagement.OnZoneChange -= this.ZoneManagement_OnZoneChange;
	}

	// Token: 0x060036E0 RID: 14048 RVA: 0x00143878 File Offset: 0x00141A78
	private void ZoneManagement_OnZoneChange(ZoneData[] zoneData)
	{
		HashSet<GTZone> hashSet = new HashSet<GTZone>(this.zones);
		bool flag = false;
		for (int i = 0; i < zoneData.Length; i++)
		{
			flag |= (zoneData[i].active && hashSet.Contains(zoneData[i].zone));
		}
		for (int j = 0; j < this.gameObjects.Length; j++)
		{
			this.gameObjects[j].SetActive(flag);
		}
	}

	// Token: 0x040039E5 RID: 14821
	[SerializeField]
	private GTZone[] zones;

	// Token: 0x040039E6 RID: 14822
	[SerializeField]
	private GameObject[] gameObjects;
}
