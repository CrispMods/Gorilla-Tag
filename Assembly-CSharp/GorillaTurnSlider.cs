using System;
using UnityEngine;

// Token: 0x02000652 RID: 1618
public class GorillaTurnSlider : MonoBehaviour
{
	// Token: 0x06002814 RID: 10260 RVA: 0x0004B48C File Offset: 0x0004968C
	private void Awake()
	{
		this.startingLocation = base.transform.position;
		this.SetPosition(this.gorillaTurn.currentSpeed);
	}

	// Token: 0x06002815 RID: 10261 RVA: 0x00030607 File Offset: 0x0002E807
	private void FixedUpdate()
	{
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x0010FA2C File Offset: 0x0010DC2C
	public void SetPosition(float speed)
	{
		float num = this.startingLocation.x - this.zRange / 2f;
		float num2 = this.startingLocation.x + this.zRange / 2f;
		float x = (speed - this.minValue) * (num2 - num) / (this.maxValue - this.minValue) + num;
		base.transform.position = new Vector3(x, this.startingLocation.y, this.startingLocation.z);
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x0010FAB0 File Offset: 0x0010DCB0
	public float InterpolateValue(float value)
	{
		float num = this.startingLocation.x - this.zRange / 2f;
		float num2 = this.startingLocation.x + this.zRange / 2f;
		return (value - num) / (num2 - num) * (this.maxValue - this.minValue) + this.minValue;
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x0010FB0C File Offset: 0x0010DD0C
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

	// Token: 0x04002D5E RID: 11614
	public float zRange;

	// Token: 0x04002D5F RID: 11615
	public float maxValue;

	// Token: 0x04002D60 RID: 11616
	public float minValue;

	// Token: 0x04002D61 RID: 11617
	public GorillaTurning gorillaTurn;

	// Token: 0x04002D62 RID: 11618
	private float startingZ;

	// Token: 0x04002D63 RID: 11619
	public Vector3 startingLocation;
}
