using System;

namespace OculusSampleFramework
{
	// Token: 0x02000A76 RID: 2678
	public class ColliderZoneArgs : EventArgs
	{
		// Token: 0x060042DA RID: 17114 RVA: 0x0005BA47 File Offset: 0x00059C47
		public ColliderZoneArgs(ColliderZone collider, float frameTime, InteractableTool collidingTool, InteractionType interactionType)
		{
			this.Collider = collider;
			this.FrameTime = frameTime;
			this.CollidingTool = collidingTool;
			this.InteractionT = interactionType;
		}

		// Token: 0x040043AA RID: 17322
		public readonly ColliderZone Collider;

		// Token: 0x040043AB RID: 17323
		public readonly float FrameTime;

		// Token: 0x040043AC RID: 17324
		public readonly InteractableTool CollidingTool;

		// Token: 0x040043AD RID: 17325
		public readonly InteractionType InteractionT;
	}
}
