using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B54 RID: 2900
	public class GorillaRopeSwingUpdateManager : MonoBehaviour
	{
		// Token: 0x0600488B RID: 18571 RVA: 0x0015FCDE File Offset: 0x0015DEDE
		protected void Awake()
		{
			if (GorillaRopeSwingUpdateManager.hasInstance && GorillaRopeSwingUpdateManager.instance != null && GorillaRopeSwingUpdateManager.instance != this)
			{
				Object.Destroy(this);
				return;
			}
			GorillaRopeSwingUpdateManager.SetInstance(this);
		}

		// Token: 0x0600488C RID: 18572 RVA: 0x0015FD0E File Offset: 0x0015DF0E
		public static void CreateManager()
		{
			GorillaRopeSwingUpdateManager.SetInstance(new GameObject("GorillaRopeSwingUpdateManager").AddComponent<GorillaRopeSwingUpdateManager>());
		}

		// Token: 0x0600488D RID: 18573 RVA: 0x0015FD24 File Offset: 0x0015DF24
		private static void SetInstance(GorillaRopeSwingUpdateManager manager)
		{
			GorillaRopeSwingUpdateManager.instance = manager;
			GorillaRopeSwingUpdateManager.hasInstance = true;
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(manager);
			}
		}

		// Token: 0x0600488E RID: 18574 RVA: 0x0015FD3F File Offset: 0x0015DF3F
		public static void RegisterRopeSwing(GorillaRopeSwing ropeSwing)
		{
			if (!GorillaRopeSwingUpdateManager.hasInstance)
			{
				GorillaRopeSwingUpdateManager.CreateManager();
			}
			if (!GorillaRopeSwingUpdateManager.allGorillaRopeSwings.Contains(ropeSwing))
			{
				GorillaRopeSwingUpdateManager.allGorillaRopeSwings.Add(ropeSwing);
			}
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x0015FD65 File Offset: 0x0015DF65
		public static void UnregisterRopeSwing(GorillaRopeSwing ropeSwing)
		{
			if (!GorillaRopeSwingUpdateManager.hasInstance)
			{
				GorillaRopeSwingUpdateManager.CreateManager();
			}
			if (GorillaRopeSwingUpdateManager.allGorillaRopeSwings.Contains(ropeSwing))
			{
				GorillaRopeSwingUpdateManager.allGorillaRopeSwings.Remove(ropeSwing);
			}
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x0015FD8C File Offset: 0x0015DF8C
		public void Update()
		{
			for (int i = 0; i < GorillaRopeSwingUpdateManager.allGorillaRopeSwings.Count; i++)
			{
				GorillaRopeSwingUpdateManager.allGorillaRopeSwings[i].InvokeUpdate();
			}
		}

		// Token: 0x04004B2D RID: 19245
		public static GorillaRopeSwingUpdateManager instance;

		// Token: 0x04004B2E RID: 19246
		public static bool hasInstance = false;

		// Token: 0x04004B2F RID: 19247
		public static List<GorillaRopeSwing> allGorillaRopeSwings = new List<GorillaRopeSwing>();
	}
}
