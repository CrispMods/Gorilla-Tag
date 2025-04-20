using System;
using System.Collections.Generic;
using GorillaTag.Rendering;
using UnityEngine;

// Token: 0x02000874 RID: 2164
[RequireComponent(typeof(MeshRenderer))]
public class MaterialUVOffsetListSetter : MonoBehaviour, IBuildValidation
{
	// Token: 0x0600349D RID: 13469 RVA: 0x00052AC4 File Offset: 0x00050CC4
	private void Awake()
	{
		this.matPropertyBlock = new MaterialPropertyBlock();
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.meshRenderer.GetPropertyBlock(this.matPropertyBlock);
	}

	// Token: 0x0600349E RID: 13470 RVA: 0x0013E45C File Offset: 0x0013C65C
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

	// Token: 0x0600349F RID: 13471 RVA: 0x0013E500 File Offset: 0x0013C700
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

	// Token: 0x040037C6 RID: 14278
	[SerializeField]
	private List<Vector2> uvOffsetList = new List<Vector2>();

	// Token: 0x040037C7 RID: 14279
	private MeshRenderer meshRenderer;

	// Token: 0x040037C8 RID: 14280
	private MaterialPropertyBlock matPropertyBlock;

	// Token: 0x040037C9 RID: 14281
	private int shaderPropertyID = Shader.PropertyToID("_BaseMap_ST");
}
