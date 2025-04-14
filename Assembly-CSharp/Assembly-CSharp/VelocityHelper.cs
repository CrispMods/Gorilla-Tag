using System;
using UnityEngine;

// Token: 0x020006D8 RID: 1752
[Serializable]
public class VelocityHelper
{
	// Token: 0x06002BB7 RID: 11191 RVA: 0x000D6E49 File Offset: 0x000D5049
	public VelocityHelper(int historySize = 12)
	{
		this._size = historySize;
		this._samples = new float[historySize * 4];
	}

	// Token: 0x06002BB8 RID: 11192 RVA: 0x000D6E68 File Offset: 0x000D5068
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

	// Token: 0x06002BB9 RID: 11193 RVA: 0x000D6EB0 File Offset: 0x000D50B0
	private void _InitSamples(Vector3 position, float dt)
	{
		for (int i = 0; i < this._size; i++)
		{
			this._SetSample(i, position, dt);
		}
		this._initialized = true;
	}

	// Token: 0x06002BBA RID: 11194 RVA: 0x000D6EDE File Offset: 0x000D50DE
	private void _SetSample(int i, Vector3 position, float dt)
	{
		this._samples[i] = position.x;
		this._samples[i + 1] = position.y;
		this._samples[i + 2] = position.z;
		this._samples[i + 3] = dt;
	}

	// Token: 0x040030CB RID: 12491
	private float[] _samples;

	// Token: 0x040030CC RID: 12492
	private int _latest;

	// Token: 0x040030CD RID: 12493
	private int _size;

	// Token: 0x040030CE RID: 12494
	private bool _initialized;
}
