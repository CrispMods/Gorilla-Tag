using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200062B RID: 1579
public class RandomizeTest : MonoBehaviour
{
	// Token: 0x0600274D RID: 10061 RVA: 0x000C0F50 File Offset: 0x000BF150
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

	// Token: 0x0600274E RID: 10062 RVA: 0x000C1008 File Offset: 0x000BF208
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

	// Token: 0x04002B12 RID: 11026
	public List<int> testList = new List<int>();

	// Token: 0x04002B13 RID: 11027
	public int[] testListArray = new int[10];

	// Token: 0x04002B14 RID: 11028
	public int randomIterator;

	// Token: 0x04002B15 RID: 11029
	public int tempRandIndex;

	// Token: 0x04002B16 RID: 11030
	public int tempRandValue;
}
