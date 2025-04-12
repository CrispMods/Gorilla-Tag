using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Text;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class ParticleEffectsPool : MonoBehaviour
{
	// Token: 0x06000935 RID: 2357 RVA: 0x00035855 File Offset: 0x00033A55
	public void Awake()
	{
		this.OnPoolAwake();
		this.Setup();
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnPoolAwake()
	{
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0008F6E0 File Offset: 0x0008D8E0
	private void Setup()
	{
		this.MoveToSceneWorldRoot();
		this._pools = new RingBuffer<ParticleEffect>[this.effects.Length];
		this._effectToPool = new Dictionary<long, int>(this.effects.Length);
		for (int i = 0; i < this.effects.Length; i++)
		{
			ParticleEffect particleEffect = this.effects[i];
			this._pools[i] = this.InitPoolForPrefab(i, this.effects[i]);
			this._effectToPool.TryAdd(particleEffect.effectID, i);
		}
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x00035863 File Offset: 0x00033A63
	private void MoveToSceneWorldRoot()
	{
		Transform transform = base.transform;
		transform.parent = null;
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0008F760 File Offset: 0x0008D960
	private RingBuffer<ParticleEffect> InitPoolForPrefab(int index, ParticleEffect prefab)
	{
		RingBuffer<ParticleEffect> ringBuffer = new RingBuffer<ParticleEffect>(this.poolSize);
		string arg = prefab.name.Trim();
		for (int i = 0; i < this.poolSize; i++)
		{
			ParticleEffect particleEffect = UnityEngine.Object.Instantiate<ParticleEffect>(prefab, base.transform);
			particleEffect.gameObject.SetActive(false);
			particleEffect.pool = this;
			particleEffect.poolIndex = index;
			particleEffect.name = ZString.Concat<string, string, int>(arg, "*", i);
			ringBuffer.Push(particleEffect);
		}
		return ringBuffer;
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x00035892 File Offset: 0x00033A92
	public void PlayEffect(ParticleEffect effect, Vector3 worldPos)
	{
		this.PlayEffect(effect.effectID, worldPos);
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x000358A1 File Offset: 0x00033AA1
	public void PlayEffect(ParticleEffect effect, Vector3 worldPos, float delay)
	{
		this.PlayEffect(effect.effectID, worldPos, delay);
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x000358B1 File Offset: 0x00033AB1
	public void PlayEffect(long effectID, Vector3 worldPos)
	{
		this.PlayEffect(this.GetPoolIndex(effectID), worldPos);
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x000358C1 File Offset: 0x00033AC1
	public void PlayEffect(long effectID, Vector3 worldPos, float delay)
	{
		this.PlayEffect(this.GetPoolIndex(effectID), worldPos, delay);
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0008F7D8 File Offset: 0x0008D9D8
	public void PlayEffect(int index, Vector3 worldPos)
	{
		if (index == -1)
		{
			return;
		}
		ParticleEffect particleEffect;
		if (!this._pools[index].TryPop(out particleEffect))
		{
			return;
		}
		particleEffect.transform.localPosition = worldPos;
		particleEffect.Play();
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x000358D2 File Offset: 0x00033AD2
	public void PlayEffect(int index, Vector3 worldPos, float delay)
	{
		if (delay.Approx(0f, 1E-06f))
		{
			this.PlayEffect(index, worldPos);
			return;
		}
		base.StartCoroutine(this.PlayDelayed(index, worldPos, delay));
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x000358FF File Offset: 0x00033AFF
	private IEnumerator PlayDelayed(int index, Vector3 worldPos, float delay)
	{
		yield return new WaitForSeconds(delay);
		this.PlayEffect(index, worldPos);
		yield break;
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x00035923 File Offset: 0x00033B23
	public void Return(ParticleEffect effect)
	{
		this._pools[effect.poolIndex].Push(effect);
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0008F810 File Offset: 0x0008DA10
	public int GetPoolIndex(long effectID)
	{
		int result;
		if (this._effectToPool.TryGetValue(effectID, out result))
		{
			return result;
		}
		return -1;
	}

	// Token: 0x04000B36 RID: 2870
	public ParticleEffect[] effects = new ParticleEffect[0];

	// Token: 0x04000B37 RID: 2871
	[Space]
	public int poolSize = 10;

	// Token: 0x04000B38 RID: 2872
	[Space]
	private RingBuffer<ParticleEffect>[] _pools = new RingBuffer<ParticleEffect>[0];

	// Token: 0x04000B39 RID: 2873
	private Dictionary<long, int> _effectToPool = new Dictionary<long, int>();
}
