using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OVRTouchSample
{
	// Token: 0x02000A7E RID: 2686
	[RequireComponent(typeof(OVRGrabber))]
	public class Hand : MonoBehaviour
	{
		// Token: 0x060042FA RID: 17146 RVA: 0x0013B9B6 File Offset: 0x00139BB6
		private void Awake()
		{
			this.m_grabber = base.GetComponent<OVRGrabber>();
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x0013B9C4 File Offset: 0x00139BC4
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

		// Token: 0x060042FC RID: 17148 RVA: 0x0013BA86 File Offset: 0x00139C86
		private void OnDestroy()
		{
			OVRManager.InputFocusAcquired -= this.OnInputFocusAcquired;
			OVRManager.InputFocusLost -= this.OnInputFocusLost;
		}

		// Token: 0x060042FD RID: 17149 RVA: 0x0013BAAC File Offset: 0x00139CAC
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

		// Token: 0x060042FE RID: 17150 RVA: 0x0013BB2B File Offset: 0x00139D2B
		private void UpdateCapTouchStates()
		{
			this.m_isPointing = !OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger, this.m_controller);
			this.m_isGivingThumbsUp = !OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons, this.m_controller);
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x0013BB58 File Offset: 0x00139D58
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

		// Token: 0x06004300 RID: 17152 RVA: 0x0013BBE0 File Offset: 0x00139DE0
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

		// Token: 0x06004301 RID: 17153 RVA: 0x0013BC4C File Offset: 0x00139E4C
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

		// Token: 0x06004302 RID: 17154 RVA: 0x0013BCB0 File Offset: 0x00139EB0
		private float InputValueRateChange(bool isDown, float value)
		{
			float num = Time.deltaTime * 20f;
			float num2 = isDown ? 1f : -1f;
			return Mathf.Clamp01(value + num * num2);
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x0013BCE4 File Offset: 0x00139EE4
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

		// Token: 0x06004304 RID: 17156 RVA: 0x0013BDE0 File Offset: 0x00139FE0
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

		// Token: 0x0400441D RID: 17437
		public const string ANIM_LAYER_NAME_POINT = "Point Layer";

		// Token: 0x0400441E RID: 17438
		public const string ANIM_LAYER_NAME_THUMB = "Thumb Layer";

		// Token: 0x0400441F RID: 17439
		public const string ANIM_PARAM_NAME_FLEX = "Flex";

		// Token: 0x04004420 RID: 17440
		public const string ANIM_PARAM_NAME_POSE = "Pose";

		// Token: 0x04004421 RID: 17441
		public const float THRESH_COLLISION_FLEX = 0.9f;

		// Token: 0x04004422 RID: 17442
		public const float INPUT_RATE_CHANGE = 20f;

		// Token: 0x04004423 RID: 17443
		public const float COLLIDER_SCALE_MIN = 0.01f;

		// Token: 0x04004424 RID: 17444
		public const float COLLIDER_SCALE_MAX = 1f;

		// Token: 0x04004425 RID: 17445
		public const float COLLIDER_SCALE_PER_SECOND = 1f;

		// Token: 0x04004426 RID: 17446
		public const float TRIGGER_DEBOUNCE_TIME = 0.05f;

		// Token: 0x04004427 RID: 17447
		public const float THUMB_DEBOUNCE_TIME = 0.15f;

		// Token: 0x04004428 RID: 17448
		[SerializeField]
		private OVRInput.Controller m_controller;

		// Token: 0x04004429 RID: 17449
		[SerializeField]
		private Animator m_animator;

		// Token: 0x0400442A RID: 17450
		[SerializeField]
		private HandPose m_defaultGrabPose;

		// Token: 0x0400442B RID: 17451
		private Collider[] m_colliders;

		// Token: 0x0400442C RID: 17452
		private bool m_collisionEnabled = true;

		// Token: 0x0400442D RID: 17453
		private OVRGrabber m_grabber;

		// Token: 0x0400442E RID: 17454
		private List<Renderer> m_showAfterInputFocusAcquired;

		// Token: 0x0400442F RID: 17455
		private int m_animLayerIndexThumb = -1;

		// Token: 0x04004430 RID: 17456
		private int m_animLayerIndexPoint = -1;

		// Token: 0x04004431 RID: 17457
		private int m_animParamIndexFlex = -1;

		// Token: 0x04004432 RID: 17458
		private int m_animParamIndexPose = -1;

		// Token: 0x04004433 RID: 17459
		private bool m_isPointing;

		// Token: 0x04004434 RID: 17460
		private bool m_isGivingThumbsUp;

		// Token: 0x04004435 RID: 17461
		private float m_pointBlend;

		// Token: 0x04004436 RID: 17462
		private float m_thumbsUpBlend;

		// Token: 0x04004437 RID: 17463
		private bool m_restoreOnInputAcquired;

		// Token: 0x04004438 RID: 17464
		private float m_collisionScaleCurrent;
	}
}
