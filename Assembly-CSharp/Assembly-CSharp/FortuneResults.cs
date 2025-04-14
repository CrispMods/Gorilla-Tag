using System;
using UnityEngine;

// Token: 0x02000525 RID: 1317
public class FortuneResults : ScriptableObject
{
	// Token: 0x06001FF0 RID: 8176 RVA: 0x000A14D0 File Offset: 0x0009F6D0
	private void OnValidate()
	{
		this.totalChance = 0f;
		for (int i = 0; i < this.fortuneResults.Length; i++)
		{
			this.totalChance += this.fortuneResults[i].weightedChance;
		}
	}

	// Token: 0x06001FF1 RID: 8177 RVA: 0x000A151C File Offset: 0x0009F71C
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

	// Token: 0x06001FF2 RID: 8178 RVA: 0x000A15A0 File Offset: 0x0009F7A0
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

	// Token: 0x040023F6 RID: 9206
	[SerializeField]
	private FortuneResults.FortuneCategory[] fortuneResults;

	// Token: 0x040023F7 RID: 9207
	[SerializeField]
	private float totalChance;

	// Token: 0x02000526 RID: 1318
	public enum FortuneCategoryType
	{
		// Token: 0x040023F9 RID: 9209
		Invalid,
		// Token: 0x040023FA RID: 9210
		Positive,
		// Token: 0x040023FB RID: 9211
		Neutral,
		// Token: 0x040023FC RID: 9212
		Negative,
		// Token: 0x040023FD RID: 9213
		Seasonal
	}

	// Token: 0x02000527 RID: 1319
	[Serializable]
	public struct FortuneCategory
	{
		// Token: 0x040023FE RID: 9214
		public FortuneResults.FortuneCategoryType fortuneType;

		// Token: 0x040023FF RID: 9215
		public float weightedChance;

		// Token: 0x04002400 RID: 9216
		public string[] textResults;
	}

	// Token: 0x02000528 RID: 1320
	public struct FortuneResult
	{
		// Token: 0x06001FF4 RID: 8180 RVA: 0x000A161B File Offset: 0x0009F81B
		public FortuneResult(FortuneResults.FortuneCategoryType fortuneType, int resultIndex)
		{
			this.fortuneType = fortuneType;
			this.resultIndex = resultIndex;
		}

		// Token: 0x04002401 RID: 9217
		public FortuneResults.FortuneCategoryType fortuneType;

		// Token: 0x04002402 RID: 9218
		public int resultIndex;
	}
}
