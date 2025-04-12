using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A43 RID: 2627
	public class GrabbableCrosshair : MonoBehaviour
	{
		// Token: 0x06004179 RID: 16761 RVA: 0x00059F0D File Offset: 0x0005810D
		private void Start()
		{
			this.m_centerEyeAnchor = GameObject.Find("CenterEyeAnchor").transform;
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x0016F5F4 File Offset: 0x0016D7F4
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

		// Token: 0x0600417B RID: 16763 RVA: 0x00059F24 File Offset: 0x00058124
		private void Update()
		{
			if (this.m_state != GrabbableCrosshair.CrosshairState.Disabled)
			{
				base.transform.LookAt(this.m_centerEyeAnchor);
			}
		}

		// Token: 0x0400429F RID: 17055
		private GrabbableCrosshair.CrosshairState m_state;

		// Token: 0x040042A0 RID: 17056
		private Transform m_centerEyeAnchor;

		// Token: 0x040042A1 RID: 17057
		[SerializeField]
		private GameObject m_targetedCrosshair;

		// Token: 0x040042A2 RID: 17058
		[SerializeField]
		private GameObject m_enabledCrosshair;

		// Token: 0x02000A44 RID: 2628
		public enum CrosshairState
		{
			// Token: 0x040042A4 RID: 17060
			Disabled,
			// Token: 0x040042A5 RID: 17061
			Enabled,
			// Token: 0x040042A6 RID: 17062
			Targeted
		}
	}
}
