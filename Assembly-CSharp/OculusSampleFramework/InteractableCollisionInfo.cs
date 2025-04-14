using System;

namespace OculusSampleFramework
{
	// Token: 0x02000A5C RID: 2652
	public class InteractableCollisionInfo
	{
		// Token: 0x06004204 RID: 16900 RVA: 0x00138101 File Offset: 0x00136301
		public InteractableCollisionInfo(ColliderZone collider, InteractableCollisionDepth collisionDepth, InteractableTool collidingTool)
		{
			this.InteractableCollider = collider;
			this.CollisionDepth = collisionDepth;
			this.CollidingTool = collidingTool;
		}

		// Token: 0x04004314 RID: 17172
		public ColliderZone InteractableCollider;

		// Token: 0x04004315 RID: 17173
		public InteractableCollisionDepth CollisionDepth;

		// Token: 0x04004316 RID: 17174
		public InteractableTool CollidingTool;
	}
}
