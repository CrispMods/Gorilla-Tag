using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009EA RID: 2538
	public class GorillaTimerManager : MonoBehaviour
	{
		// Token: 0x06003F61 RID: 16225 RVA: 0x00059530 File Offset: 0x00057730
		protected void Awake()
		{
			if (GorillaTimerManager.hasInstance && GorillaTimerManager.instance != null && GorillaTimerManager.instance != this)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			GorillaTimerManager.SetInstance(this);
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x00059560 File Offset: 0x00057760
		public static void CreateManager()
		{
			GorillaTimerManager.SetInstance(new GameObject("GorillaTimerManager").AddComponent<GorillaTimerManager>());
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x00059576 File Offset: 0x00057776
		private static void SetInstance(GorillaTimerManager manager)
		{
			GorillaTimerManager.instance = manager;
			GorillaTimerManager.hasInstance = true;
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(manager);
			}
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x00059591 File Offset: 0x00057791
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

		// Token: 0x06003F65 RID: 16229 RVA: 0x000595B7 File Offset: 0x000577B7
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

		// Token: 0x06003F66 RID: 16230 RVA: 0x00168640 File Offset: 0x00166840
		public void Update()
		{
			for (int i = 0; i < GorillaTimerManager.allTimers.Count; i++)
			{
				GorillaTimerManager.allTimers[i].InvokeUpdate();
			}
		}

		// Token: 0x04004057 RID: 16471
		public static GorillaTimerManager instance;

		// Token: 0x04004058 RID: 16472
		public static bool hasInstance = false;

		// Token: 0x04004059 RID: 16473
		public static List<GorillaTimer> allTimers = new List<GorillaTimer>();
	}
}
