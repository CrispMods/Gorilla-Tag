using System;
using UnityEngine;

// Token: 0x02000152 RID: 338
public class CrankableToyCarDeployed : MonoBehaviour
{
	// Token: 0x0600089D RID: 2205 RVA: 0x0008ED0C File Offset: 0x0008CF0C
	public void Deploy(CrankableToyCarHoldable holdable, Vector3 launchPos, Quaternion launchRot, Vector3 releaseVel, float lifetime, bool isRemote = false)
	{
		this.holdable = holdable;
		holdable.OnCarDeployed();
		base.transform.position = launchPos;
		base.transform.rotation = launchRot;
		base.transform.localScale = holdable.transform.lossyScale;
		this.rb.velocity = releaseVel;
		this.startedAtTimestamp = Time.time;
		this.expiresAtTimestamp = Time.time + lifetime;
		this.isRemote = isRemote;
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x0008ED84 File Offset: 0x0008CF84
	private void Update()
	{
		if (!this.isRemote && Time.time > this.expiresAtTimestamp)
		{
			if (this.holdable != null)
			{
				this.holdable.OnCarReturned();
			}
			return;
		}
		if (!this.wheelDriver.hasCollision)
		{
			this.expiresAtTimestamp -= Time.deltaTime;
			if (!this.offGroundDrivingAudio.isPlaying)
			{
				this.offGroundDrivingAudio.Play();
				this.drivingAudio.Stop();
			}
		}
		else if (!this.drivingAudio.isPlaying)
		{
			this.drivingAudio.Play();
			this.offGroundDrivingAudio.Stop();
		}
		float time = Mathf.InverseLerp(this.startedAtTimestamp, this.expiresAtTimestamp, Time.time);
		float d = this.thrustCurve.Evaluate(time);
		this.wheelDriver.SetThrust(this.maxThrust * d);
	}

	// Token: 0x04000A26 RID: 2598
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x04000A27 RID: 2599
	[SerializeField]
	private FakeWheelDriver wheelDriver;

	// Token: 0x04000A28 RID: 2600
	[SerializeField]
	private Vector3 maxThrust;

	// Token: 0x04000A29 RID: 2601
	[SerializeField]
	private AnimationCurve thrustCurve;

	// Token: 0x04000A2A RID: 2602
	private float startedAtTimestamp;

	// Token: 0x04000A2B RID: 2603
	private float expiresAtTimestamp;

	// Token: 0x04000A2C RID: 2604
	private CrankableToyCarHoldable holdable;

	// Token: 0x04000A2D RID: 2605
	[SerializeField]
	private AudioSource drivingAudio;

	// Token: 0x04000A2E RID: 2606
	[SerializeField]
	private AudioSource offGroundDrivingAudio;

	// Token: 0x04000A2F RID: 2607
	private bool isRemote;
}
