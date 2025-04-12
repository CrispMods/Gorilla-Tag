using System;
using GorillaExtensions;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C36 RID: 3126
	public class HeadlessHead : HoldableObject
	{
		// Token: 0x06004E08 RID: 19976 RVA: 0x001AF334 File Offset: 0x001AD534
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

		// Token: 0x06004E09 RID: 19977 RVA: 0x001AF3B8 File Offset: 0x001AD5B8
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

		// Token: 0x06004E0A RID: 19978 RVA: 0x000622E0 File Offset: 0x000604E0
		private void OnDisable()
		{
			this.ownerRig.bodyRenderer.SetCosmeticBodyType(GorillaBodyType.Default);
		}

		// Token: 0x06004E0B RID: 19979 RVA: 0x000622F3 File Offset: 0x000604F3
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

		// Token: 0x06004E0C RID: 19980 RVA: 0x00062311 File Offset: 0x00060511
		protected virtual void LateUpdateLocal()
		{
			this.ownerRig.WearablePackedStates = GTBitOps.WriteBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo, (this.isHeld ? 1 : 0) + (this.isHeldLeftHand ? 2 : 0));
		}

		// Token: 0x06004E0D RID: 19981 RVA: 0x001AF414 File Offset: 0x001AD614
		protected virtual void LateUpdateReplicated()
		{
			int num = GTBitOps.ReadBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo.index, this.stateBitsWriteInfo.valueMask);
			this.isHeld = (num != 0);
			this.isHeldLeftHand = ((num & 2) != 0);
		}

		// Token: 0x06004E0E RID: 19982 RVA: 0x001AF460 File Offset: 0x001AD660
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

		// Token: 0x06004E0F RID: 19983 RVA: 0x0002F75F File Offset: 0x0002D95F
		public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
		{
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x0006234D File Offset: 0x0006054D
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			this.isHeld = true;
			this.isHeldLeftHand = (grabbingHand == EquipmentInteractor.instance.leftHand);
			EquipmentInteractor.instance.UpdateHandEquipment(this, this.isHeldLeftHand);
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x00062381 File Offset: 0x00060581
		public override void DropItemCleanup()
		{
			this.isHeld = false;
			this.isHeldLeftHand = false;
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x001AF668 File Offset: 0x001AD868
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

		// Token: 0x04005128 RID: 20776
		[Tooltip("The slot this cosmetic resides.")]
		public VRRig.WearablePackedStateSlots wearablePackedStateSlot = VRRig.WearablePackedStateSlots.Face;

		// Token: 0x04005129 RID: 20777
		[SerializeField]
		private Vector3 offsetFromLeftHand = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x0400512A RID: 20778
		[SerializeField]
		private Vector3 offsetFromRightHand = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x0400512B RID: 20779
		[SerializeField]
		private Quaternion rotationFromLeftHand = Quaternion.Euler(14.063973f, 52.56744f, 10.067408f);

		// Token: 0x0400512C RID: 20780
		[SerializeField]
		private Quaternion rotationFromRightHand = Quaternion.Euler(14.063973f, 52.56744f, 10.067408f);

		// Token: 0x0400512D RID: 20781
		private Vector3 baseLocalPosition;

		// Token: 0x0400512E RID: 20782
		private VRRig ownerRig;

		// Token: 0x0400512F RID: 20783
		private bool isLocal;

		// Token: 0x04005130 RID: 20784
		private bool isHeld;

		// Token: 0x04005131 RID: 20785
		private bool isHeldLeftHand;

		// Token: 0x04005132 RID: 20786
		private GTBitOps.BitWriteInfo stateBitsWriteInfo;

		// Token: 0x04005133 RID: 20787
		[SerializeField]
		private MeshRenderer firstPersonRenderer;

		// Token: 0x04005134 RID: 20788
		[SerializeField]
		private float firstPersonHiddenRadius;

		// Token: 0x04005135 RID: 20789
		[SerializeField]
		private Transform firstPersonHideCenter;

		// Token: 0x04005136 RID: 20790
		[SerializeField]
		private Transform holdAnchorPoint;

		// Token: 0x04005137 RID: 20791
		private bool hasFirstPersonRenderer;

		// Token: 0x04005138 RID: 20792
		private Vector3 blendingFromPosition;

		// Token: 0x04005139 RID: 20793
		private Quaternion blendingFromRotation;

		// Token: 0x0400513A RID: 20794
		private float blendFraction;

		// Token: 0x0400513B RID: 20795
		private bool wasHeld;

		// Token: 0x0400513C RID: 20796
		private bool wasHeldLeftHand;

		// Token: 0x0400513D RID: 20797
		[SerializeField]
		private float blendDuration = 0.3f;
	}
}
