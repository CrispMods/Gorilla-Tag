using System;
using UnityEngine;

// Token: 0x020005BE RID: 1470
public class FreeHoverboardInstance : MonoBehaviour
{
	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x0600247F RID: 9343 RVA: 0x000B5D6C File Offset: 0x000B3F6C
	// (set) Token: 0x06002480 RID: 9344 RVA: 0x000B5D74 File Offset: 0x000B3F74
	public Rigidbody Rigidbody { get; private set; }

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06002481 RID: 9345 RVA: 0x000B5D7D File Offset: 0x000B3F7D
	// (set) Token: 0x06002482 RID: 9346 RVA: 0x000B5D85 File Offset: 0x000B3F85
	public Color boardColor { get; private set; }

	// Token: 0x06002483 RID: 9347 RVA: 0x000B5D90 File Offset: 0x000B3F90
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		Material[] sharedMaterials = this.boardMesh.sharedMaterials;
		this.colorMaterial = new Material(sharedMaterials[1]);
		sharedMaterials[1] = this.colorMaterial;
		this.boardMesh.sharedMaterials = sharedMaterials;
	}

	// Token: 0x06002484 RID: 9348 RVA: 0x000B5DD8 File Offset: 0x000B3FD8
	public void SetColor(Color col)
	{
		this.colorMaterial.color = col;
		this.boardColor = col;
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x000B5DF0 File Offset: 0x000B3FF0
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

	// Token: 0x06002486 RID: 9350 RVA: 0x000B5E6C File Offset: 0x000B406C
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

	// Token: 0x040028A8 RID: 10408
	public int ownerActorNumber;

	// Token: 0x040028A9 RID: 10409
	public int boardIndex;

	// Token: 0x040028AA RID: 10410
	[SerializeField]
	private Vector3 sphereCastCenter;

	// Token: 0x040028AB RID: 10411
	[SerializeField]
	private float sphereCastRadius;

	// Token: 0x040028AC RID: 10412
	[SerializeField]
	private LayerMask hoverRaycastMask;

	// Token: 0x040028AD RID: 10413
	[SerializeField]
	private float hoverHeight;

	// Token: 0x040028AE RID: 10414
	[SerializeField]
	private float hoverRotationLerp;

	// Token: 0x040028AF RID: 10415
	[SerializeField]
	private float avelocityDragWhileHovering;

	// Token: 0x040028B0 RID: 10416
	[SerializeField]
	private MeshRenderer boardMesh;

	// Token: 0x040028B2 RID: 10418
	private Material colorMaterial;

	// Token: 0x040028B3 RID: 10419
	private bool hasHoverPoint;

	// Token: 0x040028B4 RID: 10420
	private Vector3 hoverPoint;

	// Token: 0x040028B5 RID: 10421
	private Vector3 hoverNormal;
}
