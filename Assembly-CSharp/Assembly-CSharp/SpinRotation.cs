using System;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class SpinRotation : MonoBehaviour, ITickSystemTick
{
	// Token: 0x170000ED RID: 237
	// (get) Token: 0x0600096F RID: 2415 RVA: 0x000327B4 File Offset: 0x000309B4
	// (set) Token: 0x06000970 RID: 2416 RVA: 0x000327BC File Offset: 0x000309BC
	public bool TickRunning { get; set; }

	// Token: 0x06000971 RID: 2417 RVA: 0x000327C5 File Offset: 0x000309C5
	public void Tick()
	{
		base.transform.localRotation = Quaternion.Euler(this.rotationPerSecondEuler * (Time.time - this.baseTime)) * this.baseRotation;
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x000327F9 File Offset: 0x000309F9
	private void Awake()
	{
		this.baseRotation = base.transform.localRotation;
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x0003280C File Offset: 0x00030A0C
	private void OnEnable()
	{
		TickSystem<object>.AddTickCallback(this);
		this.baseTime = Time.time;
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x0002B7C1 File Offset: 0x000299C1
	private void OnDisable()
	{
		TickSystem<object>.RemoveTickCallback(this);
	}

	// Token: 0x04000B63 RID: 2915
	[SerializeField]
	private Vector3 rotationPerSecondEuler;

	// Token: 0x04000B64 RID: 2916
	private Quaternion baseRotation;

	// Token: 0x04000B65 RID: 2917
	private float baseTime;
}
