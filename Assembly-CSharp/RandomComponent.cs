using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020007DB RID: 2011
public abstract class RandomComponent<T> : MonoBehaviour
{
	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x060031D7 RID: 12759 RVA: 0x000EFF60 File Offset: 0x000EE160
	public T lastItem
	{
		get
		{
			return this._lastItem;
		}
	}

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x060031D8 RID: 12760 RVA: 0x000EFF68 File Offset: 0x000EE168
	public int lastItemIndex
	{
		get
		{
			return this._lastItemIndex;
		}
	}

	// Token: 0x060031D9 RID: 12761 RVA: 0x000EFF70 File Offset: 0x000EE170
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

	// Token: 0x060031DA RID: 12762 RVA: 0x000EFFD0 File Offset: 0x000EE1D0
	public void Reset()
	{
		this.ResetRandom(null);
		this._lastItem = default(T);
		this._lastItemIndex = -1;
	}

	// Token: 0x060031DB RID: 12763 RVA: 0x000EFFFF File Offset: 0x000EE1FF
	private void Awake()
	{
		this.Reset();
	}

	// Token: 0x060031DC RID: 12764 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnNextItem(T item)
	{
	}

	// Token: 0x060031DD RID: 12765 RVA: 0x000F0007 File Offset: 0x000EE207
	public virtual T GetItem(int index)
	{
		return this.items[index];
	}

	// Token: 0x060031DE RID: 12766 RVA: 0x000F0018 File Offset: 0x000EE218
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

	// Token: 0x0400356C RID: 13676
	public T[] items = new T[0];

	// Token: 0x0400356D RID: 13677
	public int seed;

	// Token: 0x0400356E RID: 13678
	public bool staticSeed;

	// Token: 0x0400356F RID: 13679
	public bool distinct = true;

	// Token: 0x04003570 RID: 13680
	[Space]
	[NonSerialized]
	private int _seed;

	// Token: 0x04003571 RID: 13681
	[NonSerialized]
	private T _lastItem;

	// Token: 0x04003572 RID: 13682
	[NonSerialized]
	private int _lastItemIndex = -1;

	// Token: 0x04003573 RID: 13683
	[NonSerialized]
	private SRand _rnd;

	// Token: 0x04003574 RID: 13684
	public UnityEvent<T> onNextItem;
}
