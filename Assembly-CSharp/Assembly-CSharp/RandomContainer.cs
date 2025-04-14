using System;
using UnityEngine;

// Token: 0x020007DF RID: 2015
public abstract class RandomContainer<T> : ScriptableObject
{
	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x060031EC RID: 12780 RVA: 0x000F0680 File Offset: 0x000EE880
	public T lastItem
	{
		get
		{
			return this._lastItem;
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x060031ED RID: 12781 RVA: 0x000F0688 File Offset: 0x000EE888
	public int lastItemIndex
	{
		get
		{
			return this._lastItemIndex;
		}
	}

	// Token: 0x060031EE RID: 12782 RVA: 0x000F0690 File Offset: 0x000EE890
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

	// Token: 0x060031EF RID: 12783 RVA: 0x000F06F0 File Offset: 0x000EE8F0
	public void Reset()
	{
		this.ResetRandom(null);
		this._lastItem = default(T);
		this._lastItemIndex = -1;
	}

	// Token: 0x060031F0 RID: 12784 RVA: 0x000F071F File Offset: 0x000EE91F
	private void Awake()
	{
		this.Reset();
	}

	// Token: 0x060031F1 RID: 12785 RVA: 0x000F0727 File Offset: 0x000EE927
	public virtual T GetItem(int index)
	{
		return this.items[index];
	}

	// Token: 0x060031F2 RID: 12786 RVA: 0x000F0738 File Offset: 0x000EE938
	public virtual T NextItem()
	{
		this._lastItemIndex = (this.distinct ? this._rnd.NextIntWithExclusion(0, this.items.Length, this._lastItemIndex) : this._rnd.NextInt(0, this.items.Length));
		T t = this.items[this._lastItemIndex];
		this._lastItem = t;
		return t;
	}

	// Token: 0x04003587 RID: 13703
	public T[] items = new T[0];

	// Token: 0x04003588 RID: 13704
	public int seed;

	// Token: 0x04003589 RID: 13705
	public bool staticSeed;

	// Token: 0x0400358A RID: 13706
	public bool distinct = true;

	// Token: 0x0400358B RID: 13707
	[Space]
	[NonSerialized]
	private int _seed;

	// Token: 0x0400358C RID: 13708
	[NonSerialized]
	private T _lastItem;

	// Token: 0x0400358D RID: 13709
	[NonSerialized]
	private int _lastItemIndex = -1;

	// Token: 0x0400358E RID: 13710
	[NonSerialized]
	private SRand _rnd;
}
