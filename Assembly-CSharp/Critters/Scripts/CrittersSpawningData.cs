using System;
using System.Collections.Generic;
using UnityEngine;

namespace Critters.Scripts
{
	// Token: 0x02000CAD RID: 3245
	public class CrittersSpawningData : MonoBehaviour
	{
		// Token: 0x06005213 RID: 21011 RVA: 0x001BF494 File Offset: 0x001BD694
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

		// Token: 0x06005214 RID: 21012 RVA: 0x001BF4E0 File Offset: 0x001BD6E0
		public int GetRandomTemplate()
		{
			int index = UnityEngine.Random.Range(0, this.templateCollection.Count - 1);
			return this.templateCollection[index];
		}

		// Token: 0x0400542F RID: 21551
		public List<CrittersSpawningData.CreatureSpawnParameters> SpawnParametersList;

		// Token: 0x04005430 RID: 21552
		private List<int> templateCollection = new List<int>();

		// Token: 0x02000CAE RID: 3246
		[Serializable]
		public class CreatureSpawnParameters
		{
			// Token: 0x04005431 RID: 21553
			public CritterTemplate Template;

			// Token: 0x04005432 RID: 21554
			public int ChancesToSpawn;

			// Token: 0x04005433 RID: 21555
			[HideInInspector]
			[NonSerialized]
			public int StartingIndex;
		}
	}
}
