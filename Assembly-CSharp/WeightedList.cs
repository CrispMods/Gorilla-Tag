using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class WeightedList<T>
{
	// Token: 0x1700002C RID: 44
	// (get) Token: 0x060002F9 RID: 761 RVA: 0x000126B4 File Offset: 0x000108B4
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x060002FA RID: 762 RVA: 0x000126C4 File Offset: 0x000108C4
	public void Add(T item, float weight)
	{
		if (weight <= 0f)
		{
			throw new ArgumentException("Weight must be greater than zero.");
		}
		this.totalWeight += weight;
		this.items.Add(item);
		this.weights.Add(weight);
		this.cumulativeWeights.Add(this.totalWeight);
	}

	// Token: 0x1700002D RID: 45
	[TupleElementNames(new string[]
	{
		"Item",
		"Weight"
	})]
	public ValueTuple<T, float> this[int index]
	{
		[return: TupleElementNames(new string[]
		{
			"Item",
			"Weight"
		})]
		get
		{
			if (index < 0 || index >= this.items.Count)
			{
				throw new IndexOutOfRangeException();
			}
			return new ValueTuple<T, float>(this.items[index], this.weights[index]);
		}
	}

	// Token: 0x060002FC RID: 764 RVA: 0x00012752 File Offset: 0x00010952
	public T GetRandomItem()
	{
		return this.items[this.GetRandomIndex()];
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00012768 File Offset: 0x00010968
	public int GetRandomIndex()
	{
		if (this.items.Count == 0)
		{
			throw new InvalidOperationException("The list is empty.");
		}
		float item = Random.value * this.totalWeight;
		int num = this.cumulativeWeights.BinarySearch(item);
		if (num < 0)
		{
			num = ~num;
		}
		return num;
	}

	// Token: 0x060002FE RID: 766 RVA: 0x000127B0 File Offset: 0x000109B0
	public bool Remove(T item)
	{
		int num = this.items.IndexOf(item);
		if (num == -1)
		{
			return false;
		}
		this.RemoveAt(num);
		return true;
	}

	// Token: 0x060002FF RID: 767 RVA: 0x000127D8 File Offset: 0x000109D8
	public void RemoveAt(int index)
	{
		if (index < 0 || index >= this.items.Count)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		this.totalWeight -= this.weights[index];
		this.items.RemoveAt(index);
		this.weights.RemoveAt(index);
		this.RecalculateCumulativeWeights();
	}

	// Token: 0x06000300 RID: 768 RVA: 0x0001283C File Offset: 0x00010A3C
	private void RecalculateCumulativeWeights()
	{
		this.cumulativeWeights.Clear();
		float num = 0f;
		foreach (float num2 in this.weights)
		{
			num += num2;
			this.cumulativeWeights.Add(num);
		}
		this.totalWeight = num;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x000128B0 File Offset: 0x00010AB0
	public void Clear()
	{
		this.items.Clear();
		this.weights.Clear();
		this.cumulativeWeights.Clear();
		this.totalWeight = 0f;
	}

	// Token: 0x0400039C RID: 924
	private List<T> items = new List<T>();

	// Token: 0x0400039D RID: 925
	private List<float> weights = new List<float>();

	// Token: 0x0400039E RID: 926
	private List<float> cumulativeWeights = new List<float>();

	// Token: 0x0400039F RID: 927
	private float totalWeight;
}
