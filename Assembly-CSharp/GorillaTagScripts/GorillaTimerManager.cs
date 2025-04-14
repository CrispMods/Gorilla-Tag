using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009C4 RID: 2500
	public class GorillaTimerManager : MonoBehaviour
	{
		// Token: 0x06003E49 RID: 15945 RVA: 0x001270D7 File Offset: 0x001252D7
		protected void Awake()
		{
			if (GorillaTimerManager.hasInstance && GorillaTimerManager.instance != null && GorillaTimerManager.instance != this)
			{
				Object.Destroy(this);
				return;
			}
			GorillaTimerManager.SetInstance(this);
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x00127107 File Offset: 0x00125307
		public static void CreateManager()
		{
			GorillaTimerManager.SetInstance(new GameObject("GorillaTimerManager").AddComponent<GorillaTimerManager>());
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x0012711D File Offset: 0x0012531D
		private static void SetInstance(GorillaTimerManager manager)
		{
			GorillaTimerManager.instance = manager;
			GorillaTimerManager.hasInstance = true;
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(manager);
			}
		}

		// Token: 0x06003E4C RID: 15948 RVA: 0x00127138 File Offset: 0x00125338
		public static void RegisterGorillaTimer(GorillaTimer gTimer)
		{
			if (!GorillaTimerManager.hasInstance)
			{
				GorillaTimerManager.CreateManager();
			}
			if (!GorillaTimerManager.allTimers.Contains(gTimer))
			{
				GorillaTimerManager.allTimers.Add(gTimer);
			}
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x0012715E File Offset: 0x0012535E
		public static void UnregisterGorillaTimer(GorillaTimer gTimer)
		{
			if (!GorillaTimerManager.hasInstance)
			{
				GorillaTimerManager.CreateManager();
			}
			if (GorillaTimerManager.allTimers.Contains(gTimer))
			{
				GorillaTimerManager.allTimers.Remove(gTimer);
			}
		}

		// Token: 0x06003E4E RID: 15950 RVA: 0x00127188 File Offset: 0x00125388
		public void Update()
		{
			for (int i = 0; i < GorillaTimerManager.allTimers.Count; i++)
			{
				GorillaTimerManager.allTimers[i].InvokeUpdate();
			}
		}

		// Token: 0x04003F7D RID: 16253
		public static GorillaTimerManager instance;

		// Token: 0x04003F7E RID: 16254
		public static bool hasInstance = false;

		// Token: 0x04003F7F RID: 16255
		public static List<GorillaTimer> allTimers = new List<GorillaTimer>();
	}
}
