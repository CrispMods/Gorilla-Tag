using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A5A RID: 2650
	public class FingerTipPokeTool : InteractableTool
	{
		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060041F1 RID: 16881 RVA: 0x000EEE43 File Offset: 0x000ED043
		public override InteractableToolTags ToolTags
		{
			get
			{
				return InteractableToolTags.Poke;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060041F2 RID: 16882 RVA: 0x00002076 File Offset: 0x00000276
		public override ToolInputState ToolInputState
		{
			get
			{
				return ToolInputState.Inactive;
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060041F3 RID: 16883 RVA: 0x00002076 File Offset: 0x00000276
		public override bool IsFarFieldTool
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060041F4 RID: 16884 RVA: 0x00138190 File Offset: 0x00136390
		// (set) Token: 0x060041F5 RID: 16885 RVA: 0x001381A2 File Offset: 0x001363A2
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

		// Token: 0x060041F6 RID: 16886 RVA: 0x001381B8 File Offset: 0x001363B8
		public override void Initialize()
		{
			InteractableToolsInputRouter.Instance.RegisterInteractableTool(this);
			this._fingerTipPokeToolView.InteractableTool = this;
			this._velocityFrames = new Vector3[10];
			Array.Clear(this._velocityFrames, 0, 10);
			base.StartCoroutine(this.AttachTriggerLogic());
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x00138204 File Offset: 0x00136404
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

		// Token: 0x060041F8 RID: 16888 RVA: 0x00138214 File Offset: 0x00136414
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

		// Token: 0x060041F9 RID: 16889 RVA: 0x001382FC File Offset: 0x001364FC
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

		// Token: 0x060041FA RID: 16890 RVA: 0x001383D4 File Offset: 0x001365D4
		private void CheckAndUpdateScale()
		{
			float num = base.IsRightHandedTool ? HandsManager.Instance.RightHand.HandScale : HandsManager.Instance.LeftHand.HandScale;
			if (Mathf.Abs(num - this._lastScale) > Mathf.Epsilon)
			{
				base.transform.localScale = new Vector3(num, num, num);
				this._lastScale = num;
			}
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x00138438 File Offset: 0x00136638
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

		// Token: 0x060041FC RID: 16892 RVA: 0x000023F4 File Offset: 0x000005F4
		public override void FocusOnInteractable(Interactable focusedInteractable, ColliderZone colliderZone)
		{
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x000023F4 File Offset: 0x000005F4
		public override void DeFocus()
		{
		}

		// Token: 0x0400430A RID: 17162
		private const int NUM_VELOCITY_FRAMES = 10;

		// Token: 0x0400430B RID: 17163
		[SerializeField]
		private FingerTipPokeToolView _fingerTipPokeToolView;

		// Token: 0x0400430C RID: 17164
		[SerializeField]
		private OVRPlugin.HandFinger _fingerToFollow = OVRPlugin.HandFinger.Index;

		// Token: 0x0400430D RID: 17165
		private Vector3[] _velocityFrames;

		// Token: 0x0400430E RID: 17166
		private int _currVelocityFrame;

		// Token: 0x0400430F RID: 17167
		private bool _sampledMaxFramesAlready;

		// Token: 0x04004310 RID: 17168
		private Vector3 _position;

		// Token: 0x04004311 RID: 17169
		private BoneCapsuleTriggerLogic[] _boneCapsuleTriggerLogic;

		// Token: 0x04004312 RID: 17170
		private float _lastScale = 1f;

		// Token: 0x04004313 RID: 17171
		private bool _isInitialized;

		// Token: 0x04004314 RID: 17172
		private OVRBoneCapsule _capsuleToTrack;
	}
}
