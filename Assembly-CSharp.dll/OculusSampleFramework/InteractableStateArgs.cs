using System;

namespace OculusSampleFramework
{
	// Token: 0x02000A55 RID: 2645
	public class InteractableStateArgs : EventArgs
	{
		// Token: 0x060041D8 RID: 16856 RVA: 0x0005A216 File Offset: 0x00058416
		public InteractableStateArgs(Interactable interactable, InteractableTool tool, InteractableState newInteractableState, InteractableState oldState, ColliderZoneArgs colliderArgs)
		{
			this.Interactable = interactable;
			this.Tool = tool;
			this.NewInteractableState = newInteractableState;
			this.OldInteractableState = oldState;
			this.ColliderArgs = colliderArgs;
		}

		// Token: 0x040042F2 RID: 17138
		public readonly Interactable Interactable;

		// Token: 0x040042F3 RID: 17139
		public readonly InteractableTool Tool;

		// Token: 0x040042F4 RID: 17140
		public readonly InteractableState OldInteractableState;

		// Token: 0x040042F5 RID: 17141
		public readonly InteractableState NewInteractableState;

		// Token: 0x040042F6 RID: 17142
		public readonly ColliderZoneArgs ColliderArgs;
	}
}
