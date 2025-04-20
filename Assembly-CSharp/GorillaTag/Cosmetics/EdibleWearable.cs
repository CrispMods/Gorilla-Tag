using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C62 RID: 3170
	public class EdibleWearable : MonoBehaviour
	{
		// Token: 0x06004F54 RID: 20308 RVA: 0x001B6EDC File Offset: 0x001B50DC
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

		// Token: 0x06004F55 RID: 20309 RVA: 0x001B6F68 File Offset: 0x001B5168
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

		// Token: 0x06004F56 RID: 20310 RVA: 0x00063CB9 File Offset: 0x00061EB9
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

		// Token: 0x06004F57 RID: 20311 RVA: 0x001B6FE4 File Offset: 0x001B51E4
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

		// Token: 0x06004F58 RID: 20312 RVA: 0x00063CD7 File Offset: 0x00061ED7
		protected virtual void LateUpdateReplicated()
		{
			this.edibleState = GTBitOps.ReadBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo.index, this.stateBitsWriteInfo.valueMask);
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x001B7228 File Offset: 0x001B5428
		protected virtual void LateUpdateShared()
		{
			int num = this.edibleState;
			if (num != this.previousEdibleState)
			{
				this.OnEdibleHoldableStateChange();
			}
			this.previousEdibleState = num;
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x001B7254 File Offset: 0x001B5454
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

		// Token: 0x0400520C RID: 21004
		[Tooltip("Check when using non cosmetic edible items like honeycomb")]
		public bool isNonRespawnable;

		// Token: 0x0400520D RID: 21005
		[Tooltip("Eating sounds are played through this AudioSource using PlayOneShot.")]
		public AudioSource audioSource;

		// Token: 0x0400520E RID: 21006
		[Tooltip("Volume each bite should play at.")]
		public float volume = 0.08f;

		// Token: 0x0400520F RID: 21007
		[Tooltip("The slot this cosmetic resides.")]
		public VRRig.WearablePackedStateSlots wearablePackedStateSlot = VRRig.WearablePackedStateSlots.LeftHand;

		// Token: 0x04005210 RID: 21008
		[Tooltip("Time between bites.")]
		public float biteCooldown = 1f;

		// Token: 0x04005211 RID: 21009
		[Tooltip("How long it takes to pop back to the uneaten state after being fully eaten.")]
		public float respawnTime = 7f;

		// Token: 0x04005212 RID: 21010
		[Tooltip("Distance from mouth to item required to trigger a bite.")]
		public float biteDistance = 0.5f;

		// Token: 0x04005213 RID: 21011
		[Tooltip("Offset from Gorilla's head to mouth.")]
		public Vector3 gorillaHeadMouthOffset = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x04005214 RID: 21012
		[Tooltip("Offset from edible's transform to the bite point.")]
		public Vector3 edibleBiteOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04005215 RID: 21013
		public EdibleWearable.EdibleStateInfo[] edibleStateInfos;

		// Token: 0x04005216 RID: 21014
		private VRRig ownerRig;

		// Token: 0x04005217 RID: 21015
		private bool isLocal;

		// Token: 0x04005218 RID: 21016
		private bool isHandSlot;

		// Token: 0x04005219 RID: 21017
		private bool isLeftHand;

		// Token: 0x0400521A RID: 21018
		private GTBitOps.BitWriteInfo stateBitsWriteInfo;

		// Token: 0x0400521B RID: 21019
		private int edibleState;

		// Token: 0x0400521C RID: 21020
		private int previousEdibleState;

		// Token: 0x0400521D RID: 21021
		private float lastEatTime;

		// Token: 0x0400521E RID: 21022
		private float lastFullyEatenTime;

		// Token: 0x0400521F RID: 21023
		private bool wasInBiteZoneLastFrame;

		// Token: 0x02000C63 RID: 3171
		[Serializable]
		public struct EdibleStateInfo
		{
			// Token: 0x04005220 RID: 21024
			[Tooltip("Will be activated when this stage is reached.")]
			public GameObject gameObject;

			// Token: 0x04005221 RID: 21025
			[Tooltip("Will be played when this stage is reached.")]
			public AudioClip sound;
		}
	}
}
