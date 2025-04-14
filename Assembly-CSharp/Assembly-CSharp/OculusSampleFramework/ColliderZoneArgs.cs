using System;

namespace OculusSampleFramework
{
	// Token: 0x02000A4C RID: 2636
	public class ColliderZoneArgs : EventArgs
	{
		// Token: 0x060041A1 RID: 16801 RVA: 0x00137313 File Offset: 0x00135513
		public ColliderZoneArgs(ColliderZone collider, float frameTime, InteractableTool collidingTool, InteractionType interactionType)
		{
			this.Collider = collider;
			this.FrameTime = frameTime;
			this.CollidingTool = collidingTool;
			this.InteractionT = interactionType;
		}

		// Token: 0x040042C2 RID: 17090
		public readonly ColliderZone Collider;

		// Token: 0x040042C3 RID: 17091
		public readonly float FrameTime;

		// Token: 0x040042C4 RID: 17092
		public readonly InteractableTool CollidingTool;

		// Token: 0x040042C5 RID: 17093
		public readonly InteractionType InteractionT;
	}
}
