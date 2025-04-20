using System;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200018A RID: 394
public class TriggerOnSpeed : MonoBehaviour, ITickSystemTick
{
	// Token: 0x060009E3 RID: 2531 RVA: 0x00033683 File Offset: 0x00031883
	private void OnEnable()
	{
		TickSystem<object>.AddCallbackTarget(this);
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x0003368B File Offset: 0x0003188B
	private void OnDisable()
	{
		TickSystem<object>.RemoveCallbackTarget(this);
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x00093208 File Offset: 0x00091408
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

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x060009E6 RID: 2534 RVA: 0x00036FD8 File Offset: 0x000351D8
	// (set) Token: 0x060009E7 RID: 2535 RVA: 0x00036FE0 File Offset: 0x000351E0
	public bool TickRunning { get; set; }

	// Token: 0x04000BE0 RID: 3040
	[SerializeField]
	private float speedThreshold;

	// Token: 0x04000BE1 RID: 3041
	[SerializeField]
	private UnityEvent onFaster;

	// Token: 0x04000BE2 RID: 3042
	[SerializeField]
	private UnityEvent onSlower;

	// Token: 0x04000BE3 RID: 3043
	[SerializeField]
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04000BE4 RID: 3044
	private bool wasFaster;
}
