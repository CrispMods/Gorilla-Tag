using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B8F RID: 2959
	[CreateAssetMenu(fileName = "MaterialDatasSO", menuName = "Gorilla Tag/MaterialDatasSO")]
	public class MaterialDatasSO : ScriptableObject
	{
		// Token: 0x04004C2A RID: 19498
		public List<GTPlayer.MaterialData> datas;

		// Token: 0x04004C2B RID: 19499
		public List<HashWrapper> surfaceEffects;
	}
}
