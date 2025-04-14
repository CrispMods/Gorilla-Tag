using System;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200017F RID: 383
public class TriggerOnSpeed : MonoBehaviour, ITickSystemTick
{
	// Token: 0x06000999 RID: 2457 RVA: 0x00019E2F File Offset: 0x0001802F
	private void OnEnable()
	{
		TickSystem<object>.AddCallbackTarget(this);
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x00019E37 File Offset: 0x00018037
	private void OnDisable()
	{
		TickSystem<object>.RemoveCallbackTarget(this);
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0003333C File Offset: 0x0003153C
	public void Tick()
	{
		bool flag = this.velocityEstimator.linearVelocity.IsLongerThan(this.speedThreshold);
		if (flag != this.wasFaster)
		{
			if (flag)
			{
				this.onFaster.Invoke();
			}
			else
			{
				this.onSlower.Invoke();
			}
			this.wasFaster = flag;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x0600099C RID: 2460 RVA: 0x0003338B File Offset: 0x0003158B
	// (set) Token: 0x0600099D RID: 2461 RVA: 0x00033393 File Offset: 0x00031593
	public bool TickRunning { get; set; }

	// Token: 0x04000B9B RID: 2971
	[SerializeField]
	private float speedThreshold;

	// Token: 0x04000B9C RID: 2972
	[SerializeField]
	private UnityEvent onFaster;

	// Token: 0x04000B9D RID: 2973
	[SerializeField]
	private UnityEvent onSlower;

	// Token: 0x04000B9E RID: 2974
	[SerializeField]
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04000B9F RID: 2975
	private bool wasFaster;
}
