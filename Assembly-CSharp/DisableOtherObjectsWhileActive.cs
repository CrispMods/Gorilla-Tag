using System;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class DisableOtherObjectsWhileActive : MonoBehaviour
{
	// Token: 0x060009F3 RID: 2547 RVA: 0x000372D6 File Offset: 0x000354D6
	private void OnEnable()
	{
		this.SetAllActive(false);
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x000372DF File Offset: 0x000354DF
	private void OnDisable()
	{
		this.SetAllActive(true);
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x000372E8 File Offset: 0x000354E8
	private void SetAllActive(bool active)
	{
		foreach (GameObject gameObject in this.otherObjects)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(active);
			}
		}
		foreach (XSceneRef xsceneRef in this.otherXSceneObjects)
		{
			GameObject gameObject2;
			if (xsceneRef.TryResolve(out gameObject2))
			{
				gameObject2.SetActive(active);
			}
		}
	}

	// Token: 0x04000C53 RID: 3155
	public GameObject[] otherObjects;

	// Token: 0x04000C54 RID: 3156
	public XSceneRef[] otherXSceneObjects;
}
