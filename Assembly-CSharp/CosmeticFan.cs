using System;
using UnityEngine;

// Token: 0x0200014E RID: 334
public class CosmeticFan : MonoBehaviour
{
	// Token: 0x06000892 RID: 2194 RVA: 0x0003607C File Offset: 0x0003427C
	private void Start()
	{
		this.spinUpRate = this.maxSpeed / this.spinUpDuration;
		this.spinDownRate = this.maxSpeed / this.spinDownDuration;
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0008EC00 File Offset: 0x0008CE00
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

	// Token: 0x06000894 RID: 2196 RVA: 0x000360A4 File Offset: 0x000342A4
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

	// Token: 0x06000895 RID: 2197 RVA: 0x000360DD File Offset: 0x000342DD
	public void InstantStop()
	{
		this.targetSpeed = 0f;
		this.currentSpeed = 0f;
		base.enabled = false;
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0008EC50 File Offset: 0x0008CE50
	private void Update()
	{
		this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, this.targetSpeed, this.currentAccelRate * Time.deltaTime);
		base.transform.localRotation = base.transform.localRotation * Quaternion.AngleAxis(this.currentSpeed * Time.deltaTime, this.axis);
		if (this.currentSpeed == 0f && this.targetSpeed == 0f)
		{
			base.enabled = false;
		}
	}

	// Token: 0x04000A12 RID: 2578
	[SerializeField]
	private Vector3 axis;

	// Token: 0x04000A13 RID: 2579
	[SerializeField]
	private float spinUpDuration = 0.3f;

	// Token: 0x04000A14 RID: 2580
	[SerializeField]
	private float spinDownDuration = 0.3f;

	// Token: 0x04000A15 RID: 2581
	[SerializeField]
	private float maxSpeed = 360f;

	// Token: 0x04000A16 RID: 2582
	private float currentSpeed;

	// Token: 0x04000A17 RID: 2583
	private float targetSpeed;

	// Token: 0x04000A18 RID: 2584
	private float currentAccelRate;

	// Token: 0x04000A19 RID: 2585
	private float spinUpRate;

	// Token: 0x04000A1A RID: 2586
	private float spinDownRate;
}
