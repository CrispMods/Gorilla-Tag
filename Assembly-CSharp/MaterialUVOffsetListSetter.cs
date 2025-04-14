using System;
using System.Collections.Generic;
using GorillaTag.Rendering;
using UnityEngine;

// Token: 0x02000858 RID: 2136
[RequireComponent(typeof(MeshRenderer))]
public class MaterialUVOffsetListSetter : MonoBehaviour, IBuildValidation
{
	// Token: 0x060033D3 RID: 13267 RVA: 0x000F6EA4 File Offset: 0x000F50A4
	private void Awake()
	{
		this.matPropertyBlock = new MaterialPropertyBlock();
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.meshRenderer.GetPropertyBlock(this.matPropertyBlock);
	}

	// Token: 0x060033D4 RID: 13268 RVA: 0x000F6ED0 File Offset: 0x000F50D0
	public void SetUVOffset(int listIndex)
	{
		if (listIndex >= this.uvOffsetList.Count || listIndex < 0)
		{
			Debug.LogError("Invalid uv offset list index provided.");
			return;
		}
		if (this.matPropertyBlock == null || this.meshRenderer == null)
		{
			Debug.LogError("MaterialUVOffsetListSetter settings are incorrect somehow, please fix", base.gameObject);
			this.Awake();
			return;
		}
		Vector2 vector = this.uvOffsetList[listIndex];
		this.matPropertyBlock.SetVector(this.shaderPropertyID, new Vector4(1f, 1f, vector.x, vector.y));
		this.meshRenderer.SetPropertyBlock(this.matPropertyBlock);
	}

	// Token: 0x060033D5 RID: 13269 RVA: 0x000F6F74 File Offset: 0x000F5174
	public bool BuildValidationCheck()
	{
		if (base.GetComponent<MeshRenderer>() == null)
		{
			Debug.LogError("missing a mesh renderer for the materialuvoffsetlistsetter", base.gameObject);
			return false;
		}
		if (base.GetComponentInParent<EdMeshCombinerMono>() != null && base.GetComponentInParent<EdDoNotMeshCombine>() == null)
		{
			Debug.LogError("the meshrenderer is going to getcombined, that will likely cause issues for the materialuvoffsetlistsetter", base.gameObject);
			return false;
		}
		return true;
	}

	// Token: 0x04003706 RID: 14086
	[SerializeField]
	private List<Vector2> uvOffsetList = new List<Vector2>();

	// Token: 0x04003707 RID: 14087
	private MeshRenderer meshRenderer;

	// Token: 0x04003708 RID: 14088
	private MaterialPropertyBlock matPropertyBlock;

	// Token: 0x04003709 RID: 14089
	private int shaderPropertyID = Shader.PropertyToID("_BaseMap_ST");
}
