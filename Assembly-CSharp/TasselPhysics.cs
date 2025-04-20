using System;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class TasselPhysics : MonoBehaviour
{
	// Token: 0x060009D9 RID: 2521 RVA: 0x00092D88 File Offset: 0x00090F88
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

	// Token: 0x060009DA RID: 2522 RVA: 0x00092DE0 File Offset: 0x00090FE0
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

	// Token: 0x04000BCB RID: 3019
	[SerializeField]
	private GameObject[] tasselInstances;

	// Token: 0x04000BCC RID: 3020
	[SerializeField]
	private Vector3 localCenterOfMass;

	// Token: 0x04000BCD RID: 3021
	[SerializeField]
	private float gravityStrength;

	// Token: 0x04000BCE RID: 3022
	[SerializeField]
	private float drag;

	// Token: 0x04000BCF RID: 3023
	[SerializeField]
	private bool LockXAxis;

	// Token: 0x04000BD0 RID: 3024
	private Vector3 lastCenterPos;

	// Token: 0x04000BD1 RID: 3025
	private Vector3 velocity;

	// Token: 0x04000BD2 RID: 3026
	private float centerOfMassLength;

	// Token: 0x04000BD3 RID: 3027
	private Quaternion rotCorrection;
}
