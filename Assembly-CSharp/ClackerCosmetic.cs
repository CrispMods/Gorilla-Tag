using System;
using Cinemachine.Utility;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class ClackerCosmetic : MonoBehaviour
{
	// Token: 0x0600088D RID: 2189 RVA: 0x0008E6D8 File Offset: 0x0008C8D8
	private void Start()
	{
		this.LocalRotationAxis = this.LocalRotationAxis.normalized;
		this.arm1.parent = this;
		this.arm2.parent = this;
		this.arm1.transform = this.clackerArm1;
		this.arm2.transform = this.clackerArm2;
		this.arm1.lastWorldPosition = this.clackerArm1.transform.TransformPoint(this.LocalCenterOfMass);
		this.arm2.lastWorldPosition = this.clackerArm2.transform.TransformPoint(this.LocalCenterOfMass);
		this.centerOfMassRadius = this.LocalCenterOfMass.magnitude;
		this.RotationCorrection = Quaternion.Euler(this.RotationCorrectionEuler);
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x0008E794 File Offset: 0x0008C994
	private void Update()
	{
		Vector3 lastWorldPosition = this.arm1.lastWorldPosition;
		this.arm1.UpdateArm();
		this.arm2.UpdateArm();
		ref Vector3 eulerAngles = this.clackerArm1.transform.eulerAngles;
		Vector3 eulerAngles2 = this.clackerArm2.transform.eulerAngles;
		Mathf.DeltaAngle(eulerAngles.y, eulerAngles2.y);
		if ((this.arm1.lastWorldPosition - this.arm2.lastWorldPosition).IsShorterThan(this.collisionDistance))
		{
			float sqrMagnitude = (this.arm1.velocity - this.arm2.velocity).sqrMagnitude;
			if (this.parentHoldable.InHand())
			{
				if (sqrMagnitude > this.heavyClackSpeed * this.heavyClackSpeed)
				{
					this.heavyClackAudio.Play();
				}
				else if (sqrMagnitude > this.mediumClackSpeed * this.mediumClackSpeed)
				{
					this.mediumClackAudio.Play();
				}
				else if (sqrMagnitude > this.minimumClackSpeed * this.minimumClackSpeed)
				{
					this.lightClackAudio.Play();
				}
			}
			Vector3 a = (this.arm1.lastWorldPosition + this.arm2.lastWorldPosition) / 2f;
			Vector3 vector = (this.arm1.lastWorldPosition - this.arm2.lastWorldPosition).normalized * (this.collisionDistance + 0.001f) / 2f;
			Vector3 b = a + vector;
			Vector3 b2 = a - vector;
			if ((lastWorldPosition - b).IsLongerThan(lastWorldPosition - b2))
			{
				vector = -vector;
			}
			this.arm1.SetPosition(a + vector);
			this.arm2.SetPosition(a - vector);
			ref Vector3 ptr = ref this.arm1.velocity;
			Vector3 velocity = this.arm2.velocity;
			Vector3 velocity2 = this.arm1.velocity;
			ptr = velocity;
			this.arm2.velocity = velocity2;
			Vector3 b3 = (this.arm1.lastWorldPosition - this.arm2.lastWorldPosition).normalized * this.pushApartStrength * Mathf.Sqrt(sqrMagnitude);
			this.arm1.velocity = this.arm1.velocity + b3;
			this.arm2.velocity = this.arm2.velocity - b3;
		}
	}

	// Token: 0x040009F9 RID: 2553
	[SerializeField]
	private TransferrableObject parentHoldable;

	// Token: 0x040009FA RID: 2554
	[SerializeField]
	private Transform clackerArm1;

	// Token: 0x040009FB RID: 2555
	[SerializeField]
	private Transform clackerArm2;

	// Token: 0x040009FC RID: 2556
	[SerializeField]
	private Vector3 LocalCenterOfMass;

	// Token: 0x040009FD RID: 2557
	[SerializeField]
	private Vector3 LocalRotationAxis;

	// Token: 0x040009FE RID: 2558
	[SerializeField]
	private Vector3 RotationCorrectionEuler;

	// Token: 0x040009FF RID: 2559
	[SerializeField]
	private float drag;

	// Token: 0x04000A00 RID: 2560
	[SerializeField]
	private float gravity;

	// Token: 0x04000A01 RID: 2561
	[SerializeField]
	private float localFriction;

	// Token: 0x04000A02 RID: 2562
	[SerializeField]
	private float minimumClackSpeed;

	// Token: 0x04000A03 RID: 2563
	[SerializeField]
	private SoundBankPlayer lightClackAudio;

	// Token: 0x04000A04 RID: 2564
	[SerializeField]
	private float mediumClackSpeed;

	// Token: 0x04000A05 RID: 2565
	[SerializeField]
	private SoundBankPlayer mediumClackAudio;

	// Token: 0x04000A06 RID: 2566
	[SerializeField]
	private float heavyClackSpeed;

	// Token: 0x04000A07 RID: 2567
	[SerializeField]
	private SoundBankPlayer heavyClackAudio;

	// Token: 0x04000A08 RID: 2568
	[SerializeField]
	private float collisionDistance;

	// Token: 0x04000A09 RID: 2569
	private float centerOfMassRadius;

	// Token: 0x04000A0A RID: 2570
	[SerializeField]
	private float pushApartStrength;

	// Token: 0x04000A0B RID: 2571
	private ClackerCosmetic.PerArmData arm1;

	// Token: 0x04000A0C RID: 2572
	private ClackerCosmetic.PerArmData arm2;

	// Token: 0x04000A0D RID: 2573
	private Quaternion RotationCorrection;

	// Token: 0x0200014D RID: 333
	private struct PerArmData
	{
		// Token: 0x06000890 RID: 2192 RVA: 0x0008EA20 File Offset: 0x0008CC20
		public void UpdateArm()
		{
			Vector3 target = this.transform.TransformPoint(this.parent.LocalCenterOfMass);
			Vector3 a = this.lastWorldPosition + this.velocity * Time.deltaTime * this.parent.drag;
			Vector3 vector = this.transform.parent.TransformDirection(this.parent.LocalRotationAxis);
			Vector3 vector2 = this.transform.position + (a - this.transform.position).ProjectOntoPlane(vector).normalized * this.parent.centerOfMassRadius;
			vector2 = Vector3.MoveTowards(vector2, target, this.parent.localFriction * Time.deltaTime);
			this.velocity = (vector2 - this.lastWorldPosition) / Time.deltaTime;
			this.velocity += Vector3.down * this.parent.gravity * Time.deltaTime;
			this.lastWorldPosition = vector2;
			this.transform.rotation = Quaternion.LookRotation(vector, vector2 - this.transform.position) * this.parent.RotationCorrection;
			this.lastWorldPosition = this.transform.TransformPoint(this.parent.LocalCenterOfMass);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0008EB88 File Offset: 0x0008CD88
		public void SetPosition(Vector3 newPosition)
		{
			Vector3 forward = this.transform.parent.TransformDirection(this.parent.LocalRotationAxis);
			this.transform.rotation = Quaternion.LookRotation(forward, newPosition - this.transform.position) * this.parent.RotationCorrection;
			this.lastWorldPosition = this.transform.TransformPoint(this.parent.LocalCenterOfMass);
		}

		// Token: 0x04000A0E RID: 2574
		public ClackerCosmetic parent;

		// Token: 0x04000A0F RID: 2575
		public Transform transform;

		// Token: 0x04000A10 RID: 2576
		public Vector3 velocity;

		// Token: 0x04000A11 RID: 2577
		public Vector3 lastWorldPosition;
	}
}
