using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200097C RID: 2428
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WhackAMoleLevelSetting", order = 1)]
	public class WhackAMoleLevelSO : ScriptableObject
	{
		// Token: 0x06003B59 RID: 15193 RVA: 0x001117EC File Offset: 0x0010F9EC
		public int GetMinScore(bool isCoop)
		{
			if (!isCoop)
			{
				return this.minScore;
			}
			return this.minScore * 2;
		}

		// Token: 0x04003C8A RID: 15498
		public int levelNumber;

		// Token: 0x04003C8B RID: 15499
		public float levelDuration;

		// Token: 0x04003C8C RID: 15500
		[Tooltip("For how long do the moles stay visible?")]
		public float showMoleDuration;

		// Token: 0x04003C8D RID: 15501
		[Tooltip("How fast we pick a random new mole?")]
		public float pickNextMoleTime;

		// Token: 0x04003C8E RID: 15502
		[Tooltip("Minimum score to get in order to be able to proceed to the next level")]
		[SerializeField]
		private int minScore;

		// Token: 0x04003C8F RID: 15503
		[Tooltip("Chance of each mole being a hazard mole at the start, and end, of the level.")]
		public Vector2 hazardMoleChance = new Vector2(0f, 0.5f);

		// Token: 0x04003C90 RID: 15504
		[Tooltip("Minimum number of moles selected as level progresses.")]
		public Vector2 minimumMoleCount = new Vector2(1f, 2f);

		// Token: 0x04003C91 RID: 15505
		[Tooltip("Minimum number of moles selected as level progresses.")]
		public Vector2 maximumMoleCount = new Vector2(1.5f, 3f);
	}
}
