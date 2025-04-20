using System;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class SpinRotation : MonoBehaviour, ITickSystemTick
{
	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x060009B9 RID: 2489 RVA: 0x00036E08 File Offset: 0x00035008
	// (set) Token: 0x060009BA RID: 2490 RVA: 0x00036E10 File Offset: 0x00035010
	public bool TickRunning { get; set; }

	// Token: 0x060009BB RID: 2491 RVA: 0x00036E19 File Offset: 0x00035019
	public void Tick()
	{
		base.transform.localRotation = Quaternion.Euler(this.rotationPerSecondEuler * (Time.time - this.baseTime)) * this.baseRotation;
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x00036E4D File Offset: 0x0003504D
	private void Awake()
	{
		this.baseRotation = base.transform.localRotation;
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x00036E60 File Offset: 0x00035060
	private void OnEnable()
	{
		TickSystem<object>.AddTickCallback(this);
		this.baseTime = Time.time;
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x00035BE1 File Offset: 0x00033DE1
	private void OnDisable()
	{
		TickSystem<object>.RemoveTickCallback(this);
	}

	// Token: 0x04000BA8 RID: 2984
	[SerializeField]
	private Vector3 rotationPerSecondEuler;

	// Token: 0x04000BA9 RID: 2985
	private Quaternion baseRotation;

	// Token: 0x04000BAA RID: 2986
	private float baseTime;
}
