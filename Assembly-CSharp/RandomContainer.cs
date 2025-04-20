using System;
using UnityEngine;

// Token: 0x020007F6 RID: 2038
public abstract class RandomContainer<T> : ScriptableObject
{
	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x06003296 RID: 12950 RVA: 0x00051736 File Offset: 0x0004F936
	public T lastItem
	{
		get
		{
			return this._lastItem;
		}
	}

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06003297 RID: 12951 RVA: 0x0005173E File Offset: 0x0004F93E
	public int lastItemIndex
	{
		get
		{
			return this._lastItemIndex;
		}
	}

	// Token: 0x06003298 RID: 12952 RVA: 0x00138578 File Offset: 0x00136778
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

	// Token: 0x06003299 RID: 12953 RVA: 0x001385D8 File Offset: 0x001367D8
	public void Reset()
	{
		this.ResetRandom(null);
		this._lastItem = default(T);
		this._lastItemIndex = -1;
	}

	// Token: 0x0600329A RID: 12954 RVA: 0x00051746 File Offset: 0x0004F946
	private void Awake()
	{
		this.Reset();
	}

	// Token: 0x0600329B RID: 12955 RVA: 0x0005174E File Offset: 0x0004F94E
	public virtual T GetItem(int index)
	{
		return this.items[index];
	}

	// Token: 0x0600329C RID: 12956 RVA: 0x00138608 File Offset: 0x00136808
	public virtual T NextItem()
	{
		this._lastItemIndex = (this.distinct ? this._rnd.NextIntWithExclusion(0, this.items.Length, this._lastItemIndex) : this._rnd.NextInt(0, this.items.Length));
		T t = this.items[this._lastItemIndex];
		this._lastItem = t;
		return t;
	}

	// Token: 0x0400362B RID: 13867
	public T[] items = new T[0];

	// Token: 0x0400362C RID: 13868
	public int seed;

	// Token: 0x0400362D RID: 13869
	public bool staticSeed;

	// Token: 0x0400362E RID: 13870
	public bool distinct = true;

	// Token: 0x0400362F RID: 13871
	[Space]
	[NonSerialized]
	private int _seed;

	// Token: 0x04003630 RID: 13872
	[NonSerialized]
	private T _lastItem;

	// Token: 0x04003631 RID: 13873
	[NonSerialized]
	private int _lastItemIndex = -1;

	// Token: 0x04003632 RID: 13874
	[NonSerialized]
	private SRand _rnd;
}
