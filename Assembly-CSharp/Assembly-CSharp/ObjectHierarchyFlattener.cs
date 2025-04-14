using System;
using UnityEngine;

// Token: 0x020001CA RID: 458
public class ObjectHierarchyFlattener : MonoBehaviour
{
	// Token: 0x06000AAC RID: 2732 RVA: 0x0003A2E8 File Offset: 0x000384E8
	private void ResetTransform()
	{
		base.transform.SetParent(this.originalParentTransform);
		base.transform.localPosition = this.originalLocalPosition;
		base.transform.localRotation = this.originalLocalRotation;
		base.transform.localScale = this.originalScale;
		if (this.crumb != null)
		{
			Object.Destroy(this.crumb);
		}
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0003A354 File Offset: 0x00038554
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

	// Token: 0x06000AAE RID: 2734 RVA: 0x0003A430 File Offset: 0x00038630
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

	// Token: 0x06000AAF RID: 2735 RVA: 0x0003A4FA File Offset: 0x000386FA
	private void OnDisable()
	{
		ObjectHierarchyFlattenerManager.UnregisterOHF(this);
		base.Invoke("ResetTransform", 0f);
	}

	// Token: 0x04000D1E RID: 3358
	private GameObject originalParentGO;

	// Token: 0x04000D1F RID: 3359
	private Transform originalParentTransform;

	// Token: 0x04000D20 RID: 3360
	private Vector3 originalLocalPosition;

	// Token: 0x04000D21 RID: 3361
	private Vector3 calcOffset;

	// Token: 0x04000D22 RID: 3362
	private Quaternion originalLocalRotation;

	// Token: 0x04000D23 RID: 3363
	private Vector3 originalScale;

	// Token: 0x04000D24 RID: 3364
	private float originalParentScale;

	// Token: 0x04000D25 RID: 3365
	public bool trackTransformOfParent;

	// Token: 0x04000D26 RID: 3366
	public bool maintainRelativeScale;

	// Token: 0x04000D27 RID: 3367
	private FlattenerCrumb crumb;
}
