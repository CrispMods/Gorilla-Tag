using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CFB RID: 3323
	public class BoingManagerPreUpdatePump : MonoBehaviour
	{
		// Token: 0x060053F5 RID: 21493 RVA: 0x000666D3 File Offset: 0x000648D3
		private void FixedUpdate()
		{
			this.TryPump();
		}

		// Token: 0x060053F6 RID: 21494 RVA: 0x000666D3 File Offset: 0x000648D3
		private void Update()
		{
			this.TryPump();
		}

		// Token: 0x060053F7 RID: 21495 RVA: 0x000666DB File Offset: 0x000648DB
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

		// Token: 0x060053F8 RID: 21496 RVA: 0x00066705 File Offset: 0x00064905
		private void DoPump()
		{
			BoingManager.RestoreBehaviors();
			BoingManager.RestoreReactors();
			BoingManager.RestoreBones();
			BoingManager.DispatchReactorFieldCompute();
		}

		// Token: 0x040055DA RID: 21978
		private int m_lastPumpedFrame = -1;
	}
}
