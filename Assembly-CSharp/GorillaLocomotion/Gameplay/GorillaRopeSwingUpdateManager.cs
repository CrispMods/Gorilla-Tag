using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B81 RID: 2945
	public class GorillaRopeSwingUpdateManager : MonoBehaviour
	{
		// Token: 0x060049D6 RID: 18902 RVA: 0x0006002B File Offset: 0x0005E22B
		protected void Awake()
		{
			if (GorillaRopeSwingUpdateManager.hasInstance && GorillaRopeSwingUpdateManager.instance != null && GorillaRopeSwingUpdateManager.instance != this)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			GorillaRopeSwingUpdateManager.SetInstance(this);
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x0006005B File Offset: 0x0005E25B
		public static void CreateManager()
		{
			GorillaRopeSwingUpdateManager.SetInstance(new GameObject("GorillaRopeSwingUpdateManager").AddComponent<GorillaRopeSwingUpdateManager>());
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x00060071 File Offset: 0x0005E271
		private static void SetInstance(GorillaRopeSwingUpdateManager manager)
		{
			GorillaRopeSwingUpdateManager.instance = manager;
			GorillaRopeSwingUpdateManager.hasInstance = true;
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(manager);
			}
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x0006008C File Offset: 0x0005E28C
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

		// Token: 0x060049DA RID: 18906 RVA: 0x000600B2 File Offset: 0x0005E2B2
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

		// Token: 0x060049DB RID: 18907 RVA: 0x0019B574 File Offset: 0x00199774
		public void Update()
		{
			for (int i = 0; i < GorillaRopeSwingUpdateManager.allGorillaRopeSwings.Count; i++)
			{
				GorillaRopeSwingUpdateManager.allGorillaRopeSwings[i].InvokeUpdate();
			}
		}

		// Token: 0x04004C23 RID: 19491
		public static GorillaRopeSwingUpdateManager instance;

		// Token: 0x04004C24 RID: 19492
		public static bool hasInstance = false;

		// Token: 0x04004C25 RID: 19493
		public static List<GorillaRopeSwing> allGorillaRopeSwings = new List<GorillaRopeSwing>();
	}
}
