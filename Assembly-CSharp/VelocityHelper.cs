using System;
using UnityEngine;

// Token: 0x020006EC RID: 1772
[Serializable]
public class VelocityHelper
{
	// Token: 0x06002C45 RID: 11333 RVA: 0x0004E101 File Offset: 0x0004C301
	public VelocityHelper(int historySize = 12)
	{
		this._size = historySize;
		this._samples = new float[historySize * 4];
	}

	// Token: 0x06002C46 RID: 11334 RVA: 0x00121CA8 File Offset: 0x0011FEA8
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

	// Token: 0x06002C47 RID: 11335 RVA: 0x00121CF0 File Offset: 0x0011FEF0
	private void _InitSamples(Vector3 position, float dt)
	{
		for (int i = 0; i < this._size; i++)
		{
			this._SetSample(i, position, dt);
		}
		this._initialized = true;
	}

	// Token: 0x06002C48 RID: 11336 RVA: 0x0004E11E File Offset: 0x0004C31E
	private void _SetSample(int i, Vector3 position, float dt)
	{
		this._samples[i] = position.x;
		this._samples[i + 1] = position.y;
		this._samples[i + 2] = position.z;
		this._samples[i + 3] = dt;
	}

	// Token: 0x04003162 RID: 12642
	private float[] _samples;

	// Token: 0x04003163 RID: 12643
	private int _latest;

	// Token: 0x04003164 RID: 12644
	private int _size;

	// Token: 0x04003165 RID: 12645
	private bool _initialized;
}
