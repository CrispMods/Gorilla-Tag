using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A84 RID: 2692
	public class FingerTipPokeTool : InteractableTool
	{
		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x0600432A RID: 17194 RVA: 0x0005144C File Offset: 0x0004F64C
		public override InteractableToolTags ToolTags
		{
			get
			{
				return InteractableToolTags.Poke;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x0600432B RID: 17195 RVA: 0x00030498 File Offset: 0x0002E698
		public override ToolInputState ToolInputState
		{
			get
			{
				return ToolInputState.Inactive;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x0600432C RID: 17196 RVA: 0x00030498 File Offset: 0x0002E698
		public override bool IsFarFieldTool
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x0600432D RID: 17197 RVA: 0x0005BD34 File Offset: 0x00059F34
		// (set) Token: 0x0600432E RID: 17198 RVA: 0x0005BD46 File Offset: 0x00059F46
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

		// Token: 0x0600432F RID: 17199 RVA: 0x001776E4 File Offset: 0x001758E4
		public override void Initialize()
		{
			InteractableToolsInputRouter.Instance.RegisterInteractableTool(this);
			this._fingerTipPokeToolView.InteractableTool = this;
			this._velocityFrames = new Vector3[10];
			Array.Clear(this._velocityFrames, 0, 10);
			base.StartCoroutine(this.AttachTriggerLogic());
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x0005BD59 File Offset: 0x00059F59
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

		// Token: 0x06004331 RID: 17201 RVA: 0x00177730 File Offset: 0x00175930
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

		// Token: 0x06004332 RID: 17202 RVA: 0x00177818 File Offset: 0x00175A18
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

		// Token: 0x06004333 RID: 17203 RVA: 0x001778F0 File Offset: 0x00175AF0
		private void CheckAndUpdateScale()
		{
			float num = base.IsRightHandedTool ? HandsManager.Instance.RightHand.HandScale : HandsManager.Instance.LeftHand.HandScale;
			if (Mathf.Abs(num - this._lastScale) > Mathf.Epsilon)
			{
				base.transform.localScale = new Vector3(num, num, num);
				this._lastScale = num;
			}
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x00177954 File Offset: 0x00175B54
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

		// Token: 0x06004335 RID: 17205 RVA: 0x00030607 File Offset: 0x0002E807
		public override void FocusOnInteractable(Interactable focusedInteractable, ColliderZone colliderZone)
		{
		}

		// Token: 0x06004336 RID: 17206 RVA: 0x00030607 File Offset: 0x0002E807
		public override void DeFocus()
		{
		}

		// Token: 0x040043F2 RID: 17394
		private const int NUM_VELOCITY_FRAMES = 10;

		// Token: 0x040043F3 RID: 17395
		[SerializeField]
		private FingerTipPokeToolView _fingerTipPokeToolView;

		// Token: 0x040043F4 RID: 17396
		[SerializeField]
		private OVRPlugin.HandFinger _fingerToFollow = OVRPlugin.HandFinger.Index;

		// Token: 0x040043F5 RID: 17397
		private Vector3[] _velocityFrames;

		// Token: 0x040043F6 RID: 17398
		private int _currVelocityFrame;

		// Token: 0x040043F7 RID: 17399
		private bool _sampledMaxFramesAlready;

		// Token: 0x040043F8 RID: 17400
		private Vector3 _position;

		// Token: 0x040043F9 RID: 17401
		private BoneCapsuleTriggerLogic[] _boneCapsuleTriggerLogic;

		// Token: 0x040043FA RID: 17402
		private float _lastScale = 1f;

		// Token: 0x040043FB RID: 17403
		private bool _isInitialized;

		// Token: 0x040043FC RID: 17404
		private OVRBoneCapsule _capsuleToTrack;
	}
}
