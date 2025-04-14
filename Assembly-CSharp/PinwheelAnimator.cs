using System;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class PinwheelAnimator : MonoBehaviour
{
	// Token: 0x06000560 RID: 1376 RVA: 0x0001FF2A File Offset: 0x0001E12A
	protected void OnEnable()
	{
		this.oldPos = this.spinnerTransform.position;
		this.spinSpeed = 0f;
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x0001FF48 File Offset: 0x0001E148
	protected void LateUpdate()
	{
		Vector3 position = this.spinnerTransform.position;
		Vector3 forward = base.transform.forward;
		Vector3 vector = position - this.oldPos;
		float b = Mathf.Clamp(vector.magnitude / Time.deltaTime * Vector3.Dot(vector.normalized, forward) * this.spinSpeedMultiplier, -this.maxSpinSpeed, this.maxSpinSpeed);
		this.spinSpeed = Mathf.Lerp(this.spinSpeed, b, Time.deltaTime * this.damping);
		this.spinnerTransform.Rotate(Vector3.forward, this.spinSpeed * 360f * Time.deltaTime);
		this.oldPos = position;
	}

	// Token: 0x0400063C RID: 1596
	public Transform spinnerTransform;

	// Token: 0x0400063D RID: 1597
	[Tooltip("In revolutions per second.")]
	public float maxSpinSpeed = 4f;

	// Token: 0x0400063E RID: 1598
	public float spinSpeedMultiplier = 5f;

	// Token: 0x0400063F RID: 1599
	public float damping = 0.5f;

	// Token: 0x04000640 RID: 1600
	private Vector3 oldPos;

	// Token: 0x04000641 RID: 1601
	private float spinSpeed;
}
