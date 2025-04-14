using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OVRTouchSample
{
	// Token: 0x02000A81 RID: 2689
	[RequireComponent(typeof(OVRGrabber))]
	public class Hand : MonoBehaviour
	{
		// Token: 0x06004306 RID: 17158 RVA: 0x0013BF7E File Offset: 0x0013A17E
		private void Awake()
		{
			this.m_grabber = base.GetComponent<OVRGrabber>();
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x0013BF8C File Offset: 0x0013A18C
		private void Start()
		{
			this.m_showAfterInputFocusAcquired = new List<Renderer>();
			this.m_colliders = (from childCollider in base.GetComponentsInChildren<Collider>()
			where !childCollider.isTrigger
			select childCollider).ToArray<Collider>();
			this.CollisionEnable(false);
			this.m_animLayerIndexPoint = this.m_animator.GetLayerIndex("Point Layer");
			this.m_animLayerIndexThumb = this.m_animator.GetLayerIndex("Thumb Layer");
			this.m_animParamIndexFlex = Animator.StringToHash("Flex");
			this.m_animParamIndexPose = Animator.StringToHash("Pose");
			OVRManager.InputFocusAcquired += this.OnInputFocusAcquired;
			OVRManager.InputFocusLost += this.OnInputFocusLost;
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x0013C04E File Offset: 0x0013A24E
		private void OnDestroy()
		{
			OVRManager.InputFocusAcquired -= this.OnInputFocusAcquired;
			OVRManager.InputFocusLost -= this.OnInputFocusLost;
		}

		// Token: 0x06004309 RID: 17161 RVA: 0x0013C074 File Offset: 0x0013A274
		private void Update()
		{
			this.UpdateCapTouchStates();
			this.m_pointBlend = this.InputValueRateChange(this.m_isPointing, this.m_pointBlend);
			this.m_thumbsUpBlend = this.InputValueRateChange(this.m_isGivingThumbsUp, this.m_thumbsUpBlend);
			float num = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, this.m_controller);
			bool enabled = this.m_grabber.grabbedObject == null && num >= 0.9f;
			this.CollisionEnable(enabled);
			this.UpdateAnimStates();
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x0013C0F3 File Offset: 0x0013A2F3
		private void UpdateCapTouchStates()
		{
			this.m_isPointing = !OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger, this.m_controller);
			this.m_isGivingThumbsUp = !OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons, this.m_controller);
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x0013C120 File Offset: 0x0013A320
		private void LateUpdate()
		{
			if (this.m_collisionEnabled && this.m_collisionScaleCurrent + Mathf.Epsilon < 1f)
			{
				this.m_collisionScaleCurrent = Mathf.Min(1f, this.m_collisionScaleCurrent + Time.deltaTime * 1f);
				for (int i = 0; i < this.m_colliders.Length; i++)
				{
					this.m_colliders[i].transform.localScale = new Vector3(this.m_collisionScaleCurrent, this.m_collisionScaleCurrent, this.m_collisionScaleCurrent);
				}
			}
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x0013C1A8 File Offset: 0x0013A3A8
		private void OnInputFocusLost()
		{
			if (base.gameObject.activeInHierarchy)
			{
				this.m_showAfterInputFocusAcquired.Clear();
				Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i].enabled)
					{
						componentsInChildren[i].enabled = false;
						this.m_showAfterInputFocusAcquired.Add(componentsInChildren[i]);
					}
				}
				this.CollisionEnable(false);
				this.m_restoreOnInputAcquired = true;
			}
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x0013C214 File Offset: 0x0013A414
		private void OnInputFocusAcquired()
		{
			if (this.m_restoreOnInputAcquired)
			{
				for (int i = 0; i < this.m_showAfterInputFocusAcquired.Count; i++)
				{
					if (this.m_showAfterInputFocusAcquired[i])
					{
						this.m_showAfterInputFocusAcquired[i].enabled = true;
					}
				}
				this.m_showAfterInputFocusAcquired.Clear();
				this.m_restoreOnInputAcquired = false;
			}
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x0013C278 File Offset: 0x0013A478
		private float InputValueRateChange(bool isDown, float value)
		{
			float num = Time.deltaTime * 20f;
			float num2 = isDown ? 1f : -1f;
			return Mathf.Clamp01(value + num * num2);
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x0013C2AC File Offset: 0x0013A4AC
		private void UpdateAnimStates()
		{
			bool flag = this.m_grabber.grabbedObject != null;
			HandPose handPose = this.m_defaultGrabPose;
			if (flag)
			{
				HandPose component = this.m_grabber.grabbedObject.GetComponent<HandPose>();
				if (component != null)
				{
					handPose = component;
				}
			}
			HandPoseId poseId = handPose.PoseId;
			this.m_animator.SetInteger(this.m_animParamIndexPose, (int)poseId);
			float value = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, this.m_controller);
			this.m_animator.SetFloat(this.m_animParamIndexFlex, value);
			float weight = (!flag || handPose.AllowPointing) ? this.m_pointBlend : 0f;
			this.m_animator.SetLayerWeight(this.m_animLayerIndexPoint, weight);
			float weight2 = (!flag || handPose.AllowThumbsUp) ? this.m_thumbsUpBlend : 0f;
			this.m_animator.SetLayerWeight(this.m_animLayerIndexThumb, weight2);
			float value2 = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, this.m_controller);
			this.m_animator.SetFloat("Pinch", value2);
		}

		// Token: 0x06004310 RID: 17168 RVA: 0x0013C3A8 File Offset: 0x0013A5A8
		private void CollisionEnable(bool enabled)
		{
			if (this.m_collisionEnabled == enabled)
			{
				return;
			}
			this.m_collisionEnabled = enabled;
			if (enabled)
			{
				this.m_collisionScaleCurrent = 0.01f;
				for (int i = 0; i < this.m_colliders.Length; i++)
				{
					Collider collider = this.m_colliders[i];
					collider.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
					collider.enabled = true;
				}
				return;
			}
			this.m_collisionScaleCurrent = 1f;
			for (int j = 0; j < this.m_colliders.Length; j++)
			{
				Collider collider2 = this.m_colliders[j];
				collider2.enabled = false;
				collider2.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			}
		}

		// Token: 0x0400442F RID: 17455
		public const string ANIM_LAYER_NAME_POINT = "Point Layer";

		// Token: 0x04004430 RID: 17456
		public const string ANIM_LAYER_NAME_THUMB = "Thumb Layer";

		// Token: 0x04004431 RID: 17457
		public const string ANIM_PARAM_NAME_FLEX = "Flex";

		// Token: 0x04004432 RID: 17458
		public const string ANIM_PARAM_NAME_POSE = "Pose";

		// Token: 0x04004433 RID: 17459
		public const float THRESH_COLLISION_FLEX = 0.9f;

		// Token: 0x04004434 RID: 17460
		public const float INPUT_RATE_CHANGE = 20f;

		// Token: 0x04004435 RID: 17461
		public const float COLLIDER_SCALE_MIN = 0.01f;

		// Token: 0x04004436 RID: 17462
		public const float COLLIDER_SCALE_MAX = 1f;

		// Token: 0x04004437 RID: 17463
		public const float COLLIDER_SCALE_PER_SECOND = 1f;

		// Token: 0x04004438 RID: 17464
		public const float TRIGGER_DEBOUNCE_TIME = 0.05f;

		// Token: 0x04004439 RID: 17465
		public const float THUMB_DEBOUNCE_TIME = 0.15f;

		// Token: 0x0400443A RID: 17466
		[SerializeField]
		private OVRInput.Controller m_controller;

		// Token: 0x0400443B RID: 17467
		[SerializeField]
		private Animator m_animator;

		// Token: 0x0400443C RID: 17468
		[SerializeField]
		private HandPose m_defaultGrabPose;

		// Token: 0x0400443D RID: 17469
		private Collider[] m_colliders;

		// Token: 0x0400443E RID: 17470
		private bool m_collisionEnabled = true;

		// Token: 0x0400443F RID: 17471
		private OVRGrabber m_grabber;

		// Token: 0x04004440 RID: 17472
		private List<Renderer> m_showAfterInputFocusAcquired;

		// Token: 0x04004441 RID: 17473
		private int m_animLayerIndexThumb = -1;

		// Token: 0x04004442 RID: 17474
		private int m_animLayerIndexPoint = -1;

		// Token: 0x04004443 RID: 17475
		private int m_animParamIndexFlex = -1;

		// Token: 0x04004444 RID: 17476
		private int m_animParamIndexPose = -1;

		// Token: 0x04004445 RID: 17477
		private bool m_isPointing;

		// Token: 0x04004446 RID: 17478
		private bool m_isGivingThumbsUp;

		// Token: 0x04004447 RID: 17479
		private float m_pointBlend;

		// Token: 0x04004448 RID: 17480
		private float m_thumbsUpBlend;

		// Token: 0x04004449 RID: 17481
		private bool m_restoreOnInputAcquired;

		// Token: 0x0400444A RID: 17482
		private float m_collisionScaleCurrent;
	}
}
