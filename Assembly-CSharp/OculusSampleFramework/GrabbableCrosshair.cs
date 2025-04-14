using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A40 RID: 2624
	public class GrabbableCrosshair : MonoBehaviour
	{
		// Token: 0x0600416D RID: 16749 RVA: 0x0013653B File Offset: 0x0013473B
		private void Start()
		{
			this.m_centerEyeAnchor = GameObject.Find("CenterEyeAnchor").transform;
		}

		// Token: 0x0600416E RID: 16750 RVA: 0x00136554 File Offset: 0x00134754
		public void SetState(GrabbableCrosshair.CrosshairState cs)
		{
			this.m_state = cs;
			if (cs == GrabbableCrosshair.CrosshairState.Disabled)
			{
				this.m_targetedCrosshair.SetActive(false);
				this.m_enabledCrosshair.SetActive(false);
				return;
			}
			if (cs == GrabbableCrosshair.CrosshairState.Enabled)
			{
				this.m_targetedCrosshair.SetActive(false);
				this.m_enabledCrosshair.SetActive(true);
				return;
			}
			if (cs == GrabbableCrosshair.CrosshairState.Targeted)
			{
				this.m_targetedCrosshair.SetActive(true);
				this.m_enabledCrosshair.SetActive(false);
			}
		}

		// Token: 0x0600416F RID: 16751 RVA: 0x001365BD File Offset: 0x001347BD
		private void Update()
		{
			if (this.m_state != GrabbableCrosshair.CrosshairState.Disabled)
			{
				base.transform.LookAt(this.m_centerEyeAnchor);
			}
		}

		// Token: 0x0400428D RID: 17037
		private GrabbableCrosshair.CrosshairState m_state;

		// Token: 0x0400428E RID: 17038
		private Transform m_centerEyeAnchor;

		// Token: 0x0400428F RID: 17039
		[SerializeField]
		private GameObject m_targetedCrosshair;

		// Token: 0x04004290 RID: 17040
		[SerializeField]
		private GameObject m_enabledCrosshair;

		// Token: 0x02000A41 RID: 2625
		public enum CrosshairState
		{
			// Token: 0x04004292 RID: 17042
			Disabled,
			// Token: 0x04004293 RID: 17043
			Enabled,
			// Token: 0x04004294 RID: 17044
			Targeted
		}
	}
}
