using System;
using UnityEngine;

// Token: 0x020005BF RID: 1471
public class FreeHoverboardInstance : MonoBehaviour
{
	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06002487 RID: 9351 RVA: 0x000B61EC File Offset: 0x000B43EC
	// (set) Token: 0x06002488 RID: 9352 RVA: 0x000B61F4 File Offset: 0x000B43F4
	public Rigidbody Rigidbody { get; private set; }

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06002489 RID: 9353 RVA: 0x000B61FD File Offset: 0x000B43FD
	// (set) Token: 0x0600248A RID: 9354 RVA: 0x000B6205 File Offset: 0x000B4405
	public Color boardColor { get; private set; }

	// Token: 0x0600248B RID: 9355 RVA: 0x000B6210 File Offset: 0x000B4410
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		Material[] sharedMaterials = this.boardMesh.sharedMaterials;
		this.colorMaterial = new Material(sharedMaterials[1]);
		sharedMaterials[1] = this.colorMaterial;
		this.boardMesh.sharedMaterials = sharedMaterials;
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x000B6258 File Offset: 0x000B4458
	public void SetColor(Color col)
	{
		this.colorMaterial.color = col;
		this.boardColor = col;
	}

	// Token: 0x0600248D RID: 9357 RVA: 0x000B6270 File Offset: 0x000B4470
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

	// Token: 0x0600248E RID: 9358 RVA: 0x000B62EC File Offset: 0x000B44EC
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

	// Token: 0x040028AE RID: 10414
	public int ownerActorNumber;

	// Token: 0x040028AF RID: 10415
	public int boardIndex;

	// Token: 0x040028B0 RID: 10416
	[SerializeField]
	private Vector3 sphereCastCenter;

	// Token: 0x040028B1 RID: 10417
	[SerializeField]
	private float sphereCastRadius;

	// Token: 0x040028B2 RID: 10418
	[SerializeField]
	private LayerMask hoverRaycastMask;

	// Token: 0x040028B3 RID: 10419
	[SerializeField]
	private float hoverHeight;

	// Token: 0x040028B4 RID: 10420
	[SerializeField]
	private float hoverRotationLerp;

	// Token: 0x040028B5 RID: 10421
	[SerializeField]
	private float avelocityDragWhileHovering;

	// Token: 0x040028B6 RID: 10422
	[SerializeField]
	private MeshRenderer boardMesh;

	// Token: 0x040028B8 RID: 10424
	private Material colorMaterial;

	// Token: 0x040028B9 RID: 10425
	private bool hasHoverPoint;

	// Token: 0x040028BA RID: 10426
	private Vector3 hoverPoint;

	// Token: 0x040028BB RID: 10427
	private Vector3 hoverNormal;
}
