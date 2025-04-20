using System;
using UnityEngine;

namespace GorillaLocomotion.Climbing
{
	// Token: 0x02000B91 RID: 2961
	public class GorillaClimbable : MonoBehaviour
	{
		// Token: 0x06004A45 RID: 19013 RVA: 0x000604F9 File Offset: 0x0005E6F9
		private void Awake()
		{
			this.colliderCache = base.GetComponent<Collider>();
		}

		// Token: 0x04004C97 RID: 19607
		public bool snapX;

		// Token: 0x04004C98 RID: 19608
		public bool snapY;

		// Token: 0x04004C99 RID: 19609
		public bool snapZ;

		// Token: 0x04004C9A RID: 19610
		public float maxDistanceSnap = 0.05f;

		// Token: 0x04004C9B RID: 19611
		public AudioClip clip;

		// Token: 0x04004C9C RID: 19612
		public AudioClip clipOnFullRelease;

		// Token: 0x04004C9D RID: 19613
		public Action<GorillaHandClimber, GorillaClimbableRef> onBeforeClimb;

		// Token: 0x04004C9E RID: 19614
		public bool climbOnlyWhileSmall;

		// Token: 0x04004C9F RID: 19615
		public bool IsPlayerAttached;

		// Token: 0x04004CA0 RID: 19616
		[NonSerialized]
		public bool isBeingClimbed;

		// Token: 0x04004CA1 RID: 19617
		[NonSerialized]
		public Collider colliderCache;
	}
}
