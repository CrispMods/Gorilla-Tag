using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000907 RID: 2311
public class ZoneBasedGameObjectActivator : MonoBehaviour
{
	// Token: 0x0600379A RID: 14234 RVA: 0x00054BB3 File Offset: 0x00052DB3
	private void OnEnable()
	{
		ZoneManagement.OnZoneChange += this.ZoneManagement_OnZoneChange;
	}

	// Token: 0x0600379B RID: 14235 RVA: 0x00054BC6 File Offset: 0x00052DC6
	private void OnDisable()
	{
		ZoneManagement.OnZoneChange -= this.ZoneManagement_OnZoneChange;
	}

	// Token: 0x0600379C RID: 14236 RVA: 0x00148E38 File Offset: 0x00147038
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

	// Token: 0x04003A94 RID: 14996
	[SerializeField]
	private GTZone[] zones;

	// Token: 0x04003A95 RID: 14997
	[SerializeField]
	private GameObject[] gameObjects;
}
