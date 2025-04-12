using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A56 RID: 2646
	public class InteractableRegistry : MonoBehaviour
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060041D9 RID: 16857 RVA: 0x0005A243 File Offset: 0x00058443
		public static HashSet<Interactable> Interactables
		{
			get
			{
				return InteractableRegistry._interactables;
			}
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x0005A24A File Offset: 0x0005844A
		public static void RegisterInteractable(Interactable interactable)
		{
			InteractableRegistry.Interactables.Add(interactable);
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x0005A258 File Offset: 0x00058458
		public static void UnregisterInteractable(Interactable interactable)
		{
			InteractableRegistry.Interactables.Remove(interactable);
		}

		// Token: 0x040042F7 RID: 17143
		public static HashSet<Interactable> _interactables = new HashSet<Interactable>();
	}
}
