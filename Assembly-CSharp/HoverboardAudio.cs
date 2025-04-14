using System;
using UnityEngine;

// Token: 0x020005C2 RID: 1474
public class HoverboardAudio : MonoBehaviour
{
	// Token: 0x0600249B RID: 9371 RVA: 0x000B65E5 File Offset: 0x000B47E5
	private void Start()
	{
		this.Stop();
	}

	// Token: 0x0600249C RID: 9372 RVA: 0x000B65ED File Offset: 0x000B47ED
	public void PlayTurnSound(float angle)
	{
		if (Time.time > this.turnSoundCooldownUntilTimestamp && angle > this.minAngleDeltaForTurnSound)
		{
			this.turnSoundCooldownUntilTimestamp = Time.time + this.turnSoundCooldownDuration;
			this.turnSounds.Play();
		}
	}

	// Token: 0x0600249D RID: 9373 RVA: 0x000B6624 File Offset: 0x000B4824
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

	// Token: 0x0600249E RID: 9374 RVA: 0x000B66E0 File Offset: 0x000B48E0
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

	// Token: 0x040028C0 RID: 10432
	[SerializeField]
	private AudioSource hum1;

	// Token: 0x040028C1 RID: 10433
	[SerializeField]
	private SoundBankPlayer turnSounds;

	// Token: 0x040028C2 RID: 10434
	private bool didInitHum1BaseVolume;

	// Token: 0x040028C3 RID: 10435
	private float hum1BaseVolume;

	// Token: 0x040028C4 RID: 10436
	[SerializeField]
	private float fadeSpeed;

	// Token: 0x040028C5 RID: 10437
	[SerializeField]
	private AudioAnimator windRushAnimator;

	// Token: 0x040028C6 RID: 10438
	[SerializeField]
	private AudioAnimator motorAnimator;

	// Token: 0x040028C7 RID: 10439
	[SerializeField]
	private AudioAnimator grindAnimator;

	// Token: 0x040028C8 RID: 10440
	[SerializeField]
	private float turnSoundCooldownDuration;

	// Token: 0x040028C9 RID: 10441
	[SerializeField]
	private float minAngleDeltaForTurnSound;

	// Token: 0x040028CA RID: 10442
	private float turnSoundCooldownUntilTimestamp;
}
