using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A43 RID: 2627
	public class PauseOnInputLoss : MonoBehaviour
	{
		// Token: 0x06004174 RID: 16756 RVA: 0x0013661F File Offset: 0x0013481F
		private void Start()
		{
			OVRManager.InputFocusAcquired += this.OnInputFocusAcquired;
			OVRManager.InputFocusLost += this.OnInputFocusLost;
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x00136643 File Offset: 0x00134843
		private void OnInputFocusLost()
		{
			Time.timeScale = 0f;
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x0013664F File Offset: 0x0013484F
		private void OnInputFocusAcquired()
		{
			Time.timeScale = 1f;
		}
	}
}
