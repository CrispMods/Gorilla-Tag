using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B70 RID: 2928
	public class RigidbodyWaterInteractionManager : MonoBehaviour
	{
		// Token: 0x0600495F RID: 18783 RVA: 0x0005FC10 File Offset: 0x0005DE10
		protected void Awake()
		{
			if (RigidbodyWaterInteractionManager.hasInstance && RigidbodyWaterInteractionManager.instance != this)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			RigidbodyWaterInteractionManager.SetInstance(this);
		}

		// Token: 0x06004960 RID: 18784 RVA: 0x0005FC33 File Offset: 0x0005DE33
		public static void CreateManager()
		{
			RigidbodyWaterInteractionManager.SetInstance(new GameObject("RigidbodyWaterInteractionManager").AddComponent<RigidbodyWaterInteractionManager>());
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x0005FC49 File Offset: 0x0005DE49
		private static void SetInstance(RigidbodyWaterInteractionManager manager)
		{
			RigidbodyWaterInteractionManager.instance = manager;
			RigidbodyWaterInteractionManager.hasInstance = true;
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(manager);
			}
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x0005FC64 File Offset: 0x0005DE64
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

		// Token: 0x06004963 RID: 18787 RVA: 0x0005FC8A File Offset: 0x0005DE8A
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

		// Token: 0x06004964 RID: 18788 RVA: 0x001979B0 File Offset: 0x00195BB0
		public void FixedUpdate()
		{
			for (int i = 0; i < RigidbodyWaterInteractionManager.allrBWI.Count; i++)
			{
				RigidbodyWaterInteractionManager.allrBWI[i].InvokeFixedUpdate();
			}
		}

		// Token: 0x04004B7B RID: 19323
		public static RigidbodyWaterInteractionManager instance;

		// Token: 0x04004B7C RID: 19324
		[OnEnterPlay_Set(false)]
		public static bool hasInstance = false;

		// Token: 0x04004B7D RID: 19325
		public static List<RigidbodyWaterInteraction> allrBWI = new List<RigidbodyWaterInteraction>();
	}
}
