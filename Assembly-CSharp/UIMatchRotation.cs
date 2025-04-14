using System;
using UnityEngine;

// Token: 0x0200088C RID: 2188
public class UIMatchRotation : MonoBehaviour
{
	// Token: 0x060034FF RID: 13567 RVA: 0x000FD011 File Offset: 0x000FB211
	private void Start()
	{
		this.referenceTransform = Camera.main.transform;
		base.transform.forward = this.x0z(this.referenceTransform.forward);
	}

	// Token: 0x06003500 RID: 13568 RVA: 0x000FD040 File Offset: 0x000FB240
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

	// Token: 0x06003501 RID: 13569 RVA: 0x000FD0E4 File Offset: 0x000FB2E4
	private Vector3 x0z(Vector3 vector)
	{
		vector.y = 0f;
		return vector.normalized;
	}

	// Token: 0x04003792 RID: 14226
	[SerializeField]
	private Transform referenceTransform;

	// Token: 0x04003793 RID: 14227
	[SerializeField]
	private float threshold = 0.35f;

	// Token: 0x04003794 RID: 14228
	[SerializeField]
	private float lerpSpeed = 5f;

	// Token: 0x04003795 RID: 14229
	private UIMatchRotation.State state;

	// Token: 0x0200088D RID: 2189
	private enum State
	{
		// Token: 0x04003797 RID: 14231
		Ready,
		// Token: 0x04003798 RID: 14232
		Rotating
	}
}
