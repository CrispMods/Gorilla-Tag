using System;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class RockPiles : MonoBehaviour
{
	// Token: 0x06000764 RID: 1892 RVA: 0x0008A738 File Offset: 0x00088938
	public void Show(int visiblePercentage)
	{
		if (visiblePercentage <= 0)
		{
			this.ShowRock(-1);
			return;
		}
		int rockToShow = -1;
		int num = -1;
		for (int i = 0; i < this._rocks.Length; i++)
		{
			RockPiles.RockPile rockPile = this._rocks[i];
			if (visiblePercentage >= rockPile.threshold && num < rockPile.threshold)
			{
				rockToShow = i;
				num = rockPile.threshold;
			}
		}
		this.ShowRock(rockToShow);
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x0008A798 File Offset: 0x00088998
	private void ShowRock(int rockToShow)
	{
		for (int i = 0; i < this._rocks.Length; i++)
		{
			this._rocks[i].visual.SetActive(i == rockToShow);
		}
	}

	// Token: 0x040008BF RID: 2239
	[SerializeField]
	private RockPiles.RockPile[] _rocks;

	// Token: 0x02000114 RID: 276
	[Serializable]
	public struct RockPile
	{
		// Token: 0x040008C0 RID: 2240
		public GameObject visual;

		// Token: 0x040008C1 RID: 2241
		public int threshold;
	}
}
