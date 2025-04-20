using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004C1 RID: 1217
public class BuilderZoneRenderers : MonoBehaviour
{
	// Token: 0x06001D7F RID: 7551 RVA: 0x000E1284 File Offset: 0x000DF484
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

	// Token: 0x06001D80 RID: 7552 RVA: 0x000442DD File Offset: 0x000424DD
	private void OnDestroy()
	{
		if (ZoneManagement.instance != null)
		{
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
		}
	}

	// Token: 0x06001D81 RID: 7553 RVA: 0x000E1334 File Offset: 0x000DF534
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

	// Token: 0x0400207F RID: 8319
	public List<Renderer> renderers;

	// Token: 0x04002080 RID: 8320
	public List<Canvas> canvases;

	// Token: 0x04002081 RID: 8321
	public List<GameObject> rootObjects;

	// Token: 0x04002082 RID: 8322
	private bool inBuilderZone;

	// Token: 0x04002083 RID: 8323
	private List<Renderer> allRenderers = new List<Renderer>(200);
}
