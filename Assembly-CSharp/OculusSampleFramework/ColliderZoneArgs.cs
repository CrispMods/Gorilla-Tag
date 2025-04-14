using System;

namespace OculusSampleFramework
{
	// Token: 0x02000A49 RID: 2633
	public class ColliderZoneArgs : EventArgs
	{
		// Token: 0x06004195 RID: 16789 RVA: 0x00136D4B File Offset: 0x00134F4B
		public ColliderZoneArgs(ColliderZone collider, float frameTime, InteractableTool collidingTool, InteractionType interactionType)
		{
			this.Collider = collider;
			this.FrameTime = frameTime;
			this.CollidingTool = collidingTool;
			this.InteractionT = interactionType;
		}

		// Token: 0x040042B0 RID: 17072
		public readonly ColliderZone Collider;

		// Token: 0x040042B1 RID: 17073
		public readonly float FrameTime;

		// Token: 0x040042B2 RID: 17074
		public readonly InteractableTool CollidingTool;

		// Token: 0x040042B3 RID: 17075
		public readonly InteractionType InteractionT;
	}
}
