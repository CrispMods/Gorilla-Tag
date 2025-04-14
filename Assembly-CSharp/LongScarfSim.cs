using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200016D RID: 365
public class LongScarfSim : MonoBehaviour
{
	// Token: 0x0600091F RID: 2335 RVA: 0x000314A0 File Offset: 0x0002F6A0
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

	// Token: 0x06000920 RID: 2336 RVA: 0x00031508 File Offset: 0x0002F708
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

	// Token: 0x04000B1B RID: 2843
	[SerializeField]
	private GameObject[] gameObjects;

	// Token: 0x04000B1C RID: 2844
	[SerializeField]
	private float speedThreshold = 1f;

	// Token: 0x04000B1D RID: 2845
	[SerializeField]
	private float blendAmountPerSecond = 1f;

	// Token: 0x04000B1E RID: 2846
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04000B1F RID: 2847
	private Quaternion[] baseLocalRotations;

	// Token: 0x04000B20 RID: 2848
	private float currentBlend;

	// Token: 0x04000B21 RID: 2849
	[SerializeField]
	private float centerOfMassLength;

	// Token: 0x04000B22 RID: 2850
	[SerializeField]
	private float gravityStrength;

	// Token: 0x04000B23 RID: 2851
	[SerializeField]
	private float drag;

	// Token: 0x04000B24 RID: 2852
	[SerializeField]
	private Vector3 clampToPlane;

	// Token: 0x04000B25 RID: 2853
	private Vector3 lastCenterPos;

	// Token: 0x04000B26 RID: 2854
	private Vector3 velocity;
}
