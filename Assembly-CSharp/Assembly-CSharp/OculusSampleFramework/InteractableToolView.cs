using System;

namespace OculusSampleFramework
{
	// Token: 0x02000A61 RID: 2657
	public interface InteractableToolView
	{
		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06004227 RID: 16935
		InteractableTool InteractableTool { get; }

		// Token: 0x06004228 RID: 16936
		void SetFocusedInteractable(Interactable interactable);

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06004229 RID: 16937
		// (set) Token: 0x0600422A RID: 16938
		bool EnableState { get; set; }

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x0600422B RID: 16939
		// (set) Token: 0x0600422C RID: 16940
		bool ToolActivateState { get; set; }
	}
}
