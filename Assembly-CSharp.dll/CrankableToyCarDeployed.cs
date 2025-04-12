using System;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class CrankableToyCarDeployed : MonoBehaviour
{
	// Token: 0x0600085B RID: 2139 RVA: 0x0008C384 File Offset: 0x0008A584
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

	// Token: 0x0600085C RID: 2140 RVA: 0x0008C3FC File Offset: 0x0008A5FC
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

	// Token: 0x040009E4 RID: 2532
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x040009E5 RID: 2533
	[SerializeField]
	private FakeWheelDriver wheelDriver;

	// Token: 0x040009E6 RID: 2534
	[SerializeField]
	private Vector3 maxThrust;

	// Token: 0x040009E7 RID: 2535
	[SerializeField]
	private AnimationCurve thrustCurve;

	// Token: 0x040009E8 RID: 2536
	private float startedAtTimestamp;

	// Token: 0x040009E9 RID: 2537
	private float expiresAtTimestamp;

	// Token: 0x040009EA RID: 2538
	private CrankableToyCarHoldable holdable;

	// Token: 0x040009EB RID: 2539
	[SerializeField]
	private AudioSource drivingAudio;

	// Token: 0x040009EC RID: 2540
	[SerializeField]
	private AudioSource offGroundDrivingAudio;

	// Token: 0x040009ED RID: 2541
	private bool isRemote;
}
