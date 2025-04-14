using System;
using UnityEngine;

// Token: 0x020007BD RID: 1981
public class GliderWindVolume : MonoBehaviour
{
	// Token: 0x060030D9 RID: 12505 RVA: 0x000ED263 File Offset: 0x000EB463
	public void SetProperties(float speed, float accel, AnimationCurve svaCurve, Vector3 windDirection)
	{
		this.maxSpeed = speed;
		this.maxAccel = accel;
		this.speedVsAccelCurve.CopyFrom(svaCurve);
		this.localWindDirection = windDirection;
	}

	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x060030DA RID: 12506 RVA: 0x000ED287 File Offset: 0x000EB487
	public Vector3 WindDirection
	{
		get
		{
			return base.transform.TransformDirection(this.localWindDirection);
		}
	}

	// Token: 0x060030DB RID: 12507 RVA: 0x000ED29C File Offset: 0x000EB49C
	public Vector3 GetAccelFromVelocity(Vector3 velocity)
	{
		Vector3 windDirection = this.WindDirection;
		float time = Mathf.Clamp(Vector3.Dot(velocity, windDirection), -this.maxSpeed, this.maxSpeed) / this.maxSpeed;
		float d = this.speedVsAccelCurve.Evaluate(time) * this.maxAccel;
		return windDirection * d;
	}

	// Token: 0x0400351E RID: 13598
	[SerializeField]
	private float maxSpeed = 30f;

	// Token: 0x0400351F RID: 13599
	[SerializeField]
	private float maxAccel = 15f;

	// Token: 0x04003520 RID: 13600
	[SerializeField]
	private AnimationCurve speedVsAccelCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

	// Token: 0x04003521 RID: 13601
	[SerializeField]
	private Vector3 localWindDirection = Vector3.up;
}
