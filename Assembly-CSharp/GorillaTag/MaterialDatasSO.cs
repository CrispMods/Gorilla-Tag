using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BBC RID: 3004
	[CreateAssetMenu(fileName = "MaterialDatasSO", menuName = "Gorilla Tag/MaterialDatasSO")]
	public class MaterialDatasSO : ScriptableObject
	{
		// Token: 0x04004D20 RID: 19744
		public List<GTPlayer.MaterialData> datas;

		// Token: 0x04004D21 RID: 19745
		public List<HashWrapper> surfaceEffects;
	}
}
