using System;
using UnityEngine;

// Token: 0x020007E0 RID: 2016
public class CopyMaterialScript : MonoBehaviour
{
	// Token: 0x060031EB RID: 12779 RVA: 0x000F0212 File Offset: 0x000EE412
	private void Start()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060031EC RID: 12780 RVA: 0x000F0229 File Offset: 0x000EE429
	private void Update()
	{
		if (this.sourceToCopyMaterialFrom.material != this.mySkinnedMeshRenderer.material)
		{
			this.mySkinnedMeshRenderer.material = this.sourceToCopyMaterialFrom.material;
		}
	}

	// Token: 0x0400357E RID: 13694
	public SkinnedMeshRenderer sourceToCopyMaterialFrom;

	// Token: 0x0400357F RID: 13695
	public SkinnedMeshRenderer mySkinnedMeshRenderer;
}
