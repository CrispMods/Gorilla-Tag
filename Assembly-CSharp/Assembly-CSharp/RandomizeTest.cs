using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200062C RID: 1580
public class RandomizeTest : MonoBehaviour
{
	// Token: 0x06002755 RID: 10069 RVA: 0x000C13D0 File Offset: 0x000BF5D0
	private void Start()
	{
		for (int i = 0; i < 10; i++)
		{
			this.testList.Add(i);
		}
		for (int j = 0; j < 10; j++)
		{
			this.testListArray[j] = 0;
		}
		for (int k = 0; k < this.testList.Count; k++)
		{
			this.testListArray[k] = this.testList[k];
		}
		this.RandomizeList(ref this.testList);
		for (int l = 0; l < 10; l++)
		{
			this.testListArray[l] = 0;
		}
		for (int m = 0; m < this.testList.Count; m++)
		{
			this.testListArray[m] = this.testList[m];
		}
	}

	// Token: 0x06002756 RID: 10070 RVA: 0x000C1488 File Offset: 0x000BF688
	public void RandomizeList(ref List<int> listToRandomize)
	{
		this.randomIterator = 0;
		while (this.randomIterator < listToRandomize.Count)
		{
			this.tempRandIndex = Random.Range(this.randomIterator, listToRandomize.Count);
			this.tempRandValue = listToRandomize[this.randomIterator];
			listToRandomize[this.randomIterator] = listToRandomize[this.tempRandIndex];
			listToRandomize[this.tempRandIndex] = this.tempRandValue;
			this.randomIterator++;
		}
	}

	// Token: 0x04002B18 RID: 11032
	public List<int> testList = new List<int>();

	// Token: 0x04002B19 RID: 11033
	public int[] testListArray = new int[10];

	// Token: 0x04002B1A RID: 11034
	public int randomIterator;

	// Token: 0x04002B1B RID: 11035
	public int tempRandIndex;

	// Token: 0x04002B1C RID: 11036
	public int tempRandValue;
}
