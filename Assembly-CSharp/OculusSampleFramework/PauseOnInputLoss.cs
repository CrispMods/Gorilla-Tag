using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A70 RID: 2672
	public class PauseOnInputLoss : MonoBehaviour
	{
		// Token: 0x060042B9 RID: 17081 RVA: 0x0005B941 File Offset: 0x00059B41
		private void Start()
		{
			OVRManager.InputFocusAcquired += this.OnInputFocusAcquired;
			OVRManager.InputFocusLost += this.OnInputFocusLost;
		}

		// Token: 0x060042BA RID: 17082 RVA: 0x0005B965 File Offset: 0x00059B65
		private void OnInputFocusLost()
		{
			Time.timeScale = 0f;
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x0005B971 File Offset: 0x00059B71
		private void OnInputFocusAcquired()
		{
			Time.timeScale = 1f;
		}
	}
}
