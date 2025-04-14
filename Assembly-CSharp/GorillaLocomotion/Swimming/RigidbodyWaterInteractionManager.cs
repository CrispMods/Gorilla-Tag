using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B43 RID: 2883
	public class RigidbodyWaterInteractionManager : MonoBehaviour
	{
		// Token: 0x06004816 RID: 18454 RVA: 0x0015BD3E File Offset: 0x00159F3E
		protected void Awake()
		{
			if (RigidbodyWaterInteractionManager.hasInstance && RigidbodyWaterInteractionManager.instance != this)
			{
				Object.Destroy(this);
				return;
			}
			RigidbodyWaterInteractionManager.SetInstance(this);
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x0015BD61 File Offset: 0x00159F61
		public static void CreateManager()
		{
			RigidbodyWaterInteractionManager.SetInstance(new GameObject("RigidbodyWaterInteractionManager").AddComponent<RigidbodyWaterInteractionManager>());
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x0015BD77 File Offset: 0x00159F77
		private static void SetInstance(RigidbodyWaterInteractionManager manager)
		{
			RigidbodyWaterInteractionManager.instance = manager;
			RigidbodyWaterInteractionManager.hasInstance = true;
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(manager);
			}
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x0015BD92 File Offset: 0x00159F92
		public static void RegisterRBWI(RigidbodyWaterInteraction rbWI)
		{
			if (!RigidbodyWaterInteractionManager.hasInstance)
			{
				RigidbodyWaterInteractionManager.CreateManager();
			}
			if (!RigidbodyWaterInteractionManager.allrBWI.Contains(rbWI))
			{
				RigidbodyWaterInteractionManager.allrBWI.Add(rbWI);
			}
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x0015BDB8 File Offset: 0x00159FB8
		public static void UnregisterRBWI(RigidbodyWaterInteraction rbWI)
		{
			if (!RigidbodyWaterInteractionManager.hasInstance)
			{
				RigidbodyWaterInteractionManager.CreateManager();
			}
			if (RigidbodyWaterInteractionManager.allrBWI.Contains(rbWI))
			{
				RigidbodyWaterInteractionManager.allrBWI.Remove(rbWI);
			}
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x0015BDE0 File Offset: 0x00159FE0
		public void FixedUpdate()
		{
			for (int i = 0; i < RigidbodyWaterInteractionManager.allrBWI.Count; i++)
			{
				RigidbodyWaterInteractionManager.allrBWI[i].InvokeFixedUpdate();
			}
		}

		// Token: 0x04004A85 RID: 19077
		public static RigidbodyWaterInteractionManager instance;

		// Token: 0x04004A86 RID: 19078
		[OnEnterPlay_Set(false)]
		public static bool hasInstance = false;

		// Token: 0x04004A87 RID: 19079
		public static List<RigidbodyWaterInteraction> allrBWI = new List<RigidbodyWaterInteraction>();
	}
}
