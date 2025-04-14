using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A53 RID: 2643
	public class InteractableRegistry : MonoBehaviour
	{
		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x00137550 File Offset: 0x00135750
		public static HashSet<Interactable> Interactables
		{
			get
			{
				return InteractableRegistry._interactables;
			}
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x00137557 File Offset: 0x00135757
		public static void RegisterInteractable(Interactable interactable)
		{
			InteractableRegistry.Interactables.Add(interactable);
		}

		// Token: 0x060041CF RID: 16847 RVA: 0x00137565 File Offset: 0x00135765
		public static void UnregisterInteractable(Interactable interactable)
		{
			InteractableRegistry.Interactables.Remove(interactable);
		}

		// Token: 0x040042E5 RID: 17125
		public static HashSet<Interactable> _interactables = new HashSet<Interactable>();
	}
}
