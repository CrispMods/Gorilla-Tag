using System;
using UnityEngine;

namespace OVRTouchSample
{
	// Token: 0x02000A85 RID: 2693
	public class TouchController : MonoBehaviour
	{
		// Token: 0x06004319 RID: 17177 RVA: 0x00173F84 File Offset: 0x00172184
		private void Update()
		{
			this.m_animator.SetFloat("Button 1", OVRInput.Get(OVRInput.Button.One, this.m_controller) ? 1f : 0f);
			this.m_animator.SetFloat("Button 2", OVRInput.Get(OVRInput.Button.Two, this.m_controller) ? 1f : 0f);
			this.m_animator.SetFloat("Joy X", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, this.m_controller).x);
			this.m_animator.SetFloat("Joy Y", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, this.m_controller).y);
			this.m_animator.SetFloat("Grip", OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, this.m_controller));
			this.m_animator.SetFloat("Trigger", OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, this.m_controller));
			OVRManager.InputFocusAcquired += this.OnInputFocusAcquired;
			OVRManager.InputFocusLost += this.OnInputFocusLost;
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x0005AF25 File Offset: 0x00059125
		private void OnInputFocusLost()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(false);
				this.m_restoreOnInputAcquired = true;
			}
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x0005AF47 File Offset: 0x00059147
		private void OnInputFocusAcquired()
		{
			if (this.m_restoreOnInputAcquired)
			{
				base.gameObject.SetActive(true);
				this.m_restoreOnInputAcquired = false;
			}
		}

		// Token: 0x04004455 RID: 17493
		[SerializeField]
		private OVRInput.Controller m_controller;

		// Token: 0x04004456 RID: 17494
		[SerializeField]
		private Animator m_animator;

		// Token: 0x04004457 RID: 17495
		private bool m_restoreOnInputAcquired;
	}
}
