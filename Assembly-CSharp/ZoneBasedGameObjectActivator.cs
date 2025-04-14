using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008EB RID: 2283
public class ZoneBasedGameObjectActivator : MonoBehaviour
{
	// Token: 0x060036D2 RID: 14034 RVA: 0x00103CCA File Offset: 0x00101ECA
	private void OnEnable()
	{
		ZoneManagement.OnZoneChange += this.ZoneManagement_OnZoneChange;
	}

	// Token: 0x060036D3 RID: 14035 RVA: 0x00103CDD File Offset: 0x00101EDD
	private void OnDisable()
	{
		ZoneManagement.OnZoneChange -= this.ZoneManagement_OnZoneChange;
	}

	// Token: 0x060036D4 RID: 14036 RVA: 0x00103CF0 File Offset: 0x00101EF0
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

	// Token: 0x040039D3 RID: 14803
	[SerializeField]
	private GTZone[] zones;

	// Token: 0x040039D4 RID: 14804
	[SerializeField]
	private GameObject[] gameObjects;
}
