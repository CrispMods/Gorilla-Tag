using System;
using System.Collections.Generic;
using UnityEngine;

namespace Critters.Scripts
{
	// Token: 0x02000C7C RID: 3196
	public class CrittersSpawningData : MonoBehaviour
	{
		// Token: 0x060050B1 RID: 20657 RVA: 0x00188324 File Offset: 0x00186524
		public void InitializeSpawnCollection()
		{
			for (int i = 0; i < this.SpawnParametersList.Count; i++)
			{
				for (int j = 0; j < this.SpawnParametersList[i].ChancesToSpawn; j++)
				{
					this.templateCollection.Add(i);
				}
			}
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x00188370 File Offset: 0x00186570
		public int GetRandomTemplate()
		{
			int index = Random.Range(0, this.templateCollection.Count - 1);
			return this.templateCollection[index];
		}

		// Token: 0x04005323 RID: 21283
		public List<CrittersSpawningData.CreatureSpawnParameters> SpawnParametersList;

		// Token: 0x04005324 RID: 21284
		private List<int> templateCollection = new List<int>();

		// Token: 0x02000C7D RID: 3197
		[Serializable]
		public class CreatureSpawnParameters
		{
			// Token: 0x04005325 RID: 21285
			public CritterTemplate Template;

			// Token: 0x04005326 RID: 21286
			public int ChancesToSpawn;

			// Token: 0x04005327 RID: 21287
			[HideInInspector]
			[NonSerialized]
			public int StartingIndex;
		}
	}
}
