using System;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class ObjectHierarchyFlattener : MonoBehaviour
{
	// Token: 0x06000AF6 RID: 2806 RVA: 0x00099614 File Offset: 0x00097814
	private void ResetTransform()
	{
		base.transform.SetParent(this.originalParentTransform);
		base.transform.localPosition = this.originalLocalPosition;
		base.transform.localRotation = this.originalLocalRotation;
		base.transform.localScale = this.originalScale;
		if (this.crumb != null)
		{
			UnityEngine.Object.Destroy(this.crumb);
		}
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x00099680 File Offset: 0x00097880
	public void InvokeLateUpdate()
	{
		if (!this.originalParentGO.activeInHierarchy)
		{
			ObjectHierarchyFlattenerManager.UnregisterOHF(this);
			base.Invoke("ResetTransform", 0f);
			return;
		}
		if (!this.trackTransformOfParent)
		{
			return;
		}
		if (this.maintainRelativeScale)
		{
			base.transform.localScale = Vector3.Scale(this.originalParentTransform.lossyScale, this.originalScale);
		}
		base.transform.rotation = this.originalParentTransform.rotation * this.originalLocalRotation;
		base.transform.position = this.originalParentTransform.position + base.transform.rotation * this.calcOffset * (this.originalParentTransform.lossyScale.x / this.originalParentScale) * this.originalParentScale;
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0009975C File Offset: 0x0009795C
	private void OnEnable()
	{
		ObjectHierarchyFlattenerManager.RegisterOHF(this);
		this.originalParentTransform = base.transform.parent;
		this.originalParentGO = this.originalParentTransform.gameObject;
		this.originalLocalPosition = base.transform.localPosition;
		this.originalLocalRotation = base.transform.localRotation;
		this.originalParentScale = base.transform.parent.lossyScale.x;
		this.originalScale = base.transform.localScale;
		this.calcOffset = Vector3.Scale(this.originalLocalPosition, this.originalScale);
		if (this.originalParentGO.GetComponent<FlattenerCrumb>() == null)
		{
			this.crumb = this.originalParentGO.AddComponent<FlattenerCrumb>();
		}
		base.transform.SetParent(null);
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x00037B7F File Offset: 0x00035D7F
	private void OnDisable()
	{
		ObjectHierarchyFlattenerManager.UnregisterOHF(this);
		base.Invoke("ResetTransform", 0f);
	}

	// Token: 0x04000D63 RID: 3427
	private GameObject originalParentGO;

	// Token: 0x04000D64 RID: 3428
	private Transform originalParentTransform;

	// Token: 0x04000D65 RID: 3429
	private Vector3 originalLocalPosition;

	// Token: 0x04000D66 RID: 3430
	private Vector3 calcOffset;

	// Token: 0x04000D67 RID: 3431
	private Quaternion originalLocalRotation;

	// Token: 0x04000D68 RID: 3432
	private Vector3 originalScale;

	// Token: 0x04000D69 RID: 3433
	private float originalParentScale;

	// Token: 0x04000D6A RID: 3434
	public bool trackTransformOfParent;

	// Token: 0x04000D6B RID: 3435
	public bool maintainRelativeScale;

	// Token: 0x04000D6C RID: 3436
	private FlattenerCrumb crumb;
}
