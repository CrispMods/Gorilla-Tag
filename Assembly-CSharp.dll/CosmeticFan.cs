using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class CosmeticFan : MonoBehaviour
{
	// Token: 0x06000850 RID: 2128 RVA: 0x00034E06 File Offset: 0x00033006
	private void Start()
	{
		this.spinUpRate = this.maxSpeed / this.spinUpDuration;
		this.spinDownRate = this.maxSpeed / this.spinDownDuration;
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x0008C278 File Offset: 0x0008A478
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

	// Token: 0x06000852 RID: 2130 RVA: 0x00034E2E File Offset: 0x0003302E
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

	// Token: 0x06000853 RID: 2131 RVA: 0x00034E67 File Offset: 0x00033067
	public void InstantStop()
	{
		this.targetSpeed = 0f;
		this.currentSpeed = 0f;
		base.enabled = false;
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x0008C2C8 File Offset: 0x0008A4C8
	private void Update()
	{
		this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, this.targetSpeed, this.currentAccelRate * Time.deltaTime);
		base.transform.localRotation = base.transform.localRotation * Quaternion.AngleAxis(this.currentSpeed * Time.deltaTime, this.axis);
		if (this.currentSpeed == 0f && this.targetSpeed == 0f)
		{
			base.enabled = false;
		}
	}

	// Token: 0x040009D0 RID: 2512
	[SerializeField]
	private Vector3 axis;

	// Token: 0x040009D1 RID: 2513
	[SerializeField]
	private float spinUpDuration = 0.3f;

	// Token: 0x040009D2 RID: 2514
	[SerializeField]
	private float spinDownDuration = 0.3f;

	// Token: 0x040009D3 RID: 2515
	[SerializeField]
	private float maxSpeed = 360f;

	// Token: 0x040009D4 RID: 2516
	private float currentSpeed;

	// Token: 0x040009D5 RID: 2517
	private float targetSpeed;

	// Token: 0x040009D6 RID: 2518
	private float currentAccelRate;

	// Token: 0x040009D7 RID: 2519
	private float spinUpRate;

	// Token: 0x040009D8 RID: 2520
	private float spinDownRate;
}
