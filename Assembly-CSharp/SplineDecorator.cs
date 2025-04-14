using System;
using UnityEngine;

// Token: 0x0200087D RID: 2173
public class SplineDecorator : MonoBehaviour
{
	// Token: 0x0600349C RID: 13468 RVA: 0x000FB980 File Offset: 0x000F9B80
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
				Transform transform = Object.Instantiate<Transform>(this.items[j]);
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

	// Token: 0x04003761 RID: 14177
	public BezierSpline spline;

	// Token: 0x04003762 RID: 14178
	public int frequency;

	// Token: 0x04003763 RID: 14179
	public bool lookForward;

	// Token: 0x04003764 RID: 14180
	public Transform[] items;
}
