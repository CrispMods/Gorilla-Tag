using System;
using GorillaExtensions;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C33 RID: 3123
	public class HeadlessHead : HoldableObject
	{
		// Token: 0x06004DFC RID: 19964 RVA: 0x0017E9EC File Offset: 0x0017CBEC
		protected void Awake()
		{
			this.ownerRig = base.GetComponentInParent<VRRig>();
			if (this.ownerRig == null)
			{
				this.ownerRig = GorillaTagger.Instance.offlineVRRig;
			}
			this.isLocal = this.ownerRig.isOfflineVRRig;
			this.stateBitsWriteInfo = VRRig.WearablePackedStatesBitWriteInfos[(int)this.wearablePackedStateSlot];
			this.baseLocalPosition = base.transform.localPosition;
			this.hasFirstPersonRenderer = (this.firstPersonRenderer != null);
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x0017EA70 File Offset: 0x0017CC70
		protected void OnEnable()
		{
			if (this.ownerRig == null)
			{
				Debug.LogError("HeadlessHead \"" + base.transform.GetPath() + "\": Deactivating because ownerRig is null.", this);
				base.gameObject.SetActive(false);
				return;
			}
			this.ownerRig.bodyRenderer.SetCosmeticBodyType(GorillaBodyType.NoHead);
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x0017EAC9 File Offset: 0x0017CCC9
		private void OnDisable()
		{
			this.ownerRig.bodyRenderer.SetCosmeticBodyType(GorillaBodyType.Default);
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x0017EADC File Offset: 0x0017CCDC
		protected virtual void LateUpdate()
		{
			if (this.isLocal)
			{
				this.LateUpdateLocal();
			}
			else
			{
				this.LateUpdateReplicated();
			}
			this.LateUpdateShared();
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x0017EAFA File Offset: 0x0017CCFA
		protected virtual void LateUpdateLocal()
		{
			this.ownerRig.WearablePackedStates = GTBitOps.WriteBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo, (this.isHeld ? 1 : 0) + (this.isHeldLeftHand ? 2 : 0));
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x0017EB38 File Offset: 0x0017CD38
		protected virtual void LateUpdateReplicated()
		{
			int num = GTBitOps.ReadBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo.index, this.stateBitsWriteInfo.valueMask);
			this.isHeld = (num != 0);
			this.isHeldLeftHand = ((num & 2) != 0);
		}

		// Token: 0x06004E02 RID: 19970 RVA: 0x0017EB84 File Offset: 0x0017CD84
		protected virtual void LateUpdateShared()
		{
			if (this.isHeld != this.wasHeld || this.isHeldLeftHand != this.wasHeldLeftHand)
			{
				this.blendingFromPosition = base.transform.position;
				this.blendingFromRotation = base.transform.rotation;
				this.blendFraction = 0f;
			}
			Quaternion quaternion;
			Vector3 vector;
			if (this.isHeldLeftHand)
			{
				quaternion = this.ownerRig.leftHandTransform.rotation * this.rotationFromLeftHand;
				vector = this.ownerRig.leftHandTransform.TransformPoint(this.offsetFromLeftHand) - quaternion * this.holdAnchorPoint.transform.localPosition;
			}
			else if (this.isHeld)
			{
				quaternion = this.ownerRig.rightHandTransform.rotation * this.rotationFromRightHand;
				vector = this.ownerRig.rightHandTransform.TransformPoint(this.offsetFromRightHand) - quaternion * this.holdAnchorPoint.transform.localPosition;
			}
			else
			{
				quaternion = base.transform.parent.rotation;
				vector = base.transform.parent.TransformPoint(this.baseLocalPosition);
			}
			if (this.blendFraction < 1f)
			{
				this.blendFraction += Time.deltaTime / this.blendDuration;
				quaternion = Quaternion.Lerp(this.blendingFromRotation, quaternion, this.blendFraction);
				vector = Vector3.Lerp(this.blendingFromPosition, vector, this.blendFraction);
			}
			base.transform.rotation = quaternion;
			base.transform.position = vector;
			if (this.hasFirstPersonRenderer)
			{
				float x = base.transform.lossyScale.x;
				this.firstPersonRenderer.enabled = (this.firstPersonHideCenter.transform.position - GTPlayer.Instance.headCollider.transform.position).IsLongerThan(this.firstPersonHiddenRadius * x);
			}
			this.wasHeld = this.isHeld;
			this.wasHeldLeftHand = this.isHeldLeftHand;
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x000023F4 File Offset: 0x000005F4
		public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
		{
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x0017ED8B File Offset: 0x0017CF8B
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			this.isHeld = true;
			this.isHeldLeftHand = (grabbingHand == EquipmentInteractor.instance.leftHand);
			EquipmentInteractor.instance.UpdateHandEquipment(this, this.isHeldLeftHand);
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x0017EDBF File Offset: 0x0017CFBF
		public override void DropItemCleanup()
		{
			this.isHeld = false;
			this.isHeldLeftHand = false;
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x0017EDD0 File Offset: 0x0017CFD0
		public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
		{
			if (EquipmentInteractor.instance.rightHandHeldEquipment == this && releasingHand != EquipmentInteractor.instance.rightHand)
			{
				return false;
			}
			if (EquipmentInteractor.instance.leftHandHeldEquipment == this && releasingHand != EquipmentInteractor.instance.leftHand)
			{
				return false;
			}
			EquipmentInteractor.instance.UpdateHandEquipment(null, this.isHeldLeftHand);
			this.isHeld = false;
			this.isHeldLeftHand = false;
			return true;
		}

		// Token: 0x04005116 RID: 20758
		[Tooltip("The slot this cosmetic resides.")]
		public VRRig.WearablePackedStateSlots wearablePackedStateSlot = VRRig.WearablePackedStateSlots.Face;

		// Token: 0x04005117 RID: 20759
		[SerializeField]
		private Vector3 offsetFromLeftHand = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x04005118 RID: 20760
		[SerializeField]
		private Vector3 offsetFromRightHand = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x04005119 RID: 20761
		[SerializeField]
		private Quaternion rotationFromLeftHand = Quaternion.Euler(14.063973f, 52.56744f, 10.067408f);

		// Token: 0x0400511A RID: 20762
		[SerializeField]
		private Quaternion rotationFromRightHand = Quaternion.Euler(14.063973f, 52.56744f, 10.067408f);

		// Token: 0x0400511B RID: 20763
		private Vector3 baseLocalPosition;

		// Token: 0x0400511C RID: 20764
		private VRRig ownerRig;

		// Token: 0x0400511D RID: 20765
		private bool isLocal;

		// Token: 0x0400511E RID: 20766
		private bool isHeld;

		// Token: 0x0400511F RID: 20767
		private bool isHeldLeftHand;

		// Token: 0x04005120 RID: 20768
		private GTBitOps.BitWriteInfo stateBitsWriteInfo;

		// Token: 0x04005121 RID: 20769
		[SerializeField]
		private MeshRenderer firstPersonRenderer;

		// Token: 0x04005122 RID: 20770
		[SerializeField]
		private float firstPersonHiddenRadius;

		// Token: 0x04005123 RID: 20771
		[SerializeField]
		private Transform firstPersonHideCenter;

		// Token: 0x04005124 RID: 20772
		[SerializeField]
		private Transform holdAnchorPoint;

		// Token: 0x04005125 RID: 20773
		private bool hasFirstPersonRenderer;

		// Token: 0x04005126 RID: 20774
		private Vector3 blendingFromPosition;

		// Token: 0x04005127 RID: 20775
		private Quaternion blendingFromRotation;

		// Token: 0x04005128 RID: 20776
		private float blendFraction;

		// Token: 0x04005129 RID: 20777
		private bool wasHeld;

		// Token: 0x0400512A RID: 20778
		private bool wasHeldLeftHand;

		// Token: 0x0400512B RID: 20779
		[SerializeField]
		private float blendDuration = 0.3f;
	}
}
