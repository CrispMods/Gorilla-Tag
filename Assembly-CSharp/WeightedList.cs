using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class WeightedList<T>
{
	// Token: 0x1700002F RID: 47
	// (get) Token: 0x0600032A RID: 810 RVA: 0x00032734 File Offset: 0x00030934
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x0600032B RID: 811 RVA: 0x00032741 File Offset: 0x00030941
	public List<T> Items
	{
		get
		{
			return this.items;
		}
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00076E3C File Offset: 0x0007503C
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

	// Token: 0x17000031 RID: 49
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

	// Token: 0x0600032E RID: 814 RVA: 0x00032780 File Offset: 0x00030980
	public T GetRandomItem()
	{
		return this.items[this.GetRandomIndex()];
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00076E94 File Offset: 0x00075094
	public int GetRandomIndex()
	{
		if (this.items.Count == 0)
		{
			throw new InvalidOperationException("The list is empty.");
		}
		float item = UnityEngine.Random.value * this.totalWeight;
		int num = this.cumulativeWeights.BinarySearch(item);
		if (num < 0)
		{
			num = ~num;
		}
		return num;
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00076EDC File Offset: 0x000750DC
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

	// Token: 0x06000331 RID: 817 RVA: 0x00076F04 File Offset: 0x00075104
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

	// Token: 0x06000332 RID: 818 RVA: 0x00076F68 File Offset: 0x00075168
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

	// Token: 0x06000333 RID: 819 RVA: 0x00032793 File Offset: 0x00030993
	public void Clear()
	{
		this.items.Clear();
		this.weights.Clear();
		this.cumulativeWeights.Clear();
		this.totalWeight = 0f;
	}

	// Token: 0x040003D0 RID: 976
	private List<T> items = new List<T>();

	// Token: 0x040003D1 RID: 977
	private List<float> weights = new List<float>();

	// Token: 0x040003D2 RID: 978
	private List<float> cumulativeWeights = new List<float>();

	// Token: 0x040003D3 RID: 979
	private float totalWeight;
}
