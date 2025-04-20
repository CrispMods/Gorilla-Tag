using System;
using UnityEngine;

// Token: 0x02000228 RID: 552
public class ZoneConditionalComponentEnabling : MonoBehaviour
{
	// Token: 0x06000CD4 RID: 3284 RVA: 0x00038F6A File Offset: 0x0003716A
	private void Start()
	{
		this.OnZoneChanged();
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000CD5 RID: 3285 RVA: 0x00038F98 File Offset: 0x00037198
	private void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x000A096C File Offset: 0x0009EB6C
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

	// Token: 0x0400102D RID: 4141
	[SerializeField]
	private GTZone zone;

	// Token: 0x0400102E RID: 4142
	[SerializeField]
	private bool invisibleWhileLoaded;

	// Token: 0x0400102F RID: 4143
	[SerializeField]
	private Behaviour[] components;

	// Token: 0x04001030 RID: 4144
	[SerializeField]
	private Renderer[] m_renderers;

	// Token: 0x04001031 RID: 4145
	[SerializeField]
	private Collider[] m_colliders;
}
