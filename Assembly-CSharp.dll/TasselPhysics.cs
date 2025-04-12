﻿using System;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class TasselPhysics : MonoBehaviour
{
	// Token: 0x0600098F RID: 2447 RVA: 0x00090494 File Offset: 0x0008E694
	private void Awake()
	{
		this.centerOfMassLength = this.localCenterOfMass.magnitude;
		if (this.LockXAxis)
		{
			this.rotCorrection = Quaternion.Inverse(Quaternion.LookRotation(Vector3.right, this.localCenterOfMass));
			return;
		}
		this.rotCorrection = Quaternion.Inverse(Quaternion.LookRotation(this.localCenterOfMass));
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x000904EC File Offset: 0x0008E6EC
	private void Update()
	{
		float y = base.transform.lossyScale.y;
		this.velocity *= this.drag;
		this.velocity.y = this.velocity.y - this.gravityStrength * y * Time.deltaTime;
		Vector3 position = base.transform.position;
		Vector3 a = this.lastCenterPos + this.velocity * Time.deltaTime;
		Vector3 a2 = position + (a - position).normalized * this.centerOfMassLength * y;
		this.velocity = (a2 - this.lastCenterPos) / Time.deltaTime;
		this.lastCenterPos = a2;
		if (this.LockXAxis)
		{
			foreach (GameObject gameObject in this.tasselInstances)
			{
				gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.right, a2 - position) * this.rotCorrection;
			}
			return;
		}
		foreach (GameObject gameObject2 in this.tasselInstances)
		{
			gameObject2.transform.rotation = Quaternion.LookRotation(a2 - position, gameObject2.transform.position - position) * this.rotCorrection;
		}
	}

	// Token: 0x04000B86 RID: 2950
	[SerializeField]
	private GameObject[] tasselInstances;

	// Token: 0x04000B87 RID: 2951
	[SerializeField]
	private Vector3 localCenterOfMass;

	// Token: 0x04000B88 RID: 2952
	[SerializeField]
	private float gravityStrength;

	// Token: 0x04000B89 RID: 2953
	[SerializeField]
	private float drag;

	// Token: 0x04000B8A RID: 2954
	[SerializeField]
	private bool LockXAxis;

	// Token: 0x04000B8B RID: 2955
	private Vector3 lastCenterPos;

	// Token: 0x04000B8C RID: 2956
	private Vector3 velocity;

	// Token: 0x04000B8D RID: 2957
	private float centerOfMassLength;

	// Token: 0x04000B8E RID: 2958
	private Quaternion rotCorrection;
}
