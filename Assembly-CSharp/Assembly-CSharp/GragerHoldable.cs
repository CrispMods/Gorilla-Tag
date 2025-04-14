using System;
using Cinemachine.Utility;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class GragerHoldable : MonoBehaviour
{
	// Token: 0x06000904 RID: 2308 RVA: 0x00030DAC File Offset: 0x0002EFAC
	private void Start()
	{
		this.LocalRotationAxis = this.LocalRotationAxis.normalized;
		this.lastWorldPosition = base.transform.TransformPoint(this.LocalCenterOfMass);
		this.lastClackParentLocalPosition = base.transform.parent.InverseTransformPoint(this.lastWorldPosition);
		this.centerOfMassRadius = this.LocalCenterOfMass.magnitude;
		this.RotationCorrection = Quaternion.Euler(this.RotationCorrectionEuler);
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x00030E20 File Offset: 0x0002F020
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
			this.clackAudio.GTPlayOneShot(this.allClacks[Random.Range(0, this.allClacks.Length)], 1f);
			this.lastClackParentLocalPosition = a2;
		}
	}

	// Token: 0x04000AE7 RID: 2791
	[SerializeField]
	private Vector3 LocalCenterOfMass;

	// Token: 0x04000AE8 RID: 2792
	[SerializeField]
	private Vector3 LocalRotationAxis;

	// Token: 0x04000AE9 RID: 2793
	[SerializeField]
	private Vector3 RotationCorrectionEuler;

	// Token: 0x04000AEA RID: 2794
	[SerializeField]
	private float drag;

	// Token: 0x04000AEB RID: 2795
	[SerializeField]
	private float gravity;

	// Token: 0x04000AEC RID: 2796
	[SerializeField]
	private float localFriction;

	// Token: 0x04000AED RID: 2797
	[SerializeField]
	private float distancePerClack;

	// Token: 0x04000AEE RID: 2798
	[SerializeField]
	private AudioSource clackAudio;

	// Token: 0x04000AEF RID: 2799
	[SerializeField]
	private AudioClip[] allClacks;

	// Token: 0x04000AF0 RID: 2800
	private float centerOfMassRadius;

	// Token: 0x04000AF1 RID: 2801
	private Vector3 velocity;

	// Token: 0x04000AF2 RID: 2802
	private Vector3 lastWorldPosition;

	// Token: 0x04000AF3 RID: 2803
	private Vector3 lastClackParentLocalPosition;

	// Token: 0x04000AF4 RID: 2804
	private Quaternion RotationCorrection;
}
