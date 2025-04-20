using System;

namespace OculusSampleFramework
{
	// Token: 0x02000A89 RID: 2697
	public class InteractableCollisionInfo
	{
		// Token: 0x06004349 RID: 17225 RVA: 0x0005BE0A File Offset: 0x0005A00A
		public InteractableCollisionInfo(ColliderZone collider, InteractableCollisionDepth collisionDepth, InteractableTool collidingTool)
		{
			this.InteractableCollider = collider;
			this.CollisionDepth = collisionDepth;
			this.CollidingTool = collidingTool;
		}

		// Token: 0x0400440E RID: 17422
		public ColliderZone InteractableCollider;

		// Token: 0x0400440F RID: 17423
		public InteractableCollisionDepth CollisionDepth;

		// Token: 0x04004410 RID: 17424
		public InteractableTool CollidingTool;
	}
}
