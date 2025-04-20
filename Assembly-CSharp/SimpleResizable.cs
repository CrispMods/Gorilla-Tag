using System;
using UnityEngine;

// Token: 0x0200035B RID: 859
[ExecuteInEditMode]
public class SimpleResizable : MonoBehaviour
{
	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06001403 RID: 5123 RVA: 0x0003D785 File Offset: 0x0003B985
	public Vector3 PivotPosition
	{
		get
		{
			return this._pivotTransform.position;
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06001404 RID: 5124 RVA: 0x0003D792 File Offset: 0x0003B992
	// (set) Token: 0x06001405 RID: 5125 RVA: 0x0003D79A File Offset: 0x0003B99A
	public Vector3 DefaultSize { get; private set; }

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06001406 RID: 5126 RVA: 0x0003D7A3 File Offset: 0x0003B9A3
	// (set) Token: 0x06001407 RID: 5127 RVA: 0x0003D7AB File Offset: 0x0003B9AB
	public Mesh OriginalMesh { get; private set; }

	// Token: 0x06001408 RID: 5128 RVA: 0x0003D7B4 File Offset: 0x0003B9B4
	public void SetNewSize(Vector3 newSize)
	{
		this._newSize = newSize;
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x000BA3F4 File Offset: 0x000B85F4
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

	// Token: 0x0600140A RID: 5130 RVA: 0x000BA474 File Offset: 0x000B8674
	private void OnEnable()
	{
		this.DefaultSize = this.OriginalMesh.bounds.size;
		if (this._newSize == Vector3.zero)
		{
			this._newSize = this.DefaultSize;
		}
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x000BA4B8 File Offset: 0x000B86B8
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

	// Token: 0x0600140C RID: 5132 RVA: 0x000BA520 File Offset: 0x000B8720
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

	// Token: 0x0600140D RID: 5133 RVA: 0x000BA5F0 File Offset: 0x000B87F0
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

	// Token: 0x04001624 RID: 5668
	[Space(15f)]
	public SimpleResizable.Method ScalingX;

	// Token: 0x04001625 RID: 5669
	[Range(0f, 0.5f)]
	public float PaddingX;

	// Token: 0x04001626 RID: 5670
	[Range(-0.5f, 0f)]
	public float PaddingXMax;

	// Token: 0x04001627 RID: 5671
	[Space(15f)]
	public SimpleResizable.Method ScalingY;

	// Token: 0x04001628 RID: 5672
	[Range(0f, 0.5f)]
	public float PaddingY;

	// Token: 0x04001629 RID: 5673
	[Range(-0.5f, 0f)]
	public float PaddingYMax;

	// Token: 0x0400162A RID: 5674
	[Space(15f)]
	public SimpleResizable.Method ScalingZ;

	// Token: 0x0400162B RID: 5675
	[Range(0f, 0.5f)]
	public float PaddingZ;

	// Token: 0x0400162C RID: 5676
	[Range(-0.5f, 0f)]
	public float PaddingZMax;

	// Token: 0x0400162F RID: 5679
	private Vector3 _oldSize;

	// Token: 0x04001630 RID: 5680
	private MeshFilter _meshFilter;

	// Token: 0x04001631 RID: 5681
	[SerializeField]
	private Vector3 _newSize;

	// Token: 0x04001632 RID: 5682
	[SerializeField]
	private bool _updateInPlayMode;

	// Token: 0x04001633 RID: 5683
	[SerializeField]
	private Transform _pivotTransform;

	// Token: 0x0200035C RID: 860
	public enum Method
	{
		// Token: 0x04001635 RID: 5685
		Adapt,
		// Token: 0x04001636 RID: 5686
		AdaptWithAsymmetricalPadding,
		// Token: 0x04001637 RID: 5687
		Scale,
		// Token: 0x04001638 RID: 5688
		None
	}
}
