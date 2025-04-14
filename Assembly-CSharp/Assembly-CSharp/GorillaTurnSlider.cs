using System;
using UnityEngine;

// Token: 0x02000674 RID: 1652
public class GorillaTurnSlider : MonoBehaviour
{
	// Token: 0x060028F1 RID: 10481 RVA: 0x000C943D File Offset: 0x000C763D
	private void Awake()
	{
		this.startingLocation = base.transform.position;
		this.SetPosition(this.gorillaTurn.currentSpeed);
	}

	// Token: 0x060028F2 RID: 10482 RVA: 0x000023F4 File Offset: 0x000005F4
	private void FixedUpdate()
	{
	}

	// Token: 0x060028F3 RID: 10483 RVA: 0x000C9464 File Offset: 0x000C7664
	public void SetPosition(float speed)
	{
		float num = this.startingLocation.x - this.zRange / 2f;
		float num2 = this.startingLocation.x + this.zRange / 2f;
		float x = (speed - this.minValue) * (num2 - num) / (this.maxValue - this.minValue) + num;
		base.transform.position = new Vector3(x, this.startingLocation.y, this.startingLocation.z);
	}

	// Token: 0x060028F4 RID: 10484 RVA: 0x000C94E8 File Offset: 0x000C76E8
	public float InterpolateValue(float value)
	{
		float num = this.startingLocation.x - this.zRange / 2f;
		float num2 = this.startingLocation.x + this.zRange / 2f;
		return (value - num) / (num2 - num) * (this.maxValue - this.minValue) + this.minValue;
	}

	// Token: 0x060028F5 RID: 10485 RVA: 0x000C9544 File Offset: 0x000C7744
	public void OnSliderRelease()
	{
		if (this.zRange != 0f && (base.transform.position - this.startingLocation).magnitude > this.zRange / 2f)
		{
			if (base.transform.position.x > this.startingLocation.x)
			{
				base.transform.position = new Vector3(this.startingLocation.x + this.zRange / 2f, this.startingLocation.y, this.startingLocation.z);
				return;
			}
			base.transform.position = new Vector3(this.startingLocation.x - this.zRange / 2f, this.startingLocation.y, this.startingLocation.z);
		}
	}

	// Token: 0x04002DFE RID: 11774
	public float zRange;

	// Token: 0x04002DFF RID: 11775
	public float maxValue;

	// Token: 0x04002E00 RID: 11776
	public float minValue;

	// Token: 0x04002E01 RID: 11777
	public GorillaTurning gorillaTurn;

	// Token: 0x04002E02 RID: 11778
	private float startingZ;

	// Token: 0x04002E03 RID: 11779
	public Vector3 startingLocation;
}
