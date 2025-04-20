using System;
using UnityEngine;

// Token: 0x020003F8 RID: 1016
public class ManipulatableSlider : ManipulatableObject
{
	// Token: 0x060018CF RID: 6351 RVA: 0x00040C3B File Offset: 0x0003EE3B
	private void Awake()
	{
		this.localSpace = base.transform.worldToLocalMatrix;
		this.startingPos = base.transform.localPosition;
	}

	// Token: 0x060018D0 RID: 6352 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void OnStartManipulation(GameObject grabbingHand)
	{
	}

	// Token: 0x060018D1 RID: 6353 RVA: 0x00040C5F File Offset: 0x0003EE5F
	protected override void OnStopManipulation(GameObject releasingHand, Vector3 releaseVelocity)
	{
		if (this.applyReleaseVelocity)
		{
			this.velocity = this.localSpace.MultiplyVector(releaseVelocity);
		}
	}

	// Token: 0x060018D2 RID: 6354 RVA: 0x000CD6A8 File Offset: 0x000CB8A8
	protected override bool ShouldHandDetach(GameObject hand)
	{
		Vector3 position = base.transform.position;
		Vector3 position2 = hand.transform.position;
		return Vector3.SqrMagnitude(position - position2) > this.breakDistance * this.breakDistance;
	}

	// Token: 0x060018D3 RID: 6355 RVA: 0x000CD6E8 File Offset: 0x000CB8E8
	protected override void OnHeldUpdate(GameObject hand)
	{
		Vector3 vector = this.localSpace.MultiplyPoint3x4(hand.transform.position);
		vector.x = Mathf.Clamp(vector.x, this.minXOffset, this.maxXOffset);
		vector.y = Mathf.Clamp(vector.y, this.minYOffset, this.maxYOffset);
		vector.z = Mathf.Clamp(vector.z, this.minZOffset, this.maxZOffset);
		vector += this.startingPos;
		base.transform.localPosition = vector;
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x000CD780 File Offset: 0x000CB980
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

	// Token: 0x060018D5 RID: 6357 RVA: 0x000CD934 File Offset: 0x000CBB34
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

	// Token: 0x060018D6 RID: 6358 RVA: 0x00040C7B File Offset: 0x0003EE7B
	public float GetProgressX()
	{
		return ((base.transform.localPosition - this.startingPos).x - this.minXOffset) / (this.maxXOffset - this.minXOffset);
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x00040CAD File Offset: 0x0003EEAD
	public float GetProgressY()
	{
		return ((base.transform.localPosition - this.startingPos).y - this.minYOffset) / (this.maxYOffset - this.minYOffset);
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x00040CDF File Offset: 0x0003EEDF
	public float GetProgressZ()
	{
		return ((base.transform.localPosition - this.startingPos).z - this.minZOffset) / (this.maxZOffset - this.minZOffset);
	}

	// Token: 0x04001B6D RID: 7021
	public float breakDistance = 0.2f;

	// Token: 0x04001B6E RID: 7022
	public float maxXOffset;

	// Token: 0x04001B6F RID: 7023
	public float minXOffset;

	// Token: 0x04001B70 RID: 7024
	public float maxYOffset;

	// Token: 0x04001B71 RID: 7025
	public float minYOffset;

	// Token: 0x04001B72 RID: 7026
	public float maxZOffset;

	// Token: 0x04001B73 RID: 7027
	public float minZOffset;

	// Token: 0x04001B74 RID: 7028
	public bool applyReleaseVelocity;

	// Token: 0x04001B75 RID: 7029
	public float releaseDrag = 1f;

	// Token: 0x04001B76 RID: 7030
	private Matrix4x4 localSpace;

	// Token: 0x04001B77 RID: 7031
	private Vector3 startingPos;

	// Token: 0x04001B78 RID: 7032
	private Vector3 velocity;
}
