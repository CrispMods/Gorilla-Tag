using System;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B85 RID: 2949
	[CreateAssetMenu(fileName = "GorillaZiplineSettings", menuName = "ScriptableObjects/GorillaZiplineSettings", order = 0)]
	public class GorillaZiplineSettings : ScriptableObject
	{
		// Token: 0x04004C38 RID: 19512
		public float minSlidePitch = 0.5f;

		// Token: 0x04004C39 RID: 19513
		public float maxSlidePitch = 1f;

		// Token: 0x04004C3A RID: 19514
		public float minSlideVolume;

		// Token: 0x04004C3B RID: 19515
		public float maxSlideVolume = 0.2f;

		// Token: 0x04004C3C RID: 19516
		public float maxSpeed = 10f;

		// Token: 0x04004C3D RID: 19517
		public float gravityMulti = 1.1f;

		// Token: 0x04004C3E RID: 19518
		[Header("Friction")]
		public float friction = 0.25f;

		// Token: 0x04004C3F RID: 19519
		public float maxFriction = 1f;

		// Token: 0x04004C40 RID: 19520
		public float maxFrictionSpeed = 15f;
	}
}
