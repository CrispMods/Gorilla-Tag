using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000151 RID: 337
public class DJScratchtable : MonoBehaviour
{
	// Token: 0x06000897 RID: 2199 RVA: 0x0002F1BF File Offset: 0x0002D3BF
	public void SetPlaying(bool playing)
	{
		this.isPlaying = playing;
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0002F1C8 File Offset: 0x0002D3C8
	private void OnTriggerStay(Collider collider)
	{
		if (!base.enabled)
		{
			return;
		}
		GorillaTriggerColliderHandIndicator componentInParent = collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>();
		if (componentInParent == null)
		{
			return;
		}
		Vector3 vector = (base.transform.parent.InverseTransformPoint(collider.transform.position) - base.transform.localPosition).WithY(0f);
		float target = Mathf.Atan2(vector.z, vector.x) * 57.29578f;
		if (this.isTouching)
		{
			base.transform.localRotation = Quaternion.LookRotation(vector) * this.firstTouchRotation;
			if (this.isPlaying)
			{
				float num = Mathf.DeltaAngle(this.lastScratchSoundAngle, target);
				if (num > this.scratchMinAngle)
				{
					if (Time.time > this.cantForwardScratchUntilTimestamp)
					{
						this.scratchPlayer.Play(ScratchSoundType.Forward, this.isLeft);
						this.cantForwardScratchUntilTimestamp = Time.time + this.scratchCooldown;
						this.lastScratchSoundAngle = target;
						GorillaTagger.Instance.StartVibration(componentInParent.isLeftHand, this.hapticStrength, this.hapticDuration);
					}
				}
				else if (num < -this.scratchMinAngle && Time.time > this.cantBackScratchUntilTimestamp)
				{
					this.scratchPlayer.Play(ScratchSoundType.Back, this.isLeft);
					this.cantBackScratchUntilTimestamp = Time.time + this.scratchCooldown;
					this.lastScratchSoundAngle = target;
					GorillaTagger.Instance.StartVibration(componentInParent.isLeftHand, this.hapticStrength, this.hapticDuration);
				}
			}
		}
		else
		{
			this.firstTouchRotation = Quaternion.Inverse(Quaternion.LookRotation(base.transform.InverseTransformPoint(collider.transform.position).WithY(0f)));
			if (this.isPlaying)
			{
				this.PauseTrack();
				this.scratchPlayer.Play(ScratchSoundType.Pause, this.isLeft);
				this.lastScratchSoundAngle = target;
				this.cantForwardScratchUntilTimestamp = Time.time + this.scratchCooldown;
				this.cantBackScratchUntilTimestamp = Time.time + this.scratchCooldown;
			}
		}
		this.isTouching = true;
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0002F3CC File Offset: 0x0002D5CC
	private void OnTriggerExit(Collider collider)
	{
		if (!base.enabled)
		{
			return;
		}
		if (collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() == null)
		{
			return;
		}
		if (this.isPlaying)
		{
			this.ResumeTrack();
			this.scratchPlayer.Play(ScratchSoundType.Resume, this.isLeft);
		}
		this.isTouching = false;
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0002F418 File Offset: 0x0002D618
	public void SelectTrack(int track)
	{
		this.lastSelectedTrack = track;
		if (track == 0)
		{
			this.turntableVisual.Stop();
			this.isPlaying = false;
		}
		else
		{
			this.turntableVisual.Run();
			this.isPlaying = true;
		}
		int num = track - 1;
		for (int i = 0; i < this.tracks.Length; i++)
		{
			if (num == i)
			{
				float time = (float)(PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time)) % this.trackDuration;
				this.tracks[i].Play();
				this.tracks[i].time = time;
			}
			else
			{
				this.tracks[i].Stop();
			}
		}
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x0002F4B8 File Offset: 0x0002D6B8
	public void PauseTrack()
	{
		for (int i = 0; i < this.tracks.Length; i++)
		{
			this.tracks[i].Stop();
		}
		this.pausedUntilTimestamp = Time.time + 1f;
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0002F4F6 File Offset: 0x0002D6F6
	public void ResumeTrack()
	{
		this.SelectTrack(this.lastSelectedTrack);
		this.pausedUntilTimestamp = 0f;
	}

	// Token: 0x04000A3C RID: 2620
	[SerializeField]
	private bool isLeft;

	// Token: 0x04000A3D RID: 2621
	[SerializeField]
	private DJScratchSoundPlayer scratchPlayer;

	// Token: 0x04000A3E RID: 2622
	[SerializeField]
	private float scratchCooldown;

	// Token: 0x04000A3F RID: 2623
	[SerializeField]
	private float scratchMinAngle;

	// Token: 0x04000A40 RID: 2624
	[SerializeField]
	private AudioSource[] tracks;

	// Token: 0x04000A41 RID: 2625
	[SerializeField]
	private CosmeticFan turntableVisual;

	// Token: 0x04000A42 RID: 2626
	[SerializeField]
	private float trackDuration;

	// Token: 0x04000A43 RID: 2627
	[SerializeField]
	private float hapticStrength;

	// Token: 0x04000A44 RID: 2628
	[SerializeField]
	private float hapticDuration;

	// Token: 0x04000A45 RID: 2629
	private int lastSelectedTrack;

	// Token: 0x04000A46 RID: 2630
	private bool isPlaying;

	// Token: 0x04000A47 RID: 2631
	private bool isTouching;

	// Token: 0x04000A48 RID: 2632
	private Quaternion firstTouchRotation;

	// Token: 0x04000A49 RID: 2633
	private float lastScratchSoundAngle;

	// Token: 0x04000A4A RID: 2634
	private float cantForwardScratchUntilTimestamp;

	// Token: 0x04000A4B RID: 2635
	private float cantBackScratchUntilTimestamp;

	// Token: 0x04000A4C RID: 2636
	private float pausedUntilTimestamp;
}
