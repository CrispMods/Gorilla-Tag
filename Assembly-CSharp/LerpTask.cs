using System;
using UnityEngine;

// Token: 0x020005CB RID: 1483
public class LerpTask<T>
{
	// Token: 0x060024D0 RID: 9424 RVA: 0x000B6EC6 File Offset: 0x000B50C6
	public void Reset()
	{
		this.onLerp(this.lerpFrom, this.lerpTo, 0f);
		this.active = false;
		this.elapsed = 0f;
	}

	// Token: 0x060024D1 RID: 9425 RVA: 0x000B6EF6 File Offset: 0x000B50F6
	public void Start(T from, T to, float duration)
	{
		this.lerpFrom = from;
		this.lerpTo = to;
		this.duration = duration;
		this.elapsed = 0f;
		this.active = true;
	}

	// Token: 0x060024D2 RID: 9426 RVA: 0x000B6F20 File Offset: 0x000B5120
	public void Finish()
	{
		this.onLerp(this.lerpFrom, this.lerpTo, 1f);
		Action action = this.onLerpEnd;
		if (action != null)
		{
			action();
		}
		this.active = false;
		this.elapsed = 0f;
	}

	// Token: 0x060024D3 RID: 9427 RVA: 0x000B6F6C File Offset: 0x000B516C
	public void Update()
	{
		if (!this.active)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (this.elapsed < this.duration)
		{
			float arg = (this.elapsed + deltaTime >= this.duration) ? 1f : (this.elapsed / this.duration);
			this.onLerp(this.lerpFrom, this.lerpTo, arg);
			this.elapsed += deltaTime;
			return;
		}
		this.Finish();
	}

	// Token: 0x040028E9 RID: 10473
	public float elapsed;

	// Token: 0x040028EA RID: 10474
	public float duration;

	// Token: 0x040028EB RID: 10475
	public T lerpFrom;

	// Token: 0x040028EC RID: 10476
	public T lerpTo;

	// Token: 0x040028ED RID: 10477
	public Action<T, T, float> onLerp;

	// Token: 0x040028EE RID: 10478
	public Action onLerpEnd;

	// Token: 0x040028EF RID: 10479
	public bool active;
}
