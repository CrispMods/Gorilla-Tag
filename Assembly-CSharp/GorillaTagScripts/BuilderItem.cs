using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000989 RID: 2441
	public class BuilderItem : TransferrableObject
	{
		// Token: 0x06003BC1 RID: 15297 RVA: 0x00113632 File Offset: 0x00111832
		public override bool ShouldBeKinematic()
		{
			return this.itemState == TransferrableObject.ItemStates.State2 || this.itemState == TransferrableObject.ItemStates.State4 || base.ShouldBeKinematic();
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x00113650 File Offset: 0x00111850
		protected override void Awake()
		{
			base.Awake();
			this.parent = base.transform.parent;
			this.currTable = null;
			this.initialPosition = base.transform.position;
			this.initialRotation = base.transform.rotation;
			this.initialGrabInteractorScale = this.gripInteractor.transform.localScale;
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x00074C6C File Offset: 0x00072E6C
		internal override void OnEnable()
		{
			base.OnEnable();
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x0001FBE3 File Offset: 0x0001DDE3
		internal override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x001136B3 File Offset: 0x001118B3
		protected override void Start()
		{
			base.Start();
			this.itemState = TransferrableObject.ItemStates.State4;
			this.currentState = TransferrableObject.PositionState.Dropped;
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x001136D0 File Offset: 0x001118D0
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

		// Token: 0x06003BC7 RID: 15303 RVA: 0x00113768 File Offset: 0x00111968
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

		// Token: 0x06003BC8 RID: 15304 RVA: 0x00113810 File Offset: 0x00111A10
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

		// Token: 0x06003BC9 RID: 15305 RVA: 0x00113884 File Offset: 0x00111A84
		public override Matrix4x4 GetDefaultTransformationMatrix()
		{
			if (this.reliableState.dirty)
			{
				base.SetupHandMatrix(this.reliableState.leftHandAttachPos, this.reliableState.leftHandAttachRot, this.reliableState.rightHandAttachPos, this.reliableState.rightHandAttachRot);
				this.reliableState.dirty = false;
			}
			return base.GetDefaultTransformationMatrix();
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x001138E4 File Offset: 0x00111AE4
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

		// Token: 0x06003BCB RID: 15307 RVA: 0x0011399C File Offset: 0x00111B9C
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

		// Token: 0x06003BCC RID: 15308 RVA: 0x001139D6 File Offset: 0x00111BD6
		protected override void LateUpdateLocal()
		{
			base.LateUpdateLocal();
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x001139DE File Offset: 0x00111BDE
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			if (GorillaTagger.Instance.offlineVRRig.scaleFactor < 1f)
			{
				return;
			}
			base.OnGrab(pointGrabbed, grabbingHand);
			this.itemState = TransferrableObject.ItemStates.State0;
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x00113A06 File Offset: 0x00111C06
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

		// Token: 0x06003BCF RID: 15311 RVA: 0x00113A41 File Offset: 0x00111C41
		public void OnHoverOverTableStart(BuilderTable table)
		{
			this.currTable = table;
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x00113A4A File Offset: 0x00111C4A
		public void OnHoverOverTableEnd(BuilderTable table)
		{
			this.currTable = null;
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x00113A53 File Offset: 0x00111C53
		public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x00113A5C File Offset: 0x00111C5C
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

		// Token: 0x06003BD3 RID: 15315 RVA: 0x00092E3E File Offset: 0x0009103E
		private void PlayVFX(GameObject vfx)
		{
			ObjectPools.instance.Instantiate(vfx, base.transform.position);
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x00113ADE File Offset: 0x00111CDE
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

		// Token: 0x06003BD5 RID: 15317 RVA: 0x00113B17 File Offset: 0x00111D17
		private bool ShouldPlayFX()
		{
			return this.previousItemState == BuilderItem.BuilderItemState.isHeld || this.previousItemState == BuilderItem.BuilderItemState.dropped;
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x00113B2E File Offset: 0x00111D2E
		public static GameObject BuildEnvItem(int prefabHash, Vector3 position, Quaternion rotation)
		{
			GameObject gameObject = ObjectPools.instance.Instantiate(prefabHash);
			gameObject.transform.SetPositionAndRotation(position, rotation);
			return gameObject;
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x00113B48 File Offset: 0x00111D48
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

		// Token: 0x06003BD8 RID: 15320 RVA: 0x00113B96 File Offset: 0x00111D96
		public int GetPhotonViewId()
		{
			if (this.worldShareableInstance == null)
			{
				return -1;
			}
			return this.worldShareableInstance.ViewID;
		}

		// Token: 0x04003CEB RID: 15595
		public BuilderItemReliableState reliableState;

		// Token: 0x04003CEC RID: 15596
		public string builtItemPath;

		// Token: 0x04003CED RID: 15597
		public GameObject itemRoot;

		// Token: 0x04003CEE RID: 15598
		private bool enableCollidersWhenReady;

		// Token: 0x04003CEF RID: 15599
		private float handsFreeOfCollidersTime;

		// Token: 0x04003CF0 RID: 15600
		[NonSerialized]
		public BuilderPiece attachedPiece;

		// Token: 0x04003CF1 RID: 15601
		public List<Behaviour> onlyWhenPlacedBehaviours;

		// Token: 0x04003CF2 RID: 15602
		[NonSerialized]
		public BuilderItem parentItem;

		// Token: 0x04003CF3 RID: 15603
		public List<BuilderAttachGridPlane> gridPlanes;

		// Token: 0x04003CF4 RID: 15604
		public List<BuilderAttachEdge> edges;

		// Token: 0x04003CF5 RID: 15605
		private List<Collider> colliders;

		// Token: 0x04003CF6 RID: 15606
		private Transform parent;

		// Token: 0x04003CF7 RID: 15607
		private Vector3 initialPosition;

		// Token: 0x04003CF8 RID: 15608
		private Quaternion initialRotation;

		// Token: 0x04003CF9 RID: 15609
		private Vector3 initialGrabInteractorScale;

		// Token: 0x04003CFA RID: 15610
		private BuilderTable currTable;

		// Token: 0x04003CFB RID: 15611
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04003CFC RID: 15612
		public AudioClip snapAudio;

		// Token: 0x04003CFD RID: 15613
		public AudioClip placeAudio;

		// Token: 0x04003CFE RID: 15614
		public GameObject placeVFX;

		// Token: 0x04003CFF RID: 15615
		private BuilderItem.BuilderItemState previousItemState = BuilderItem.BuilderItemState.dropped;

		// Token: 0x0200098A RID: 2442
		private enum BuilderItemState
		{
			// Token: 0x04003D01 RID: 15617
			isHeld = 1,
			// Token: 0x04003D02 RID: 15618
			dropped,
			// Token: 0x04003D03 RID: 15619
			placed = 4,
			// Token: 0x04003D04 RID: 15620
			unused0 = 8,
			// Token: 0x04003D05 RID: 15621
			none = 16
		}
	}
}
