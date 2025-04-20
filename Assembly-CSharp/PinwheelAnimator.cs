using System;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class PinwheelAnimator : MonoBehaviour
{
	// Token: 0x060005A1 RID: 1441 RVA: 0x000341B9 File Offset: 0x000323B9
	protected void OnEnable()
	{
		this.oldPos = this.spinnerTransform.position;
		this.spinSpeed = 0f;
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x00082F0C File Offset: 0x0008110C
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

	// Token: 0x0400067D RID: 1661
	public Transform spinnerTransform;

	// Token: 0x0400067E RID: 1662
	[Tooltip("In revolutions per second.")]
	public float maxSpinSpeed = 4f;

	// Token: 0x0400067F RID: 1663
	public float spinSpeedMultiplier = 5f;

	// Token: 0x04000680 RID: 1664
	public float damping = 0.5f;

	// Token: 0x04000681 RID: 1665
	private Vector3 oldPos;

	// Token: 0x04000682 RID: 1666
	private float spinSpeed;
}
