using System;
using UnityEngine;

// Token: 0x020007DC RID: 2012
public abstract class RandomContainer<T> : ScriptableObject
{
	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x060031E0 RID: 12768 RVA: 0x000F00B8 File Offset: 0x000EE2B8
	public T lastItem
	{
		get
		{
			return this._lastItem;
		}
	}

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x060031E1 RID: 12769 RVA: 0x000F00C0 File Offset: 0x000EE2C0
	public int lastItemIndex
	{
		get
		{
			return this._lastItemIndex;
		}
	}

	// Token: 0x060031E2 RID: 12770 RVA: 0x000F00C8 File Offset: 0x000EE2C8
	public void ResetRandom(int? seedValue = null)
	{
		if (!this.staticSeed)
		{
			this._seed = (seedValue ?? StaticHash.Compute(DateTime.UtcNow.Ticks));
		}
		else
		{
			this._seed = this.seed;
		}
		this._rnd = new SRand(this._seed);
	}

	// Token: 0x060031E3 RID: 12771 RVA: 0x000F0128 File Offset: 0x000EE328
	public void Reset()
	{
		this.ResetRandom(null);
		this._lastItem = default(T);
		this._lastItemIndex = -1;
	}

	// Token: 0x060031E4 RID: 12772 RVA: 0x000F0157 File Offset: 0x000EE357
	private void Awake()
	{
		this.Reset();
	}

	// Token: 0x060031E5 RID: 12773 RVA: 0x000F015F File Offset: 0x000EE35F
	public virtual T GetItem(int index)
	{
		return this.items[index];
	}

	// Token: 0x060031E6 RID: 12774 RVA: 0x000F0170 File Offset: 0x000EE370
	public virtual T NextItem()
	{
		this._lastItemIndex = (this.distinct ? this._rnd.NextIntWithExclusion(0, this.items.Length, this._lastItemIndex) : this._rnd.NextInt(0, this.items.Length));
		T t = this.items[this._lastItemIndex];
		this._lastItem = t;
		return t;
	}

	// Token: 0x04003575 RID: 13685
	public T[] items = new T[0];

	// Token: 0x04003576 RID: 13686
	public int seed;

	// Token: 0x04003577 RID: 13687
	public bool staticSeed;

	// Token: 0x04003578 RID: 13688
	public bool distinct = true;

	// Token: 0x04003579 RID: 13689
	[Space]
	[NonSerialized]
	private int _seed;

	// Token: 0x0400357A RID: 13690
	[NonSerialized]
	private T _lastItem;

	// Token: 0x0400357B RID: 13691
	[NonSerialized]
	private int _lastItemIndex = -1;

	// Token: 0x0400357C RID: 13692
	[NonSerialized]
	private SRand _rnd;
}
