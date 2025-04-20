using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009A2 RID: 2466
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WhackAMoleLevelSetting", order = 1)]
	public class WhackAMoleLevelSO : ScriptableObject
	{
		// Token: 0x06003C71 RID: 15473 RVA: 0x00057742 File Offset: 0x00055942
		public int GetMinScore(bool isCoop)
		{
			if (!isCoop)
			{
				return this.minScore;
			}
			return this.minScore * 2;
		}

		// Token: 0x04003D64 RID: 15716
		public int levelNumber;

		// Token: 0x04003D65 RID: 15717
		public float levelDuration;

		// Token: 0x04003D66 RID: 15718
		[Tooltip("For how long do the moles stay visible?")]
		public float showMoleDuration;

		// Token: 0x04003D67 RID: 15719
		[Tooltip("How fast we pick a random new mole?")]
		public float pickNextMoleTime;

		// Token: 0x04003D68 RID: 15720
		[Tooltip("Minimum score to get in order to be able to proceed to the next level")]
		[SerializeField]
		private int minScore;

		// Token: 0x04003D69 RID: 15721
		[Tooltip("Chance of each mole being a hazard mole at the start, and end, of the level.")]
		public Vector2 hazardMoleChance = new Vector2(0f, 0.5f);

		// Token: 0x04003D6A RID: 15722
		[Tooltip("Minimum number of moles selected as level progresses.")]
		public Vector2 minimumMoleCount = new Vector2(1f, 2f);

		// Token: 0x04003D6B RID: 15723
		[Tooltip("Minimum number of moles selected as level progresses.")]
		public Vector2 maximumMoleCount = new Vector2(1.5f, 3f);
	}
}
