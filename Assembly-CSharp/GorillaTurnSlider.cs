using System;
using UnityEngine;

// Token: 0x02000673 RID: 1651
public class GorillaTurnSlider : MonoBehaviour
{
	// Token: 0x060028E9 RID: 10473 RVA: 0x000C8FBD File Offset: 0x000C71BD
	private void Awake()
	{
		this.startingLocation = base.transform.position;
		this.SetPosition(this.gorillaTurn.currentSpeed);
	}

	// Token: 0x060028EA RID: 10474 RVA: 0x000023F4 File Offset: 0x000005F4
	private void FixedUpdate()
	{
	}

	// Token: 0x060028EB RID: 10475 RVA: 0x000C8FE4 File Offset: 0x000C71E4
	public void SetPosition(float speed)
	{
		float num = this.startingLocation.x - this.zRange / 2f;
		float num2 = this.startingLocation.x + this.zRange / 2f;
		float x = (speed - this.minValue) * (num2 - num) / (this.maxValue - this.minValue) + num;
		base.transform.position = new Vector3(x, this.startingLocation.y, this.startingLocation.z);
	}

	// Token: 0x060028EC RID: 10476 RVA: 0x000C9068 File Offset: 0x000C7268
	public float InterpolateValue(float value)
	{
		float num = this.startingLocation.x - this.zRange / 2f;
		float num2 = this.startingLocation.x + this.zRange / 2f;
		return (value - num) / (num2 - num) * (this.maxValue - this.minValue) + this.minValue;
	}

	// Token: 0x060028ED RID: 10477 RVA: 0x000C90C4 File Offset: 0x000C72C4
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

	// Token: 0x04002DF8 RID: 11768
	public float zRange;

	// Token: 0x04002DF9 RID: 11769
	public float maxValue;

	// Token: 0x04002DFA RID: 11770
	public float minValue;

	// Token: 0x04002DFB RID: 11771
	public GorillaTurning gorillaTurn;

	// Token: 0x04002DFC RID: 11772
	private float startingZ;

	// Token: 0x04002DFD RID: 11773
	public Vector3 startingLocation;
}
