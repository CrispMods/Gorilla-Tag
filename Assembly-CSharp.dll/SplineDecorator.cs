using System;
using UnityEngine;

// Token: 0x02000880 RID: 2176
public class SplineDecorator : MonoBehaviour
{
	// Token: 0x060034A8 RID: 13480 RVA: 0x0013D084 File Offset: 0x0013B284
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

	// Token: 0x04003773 RID: 14195
	public BezierSpline spline;

	// Token: 0x04003774 RID: 14196
	public int frequency;

	// Token: 0x04003775 RID: 14197
	public bool lookForward;

	// Token: 0x04003776 RID: 14198
	public Transform[] items;
}
