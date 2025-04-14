using System;

namespace OculusSampleFramework
{
	// Token: 0x02000A5F RID: 2655
	public class InteractableCollisionInfo
	{
		// Token: 0x06004210 RID: 16912 RVA: 0x001386C9 File Offset: 0x001368C9
		public InteractableCollisionInfo(ColliderZone collider, InteractableCollisionDepth collisionDepth, InteractableTool collidingTool)
		{
			this.InteractableCollider = collider;
			this.CollisionDepth = collisionDepth;
			this.CollidingTool = collidingTool;
		}

		// Token: 0x04004326 RID: 17190
		public ColliderZone InteractableCollider;

		// Token: 0x04004327 RID: 17191
		public InteractableCollisionDepth CollisionDepth;

		// Token: 0x04004328 RID: 17192
		public InteractableTool CollidingTool;
	}
}
