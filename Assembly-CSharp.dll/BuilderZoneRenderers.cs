using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
public class BuilderZoneRenderers : MonoBehaviour
{
	// Token: 0x06001D29 RID: 7465 RVA: 0x000DE548 File Offset: 0x000DC748
	private void Start()
	{
		this.allRenderers.Clear();
		this.allRenderers.AddRange(this.renderers);
		foreach (GameObject gameObject in this.rootObjects)
		{
			this.allRenderers.AddRange(gameObject.GetComponentsInChildren<Renderer>(true));
		}
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
		this.inBuilderZone = true;
		this.OnZoneChanged();
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x00042F3E File Offset: 0x0004113E
	private void OnDestroy()
	{
		if (ZoneManagement.instance != null)
		{
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
		}
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x000DE5F8 File Offset: 0x000DC7F8
	private void OnZoneChanged()
	{
		bool flag = ZoneManagement.instance.IsZoneActive(GTZone.monkeBlocks);
		if (flag && !this.inBuilderZone)
		{
			this.inBuilderZone = flag;
			foreach (Renderer renderer in this.allRenderers)
			{
				renderer.enabled = true;
			}
			using (List<Canvas>.Enumerator enumerator2 = this.canvases.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Canvas canvas = enumerator2.Current;
					canvas.enabled = true;
				}
				return;
			}
		}
		if (!flag && this.inBuilderZone)
		{
			this.inBuilderZone = flag;
			foreach (Renderer renderer2 in this.allRenderers)
			{
				renderer2.enabled = false;
			}
			foreach (Canvas canvas2 in this.canvases)
			{
				canvas2.enabled = false;
			}
		}
	}

	// Token: 0x0400202D RID: 8237
	public List<Renderer> renderers;

	// Token: 0x0400202E RID: 8238
	public List<Canvas> canvases;

	// Token: 0x0400202F RID: 8239
	public List<GameObject> rootObjects;

	// Token: 0x04002030 RID: 8240
	private bool inBuilderZone;

	// Token: 0x04002031 RID: 8241
	private List<Renderer> allRenderers = new List<Renderer>(200);
}
