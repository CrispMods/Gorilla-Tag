using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CCA RID: 3274
	public class BoingManagerPreUpdatePump : MonoBehaviour
	{
		// Token: 0x06005293 RID: 21139 RVA: 0x0019441A File Offset: 0x0019261A
		private void FixedUpdate()
		{
			this.TryPump();
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x0019441A File Offset: 0x0019261A
		private void Update()
		{
			this.TryPump();
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x00194422 File Offset: 0x00192622
		private void TryPump()
		{
			if (this.m_lastPumpedFrame >= Time.frameCount)
			{
				return;
			}
			if (this.m_lastPumpedFrame >= 0)
			{
				this.DoPump();
			}
			this.m_lastPumpedFrame = Time.frameCount;
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x0019444C File Offset: 0x0019264C
		private void DoPump()
		{
			BoingManager.RestoreBehaviors();
			BoingManager.RestoreReactors();
			BoingManager.RestoreBones();
			BoingManager.DispatchReactorFieldCompute();
		}

		// Token: 0x040054CE RID: 21710
		private int m_lastPumpedFrame = -1;
	}
}
