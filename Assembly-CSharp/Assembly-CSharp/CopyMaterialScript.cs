using System;
using UnityEngine;

// Token: 0x020007E3 RID: 2019
public class CopyMaterialScript : MonoBehaviour
{
	// Token: 0x060031F7 RID: 12791 RVA: 0x000F07DA File Offset: 0x000EE9DA
	private void Start()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x000F07F1 File Offset: 0x000EE9F1
	private void Update()
	{
		if (this.sourceToCopyMaterialFrom.material != this.mySkinnedMeshRenderer.material)
		{
			this.mySkinnedMeshRenderer.material = this.sourceToCopyMaterialFrom.material;
		}
	}

	// Token: 0x04003590 RID: 13712
	public SkinnedMeshRenderer sourceToCopyMaterialFrom;

	// Token: 0x04003591 RID: 13713
	public SkinnedMeshRenderer mySkinnedMeshRenderer;
}
