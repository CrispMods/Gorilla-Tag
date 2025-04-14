using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C31 RID: 3121
	public class EdibleWearable : MonoBehaviour
	{
		// Token: 0x06004DF4 RID: 19956 RVA: 0x0017E464 File Offset: 0x0017C664
		protected void Awake()
		{
			this.edibleState = 0;
			this.previousEdibleState = 0;
			this.ownerRig = base.GetComponentInParent<VRRig>();
			this.isLocal = (this.ownerRig != null && this.ownerRig.isOfflineVRRig);
			this.isHandSlot = (this.wearablePackedStateSlot == VRRig.WearablePackedStateSlots.LeftHand || this.wearablePackedStateSlot == VRRig.WearablePackedStateSlots.RightHand);
			this.isLeftHand = (this.wearablePackedStateSlot == VRRig.WearablePackedStateSlots.LeftHand);
			this.stateBitsWriteInfo = VRRig.WearablePackedStatesBitWriteInfos[(int)this.wearablePackedStateSlot];
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x0017E4F0 File Offset: 0x0017C6F0
		protected void OnEnable()
		{
			if (this.ownerRig == null)
			{
				Debug.LogError("EdibleWearable \"" + base.transform.GetPath() + "\": Deactivating because ownerRig is null.", this);
				base.gameObject.SetActive(false);
				return;
			}
			for (int i = 0; i < this.edibleStateInfos.Length; i++)
			{
				this.edibleStateInfos[i].gameObject.SetActive(i == this.edibleState);
			}
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x0017E56A File Offset: 0x0017C76A
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

		// Token: 0x06004DF7 RID: 19959 RVA: 0x0017E588 File Offset: 0x0017C788
		protected virtual void LateUpdateLocal()
		{
			if (this.edibleState == this.edibleStateInfos.Length - 1)
			{
				if (!this.isNonRespawnable && Time.time > this.lastFullyEatenTime + this.respawnTime)
				{
					this.edibleState = 0;
					this.previousEdibleState = 0;
					this.OnEdibleHoldableStateChange();
				}
				if (this.isNonRespawnable && Time.time > this.lastFullyEatenTime)
				{
					this.edibleState = 0;
					this.previousEdibleState = 0;
					this.OnEdibleHoldableStateChange();
					GorillaGameManager.instance.FindPlayerVRRig(NetworkSystem.Instance.LocalPlayer).netView.SendRPC("EnableNonCosmeticHandItemRPC", RpcTarget.All, new object[]
					{
						false,
						this.isLeftHand
					});
				}
			}
			else if (Time.time > this.lastEatTime + this.biteCooldown)
			{
				Vector3 b = base.transform.TransformPoint(this.edibleBiteOffset);
				bool flag = false;
				float num = this.biteDistance * this.biteDistance;
				if (!GorillaParent.hasInstance)
				{
					return;
				}
				if ((GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.TransformPoint(this.gorillaHeadMouthOffset) - b).sqrMagnitude < num)
				{
					flag = true;
				}
				foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
				{
					if (!flag)
					{
						if (vrrig.head == null)
						{
							break;
						}
						if (vrrig.head.rigTarget == null)
						{
							break;
						}
						if ((vrrig.head.rigTarget.transform.TransformPoint(this.gorillaHeadMouthOffset) - b).sqrMagnitude < num)
						{
							flag = true;
						}
					}
				}
				if (flag && !this.wasInBiteZoneLastFrame && this.edibleState < this.edibleStateInfos.Length)
				{
					this.edibleState++;
					this.lastEatTime = Time.time;
					this.lastFullyEatenTime = Time.time;
				}
				this.wasInBiteZoneLastFrame = flag;
			}
			this.ownerRig.WearablePackedStates = GTBitOps.WriteBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo, this.edibleState);
		}

		// Token: 0x06004DF8 RID: 19960 RVA: 0x0017E7CC File Offset: 0x0017C9CC
		protected virtual void LateUpdateReplicated()
		{
			this.edibleState = GTBitOps.ReadBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo.index, this.stateBitsWriteInfo.valueMask);
		}

		// Token: 0x06004DF9 RID: 19961 RVA: 0x0017E7FC File Offset: 0x0017C9FC
		protected virtual void LateUpdateShared()
		{
			int num = this.edibleState;
			if (num != this.previousEdibleState)
			{
				this.OnEdibleHoldableStateChange();
			}
			this.previousEdibleState = num;
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x0017E828 File Offset: 0x0017CA28
		protected virtual void OnEdibleHoldableStateChange()
		{
			if (this.previousEdibleState >= 0 && this.previousEdibleState < this.edibleStateInfos.Length)
			{
				this.edibleStateInfos[this.previousEdibleState].gameObject.SetActive(false);
			}
			if (this.edibleState >= 0 && this.edibleState < this.edibleStateInfos.Length)
			{
				this.edibleStateInfos[this.edibleState].gameObject.SetActive(true);
			}
			if (this.edibleState > 0 && this.edibleState < this.edibleStateInfos.Length && this.audioSource != null)
			{
				this.audioSource.GTPlayOneShot(this.edibleStateInfos[this.edibleState].sound, this.volume);
			}
			if (this.edibleState == this.edibleStateInfos.Length && this.audioSource != null)
			{
				this.audioSource.GTPlayOneShot(this.edibleStateInfos[this.edibleState - 1].sound, this.volume);
			}
			float amplitude = GorillaTagger.Instance.tapHapticStrength / 4f;
			float fixedDeltaTime = Time.fixedDeltaTime;
			if (this.isLocal && this.isHandSlot)
			{
				GorillaTagger.Instance.StartVibration(this.isLeftHand, amplitude, fixedDeltaTime);
			}
		}

		// Token: 0x04005100 RID: 20736
		[Tooltip("Check when using non cosmetic edible items like honeycomb")]
		public bool isNonRespawnable;

		// Token: 0x04005101 RID: 20737
		[Tooltip("Eating sounds are played through this AudioSource using PlayOneShot.")]
		public AudioSource audioSource;

		// Token: 0x04005102 RID: 20738
		[Tooltip("Volume each bite should play at.")]
		public float volume = 0.08f;

		// Token: 0x04005103 RID: 20739
		[Tooltip("The slot this cosmetic resides.")]
		public VRRig.WearablePackedStateSlots wearablePackedStateSlot = VRRig.WearablePackedStateSlots.LeftHand;

		// Token: 0x04005104 RID: 20740
		[Tooltip("Time between bites.")]
		public float biteCooldown = 1f;

		// Token: 0x04005105 RID: 20741
		[Tooltip("How long it takes to pop back to the uneaten state after being fully eaten.")]
		public float respawnTime = 7f;

		// Token: 0x04005106 RID: 20742
		[Tooltip("Distance from mouth to item required to trigger a bite.")]
		public float biteDistance = 0.5f;

		// Token: 0x04005107 RID: 20743
		[Tooltip("Offset from Gorilla's head to mouth.")]
		public Vector3 gorillaHeadMouthOffset = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x04005108 RID: 20744
		[Tooltip("Offset from edible's transform to the bite point.")]
		public Vector3 edibleBiteOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04005109 RID: 20745
		public EdibleWearable.EdibleStateInfo[] edibleStateInfos;

		// Token: 0x0400510A RID: 20746
		private VRRig ownerRig;

		// Token: 0x0400510B RID: 20747
		private bool isLocal;

		// Token: 0x0400510C RID: 20748
		private bool isHandSlot;

		// Token: 0x0400510D RID: 20749
		private bool isLeftHand;

		// Token: 0x0400510E RID: 20750
		private GTBitOps.BitWriteInfo stateBitsWriteInfo;

		// Token: 0x0400510F RID: 20751
		private int edibleState;

		// Token: 0x04005110 RID: 20752
		private int previousEdibleState;

		// Token: 0x04005111 RID: 20753
		private float lastEatTime;

		// Token: 0x04005112 RID: 20754
		private float lastFullyEatenTime;

		// Token: 0x04005113 RID: 20755
		private bool wasInBiteZoneLastFrame;

		// Token: 0x02000C32 RID: 3122
		[Serializable]
		public struct EdibleStateInfo
		{
			// Token: 0x04005114 RID: 20756
			[Tooltip("Will be activated when this stage is reached.")]
			public GameObject gameObject;

			// Token: 0x04005115 RID: 20757
			[Tooltip("Will be played when this stage is reached.")]
			public AudioClip sound;
		}
	}
}
