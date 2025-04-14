using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020007DE RID: 2014
public abstract class RandomComponent<T> : MonoBehaviour
{
	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x060031E3 RID: 12771 RVA: 0x000F0525 File Offset: 0x000EE725
	public T lastItem
	{
		get
		{
			return this._lastItem;
		}
	}

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x060031E4 RID: 12772 RVA: 0x000F052D File Offset: 0x000EE72D
	public int lastItemIndex
	{
		get
		{
			return this._lastItemIndex;
		}
	}

	// Token: 0x060031E5 RID: 12773 RVA: 0x000F0538 File Offset: 0x000EE738
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

	// Token: 0x060031E6 RID: 12774 RVA: 0x000F0598 File Offset: 0x000EE798
	public void Reset()
	{
		this.ResetRandom(null);
		this._lastItem = default(T);
		this._lastItemIndex = -1;
	}

	// Token: 0x060031E7 RID: 12775 RVA: 0x000F05C7 File Offset: 0x000EE7C7
	private void Awake()
	{
		this.Reset();
	}

	// Token: 0x060031E8 RID: 12776 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnNextItem(T item)
	{
	}

	// Token: 0x060031E9 RID: 12777 RVA: 0x000F05CF File Offset: 0x000EE7CF
	public virtual T GetItem(int index)
	{
		return this.items[index];
	}

	// Token: 0x060031EA RID: 12778 RVA: 0x000F05E0 File Offset: 0x000EE7E0
	public virtual T NextItem()
	{
		this._lastItemIndex = (this.distinct ? this._rnd.NextIntWithExclusion(0, this.items.Length, this._lastItemIndex) : this._rnd.NextInt(0, this.items.Length));
		T t = this.items[this._lastItemIndex];
		this._lastItem = t;
		this.OnNextItem(t);
		UnityEvent<T> unityEvent = this.onNextItem;
		if (unityEvent != null)
		{
			unityEvent.Invoke(t);
		}
		return t;
	}

	// Token: 0x0400357E RID: 13694
	public T[] items = new T[0];

	// Token: 0x0400357F RID: 13695
	public int seed;

	// Token: 0x04003580 RID: 13696
	public bool staticSeed;

	// Token: 0x04003581 RID: 13697
	public bool distinct = true;

	// Token: 0x04003582 RID: 13698
	[Space]
	[NonSerialized]
	private int _seed;

	// Token: 0x04003583 RID: 13699
	[NonSerialized]
	private T _lastItem;

	// Token: 0x04003584 RID: 13700
	[NonSerialized]
	private int _lastItemIndex = -1;

	// Token: 0x04003585 RID: 13701
	[NonSerialized]
	private SRand _rnd;

	// Token: 0x04003586 RID: 13702
	public UnityEvent<T> onNextItem;
}
