using System;
using UnityEngine;

// Token: 0x020005D0 RID: 1488
public class HoverboardAudio : MonoBehaviour
{
	// Token: 0x060024FD RID: 9469 RVA: 0x00049106 File Offset: 0x00047306
	private void Start()
	{
		this.Stop();
	}

	// Token: 0x060024FE RID: 9470 RVA: 0x0004910E File Offset: 0x0004730E
	public void PlayTurnSound(float angle)
	{
		if (Time.time > this.turnSoundCooldownUntilTimestamp && angle > this.minAngleDeltaForTurnSound)
		{
			this.turnSoundCooldownUntilTimestamp = Time.time + this.turnSoundCooldownDuration;
			this.turnSounds.Play();
		}
	}

	// Token: 0x060024FF RID: 9471 RVA: 0x00104D20 File Offset: 0x00102F20
	public void UpdateAudioLoop(float speed, float airspeed, float strainLevel, float grindLevel)
	{
		this.motorAnimator.UpdateValue(speed, false);
		this.windRushAnimator.UpdateValue(airspeed, false);
		if (grindLevel > 0f)
		{
			this.grindAnimator.UpdatePitchAndVolume(speed, grindLevel + 0.5f, false);
		}
		else
		{
			this.grindAnimator.UpdatePitchAndVolume(0f, 0f, false);
		}
		strainLevel = Mathf.Clamp01(strainLevel * 10f);
		if (!this.didInitHum1BaseVolume)
		{
			this.hum1BaseVolume = this.hum1.volume;
			this.didInitHum1BaseVolume = true;
		}
		this.hum1.volume = Mathf.MoveTowards(this.hum1.volume, this.hum1BaseVolume * strainLevel, this.fadeSpeed * Time.deltaTime);
	}

	// Token: 0x06002500 RID: 9472 RVA: 0x00104DDC File Offset: 0x00102FDC
	public void Stop()
	{
		if (!this.didInitHum1BaseVolume)
		{
			this.hum1BaseVolume = this.hum1.volume;
			this.didInitHum1BaseVolume = true;
		}
		this.hum1.volume = 0f;
		this.windRushAnimator.UpdateValue(0f, true);
		this.motorAnimator.UpdateValue(0f, true);
		this.grindAnimator.UpdateValue(0f, true);
	}

	// Token: 0x0400291F RID: 10527
	[SerializeField]
	private AudioSource hum1;

	// Token: 0x04002920 RID: 10528
	[SerializeField]
	private SoundBankPlayer turnSounds;

	// Token: 0x04002921 RID: 10529
	private bool didInitHum1BaseVolume;

	// Token: 0x04002922 RID: 10530
	private float hum1BaseVolume;

	// Token: 0x04002923 RID: 10531
	[SerializeField]
	private float fadeSpeed;

	// Token: 0x04002924 RID: 10532
	[SerializeField]
	private AudioAnimator windRushAnimator;

	// Token: 0x04002925 RID: 10533
	[SerializeField]
	private AudioAnimator motorAnimator;

	// Token: 0x04002926 RID: 10534
	[SerializeField]
	private AudioAnimator grindAnimator;

	// Token: 0x04002927 RID: 10535
	[SerializeField]
	private float turnSoundCooldownDuration;

	// Token: 0x04002928 RID: 10536
	[SerializeField]
	private float minAngleDeltaForTurnSound;

	// Token: 0x04002929 RID: 10537
	private float turnSoundCooldownUntilTimestamp;
}
