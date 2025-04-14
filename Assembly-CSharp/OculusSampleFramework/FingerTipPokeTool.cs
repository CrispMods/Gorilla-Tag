using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A57 RID: 2647
	public class FingerTipPokeTool : InteractableTool
	{
		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060041E5 RID: 16869 RVA: 0x000EE9C3 File Offset: 0x000ECBC3
		public override InteractableToolTags ToolTags
		{
			get
			{
				return InteractableToolTags.Poke;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060041E6 RID: 16870 RVA: 0x00002076 File Offset: 0x00000276
		public override ToolInputState ToolInputState
		{
			get
			{
				return ToolInputState.Inactive;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060041E7 RID: 16871 RVA: 0x00002076 File Offset: 0x00000276
		public override bool IsFarFieldTool
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060041E8 RID: 16872 RVA: 0x00137BC8 File Offset: 0x00135DC8
		// (set) Token: 0x060041E9 RID: 16873 RVA: 0x00137BDA File Offset: 0x00135DDA
		public override bool EnableState
		{
			get
			{
				return this._fingerTipPokeToolView.gameObject.activeSelf;
			}
			set
			{
				this._fingerTipPokeToolView.gameObject.SetActive(value);
			}
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x00137BF0 File Offset: 0x00135DF0
		public override void Initialize()
		{
			InteractableToolsInputRouter.Instance.RegisterInteractableTool(this);
			this._fingerTipPokeToolView.InteractableTool = this;
			this._velocityFrames = new Vector3[10];
			Array.Clear(this._velocityFrames, 0, 10);
			base.StartCoroutine(this.AttachTriggerLogic());
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x00137C3C File Offset: 0x00135E3C
		private IEnumerator AttachTriggerLogic()
		{
			while (!HandsManager.Instance || !HandsManager.Instance.IsInitialized())
			{
				yield return null;
			}
			OVRSkeleton skeleton = base.IsRightHandedTool ? HandsManager.Instance.RightHandSkeleton : HandsManager.Instance.LeftHandSkeleton;
			OVRSkeleton.BoneId boneId;
			switch (this._fingerToFollow)
			{
			case OVRPlugin.HandFinger.Thumb:
				boneId = OVRSkeleton.BoneId.Hand_Thumb3;
				break;
			case OVRPlugin.HandFinger.Index:
				boneId = OVRSkeleton.BoneId.Hand_Index3;
				break;
			case OVRPlugin.HandFinger.Middle:
				boneId = OVRSkeleton.BoneId.Hand_Middle3;
				break;
			case OVRPlugin.HandFinger.Ring:
				boneId = OVRSkeleton.BoneId.Hand_Ring3;
				break;
			default:
				boneId = OVRSkeleton.BoneId.Hand_Pinky3;
				break;
			}
			List<BoneCapsuleTriggerLogic> list = new List<BoneCapsuleTriggerLogic>();
			List<OVRBoneCapsule> capsulesPerBone = HandsManager.GetCapsulesPerBone(skeleton, boneId);
			foreach (OVRBoneCapsule ovrboneCapsule in capsulesPerBone)
			{
				BoneCapsuleTriggerLogic boneCapsuleTriggerLogic = ovrboneCapsule.CapsuleRigidbody.gameObject.AddComponent<BoneCapsuleTriggerLogic>();
				ovrboneCapsule.CapsuleCollider.isTrigger = true;
				boneCapsuleTriggerLogic.ToolTags = this.ToolTags;
				list.Add(boneCapsuleTriggerLogic);
			}
			this._boneCapsuleTriggerLogic = list.ToArray();
			if (capsulesPerBone.Count > 0)
			{
				this._capsuleToTrack = capsulesPerBone[0];
			}
			this._isInitialized = true;
			yield break;
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x00137C4C File Offset: 0x00135E4C
		private void Update()
		{
			if (!HandsManager.Instance || !HandsManager.Instance.IsInitialized() || !this._isInitialized || this._capsuleToTrack == null)
			{
				return;
			}
			float handScale = (base.IsRightHandedTool ? HandsManager.Instance.RightHand : HandsManager.Instance.LeftHand).HandScale;
			Transform transform = this._capsuleToTrack.CapsuleCollider.transform;
			Vector3 right = transform.right;
			Vector3 vector = transform.position + this._capsuleToTrack.CapsuleCollider.height * 0.5f * right;
			Vector3 b = handScale * this._fingerTipPokeToolView.SphereRadius * right;
			Vector3 position = vector + b;
			base.transform.position = position;
			base.transform.rotation = transform.rotation;
			base.InteractionPosition = vector;
			this.UpdateAverageVelocity();
			this.CheckAndUpdateScale();
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x00137D34 File Offset: 0x00135F34
		private void UpdateAverageVelocity()
		{
			Vector3 position = this._position;
			Vector3 position2 = base.transform.position;
			Vector3 vector = (position2 - position) / Time.deltaTime;
			this._position = position2;
			this._velocityFrames[this._currVelocityFrame] = vector;
			this._currVelocityFrame = (this._currVelocityFrame + 1) % 10;
			base.Velocity = Vector3.zero;
			if (!this._sampledMaxFramesAlready && this._currVelocityFrame == 9)
			{
				this._sampledMaxFramesAlready = true;
			}
			int num = this._sampledMaxFramesAlready ? 10 : (this._currVelocityFrame + 1);
			for (int i = 0; i < num; i++)
			{
				base.Velocity += this._velocityFrames[i];
			}
			base.Velocity /= (float)num;
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x00137E0C File Offset: 0x0013600C
		private void CheckAndUpdateScale()
		{
			float num = base.IsRightHandedTool ? HandsManager.Instance.RightHand.HandScale : HandsManager.Instance.LeftHand.HandScale;
			if (Mathf.Abs(num - this._lastScale) > Mathf.Epsilon)
			{
				base.transform.localScale = new Vector3(num, num, num);
				this._lastScale = num;
			}
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x00137E70 File Offset: 0x00136070
		public override List<InteractableCollisionInfo> GetNextIntersectingObjects()
		{
			this._currentIntersectingObjects.Clear();
			BoneCapsuleTriggerLogic[] boneCapsuleTriggerLogic = this._boneCapsuleTriggerLogic;
			for (int i = 0; i < boneCapsuleTriggerLogic.Length; i++)
			{
				foreach (ColliderZone colliderZone in boneCapsuleTriggerLogic[i].CollidersTouchingUs)
				{
					this._currentIntersectingObjects.Add(new InteractableCollisionInfo(colliderZone, colliderZone.CollisionDepth, this));
				}
			}
			return this._currentIntersectingObjects;
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x000023F4 File Offset: 0x000005F4
		public override void FocusOnInteractable(Interactable focusedInteractable, ColliderZone colliderZone)
		{
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x000023F4 File Offset: 0x000005F4
		public override void DeFocus()
		{
		}

		// Token: 0x040042F8 RID: 17144
		private const int NUM_VELOCITY_FRAMES = 10;

		// Token: 0x040042F9 RID: 17145
		[SerializeField]
		private FingerTipPokeToolView _fingerTipPokeToolView;

		// Token: 0x040042FA RID: 17146
		[SerializeField]
		private OVRPlugin.HandFinger _fingerToFollow = OVRPlugin.HandFinger.Index;

		// Token: 0x040042FB RID: 17147
		private Vector3[] _velocityFrames;

		// Token: 0x040042FC RID: 17148
		private int _currVelocityFrame;

		// Token: 0x040042FD RID: 17149
		private bool _sampledMaxFramesAlready;

		// Token: 0x040042FE RID: 17150
		private Vector3 _position;

		// Token: 0x040042FF RID: 17151
		private BoneCapsuleTriggerLogic[] _boneCapsuleTriggerLogic;

		// Token: 0x04004300 RID: 17152
		private float _lastScale = 1f;

		// Token: 0x04004301 RID: 17153
		private bool _isInitialized;

		// Token: 0x04004302 RID: 17154
		private OVRBoneCapsule _capsuleToTrack;
	}
}
