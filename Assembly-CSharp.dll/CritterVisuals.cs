using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class CritterVisuals : MonoBehaviour
{
	// Token: 0x17000024 RID: 36
	// (get) Token: 0x0600028E RID: 654 RVA: 0x00031038 File Offset: 0x0002F238
	public CritterAppearance Appearance
	{
		get
		{
			return this._appearance;
		}
	}

	// Token: 0x0600028F RID: 655 RVA: 0x0007329C File Offset: 0x0007149C
	public void SetAppearance(CritterAppearance appearance)
	{
		this._appearance = appearance;
		float num = this._appearance.size.ClampSafe(0.25f, 1.5f);
		this.bodyRoot.localScale = new Vector3(num, num, num);
		if (!string.IsNullOrEmpty(appearance.hatName))
		{
			foreach (GameObject gameObject in this.hats)
			{
				gameObject.SetActive(gameObject.name == this._appearance.hatName);
			}
			this.hatRoot.gameObject.SetActive(true);
			return;
		}
		this.hatRoot.gameObject.SetActive(false);
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00031040 File Offset: 0x0002F240
	public void ApplyMesh(Mesh newMesh)
	{
		this.myMeshFilter.sharedMesh = newMesh;
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0003104E File Offset: 0x0002F24E
	public void ApplyMaterial(Material mat)
	{
		this.myRenderer.sharedMaterial = mat;
	}

	// Token: 0x04000338 RID: 824
	public int critterType;

	// Token: 0x04000339 RID: 825
	[Header("Visuals")]
	public Transform bodyRoot;

	// Token: 0x0400033A RID: 826
	public MeshRenderer myRenderer;

	// Token: 0x0400033B RID: 827
	public MeshFilter myMeshFilter;

	// Token: 0x0400033C RID: 828
	public Transform hatRoot;

	// Token: 0x0400033D RID: 829
	public GameObject[] hats;

	// Token: 0x0400033E RID: 830
	private CritterAppearance _appearance;
}
