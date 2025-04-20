using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CFA RID: 3322
	public class BoingManagerPostUpdatePump : MonoBehaviour
	{
		// Token: 0x060053EF RID: 21487 RVA: 0x0003D2C1 File Offset: 0x0003B4C1
		private void Start()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x060053F0 RID: 21488 RVA: 0x00066648 File Offset: 0x00064848
		private bool TryDestroyDuplicate()
		{
			if (BoingManager.s_managerGo == base.gameObject)
			{
				return false;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return true;
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x0006666A File Offset: 0x0006486A
		private void FixedUpdate()
		{
			if (this.TryDestroyDuplicate())
			{
				return;
			}
			BoingManager.Execute(BoingManager.UpdateMode.FixedUpdate);
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x0006667B File Offset: 0x0006487B
		private void Update()
		{
			if (this.TryDestroyDuplicate())
			{
				return;
			}
			BoingManager.Execute(BoingManager.UpdateMode.EarlyUpdate);
			BoingManager.PullBehaviorResults(BoingManager.UpdateMode.EarlyUpdate);
			BoingManager.PullReactorResults(BoingManager.UpdateMode.EarlyUpdate);
			BoingManager.PullBonesResults(BoingManager.UpdateMode.EarlyUpdate);
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x0006669E File Offset: 0x0006489E
		private void LateUpdate()
		{
			if (this.TryDestroyDuplicate())
			{
				return;
			}
			BoingManager.PullBehaviorResults(BoingManager.UpdateMode.FixedUpdate);
			BoingManager.PullReactorResults(BoingManager.UpdateMode.FixedUpdate);
			BoingManager.PullBonesResults(BoingManager.UpdateMode.FixedUpdate);
			BoingManager.Execute(BoingManager.UpdateMode.LateUpdate);
			BoingManager.PullBehaviorResults(BoingManager.UpdateMode.LateUpdate);
			BoingManager.PullReactorResults(BoingManager.UpdateMode.LateUpdate);
			BoingManager.PullBonesResults(BoingManager.UpdateMode.LateUpdate);
		}
	}
}
