using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A46 RID: 2630
	public class PauseOnInputLoss : MonoBehaviour
	{
		// Token: 0x06004180 RID: 16768 RVA: 0x00136BE7 File Offset: 0x00134DE7
		private void Start()
		{
			OVRManager.InputFocusAcquired += this.OnInputFocusAcquired;
			OVRManager.InputFocusLost += this.OnInputFocusLost;
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x00136C0B File Offset: 0x00134E0B
		private void OnInputFocusLost()
		{
			Time.timeScale = 0f;
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x00136C17 File Offset: 0x00134E17
		private void OnInputFocusAcquired()
		{
			Time.timeScale = 1f;
		}
	}
}
