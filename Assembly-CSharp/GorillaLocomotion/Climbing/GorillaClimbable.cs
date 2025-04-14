using System;
using UnityEngine;

namespace GorillaLocomotion.Climbing
{
	// Token: 0x02000B64 RID: 2916
	public class GorillaClimbable : MonoBehaviour
	{
		// Token: 0x060048FA RID: 18682 RVA: 0x00162BD3 File Offset: 0x00160DD3
		private void Awake()
		{
			this.colliderCache = base.GetComponent<Collider>();
		}

		// Token: 0x04004BA1 RID: 19361
		public bool snapX;

		// Token: 0x04004BA2 RID: 19362
		public bool snapY;

		// Token: 0x04004BA3 RID: 19363
		public bool snapZ;

		// Token: 0x04004BA4 RID: 19364
		public float maxDistanceSnap = 0.05f;

		// Token: 0x04004BA5 RID: 19365
		public AudioClip clip;

		// Token: 0x04004BA6 RID: 19366
		public AudioClip clipOnFullRelease;

		// Token: 0x04004BA7 RID: 19367
		public Action<GorillaHandClimber, GorillaClimbableRef> onBeforeClimb;

		// Token: 0x04004BA8 RID: 19368
		public bool climbOnlyWhileSmall;

		// Token: 0x04004BA9 RID: 19369
		public bool IsPlayerAttached;

		// Token: 0x04004BAA RID: 19370
		[NonSerialized]
		public bool isBeingClimbed;

		// Token: 0x04004BAB RID: 19371
		[NonSerialized]
		public Collider colliderCache;
	}
}
