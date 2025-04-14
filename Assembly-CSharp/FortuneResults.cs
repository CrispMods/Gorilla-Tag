using System;
using UnityEngine;

// Token: 0x02000525 RID: 1317
public class FortuneResults : ScriptableObject
{
	// Token: 0x06001FED RID: 8173 RVA: 0x000A114C File Offset: 0x0009F34C
	private void OnValidate()
	{
		this.totalChance = 0f;
		for (int i = 0; i < this.fortuneResults.Length; i++)
		{
			this.totalChance += this.fortuneResults[i].weightedChance;
		}
	}

	// Token: 0x06001FEE RID: 8174 RVA: 0x000A1198 File Offset: 0x0009F398
	public FortuneResults.FortuneResult GetResult()
	{
		float num = Random.Range(0f, this.totalChance);
		int i = 0;
		while (i < this.fortuneResults.Length)
		{
			FortuneResults.FortuneCategory fortuneCategory = this.fortuneResults[i];
			if (num <= fortuneCategory.weightedChance)
			{
				if (fortuneCategory.textResults.Length == 0)
				{
					return new FortuneResults.FortuneResult(FortuneResults.FortuneCategoryType.Invalid, -1);
				}
				int resultIndex = Random.Range(0, fortuneCategory.textResults.Length);
				return new FortuneResults.FortuneResult(fortuneCategory.fortuneType, resultIndex);
			}
			else
			{
				num -= fortuneCategory.weightedChance;
				i++;
			}
		}
		return new FortuneResults.FortuneResult(FortuneResults.FortuneCategoryType.Invalid, -1);
	}

	// Token: 0x06001FEF RID: 8175 RVA: 0x000A121C File Offset: 0x0009F41C
	public string GetResultText(FortuneResults.FortuneResult result)
	{
		for (int i = 0; i < this.fortuneResults.Length; i++)
		{
			if (this.fortuneResults[i].fortuneType == result.fortuneType && result.resultIndex >= 0 && result.resultIndex < this.fortuneResults[i].textResults.Length)
			{
				return this.fortuneResults[i].textResults[result.resultIndex];
			}
		}
		return "!! Invalid Fortune !!";
	}

	// Token: 0x040023F5 RID: 9205
	[SerializeField]
	private FortuneResults.FortuneCategory[] fortuneResults;

	// Token: 0x040023F6 RID: 9206
	[SerializeField]
	private float totalChance;

	// Token: 0x02000526 RID: 1318
	public enum FortuneCategoryType
	{
		// Token: 0x040023F8 RID: 9208
		Invalid,
		// Token: 0x040023F9 RID: 9209
		Positive,
		// Token: 0x040023FA RID: 9210
		Neutral,
		// Token: 0x040023FB RID: 9211
		Negative,
		// Token: 0x040023FC RID: 9212
		Seasonal
	}

	// Token: 0x02000527 RID: 1319
	[Serializable]
	public struct FortuneCategory
	{
		// Token: 0x040023FD RID: 9213
		public FortuneResults.FortuneCategoryType fortuneType;

		// Token: 0x040023FE RID: 9214
		public float weightedChance;

		// Token: 0x040023FF RID: 9215
		public string[] textResults;
	}

	// Token: 0x02000528 RID: 1320
	public struct FortuneResult
	{
		// Token: 0x06001FF1 RID: 8177 RVA: 0x000A1297 File Offset: 0x0009F497
		public FortuneResult(FortuneResults.FortuneCategoryType fortuneType, int resultIndex)
		{
			this.fortuneType = fortuneType;
			this.resultIndex = resultIndex;
		}

		// Token: 0x04002400 RID: 9216
		public FortuneResults.FortuneCategoryType fortuneType;

		// Token: 0x04002401 RID: 9217
		public int resultIndex;
	}
}
