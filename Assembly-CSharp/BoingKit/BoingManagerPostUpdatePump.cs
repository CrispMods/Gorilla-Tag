using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CC9 RID: 3273
	public class BoingManagerPostUpdatePump : MonoBehaviour
	{
		// Token: 0x0600528D RID: 21133 RVA: 0x0005CFAC File Offset: 0x0005B1AC
		private void Start()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x0019438F File Offset: 0x0019258F
		private bool TryDestroyDuplicate()
		{
			if (BoingManager.s_managerGo == base.gameObject)
			{
				return false;
			}
			Object.Destroy(base.gameObject);
			return true;
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x001943B1 File Offset: 0x001925B1
		private void FixedUpdate()
		{
			if (this.TryDestroyDuplicate())
			{
				return;
			}
			BoingManager.Execute(BoingManager.UpdateMode.FixedUpdate);
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x001943C2 File Offset: 0x001925C2
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

		// Token: 0x06005291 RID: 21137 RVA: 0x001943E5 File Offset: 0x001925E5
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
