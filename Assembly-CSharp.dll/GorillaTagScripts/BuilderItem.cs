using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200098C RID: 2444
	public class BuilderItem : TransferrableObject
	{
		// Token: 0x06003BCD RID: 15309 RVA: 0x000562B8 File Offset: 0x000544B8
		public override bool ShouldBeKinematic()
		{
			return this.itemState == TransferrableObject.ItemStates.State2 || this.itemState == TransferrableObject.ItemStates.State4 || base.ShouldBeKinematic();
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x00150590 File Offset: 0x0014E790
		protected override void Awake()
		{
			base.Awake();
			this.parent = base.transform.parent;
			this.currTable = null;
			this.initialPosition = base.transform.position;
			this.initialRotation = base.transform.rotation;
			this.initialGrabInteractorScale = this.gripInteractor.transform.localScale;
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x0003F50D File Offset: 0x0003D70D
		internal override void OnEnable()
		{
			base.OnEnable();
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x00032F07 File Offset: 0x00031107
		internal override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x000562D5 File Offset: 0x000544D5
		protected override void Start()
		{
			base.Start();
			this.itemState = TransferrableObject.ItemStates.State4;
			this.currentState = TransferrableObject.PositionState.Dropped;
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x001505F4 File Offset: 0x0014E7F4
		public void AttachPiece(BuilderPiece piece)
		{
			base.transform.SetPositionAndRotation(piece.transform.position, piece.transform.rotation);
			piece.transform.localScale = Vector3.one;
			piece.transform.SetParent(this.itemRoot.transform);
			Debug.LogFormat(piece.gameObject, "Attach Piece {0} to container {1}", new object[]
			{
				piece.gameObject.GetInstanceID(),
				base.gameObject.GetInstanceID()
			});
			this.attachedPiece = piece;
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x0015068C File Offset: 0x0014E88C
		public void DetachPiece(BuilderPiece piece)
		{
			if (piece != this.attachedPiece)
			{
				Debug.LogErrorFormat("Trying to detach piece {0} from a container containing {1}", new object[]
				{
					piece.pieceId,
					this.attachedPiece.pieceId
				});
				return;
			}
			piece.transform.SetParent(null);
			Debug.LogFormat(this.attachedPiece.gameObject, "Detach Piece {0} from container {1}", new object[]
			{
				this.attachedPiece.gameObject.GetInstanceID(),
				base.gameObject.GetInstanceID()
			});
			this.attachedPiece = null;
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x00150734 File Offset: 0x0014E934
		private new void OnStateChanged()
		{
			if (this.itemState == TransferrableObject.ItemStates.State2)
			{
				this.enableCollidersWhenReady = true;
				this.gripInteractor.transform.localScale = this.initialGrabInteractorScale * 2f;
				this.handsFreeOfCollidersTime = 0f;
				return;
			}
			this.enableCollidersWhenReady = false;
			this.gripInteractor.transform.localScale = this.initialGrabInteractorScale;
			this.handsFreeOfCollidersTime = 0f;
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x001507A8 File Offset: 0x0014E9A8
		public override Matrix4x4 GetDefaultTransformationMatrix()
		{
			if (this.reliableState.dirty)
			{
				base.SetupHandMatrix(this.reliableState.leftHandAttachPos, this.reliableState.leftHandAttachRot, this.reliableState.rightHandAttachPos, this.reliableState.rightHandAttachRot);
				this.reliableState.dirty = false;
			}
			return base.GetDefaultTransformationMatrix();
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x00150808 File Offset: 0x0014EA08
		protected override void LateUpdateShared()
		{
			base.LateUpdateShared();
			if (base.InHand())
			{
				this.itemState = TransferrableObject.ItemStates.State0;
			}
			BuilderItem.BuilderItemState itemState = (BuilderItem.BuilderItemState)this.itemState;
			if (itemState != this.previousItemState)
			{
				this.OnStateChanged();
			}
			this.previousItemState = itemState;
			if (this.enableCollidersWhenReady)
			{
				bool flag = this.IsOverlapping(EquipmentInteractor.instance.overlapInteractionPointsRight) || this.IsOverlapping(EquipmentInteractor.instance.overlapInteractionPointsLeft);
				this.handsFreeOfCollidersTime += (flag ? 0f : Time.deltaTime);
				if (this.handsFreeOfCollidersTime > 0.1f)
				{
					this.gripInteractor.transform.localScale = this.initialGrabInteractorScale;
					this.enableCollidersWhenReady = false;
				}
			}
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x001508C0 File Offset: 0x0014EAC0
		private bool IsOverlapping(List<InteractionPoint> interactionPoints)
		{
			if (interactionPoints == null)
			{
				return false;
			}
			for (int i = 0; i < interactionPoints.Count; i++)
			{
				if (interactionPoints[i] == this.gripInteractor)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x000562F0 File Offset: 0x000544F0
		protected override void LateUpdateLocal()
		{
			base.LateUpdateLocal();
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x000562F8 File Offset: 0x000544F8
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			if (GorillaTagger.Instance.offlineVRRig.scaleFactor < 1f)
			{
				return;
			}
			base.OnGrab(pointGrabbed, grabbingHand);
			this.itemState = TransferrableObject.ItemStates.State0;
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x00056320 File Offset: 0x00054520
		public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
		{
			if (!base.OnRelease(zoneReleased, releasingHand))
			{
				return false;
			}
			this.itemState = TransferrableObject.ItemStates.State1;
			this.Reparent(null);
			this.parentItem = null;
			this.gripInteractor.transform.localScale = this.initialGrabInteractorScale;
			return true;
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x0005635B File Offset: 0x0005455B
		public void OnHoverOverTableStart(BuilderTable table)
		{
			this.currTable = table;
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x00056364 File Offset: 0x00054564
		public void OnHoverOverTableEnd(BuilderTable table)
		{
			this.currTable = null;
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x0005636D File Offset: 0x0005456D
		public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x001508FC File Offset: 0x0014EAFC
		public override void OnLeftRoom()
		{
			base.OnLeftRoom();
			base.transform.position = this.initialPosition;
			base.transform.rotation = this.initialRotation;
			if (this.worldShareableInstance != null)
			{
				this.worldShareableInstance.transform.position = this.initialPosition;
				this.worldShareableInstance.transform.rotation = this.initialRotation;
			}
			this.itemState = TransferrableObject.ItemStates.State4;
			this.currentState = TransferrableObject.PositionState.Dropped;
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x00043726 File Offset: 0x00041926
		private void PlayVFX(GameObject vfx)
		{
			ObjectPools.instance.Instantiate(vfx, base.transform.position);
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x00056375 File Offset: 0x00054575
		private bool Reparent(Transform _transform)
		{
			if (!this.allowReparenting)
			{
				return false;
			}
			if (this.parent)
			{
				this.parent.SetParent(_transform);
				base.transform.SetParent(this.parent);
				return true;
			}
			return false;
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x000563AE File Offset: 0x000545AE
		private bool ShouldPlayFX()
		{
			return this.previousItemState == BuilderItem.BuilderItemState.isHeld || this.previousItemState == BuilderItem.BuilderItemState.dropped;
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x000563C5 File Offset: 0x000545C5
		public static GameObject BuildEnvItem(int prefabHash, Vector3 position, Quaternion rotation)
		{
			GameObject gameObject = ObjectPools.instance.Instantiate(prefabHash);
			gameObject.transform.SetPositionAndRotation(position, rotation);
			return gameObject;
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x00150980 File Offset: 0x0014EB80
		protected override void OnHandMatrixUpdate(Vector3 localPosition, Quaternion localRotation, bool leftHand)
		{
			if (leftHand)
			{
				this.reliableState.leftHandAttachPos = localPosition;
				this.reliableState.leftHandAttachRot = localRotation;
			}
			else
			{
				this.reliableState.rightHandAttachPos = localPosition;
				this.reliableState.rightHandAttachRot = localRotation;
			}
			this.reliableState.dirty = true;
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x000563DF File Offset: 0x000545DF
		public int GetPhotonViewId()
		{
			if (this.worldShareableInstance == null)
			{
				return -1;
			}
			return this.worldShareableInstance.ViewID;
		}

		// Token: 0x04003CFD RID: 15613
		public BuilderItemReliableState reliableState;

		// Token: 0x04003CFE RID: 15614
		public string builtItemPath;

		// Token: 0x04003CFF RID: 15615
		public GameObject itemRoot;

		// Token: 0x04003D00 RID: 15616
		private bool enableCollidersWhenReady;

		// Token: 0x04003D01 RID: 15617
		private float handsFreeOfCollidersTime;

		// Token: 0x04003D02 RID: 15618
		[NonSerialized]
		public BuilderPiece attachedPiece;

		// Token: 0x04003D03 RID: 15619
		public List<Behaviour> onlyWhenPlacedBehaviours;

		// Token: 0x04003D04 RID: 15620
		[NonSerialized]
		public BuilderItem parentItem;

		// Token: 0x04003D05 RID: 15621
		public List<BuilderAttachGridPlane> gridPlanes;

		// Token: 0x04003D06 RID: 15622
		public List<BuilderAttachEdge> edges;

		// Token: 0x04003D07 RID: 15623
		private List<Collider> colliders;

		// Token: 0x04003D08 RID: 15624
		private Transform parent;

		// Token: 0x04003D09 RID: 15625
		private Vector3 initialPosition;

		// Token: 0x04003D0A RID: 15626
		private Quaternion initialRotation;

		// Token: 0x04003D0B RID: 15627
		private Vector3 initialGrabInteractorScale;

		// Token: 0x04003D0C RID: 15628
		private BuilderTable currTable;

		// Token: 0x04003D0D RID: 15629
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04003D0E RID: 15630
		public AudioClip snapAudio;

		// Token: 0x04003D0F RID: 15631
		public AudioClip placeAudio;

		// Token: 0x04003D10 RID: 15632
		public GameObject placeVFX;

		// Token: 0x04003D11 RID: 15633
		private BuilderItem.BuilderItemState previousItemState = BuilderItem.BuilderItemState.dropped;

		// Token: 0x0200098D RID: 2445
		private enum BuilderItemState
		{
			// Token: 0x04003D13 RID: 15635
			isHeld = 1,
			// Token: 0x04003D14 RID: 15636
			dropped,
			// Token: 0x04003D15 RID: 15637
			placed = 4,
			// Token: 0x04003D16 RID: 15638
			unused0 = 8,
			// Token: 0x04003D17 RID: 15639
			none = 16
		}
	}
}
