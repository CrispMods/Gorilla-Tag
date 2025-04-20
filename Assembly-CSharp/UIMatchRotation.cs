using System;
using UnityEngine;

// Token: 0x020008A8 RID: 2216
public class UIMatchRotation : MonoBehaviour
{
	// Token: 0x060035CB RID: 13771 RVA: 0x000534BA File Offset: 0x000516BA
	private void Start()
	{
		this.referenceTransform = Camera.main.transform;
		base.transform.forward = this.x0z(this.referenceTransform.forward);
	}

	// Token: 0x060035CC RID: 13772 RVA: 0x001439A4 File Offset: 0x00141BA4
	private void Update()
	{
		Vector3 lhs = this.x0z(base.transform.forward);
		Vector3 vector = this.x0z(this.referenceTransform.forward);
		float num = Vector3.Dot(lhs, vector);
		UIMatchRotation.State state = this.state;
		if (state != UIMatchRotation.State.Ready)
		{
			if (state != UIMatchRotation.State.Rotating)
			{
				return;
			}
			base.transform.forward = Vector3.Lerp(base.transform.forward, vector, Time.deltaTime * this.lerpSpeed);
			if (Vector3.Dot(base.transform.forward, vector) > 0.995f)
			{
				this.state = UIMatchRotation.State.Ready;
			}
		}
		else if (num < 1f - this.threshold)
		{
			this.state = UIMatchRotation.State.Rotating;
			return;
		}
	}

	// Token: 0x060035CD RID: 13773 RVA: 0x000534E8 File Offset: 0x000516E8
	private Vector3 x0z(Vector3 vector)
	{
		vector.y = 0f;
		return vector.normalized;
	}

	// Token: 0x04003852 RID: 14418
	[SerializeField]
	private Transform referenceTransform;

	// Token: 0x04003853 RID: 14419
	[SerializeField]
	private float threshold = 0.35f;

	// Token: 0x04003854 RID: 14420
	[SerializeField]
	private float lerpSpeed = 5f;

	// Token: 0x04003855 RID: 14421
	private UIMatchRotation.State state;

	// Token: 0x020008A9 RID: 2217
	private enum State
	{
		// Token: 0x04003857 RID: 14423
		Ready,
		// Token: 0x04003858 RID: 14424
		Rotating
	}
}
