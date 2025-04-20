using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020007F5 RID: 2037
public abstract class RandomComponent<T> : MonoBehaviour
{
	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x0600328D RID: 12941 RVA: 0x000516EE File Offset: 0x0004F8EE
	public T lastItem
	{
		get
		{
			return this._lastItem;
		}
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x0600328E RID: 12942 RVA: 0x000516F6 File Offset: 0x0004F8F6
	public int lastItemIndex
	{
		get
		{
			return this._lastItemIndex;
		}
	}

	// Token: 0x0600328F RID: 12943 RVA: 0x00138468 File Offset: 0x00136668
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

	// Token: 0x06003290 RID: 12944 RVA: 0x001384C8 File Offset: 0x001366C8
	public void Reset()
	{
		this.ResetRandom(null);
		this._lastItem = default(T);
		this._lastItemIndex = -1;
	}

	// Token: 0x06003291 RID: 12945 RVA: 0x000516FE File Offset: 0x0004F8FE
	private void Awake()
	{
		this.Reset();
	}

	// Token: 0x06003292 RID: 12946 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnNextItem(T item)
	{
	}

	// Token: 0x06003293 RID: 12947 RVA: 0x00051706 File Offset: 0x0004F906
	public virtual T GetItem(int index)
	{
		return this.items[index];
	}

	// Token: 0x06003294 RID: 12948 RVA: 0x001384F8 File Offset: 0x001366F8
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

	// Token: 0x04003622 RID: 13858
	public T[] items = new T[0];

	// Token: 0x04003623 RID: 13859
	public int seed;

	// Token: 0x04003624 RID: 13860
	public bool staticSeed;

	// Token: 0x04003625 RID: 13861
	public bool distinct = true;

	// Token: 0x04003626 RID: 13862
	[Space]
	[NonSerialized]
	private int _seed;

	// Token: 0x04003627 RID: 13863
	[NonSerialized]
	private T _lastItem;

	// Token: 0x04003628 RID: 13864
	[NonSerialized]
	private int _lastItemIndex = -1;

	// Token: 0x04003629 RID: 13865
	[NonSerialized]
	private SRand _rnd;

	// Token: 0x0400362A RID: 13866
	public UnityEvent<T> onNextItem;
}
