using System;
using System.Collections.Generic;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004DB RID: 1243
public class BuilderPiece : MonoBehaviour
{
	// Token: 0x06001E1C RID: 7708 RVA: 0x000E44C8 File Offset: 0x000E26C8
	private void Awake()
	{
		if (this.vFXInfo == null)
		{
			Debug.LogErrorFormat("BuilderPiece {0} is missing Effect Info", new object[]
			{
				base.gameObject.name
			});
		}
		this.materialType = -1;
		this.pieceType = -1;
		this.pieceId = -1;
		this.pieceDataIndex = -1;
		this.state = BuilderPiece.State.None;
		this.isStatic = true;
		this.parentPiece = null;
		this.firstChildPiece = null;
		this.nextSiblingPiece = null;
		this.attachIndex = -1;
		this.parentAttachIndex = -1;
		this.parentHeld = null;
		this.heldByPlayerActorNumber = -1;
		this.placedOnlyColliders = new List<Collider>(4);
		List<Collider> list = new List<Collider>(4);
		foreach (GameObject gameObject in this.onlyWhenPlaced)
		{
			list.Clear();
			gameObject.GetComponentsInChildren<Collider>(list);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].isTrigger)
				{
					BuilderPieceCollider builderPieceCollider = list[i].GetComponent<BuilderPieceCollider>();
					if (builderPieceCollider == null)
					{
						builderPieceCollider = list[i].AddComponent<BuilderPieceCollider>();
					}
					builderPieceCollider.piece = this;
					this.placedOnlyColliders.Add(list[i]);
				}
			}
		}
		this.SetActive(this.onlyWhenPlaced, false);
		this.SetActive(this.onlyWhenNotPlaced, true);
		this.colliders = new List<Collider>(4);
		base.GetComponentsInChildren<Collider>(this.colliders);
		for (int j = this.colliders.Count - 1; j >= 0; j--)
		{
			if (this.colliders[j].isTrigger)
			{
				this.colliders.RemoveAt(j);
			}
			else
			{
				BuilderPieceCollider builderPieceCollider2 = this.colliders[j].GetComponent<BuilderPieceCollider>();
				if (builderPieceCollider2 == null)
				{
					builderPieceCollider2 = this.colliders[j].AddComponent<BuilderPieceCollider>();
				}
				builderPieceCollider2.piece = this;
			}
		}
		this.gridPlanes = new List<BuilderAttachGridPlane>(8);
		base.GetComponentsInChildren<BuilderAttachGridPlane>(this.gridPlanes);
		this.pieceComponents = new List<IBuilderPieceComponent>(1);
		base.GetComponentsInChildren<IBuilderPieceComponent>(true, this.pieceComponents);
		this.pieceComponentsActive = false;
		this.functionalPieceComponent = base.GetComponentInChildren<IBuilderPieceFunctional>();
		this.SetCollidersEnabled<Collider>(this.colliders, false);
		this.SetBehavioursEnabled<Behaviour>(this.onlyWhenPlacedBehaviours, false);
		this.preventSnapUntilMoved = 0;
		this.preventSnapUntilMovedFromPos = Vector3.zero;
		this.renderingIndirect = new List<MeshRenderer>(4);
		this.paintingCount = 0;
		this.potentialGrabCount = 0;
		this.potentialGrabChildCount = 0;
		this.isPrivatePlot = (this.plotComponent != null);
		this.privatePlotIndex = -1;
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x000E476C File Offset: 0x000E296C
	public void OnReturnToPool()
	{
		BuilderTable.instance.builderRenderer.RemovePiece(this);
		for (int i = 0; i < this.pieceComponents.Count; i++)
		{
			this.pieceComponents[i].OnPieceDestroy();
		}
		this.functionalPieceState = 0;
		this.state = BuilderPiece.State.None;
		this.isStatic = true;
		this.materialType = -1;
		this.pieceType = -1;
		this.pieceId = -1;
		this.pieceDataIndex = -1;
		this.parentPiece = null;
		this.firstChildPiece = null;
		this.nextSiblingPiece = null;
		this.attachIndex = -1;
		this.parentAttachIndex = -1;
		this.overrideSavedPiece = false;
		this.savedMaterialType = -1;
		this.savedPieceType = -1;
		this.shelfOwner = -1;
		this.parentHeld = null;
		this.heldByPlayerActorNumber = -1;
		this.activatedTimeStamp = 0;
		this.SetActive(this.onlyWhenPlaced, false);
		this.SetActive(this.onlyWhenNotPlaced, true);
		this.SetCollidersEnabled<Collider>(this.colliders, false);
		this.SetBehavioursEnabled<Behaviour>(this.onlyWhenPlacedBehaviours, false);
		this.preventSnapUntilMoved = 0;
		this.preventSnapUntilMovedFromPos = Vector3.zero;
		base.transform.localScale = Vector3.one;
		if (this.isArmShelf)
		{
			if (this.armShelf != null)
			{
				this.armShelf.piece = null;
			}
			this.armShelf = null;
		}
		for (int j = 0; j < this.gridPlanes.Count; j++)
		{
			this.gridPlanes[j].OnReturnToPool(BuilderTable.instance.builderPool);
		}
	}

	// Token: 0x06001E1E RID: 7710 RVA: 0x000448D6 File Offset: 0x00042AD6
	public void OnCreatedByPool()
	{
		this.materialSwapTargets = new List<MeshRenderer>(4);
		base.GetComponentsInChildren<MeshRenderer>(this.areMeshesToggledOnPlace, this.materialSwapTargets);
		this.surfaceOverrides = new List<GorillaSurfaceOverride>(4);
		base.GetComponentsInChildren<GorillaSurfaceOverride>(this.areMeshesToggledOnPlace, this.surfaceOverrides);
	}

	// Token: 0x06001E1F RID: 7711 RVA: 0x000E48E8 File Offset: 0x000E2AE8
	public void SetupPiece(float gridSize)
	{
		for (int i = 0; i < this.gridPlanes.Count; i++)
		{
			this.gridPlanes[i].Setup(this, i, gridSize);
		}
	}

	// Token: 0x06001E20 RID: 7712 RVA: 0x000E4920 File Offset: 0x000E2B20
	public void SetMaterial(int inMaterialType, bool force = false)
	{
		if (this.materialOptions == null || this.materialSwapTargets == null || this.materialSwapTargets.Count < 1)
		{
			return;
		}
		if (this.materialType == inMaterialType && !force)
		{
			return;
		}
		this.materialType = inMaterialType;
		Material material = null;
		int num = -1;
		if (inMaterialType == -1)
		{
			this.materialOptions.GetDefaultMaterial(out this.materialType, out material, out num);
		}
		else
		{
			this.materialOptions.GetMaterialFromType(this.materialType, out material, out num);
			if (material == null)
			{
				this.materialOptions.GetDefaultMaterial(out this.materialType, out material, out num);
			}
		}
		if (material == null)
		{
			Debug.LogErrorFormat("Piece {0} has no material matching Type {1}", new object[]
			{
				this.GetPieceId(),
				inMaterialType
			});
			return;
		}
		foreach (MeshRenderer meshRenderer in this.materialSwapTargets)
		{
			if (!(meshRenderer == null) && meshRenderer.enabled)
			{
				meshRenderer.material = material;
			}
		}
		if (this.surfaceOverrides != null && num != -1)
		{
			foreach (GorillaSurfaceOverride gorillaSurfaceOverride in this.surfaceOverrides)
			{
				gorillaSurfaceOverride.overrideIndex = num;
			}
		}
		if (this.renderingIndirect.Count > 0)
		{
			BuilderTable.instance.builderRenderer.ChangePieceIndirectMaterial(this, this.materialSwapTargets, material);
		}
	}

	// Token: 0x06001E21 RID: 7713 RVA: 0x00044914 File Offset: 0x00042B14
	public int GetPieceId()
	{
		return this.pieceId;
	}

	// Token: 0x06001E22 RID: 7714 RVA: 0x0004491C File Offset: 0x00042B1C
	public int GetParentPieceId()
	{
		if (!(this.parentPiece == null))
		{
			return this.parentPiece.pieceId;
		}
		return -1;
	}

	// Token: 0x06001E23 RID: 7715 RVA: 0x00044939 File Offset: 0x00042B39
	public int GetAttachIndex()
	{
		return this.attachIndex;
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x00044941 File Offset: 0x00042B41
	public int GetParentAttachIndex()
	{
		return this.parentAttachIndex;
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x000E4AB4 File Offset: 0x000E2CB4
	private void SetPieceActive(List<IBuilderPieceComponent> components, bool active)
	{
		if (components == null || active == this.pieceComponentsActive)
		{
			return;
		}
		this.pieceComponentsActive = active;
		for (int i = 0; i < components.Count; i++)
		{
			if (components[i] != null)
			{
				if (active)
				{
					components[i].OnPieceActivate();
				}
				else
				{
					components[i].OnPieceDeactivate();
				}
			}
		}
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x000E4B0C File Offset: 0x000E2D0C
	private void SetBehavioursEnabled<T>(List<T> components, bool enabled) where T : Behaviour
	{
		if (components == null)
		{
			return;
		}
		for (int i = 0; i < components.Count; i++)
		{
			if (components[i] != null)
			{
				components[i].enabled = enabled;
			}
		}
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x000E4B54 File Offset: 0x000E2D54
	private void SetCollidersEnabled<T>(List<T> components, bool enabled) where T : Collider
	{
		if (components == null)
		{
			return;
		}
		for (int i = 0; i < components.Count; i++)
		{
			if (components[i] != null)
			{
				components[i].enabled = enabled;
			}
		}
	}

	// Token: 0x06001E28 RID: 7720 RVA: 0x000E4B9C File Offset: 0x000E2D9C
	public void SetColliderLayers<T>(List<T> components, int layer) where T : Collider
	{
		if (components == null)
		{
			return;
		}
		for (int i = 0; i < components.Count; i++)
		{
			if (components[i] != null)
			{
				components[i].gameObject.layer = layer;
			}
		}
	}

	// Token: 0x06001E29 RID: 7721 RVA: 0x000E4BEC File Offset: 0x000E2DEC
	private void SetActive(List<GameObject> gameObjects, bool active)
	{
		if (gameObjects == null)
		{
			return;
		}
		for (int i = 0; i < gameObjects.Count; i++)
		{
			if (gameObjects[i] != null)
			{
				gameObjects[i].SetActive(active);
			}
		}
	}

	// Token: 0x06001E2A RID: 7722 RVA: 0x00044949 File Offset: 0x00042B49
	public void SetFunctionalPieceState(byte fState, NetPlayer instigator, int timeStamp)
	{
		if (this.functionalPieceComponent == null || !this.functionalPieceComponent.IsStateValid(fState))
		{
			fState = 0;
		}
		this.functionalPieceState = fState;
		IBuilderPieceFunctional builderPieceFunctional = this.functionalPieceComponent;
		if (builderPieceFunctional == null)
		{
			return;
		}
		builderPieceFunctional.OnStateChanged(fState, instigator, timeStamp);
	}

	// Token: 0x06001E2B RID: 7723 RVA: 0x0004497E File Offset: 0x00042B7E
	public void SetScale(float scale)
	{
		if (this.scaleRoot != null)
		{
			this.scaleRoot.localScale = Vector3.one * scale;
		}
	}

	// Token: 0x06001E2C RID: 7724 RVA: 0x000449A4 File Offset: 0x00042BA4
	public void PaintingTint(bool enable)
	{
		if (enable)
		{
			this.paintingCount++;
			if (this.paintingCount == 1)
			{
				this.RefreshTint();
				return;
			}
		}
		else
		{
			this.paintingCount--;
			if (this.paintingCount == 0)
			{
				this.RefreshTint();
			}
		}
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x000E4C2C File Offset: 0x000E2E2C
	public void PotentialGrab(bool enable)
	{
		if (enable)
		{
			this.potentialGrabCount++;
			if (this.potentialGrabCount == 1 && this.potentialGrabChildCount == 0)
			{
				this.RefreshTint();
				return;
			}
		}
		else
		{
			this.potentialGrabCount--;
			if (this.potentialGrabCount == 0 && this.potentialGrabChildCount == 0)
			{
				this.RefreshTint();
			}
		}
	}

	// Token: 0x06001E2E RID: 7726 RVA: 0x000E4C88 File Offset: 0x000E2E88
	public static void PotentialGrabChildren(BuilderPiece piece, bool enable)
	{
		BuilderPiece builderPiece = piece.firstChildPiece;
		while (builderPiece != null)
		{
			if (enable)
			{
				builderPiece.potentialGrabChildCount++;
				if (builderPiece.potentialGrabChildCount == 1 && builderPiece.potentialGrabCount == 0)
				{
					builderPiece.RefreshTint();
				}
			}
			else
			{
				builderPiece.potentialGrabChildCount--;
				if (builderPiece.potentialGrabChildCount == 0 && builderPiece.potentialGrabCount == 0)
				{
					builderPiece.RefreshTint();
				}
			}
			BuilderPiece.PotentialGrabChildren(builderPiece, enable);
			builderPiece = builderPiece.nextSiblingPiece;
		}
	}

	// Token: 0x06001E2F RID: 7727 RVA: 0x000E4D04 File Offset: 0x000E2F04
	private void RefreshTint()
	{
		if (this.potentialGrabCount > 0 || this.potentialGrabChildCount > 0)
		{
			this.SetTint(BuilderTable.instance.potentialGrabTint);
			return;
		}
		if (this.paintingCount > 0)
		{
			this.SetTint(BuilderTable.instance.paintingTint);
			return;
		}
		switch (this.state)
		{
		case BuilderPiece.State.AttachedToDropped:
		case BuilderPiece.State.Dropped:
			this.SetTint(BuilderTable.instance.droppedTint);
			return;
		case BuilderPiece.State.Grabbed:
		case BuilderPiece.State.GrabbedLocal:
		case BuilderPiece.State.AttachedToArm:
			this.SetTint(BuilderTable.instance.grabbedTint);
			return;
		case BuilderPiece.State.OnShelf:
		case BuilderPiece.State.OnConveyor:
			this.SetTint(BuilderTable.instance.shelfTint);
			return;
		}
		this.SetTint(BuilderTable.instance.defaultTint);
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x000449E3 File Offset: 0x00042BE3
	private void SetTint(float tint)
	{
		if (tint == this.tint)
		{
			return;
		}
		this.tint = tint;
		BuilderTable.instance.builderRenderer.SetPieceTint(this, tint);
	}

	// Token: 0x06001E31 RID: 7729 RVA: 0x000E4DC4 File Offset: 0x000E2FC4
	public void SetParentPiece(int newAttachIndex, BuilderPiece newParentPiece, int newParentAttachIndex)
	{
		if (this.parentHeld != null)
		{
			Debug.LogErrorFormat(newParentPiece.gameObject, "Cannot attach to piece {0} while already held", new object[]
			{
				(newParentPiece == null) ? null : newParentPiece.gameObject.name
			});
			return;
		}
		BuilderPiece.RemovePieceFromParent(this);
		this.attachIndex = newAttachIndex;
		this.parentPiece = newParentPiece;
		this.parentAttachIndex = newParentAttachIndex;
		this.AddPieceToParent(this);
		Transform parent = null;
		if (newParentPiece != null)
		{
			if (newParentAttachIndex >= 0)
			{
				parent = newParentPiece.gridPlanes[newParentAttachIndex].transform;
			}
			else
			{
				parent = newParentPiece.transform;
			}
		}
		base.transform.SetParent(parent, true);
		this.requestedParentPiece = null;
		BuilderTable.instance.UpdatePieceData(this);
	}

	// Token: 0x06001E32 RID: 7730 RVA: 0x000E4E7C File Offset: 0x000E307C
	public void ClearParentPiece(bool ignoreSnaps = false)
	{
		if (this.parentPiece == null)
		{
			if (!ignoreSnaps)
			{
				BuilderPiece.RemoveOverlapsWithDifferentPieceRoot(this, this, BuilderTable.instance.builderPool);
			}
			return;
		}
		BuilderPiece builderPiece = this.parentPiece;
		BuilderPiece.RemovePieceFromParent(this);
		this.attachIndex = -1;
		this.parentPiece = null;
		this.parentAttachIndex = -1;
		base.transform.SetParent(null, true);
		this.requestedParentPiece = null;
		BuilderTable.instance.UpdatePieceData(this);
		if (!ignoreSnaps)
		{
			BuilderPiece.RemoveOverlapsWithDifferentPieceRoot(this, this.GetRootPiece(), BuilderTable.instance.builderPool);
		}
	}

	// Token: 0x06001E33 RID: 7731 RVA: 0x000E4F08 File Offset: 0x000E3108
	public static void RemoveOverlapsWithDifferentPieceRoot(BuilderPiece piece, BuilderPiece root, BuilderPool pool)
	{
		for (int i = 0; i < piece.gridPlanes.Count; i++)
		{
			piece.gridPlanes[i].RemoveSnapsWithDifferentRoot(root, pool);
		}
		BuilderPiece builderPiece = piece.firstChildPiece;
		while (builderPiece != null)
		{
			BuilderPiece.RemoveOverlapsWithDifferentPieceRoot(builderPiece, root, pool);
			builderPiece = builderPiece.nextSiblingPiece;
		}
	}

	// Token: 0x06001E34 RID: 7732 RVA: 0x000E4F60 File Offset: 0x000E3160
	private void AddPieceToParent(BuilderPiece piece)
	{
		BuilderPiece builderPiece = piece.parentPiece;
		if (builderPiece == null)
		{
			return;
		}
		this.nextSiblingPiece = builderPiece.firstChildPiece;
		builderPiece.firstChildPiece = piece;
		if (piece.parentAttachIndex >= 0 && piece.parentAttachIndex < builderPiece.gridPlanes.Count)
		{
			builderPiece.gridPlanes[piece.parentAttachIndex].ChangeChildPieceCount(1 + piece.GetChildCount());
		}
	}

	// Token: 0x06001E35 RID: 7733 RVA: 0x000E4FCC File Offset: 0x000E31CC
	private static void RemovePieceFromParent(BuilderPiece piece)
	{
		BuilderPiece builderPiece = piece.parentPiece;
		if (builderPiece == null)
		{
			return;
		}
		BuilderPiece builderPiece2 = builderPiece.firstChildPiece;
		if (builderPiece2 == null)
		{
			Debug.LogErrorFormat("Parent {0} of piece {1} doesn't have any children", new object[]
			{
				builderPiece.name,
				piece.name
			});
		}
		bool flag = false;
		if (builderPiece2 == piece)
		{
			builderPiece.firstChildPiece = builderPiece2.nextSiblingPiece;
			flag = true;
		}
		else
		{
			while (builderPiece2 != null)
			{
				if (builderPiece2.nextSiblingPiece == piece)
				{
					builderPiece2.nextSiblingPiece = piece.nextSiblingPiece;
					piece.nextSiblingPiece = null;
					flag = true;
					break;
				}
				builderPiece2 = builderPiece2.nextSiblingPiece;
			}
		}
		if (!flag)
		{
			Debug.LogErrorFormat("Parent {0} of piece {1} doesn't have the piece a child", new object[]
			{
				builderPiece.name,
				piece.name
			});
			return;
		}
		if (piece.parentAttachIndex >= 0 && piece.parentAttachIndex < builderPiece.gridPlanes.Count)
		{
			builderPiece.gridPlanes[piece.parentAttachIndex].ChangeChildPieceCount(-1 * (1 + piece.GetChildCount()));
		}
	}

	// Token: 0x06001E36 RID: 7734 RVA: 0x000E50D0 File Offset: 0x000E32D0
	public void SetParentHeld(Transform parentHeld, int heldByPlayerActorNumber, bool heldInLeftHand)
	{
		if (this.parentPiece != null)
		{
			Debug.LogErrorFormat(this.parentPiece.gameObject, "Cannot hold while already attached to piece {0}", new object[]
			{
				this.parentPiece.gameObject.name
			});
			return;
		}
		this.heldByPlayerActorNumber = heldByPlayerActorNumber;
		this.parentHeld = parentHeld;
		this.heldInLeftHand = heldInLeftHand;
		base.transform.SetParent(parentHeld);
		BuilderTable.instance.UpdatePieceData(this);
	}

	// Token: 0x06001E37 RID: 7735 RVA: 0x000E5148 File Offset: 0x000E3348
	public void ClearParentHeld()
	{
		if (this.parentHeld == null)
		{
			return;
		}
		if (this.isArmShelf && this.armShelf != null)
		{
			this.armShelf.piece = null;
			this.armShelf = null;
		}
		this.heldByPlayerActorNumber = -1;
		this.parentHeld = null;
		this.heldInLeftHand = false;
		base.transform.SetParent(this.parentHeld);
		BuilderTable.instance.UpdatePieceData(this);
	}

	// Token: 0x06001E38 RID: 7736 RVA: 0x00044A07 File Offset: 0x00042C07
	public bool IsHeldLocal()
	{
		return this.heldByPlayerActorNumber != -1 && this.heldByPlayerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber;
	}

	// Token: 0x06001E39 RID: 7737 RVA: 0x00044A26 File Offset: 0x00042C26
	public bool IsHeldBy(int actorNumber)
	{
		return actorNumber != -1 && this.heldByPlayerActorNumber == actorNumber;
	}

	// Token: 0x06001E3A RID: 7738 RVA: 0x00044A37 File Offset: 0x00042C37
	public bool IsHeldInLeftHand()
	{
		return this.heldInLeftHand;
	}

	// Token: 0x06001E3B RID: 7739 RVA: 0x00044A3F File Offset: 0x00042C3F
	public static bool IsDroppedState(BuilderPiece.State state)
	{
		return state == BuilderPiece.State.Dropped || state == BuilderPiece.State.AttachedToDropped || state == BuilderPiece.State.OnShelf || state == BuilderPiece.State.OnConveyor;
	}

	// Token: 0x06001E3C RID: 7740 RVA: 0x000E51C0 File Offset: 0x000E33C0
	public void SetActivateTimeStamp(int timeStamp)
	{
		this.activatedTimeStamp = timeStamp;
		BuilderPiece builderPiece = this.firstChildPiece;
		while (builderPiece != null)
		{
			builderPiece.SetActivateTimeStamp(timeStamp);
			builderPiece = builderPiece.nextSiblingPiece;
		}
	}

	// Token: 0x06001E3D RID: 7741 RVA: 0x000E51F4 File Offset: 0x000E33F4
	public void SetState(BuilderPiece.State newState, bool force = false)
	{
		if (newState == this.state && !force)
		{
			return;
		}
		if (newState == BuilderPiece.State.Dropped && this.state != BuilderPiece.State.Dropped)
		{
			BuilderTable.instance.AddPieceToDropList(this);
		}
		else if (this.state == BuilderPiece.State.Dropped && newState != BuilderPiece.State.Dropped)
		{
			BuilderTable.instance.RemovePieceFromDropList(this);
		}
		BuilderPiece.State state = this.state;
		this.state = newState;
		if (this.pieceDataIndex >= 0)
		{
			BuilderTable.instance.UpdatePieceData(this);
		}
		switch (this.state)
		{
		case BuilderPiece.State.None:
			this.SetCollidersEnabled<Collider>(this.colliders, false);
			this.SetBehavioursEnabled<Behaviour>(this.onlyWhenPlacedBehaviours, false);
			this.SetActive(this.onlyWhenPlaced, false);
			this.SetActive(this.onlyWhenNotPlaced, true);
			this.SetKinematic(true, false);
			this.SetColliderLayers<Collider>(this.colliders, BuilderTable.droppedLayer);
			this.SetChildrenState(BuilderPiece.State.None, force);
			BuilderTable.instance.builderRenderer.RemovePiece(this);
			this.isStatic = true;
			this.SetPieceActive(this.pieceComponents, false);
			this.RefreshTint();
			return;
		case BuilderPiece.State.AttachedAndPlaced:
			this.SetCollidersEnabled<Collider>(this.colliders, true);
			this.SetBehavioursEnabled<Behaviour>(this.onlyWhenPlacedBehaviours, true);
			this.SetActive(this.onlyWhenPlaced, true);
			this.SetActive(this.onlyWhenNotPlaced, false);
			this.SetKinematic(true, true);
			this.SetColliderLayers<Collider>(this.colliders, BuilderTable.placedLayer);
			this.SetChildrenState(BuilderPiece.State.AttachedAndPlaced, force);
			this.SetStatic(false, force || this.areMeshesToggledOnPlace);
			this.SetPieceActive(this.pieceComponents, true);
			this.RefreshTint();
			return;
		case BuilderPiece.State.AttachedToDropped:
			this.SetCollidersEnabled<Collider>(this.colliders, true);
			this.SetBehavioursEnabled<Behaviour>(this.onlyWhenPlacedBehaviours, false);
			this.SetActive(this.onlyWhenPlaced, false);
			this.SetActive(this.onlyWhenNotPlaced, true);
			this.SetKinematic(true, true);
			this.SetColliderLayers<Collider>(this.colliders, BuilderTable.droppedLayer);
			this.SetChildrenState(BuilderPiece.State.AttachedToDropped, force);
			this.SetStatic(false, force);
			this.SetPieceActive(this.pieceComponents, false);
			this.RefreshTint();
			return;
		case BuilderPiece.State.Grabbed:
			this.SetCollidersEnabled<Collider>(this.colliders, true);
			this.SetBehavioursEnabled<Behaviour>(this.onlyWhenPlacedBehaviours, false);
			this.SetActive(this.onlyWhenPlaced, false);
			this.SetActive(this.onlyWhenNotPlaced, true);
			this.SetKinematic(true, true);
			this.SetColliderLayers<Collider>(this.colliders, BuilderTable.heldLayer);
			this.SetChildrenState(BuilderPiece.State.Grabbed, force);
			this.SetStatic(false, force || (this.areMeshesToggledOnPlace && state == BuilderPiece.State.AttachedAndPlaced));
			this.SetPieceActive(this.pieceComponents, false);
			this.SetActivateTimeStamp(0);
			this.RefreshTint();
			return;
		case BuilderPiece.State.Dropped:
			this.SetCollidersEnabled<Collider>(this.colliders, true);
			this.SetBehavioursEnabled<Behaviour>(this.onlyWhenPlacedBehaviours, false);
			this.SetActive(this.onlyWhenPlaced, false);
			this.SetActive(this.onlyWhenNotPlaced, true);
			this.SetKinematic(false, true);
			this.SetColliderLayers<Collider>(this.colliders, BuilderTable.droppedLayer);
			this.SetChildrenState(BuilderPiece.State.AttachedToDropped, force);
			this.SetStatic(false, force);
			this.SetPieceActive(this.pieceComponents, false);
			this.RefreshTint();
			return;
		case BuilderPiece.State.OnShelf:
			this.SetCollidersEnabled<Collider>(this.colliders, true);
			this.SetBehavioursEnabled<Behaviour>(this.onlyWhenPlacedBehaviours, false);
			this.SetActive(this.onlyWhenPlaced, false);
			this.SetActive(this.onlyWhenNotPlaced, true);
			this.SetKinematic(true, true);
			this.SetColliderLayers<Collider>(this.colliders, BuilderTable.droppedLayer);
			this.SetChildrenState(BuilderPiece.State.OnShelf, force);
			this.SetStatic(true, force);
			this.SetPieceActive(this.pieceComponents, false);
			this.RefreshTint();
			return;
		case BuilderPiece.State.Displayed:
			this.SetCollidersEnabled<Collider>(this.colliders, false);
			this.SetBehavioursEnabled<Behaviour>(this.onlyWhenPlacedBehaviours, false);
			this.SetActive(this.onlyWhenPlaced, false);
			this.SetActive(this.onlyWhenNotPlaced, true);
			this.SetKinematic(true, true);
			this.SetChildrenState(BuilderPiece.State.Displayed, force);
			this.SetStatic(false, force);
			this.SetPieceActive(this.pieceComponents, false);
			this.RefreshTint();
			return;
		case BuilderPiece.State.GrabbedLocal:
			this.SetCollidersEnabled<Collider>(this.colliders, true);
			this.SetBehavioursEnabled<Behaviour>(this.onlyWhenPlacedBehaviours, false);
			this.SetActive(this.onlyWhenPlaced, false);
			this.SetActive(this.onlyWhenNotPlaced, true);
			this.SetKinematic(true, true);
			this.SetColliderLayers<Collider>(this.colliders, BuilderTable.heldLayerLocal);
			this.SetChildrenState(BuilderPiece.State.GrabbedLocal, force);
			this.SetStatic(false, force || (this.areMeshesToggledOnPlace && state == BuilderPiece.State.AttachedAndPlaced));
			this.SetPieceActive(this.pieceComponents, false);
			this.SetActivateTimeStamp(0);
			this.RefreshTint();
			return;
		case BuilderPiece.State.OnConveyor:
			this.SetCollidersEnabled<Collider>(this.colliders, true);
			this.SetBehavioursEnabled<Behaviour>(this.onlyWhenPlacedBehaviours, false);
			this.SetActive(this.onlyWhenPlaced, false);
			this.SetActive(this.onlyWhenNotPlaced, true);
			this.SetKinematic(true, true);
			this.SetColliderLayers<Collider>(this.colliders, BuilderTable.droppedLayer);
			this.SetChildrenState(BuilderPiece.State.OnConveyor, force);
			this.SetStatic(false, force);
			this.SetPieceActive(this.pieceComponents, false);
			this.RefreshTint();
			return;
		case BuilderPiece.State.AttachedToArm:
			this.SetCollidersEnabled<Collider>(this.colliders, true);
			this.SetBehavioursEnabled<Behaviour>(this.onlyWhenPlacedBehaviours, false);
			this.SetActive(this.onlyWhenPlaced, false);
			this.SetActive(this.onlyWhenNotPlaced, true);
			this.SetKinematic(true, true);
			this.SetColliderLayers<Collider>(this.colliders, BuilderTable.heldLayerLocal);
			this.SetChildrenState(BuilderPiece.State.AttachedToArm, force);
			this.SetStatic(false, force);
			this.SetPieceActive(this.pieceComponents, false);
			this.RefreshTint();
			return;
		default:
			return;
		}
	}

	// Token: 0x06001E3E RID: 7742 RVA: 0x000E5744 File Offset: 0x000E3944
	public void SetKinematic(bool kinematic, bool destroyImmediate = true)
	{
		if (kinematic && this.rigidBody != null)
		{
			if (destroyImmediate)
			{
				UnityEngine.Object.DestroyImmediate(this.rigidBody);
				this.rigidBody = null;
			}
			else
			{
				UnityEngine.Object.Destroy(this.rigidBody);
				this.rigidBody = null;
			}
		}
		else if (!kinematic && this.rigidBody == null)
		{
			this.rigidBody = base.gameObject.GetComponent<Rigidbody>();
			if (this.rigidBody != null)
			{
				Debug.LogErrorFormat("We should never already have a rigid body here {0} {1}", new object[]
				{
					this.pieceId,
					this.pieceType
				});
			}
			if (this.rigidBody == null)
			{
				this.rigidBody = base.gameObject.AddComponent<Rigidbody>();
			}
			if (this.rigidBody != null)
			{
				this.rigidBody.isKinematic = kinematic;
			}
		}
		if (this.rigidBody != null)
		{
			this.rigidBody.mass = 1f;
		}
	}

	// Token: 0x06001E3F RID: 7743 RVA: 0x00030607 File Offset: 0x0002E807
	public void SetStatic(bool isStatic, bool force = false)
	{
	}

	// Token: 0x06001E40 RID: 7744 RVA: 0x000E584C File Offset: 0x000E3A4C
	private void SetChildrenState(BuilderPiece.State newState, bool force)
	{
		BuilderPiece builderPiece = this.firstChildPiece;
		while (builderPiece != null)
		{
			builderPiece.SetState(newState, force);
			builderPiece = builderPiece.nextSiblingPiece;
		}
	}

	// Token: 0x06001E41 RID: 7745 RVA: 0x000E587C File Offset: 0x000E3A7C
	public void OnCreate()
	{
		for (int i = 0; i < this.pieceComponents.Count; i++)
		{
			this.pieceComponents[i].OnPieceCreate(this.pieceType, this.pieceId);
		}
	}

	// Token: 0x06001E42 RID: 7746 RVA: 0x000E58BC File Offset: 0x000E3ABC
	public void OnPlacementDeserialized()
	{
		for (int i = 0; i < this.pieceComponents.Count; i++)
		{
			this.pieceComponents[i].OnPiecePlacementDeserialized();
		}
	}

	// Token: 0x06001E43 RID: 7747 RVA: 0x00044A53 File Offset: 0x00042C53
	public void PlayPlacementFx()
	{
		this.PlayVFX(this.vFXInfo.placeVFX);
	}

	// Token: 0x06001E44 RID: 7748 RVA: 0x00044A66 File Offset: 0x00042C66
	public void PlayDisconnectFx()
	{
		this.PlayVFX(this.vFXInfo.disconnectVFX);
	}

	// Token: 0x06001E45 RID: 7749 RVA: 0x00044A79 File Offset: 0x00042C79
	public void PlayGrabbedFx()
	{
		this.PlayVFX(this.vFXInfo.grabbedVFX);
	}

	// Token: 0x06001E46 RID: 7750 RVA: 0x00044A8C File Offset: 0x00042C8C
	public void PlayTooHeavyFx()
	{
		this.PlayVFX(this.vFXInfo.tooHeavyVFX);
	}

	// Token: 0x06001E47 RID: 7751 RVA: 0x00044A9F File Offset: 0x00042C9F
	public void PlayLocationLockFx()
	{
		this.PlayVFX(this.vFXInfo.locationLockVFX);
	}

	// Token: 0x06001E48 RID: 7752 RVA: 0x00044AB2 File Offset: 0x00042CB2
	public void PlayRecycleFx()
	{
		this.PlayVFX(this.vFXInfo.recycleVFX);
	}

	// Token: 0x06001E49 RID: 7753 RVA: 0x00044AC5 File Offset: 0x00042CC5
	private void PlayVFX(GameObject vfx)
	{
		ObjectPools.instance.Instantiate(vfx, base.transform.position);
	}

	// Token: 0x06001E4A RID: 7754 RVA: 0x000E58F0 File Offset: 0x000E3AF0
	public static BuilderPiece GetBuilderPieceFromCollider(Collider collider)
	{
		if (collider == null)
		{
			return null;
		}
		BuilderPieceCollider component = collider.GetComponent<BuilderPieceCollider>();
		if (!(component == null))
		{
			return component.piece;
		}
		return null;
	}

	// Token: 0x06001E4B RID: 7755 RVA: 0x000E5920 File Offset: 0x000E3B20
	public static BuilderPiece GetBuilderPieceFromTransform(Transform transform)
	{
		while (transform != null)
		{
			BuilderPiece component = transform.GetComponent<BuilderPiece>();
			if (component != null)
			{
				return component;
			}
			transform = transform.parent;
		}
		return null;
	}

	// Token: 0x06001E4C RID: 7756 RVA: 0x000E5954 File Offset: 0x000E3B54
	public static void MakePieceRoot(BuilderPiece piece)
	{
		if (piece == null)
		{
			return;
		}
		if (piece.parentPiece == null || piece.parentPiece.isBuiltIntoTable)
		{
			return;
		}
		BuilderPiece.MakePieceRoot(piece.parentPiece);
		int newAttachIndex = piece.parentAttachIndex;
		int newParentAttachIndex = piece.attachIndex;
		BuilderPiece builderPiece = piece.parentPiece;
		bool ignoreSnaps = true;
		piece.ClearParentPiece(ignoreSnaps);
		builderPiece.SetParentPiece(newAttachIndex, piece, newParentAttachIndex);
	}

	// Token: 0x06001E4D RID: 7757 RVA: 0x000E59B8 File Offset: 0x000E3BB8
	public BuilderPiece GetRootPiece()
	{
		BuilderPiece builderPiece = this;
		while (builderPiece.parentPiece != null && !builderPiece.parentPiece.isBuiltIntoTable)
		{
			builderPiece = builderPiece.parentPiece;
		}
		return builderPiece;
	}

	// Token: 0x06001E4E RID: 7758 RVA: 0x00044ADE File Offset: 0x00042CDE
	public bool IsPrivatePlot()
	{
		return this.isPrivatePlot;
	}

	// Token: 0x06001E4F RID: 7759 RVA: 0x00044AE6 File Offset: 0x00042CE6
	public bool TryGetPlotComponent(out BuilderPiecePrivatePlot plot)
	{
		plot = this.plotComponent;
		return this.isPrivatePlot;
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x000E59EC File Offset: 0x000E3BEC
	public static bool CanPlayerAttachPieceToPiece(int playerActorNumber, BuilderPiece attachingPiece, BuilderPiece attachToPiece)
	{
		if (attachToPiece.state != BuilderPiece.State.AttachedAndPlaced && !attachToPiece.IsPrivatePlot() && attachToPiece.state != BuilderPiece.State.AttachedToArm)
		{
			return true;
		}
		BuilderPiece attachedBuiltInPiece = attachToPiece.GetAttachedBuiltInPiece();
		if (attachedBuiltInPiece == null || (!attachedBuiltInPiece.isPrivatePlot && !attachedBuiltInPiece.isArmShelf))
		{
			return true;
		}
		if (attachedBuiltInPiece.isArmShelf)
		{
			return attachedBuiltInPiece.heldByPlayerActorNumber == playerActorNumber && attachedBuiltInPiece.armShelf != null && attachedBuiltInPiece.armShelf.CanAttachToArmPiece();
		}
		BuilderPiecePrivatePlot builderPiecePrivatePlot;
		return !attachedBuiltInPiece.TryGetPlotComponent(out builderPiecePrivatePlot) || (builderPiecePrivatePlot.CanPlayerAttachToPlot(playerActorNumber) && builderPiecePrivatePlot.IsChainUnderCapacity(attachingPiece));
	}

	// Token: 0x06001E51 RID: 7761 RVA: 0x000E5A84 File Offset: 0x000E3C84
	public bool CanPlayerGrabPiece(int actorNumber, Vector3 worldPosition)
	{
		if (this.state != BuilderPiece.State.AttachedAndPlaced && !this.isPrivatePlot)
		{
			return true;
		}
		BuilderPiece attachedBuiltInPiece = this.GetAttachedBuiltInPiece();
		BuilderPiecePrivatePlot builderPiecePrivatePlot;
		return attachedBuiltInPiece == null || !attachedBuiltInPiece.isPrivatePlot || !attachedBuiltInPiece.TryGetPlotComponent(out builderPiecePrivatePlot) || builderPiecePrivatePlot.CanPlayerGrabFromPlot(actorNumber, worldPosition) || BuilderTable.instance.IsLocationWithinSharedBuildArea(worldPosition);
	}

	// Token: 0x06001E52 RID: 7762 RVA: 0x000E5AE0 File Offset: 0x000E3CE0
	public bool IsPieceMoving()
	{
		if (this.state != BuilderPiece.State.AttachedAndPlaced)
		{
			return false;
		}
		if (this.attachIndex < 0 || this.attachIndex >= this.gridPlanes.Count)
		{
			return false;
		}
		if (this.gridPlanes[this.attachIndex].IsAttachedToMovingGrid())
		{
			return true;
		}
		using (List<BuilderAttachGridPlane>.Enumerator enumerator = this.gridPlanes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.isMoving)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001E53 RID: 7763 RVA: 0x000E5B7C File Offset: 0x000E3D7C
	public BuilderPiece GetAttachedBuiltInPiece()
	{
		if (this.isBuiltIntoTable)
		{
			return this;
		}
		if (this.state != BuilderPiece.State.AttachedAndPlaced)
		{
			return null;
		}
		BuilderPiece rootPiece = this.GetRootPiece();
		if (rootPiece.parentPiece != null)
		{
			rootPiece = rootPiece.parentPiece;
		}
		if (rootPiece.isBuiltIntoTable)
		{
			return rootPiece;
		}
		return null;
	}

	// Token: 0x06001E54 RID: 7764 RVA: 0x000E5BC4 File Offset: 0x000E3DC4
	public int GetChainCostAndCount(int[] costArray)
	{
		for (int i = 0; i < costArray.Length; i++)
		{
			costArray[i] = 0;
		}
		foreach (BuilderResourceQuantity builderResourceQuantity in this.cost.quantities)
		{
			if (builderResourceQuantity.type >= BuilderResourceType.Basic && builderResourceQuantity.type < BuilderResourceType.Count)
			{
				costArray[(int)builderResourceQuantity.type] += builderResourceQuantity.count;
			}
		}
		return 1 + this.GetChildCountAndCost(costArray);
	}

	// Token: 0x06001E55 RID: 7765 RVA: 0x000E5C58 File Offset: 0x000E3E58
	public int GetChildCountAndCost(int[] costArray)
	{
		int num = 0;
		BuilderPiece builderPiece = this.firstChildPiece;
		while (builderPiece != null)
		{
			num++;
			foreach (BuilderResourceQuantity builderResourceQuantity in builderPiece.cost.quantities)
			{
				if (builderResourceQuantity.type >= BuilderResourceType.Basic && builderResourceQuantity.type < BuilderResourceType.Count)
				{
					costArray[(int)builderResourceQuantity.type] += builderResourceQuantity.count;
				}
			}
			num += builderPiece.GetChildCountAndCost(costArray);
			builderPiece = builderPiece.nextSiblingPiece;
		}
		return num;
	}

	// Token: 0x06001E56 RID: 7766 RVA: 0x000E5CFC File Offset: 0x000E3EFC
	public int GetChildCount()
	{
		int num = 0;
		foreach (BuilderAttachGridPlane builderAttachGridPlane in this.gridPlanes)
		{
			num += builderAttachGridPlane.GetChildCount();
		}
		return num;
	}

	// Token: 0x06001E57 RID: 7767 RVA: 0x000E5D54 File Offset: 0x000E3F54
	public void GetChainCost(int[] costArray)
	{
		for (int i = 0; i < costArray.Length; i++)
		{
			costArray[i] = 0;
		}
		foreach (BuilderResourceQuantity builderResourceQuantity in this.cost.quantities)
		{
			if (builderResourceQuantity.type >= BuilderResourceType.Basic && builderResourceQuantity.type < BuilderResourceType.Count)
			{
				costArray[(int)builderResourceQuantity.type] += builderResourceQuantity.count;
			}
		}
		this.AddChildCost(costArray);
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x000E5DE8 File Offset: 0x000E3FE8
	public void AddChildCost(int[] costArray)
	{
		int num = 0;
		BuilderPiece builderPiece = this.firstChildPiece;
		while (builderPiece != null)
		{
			num++;
			foreach (BuilderResourceQuantity builderResourceQuantity in builderPiece.cost.quantities)
			{
				if (builderResourceQuantity.type >= BuilderResourceType.Basic && builderResourceQuantity.type < BuilderResourceType.Count)
				{
					costArray[(int)builderResourceQuantity.type] += builderResourceQuantity.count;
				}
			}
			builderPiece.AddChildCost(costArray);
			builderPiece = builderPiece.nextSiblingPiece;
		}
	}

	// Token: 0x06001E59 RID: 7769 RVA: 0x000E5E88 File Offset: 0x000E4088
	public void BumpTwistToPositionRotation(byte twist, sbyte xOffset, sbyte zOffset, int potentialAttachIndex, BuilderAttachGridPlane potentialParentGridPlane, out Vector3 localPosition, out Quaternion localRotation, out Vector3 worldPosition, out Quaternion worldRotation)
	{
		float gridSize = BuilderTable.instance.gridSize;
		BuilderAttachGridPlane builderAttachGridPlane = this.gridPlanes[potentialAttachIndex];
		bool flag = (long)(twist % 2) == 1L;
		Transform center = potentialParentGridPlane.center;
		Vector3 position = center.position;
		Quaternion rotation = center.rotation;
		float num = flag ? builderAttachGridPlane.lengthOffset : builderAttachGridPlane.widthOffset;
		float num2 = flag ? builderAttachGridPlane.widthOffset : builderAttachGridPlane.lengthOffset;
		float num3 = num - potentialParentGridPlane.widthOffset;
		float num4 = num2 - potentialParentGridPlane.lengthOffset;
		Quaternion quaternion = Quaternion.Euler(0f, (float)twist * 90f, 0f);
		Quaternion lhs = rotation * quaternion;
		float x = (float)xOffset * gridSize + num3;
		float z = (float)zOffset * gridSize + num4;
		Vector3 point = new Vector3(x, 0f, z);
		Vector3 a = position + rotation * point;
		Transform center2 = builderAttachGridPlane.center;
		Quaternion quaternion2 = lhs * Quaternion.Inverse(center2.localRotation);
		Vector3 point2 = base.transform.InverseTransformPoint(center2.position);
		Vector3 vector = a - quaternion2 * point2;
		localPosition = potentialParentGridPlane.transform.InverseTransformPoint(vector);
		localRotation = quaternion * Quaternion.Inverse(center2.localRotation);
		worldPosition = vector;
		worldRotation = quaternion2;
	}

	// Token: 0x06001E5A RID: 7770 RVA: 0x000E5FD8 File Offset: 0x000E41D8
	public Quaternion TwistToLocalRotation(byte twist, int potentialAttachIndex)
	{
		float y = 90f * (float)twist;
		Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
		if (potentialAttachIndex < 0 || potentialAttachIndex >= this.gridPlanes.Count)
		{
			return quaternion;
		}
		BuilderAttachGridPlane builderAttachGridPlane = this.gridPlanes[potentialAttachIndex];
		Transform transform = (builderAttachGridPlane.center != null) ? builderAttachGridPlane.center : builderAttachGridPlane.transform;
		return quaternion * Quaternion.Inverse(transform.localRotation);
	}

	// Token: 0x06001E5B RID: 7771 RVA: 0x000E6050 File Offset: 0x000E4250
	public int GetPiecePlacement()
	{
		byte pieceTwist = this.GetPieceTwist();
		sbyte xOffset;
		sbyte zOffset;
		this.GetPieceBumpOffset(pieceTwist, out xOffset, out zOffset);
		return BuilderTable.PackPiecePlacement(pieceTwist, xOffset, zOffset);
	}

	// Token: 0x06001E5C RID: 7772 RVA: 0x000E6078 File Offset: 0x000E4278
	public byte GetPieceTwist()
	{
		if (this.attachIndex == -1)
		{
			return 0;
		}
		Quaternion localRotation = base.transform.localRotation;
		BuilderAttachGridPlane builderAttachGridPlane = this.gridPlanes[this.attachIndex];
		Quaternion rotation = localRotation * builderAttachGridPlane.transform.localRotation;
		float num = 0.866f;
		Vector3 lhs = rotation * Vector3.forward;
		float num2 = Vector3.Dot(lhs, Vector3.forward);
		float num3 = Vector3.Dot(lhs, Vector3.right);
		bool flag = Mathf.Abs(num2) > num;
		bool flag2 = Mathf.Abs(num3) > num;
		if (!flag && !flag2)
		{
			return 0;
		}
		uint num4;
		if (flag)
		{
			num4 = ((num2 > 0f) ? 0U : 2U);
		}
		else
		{
			num4 = ((num3 > 0f) ? 1U : 3U);
		}
		return (byte)num4;
	}

	// Token: 0x06001E5D RID: 7773 RVA: 0x000E612C File Offset: 0x000E432C
	public void GetPieceBumpOffset(byte twist, out sbyte xOffset, out sbyte zOffset)
	{
		if (this.attachIndex == -1 || this.parentPiece == null)
		{
			xOffset = 0;
			zOffset = 0;
			return;
		}
		float gridSize = BuilderTable.instance.gridSize;
		BuilderAttachGridPlane builderAttachGridPlane = this.gridPlanes[this.attachIndex];
		BuilderAttachGridPlane builderAttachGridPlane2 = this.parentPiece.gridPlanes[this.parentAttachIndex];
		bool flag = (long)(twist % 2) == 1L;
		float num = flag ? builderAttachGridPlane.lengthOffset : builderAttachGridPlane.widthOffset;
		float num2 = flag ? builderAttachGridPlane.widthOffset : builderAttachGridPlane.lengthOffset;
		float num3 = num - builderAttachGridPlane2.widthOffset;
		float num4 = num2 - builderAttachGridPlane2.lengthOffset;
		Vector3 position = builderAttachGridPlane.center.position;
		Vector3 position2 = builderAttachGridPlane2.center.position;
		Vector3 vector = Quaternion.Inverse(builderAttachGridPlane2.center.rotation) * (position - position2);
		xOffset = (sbyte)Mathf.RoundToInt((vector.x - num3) / gridSize);
		zOffset = (sbyte)Mathf.RoundToInt((vector.z - num4) / gridSize);
	}

	// Token: 0x0400212E RID: 8494
	public const int INVALID = -1;

	// Token: 0x0400212F RID: 8495
	public const float LIGHT_MASS = 1f;

	// Token: 0x04002130 RID: 8496
	public const float HEAVY_MASS = 10000f;

	// Token: 0x04002131 RID: 8497
	[Header("Piece Properties")]
	public string displayName;

	// Token: 0x04002132 RID: 8498
	public BuilderMaterialOptions materialOptions;

	// Token: 0x04002133 RID: 8499
	public BuilderResources cost;

	// Token: 0x04002134 RID: 8500
	private List<MeshRenderer> materialSwapTargets;

	// Token: 0x04002135 RID: 8501
	private List<GorillaSurfaceOverride> surfaceOverrides;

	// Token: 0x04002136 RID: 8502
	public Transform scaleRoot;

	// Token: 0x04002137 RID: 8503
	public bool isBuiltIntoTable;

	// Token: 0x04002138 RID: 8504
	public bool isArmShelf;

	// Token: 0x04002139 RID: 8505
	[HideInInspector]
	public BuilderArmShelf armShelf;

	// Token: 0x0400213A RID: 8506
	public Vector3 desiredShelfOffset = Vector3.zero;

	// Token: 0x0400213B RID: 8507
	public Vector3 desiredShelfRotationOffset = Vector3.zero;

	// Token: 0x0400213C RID: 8508
	private bool isPrivatePlot;

	// Token: 0x0400213D RID: 8509
	[HideInInspector]
	public int privatePlotIndex;

	// Token: 0x0400213E RID: 8510
	public BuilderPiecePrivatePlot plotComponent;

	// Token: 0x0400213F RID: 8511
	[Header("VFX")]
	[SerializeField]
	private BuilderPieceEffectInfo vFXInfo;

	// Token: 0x04002140 RID: 8512
	[Header("Piece Properties")]
	public int pieceType;

	// Token: 0x04002141 RID: 8513
	public int pieceId;

	// Token: 0x04002142 RID: 8514
	public int pieceDataIndex;

	// Token: 0x04002143 RID: 8515
	public int materialType = -1;

	// Token: 0x04002144 RID: 8516
	public bool suppressMaterialWarnings;

	// Token: 0x04002145 RID: 8517
	public int heldByPlayerActorNumber;

	// Token: 0x04002146 RID: 8518
	public bool heldInLeftHand;

	// Token: 0x04002147 RID: 8519
	public Transform parentHeld;

	// Token: 0x04002148 RID: 8520
	[HideInInspector]
	public BuilderPiece parentPiece;

	// Token: 0x04002149 RID: 8521
	[HideInInspector]
	public BuilderPiece firstChildPiece;

	// Token: 0x0400214A RID: 8522
	[HideInInspector]
	public BuilderPiece nextSiblingPiece;

	// Token: 0x0400214B RID: 8523
	[HideInInspector]
	public int attachIndex;

	// Token: 0x0400214C RID: 8524
	[HideInInspector]
	public int parentAttachIndex;

	// Token: 0x0400214D RID: 8525
	public int shelfOwner = -1;

	// Token: 0x0400214E RID: 8526
	[HideInInspector]
	public List<BuilderAttachGridPlane> gridPlanes;

	// Token: 0x0400214F RID: 8527
	[HideInInspector]
	public List<Collider> colliders;

	// Token: 0x04002150 RID: 8528
	public List<Collider> placedOnlyColliders;

	// Token: 0x04002151 RID: 8529
	[Header("Toggle on Place")]
	public List<Behaviour> onlyWhenPlacedBehaviours;

	// Token: 0x04002152 RID: 8530
	public List<IBuilderPieceComponent> pieceComponents;

	// Token: 0x04002153 RID: 8531
	public IBuilderPieceFunctional functionalPieceComponent;

	// Token: 0x04002154 RID: 8532
	public byte functionalPieceState;

	// Token: 0x04002155 RID: 8533
	public List<IBuilderPieceFunctional> pieceFunctionComponents;

	// Token: 0x04002156 RID: 8534
	private bool pieceComponentsActive;

	// Token: 0x04002157 RID: 8535
	public List<GameObject> onlyWhenPlaced;

	// Token: 0x04002158 RID: 8536
	public List<GameObject> onlyWhenNotPlaced;

	// Token: 0x04002159 RID: 8537
	public bool areMeshesToggledOnPlace;

	// Token: 0x0400215A RID: 8538
	[NonSerialized]
	public Rigidbody rigidBody;

	// Token: 0x0400215B RID: 8539
	[NonSerialized]
	public int activatedTimeStamp;

	// Token: 0x0400215C RID: 8540
	[HideInInspector]
	public int preventSnapUntilMoved;

	// Token: 0x0400215D RID: 8541
	[HideInInspector]
	public Vector3 preventSnapUntilMovedFromPos;

	// Token: 0x0400215E RID: 8542
	[HideInInspector]
	public BuilderPiece requestedParentPiece;

	// Token: 0x0400215F RID: 8543
	public PieceFallbackInfo fallbackInfo;

	// Token: 0x04002160 RID: 8544
	[NonSerialized]
	public bool overrideSavedPiece;

	// Token: 0x04002161 RID: 8545
	[NonSerialized]
	public int savedPieceType = -1;

	// Token: 0x04002162 RID: 8546
	[NonSerialized]
	public int savedMaterialType = -1;

	// Token: 0x04002163 RID: 8547
	[Header("Mesh Combining")]
	public List<MeshRenderer> meshesToCombine;

	// Token: 0x04002164 RID: 8548
	public GameObject bumpPrefab;

	// Token: 0x04002165 RID: 8549
	public List<GameObject> bumps;

	// Token: 0x04002166 RID: 8550
	[Header("Old")]
	public List<GameObject> moveToRoot;

	// Token: 0x04002167 RID: 8551
	public BasePlatform platform;

	// Token: 0x04002168 RID: 8552
	[HideInInspector]
	public BuilderPiece.State state;

	// Token: 0x04002169 RID: 8553
	[HideInInspector]
	public bool isStatic;

	// Token: 0x0400216A RID: 8554
	[HideInInspector]
	public List<MeshRenderer> renderingIndirect;

	// Token: 0x0400216B RID: 8555
	[HideInInspector]
	public List<int> renderingIndirectTransformIndex;

	// Token: 0x0400216C RID: 8556
	[HideInInspector]
	public float tint;

	// Token: 0x0400216D RID: 8557
	private int paintingCount;

	// Token: 0x0400216E RID: 8558
	private int potentialGrabCount;

	// Token: 0x0400216F RID: 8559
	private int potentialGrabChildCount;

	// Token: 0x020004DC RID: 1244
	public enum State
	{
		// Token: 0x04002171 RID: 8561
		None = -1,
		// Token: 0x04002172 RID: 8562
		AttachedAndPlaced,
		// Token: 0x04002173 RID: 8563
		AttachedToDropped,
		// Token: 0x04002174 RID: 8564
		Grabbed,
		// Token: 0x04002175 RID: 8565
		Dropped,
		// Token: 0x04002176 RID: 8566
		OnShelf,
		// Token: 0x04002177 RID: 8567
		Displayed,
		// Token: 0x04002178 RID: 8568
		GrabbedLocal,
		// Token: 0x04002179 RID: 8569
		OnConveyor,
		// Token: 0x0400217A RID: 8570
		AttachedToArm
	}
}
