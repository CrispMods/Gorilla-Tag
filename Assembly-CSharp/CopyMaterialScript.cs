using System;
using UnityEngine;

// Token: 0x020007FA RID: 2042
public class CopyMaterialScript : MonoBehaviour
{
	// Token: 0x060032A1 RID: 12961 RVA: 0x00051799 File Offset: 0x0004F999
	private void Start()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060032A2 RID: 12962 RVA: 0x000517B0 File Offset: 0x0004F9B0
	private void Update()
	{
		if (this.sourceToCopyMaterialFrom.material != this.mySkinnedMeshRenderer.material)
		{
			this.mySkinnedMeshRenderer.material = this.sourceToCopyMaterialFrom.material;
		}
	}

	// Token: 0x04003634 RID: 13876
	public SkinnedMeshRenderer sourceToCopyMaterialFrom;

	// Token: 0x04003635 RID: 13877
	public SkinnedMeshRenderer mySkinnedMeshRenderer;
}
