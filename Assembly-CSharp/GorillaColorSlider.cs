using System;
using UnityEngine;

// Token: 0x0200065C RID: 1628
public class GorillaColorSlider : MonoBehaviour
{
	// Token: 0x0600285A RID: 10330 RVA: 0x000C6659 File Offset: 0x000C4859
	private void Start()
	{
		if (!this.setRandomly)
		{
			this.startingLocation = base.transform.position;
		}
	}

	// Token: 0x0600285B RID: 10331 RVA: 0x000C6674 File Offset: 0x000C4874
	public void SetPosition(float speed)
	{
		float num = this.startingLocation.x - this.zRange / 2f;
		float num2 = this.startingLocation.x + this.zRange / 2f;
		float x = (speed - this.minValue) * (num2 - num) / (this.maxValue - this.minValue) + num;
		base.transform.position = new Vector3(x, this.startingLocation.y, this.startingLocation.z);
		this.valueImReporting = this.InterpolateValue(base.transform.position.x);
	}

	// Token: 0x0600285C RID: 10332 RVA: 0x000C6714 File Offset: 0x000C4914
	public float InterpolateValue(float value)
	{
		float num = this.startingLocation.x - this.zRange / 2f;
		float num2 = this.startingLocation.x + this.zRange / 2f;
		return (value - num) / (num2 - num) * (this.maxValue - this.minValue) + this.minValue;
	}

	// Token: 0x0600285D RID: 10333 RVA: 0x000C6770 File Offset: 0x000C4970
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

	// Token: 0x04002D33 RID: 11571
	public bool setRandomly;

	// Token: 0x04002D34 RID: 11572
	public float zRange;

	// Token: 0x04002D35 RID: 11573
	public float maxValue;

	// Token: 0x04002D36 RID: 11574
	public float minValue;

	// Token: 0x04002D37 RID: 11575
	public Vector3 startingLocation;

	// Token: 0x04002D38 RID: 11576
	public int valueIndex;

	// Token: 0x04002D39 RID: 11577
	public float valueImReporting;

	// Token: 0x04002D3A RID: 11578
	public GorillaTriggerBox gorilla;

	// Token: 0x04002D3B RID: 11579
	private float startingZ;
}
