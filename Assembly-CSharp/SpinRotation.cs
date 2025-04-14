using System;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class SpinRotation : MonoBehaviour, ITickSystemTick
{
	// Token: 0x170000ED RID: 237
	// (get) Token: 0x0600096D RID: 2413 RVA: 0x00032490 File Offset: 0x00030690
	// (set) Token: 0x0600096E RID: 2414 RVA: 0x00032498 File Offset: 0x00030698
	public bool TickRunning { get; set; }

	// Token: 0x0600096F RID: 2415 RVA: 0x000324A1 File Offset: 0x000306A1
	public void Tick()
	{
		base.transform.localRotation = Quaternion.Euler(this.rotationPerSecondEuler * (Time.time - this.baseTime)) * this.baseRotation;
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x000324D5 File Offset: 0x000306D5
	private void Awake()
	{
		this.baseRotation = base.transform.localRotation;
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x000324E8 File Offset: 0x000306E8
	private void OnEnable()
	{
		TickSystem<object>.AddTickCallback(this);
		this.baseTime = Time.time;
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0002B49D File Offset: 0x0002969D
	private void OnDisable()
	{
		TickSystem<object>.RemoveTickCallback(this);
	}

	// Token: 0x04000B62 RID: 2914
	[SerializeField]
	private Vector3 rotationPerSecondEuler;

	// Token: 0x04000B63 RID: 2915
	private Quaternion baseRotation;

	// Token: 0x04000B64 RID: 2916
	private float baseTime;
}
