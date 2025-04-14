using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009C7 RID: 2503
	public class GorillaTimerManager : MonoBehaviour
	{
		// Token: 0x06003E55 RID: 15957 RVA: 0x0012769F File Offset: 0x0012589F
		protected void Awake()
		{
			if (GorillaTimerManager.hasInstance && GorillaTimerManager.instance != null && GorillaTimerManager.instance != this)
			{
				Object.Destroy(this);
				return;
			}
			GorillaTimerManager.SetInstance(this);
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x001276CF File Offset: 0x001258CF
		public static void CreateManager()
		{
			GorillaTimerManager.SetInstance(new GameObject("GorillaTimerManager").AddComponent<GorillaTimerManager>());
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x001276E5 File Offset: 0x001258E5
		private static void SetInstance(GorillaTimerManager manager)
		{
			GorillaTimerManager.instance = manager;
			GorillaTimerManager.hasInstance = true;
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(manager);
			}
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x00127700 File Offset: 0x00125900
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

		// Token: 0x06003E59 RID: 15961 RVA: 0x00127726 File Offset: 0x00125926
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

		// Token: 0x06003E5A RID: 15962 RVA: 0x00127750 File Offset: 0x00125950
		public void Update()
		{
			for (int i = 0; i < GorillaTimerManager.allTimers.Count; i++)
			{
				GorillaTimerManager.allTimers[i].InvokeUpdate();
			}
		}

		// Token: 0x04003F8F RID: 16271
		public static GorillaTimerManager instance;

		// Token: 0x04003F90 RID: 16272
		public static bool hasInstance = false;

		// Token: 0x04003F91 RID: 16273
		public static List<GorillaTimer> allTimers = new List<GorillaTimer>();
	}
}
