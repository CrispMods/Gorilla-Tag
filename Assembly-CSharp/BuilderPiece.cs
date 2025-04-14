using System;
using System.Collections.Generic;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004CE RID: 1230
public class BuilderPiece : MonoBehaviour
{
	// Token: 0x06001DC3 RID: 7619 RVA: 0x00091828 File Offset: 0x0008FA28
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

	// Token: 0x06001DC4 RID: 7620 RVA: 0x00091ACC File Offset: 0x0008FCCC
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

	// Token: 0x06001DC5 RID: 7621 RVA: 0x00091C45 File Offset: 0x0008FE45
	public void OnCreatedByPool()
	{
		this.materialSwapTargets = new List<MeshRenderer>(4);
		base.GetComponentsInChildren<MeshRenderer>(this.areMeshesToggledOnPlace, this.materialSwapTargets);
		this.surfaceOverrides = new List<GorillaSurfaceOverride>(4);
		base.GetComponentsInChildren<GorillaSurfaceOverride>(this.areMeshesToggledOnPlace, this.surfaceOverrides);
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x00091C84 File Offset: 0x0008FE84
	public void SetupPiece(float gridSize)
	{
		for (int i = 0; i < this.gridPlanes.Count; i++)
		{
			this.gridPlanes[i].Setup(this, i, gridSize);
		}
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x00091CBC File Offset: 0x0008FEBC
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

	// Token: 0x06001DC8 RID: 7624 RVA: 0x00091E50 File Offset: 0x00090050
	public int GetPieceId()
	{
		return this.pieceId;
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x00091E58 File Offset: 0x00090058
	public int GetParentPieceId()
	{
		if (!(this.parentPiece == null))
		{
			return this.parentPiece.pieceId;
		}
		return -1;
	}

	// Token: 0x06001DCA RID: 7626 RVA: 0x00091E75 File Offset: 0x00090075
	public int GetAttachIndex()
	{
		return this.attachIndex;
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x00091E7D File Offset: 0x0009007D
	public int GetParentAttachIndex()
	{
		return this.parentAttachIndex;
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x00091E88 File Offset: 0x00090088
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

	// Token: 0x06001DCD RID: 7629 RVA: 0x00091EE0 File Offset: 0x000900E0
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

	// Token: 0x06001DCE RID: 7630 RVA: 0x00091F28 File Offset: 0x00090128
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

	// Token: 0x06001DCF RID: 7631 RVA: 0x00091F70 File Offset: 0x00090170
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

	// Token: 0x06001DD0 RID: 7632 RVA: 0x00091FC0 File Offset: 0x000901C0
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

	// Token: 0x06001DD1 RID: 7633 RVA: 0x00091FFE File Offset: 0x000901FE
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

	// Token: 0x06001DD2 RID: 7634 RVA: 0x00092033 File Offset: 0x00090233
	public void SetScale(float scale)
	{
		if (this.scaleRoot != null)
		{
			this.scaleRoot.localScale = Vector3.one * scale;
		}
	}

	// Token: 0x06001DD3 RID: 7635 RVA: 0x00092059 File Offset: 0x00090259
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

	// Token: 0x06001DD4 RID: 7636 RVA: 0x00092098 File Offset: 0x00090298
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

	// Token: 0x06001DD5 RID: 7637 RVA: 0x000920F4 File Offset: 0x000902F4
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

	// Token: 0x06001DD6 RID: 7638 RVA: 0x00092170 File Offset: 0x00090370
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

	// Token: 0x06001DD7 RID: 7639 RVA: 0x0009222E File Offset: 0x0009042E
	private void SetTint(float tint)
	{
		if (tint == this.tint)
		{
			return;
		}
		this.tint = tint;
		BuilderTable.instance.builderRenderer.SetPieceTint(this, tint);
	}

	// Token: 0x06001DD8 RID: 7640 RVA: 0x00092254 File Offset: 0x00090454
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

	// Token: 0x06001DD9 RID: 7641 RVA: 0x0009230C File Offset: 0x0009050C
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

	// Token: 0x06001DDA RID: 7642 RVA: 0x00092398 File Offset: 0x00090598
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

	// Token: 0x06001DDB RID: 7643 RVA: 0x000923F0 File Offset: 0x000905F0
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

	// Token: 0x06001DDC RID: 7644 RVA: 0x0009245C File Offset: 0x0009065C
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

	// Token: 0x06001DDD RID: 7645 RVA: 0x00092560 File Offset: 0x00090760
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

	// Token: 0x06001DDE RID: 7646 RVA: 0x000925D8 File Offset: 0x000907D8
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

	// Token: 0x06001DDF RID: 7647 RVA: 0x0009264E File Offset: 0x0009084E
	public bool IsHeldLocal()
	{
		return this.heldByPlayerActorNumber != -1 && this.heldByPlayerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber;
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x0009266D File Offset: 0x0009086D
	public bool IsHeldBy(int actorNumber)
	{
		return actorNumber != -1 && this.heldByPlayerActorNumber == actorNumber;
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x0009267E File Offset: 0x0009087E
	public bool IsHeldInLeftHand()
	{
		return this.heldInLeftHand;
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x00092686 File Offset: 0x00090886
	public static bool IsDroppedState(BuilderPiece.State state)
	{
		return state == BuilderPiece.State.Dropped || state == BuilderPiece.State.AttachedToDropped || state == BuilderPiece.State.OnShelf || state == BuilderPiece.State.OnConveyor;
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x0009269C File Offset: 0x0009089C
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

	// Token: 0x06001DE4 RID: 7652 RVA: 0x000926D0 File Offset: 0x000908D0
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

	// Token: 0x06001DE5 RID: 7653 RVA: 0x00092C20 File Offset: 0x00090E20
	public void SetKinematic(bool kinematic, bool destroyImmediate = true)
	{
		if (kinematic && this.rigidBody != null)
		{
			if (destroyImmediate)
			{
				Object.DestroyImmediate(this.rigidBody);
				this.rigidBody = null;
			}
			else
			{
				Object.Destroy(this.rigidBody);
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

	// Token: 0x06001DE6 RID: 7654 RVA: 0x000023F4 File Offset: 0x000005F4
	public void SetStatic(bool isStatic, bool force = false)
	{
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x00092D28 File Offset: 0x00090F28
	private void SetChildrenState(BuilderPiece.State newState, bool force)
	{
		BuilderPiece builderPiece = this.firstChildPiece;
		while (builderPiece != null)
		{
			builderPiece.SetState(newState, force);
			builderPiece = builderPiece.nextSiblingPiece;
		}
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x00092D58 File Offset: 0x00090F58
	public void OnCreate()
	{
		for (int i = 0; i < this.pieceComponents.Count; i++)
		{
			this.pieceComponents[i].OnPieceCreate(this.pieceType, this.pieceId);
		}
	}

	// Token: 0x06001DE9 RID: 7657 RVA: 0x00092D98 File Offset: 0x00090F98
	public void OnPlacementDeserialized()
	{
		for (int i = 0; i < this.pieceComponents.Count; i++)
		{
			this.pieceComponents[i].OnPiecePlacementDeserialized();
		}
	}

	// Token: 0x06001DEA RID: 7658 RVA: 0x00092DCC File Offset: 0x00090FCC
	public void PlayPlacementFx()
	{
		this.PlayVFX(this.vFXInfo.placeVFX);
	}

	// Token: 0x06001DEB RID: 7659 RVA: 0x00092DDF File Offset: 0x00090FDF
	public void PlayDisconnectFx()
	{
		this.PlayVFX(this.vFXInfo.disconnectVFX);
	}

	// Token: 0x06001DEC RID: 7660 RVA: 0x00092DF2 File Offset: 0x00090FF2
	public void PlayGrabbedFx()
	{
		this.PlayVFX(this.vFXInfo.grabbedVFX);
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x00092E05 File Offset: 0x00091005
	public void PlayTooHeavyFx()
	{
		this.PlayVFX(this.vFXInfo.tooHeavyVFX);
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x00092E18 File Offset: 0x00091018
	public void PlayLocationLockFx()
	{
		this.PlayVFX(this.vFXInfo.locationLockVFX);
	}

	// Token: 0x06001DEF RID: 7663 RVA: 0x00092E2B File Offset: 0x0009102B
	public void PlayRecycleFx()
	{
		this.PlayVFX(this.vFXInfo.recycleVFX);
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x00092E3E File Offset: 0x0009103E
	private void PlayVFX(GameObject vfx)
	{
		ObjectPools.instance.Instantiate(vfx, base.transform.position);
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x00092E58 File Offset: 0x00091058
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

	// Token: 0x06001DF2 RID: 7666 RVA: 0x00092E88 File Offset: 0x00091088
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

	// Token: 0x06001DF3 RID: 7667 RVA: 0x00092EBC File Offset: 0x000910BC
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

	// Token: 0x06001DF4 RID: 7668 RVA: 0x00092F20 File Offset: 0x00091120
	public BuilderPiece GetRootPiece()
	{
		BuilderPiece builderPiece = this;
		while (builderPiece.parentPiece != null && !builderPiece.parentPiece.isBuiltIntoTable)
		{
			builderPiece = builderPiece.parentPiece;
		}
		return builderPiece;
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x00092F54 File Offset: 0x00091154
	public bool IsPrivatePlot()
	{
		return this.isPrivatePlot;
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x00092F5C File Offset: 0x0009115C
	public bool TryGetPlotComponent(out BuilderPiecePrivatePlot plot)
	{
		plot = this.plotComponent;
		return this.isPrivatePlot;
	}

	// Token: 0x06001DF7 RID: 7671 RVA: 0x00092F74 File Offset: 0x00091174
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

	// Token: 0x06001DF8 RID: 7672 RVA: 0x0009300C File Offset: 0x0009120C
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

	// Token: 0x06001DF9 RID: 7673 RVA: 0x00093068 File Offset: 0x00091268
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

	// Token: 0x06001DFA RID: 7674 RVA: 0x00093104 File Offset: 0x00091304
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

	// Token: 0x06001DFB RID: 7675 RVA: 0x0009314C File Offset: 0x0009134C
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

	// Token: 0x06001DFC RID: 7676 RVA: 0x000931E0 File Offset: 0x000913E0
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

	// Token: 0x06001DFD RID: 7677 RVA: 0x00093284 File Offset: 0x00091484
	public int GetChildCount()
	{
		int num = 0;
		foreach (BuilderAttachGridPlane builderAttachGridPlane in this.gridPlanes)
		{
			num += builderAttachGridPlane.GetChildCount();
		}
		return num;
	}

	// Token: 0x06001DFE RID: 7678 RVA: 0x000932DC File Offset: 0x000914DC
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

	// Token: 0x06001DFF RID: 7679 RVA: 0x00093370 File Offset: 0x00091570
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

	// Token: 0x06001E00 RID: 7680 RVA: 0x00093410 File Offset: 0x00091610
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

	// Token: 0x06001E01 RID: 7681 RVA: 0x00093560 File Offset: 0x00091760
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

	// Token: 0x06001E02 RID: 7682 RVA: 0x000935D8 File Offset: 0x000917D8
	public int GetPiecePlacement()
	{
		byte pieceTwist = this.GetPieceTwist();
		sbyte xOffset;
		sbyte zOffset;
		this.GetPieceBumpOffset(pieceTwist, out xOffset, out zOffset);
		return BuilderTable.PackPiecePlacement(pieceTwist, xOffset, zOffset);
	}

	// Token: 0x06001E03 RID: 7683 RVA: 0x00093600 File Offset: 0x00091800
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

	// Token: 0x06001E04 RID: 7684 RVA: 0x000936B4 File Offset: 0x000918B4
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

	// Token: 0x040020DB RID: 8411
	public const int INVALID = -1;

	// Token: 0x040020DC RID: 8412
	public const float LIGHT_MASS = 1f;

	// Token: 0x040020DD RID: 8413
	public const float HEAVY_MASS = 10000f;

	// Token: 0x040020DE RID: 8414
	[Header("Piece Properties")]
	public string displayName;

	// Token: 0x040020DF RID: 8415
	public BuilderMaterialOptions materialOptions;

	// Token: 0x040020E0 RID: 8416
	public BuilderResources cost;

	// Token: 0x040020E1 RID: 8417
	private List<MeshRenderer> materialSwapTargets;

	// Token: 0x040020E2 RID: 8418
	private List<GorillaSurfaceOverride> surfaceOverrides;

	// Token: 0x040020E3 RID: 8419
	public Transform scaleRoot;

	// Token: 0x040020E4 RID: 8420
	public bool isBuiltIntoTable;

	// Token: 0x040020E5 RID: 8421
	public bool isArmShelf;

	// Token: 0x040020E6 RID: 8422
	[HideInInspector]
	public BuilderArmShelf armShelf;

	// Token: 0x040020E7 RID: 8423
	public Vector3 desiredShelfOffset = Vector3.zero;

	// Token: 0x040020E8 RID: 8424
	public Vector3 desiredShelfRotationOffset = Vector3.zero;

	// Token: 0x040020E9 RID: 8425
	private bool isPrivatePlot;

	// Token: 0x040020EA RID: 8426
	[HideInInspector]
	public int privatePlotIndex;

	// Token: 0x040020EB RID: 8427
	public BuilderPiecePrivatePlot plotComponent;

	// Token: 0x040020EC RID: 8428
	[Header("VFX")]
	[SerializeField]
	private BuilderPieceEffectInfo vFXInfo;

	// Token: 0x040020ED RID: 8429
	[Header("Piece Properties")]
	public int pieceType;

	// Token: 0x040020EE RID: 8430
	public int pieceId;

	// Token: 0x040020EF RID: 8431
	public int pieceDataIndex;

	// Token: 0x040020F0 RID: 8432
	public int materialType = -1;

	// Token: 0x040020F1 RID: 8433
	public bool suppressMaterialWarnings;

	// Token: 0x040020F2 RID: 8434
	public int heldByPlayerActorNumber;

	// Token: 0x040020F3 RID: 8435
	public bool heldInLeftHand;

	// Token: 0x040020F4 RID: 8436
	public Transform parentHeld;

	// Token: 0x040020F5 RID: 8437
	[HideInInspector]
	public BuilderPiece parentPiece;

	// Token: 0x040020F6 RID: 8438
	[HideInInspector]
	public BuilderPiece firstChildPiece;

	// Token: 0x040020F7 RID: 8439
	[HideInInspector]
	public BuilderPiece nextSiblingPiece;

	// Token: 0x040020F8 RID: 8440
	[HideInInspector]
	public int attachIndex;

	// Token: 0x040020F9 RID: 8441
	[HideInInspector]
	public int parentAttachIndex;

	// Token: 0x040020FA RID: 8442
	public int shelfOwner = -1;

	// Token: 0x040020FB RID: 8443
	[HideInInspector]
	public List<BuilderAttachGridPlane> gridPlanes;

	// Token: 0x040020FC RID: 8444
	[HideInInspector]
	public List<Collider> colliders;

	// Token: 0x040020FD RID: 8445
	public List<Collider> placedOnlyColliders;

	// Token: 0x040020FE RID: 8446
	[Header("Toggle on Place")]
	public List<Behaviour> onlyWhenPlacedBehaviours;

	// Token: 0x040020FF RID: 8447
	public List<IBuilderPieceComponent> pieceComponents;

	// Token: 0x04002100 RID: 8448
	public IBuilderPieceFunctional functionalPieceComponent;

	// Token: 0x04002101 RID: 8449
	public byte functionalPieceState;

	// Token: 0x04002102 RID: 8450
	public List<IBuilderPieceFunctional> pieceFunctionComponents;

	// Token: 0x04002103 RID: 8451
	private bool pieceComponentsActive;

	// Token: 0x04002104 RID: 8452
	public List<GameObject> onlyWhenPlaced;

	// Token: 0x04002105 RID: 8453
	public List<GameObject> onlyWhenNotPlaced;

	// Token: 0x04002106 RID: 8454
	public bool areMeshesToggledOnPlace;

	// Token: 0x04002107 RID: 8455
	[NonSerialized]
	public Rigidbody rigidBody;

	// Token: 0x04002108 RID: 8456
	[NonSerialized]
	public int activatedTimeStamp;

	// Token: 0x04002109 RID: 8457
	[HideInInspector]
	public int preventSnapUntilMoved;

	// Token: 0x0400210A RID: 8458
	[HideInInspector]
	public Vector3 preventSnapUntilMovedFromPos;

	// Token: 0x0400210B RID: 8459
	[HideInInspector]
	public BuilderPiece requestedParentPiece;

	// Token: 0x0400210C RID: 8460
	public PieceFallbackInfo fallbackInfo;

	// Token: 0x0400210D RID: 8461
	[NonSerialized]
	public bool overrideSavedPiece;

	// Token: 0x0400210E RID: 8462
	[NonSerialized]
	public int savedPieceType = -1;

	// Token: 0x0400210F RID: 8463
	[NonSerialized]
	public int savedMaterialType = -1;

	// Token: 0x04002110 RID: 8464
	[Header("Mesh Combining")]
	public List<MeshRenderer> meshesToCombine;

	// Token: 0x04002111 RID: 8465
	public GameObject bumpPrefab;

	// Token: 0x04002112 RID: 8466
	public List<GameObject> bumps;

	// Token: 0x04002113 RID: 8467
	[Header("Old")]
	public List<GameObject> moveToRoot;

	// Token: 0x04002114 RID: 8468
	public BasePlatform platform;

	// Token: 0x04002115 RID: 8469
	[HideInInspector]
	public BuilderPiece.State state;

	// Token: 0x04002116 RID: 8470
	[HideInInspector]
	public bool isStatic;

	// Token: 0x04002117 RID: 8471
	[HideInInspector]
	public List<MeshRenderer> renderingIndirect;

	// Token: 0x04002118 RID: 8472
	[HideInInspector]
	public List<int> renderingIndirectTransformIndex;

	// Token: 0x04002119 RID: 8473
	[HideInInspector]
	public float tint;

	// Token: 0x0400211A RID: 8474
	private int paintingCount;

	// Token: 0x0400211B RID: 8475
	private int potentialGrabCount;

	// Token: 0x0400211C RID: 8476
	private int potentialGrabChildCount;

	// Token: 0x020004CF RID: 1231
	public enum State
	{
		// Token: 0x0400211E RID: 8478
		None = -1,
		// Token: 0x0400211F RID: 8479
		AttachedAndPlaced,
		// Token: 0x04002120 RID: 8480
		AttachedToDropped,
		// Token: 0x04002121 RID: 8481
		Grabbed,
		// Token: 0x04002122 RID: 8482
		Dropped,
		// Token: 0x04002123 RID: 8483
		OnShelf,
		// Token: 0x04002124 RID: 8484
		Displayed,
		// Token: 0x04002125 RID: 8485
		GrabbedLocal,
		// Token: 0x04002126 RID: 8486
		OnConveyor,
		// Token: 0x04002127 RID: 8487
		AttachedToArm
	}
}
