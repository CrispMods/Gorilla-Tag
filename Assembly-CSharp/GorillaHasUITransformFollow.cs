using System;
using UnityEngine;

// Token: 0x02000566 RID: 1382
public class GorillaHasUITransformFollow : MonoBehaviour
{
	// Token: 0x0600221B RID: 8731 RVA: 0x000A8944 File Offset: 0x000A6B44
	private void Awake()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(base.gameObject.activeSelf);
		}
	}

	// Token: 0x0600221C RID: 8732 RVA: 0x000A8980 File Offset: 0x000A6B80
	private void OnDestroy()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			Object.Destroy(array[i].gameObject);
		}
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x000A89B0 File Offset: 0x000A6BB0
	private void OnEnable()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(true);
		}
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x000A89E0 File Offset: 0x000A6BE0
	private void OnDisable()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x04002596 RID: 9622
	public GorillaUITransformFollow[] transformFollowers;
}
