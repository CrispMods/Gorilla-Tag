using System;
using UnityEngine;

// Token: 0x020005D9 RID: 1497
public class LerpTask<T>
{
	// Token: 0x06002532 RID: 9522 RVA: 0x00049350 File Offset: 0x00047550
	public void Reset()
	{
		this.onLerp(this.lerpFrom, this.lerpTo, 0f);
		this.active = false;
		this.elapsed = 0f;
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x00049380 File Offset: 0x00047580
	public void Start(T from, T to, float duration)
	{
		this.lerpFrom = from;
		this.lerpTo = to;
		this.duration = duration;
		this.elapsed = 0f;
		this.active = true;
	}

	// Token: 0x06002534 RID: 9524 RVA: 0x001053BC File Offset: 0x001035BC
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

	// Token: 0x06002535 RID: 9525 RVA: 0x00105408 File Offset: 0x00103608
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

	// Token: 0x04002948 RID: 10568
	public float elapsed;

	// Token: 0x04002949 RID: 10569
	public float duration;

	// Token: 0x0400294A RID: 10570
	public T lerpFrom;

	// Token: 0x0400294B RID: 10571
	public T lerpTo;

	// Token: 0x0400294C RID: 10572
	public Action<T, T, float> onLerp;

	// Token: 0x0400294D RID: 10573
	public Action onLerpEnd;

	// Token: 0x0400294E RID: 10574
	public bool active;
}
