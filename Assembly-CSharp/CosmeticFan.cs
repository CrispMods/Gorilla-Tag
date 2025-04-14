using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class CosmeticFan : MonoBehaviour
{
	// Token: 0x0600084E RID: 2126 RVA: 0x0002DA5F File Offset: 0x0002BC5F
	private void Start()
	{
		this.spinUpRate = this.maxSpeed / this.spinUpDuration;
		this.spinDownRate = this.maxSpeed / this.spinDownDuration;
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x0002DA88 File Offset: 0x0002BC88
	public void Run()
	{
		this.targetSpeed = this.maxSpeed;
		if (this.spinUpDuration > 0f)
		{
			base.enabled = true;
			this.currentAccelRate = this.spinUpRate;
		}
		else
		{
			this.currentSpeed = this.maxSpeed;
		}
		base.enabled = true;
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x0002DAD6 File Offset: 0x0002BCD6
	public void Stop()
	{
		this.targetSpeed = 0f;
		if (this.spinDownDuration > 0f)
		{
			base.enabled = true;
			this.currentAccelRate = this.spinDownRate;
			return;
		}
		this.currentSpeed = 0f;
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x0002DB0F File Offset: 0x0002BD0F
	public void InstantStop()
	{
		this.targetSpeed = 0f;
		this.currentSpeed = 0f;
		base.enabled = false;
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0002DB30 File Offset: 0x0002BD30
	private void Update()
	{
		this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, this.targetSpeed, this.currentAccelRate * Time.deltaTime);
		base.transform.localRotation = base.transform.localRotation * Quaternion.AngleAxis(this.currentSpeed * Time.deltaTime, this.axis);
		if (this.currentSpeed == 0f && this.targetSpeed == 0f)
		{
			base.enabled = false;
		}
	}

	// Token: 0x040009CF RID: 2511
	[SerializeField]
	private Vector3 axis;

	// Token: 0x040009D0 RID: 2512
	[SerializeField]
	private float spinUpDuration = 0.3f;

	// Token: 0x040009D1 RID: 2513
	[SerializeField]
	private float spinDownDuration = 0.3f;

	// Token: 0x040009D2 RID: 2514
	[SerializeField]
	private float maxSpeed = 360f;

	// Token: 0x040009D3 RID: 2515
	private float currentSpeed;

	// Token: 0x040009D4 RID: 2516
	private float targetSpeed;

	// Token: 0x040009D5 RID: 2517
	private float currentAccelRate;

	// Token: 0x040009D6 RID: 2518
	private float spinUpRate;

	// Token: 0x040009D7 RID: 2519
	private float spinDownRate;
}
