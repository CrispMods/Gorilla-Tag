using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006CA RID: 1738
public class ObjectGroup : MonoBehaviour
{
	// Token: 0x06002B0B RID: 11019 RVA: 0x000D5764 File Offset: 0x000D3964
	private void OnEnable()
	{
		if (this.syncWithGroupState)
		{
			this.SetObjectStates(true);
		}
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x000D5775 File Offset: 0x000D3975
	private void OnDisable()
	{
		if (this.syncWithGroupState)
		{
			this.SetObjectStates(false);
		}
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x000D5788 File Offset: 0x000D3988
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

	// Token: 0x04003091 RID: 12433
	public List<GameObject> gameObjects = new List<GameObject>(16);

	// Token: 0x04003092 RID: 12434
	public List<Behaviour> behaviours = new List<Behaviour>(16);

	// Token: 0x04003093 RID: 12435
	public List<Renderer> renderers = new List<Renderer>(16);

	// Token: 0x04003094 RID: 12436
	public List<Collider> colliders = new List<Collider>(16);

	// Token: 0x04003095 RID: 12437
	public bool syncWithGroupState = true;
}
