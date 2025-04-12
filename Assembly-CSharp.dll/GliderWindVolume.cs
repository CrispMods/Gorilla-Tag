using System;
using UnityEngine;

// Token: 0x020007BE RID: 1982
public class GliderWindVolume : MonoBehaviour
{
	// Token: 0x060030E1 RID: 12513 RVA: 0x0004F993 File Offset: 0x0004DB93
	public void SetProperties(float speed, float accel, AnimationCurve svaCurve, Vector3 windDirection)
	{
		this.maxSpeed = speed;
		this.maxAccel = accel;
		this.speedVsAccelCurve.CopyFrom(svaCurve);
		this.localWindDirection = windDirection;
	}

	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x060030E2 RID: 12514 RVA: 0x0004F9B7 File Offset: 0x0004DBB7
	public Vector3 WindDirection
	{
		get
		{
			return base.transform.TransformDirection(this.localWindDirection);
		}
	}

	// Token: 0x060030E3 RID: 12515 RVA: 0x00130D64 File Offset: 0x0012EF64
	public Vector3 GetAccelFromVelocity(Vector3 velocity)
	{
		Vector3 windDirection = this.WindDirection;
		float time = Mathf.Clamp(Vector3.Dot(velocity, windDirection), -this.maxSpeed, this.maxSpeed) / this.maxSpeed;
		float d = this.speedVsAccelCurve.Evaluate(time) * this.maxAccel;
		return windDirection * d;
	}

	// Token: 0x04003524 RID: 13604
	[SerializeField]
	private float maxSpeed = 30f;

	// Token: 0x04003525 RID: 13605
	[SerializeField]
	private float maxAccel = 15f;

	// Token: 0x04003526 RID: 13606
	[SerializeField]
	private AnimationCurve speedVsAccelCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

	// Token: 0x04003527 RID: 13607
	[SerializeField]
	private Vector3 localWindDirection = Vector3.up;
}
