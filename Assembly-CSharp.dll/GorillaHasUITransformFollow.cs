using System;
using UnityEngine;

// Token: 0x02000567 RID: 1383
public class GorillaHasUITransformFollow : MonoBehaviour
{
	// Token: 0x06002223 RID: 8739 RVA: 0x000F5B10 File Offset: 0x000F3D10
	private void Awake()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(base.gameObject.activeSelf);
		}
	}

	// Token: 0x06002224 RID: 8740 RVA: 0x000F5B4C File Offset: 0x000F3D4C
	private void OnDestroy()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i].gameObject);
		}
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x000F5B7C File Offset: 0x000F3D7C
	private void OnEnable()
	{
		GorillaUITransformFollow[] array = this.transformFollowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(true);
		}
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x000F5BAC File Offset: 0x000F3DAC
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
