using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class LongScarfSim : MonoBehaviour
{
	// Token: 0x0600096C RID: 2412 RVA: 0x00091CE4 File Offset: 0x0008FEE4
	private void Start()
	{
		this.clampToPlane.Normalize();
		this.velocityEstimator = base.GetComponent<GorillaVelocityEstimator>();
		this.baseLocalRotations = new Quaternion[this.gameObjects.Length];
		for (int i = 0; i < this.gameObjects.Length; i++)
		{
			this.baseLocalRotations[i] = this.gameObjects[i].transform.localRotation;
		}
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x00091D4C File Offset: 0x0008FF4C
	private void LateUpdate()
	{
		this.velocity *= this.drag;
		this.velocity.y = this.velocity.y - this.gravityStrength * Time.deltaTime;
		Vector3 position = base.transform.position;
		Vector3 a = this.lastCenterPos + this.velocity * Time.deltaTime;
		Vector3 vector = position + (a - position).normalized * this.centerOfMassLength;
		Vector3 vector2 = base.transform.InverseTransformPoint(vector);
		float num = Vector3.Dot(vector2, this.clampToPlane);
		if (num < 0f)
		{
			vector2 -= this.clampToPlane * num;
			vector = base.transform.TransformPoint(vector2);
		}
		Vector3 a2 = vector;
		this.velocity = (a2 - this.lastCenterPos) / Time.deltaTime;
		this.lastCenterPos = a2;
		float target = (float)(this.velocityEstimator.linearVelocity.IsLongerThan(this.speedThreshold) ? 1 : 0);
		this.currentBlend = Mathf.MoveTowards(this.currentBlend, target, this.blendAmountPerSecond * Time.deltaTime);
		Quaternion b = Quaternion.LookRotation(a2 - position);
		for (int i = 0; i < this.gameObjects.Length; i++)
		{
			Quaternion a3 = this.gameObjects[i].transform.parent.rotation * this.baseLocalRotations[i];
			this.gameObjects[i].transform.rotation = Quaternion.Lerp(a3, b, this.currentBlend);
		}
	}

	// Token: 0x04000B62 RID: 2914
	[SerializeField]
	private GameObject[] gameObjects;

	// Token: 0x04000B63 RID: 2915
	[SerializeField]
	private float speedThreshold = 1f;

	// Token: 0x04000B64 RID: 2916
	[SerializeField]
	private float blendAmountPerSecond = 1f;

	// Token: 0x04000B65 RID: 2917
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04000B66 RID: 2918
	private Quaternion[] baseLocalRotations;

	// Token: 0x04000B67 RID: 2919
	private float currentBlend;

	// Token: 0x04000B68 RID: 2920
	[SerializeField]
	private float centerOfMassLength;

	// Token: 0x04000B69 RID: 2921
	[SerializeField]
	private float gravityStrength;

	// Token: 0x04000B6A RID: 2922
	[SerializeField]
	private float drag;

	// Token: 0x04000B6B RID: 2923
	[SerializeField]
	private Vector3 clampToPlane;

	// Token: 0x04000B6C RID: 2924
	private Vector3 lastCenterPos;

	// Token: 0x04000B6D RID: 2925
	private Vector3 velocity;
}
