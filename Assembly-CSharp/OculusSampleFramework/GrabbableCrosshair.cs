using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A6D RID: 2669
	public class GrabbableCrosshair : MonoBehaviour
	{
		// Token: 0x060042B2 RID: 17074 RVA: 0x0005B90F File Offset: 0x00059B0F
		private void Start()
		{
			this.m_centerEyeAnchor = GameObject.Find("CenterEyeAnchor").transform;
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x00176478 File Offset: 0x00174678
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

		// Token: 0x060042B4 RID: 17076 RVA: 0x0005B926 File Offset: 0x00059B26
		private void Update()
		{
			if (this.m_state != GrabbableCrosshair.CrosshairState.Disabled)
			{
				base.transform.LookAt(this.m_centerEyeAnchor);
			}
		}

		// Token: 0x04004387 RID: 17287
		private GrabbableCrosshair.CrosshairState m_state;

		// Token: 0x04004388 RID: 17288
		private Transform m_centerEyeAnchor;

		// Token: 0x04004389 RID: 17289
		[SerializeField]
		private GameObject m_targetedCrosshair;

		// Token: 0x0400438A RID: 17290
		[SerializeField]
		private GameObject m_enabledCrosshair;

		// Token: 0x02000A6E RID: 2670
		public enum CrosshairState
		{
			// Token: 0x0400438C RID: 17292
			Disabled,
			// Token: 0x0400438D RID: 17293
			Enabled,
			// Token: 0x0400438E RID: 17294
			Targeted
		}
	}
}
