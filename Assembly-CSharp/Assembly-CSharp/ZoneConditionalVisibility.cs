using System;
using UnityEngine;

// Token: 0x0200021F RID: 543
public class ZoneConditionalVisibility : MonoBehaviour
{
	// Token: 0x06000C93 RID: 3219 RVA: 0x00042CAB File Offset: 0x00040EAB
	private void Start()
	{
		this.OnZoneChanged();
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x00042CD9 File Offset: 0x00040ED9
	private void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x00042D01 File Offset: 0x00040F01
	private void OnZoneChanged()
	{
		if (this.invisibleWhileLoaded)
		{
			base.gameObject.SetActive(!ZoneManagement.IsInZone(this.zone));
			return;
		}
		base.gameObject.SetActive(ZoneManagement.IsInZone(this.zone));
	}

	// Token: 0x04000FF0 RID: 4080
	[SerializeField]
	private GTZone zone;

	// Token: 0x04000FF1 RID: 4081
	[SerializeField]
	private bool invisibleWhileLoaded;
}
