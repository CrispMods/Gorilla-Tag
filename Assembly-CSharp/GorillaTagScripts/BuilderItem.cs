using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009AF RID: 2479
	public class BuilderItem : TransferrableObject
	{
		// Token: 0x06003CD9 RID: 15577 RVA: 0x00057B4F File Offset: 0x00055D4F
		public override bool ShouldBeKinematic()
		{
			return this.itemState == TransferrableObject.ItemStates.State2 || this.itemState == TransferrableObject.ItemStates.State4 || base.ShouldBeKinematic();
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x00156578 File Offset: 0x00154778
		protected override void Awake()
		{
			base.Awake();
			this.parent = base.transform.parent;
			this.currTable = null;
			this.initialPosition = base.transform.position;
			this.initialRotation = base.transform.rotation;
			this.initialGrabInteractorScale = this.gripInteractor.transform.localScale;
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x000407F7 File Offset: 0x0003E9F7
		internal override void OnEnable()
		{
			base.OnEnable();
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x0003416B File Offset: 0x0003236B
		internal override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x00057B6C File Offset: 0x00055D6C
		protected override void Start()
		{
			base.Start();
			this.itemState = TransferrableObject.ItemStates.State4;
			this.currentState = TransferrableObject.PositionState.Dropped;
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x001565DC File Offset: 0x001547DC
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

		// Token: 0x06003CDF RID: 15583 RVA: 0x00156674 File Offset: 0x00154874
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

		// Token: 0x06003CE0 RID: 15584 RVA: 0x0015671C File Offset: 0x0015491C
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

		// Token: 0x06003CE1 RID: 15585 RVA: 0x00156790 File Offset: 0x00154990
		public override Matrix4x4 GetDefaultTransformationMatrix()
		{
			if (this.reliableState.dirty)
			{
				base.SetupHandMatrix(this.reliableState.leftHandAttachPos, this.reliableState.leftHandAttachRot, this.reliableState.rightHandAttachPos, this.reliableState.rightHandAttachRot);
				this.reliableState.dirty = false;
			}
			return base.GetDefaultTransformationMatrix();
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x001567F0 File Offset: 0x001549F0
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

		// Token: 0x06003CE3 RID: 15587 RVA: 0x001568A8 File Offset: 0x00154AA8
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

		// Token: 0x06003CE4 RID: 15588 RVA: 0x00057B87 File Offset: 0x00055D87
		protected override void LateUpdateLocal()
		{
			base.LateUpdateLocal();
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x00057B8F File Offset: 0x00055D8F
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			if (GorillaTagger.Instance.offlineVRRig.scaleFactor < 1f)
			{
				return;
			}
			base.OnGrab(pointGrabbed, grabbingHand);
			this.itemState = TransferrableObject.ItemStates.State0;
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x00057BB7 File Offset: 0x00055DB7
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

		// Token: 0x06003CE7 RID: 15591 RVA: 0x00057BF2 File Offset: 0x00055DF2
		public void OnHoverOverTableStart(BuilderTable table)
		{
			this.currTable = table;
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x00057BFB File Offset: 0x00055DFB
		public void OnHoverOverTableEnd(BuilderTable table)
		{
			this.currTable = null;
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x00057C04 File Offset: 0x00055E04
		public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x001568E4 File Offset: 0x00154AE4
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

		// Token: 0x06003CEB RID: 15595 RVA: 0x00044AC5 File Offset: 0x00042CC5
		private void PlayVFX(GameObject vfx)
		{
			ObjectPools.instance.Instantiate(vfx, base.transform.position);
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x00057C0C File Offset: 0x00055E0C
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

		// Token: 0x06003CED RID: 15597 RVA: 0x00057C45 File Offset: 0x00055E45
		private bool ShouldPlayFX()
		{
			return this.previousItemState == BuilderItem.BuilderItemState.isHeld || this.previousItemState == BuilderItem.BuilderItemState.dropped;
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x00057C5C File Offset: 0x00055E5C
		public static GameObject BuildEnvItem(int prefabHash, Vector3 position, Quaternion rotation)
		{
			GameObject gameObject = ObjectPools.instance.Instantiate(prefabHash);
			gameObject.transform.SetPositionAndRotation(position, rotation);
			return gameObject;
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x00156968 File Offset: 0x00154B68
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

		// Token: 0x06003CF0 RID: 15600 RVA: 0x00057C76 File Offset: 0x00055E76
		public int GetPhotonViewId()
		{
			if (this.worldShareableInstance == null)
			{
				return -1;
			}
			return this.worldShareableInstance.ViewID;
		}

		// Token: 0x04003DC5 RID: 15813
		public BuilderItemReliableState reliableState;

		// Token: 0x04003DC6 RID: 15814
		public string builtItemPath;

		// Token: 0x04003DC7 RID: 15815
		public GameObject itemRoot;

		// Token: 0x04003DC8 RID: 15816
		private bool enableCollidersWhenReady;

		// Token: 0x04003DC9 RID: 15817
		private float handsFreeOfCollidersTime;

		// Token: 0x04003DCA RID: 15818
		[NonSerialized]
		public BuilderPiece attachedPiece;

		// Token: 0x04003DCB RID: 15819
		public List<Behaviour> onlyWhenPlacedBehaviours;

		// Token: 0x04003DCC RID: 15820
		[NonSerialized]
		public BuilderItem parentItem;

		// Token: 0x04003DCD RID: 15821
		public List<BuilderAttachGridPlane> gridPlanes;

		// Token: 0x04003DCE RID: 15822
		public List<BuilderAttachEdge> edges;

		// Token: 0x04003DCF RID: 15823
		private List<Collider> colliders;

		// Token: 0x04003DD0 RID: 15824
		private Transform parent;

		// Token: 0x04003DD1 RID: 15825
		private Vector3 initialPosition;

		// Token: 0x04003DD2 RID: 15826
		private Quaternion initialRotation;

		// Token: 0x04003DD3 RID: 15827
		private Vector3 initialGrabInteractorScale;

		// Token: 0x04003DD4 RID: 15828
		private BuilderTable currTable;

		// Token: 0x04003DD5 RID: 15829
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04003DD6 RID: 15830
		public AudioClip snapAudio;

		// Token: 0x04003DD7 RID: 15831
		public AudioClip placeAudio;

		// Token: 0x04003DD8 RID: 15832
		public GameObject placeVFX;

		// Token: 0x04003DD9 RID: 15833
		private BuilderItem.BuilderItemState previousItemState = BuilderItem.BuilderItemState.dropped;

		// Token: 0x020009B0 RID: 2480
		private enum BuilderItemState
		{
			// Token: 0x04003DDB RID: 15835
			isHeld = 1,
			// Token: 0x04003DDC RID: 15836
			dropped,
			// Token: 0x04003DDD RID: 15837
			placed = 4,
			// Token: 0x04003DDE RID: 15838
			unused0 = 8,
			// Token: 0x04003DDF RID: 15839
			none = 16
		}
	}
}
