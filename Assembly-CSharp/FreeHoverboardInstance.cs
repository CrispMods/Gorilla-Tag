using System;
using UnityEngine;

// Token: 0x020005CC RID: 1484
public class FreeHoverboardInstance : MonoBehaviour
{
	// Token: 0x170003BE RID: 958
	// (get) Token: 0x060024E1 RID: 9441 RVA: 0x00049011 File Offset: 0x00047211
	// (set) Token: 0x060024E2 RID: 9442 RVA: 0x00049019 File Offset: 0x00047219
	public Rigidbody Rigidbody { get; private set; }

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x060024E3 RID: 9443 RVA: 0x00049022 File Offset: 0x00047222
	// (set) Token: 0x060024E4 RID: 9444 RVA: 0x0004902A File Offset: 0x0004722A
	public Color boardColor { get; private set; }

	// Token: 0x060024E5 RID: 9445 RVA: 0x001045A0 File Offset: 0x001027A0
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		Material[] sharedMaterials = this.boardMesh.sharedMaterials;
		this.colorMaterial = new Material(sharedMaterials[1]);
		sharedMaterials[1] = this.colorMaterial;
		this.boardMesh.sharedMaterials = sharedMaterials;
	}

	// Token: 0x060024E6 RID: 9446 RVA: 0x00049033 File Offset: 0x00047233
	public void SetColor(Color col)
	{
		this.colorMaterial.color = col;
		this.boardColor = col;
	}

	// Token: 0x060024E7 RID: 9447 RVA: 0x001045E8 File Offset: 0x001027E8
	private void Update()
	{
		RaycastHit raycastHit;
		if (Physics.SphereCast(new Ray(base.transform.TransformPoint(this.sphereCastCenter), base.transform.TransformVector(Vector3.down)), this.sphereCastRadius, out raycastHit, 1f, this.hoverRaycastMask.value))
		{
			this.hasHoverPoint = true;
			this.hoverPoint = raycastHit.point;
			this.hoverNormal = raycastHit.normal;
			return;
		}
		this.hasHoverPoint = false;
	}

	// Token: 0x060024E8 RID: 9448 RVA: 0x00104664 File Offset: 0x00102864
	private void FixedUpdate()
	{
		if (this.hasHoverPoint)
		{
			float num = Vector3.Dot(base.transform.TransformPoint(this.sphereCastCenter) - this.hoverPoint, this.hoverNormal);
			if (num < this.hoverHeight)
			{
				base.transform.position += this.hoverNormal * (this.hoverHeight - num);
				this.Rigidbody.velocity = Vector3.ProjectOnPlane(this.Rigidbody.velocity, this.hoverNormal);
				Vector3 point = Quaternion.Inverse(base.transform.rotation) * this.Rigidbody.angularVelocity;
				point.x *= this.avelocityDragWhileHovering;
				point.z *= this.avelocityDragWhileHovering;
				this.Rigidbody.angularVelocity = base.transform.rotation * point;
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(base.transform.forward, this.hoverNormal), this.hoverNormal), this.hoverRotationLerp);
			}
		}
	}

	// Token: 0x04002907 RID: 10503
	public int ownerActorNumber;

	// Token: 0x04002908 RID: 10504
	public int boardIndex;

	// Token: 0x04002909 RID: 10505
	[SerializeField]
	private Vector3 sphereCastCenter;

	// Token: 0x0400290A RID: 10506
	[SerializeField]
	private float sphereCastRadius;

	// Token: 0x0400290B RID: 10507
	[SerializeField]
	private LayerMask hoverRaycastMask;

	// Token: 0x0400290C RID: 10508
	[SerializeField]
	private float hoverHeight;

	// Token: 0x0400290D RID: 10509
	[SerializeField]
	private float hoverRotationLerp;

	// Token: 0x0400290E RID: 10510
	[SerializeField]
	private float avelocityDragWhileHovering;

	// Token: 0x0400290F RID: 10511
	[SerializeField]
	private MeshRenderer boardMesh;

	// Token: 0x04002911 RID: 10513
	private Material colorMaterial;

	// Token: 0x04002912 RID: 10514
	private bool hasHoverPoint;

	// Token: 0x04002913 RID: 10515
	private Vector3 hoverPoint;

	// Token: 0x04002914 RID: 10516
	private Vector3 hoverNormal;
}
