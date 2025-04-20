using System;
using UnityEngine;

// Token: 0x020003F5 RID: 1013
public class TestManipulatableCube : ManipulatableObject
{
	// Token: 0x060018BC RID: 6332 RVA: 0x00040B94 File Offset: 0x0003ED94
	private void Awake()
	{
		this.localSpace = base.transform.worldToLocalMatrix;
		this.startingPos = base.transform.localPosition;
	}

	// Token: 0x060018BD RID: 6333 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void OnStartManipulation(GameObject grabbingHand)
	{
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x00040BB8 File Offset: 0x0003EDB8
	protected override void OnStopManipulation(GameObject releasingHand, Vector3 releaseVelocity)
	{
		if (this.applyReleaseVelocity)
		{
			this.velocity = this.localSpace.MultiplyVector(releaseVelocity);
		}
	}

	// Token: 0x060018BF RID: 6335 RVA: 0x000CD00C File Offset: 0x000CB20C
	protected override bool ShouldHandDetach(GameObject hand)
	{
		Vector3 position = base.transform.position;
		Vector3 position2 = hand.transform.position;
		return Vector3.SqrMagnitude(position - position2) > this.breakDistance * this.breakDistance;
	}

	// Token: 0x060018C0 RID: 6336 RVA: 0x000CD04C File Offset: 0x000CB24C
	protected override void OnHeldUpdate(GameObject hand)
	{
		Vector3 vector = this.localSpace.MultiplyPoint3x4(hand.transform.position);
		vector.x = Mathf.Clamp(vector.x, this.minXOffset, this.maxXOffset);
		vector.y = Mathf.Clamp(vector.y, this.minYOffset, this.maxYOffset);
		vector.z = Mathf.Clamp(vector.z, this.minZOffset, this.maxZOffset);
		vector += this.startingPos;
		base.transform.localPosition = vector;
	}

	// Token: 0x060018C1 RID: 6337 RVA: 0x000CD0E4 File Offset: 0x000CB2E4
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

	// Token: 0x060018C2 RID: 6338 RVA: 0x00040BD4 File Offset: 0x0003EDD4
	public Matrix4x4 GetLocalSpace()
	{
		return this.localSpace;
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x000CD298 File Offset: 0x000CB498
	public void SetCubeToSpecificPosition(Vector3 pos)
	{
		Vector3 vector = this.localSpace.MultiplyPoint3x4(pos);
		vector.x = Mathf.Clamp(vector.x, this.minXOffset, this.maxXOffset);
		vector.y = Mathf.Clamp(vector.y, this.minYOffset, this.maxYOffset);
		vector.z = Mathf.Clamp(vector.z, this.minZOffset, this.maxZOffset);
		vector += this.startingPos;
		base.transform.localPosition = vector;
	}

	// Token: 0x060018C4 RID: 6340 RVA: 0x000CD328 File Offset: 0x000CB528
	public void SetCubeToSpecificPosition(float x, float y, float z)
	{
		Vector3 vector = new Vector3(0f, 0f, 0f);
		vector.x = Mathf.Clamp(x, this.minXOffset, this.maxXOffset);
		vector.y = Mathf.Clamp(y, this.minYOffset, this.maxYOffset);
		vector.z = Mathf.Clamp(z, this.minZOffset, this.maxZOffset);
		vector += this.startingPos;
		base.transform.localPosition = vector;
	}

	// Token: 0x04001B53 RID: 6995
	public float breakDistance = 0.2f;

	// Token: 0x04001B54 RID: 6996
	public float maxXOffset;

	// Token: 0x04001B55 RID: 6997
	public float minXOffset;

	// Token: 0x04001B56 RID: 6998
	public float maxYOffset;

	// Token: 0x04001B57 RID: 6999
	public float minYOffset;

	// Token: 0x04001B58 RID: 7000
	public float maxZOffset;

	// Token: 0x04001B59 RID: 7001
	public float minZOffset;

	// Token: 0x04001B5A RID: 7002
	public bool applyReleaseVelocity;

	// Token: 0x04001B5B RID: 7003
	public float releaseDrag = 1f;

	// Token: 0x04001B5C RID: 7004
	private Matrix4x4 localSpace;

	// Token: 0x04001B5D RID: 7005
	private Vector3 startingPos;

	// Token: 0x04001B5E RID: 7006
	private Vector3 velocity;
}
