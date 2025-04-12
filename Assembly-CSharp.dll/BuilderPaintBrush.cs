using System;
using GorillaLocomotion.Climbing;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004AA RID: 1194
public class BuilderPaintBrush : HoldableObject
{
	// Token: 0x06001D0A RID: 7434 RVA: 0x000DDA94 File Offset: 0x000DBC94
	private void Awake()
	{
		this.pieceLayers |= 1 << LayerMask.NameToLayer("Gorilla Object");
		this.pieceLayers |= 1 << LayerMask.NameToLayer("BuilderProp");
		this.pieceLayers |= 1 << LayerMask.NameToLayer("Prop");
		this.paintDistance = Vector3.SqrMagnitude(this.paintVolumeHalfExtents);
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06001D0B RID: 7435 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void DropItemCleanup()
	{
	}

	// Token: 0x06001D0C RID: 7436 RVA: 0x000DDB30 File Offset: 0x000DBD30
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		this.holdingHand = grabbingHand;
		this.handVelocity = grabbingHand.GetComponent<GorillaVelocityTracker>();
		if (this.handVelocity == null)
		{
			Debug.Log("No Velocity Estimator");
		}
		this.inLeftHand = (grabbingHand == EquipmentInteractor.instance.leftHand);
		BodyDockPositions myBodyDockPositions = GorillaTagger.Instance.offlineVRRig.myBodyDockPositions;
		this.rb.isKinematic = true;
		this.rb.useGravity = false;
		if (this.inLeftHand)
		{
			base.transform.SetParent(myBodyDockPositions.leftHandTransform, true);
		}
		else
		{
			base.transform.SetParent(myBodyDockPositions.rightHandTransform, true);
		}
		base.transform.localScale = Vector3.one;
		EquipmentInteractor.instance.UpdateHandEquipment(this, this.inLeftHand);
		GorillaTagger.Instance.StartVibration(this.inLeftHand, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
		this.brushState = BuilderPaintBrush.PaintBrushState.Held;
	}

	// Token: 0x06001D0D RID: 7437 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x06001D0E RID: 7438 RVA: 0x000DDC30 File Offset: 0x000DBE30
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (base.OnRelease(zoneReleased, releasingHand))
		{
			this.holdingHand = null;
			EquipmentInteractor.instance.UpdateHandEquipment(null, this.inLeftHand);
			this.inLeftHand = false;
			this.handVelocity = null;
			this.ClearHoveredPiece();
			base.transform.parent = null;
			base.transform.localScale = Vector3.one;
			this.rb.isKinematic = false;
			this.rb.velocity = Vector3.zero;
			this.rb.angularVelocity = Vector3.zero;
			this.rb.useGravity = true;
			return true;
		}
		return false;
	}

	// Token: 0x06001D0F RID: 7439 RVA: 0x00042E86 File Offset: 0x00041086
	private void LateUpdate()
	{
		if (this.brushState == BuilderPaintBrush.PaintBrushState.Inactive)
		{
			return;
		}
		if (this.holdingHand == null || this.materialType == -1)
		{
			this.brushState = BuilderPaintBrush.PaintBrushState.Inactive;
			return;
		}
		this.FindPieceToPaint();
	}

	// Token: 0x06001D10 RID: 7440 RVA: 0x000DDCD0 File Offset: 0x000DBED0
	private void FindPieceToPaint()
	{
		switch (this.brushState)
		{
		case BuilderPaintBrush.PaintBrushState.Held:
		{
			if (this.materialType == -1)
			{
				return;
			}
			Array.Clear(this.hitColliders, 0, this.hitColliders.Length);
			int num = Physics.OverlapBoxNonAlloc(this.brushSurface.transform.position - this.brushSurface.up * this.paintVolumeHalfExtents.y, this.paintVolumeHalfExtents, this.hitColliders, this.brushSurface.transform.rotation, this.pieceLayers, QueryTriggerInteraction.Ignore);
			BuilderPieceCollider builderPieceCollider = null;
			Collider collider = null;
			float num2 = float.MaxValue;
			for (int i = 0; i < num; i++)
			{
				BuilderPieceCollider component = this.hitColliders[i].GetComponent<BuilderPieceCollider>();
				if (component != null && component.piece.materialType != this.materialType && component.piece.materialType != -1)
				{
					float sqrMagnitude = (this.brushSurface.transform.position - component.transform.position).sqrMagnitude;
					if (sqrMagnitude < num2 && component.piece.CanPlayerGrabPiece(PhotonNetwork.LocalPlayer.ActorNumber, component.piece.transform.position))
					{
						num2 = sqrMagnitude;
						builderPieceCollider = component;
						collider = this.hitColliders[i];
					}
				}
			}
			if (builderPieceCollider != null)
			{
				this.ClearHoveredPiece();
				this.hoveredPiece = builderPieceCollider.piece;
				this.hoveredPieceCollider = collider;
				this.hoveredPiece.PaintingTint(true);
				GorillaTagger.Instance.StartVibration(this.inLeftHand, GorillaTagger.Instance.tapHapticStrength / 4f, GorillaTagger.Instance.tapHapticDuration);
				this.positionDelta = 0f;
				this.lastPosition = this.brushSurface.transform.position;
				this.brushState = BuilderPaintBrush.PaintBrushState.Hover;
				return;
			}
			break;
		}
		case BuilderPaintBrush.PaintBrushState.Hover:
		{
			if (this.hoveredPiece == null || this.hoveredPieceCollider == null)
			{
				this.ClearHoveredPiece();
				return;
			}
			float sqrMagnitude2 = this.handVelocity.GetLatestVelocity(false).sqrMagnitude;
			float sqrMagnitude3 = this.handVelocity.GetAverageVelocity(false, 0.15f, false).sqrMagnitude;
			if (this.handVelocity != null && (sqrMagnitude2 > this.maxPaintVelocitySqrMag || sqrMagnitude3 > this.maxPaintVelocitySqrMag))
			{
				this.ClearHoveredPiece();
				return;
			}
			Vector3 vector = this.brushSurface.position - this.brushSurface.up * this.paintVolumeHalfExtents.y;
			Vector3 b = this.hoveredPieceCollider.ClosestPointOnBounds(vector);
			if (Vector3.SqrMagnitude(vector - b) > this.paintDistance)
			{
				this.ClearHoveredPiece();
				return;
			}
			GorillaTagger.Instance.StartVibration(this.inLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, Time.deltaTime);
			float num3 = Vector3.Distance(this.lastPosition, this.brushSurface.position);
			if (num3 < this.minimumWiggleFrameDistance)
			{
				this.lastPosition = this.brushSurface.position;
				return;
			}
			this.positionDelta += Math.Min(num3, this.maximumWiggleFrameDistance);
			this.lastPosition = this.brushSurface.position;
			if (this.positionDelta >= this.wiggleDistanceRequirement)
			{
				this.positionDelta = 0f;
				this.audioSource.clip = this.paintSound;
				this.audioSource.GTPlay();
				this.PaintPiece();
				this.brushState = BuilderPaintBrush.PaintBrushState.JustPainted;
				return;
			}
			break;
		}
		case BuilderPaintBrush.PaintBrushState.JustPainted:
			if (this.paintTimeElapsed > this.paintDelay)
			{
				this.paintTimeElapsed = 0f;
				this.brushState = BuilderPaintBrush.PaintBrushState.Held;
				return;
			}
			this.paintTimeElapsed += Time.deltaTime;
			break;
		default:
			return;
		}
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x000DE098 File Offset: 0x000DC298
	private void PaintPiece()
	{
		BuilderTable.instance.RequestPaintPiece(this.hoveredPiece.pieceId, this.materialType);
		this.hoveredPiece.PaintingTint(false);
		this.hoveredPiece = null;
		this.hoveredPieceCollider = null;
		this.paintTimeElapsed = 0f;
		GorillaTagger.Instance.StartVibration(this.inLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x000DE110 File Offset: 0x000DC310
	private void ClearHoveredPiece()
	{
		if (this.hoveredPiece != null)
		{
			this.hoveredPiece.PaintingTint(false);
		}
		this.hoveredPiece = null;
		this.hoveredPieceCollider = null;
		this.positionDelta = 0f;
		this.brushState = ((this.holdingHand == null || this.materialType == -1) ? BuilderPaintBrush.PaintBrushState.Inactive : BuilderPaintBrush.PaintBrushState.Held);
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x000DE174 File Offset: 0x000DC374
	public void SetBrushMaterial(int inMaterialType)
	{
		this.materialType = inMaterialType;
		this.audioSource.clip = this.paintSound;
		this.audioSource.GTPlay();
		if (this.holdingHand != null)
		{
			GorillaTagger.Instance.StartVibration(this.inLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
		}
		if (this.materialType == -1)
		{
			this.ClearHoveredPiece();
		}
		else if (this.brushState == BuilderPaintBrush.PaintBrushState.Inactive && this.holdingHand != null)
		{
			this.brushState = BuilderPaintBrush.PaintBrushState.Held;
		}
		if (this.paintBrushMaterialOptions != null && this.brushRenderer != null)
		{
			Material material;
			int num;
			this.paintBrushMaterialOptions.GetMaterialFromType(this.materialType, out material, out num);
			if (material != null)
			{
				this.brushRenderer.material = material;
			}
		}
	}

	// Token: 0x04001FF3 RID: 8179
	[SerializeField]
	private Transform brushSurface;

	// Token: 0x04001FF4 RID: 8180
	[SerializeField]
	private Vector3 paintVolumeHalfExtents;

	// Token: 0x04001FF5 RID: 8181
	[SerializeField]
	private BuilderMaterialOptions paintBrushMaterialOptions;

	// Token: 0x04001FF6 RID: 8182
	[SerializeField]
	private MeshRenderer brushRenderer;

	// Token: 0x04001FF7 RID: 8183
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04001FF8 RID: 8184
	[SerializeField]
	private AudioClip paintSound;

	// Token: 0x04001FF9 RID: 8185
	[SerializeField]
	private AudioClip brushStrokeSound;

	// Token: 0x04001FFA RID: 8186
	private GameObject holdingHand;

	// Token: 0x04001FFB RID: 8187
	private bool inLeftHand;

	// Token: 0x04001FFC RID: 8188
	private GorillaVelocityTracker handVelocity;

	// Token: 0x04001FFD RID: 8189
	private BuilderPiece hoveredPiece;

	// Token: 0x04001FFE RID: 8190
	private Collider hoveredPieceCollider;

	// Token: 0x04001FFF RID: 8191
	private Collider[] hitColliders = new Collider[16];

	// Token: 0x04002000 RID: 8192
	private LayerMask pieceLayers = 0;

	// Token: 0x04002001 RID: 8193
	private Vector3 lastPosition = Vector3.zero;

	// Token: 0x04002002 RID: 8194
	private float positionDelta;

	// Token: 0x04002003 RID: 8195
	private float wiggleDistanceRequirement = 0.08f;

	// Token: 0x04002004 RID: 8196
	private float minimumWiggleFrameDistance = 0.005f;

	// Token: 0x04002005 RID: 8197
	private float maximumWiggleFrameDistance = 0.04f;

	// Token: 0x04002006 RID: 8198
	private float maxPaintVelocitySqrMag = 0.5f;

	// Token: 0x04002007 RID: 8199
	private float paintDelay = 0.2f;

	// Token: 0x04002008 RID: 8200
	private float paintTimeElapsed = -1f;

	// Token: 0x04002009 RID: 8201
	private float paintDistance;

	// Token: 0x0400200A RID: 8202
	private int materialType = -1;

	// Token: 0x0400200B RID: 8203
	private BuilderPaintBrush.PaintBrushState brushState;

	// Token: 0x0400200C RID: 8204
	private Rigidbody rb;

	// Token: 0x020004AB RID: 1195
	public enum PaintBrushState
	{
		// Token: 0x0400200E RID: 8206
		Inactive,
		// Token: 0x0400200F RID: 8207
		HeldRemote,
		// Token: 0x04002010 RID: 8208
		Held,
		// Token: 0x04002011 RID: 8209
		Hover,
		// Token: 0x04002012 RID: 8210
		JustPainted
	}
}
