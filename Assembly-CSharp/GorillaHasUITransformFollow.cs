using System;
using UnityEngine;

// Token: 0x02000574 RID: 1396
public class GorillaHasUITransformFollow : MonoBehaviour
{
	// Token: 0x06002279 RID: 8825 RVA: 0x000F888C File Offset: 0x000F6A8C
	private void Awake()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(base.gameObject.activeSelf);
		}
	}

	// Token: 0x0600227A RID: 8826 RVA: 0x000F88C8 File Offset: 0x000F6AC8
	private void OnDestroy()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i].gameObject);
		}
	}

	// Token: 0x0600227B RID: 8827 RVA: 0x000F88F8 File Offset: 0x000F6AF8
	private void OnEnable()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(true);
		}
	}

	// Token: 0x0600227C RID: 8828 RVA: 0x000F8928 File Offset: 0x000F6B28
	private void OnDisable()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x040025EE RID: 9710
	public GorillaUITransformFollow[] transformFollowers;
}
