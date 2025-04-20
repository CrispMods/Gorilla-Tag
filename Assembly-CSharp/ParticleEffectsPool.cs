using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Text;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class ParticleEffectsPool : MonoBehaviour
{
	// Token: 0x06000980 RID: 2432 RVA: 0x00036B20 File Offset: 0x00034D20
	public void Awake()
	{
		this.OnPoolAwake();
		this.Setup();
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnPoolAwake()
	{
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00092068 File Offset: 0x00090268
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

	// Token: 0x06000983 RID: 2435 RVA: 0x00036B2E File Offset: 0x00034D2E
	private void MoveToSceneWorldRoot()
	{
		Transform transform = base.transform;
		transform.parent = null;
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x000920E8 File Offset: 0x000902E8
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

	// Token: 0x06000985 RID: 2437 RVA: 0x00036B5D File Offset: 0x00034D5D
	public void PlayEffect(ParticleEffect effect, Vector3 worldPos)
	{
		this.PlayEffect(effect.effectID, worldPos);
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x00036B6C File Offset: 0x00034D6C
	public void PlayEffect(ParticleEffect effect, Vector3 worldPos, float delay)
	{
		this.PlayEffect(effect.effectID, worldPos, delay);
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x00036B7C File Offset: 0x00034D7C
	public void PlayEffect(long effectID, Vector3 worldPos)
	{
		this.PlayEffect(this.GetPoolIndex(effectID), worldPos);
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x00036B8C File Offset: 0x00034D8C
	public void PlayEffect(long effectID, Vector3 worldPos, float delay)
	{
		this.PlayEffect(this.GetPoolIndex(effectID), worldPos, delay);
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x00092160 File Offset: 0x00090360
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

	// Token: 0x0600098A RID: 2442 RVA: 0x00036B9D File Offset: 0x00034D9D
	public void PlayEffect(int index, Vector3 worldPos, float delay)
	{
		if (delay.Approx(0f, 1E-06f))
		{
			this.PlayEffect(index, worldPos);
			return;
		}
		base.StartCoroutine(this.PlayDelayed(index, worldPos, delay));
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x00036BCA File Offset: 0x00034DCA
	private IEnumerator PlayDelayed(int index, Vector3 worldPos, float delay)
	{
		yield return new WaitForSeconds(delay);
		this.PlayEffect(index, worldPos);
		yield break;
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x00036BEE File Offset: 0x00034DEE
	public void Return(ParticleEffect effect)
	{
		this._pools[effect.poolIndex].Push(effect);
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00092198 File Offset: 0x00090398
	public int GetPoolIndex(long effectID)
	{
		int result;
		if (this._effectToPool.TryGetValue(effectID, out result))
		{
			return result;
		}
		return -1;
	}

	// Token: 0x04000B7C RID: 2940
	public ParticleEffect[] effects = new ParticleEffect[0];

	// Token: 0x04000B7D RID: 2941
	[Space]
	public int poolSize = 10;

	// Token: 0x04000B7E RID: 2942
	[Space]
	private RingBuffer<ParticleEffect>[] _pools = new RingBuffer<ParticleEffect>[0];

	// Token: 0x04000B7F RID: 2943
	private Dictionary<long, int> _effectToPool = new Dictionary<long, int>();
}
