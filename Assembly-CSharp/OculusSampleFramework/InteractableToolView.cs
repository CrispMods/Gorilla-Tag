using System;

namespace OculusSampleFramework
{
	// Token: 0x02000A8B RID: 2699
	public interface InteractableToolView
	{
		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06004360 RID: 17248
		InteractableTool InteractableTool { get; }

		// Token: 0x06004361 RID: 17249
		void SetFocusedInteractable(Interactable interactable);

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06004362 RID: 17250
		// (set) Token: 0x06004363 RID: 17251
		bool EnableState { get; set; }

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06004364 RID: 17252
		// (set) Token: 0x06004365 RID: 17253
		bool ToolActivateState { get; set; }
	}
}
