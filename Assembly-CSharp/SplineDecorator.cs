using System;
using UnityEngine;

// Token: 0x02000899 RID: 2201
public class SplineDecorator : MonoBehaviour
{
	// Token: 0x06003568 RID: 13672 RVA: 0x0014266C File Offset: 0x0014086C
	private void Awake()
	{
		if (this.frequency <= 0 || this.items == null || this.items.Length == 0)
		{
			return;
		}
		float num = (float)(this.frequency * this.items.Length);
		if (this.spline.Loop || num == 1f)
		{
			num = 1f / num;
		}
		else
		{
			num = 1f / (num - 1f);
		}
		int num2 = 0;
		for (int i = 0; i < this.frequency; i++)
		{
			int j = 0;
			while (j < this.items.Length)
			{
				Transform transform = UnityEngine.Object.Instantiate<Transform>(this.items[j]);
				Vector3 point = this.spline.GetPoint((float)num2 * num);
				transform.transform.localPosition = point;
				if (this.lookForward)
				{
					transform.transform.LookAt(point + this.spline.GetDirection((float)num2 * num));
				}
				transform.transform.parent = base.transform;
				j++;
				num2++;
			}
		}
	}

	// Token: 0x04003821 RID: 14369
	public BezierSpline spline;

	// Token: 0x04003822 RID: 14370
	public int frequency;

	// Token: 0x04003823 RID: 14371
	public bool lookForward;

	// Token: 0x04003824 RID: 14372
	public Transform[] items;
}
