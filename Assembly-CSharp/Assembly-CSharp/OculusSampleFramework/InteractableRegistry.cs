using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A56 RID: 2646
	public class InteractableRegistry : MonoBehaviour
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060041D9 RID: 16857 RVA: 0x00137B18 File Offset: 0x00135D18
		public static HashSet<Interactable> Interactables
		{
			get
			{
				return InteractableRegistry._interactables;
			}
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x00137B1F File Offset: 0x00135D1F
		public static void RegisterInteractable(Interactable interactable)
		{
			InteractableRegistry.Interactables.Add(interactable);
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x00137B2D File Offset: 0x00135D2D
		public static void UnregisterInteractable(Interactable interactable)
		{
			InteractableRegistry.Interactables.Remove(interactable);
		}

		// Token: 0x040042F7 RID: 17143
		public static HashSet<Interactable> _interactables = new HashSet<Interactable>();
	}
}
