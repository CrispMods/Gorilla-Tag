using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A80 RID: 2688
	public class InteractableRegistry : MonoBehaviour
	{
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06004312 RID: 17170 RVA: 0x0005BC45 File Offset: 0x00059E45
		public static HashSet<Interactable> Interactables
		{
			get
			{
				return InteractableRegistry._interactables;
			}
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x0005BC4C File Offset: 0x00059E4C
		public static void RegisterInteractable(Interactable interactable)
		{
			InteractableRegistry.Interactables.Add(interactable);
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x0005BC5A File Offset: 0x00059E5A
		public static void UnregisterInteractable(Interactable interactable)
		{
			InteractableRegistry.Interactables.Remove(interactable);
		}

		// Token: 0x040043DF RID: 17375
		public static HashSet<Interactable> _interactables = new HashSet<Interactable>();
	}
}
