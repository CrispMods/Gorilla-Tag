using System;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class ZoneConditionalComponentEnabling : MonoBehaviour
{
	// Token: 0x06000C8B RID: 3211 RVA: 0x00037CAA File Offset: 0x00035EAA
	private void Start()
	{
		this.OnZoneChanged();
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x00037CD8 File Offset: 0x00035ED8
	private void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x0009E0E0 File Offset: 0x0009C2E0
	private void OnZoneChanged()
	{
		bool flag = ZoneManagement.IsInZone(this.zone);
		bool enabled = this.invisibleWhileLoaded ? (!flag) : flag;
		if (this.components != null)
		{
			for (int i = 0; i < this.components.Length; i++)
			{
				if (this.components[i] != null)
				{
					this.components[i].enabled = enabled;
				}
			}
		}
		if (this.m_renderers != null)
		{
			for (int j = 0; j < this.m_renderers.Length; j++)
			{
				if (this.m_renderers[j] != null)
				{
					this.m_renderers[j].enabled = enabled;
				}
			}
		}
		if (this.m_colliders != null)
		{
			for (int k = 0; k < this.m_colliders.Length; k++)
			{
				if (this.m_colliders[k] != null)
				{
					this.m_colliders[k].enabled = enabled;
				}
			}
		}
	}

	// Token: 0x04000FE8 RID: 4072
	[SerializeField]
	private GTZone zone;

	// Token: 0x04000FE9 RID: 4073
	[SerializeField]
	private bool invisibleWhileLoaded;

	// Token: 0x04000FEA RID: 4074
	[SerializeField]
	private Behaviour[] components;

	// Token: 0x04000FEB RID: 4075
	[SerializeField]
	private Renderer[] m_renderers;

	// Token: 0x04000FEC RID: 4076
	[SerializeField]
	private Collider[] m_colliders;
}
