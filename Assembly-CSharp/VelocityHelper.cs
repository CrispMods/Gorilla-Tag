using System;
using UnityEngine;

// Token: 0x020006D7 RID: 1751
[Serializable]
public class VelocityHelper
{
	// Token: 0x06002BAF RID: 11183 RVA: 0x000D69C9 File Offset: 0x000D4BC9
	public VelocityHelper(int historySize = 12)
	{
		this._size = historySize;
		this._samples = new float[historySize * 4];
	}

	// Token: 0x06002BB0 RID: 11184 RVA: 0x000D69E8 File Offset: 0x000D4BE8
	public void SamplePosition(Transform target, float dt)
	{
		Vector3 position = target.position;
		if (!this._initialized)
		{
			this._InitSamples(position, dt);
		}
		this._SetSample(this._latest, position, dt);
		this._latest = (this._latest + 1) % this._size;
	}

	// Token: 0x06002BB1 RID: 11185 RVA: 0x000D6A30 File Offset: 0x000D4C30
	private void _InitSamples(Vector3 position, float dt)
	{
		for (int i = 0; i < this._size; i++)
		{
			this._SetSample(i, position, dt);
		}
		this._initialized = true;
	}

	// Token: 0x06002BB2 RID: 11186 RVA: 0x000D6A5E File Offset: 0x000D4C5E
	private void _SetSample(int i, Vector3 position, float dt)
	{
		this._samples[i] = position.x;
		this._samples[i + 1] = position.y;
		this._samples[i + 2] = position.z;
		this._samples[i + 3] = dt;
	}

	// Token: 0x040030C5 RID: 12485
	private float[] _samples;

	// Token: 0x040030C6 RID: 12486
	private int _latest;

	// Token: 0x040030C7 RID: 12487
	private int _size;

	// Token: 0x040030C8 RID: 12488
	private bool _initialized;
}
