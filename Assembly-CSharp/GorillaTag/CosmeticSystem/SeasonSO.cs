using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000C2B RID: 3115
	[CreateAssetMenu(fileName = "UntitledSeason_SeasonSO", menuName = "- Gorilla Tag/SeasonSO", order = 0)]
	public class SeasonSO : ScriptableObject
	{
		// Token: 0x04005014 RID: 20500
		[Delayed]
		public GTDateTimeSerializable releaseDate = new GTDateTimeSerializable(1);

		// Token: 0x04005015 RID: 20501
		[Delayed]
		public string seasonName;
	}
}
