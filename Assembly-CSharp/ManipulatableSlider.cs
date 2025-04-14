using System;
using UnityEngine;

// Token: 0x020003ED RID: 1005
public class ManipulatableSlider : ManipulatableObject
{
	// Token: 0x06001882 RID: 6274 RVA: 0x0007724E File Offset: 0x0007544E
	private void Awake()
	{
		this.localSpace = base.transform.worldToLocalMatrix;
		this.startingPos = base.transform.localPosition;
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void OnStartManipulation(GameObject grabbingHand)
	{
	}

	// Token: 0x06001884 RID: 6276 RVA: 0x00077272 File Offset: 0x00075472
	protected override void OnStopManipulation(GameObject releasingHand, Vector3 releaseVelocity)
	{
		if (this.applyReleaseVelocity)
		{
			this.velocity = this.localSpace.MultiplyVector(releaseVelocity);
		}
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x00077290 File Offset: 0x00075490
	protected override bool ShouldHandDetach(GameObject hand)
	{
		Vector3 position = base.transform.position;
		Vector3 position2 = hand.transform.position;
		return Vector3.SqrMagnitude(position - position2) > this.breakDistance * this.breakDistance;
	}

	// Token: 0x06001886 RID: 6278 RVA: 0x000772D0 File Offset: 0x000754D0
	protected override void OnHeldUpdate(GameObject hand)
	{
		Vector3 vector = this.localSpace.MultiplyPoint3x4(hand.transform.position);
		vector.x = Mathf.Clamp(vector.x, this.minXOffset, this.maxXOffset);
		vector.y = Mathf.Clamp(vector.y, this.minYOffset, this.maxYOffset);
		vector.z = Mathf.Clamp(vector.z, this.minZOffset, this.maxZOffset);
		vector += this.startingPos;
		base.transform.localPosition = vector;
	}

	// Token: 0x06001887 RID: 6279 RVA: 0x00077368 File Offset: 0x00075568
	protected override void OnReleasedUpdate()
	{
		if (this.velocity != Vector3.zero)
		{
			Vector3 vector = this.localSpace.MultiplyPoint(base.transform.position);
			vector += this.velocity * Time.deltaTime;
			if (vector.x < this.minXOffset)
			{
				vector.x = this.minXOffset;
				this.velocity.x = 0f;
			}
			else if (vector.x > this.maxXOffset)
			{
				vector.x = this.maxXOffset;
				this.velocity.x = 0f;
			}
			if (vector.y < this.minYOffset)
			{
				vector.y = this.minYOffset;
				this.velocity.y = 0f;
			}
			else if (vector.y > this.maxYOffset)
			{
				vector.y = this.maxYOffset;
				this.velocity.y = 0f;
			}
			if (vector.z < this.minZOffset)
			{
				vector.z = this.minZOffset;
				this.velocity.z = 0f;
			}
			else if (vector.z > this.maxZOffset)
			{
				vector.z = this.maxZOffset;
				this.velocity.z = 0f;
			}
			vector += this.startingPos;
			base.transform.localPosition = vector;
			this.velocity *= 1f - this.releaseDrag * Time.deltaTime;
			if (this.velocity.sqrMagnitude < 0.001f)
			{
				this.velocity = Vector3.zero;
			}
		}
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x0007751C File Offset: 0x0007571C
	public void SetProgress(float x, float y, float z)
	{
		x = Mathf.Clamp(x, 0f, 1f);
		y = Mathf.Clamp(y, 0f, 1f);
		z = Mathf.Clamp(z, 0f, 1f);
		Vector3 localPosition = this.startingPos;
		localPosition.x += Mathf.Lerp(this.minXOffset, this.maxXOffset, x);
		localPosition.y += Mathf.Lerp(this.minYOffset, this.maxYOffset, y);
		localPosition.z += Mathf.Lerp(this.minZOffset, this.maxZOffset, z);
		base.transform.localPosition = localPosition;
	}

	// Token: 0x06001889 RID: 6281 RVA: 0x000775C9 File Offset: 0x000757C9
	public float GetProgressX()
	{
		return ((base.transform.localPosition - this.startingPos).x - this.minXOffset) / (this.maxXOffset - this.minXOffset);
	}

	// Token: 0x0600188A RID: 6282 RVA: 0x000775FB File Offset: 0x000757FB
	public float GetProgressY()
	{
		return ((base.transform.localPosition - this.startingPos).y - this.minYOffset) / (this.maxYOffset - this.minYOffset);
	}

	// Token: 0x0600188B RID: 6283 RVA: 0x0007762D File Offset: 0x0007582D
	public float GetProgressZ()
	{
		return ((base.transform.localPosition - this.startingPos).z - this.minZOffset) / (this.maxZOffset - this.minZOffset);
	}

	// Token: 0x04001B24 RID: 6948
	public float breakDistance = 0.2f;

	// Token: 0x04001B25 RID: 6949
	public float maxXOffset;

	// Token: 0x04001B26 RID: 6950
	public float minXOffset;

	// Token: 0x04001B27 RID: 6951
	public float maxYOffset;

	// Token: 0x04001B28 RID: 6952
	public float minYOffset;

	// Token: 0x04001B29 RID: 6953
	public float maxZOffset;

	// Token: 0x04001B2A RID: 6954
	public float minZOffset;

	// Token: 0x04001B2B RID: 6955
	public bool applyReleaseVelocity;

	// Token: 0x04001B2C RID: 6956
	public float releaseDrag = 1f;

	// Token: 0x04001B2D RID: 6957
	private Matrix4x4 localSpace;

	// Token: 0x04001B2E RID: 6958
	private Vector3 startingPos;

	// Token: 0x04001B2F RID: 6959
	private Vector3 velocity;
}
