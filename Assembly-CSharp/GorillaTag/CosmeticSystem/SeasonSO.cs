using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BFD RID: 3069
	[CreateAssetMenu(fileName = "UntitledSeason_SeasonSO", menuName = "- Gorilla Tag/SeasonSO", order = 0)]
	public class SeasonSO : ScriptableObject
	{
		// Token: 0x04004F1E RID: 20254
		[Delayed]
		public GTDateTimeSerializable releaseDate = new GTDateTimeSerializable(1);

		// Token: 0x04004F1F RID: 20255
		[Delayed]
		public string seasonName;
	}
}
