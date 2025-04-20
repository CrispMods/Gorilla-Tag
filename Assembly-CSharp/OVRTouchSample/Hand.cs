using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OVRTouchSample
{
	// Token: 0x02000AAB RID: 2731
	[RequireComponent(typeof(OVRGrabber))]
	public class Hand : MonoBehaviour
	{
		// Token: 0x0600443F RID: 17471 RVA: 0x0005C86F File Offset: 0x0005AA6F
		private void Awake()
		{
			this.m_grabber = base.GetComponent<OVRGrabber>();
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x0017A984 File Offset: 0x00178B84
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

		// Token: 0x06004441 RID: 17473 RVA: 0x0005C87D File Offset: 0x0005AA7D
		private void OnDestroy()
		{
			OVRManager.InputFocusAcquired -= this.OnInputFocusAcquired;
			OVRManager.InputFocusLost -= this.OnInputFocusLost;
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x0017AA48 File Offset: 0x00178C48
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

		// Token: 0x06004443 RID: 17475 RVA: 0x0005C8A1 File Offset: 0x0005AAA1
		private void UpdateCapTouchStates()
		{
			this.m_isPointing = !OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger, this.m_controller);
			this.m_isGivingThumbsUp = !OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons, this.m_controller);
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x0017AAC8 File Offset: 0x00178CC8
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

		// Token: 0x06004445 RID: 17477 RVA: 0x0017AB50 File Offset: 0x00178D50
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

		// Token: 0x06004446 RID: 17478 RVA: 0x0017ABBC File Offset: 0x00178DBC
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

		// Token: 0x06004447 RID: 17479 RVA: 0x0017AC20 File Offset: 0x00178E20
		private float InputValueRateChange(bool isDown, float value)
		{
			float num = Time.deltaTime * 20f;
			float num2 = isDown ? 1f : -1f;
			return Mathf.Clamp01(value + num * num2);
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x0017AC54 File Offset: 0x00178E54
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

		// Token: 0x06004449 RID: 17481 RVA: 0x0017AD50 File Offset: 0x00178F50
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

		// Token: 0x04004517 RID: 17687
		public const string ANIM_LAYER_NAME_POINT = "Point Layer";

		// Token: 0x04004518 RID: 17688
		public const string ANIM_LAYER_NAME_THUMB = "Thumb Layer";

		// Token: 0x04004519 RID: 17689
		public const string ANIM_PARAM_NAME_FLEX = "Flex";

		// Token: 0x0400451A RID: 17690
		public const string ANIM_PARAM_NAME_POSE = "Pose";

		// Token: 0x0400451B RID: 17691
		public const float THRESH_COLLISION_FLEX = 0.9f;

		// Token: 0x0400451C RID: 17692
		public const float INPUT_RATE_CHANGE = 20f;

		// Token: 0x0400451D RID: 17693
		public const float COLLIDER_SCALE_MIN = 0.01f;

		// Token: 0x0400451E RID: 17694
		public const float COLLIDER_SCALE_MAX = 1f;

		// Token: 0x0400451F RID: 17695
		public const float COLLIDER_SCALE_PER_SECOND = 1f;

		// Token: 0x04004520 RID: 17696
		public const float TRIGGER_DEBOUNCE_TIME = 0.05f;

		// Token: 0x04004521 RID: 17697
		public const float THUMB_DEBOUNCE_TIME = 0.15f;

		// Token: 0x04004522 RID: 17698
		[SerializeField]
		private OVRInput.Controller m_controller;

		// Token: 0x04004523 RID: 17699
		[SerializeField]
		private Animator m_animator;

		// Token: 0x04004524 RID: 17700
		[SerializeField]
		private HandPose m_defaultGrabPose;

		// Token: 0x04004525 RID: 17701
		private Collider[] m_colliders;

		// Token: 0x04004526 RID: 17702
		private bool m_collisionEnabled = true;

		// Token: 0x04004527 RID: 17703
		private OVRGrabber m_grabber;

		// Token: 0x04004528 RID: 17704
		private List<Renderer> m_showAfterInputFocusAcquired;

		// Token: 0x04004529 RID: 17705
		private int m_animLayerIndexThumb = -1;

		// Token: 0x0400452A RID: 17706
		private int m_animLayerIndexPoint = -1;

		// Token: 0x0400452B RID: 17707
		private int m_animParamIndexFlex = -1;

		// Token: 0x0400452C RID: 17708
		private int m_animParamIndexPose = -1;

		// Token: 0x0400452D RID: 17709
		private bool m_isPointing;

		// Token: 0x0400452E RID: 17710
		private bool m_isGivingThumbsUp;

		// Token: 0x0400452F RID: 17711
		private float m_pointBlend;

		// Token: 0x04004530 RID: 17712
		private float m_thumbsUpBlend;

		// Token: 0x04004531 RID: 17713
		private bool m_restoreOnInputAcquired;

		// Token: 0x04004532 RID: 17714
		private float m_collisionScaleCurrent;
	}
}
