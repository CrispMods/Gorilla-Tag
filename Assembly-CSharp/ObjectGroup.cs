using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006C9 RID: 1737
public class ObjectGroup : MonoBehaviour
{
	// Token: 0x06002B03 RID: 11011 RVA: 0x000D52E4 File Offset: 0x000D34E4
	private void OnEnable()
	{
		if (this.syncWithGroupState)
		{
			this.SetObjectStates(true);
		}
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x000D52F5 File Offset: 0x000D34F5
	private void OnDisable()
	{
		if (this.syncWithGroupState)
		{
			this.SetObjectStates(false);
		}
	}

	// Token: 0x06002B05 RID: 11013 RVA: 0x000D5308 File Offset: 0x000D3508
	public void SetObjectStates(bool active)
	{
		int count = this.gameObjects.Count;
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = this.gameObjects[i];
			if (!(gameObject == null))
			{
				gameObject.SetActive(active);
			}
		}
		int count2 = this.behaviours.Count;
		for (int j = 0; j < count2; j++)
		{
			Behaviour behaviour = this.behaviours[j];
			if (!(behaviour == null))
			{
				behaviour.enabled = active;
			}
		}
		int count3 = this.renderers.Count;
		for (int k = 0; k < count3; k++)
		{
			Renderer renderer = this.renderers[k];
			if (!(renderer == null))
			{
				renderer.enabled = active;
			}
		}
		int count4 = this.colliders.Count;
		for (int l = 0; l < count4; l++)
		{
			Collider collider = this.colliders[l];
			if (!(collider == null))
			{
				collider.enabled = active;
			}
		}
	}

	// Token: 0x0400308B RID: 12427
	public List<GameObject> gameObjects = new List<GameObject>(16);

	// Token: 0x0400308C RID: 12428
	public List<Behaviour> behaviours = new List<Behaviour>(16);

	// Token: 0x0400308D RID: 12429
	public List<Renderer> renderers = new List<Renderer>(16);

	// Token: 0x0400308E RID: 12430
	public List<Collider> colliders = new List<Collider>(16);

	// Token: 0x0400308F RID: 12431
	public bool syncWithGroupState = true;
}
