using System;
using UnityEngine;

// Token: 0x020005CC RID: 1484
public class LerpTask<T>
{
	// Token: 0x060024D8 RID: 9432 RVA: 0x00047F35 File Offset: 0x00046135
	public void Reset()
	{
		this.onLerp(this.lerpFrom, this.lerpTo, 0f);
		this.active = false;
		this.elapsed = 0f;
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x00047F65 File Offset: 0x00046165
	public void Start(T from, T to, float duration)
	{
		this.lerpFrom = from;
		this.lerpTo = to;
		this.duration = duration;
		this.elapsed = 0f;
		this.active = true;
	}

	// Token: 0x060024DA RID: 9434 RVA: 0x001024D8 File Offset: 0x001006D8
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

	// Token: 0x060024DB RID: 9435 RVA: 0x00102524 File Offset: 0x00100724
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

	// Token: 0x040028EF RID: 10479
	public float elapsed;

	// Token: 0x040028F0 RID: 10480
	public float duration;

	// Token: 0x040028F1 RID: 10481
	public T lerpFrom;

	// Token: 0x040028F2 RID: 10482
	public T lerpTo;

	// Token: 0x040028F3 RID: 10483
	public Action<T, T, float> onLerp;

	// Token: 0x040028F4 RID: 10484
	public Action onLerpEnd;

	// Token: 0x040028F5 RID: 10485
	public bool active;
}
