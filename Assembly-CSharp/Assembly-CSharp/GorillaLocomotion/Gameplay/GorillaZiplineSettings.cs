using System;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B5B RID: 2907
	[CreateAssetMenu(fileName = "GorillaZiplineSettings", menuName = "ScriptableObjects/GorillaZiplineSettings", order = 0)]
	public class GorillaZiplineSettings : ScriptableObject
	{
		// Token: 0x04004B54 RID: 19284
		public float minSlidePitch = 0.5f;

		// Token: 0x04004B55 RID: 19285
		public float maxSlidePitch = 1f;

		// Token: 0x04004B56 RID: 19286
		public float minSlideVolume;

		// Token: 0x04004B57 RID: 19287
		public float maxSlideVolume = 0.2f;

		// Token: 0x04004B58 RID: 19288
		public float maxSpeed = 10f;

		// Token: 0x04004B59 RID: 19289
		public float gravityMulti = 1.1f;

		// Token: 0x04004B5A RID: 19290
		[Header("Friction")]
		public float friction = 0.25f;

		// Token: 0x04004B5B RID: 19291
		public float maxFriction = 1f;

		// Token: 0x04004B5C RID: 19292
		public float maxFrictionSpeed = 15f;
	}
}
