using System;

namespace OculusSampleFramework
{
	// Token: 0x02000A7F RID: 2687
	public class InteractableStateArgs : EventArgs
	{
		// Token: 0x06004311 RID: 17169 RVA: 0x0005BC18 File Offset: 0x00059E18
		public InteractableStateArgs(Interactable interactable, InteractableTool tool, InteractableState newInteractableState, InteractableState oldState, ColliderZoneArgs colliderArgs)
		{
			this.Interactable = interactable;
			this.Tool = tool;
			this.NewInteractableState = newInteractableState;
			this.OldInteractableState = oldState;
			this.ColliderArgs = colliderArgs;
		}

		// Token: 0x040043DA RID: 17370
		public readonly Interactable Interactable;

		// Token: 0x040043DB RID: 17371
		public readonly InteractableTool Tool;

		// Token: 0x040043DC RID: 17372
		public readonly InteractableState OldInteractableState;

		// Token: 0x040043DD RID: 17373
		public readonly InteractableState NewInteractableState;

		// Token: 0x040043DE RID: 17374
		public readonly ColliderZoneArgs ColliderArgs;
	}
}
