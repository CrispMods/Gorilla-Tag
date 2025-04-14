using System;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class RockPiles : MonoBehaviour
{
	// Token: 0x06000723 RID: 1827 RVA: 0x00028A34 File Offset: 0x00026C34
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

	// Token: 0x06000724 RID: 1828 RVA: 0x00028A94 File Offset: 0x00026C94
	private void ShowRock(int rockToShow)
	{
		for (int i = 0; i < this._rocks.Length; i++)
		{
			this._rocks[i].visual.SetActive(i == rockToShow);
		}
	}

	// Token: 0x0400087E RID: 2174
	[SerializeField]
	private RockPiles.RockPile[] _rocks;

	// Token: 0x0200010A RID: 266
	[Serializable]
	public struct RockPile
	{
		// Token: 0x0400087F RID: 2175
		public GameObject visual;

		// Token: 0x04000880 RID: 2176
		public int threshold;
	}
}
