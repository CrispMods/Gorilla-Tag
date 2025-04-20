using System;
using GorillaExtensions;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C64 RID: 3172
	public class HeadlessHead : HoldableObject
	{
		// Token: 0x06004F5C RID: 20316 RVA: 0x001B7418 File Offset: 0x001B5618
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

		// Token: 0x06004F5D RID: 20317 RVA: 0x001B749C File Offset: 0x001B569C
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

		// Token: 0x06004F5E RID: 20318 RVA: 0x00063D05 File Offset: 0x00061F05
		private void OnDisable()
		{
			this.ownerRig.bodyRenderer.SetCosmeticBodyType(GorillaBodyType.Default);
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x00063D18 File Offset: 0x00061F18
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

		// Token: 0x06004F60 RID: 20320 RVA: 0x00063D36 File Offset: 0x00061F36
		protected virtual void LateUpdateLocal()
		{
			this.ownerRig.WearablePackedStates = GTBitOps.WriteBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo, (this.isHeld ? 1 : 0) + (this.isHeldLeftHand ? 2 : 0));
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x001B74F8 File Offset: 0x001B56F8
		protected virtual void LateUpdateReplicated()
		{
			int num = GTBitOps.ReadBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo.index, this.stateBitsWriteInfo.valueMask);
			this.isHeld = (num != 0);
			this.isHeldLeftHand = ((num & 2) != 0);
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x001B7544 File Offset: 0x001B5744
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

		// Token: 0x06004F63 RID: 20323 RVA: 0x00030607 File Offset: 0x0002E807
		public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
		{
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x00063D72 File Offset: 0x00061F72
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			this.isHeld = true;
			this.isHeldLeftHand = (grabbingHand == EquipmentInteractor.instance.leftHand);
			EquipmentInteractor.instance.UpdateHandEquipment(this, this.isHeldLeftHand);
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x00063DA6 File Offset: 0x00061FA6
		public override void DropItemCleanup()
		{
			this.isHeld = false;
			this.isHeldLeftHand = false;
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x001B774C File Offset: 0x001B594C
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

		// Token: 0x04005222 RID: 21026
		[Tooltip("The slot this cosmetic resides.")]
		public VRRig.WearablePackedStateSlots wearablePackedStateSlot = VRRig.WearablePackedStateSlots.Face;

		// Token: 0x04005223 RID: 21027
		[SerializeField]
		private Vector3 offsetFromLeftHand = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x04005224 RID: 21028
		[SerializeField]
		private Vector3 offsetFromRightHand = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x04005225 RID: 21029
		[SerializeField]
		private Quaternion rotationFromLeftHand = Quaternion.Euler(14.063973f, 52.56744f, 10.067408f);

		// Token: 0x04005226 RID: 21030
		[SerializeField]
		private Quaternion rotationFromRightHand = Quaternion.Euler(14.063973f, 52.56744f, 10.067408f);

		// Token: 0x04005227 RID: 21031
		private Vector3 baseLocalPosition;

		// Token: 0x04005228 RID: 21032
		private VRRig ownerRig;

		// Token: 0x04005229 RID: 21033
		private bool isLocal;

		// Token: 0x0400522A RID: 21034
		private bool isHeld;

		// Token: 0x0400522B RID: 21035
		private bool isHeldLeftHand;

		// Token: 0x0400522C RID: 21036
		private GTBitOps.BitWriteInfo stateBitsWriteInfo;

		// Token: 0x0400522D RID: 21037
		[SerializeField]
		private MeshRenderer firstPersonRenderer;

		// Token: 0x0400522E RID: 21038
		[SerializeField]
		private float firstPersonHiddenRadius;

		// Token: 0x0400522F RID: 21039
		[SerializeField]
		private Transform firstPersonHideCenter;

		// Token: 0x04005230 RID: 21040
		[SerializeField]
		private Transform holdAnchorPoint;

		// Token: 0x04005231 RID: 21041
		private bool hasFirstPersonRenderer;

		// Token: 0x04005232 RID: 21042
		private Vector3 blendingFromPosition;

		// Token: 0x04005233 RID: 21043
		private Quaternion blendingFromRotation;

		// Token: 0x04005234 RID: 21044
		private float blendFraction;

		// Token: 0x04005235 RID: 21045
		private bool wasHeld;

		// Token: 0x04005236 RID: 21046
		private bool wasHeldLeftHand;

		// Token: 0x04005237 RID: 21047
		[SerializeField]
		private float blendDuration = 0.3f;
	}
}
