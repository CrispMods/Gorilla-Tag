using System;

namespace OculusSampleFramework
{
	// Token: 0x02000A52 RID: 2642
	public class InteractableStateArgs : EventArgs
	{
		// Token: 0x060041CC RID: 16844 RVA: 0x00137523 File Offset: 0x00135723
		public InteractableStateArgs(Interactable interactable, InteractableTool tool, InteractableState newInteractableState, InteractableState oldState, ColliderZoneArgs colliderArgs)
		{
			this.Interactable = interactable;
			this.Tool = tool;
			this.NewInteractableState = newInteractableState;
			this.OldInteractableState = oldState;
			this.ColliderArgs = colliderArgs;
		}

		// Token: 0x040042E0 RID: 17120
		public readonly Interactable Interactable;

		// Token: 0x040042E1 RID: 17121
		public readonly InteractableTool Tool;

		// Token: 0x040042E2 RID: 17122
		public readonly InteractableState OldInteractableState;

		// Token: 0x040042E3 RID: 17123
		public readonly InteractableState NewInteractableState;

		// Token: 0x040042E4 RID: 17124
		public readonly ColliderZoneArgs ColliderArgs;
	}
}
