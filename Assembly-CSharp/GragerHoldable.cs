using System;
using Cinemachine.Utility;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000173 RID: 371
public class GragerHoldable : MonoBehaviour
{
	// Token: 0x0600094F RID: 2383 RVA: 0x000913B8 File Offset: 0x0008F5B8
	private void Start()
	{
		this.LocalRotationAxis = this.LocalRotationAxis.normalized;
		this.lastWorldPosition = base.transform.TransformPoint(this.LocalCenterOfMass);
		this.lastClackParentLocalPosition = base.transform.parent.InverseTransformPoint(this.lastWorldPosition);
		this.centerOfMassRadius = this.LocalCenterOfMass.magnitude;
		this.RotationCorrection = Quaternion.Euler(this.RotationCorrectionEuler);
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0009142C File Offset: 0x0008F62C
	private void Update()
	{
		Vector3 target = base.transform.TransformPoint(this.LocalCenterOfMass);
		Vector3 a = this.lastWorldPosition + this.velocity * Time.deltaTime * this.drag;
		Vector3 vector = base.transform.parent.TransformDirection(this.LocalRotationAxis);
		Vector3 vector2 = base.transform.position + (a - base.transform.position).ProjectOntoPlane(vector).normalized * this.centerOfMassRadius;
		vector2 = Vector3.MoveTowards(vector2, target, this.localFriction * Time.deltaTime);
		this.velocity = (vector2 - this.lastWorldPosition) / Time.deltaTime;
		this.velocity += Vector3.down * this.gravity * Time.deltaTime;
		this.lastWorldPosition = vector2;
		base.transform.rotation = Quaternion.LookRotation(vector2 - base.transform.position, vector) * this.RotationCorrection;
		Vector3 a2 = base.transform.parent.InverseTransformPoint(base.transform.TransformPoint(this.LocalCenterOfMass));
		if ((a2 - this.lastClackParentLocalPosition).IsLongerThan(this.distancePerClack))
		{
			this.clackAudio.GTPlayOneShot(this.allClacks[UnityEngine.Random.Range(0, this.allClacks.Length)], 1f);
			this.lastClackParentLocalPosition = a2;
		}
	}

	// Token: 0x04000B2D RID: 2861
	[SerializeField]
	private Vector3 LocalCenterOfMass;

	// Token: 0x04000B2E RID: 2862
	[SerializeField]
	private Vector3 LocalRotationAxis;

	// Token: 0x04000B2F RID: 2863
	[SerializeField]
	private Vector3 RotationCorrectionEuler;

	// Token: 0x04000B30 RID: 2864
	[SerializeField]
	private float drag;

	// Token: 0x04000B31 RID: 2865
	[SerializeField]
	private float gravity;

	// Token: 0x04000B32 RID: 2866
	[SerializeField]
	private float localFriction;

	// Token: 0x04000B33 RID: 2867
	[SerializeField]
	private float distancePerClack;

	// Token: 0x04000B34 RID: 2868
	[SerializeField]
	private AudioSource clackAudio;

	// Token: 0x04000B35 RID: 2869
	[SerializeField]
	private AudioClip[] allClacks;

	// Token: 0x04000B36 RID: 2870
	private float centerOfMassRadius;

	// Token: 0x04000B37 RID: 2871
	private Vector3 velocity;

	// Token: 0x04000B38 RID: 2872
	private Vector3 lastWorldPosition;

	// Token: 0x04000B39 RID: 2873
	private Vector3 lastClackParentLocalPosition;

	// Token: 0x04000B3A RID: 2874
	private Quaternion RotationCorrection;
}
