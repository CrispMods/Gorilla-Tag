using System;
using UnityEngine;

// Token: 0x02000567 RID: 1383
public class GorillaHasUITransformFollow : MonoBehaviour
{
	// Token: 0x06002223 RID: 8739 RVA: 0x000A8DC4 File Offset: 0x000A6FC4
	private void Awake()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(base.gameObject.activeSelf);
		}
	}

	// Token: 0x06002224 RID: 8740 RVA: 0x000A8E00 File Offset: 0x000A7000
	private void OnDestroy()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			Object.Destroy(array[i].gameObject);
		}
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x000A8E30 File Offset: 0x000A7030
	private void OnEnable()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(true);
		}
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x000A8E60 File Offset: 0x000A7060
	private void OnDisable()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x0400259C RID: 9628
	public GorillaUITransformFollow[] transformFollowers;
}
