using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A46 RID: 2630
	public class PauseOnInputLoss : MonoBehaviour
	{
		// Token: 0x06004180 RID: 16768 RVA: 0x00059F3F File Offset: 0x0005813F
		private void Start()
		{
			OVRManager.InputFocusAcquired += this.OnInputFocusAcquired;
			OVRManager.InputFocusLost += this.OnInputFocusLost;
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x00059F63 File Offset: 0x00058163
		private void OnInputFocusLost()
		{
			Time.timeScale = 0f;
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x00059F6F File Offset: 0x0005816F
		private void OnInputFocusAcquired()
		{
			Time.timeScale = 1f;
		}
	}
}
