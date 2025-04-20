using System;
using UnityEngine;

// Token: 0x020007D5 RID: 2005
public class GliderWindVolume : MonoBehaviour
{
	// Token: 0x0600318B RID: 12683 RVA: 0x00050D95 File Offset: 0x0004EF95
	public void SetProperties(float speed, float accel, AnimationCurve svaCurve, Vector3 windDirection)
	{
		this.maxSpeed = speed;
		this.maxAccel = accel;
		this.speedVsAccelCurve.CopyFrom(svaCurve);
		this.localWindDirection = windDirection;
	}

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x0600318C RID: 12684 RVA: 0x00050DB9 File Offset: 0x0004EFB9
	public Vector3 WindDirection
	{
		get
		{
			return base.transform.TransformDirection(this.localWindDirection);
		}
	}

	// Token: 0x0600318D RID: 12685 RVA: 0x00135F84 File Offset: 0x00134184
	public Vector3 GetAccelFromVelocity(Vector3 velocity)
	{
		Vector3 windDirection = this.WindDirection;
		float time = Mathf.Clamp(Vector3.Dot(velocity, windDirection), -this.maxSpeed, this.maxSpeed) / this.maxSpeed;
		float d = this.speedVsAccelCurve.Evaluate(time) * this.maxAccel;
		return windDirection * d;
	}

	// Token: 0x040035C8 RID: 13768
	[SerializeField]
	private float maxSpeed = 30f;

	// Token: 0x040035C9 RID: 13769
	[SerializeField]
	private float maxAccel = 15f;

	// Token: 0x040035CA RID: 13770
	[SerializeField]
	private AnimationCurve speedVsAccelCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

	// Token: 0x040035CB RID: 13771
	[SerializeField]
	private Vector3 localWindDirection = Vector3.up;
}
