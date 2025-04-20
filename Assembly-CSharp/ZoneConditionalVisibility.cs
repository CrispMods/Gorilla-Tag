using System;
using UnityEngine;

// Token: 0x0200022A RID: 554
public class ZoneConditionalVisibility : MonoBehaviour
{
	// Token: 0x06000CDC RID: 3292 RVA: 0x00039016 File Offset: 0x00037216
	private void Start()
	{
		this.OnZoneChanged();
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x00039044 File Offset: 0x00037244
	private void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x0003906C File Offset: 0x0003726C
	private void OnZoneChanged()
	{
		if (this.invisibleWhileLoaded)
		{
			base.gameObject.SetActive(!ZoneManagement.IsInZone(this.zone));
			return;
		}
		base.gameObject.SetActive(ZoneManagement.IsInZone(this.zone));
	}

	// Token: 0x04001035 RID: 4149
	[SerializeField]
	private GTZone zone;

	// Token: 0x04001036 RID: 4150
	[SerializeField]
	private bool invisibleWhileLoaded;
}
