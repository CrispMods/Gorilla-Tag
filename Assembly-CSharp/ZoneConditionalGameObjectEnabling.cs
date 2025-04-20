using System;
using UnityEngine;

// Token: 0x02000229 RID: 553
public class ZoneConditionalGameObjectEnabling : MonoBehaviour
{
	// Token: 0x06000CD8 RID: 3288 RVA: 0x00038FC0 File Offset: 0x000371C0
	private void Start()
	{
		this.OnZoneChanged();
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x00038FEE File Offset: 0x000371EE
	private void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x000A0A48 File Offset: 0x0009EC48
	private void OnZoneChanged()
	{
		if (this.invisibleWhileLoaded)
		{
			if (this.gameObjects != null)
			{
				for (int i = 0; i < this.gameObjects.Length; i++)
				{
					this.gameObjects[i].SetActive(!ZoneManagement.IsInZone(this.zone));
				}
				return;
			}
		}
		else if (this.gameObjects != null)
		{
			for (int j = 0; j < this.gameObjects.Length; j++)
			{
				this.gameObjects[j].SetActive(ZoneManagement.IsInZone(this.zone));
			}
		}
	}

	// Token: 0x04001032 RID: 4146
	[SerializeField]
	private GTZone zone;

	// Token: 0x04001033 RID: 4147
	[SerializeField]
	private bool invisibleWhileLoaded;

	// Token: 0x04001034 RID: 4148
	[SerializeField]
	private GameObject[] gameObjects;
}
