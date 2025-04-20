using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200015B RID: 347
public class DJScratchtable : MonoBehaviour
{
	// Token: 0x060008D9 RID: 2265 RVA: 0x0003650A File Offset: 0x0003470A
	public void SetPlaying(bool playing)
	{
		this.isPlaying = playing;
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0008FBA8 File Offset: 0x0008DDA8
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

	// Token: 0x060008DB RID: 2267 RVA: 0x0008FDAC File Offset: 0x0008DFAC
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

	// Token: 0x060008DC RID: 2268 RVA: 0x0008FDF8 File Offset: 0x0008DFF8
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

	// Token: 0x060008DD RID: 2269 RVA: 0x0008FE98 File Offset: 0x0008E098
	public void PauseTrack()
	{
		for (int i = 0; i < this.tracks.Length; i++)
		{
			this.tracks[i].Stop();
		}
		this.pausedUntilTimestamp = Time.time + 1f;
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00036513 File Offset: 0x00034713
	public void ResumeTrack()
	{
		this.SelectTrack(this.lastSelectedTrack);
		this.pausedUntilTimestamp = 0f;
	}

	// Token: 0x04000A7E RID: 2686
	[SerializeField]
	private bool isLeft;

	// Token: 0x04000A7F RID: 2687
	[SerializeField]
	private DJScratchSoundPlayer scratchPlayer;

	// Token: 0x04000A80 RID: 2688
	[SerializeField]
	private float scratchCooldown;

	// Token: 0x04000A81 RID: 2689
	[SerializeField]
	private float scratchMinAngle;

	// Token: 0x04000A82 RID: 2690
	[SerializeField]
	private AudioSource[] tracks;

	// Token: 0x04000A83 RID: 2691
	[SerializeField]
	private CosmeticFan turntableVisual;

	// Token: 0x04000A84 RID: 2692
	[SerializeField]
	private float trackDuration;

	// Token: 0x04000A85 RID: 2693
	[SerializeField]
	private float hapticStrength;

	// Token: 0x04000A86 RID: 2694
	[SerializeField]
	private float hapticDuration;

	// Token: 0x04000A87 RID: 2695
	private int lastSelectedTrack;

	// Token: 0x04000A88 RID: 2696
	private bool isPlaying;

	// Token: 0x04000A89 RID: 2697
	private bool isTouching;

	// Token: 0x04000A8A RID: 2698
	private Quaternion firstTouchRotation;

	// Token: 0x04000A8B RID: 2699
	private float lastScratchSoundAngle;

	// Token: 0x04000A8C RID: 2700
	private float cantForwardScratchUntilTimestamp;

	// Token: 0x04000A8D RID: 2701
	private float cantBackScratchUntilTimestamp;

	// Token: 0x04000A8E RID: 2702
	private float pausedUntilTimestamp;
}
