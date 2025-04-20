using System;
using UnityEngine;

namespace OVRTouchSample
{
	// Token: 0x02000AAF RID: 2735
	public class TouchController : MonoBehaviour
	{
		// Token: 0x06004452 RID: 17490 RVA: 0x0017AE08 File Offset: 0x00179008
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

		// Token: 0x06004453 RID: 17491 RVA: 0x0005C927 File Offset: 0x0005AB27
		private void OnInputFocusLost()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(false);
				this.m_restoreOnInputAcquired = true;
			}
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x0005C949 File Offset: 0x0005AB49
		private void OnInputFocusAcquired()
		{
			if (this.m_restoreOnInputAcquired)
			{
				base.gameObject.SetActive(true);
				this.m_restoreOnInputAcquired = false;
			}
		}

		// Token: 0x0400453D RID: 17725
		[SerializeField]
		private OVRInput.Controller m_controller;

		// Token: 0x0400453E RID: 17726
		[SerializeField]
		private Animator m_animator;

		// Token: 0x0400453F RID: 17727
		private bool m_restoreOnInputAcquired;
	}
}
