using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200060A RID: 1546
public class RandomizeTest : MonoBehaviour
{
	// Token: 0x06002678 RID: 9848 RVA: 0x00108A3C File Offset: 0x00106C3C
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

	// Token: 0x06002679 RID: 9849 RVA: 0x00108AF4 File Offset: 0x00106CF4
	public void RandomizeList(ref List<int> listToRandomize)
	{
		this.randomIterator = 0;
		while (this.randomIterator < listToRandomize.Count)
		{
			this.tempRandIndex = UnityEngine.Random.Range(this.randomIterator, listToRandomize.Count);
			this.tempRandValue = listToRandomize[this.randomIterator];
			listToRandomize[this.randomIterator] = listToRandomize[this.tempRandIndex];
			listToRandomize[this.tempRandIndex] = this.tempRandValue;
			this.randomIterator++;
		}
	}

	// Token: 0x04002A78 RID: 10872
	public List<int> testList = new List<int>();

	// Token: 0x04002A79 RID: 10873
	public int[] testListArray = new int[10];

	// Token: 0x04002A7A RID: 10874
	public int randomIterator;

	// Token: 0x04002A7B RID: 10875
	public int tempRandIndex;

	// Token: 0x04002A7C RID: 10876
	public int tempRandValue;
}
