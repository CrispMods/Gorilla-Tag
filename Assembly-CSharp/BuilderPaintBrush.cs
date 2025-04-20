using System;
using GorillaLocomotion.Climbing;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004B6 RID: 1206
public class BuilderPaintBrush : HoldableObject
{
	// Token: 0x06001D5B RID: 7515 RVA: 0x000E074C File Offset: 0x000DE94C
	private void Awake()
	{
		this.pieceLayers |= 1 << LayerMask.NameToLayer("Gorilla Object");
		this.pieceLayers |= 1 << LayerMask.NameToLayer("BuilderProp");
		this.pieceLayers |= 1 << LayerMask.NameToLayer("Prop");
		this.paintDistance = Vector3.SqrMagnitude(this.paintVolumeHalfExtents);
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x00030607 File Offset: 0x0002E807
	public override void DropItemCleanup()
	{
	}

	// Token: 0x06001D5D RID: 7517 RVA: 0x000E07E8 File Offset: 0x000DE9E8
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

	// Token: 0x06001D5E RID: 7518 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x06001D5F RID: 7519 RVA: 0x000E08E8 File Offset: 0x000DEAE8
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

	// Token: 0x06001D60 RID: 7520 RVA: 0x000441BF File Offset: 0x000423BF
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

	// Token: 0x06001D61 RID: 7521 RVA: 0x000E0988 File Offset: 0x000DEB88
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

	// Token: 0x06001D62 RID: 7522 RVA: 0x000E0D50 File Offset: 0x000DEF50
	private void PaintPiece()
	{
		BuilderTable.instance.RequestPaintPiece(this.hoveredPiece.pieceId, this.materialType);
		this.hoveredPiece.PaintingTint(false);
		this.hoveredPiece = null;
		this.hoveredPieceCollider = null;
		this.paintTimeElapsed = 0f;
		GorillaTagger.Instance.StartVibration(this.inLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
	}

	// Token: 0x06001D63 RID: 7523 RVA: 0x000E0DC8 File Offset: 0x000DEFC8
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

	// Token: 0x06001D64 RID: 7524 RVA: 0x000E0E2C File Offset: 0x000DF02C
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

	// Token: 0x04002041 RID: 8257
	[SerializeField]
	private Transform brushSurface;

	// Token: 0x04002042 RID: 8258
	[SerializeField]
	private Vector3 paintVolumeHalfExtents;

	// Token: 0x04002043 RID: 8259
	[SerializeField]
	private BuilderMaterialOptions paintBrushMaterialOptions;

	// Token: 0x04002044 RID: 8260
	[SerializeField]
	private MeshRenderer brushRenderer;

	// Token: 0x04002045 RID: 8261
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04002046 RID: 8262
	[SerializeField]
	private AudioClip paintSound;

	// Token: 0x04002047 RID: 8263
	[SerializeField]
	private AudioClip brushStrokeSound;

	// Token: 0x04002048 RID: 8264
	private GameObject holdingHand;

	// Token: 0x04002049 RID: 8265
	private bool inLeftHand;

	// Token: 0x0400204A RID: 8266
	private GorillaVelocityTracker handVelocity;

	// Token: 0x0400204B RID: 8267
	private BuilderPiece hoveredPiece;

	// Token: 0x0400204C RID: 8268
	private Collider hoveredPieceCollider;

	// Token: 0x0400204D RID: 8269
	private Collider[] hitColliders = new Collider[16];

	// Token: 0x0400204E RID: 8270
	private LayerMask pieceLayers = 0;

	// Token: 0x0400204F RID: 8271
	private Vector3 lastPosition = Vector3.zero;

	// Token: 0x04002050 RID: 8272
	private float positionDelta;

	// Token: 0x04002051 RID: 8273
	private float wiggleDistanceRequirement = 0.08f;

	// Token: 0x04002052 RID: 8274
	private float minimumWiggleFrameDistance = 0.005f;

	// Token: 0x04002053 RID: 8275
	private float maximumWiggleFrameDistance = 0.04f;

	// Token: 0x04002054 RID: 8276
	private float maxPaintVelocitySqrMag = 0.5f;

	// Token: 0x04002055 RID: 8277
	private float paintDelay = 0.2f;

	// Token: 0x04002056 RID: 8278
	private float paintTimeElapsed = -1f;

	// Token: 0x04002057 RID: 8279
	private float paintDistance;

	// Token: 0x04002058 RID: 8280
	private int materialType = -1;

	// Token: 0x04002059 RID: 8281
	private BuilderPaintBrush.PaintBrushState brushState;

	// Token: 0x0400205A RID: 8282
	private Rigidbody rb;

	// Token: 0x020004B7 RID: 1207
	public enum PaintBrushState
	{
		// Token: 0x0400205C RID: 8284
		Inactive,
		// Token: 0x0400205D RID: 8285
		HeldRemote,
		// Token: 0x0400205E RID: 8286
		Held,
		// Token: 0x0400205F RID: 8287
		Hover,
		// Token: 0x04002060 RID: 8288
		JustPainted
	}
}
