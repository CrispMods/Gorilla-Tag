using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B92 RID: 2962
	[CreateAssetMenu(fileName = "MaterialDatasSO", menuName = "Gorilla Tag/MaterialDatasSO")]
	public class MaterialDatasSO : ScriptableObject
	{
		// Token: 0x04004C3C RID: 19516
		public List<GTPlayer.MaterialData> datas;

		// Token: 0x04004C3D RID: 19517
		public List<HashWrapper> surfaceEffects;
	}
}
