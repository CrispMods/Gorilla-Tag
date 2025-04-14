using System;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class ZoneConditionalComponentEnabling : MonoBehaviour
{
	// Token: 0x06000C89 RID: 3209 RVA: 0x0004275F File Offset: 0x0004095F
	private void Start()
	{
		this.OnZoneChanged();
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x0004278D File Offset: 0x0004098D
	private void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x000427B8 File Offset: 0x000409B8
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

	// Token: 0x04000FE7 RID: 4071
	[SerializeField]
	private GTZone zone;

	// Token: 0x04000FE8 RID: 4072
	[SerializeField]
	private bool invisibleWhileLoaded;

	// Token: 0x04000FE9 RID: 4073
	[SerializeField]
	private Behaviour[] components;

	// Token: 0x04000FEA RID: 4074
	[SerializeField]
	private Renderer[] m_renderers;

	// Token: 0x04000FEB RID: 4075
	[SerializeField]
	private Collider[] m_colliders;
}
