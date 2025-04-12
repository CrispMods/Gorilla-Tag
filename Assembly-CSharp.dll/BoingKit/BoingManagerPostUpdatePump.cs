using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CCC RID: 3276
	public class BoingManagerPostUpdatePump : MonoBehaviour
	{
		// Token: 0x06005299 RID: 21145 RVA: 0x0003C001 File Offset: 0x0003A201
		private void Start()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x00064BD2 File Offset: 0x00062DD2
		private bool TryDestroyDuplicate()
		{
			if (BoingManager.s_managerGo == base.gameObject)
			{
				return false;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return true;
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x00064BF4 File Offset: 0x00062DF4
		private void FixedUpdate()
		{
			if (this.TryDestroyDuplicate())
			{
				return;
			}
			BoingManager.Execute(BoingManager.UpdateMode.FixedUpdate);
		}

		// Token: 0x0600529C RID: 21148 RVA: 0x00064C05 File Offset: 0x00062E05
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

		// Token: 0x0600529D RID: 21149 RVA: 0x00064C28 File Offset: 0x00062E28
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
