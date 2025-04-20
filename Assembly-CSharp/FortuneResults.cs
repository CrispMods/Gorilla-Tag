using System;
using UnityEngine;

// Token: 0x02000532 RID: 1330
public class FortuneResults : ScriptableObject
{
	// Token: 0x06002046 RID: 8262 RVA: 0x000F2784 File Offset: 0x000F0984
	private void OnValidate()
	{
		this.totalChance = 0f;
		for (int i = 0; i < this.fortuneResults.Length; i++)
		{
			this.totalChance += this.fortuneResults[i].weightedChance;
		}
	}

	// Token: 0x06002047 RID: 8263 RVA: 0x000F27D0 File Offset: 0x000F09D0
	public FortuneResults.FortuneResult GetResult()
	{
		float num = UnityEngine.Random.Range(0f, this.totalChance);
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
				int resultIndex = UnityEngine.Random.Range(0, fortuneCategory.textResults.Length);
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

	// Token: 0x06002048 RID: 8264 RVA: 0x000F2854 File Offset: 0x000F0A54
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

	// Token: 0x04002448 RID: 9288
	[SerializeField]
	private FortuneResults.FortuneCategory[] fortuneResults;

	// Token: 0x04002449 RID: 9289
	[SerializeField]
	private float totalChance;

	// Token: 0x02000533 RID: 1331
	public enum FortuneCategoryType
	{
		// Token: 0x0400244B RID: 9291
		Invalid,
		// Token: 0x0400244C RID: 9292
		Positive,
		// Token: 0x0400244D RID: 9293
		Neutral,
		// Token: 0x0400244E RID: 9294
		Negative,
		// Token: 0x0400244F RID: 9295
		Seasonal
	}

	// Token: 0x02000534 RID: 1332
	[Serializable]
	public struct FortuneCategory
	{
		// Token: 0x04002450 RID: 9296
		public FortuneResults.FortuneCategoryType fortuneType;

		// Token: 0x04002451 RID: 9297
		public float weightedChance;

		// Token: 0x04002452 RID: 9298
		public string[] textResults;
	}

	// Token: 0x02000535 RID: 1333
	public struct FortuneResult
	{
		// Token: 0x0600204A RID: 8266 RVA: 0x00045F51 File Offset: 0x00044151
		public FortuneResult(FortuneResults.FortuneCategoryType fortuneType, int resultIndex)
		{
			this.fortuneType = fortuneType;
			this.resultIndex = resultIndex;
		}

		// Token: 0x04002453 RID: 9299
		public FortuneResults.FortuneCategoryType fortuneType;

		// Token: 0x04002454 RID: 9300
		public int resultIndex;
	}
}
