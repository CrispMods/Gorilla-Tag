using System;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class CritterVisuals : MonoBehaviour
{
	// Token: 0x17000024 RID: 36
	// (get) Token: 0x0600028C RID: 652 RVA: 0x00010C17 File Offset: 0x0000EE17
	public CritterAppearance Appearance
	{
		get
		{
			return this._appearance;
		}
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00010C20 File Offset: 0x0000EE20
	public void SetAppearance(CritterAppearance appearance)
	{
		this._appearance = appearance;
		this.bodyRoot.localScale = new Vector3(this._appearance.size, this._appearance.size, this._appearance.size);
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

	// Token: 0x0600028E RID: 654 RVA: 0x00010CC8 File Offset: 0x0000EEC8
	public void ApplyMesh(Mesh newMesh)
	{
		this.myMeshFilter.sharedMesh = newMesh;
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00010CD6 File Offset: 0x0000EED6
	public void ApplyMaterial(Material mat)
	{
		this.myRenderer.sharedMaterial = mat;
	}

	// Token: 0x04000337 RID: 823
	public int critterType;

	// Token: 0x04000338 RID: 824
	[Header("Visuals")]
	public Transform bodyRoot;

	// Token: 0x04000339 RID: 825
	public MeshRenderer myRenderer;

	// Token: 0x0400033A RID: 826
	public MeshFilter myMeshFilter;

	// Token: 0x0400033B RID: 827
	public Transform hatRoot;

	// Token: 0x0400033C RID: 828
	public GameObject[] hats;

	// Token: 0x0400033D RID: 829
	private CritterAppearance _appearance;
}
