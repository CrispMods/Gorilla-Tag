using System;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class DisableOtherObjectsWhileActive : MonoBehaviour
{
	// Token: 0x060009F5 RID: 2549 RVA: 0x00036069 File Offset: 0x00034269
	private void OnEnable()
	{
		this.SetAllActive(false);
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x00036072 File Offset: 0x00034272
	private void OnDisable()
	{
		this.SetAllActive(true);
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x00094880 File Offset: 0x00092A80
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

	// Token: 0x04000C54 RID: 3156
	public GameObject[] otherObjects;

	// Token: 0x04000C55 RID: 3157
	public XSceneRef[] otherXSceneObjects;
}
