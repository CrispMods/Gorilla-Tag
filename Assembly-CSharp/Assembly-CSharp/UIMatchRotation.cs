using System;
using UnityEngine;

// Token: 0x0200088F RID: 2191
public class UIMatchRotation : MonoBehaviour
{
	// Token: 0x0600350B RID: 13579 RVA: 0x000FD5D9 File Offset: 0x000FB7D9
	private void Start()
	{
		this.referenceTransform = Camera.main.transform;
		base.transform.forward = this.x0z(this.referenceTransform.forward);
	}

	// Token: 0x0600350C RID: 13580 RVA: 0x000FD608 File Offset: 0x000FB808
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

	// Token: 0x0600350D RID: 13581 RVA: 0x000FD6AC File Offset: 0x000FB8AC
	private Vector3 x0z(Vector3 vector)
	{
		vector.y = 0f;
		return vector.normalized;
	}

	// Token: 0x040037A4 RID: 14244
	[SerializeField]
	private Transform referenceTransform;

	// Token: 0x040037A5 RID: 14245
	[SerializeField]
	private float threshold = 0.35f;

	// Token: 0x040037A6 RID: 14246
	[SerializeField]
	private float lerpSpeed = 5f;

	// Token: 0x040037A7 RID: 14247
	private UIMatchRotation.State state;

	// Token: 0x02000890 RID: 2192
	private enum State
	{
		// Token: 0x040037A9 RID: 14249
		Ready,
		// Token: 0x040037AA RID: 14250
		Rotating
	}
}
