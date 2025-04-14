using System;
using System.Collections.Generic;
using UnityEngine;

namespace Critters.Scripts
{
	// Token: 0x02000C7F RID: 3199
	public class CrittersSpawningData : MonoBehaviour
	{
		// Token: 0x060050BD RID: 20669 RVA: 0x001888EC File Offset: 0x00186AEC
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

		// Token: 0x060050BE RID: 20670 RVA: 0x00188938 File Offset: 0x00186B38
		public int GetRandomTemplate()
		{
			int index = Random.Range(0, this.templateCollection.Count - 1);
			return this.templateCollection[index];
		}

		// Token: 0x04005335 RID: 21301
		public List<CrittersSpawningData.CreatureSpawnParameters> SpawnParametersList;

		// Token: 0x04005336 RID: 21302
		private List<int> templateCollection = new List<int>();

		// Token: 0x02000C80 RID: 3200
		[Serializable]
		public class CreatureSpawnParameters
		{
			// Token: 0x04005337 RID: 21303
			public CritterTemplate Template;

			// Token: 0x04005338 RID: 21304
			public int ChancesToSpawn;

			// Token: 0x04005339 RID: 21305
			[HideInInspector]
			[NonSerialized]
			public int StartingIndex;
		}
	}
}
