using System;
using UnityEngine;

// Token: 0x02000350 RID: 848
[ExecuteInEditMode]
public class SimpleResizable : MonoBehaviour
{
	// Token: 0x17000239 RID: 569
	// (get) Token: 0x060013B7 RID: 5047 RVA: 0x00060A79 File Offset: 0x0005EC79
	public Vector3 PivotPosition
	{
		get
		{
			return this._pivotTransform.position;
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x060013B8 RID: 5048 RVA: 0x00060A86 File Offset: 0x0005EC86
	// (set) Token: 0x060013B9 RID: 5049 RVA: 0x00060A8E File Offset: 0x0005EC8E
	public Vector3 DefaultSize { get; private set; }

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x060013BA RID: 5050 RVA: 0x00060A97 File Offset: 0x0005EC97
	// (set) Token: 0x060013BB RID: 5051 RVA: 0x00060A9F File Offset: 0x0005EC9F
	public Mesh OriginalMesh { get; private set; }

	// Token: 0x060013BC RID: 5052 RVA: 0x00060AA8 File Offset: 0x0005ECA8
	public void SetNewSize(Vector3 newSize)
	{
		this._newSize = newSize;
	}

	// Token: 0x060013BD RID: 5053 RVA: 0x00060AB4 File Offset: 0x0005ECB4
	private void Awake()
	{
		this._meshFilter = base.GetComponent<MeshFilter>();
		this.OriginalMesh = base.GetComponent<MeshFilter>().sharedMesh;
		this.DefaultSize = this.OriginalMesh.bounds.size;
		this._newSize = this.DefaultSize;
		this._oldSize = this._newSize;
		if (!this._pivotTransform)
		{
			this._pivotTransform = base.transform.Find("Pivot");
		}
	}

	// Token: 0x060013BE RID: 5054 RVA: 0x00060B34 File Offset: 0x0005ED34
	private void OnEnable()
	{
		this.DefaultSize = this.OriginalMesh.bounds.size;
		if (this._newSize == Vector3.zero)
		{
			this._newSize = this.DefaultSize;
		}
	}

	// Token: 0x060013BF RID: 5055 RVA: 0x00060B78 File Offset: 0x0005ED78
	private void Update()
	{
		if (Application.isPlaying && !this._updateInPlayMode)
		{
			return;
		}
		if (this._newSize != this._oldSize)
		{
			this._oldSize = this._newSize;
			Mesh sharedMesh = SimpleResizer.ProcessVertices(this, this._newSize, true);
			this._meshFilter.sharedMesh = sharedMesh;
			this._meshFilter.sharedMesh.RecalculateBounds();
		}
	}

	// Token: 0x060013C0 RID: 5056 RVA: 0x00060BE0 File Offset: 0x0005EDE0
	private void OnDrawGizmos()
	{
		if (!this._pivotTransform)
		{
			return;
		}
		Gizmos.color = Color.red;
		float d = 0.1f;
		Vector3 from = this._pivotTransform.position + Vector3.left * d * 0.5f;
		Vector3 from2 = this._pivotTransform.position + Vector3.down * d * 0.5f;
		Vector3 from3 = this._pivotTransform.position + Vector3.back * d * 0.5f;
		Gizmos.DrawRay(from, Vector3.right * d);
		Gizmos.DrawRay(from2, Vector3.up * d);
		Gizmos.DrawRay(from3, Vector3.forward * d);
	}

	// Token: 0x060013C1 RID: 5057 RVA: 0x00060CB0 File Offset: 0x0005EEB0
	private void OnDrawGizmosSelected()
	{
		if (this._meshFilter.sharedMesh == null)
		{
			return;
		}
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Vector3 center = this._meshFilter.sharedMesh.bounds.center;
		Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
		switch (this.ScalingX)
		{
		case SimpleResizable.Method.Adapt:
			Gizmos.DrawWireCube(center, new Vector3(this._newSize.x * this.PaddingX * 2f, this._newSize.y, this._newSize.z));
			break;
		case SimpleResizable.Method.AdaptWithAsymmetricalPadding:
			Gizmos.DrawWireCube(center + new Vector3(this._newSize.x * this.PaddingX, 0f, 0f), new Vector3(0f, this._newSize.y, this._newSize.z));
			Gizmos.DrawWireCube(center + new Vector3(this._newSize.x * this.PaddingXMax, 0f, 0f), new Vector3(0f, this._newSize.y, this._newSize.z));
			break;
		case SimpleResizable.Method.None:
			Gizmos.DrawWireCube(center, this._newSize);
			break;
		}
		Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
		switch (this.ScalingY)
		{
		case SimpleResizable.Method.Adapt:
			Gizmos.DrawWireCube(center, new Vector3(this._newSize.x, this._newSize.y * this.PaddingY * 2f, this._newSize.z));
			break;
		case SimpleResizable.Method.AdaptWithAsymmetricalPadding:
			Gizmos.DrawWireCube(center + new Vector3(0f, this._newSize.y * this.PaddingY, 0f), new Vector3(this._newSize.x, 0f, this._newSize.z));
			Gizmos.DrawWireCube(center + new Vector3(0f, this._newSize.y * this.PaddingYMax, 0f), new Vector3(this._newSize.x, 0f, this._newSize.z));
			break;
		case SimpleResizable.Method.None:
			Gizmos.DrawWireCube(center, this._newSize);
			break;
		}
		Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
		switch (this.ScalingZ)
		{
		case SimpleResizable.Method.Adapt:
			Gizmos.DrawWireCube(center, new Vector3(this._newSize.x, this._newSize.y, this._newSize.z * this.PaddingZ * 2f));
			break;
		case SimpleResizable.Method.AdaptWithAsymmetricalPadding:
			Gizmos.DrawWireCube(center + new Vector3(0f, 0f, this._newSize.z * this.PaddingZ), new Vector3(this._newSize.x, this._newSize.y, 0f));
			Gizmos.DrawWireCube(center + new Vector3(0f, 0f, this._newSize.z * this.PaddingZMax), new Vector3(this._newSize.x, this._newSize.y, 0f));
			break;
		case SimpleResizable.Method.None:
			Gizmos.DrawWireCube(center, this._newSize);
			break;
		}
		Gizmos.color = new Color(0f, 1f, 1f, 1f);
		Gizmos.DrawWireCube(center, this._newSize);
	}

	// Token: 0x040015DC RID: 5596
	[Space(15f)]
	public SimpleResizable.Method ScalingX;

	// Token: 0x040015DD RID: 5597
	[Range(0f, 0.5f)]
	public float PaddingX;

	// Token: 0x040015DE RID: 5598
	[Range(-0.5f, 0f)]
	public float PaddingXMax;

	// Token: 0x040015DF RID: 5599
	[Space(15f)]
	public SimpleResizable.Method ScalingY;

	// Token: 0x040015E0 RID: 5600
	[Range(0f, 0.5f)]
	public float PaddingY;

	// Token: 0x040015E1 RID: 5601
	[Range(-0.5f, 0f)]
	public float PaddingYMax;

	// Token: 0x040015E2 RID: 5602
	[Space(15f)]
	public SimpleResizable.Method ScalingZ;

	// Token: 0x040015E3 RID: 5603
	[Range(0f, 0.5f)]
	public float PaddingZ;

	// Token: 0x040015E4 RID: 5604
	[Range(-0.5f, 0f)]
	public float PaddingZMax;

	// Token: 0x040015E7 RID: 5607
	private Vector3 _oldSize;

	// Token: 0x040015E8 RID: 5608
	private MeshFilter _meshFilter;

	// Token: 0x040015E9 RID: 5609
	[SerializeField]
	private Vector3 _newSize;

	// Token: 0x040015EA RID: 5610
	[SerializeField]
	private bool _updateInPlayMode;

	// Token: 0x040015EB RID: 5611
	[SerializeField]
	private Transform _pivotTransform;

	// Token: 0x02000351 RID: 849
	public enum Method
	{
		// Token: 0x040015ED RID: 5613
		Adapt,
		// Token: 0x040015EE RID: 5614
		AdaptWithAsymmetricalPadding,
		// Token: 0x040015EF RID: 5615
		Scale,
		// Token: 0x040015F0 RID: 5616
		None
	}
}
