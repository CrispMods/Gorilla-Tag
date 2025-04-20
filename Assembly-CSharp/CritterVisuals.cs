using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200006C RID: 108
public class CritterVisuals : MonoBehaviour
{
	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060002BA RID: 698 RVA: 0x000321A2 File Offset: 0x000303A2
	public CritterAppearance Appearance
	{
		get
		{
			return this._appearance;
		}
	}

	// Token: 0x060002BB RID: 699 RVA: 0x000758E8 File Offset: 0x00073AE8
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

	// Token: 0x060002BC RID: 700 RVA: 0x000321AA File Offset: 0x000303AA
	public void ApplyMesh(Mesh newMesh)
	{
		this.myMeshFilter.sharedMesh = newMesh;
	}

	// Token: 0x060002BD RID: 701 RVA: 0x000321B8 File Offset: 0x000303B8
	public void ApplyMaterial(Material mat)
	{
		this.myRenderer.sharedMaterial = mat;
	}

	// Token: 0x04000369 RID: 873
	public int critterType;

	// Token: 0x0400036A RID: 874
	[Header("Visuals")]
	public Transform bodyRoot;

	// Token: 0x0400036B RID: 875
	public MeshRenderer myRenderer;

	// Token: 0x0400036C RID: 876
	public MeshFilter myMeshFilter;

	// Token: 0x0400036D RID: 877
	public Transform hatRoot;

	// Token: 0x0400036E RID: 878
	public GameObject[] hats;

	// Token: 0x0400036F RID: 879
	private CritterAppearance _appearance;
}
