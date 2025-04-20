using System;
using UnityEngine;

// Token: 0x0200063B RID: 1595
public class GorillaColorSlider : MonoBehaviour
{
	// Token: 0x06002785 RID: 10117 RVA: 0x0004AEC7 File Offset: 0x000490C7
	private void Start()
	{
		if (!this.setRandomly)
		{
			this.startingLocation = base.transform.position;
		}
	}

	// Token: 0x06002786 RID: 10118 RVA: 0x0010D680 File Offset: 0x0010B880
	public void SetPosition(float speed)
	{
		float num = this.startingLocation.x - this.zRange / 2f;
		float num2 = this.startingLocation.x + this.zRange / 2f;
		float x = (speed - this.minValue) * (num2 - num) / (this.maxValue - this.minValue) + num;
		base.transform.position = new Vector3(x, this.startingLocation.y, this.startingLocation.z);
		this.valueImReporting = this.InterpolateValue(base.transform.position.x);
	}

	// Token: 0x06002787 RID: 10119 RVA: 0x0010D720 File Offset: 0x0010B920
	public float InterpolateValue(float value)
	{
		float num = this.startingLocation.x - this.zRange / 2f;
		float num2 = this.startingLocation.x + this.zRange / 2f;
		return (value - num) / (num2 - num) * (this.maxValue - this.minValue) + this.minValue;
	}

	// Token: 0x06002788 RID: 10120 RVA: 0x0010D77C File Offset: 0x0010B97C
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

	// Token: 0x04002C99 RID: 11417
	public bool setRandomly;

	// Token: 0x04002C9A RID: 11418
	public float zRange;

	// Token: 0x04002C9B RID: 11419
	public float maxValue;

	// Token: 0x04002C9C RID: 11420
	public float minValue;

	// Token: 0x04002C9D RID: 11421
	public Vector3 startingLocation;

	// Token: 0x04002C9E RID: 11422
	public int valueIndex;

	// Token: 0x04002C9F RID: 11423
	public float valueImReporting;

	// Token: 0x04002CA0 RID: 11424
	public GorillaTriggerBox gorilla;

	// Token: 0x04002CA1 RID: 11425
	private float startingZ;
}
