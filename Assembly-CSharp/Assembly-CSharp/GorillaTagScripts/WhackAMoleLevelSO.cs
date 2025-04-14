using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200097F RID: 2431
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WhackAMoleLevelSetting", order = 1)]
	public class WhackAMoleLevelSO : ScriptableObject
	{
		// Token: 0x06003B65 RID: 15205 RVA: 0x00111DB4 File Offset: 0x0010FFB4
		public int GetMinScore(bool isCoop)
		{
			if (!isCoop)
			{
				return this.minScore;
			}
			return this.minScore * 2;
		}

		// Token: 0x04003C9C RID: 15516
		public int levelNumber;

		// Token: 0x04003C9D RID: 15517
		public float levelDuration;

		// Token: 0x04003C9E RID: 15518
		[Tooltip("For how long do the moles stay visible?")]
		public float showMoleDuration;

		// Token: 0x04003C9F RID: 15519
		[Tooltip("How fast we pick a random new mole?")]
		public float pickNextMoleTime;

		// Token: 0x04003CA0 RID: 15520
		[Tooltip("Minimum score to get in order to be able to proceed to the next level")]
		[SerializeField]
		private int minScore;

		// Token: 0x04003CA1 RID: 15521
		[Tooltip("Chance of each mole being a hazard mole at the start, and end, of the level.")]
		public Vector2 hazardMoleChance = new Vector2(0f, 0.5f);

		// Token: 0x04003CA2 RID: 15522
		[Tooltip("Minimum number of moles selected as level progresses.")]
		public Vector2 minimumMoleCount = new Vector2(1f, 2f);

		// Token: 0x04003CA3 RID: 15523
		[Tooltip("Minimum number of moles selected as level progresses.")]
		public Vector2 maximumMoleCount = new Vector2(1.5f, 3f);
	}
}
