using System;
using UnityEngine;

namespace OVRTouchSample
{
	// Token: 0x02000A82 RID: 2690
	public class TouchController : MonoBehaviour
	{
		// Token: 0x0600430D RID: 17165 RVA: 0x0013BEF4 File Offset: 0x0013A0F4
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

		// Token: 0x0600430E RID: 17166 RVA: 0x0013BFF1 File Offset: 0x0013A1F1
		private void OnInputFocusLost()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(false);
				this.m_restoreOnInputAcquired = true;
			}
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x0013C013 File Offset: 0x0013A213
		private void OnInputFocusAcquired()
		{
			if (this.m_restoreOnInputAcquired)
			{
				base.gameObject.SetActive(true);
				this.m_restoreOnInputAcquired = false;
			}
		}

		// Token: 0x04004443 RID: 17475
		[SerializeField]
		private OVRInput.Controller m_controller;

		// Token: 0x04004444 RID: 17476
		[SerializeField]
		private Animator m_animator;

		// Token: 0x04004445 RID: 17477
		private bool m_restoreOnInputAcquired;
	}
}
