using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CCD RID: 3277
	public class BoingManagerPreUpdatePump : MonoBehaviour
	{
		// Token: 0x0600529F RID: 21151 RVA: 0x001949E2 File Offset: 0x00192BE2
		private void FixedUpdate()
		{
			this.TryPump();
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x001949E2 File Offset: 0x00192BE2
		private void Update()
		{
			this.TryPump();
		}

		// Token: 0x060052A1 RID: 21153 RVA: 0x001949EA File Offset: 0x00192BEA
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

		// Token: 0x060052A2 RID: 21154 RVA: 0x00194A14 File Offset: 0x00192C14
		private void DoPump()
		{
			BoingManager.RestoreBehaviors();
			BoingManager.RestoreReactors();
			BoingManager.RestoreBones();
			BoingManager.DispatchReactorFieldCompute();
		}

		// Token: 0x040054E0 RID: 21728
		private int m_lastPumpedFrame = -1;
	}
}
