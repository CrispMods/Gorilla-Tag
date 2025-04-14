using System;
using UnityEngine;

// Token: 0x020003EA RID: 1002
public class TestManipulatableCube : ManipulatableObject
{
	// Token: 0x06001872 RID: 6258 RVA: 0x00076E90 File Offset: 0x00075090
	private void Awake()
	{
		this.localSpace = base.transform.worldToLocalMatrix;
		this.startingPos = base.transform.localPosition;
	}

	// Token: 0x06001873 RID: 6259 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void OnStartManipulation(GameObject grabbingHand)
	{
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x00076EB4 File Offset: 0x000750B4
	protected override void OnStopManipulation(GameObject releasingHand, Vector3 releaseVelocity)
	{
		if (this.applyReleaseVelocity)
		{
			this.velocity = this.localSpace.MultiplyVector(releaseVelocity);
		}
	}

	// Token: 0x06001875 RID: 6261 RVA: 0x00076ED0 File Offset: 0x000750D0
	protected override bool ShouldHandDetach(GameObject hand)
	{
		Vector3 position = base.transform.position;
		Vector3 position2 = hand.transform.position;
		return Vector3.SqrMagnitude(position - position2) > this.breakDistance * this.breakDistance;
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x00076F10 File Offset: 0x00075110
	protected override void OnHeldUpdate(GameObject hand)
	{
		Vector3 vector = this.localSpace.MultiplyPoint3x4(hand.transform.position);
		vector.x = Mathf.Clamp(vector.x, this.minXOffset, this.maxXOffset);
		vector.y = Mathf.Clamp(vector.y, this.minYOffset, this.maxYOffset);
		vector.z = Mathf.Clamp(vector.z, this.minZOffset, this.maxZOffset);
		vector += this.startingPos;
		base.transform.localPosition = vector;
	}

	// Token: 0x06001877 RID: 6263 RVA: 0x00076FA8 File Offset: 0x000751A8
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

	// Token: 0x06001878 RID: 6264 RVA: 0x00077159 File Offset: 0x00075359
	public Matrix4x4 GetLocalSpace()
	{
		return this.localSpace;
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x00077164 File Offset: 0x00075364
	public void SetCubeToSpecificPosition(Vector3 pos)
	{
		Vector3 vector = this.localSpace.MultiplyPoint3x4(pos);
		vector.x = Mathf.Clamp(vector.x, this.minXOffset, this.maxXOffset);
		vector.y = Mathf.Clamp(vector.y, this.minYOffset, this.maxYOffset);
		vector.z = Mathf.Clamp(vector.z, this.minZOffset, this.maxZOffset);
		vector += this.startingPos;
		base.transform.localPosition = vector;
	}

	// Token: 0x0600187A RID: 6266 RVA: 0x000771F4 File Offset: 0x000753F4
	public void SetCubeToSpecificPosition(float x, float y, float z)
	{
		Vector3 vector = new Vector3(0f, 0f, 0f);
		vector.x = Mathf.Clamp(x, this.minXOffset, this.maxXOffset);
		vector.y = Mathf.Clamp(y, this.minYOffset, this.maxYOffset);
		vector.z = Mathf.Clamp(z, this.minZOffset, this.maxZOffset);
		vector += this.startingPos;
		base.transform.localPosition = vector;
	}

	// Token: 0x04001B0B RID: 6923
	public float breakDistance = 0.2f;

	// Token: 0x04001B0C RID: 6924
	public float maxXOffset;

	// Token: 0x04001B0D RID: 6925
	public float minXOffset;

	// Token: 0x04001B0E RID: 6926
	public float maxYOffset;

	// Token: 0x04001B0F RID: 6927
	public float minYOffset;

	// Token: 0x04001B10 RID: 6928
	public float maxZOffset;

	// Token: 0x04001B11 RID: 6929
	public float minZOffset;

	// Token: 0x04001B12 RID: 6930
	public bool applyReleaseVelocity;

	// Token: 0x04001B13 RID: 6931
	public float releaseDrag = 1f;

	// Token: 0x04001B14 RID: 6932
	private Matrix4x4 localSpace;

	// Token: 0x04001B15 RID: 6933
	private Vector3 startingPos;

	// Token: 0x04001B16 RID: 6934
	private Vector3 velocity;
}
