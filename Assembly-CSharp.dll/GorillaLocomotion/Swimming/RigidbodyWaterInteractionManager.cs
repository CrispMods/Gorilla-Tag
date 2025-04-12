using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B46 RID: 2886
	public class RigidbodyWaterInteractionManager : MonoBehaviour
	{
		// Token: 0x06004822 RID: 18466 RVA: 0x0005E223 File Offset: 0x0005C423
		protected void Awake()
		{
			if (RigidbodyWaterInteractionManager.hasInstance && RigidbodyWaterInteractionManager.instance != this)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			RigidbodyWaterInteractionManager.SetInstance(this);
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x0005E246 File Offset: 0x0005C446
		public static void CreateManager()
		{
			RigidbodyWaterInteractionManager.SetInstance(new GameObject("RigidbodyWaterInteractionManager").AddComponent<RigidbodyWaterInteractionManager>());
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x0005E25C File Offset: 0x0005C45C
		private static void SetInstance(RigidbodyWaterInteractionManager manager)
		{
			RigidbodyWaterInteractionManager.instance = manager;
			RigidbodyWaterInteractionManager.hasInstance = true;
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(manager);
			}
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x0005E277 File Offset: 0x0005C477
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

		// Token: 0x06004826 RID: 18470 RVA: 0x0005E29D File Offset: 0x0005C49D
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

		// Token: 0x06004827 RID: 18471 RVA: 0x00190990 File Offset: 0x0018EB90
		public void FixedUpdate()
		{
			for (int i = 0; i < RigidbodyWaterInteractionManager.allrBWI.Count; i++)
			{
				RigidbodyWaterInteractionManager.allrBWI[i].InvokeFixedUpdate();
			}
		}

		// Token: 0x04004A97 RID: 19095
		public static RigidbodyWaterInteractionManager instance;

		// Token: 0x04004A98 RID: 19096
		[OnEnterPlay_Set(false)]
		public static bool hasInstance = false;

		// Token: 0x04004A99 RID: 19097
		public static List<RigidbodyWaterInteraction> allrBWI = new List<RigidbodyWaterInteraction>();
	}
}
