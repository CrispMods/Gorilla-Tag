﻿using System;
using Cinemachine.Utility;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000142 RID: 322
public class ClackerCosmetic : MonoBehaviour
{
	// Token: 0x0600084B RID: 2123 RVA: 0x0008BD50 File Offset: 0x00089F50
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

	// Token: 0x0600084C RID: 2124 RVA: 0x0008BE0C File Offset: 0x0008A00C
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

	// Token: 0x040009B7 RID: 2487
	[SerializeField]
	private TransferrableObject parentHoldable;

	// Token: 0x040009B8 RID: 2488
	[SerializeField]
	private Transform clackerArm1;

	// Token: 0x040009B9 RID: 2489
	[SerializeField]
	private Transform clackerArm2;

	// Token: 0x040009BA RID: 2490
	[SerializeField]
	private Vector3 LocalCenterOfMass;

	// Token: 0x040009BB RID: 2491
	[SerializeField]
	private Vector3 LocalRotationAxis;

	// Token: 0x040009BC RID: 2492
	[SerializeField]
	private Vector3 RotationCorrectionEuler;

	// Token: 0x040009BD RID: 2493
	[SerializeField]
	private float drag;

	// Token: 0x040009BE RID: 2494
	[SerializeField]
	private float gravity;

	// Token: 0x040009BF RID: 2495
	[SerializeField]
	private float localFriction;

	// Token: 0x040009C0 RID: 2496
	[SerializeField]
	private float minimumClackSpeed;

	// Token: 0x040009C1 RID: 2497
	[SerializeField]
	private SoundBankPlayer lightClackAudio;

	// Token: 0x040009C2 RID: 2498
	[SerializeField]
	private float mediumClackSpeed;

	// Token: 0x040009C3 RID: 2499
	[SerializeField]
	private SoundBankPlayer mediumClackAudio;

	// Token: 0x040009C4 RID: 2500
	[SerializeField]
	private float heavyClackSpeed;

	// Token: 0x040009C5 RID: 2501
	[SerializeField]
	private SoundBankPlayer heavyClackAudio;

	// Token: 0x040009C6 RID: 2502
	[SerializeField]
	private float collisionDistance;

	// Token: 0x040009C7 RID: 2503
	private float centerOfMassRadius;

	// Token: 0x040009C8 RID: 2504
	[SerializeField]
	private float pushApartStrength;

	// Token: 0x040009C9 RID: 2505
	private ClackerCosmetic.PerArmData arm1;

	// Token: 0x040009CA RID: 2506
	private ClackerCosmetic.PerArmData arm2;

	// Token: 0x040009CB RID: 2507
	private Quaternion RotationCorrection;

	// Token: 0x02000143 RID: 323
	private struct PerArmData
	{
		// Token: 0x0600084E RID: 2126 RVA: 0x0008C098 File Offset: 0x0008A298
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

		// Token: 0x0600084F RID: 2127 RVA: 0x0008C200 File Offset: 0x0008A400
		public void SetPosition(Vector3 newPosition)
		{
			Vector3 forward = this.transform.parent.TransformDirection(this.parent.LocalRotationAxis);
			this.transform.rotation = Quaternion.LookRotation(forward, newPosition - this.transform.position) * this.parent.RotationCorrection;
			this.lastWorldPosition = this.transform.TransformPoint(this.parent.LocalCenterOfMass);
		}

		// Token: 0x040009CC RID: 2508
		public ClackerCosmetic parent;

		// Token: 0x040009CD RID: 2509
		public Transform transform;

		// Token: 0x040009CE RID: 2510
		public Vector3 velocity;

		// Token: 0x040009CF RID: 2511
		public Vector3 lastWorldPosition;
	}
}
