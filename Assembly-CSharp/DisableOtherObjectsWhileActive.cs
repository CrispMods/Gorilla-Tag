using System;
using UnityEngine;

// Token: 0x020001AC RID: 428
public class DisableOtherObjectsWhileActive : MonoBehaviour
{
	// Token: 0x06000A3F RID: 2623 RVA: 0x00037329 File Offset: 0x00035529
	private void OnEnable()
	{
		this.SetAllActive(false);
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x00037332 File Offset: 0x00035532
	private void OnDisable()
	{
		this.SetAllActive(true);
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x00097174 File Offset: 0x00095374
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

	// Token: 0x04000C99 RID: 3225
	public GameObject[] otherObjects;

	// Token: 0x04000C9A RID: 3226
	public XSceneRef[] otherXSceneObjects;
}
