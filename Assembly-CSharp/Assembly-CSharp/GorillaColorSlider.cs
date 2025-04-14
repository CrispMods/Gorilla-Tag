using System;
using UnityEngine;

// Token: 0x0200065D RID: 1629
public class GorillaColorSlider : MonoBehaviour
{
	// Token: 0x06002862 RID: 10338 RVA: 0x000C6AD9 File Offset: 0x000C4CD9
	private void Start()
	{
		if (!this.setRandomly)
		{
			this.startingLocation = base.transform.position;
		}
	}

	// Token: 0x06002863 RID: 10339 RVA: 0x000C6AF4 File Offset: 0x000C4CF4
	public void SetPosition(float speed)
	{
		float num = this.startingLocation.x - this.zRange / 2f;
		float num2 = this.startingLocation.x + this.zRange / 2f;
		float x = (speed - this.minValue) * (num2 - num) / (this.maxValue - this.minValue) + num;
		base.transform.position = new Vector3(x, this.startingLocation.y, this.startingLocation.z);
		this.valueImReporting = this.InterpolateValue(base.transform.position.x);
	}

	// Token: 0x06002864 RID: 10340 RVA: 0x000C6B94 File Offset: 0x000C4D94
	public float InterpolateValue(float value)
	{
		float num = this.startingLocation.x - this.zRange / 2f;
		float num2 = this.startingLocation.x + this.zRange / 2f;
		return (value - num) / (num2 - num) * (this.maxValue - this.minValue) + this.minValue;
	}

	// Token: 0x06002865 RID: 10341 RVA: 0x000C6BF0 File Offset: 0x000C4DF0
	public void OnSliderRelease()
	{
		if (this.zRange != 0f && (base.transform.position - this.startingLocation).magnitude > this.zRange / 2f)
		{
			if (base.transform.position.x > this.startingLocation.x)
			{
				base.transform.position = new Vector3(this.startingLocation.x + this.zRange / 2f, this.startingLocation.y, this.startingLocation.z);
			}
			else
			{
				base.transform.position = new Vector3(this.startingLocation.x - this.zRange / 2f, this.startingLocation.y, this.startingLocation.z);
			}
		}
		this.valueImReporting = this.InterpolateValue(base.transform.position.x);
	}

	// Token: 0x04002D39 RID: 11577
	public bool setRandomly;

	// Token: 0x04002D3A RID: 11578
	public float zRange;

	// Token: 0x04002D3B RID: 11579
	public float maxValue;

	// Token: 0x04002D3C RID: 11580
	public float minValue;

	// Token: 0x04002D3D RID: 11581
	public Vector3 startingLocation;

	// Token: 0x04002D3E RID: 11582
	public int valueIndex;

	// Token: 0x04002D3F RID: 11583
	public float valueImReporting;

	// Token: 0x04002D40 RID: 11584
	public GorillaTriggerBox gorilla;

	// Token: 0x04002D41 RID: 11585
	private float startingZ;
}
