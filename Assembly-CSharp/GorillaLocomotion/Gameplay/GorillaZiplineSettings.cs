using System;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B58 RID: 2904
	[CreateAssetMenu(fileName = "GorillaZiplineSettings", menuName = "ScriptableObjects/GorillaZiplineSettings", order = 0)]
	public class GorillaZiplineSettings : ScriptableObject
	{
		// Token: 0x04004B42 RID: 19266
		public float minSlidePitch = 0.5f;

		// Token: 0x04004B43 RID: 19267
		public float maxSlidePitch = 1f;

		// Token: 0x04004B44 RID: 19268
		public float minSlideVolume;

		// Token: 0x04004B45 RID: 19269
		public float maxSlideVolume = 0.2f;

		// Token: 0x04004B46 RID: 19270
		public float maxSpeed = 10f;

		// Token: 0x04004B47 RID: 19271
		public float gravityMulti = 1.1f;

		// Token: 0x04004B48 RID: 19272
		[Header("Friction")]
		public float friction = 0.25f;

		// Token: 0x04004B49 RID: 19273
		public float maxFriction = 1f;

		// Token: 0x04004B4A RID: 19274
		public float maxFrictionSpeed = 15f;
	}
}
