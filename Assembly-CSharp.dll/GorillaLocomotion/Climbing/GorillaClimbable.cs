using System;
using UnityEngine;

namespace GorillaLocomotion.Climbing
{
	// Token: 0x02000B67 RID: 2919
	public class GorillaClimbable : MonoBehaviour
	{
		// Token: 0x06004906 RID: 18694 RVA: 0x0005EAC1 File Offset: 0x0005CCC1
		private void Awake()
		{
			this.colliderCache = base.GetComponent<Collider>();
		}

		// Token: 0x04004BB3 RID: 19379
		public bool snapX;

		// Token: 0x04004BB4 RID: 19380
		public bool snapY;

		// Token: 0x04004BB5 RID: 19381
		public bool snapZ;

		// Token: 0x04004BB6 RID: 19382
		public float maxDistanceSnap = 0.05f;

		// Token: 0x04004BB7 RID: 19383
		public AudioClip clip;

		// Token: 0x04004BB8 RID: 19384
		public AudioClip clipOnFullRelease;

		// Token: 0x04004BB9 RID: 19385
		public Action<GorillaHandClimber, GorillaClimbableRef> onBeforeClimb;

		// Token: 0x04004BBA RID: 19386
		public bool climbOnlyWhileSmall;

		// Token: 0x04004BBB RID: 19387
		public bool IsPlayerAttached;

		// Token: 0x04004BBC RID: 19388
		[NonSerialized]
		public bool isBeingClimbed;

		// Token: 0x04004BBD RID: 19389
		[NonSerialized]
		public Collider colliderCache;
	}
}
