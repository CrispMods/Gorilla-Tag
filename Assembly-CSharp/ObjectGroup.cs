using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006DE RID: 1758
public class ObjectGroup : MonoBehaviour
{
	// Token: 0x06002B99 RID: 11161 RVA: 0x0004D848 File Offset: 0x0004BA48
	private void OnEnable()
	{
		if (this.syncWithGroupState)
		{
			this.SetObjectStates(true);
		}
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x0004D859 File Offset: 0x0004BA59
	private void OnDisable()
	{
		if (this.syncWithGroupState)
		{
			this.SetObjectStates(false);
		}
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x00120EC4 File Offset: 0x0011F0C4
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

	// Token: 0x04003128 RID: 12584
	public List<GameObject> gameObjects = new List<GameObject>(16);

	// Token: 0x04003129 RID: 12585
	public List<Behaviour> behaviours = new List<Behaviour>(16);

	// Token: 0x0400312A RID: 12586
	public List<Renderer> renderers = new List<Renderer>(16);

	// Token: 0x0400312B RID: 12587
	public List<Collider> colliders = new List<Collider>(16);

	// Token: 0x0400312C RID: 12588
	public bool syncWithGroupState = true;
}
