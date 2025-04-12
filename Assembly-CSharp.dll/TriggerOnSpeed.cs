using System;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200017F RID: 383
public class TriggerOnSpeed : MonoBehaviour, ITickSystemTick
{
	// Token: 0x06000999 RID: 2457 RVA: 0x0003247C File Offset: 0x0003067C
	private void OnEnable()
	{
		TickSystem<object>.AddCallbackTarget(this);
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x00032484 File Offset: 0x00030684
	private void OnDisable()
	{
		TickSystem<object>.RemoveCallbackTarget(this);
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x00090914 File Offset: 0x0008EB14
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
	// (get) Token: 0x0600099C RID: 2460 RVA: 0x00035D18 File Offset: 0x00033F18
	// (set) Token: 0x0600099D RID: 2461 RVA: 0x00035D20 File Offset: 0x00033F20
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
