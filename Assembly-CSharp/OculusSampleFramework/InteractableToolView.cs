using System;

namespace OculusSampleFramework
{
	// Token: 0x02000A5E RID: 2654
	public interface InteractableToolView
	{
		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x0600421B RID: 16923
		InteractableTool InteractableTool { get; }

		// Token: 0x0600421C RID: 16924
		void SetFocusedInteractable(Interactable interactable);

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x0600421D RID: 16925
		// (set) Token: 0x0600421E RID: 16926
		bool EnableState { get; set; }

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x0600421F RID: 16927
		// (set) Token: 0x06004220 RID: 16928
		bool ToolActivateState { get; set; }
	}
}
