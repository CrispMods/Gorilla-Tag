using System;
using UnityEngine;

// Token: 0x0200021F RID: 543
public class ZoneConditionalVisibility : MonoBehaviour
{
	// Token: 0x06000C91 RID: 3217 RVA: 0x00042967 File Offset: 0x00040B67
	private void Start()
	{
		this.OnZoneChanged();
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x00042995 File Offset: 0x00040B95
	private void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x000429BD File Offset: 0x00040BBD
	private void OnZoneChanged()
	{
		if (this.invisibleWhileLoaded)
		{
			base.gameObject.SetActive(!ZoneManagement.IsInZone(this.zone));
			return;
		}
		base.gameObject.SetActive(ZoneManagement.IsInZone(this.zone));
	}

	// Token: 0x04000FEF RID: 4079
	[SerializeField]
	private GTZone zone;

	// Token: 0x04000FF0 RID: 4080
	[SerializeField]
	private bool invisibleWhileLoaded;
}
