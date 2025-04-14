using System;
using UnityEngine;

// Token: 0x0200021E RID: 542
public class ZoneConditionalGameObjectEnabling : MonoBehaviour
{
	// Token: 0x06000C8F RID: 3215 RVA: 0x00042BD6 File Offset: 0x00040DD6
	private void Start()
	{
		this.OnZoneChanged();
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x00042C04 File Offset: 0x00040E04
	private void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x00042C2C File Offset: 0x00040E2C
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

	// Token: 0x04000FED RID: 4077
	[SerializeField]
	private GTZone zone;

	// Token: 0x04000FEE RID: 4078
	[SerializeField]
	private bool invisibleWhileLoaded;

	// Token: 0x04000FEF RID: 4079
	[SerializeField]
	private GameObject[] gameObjects;
}
