using System;
using UnityEngine;

// Token: 0x020005C3 RID: 1475
public class HoverboardAudio : MonoBehaviour
{
	// Token: 0x060024A3 RID: 9379 RVA: 0x00047CEB File Offset: 0x00045EEB
	private void Start()
	{
		this.Stop();
	}

	// Token: 0x060024A4 RID: 9380 RVA: 0x00047CF3 File Offset: 0x00045EF3
	public void PlayTurnSound(float angle)
	{
		if (Time.time > this.turnSoundCooldownUntilTimestamp && angle > this.minAngleDeltaForTurnSound)
		{
			this.turnSoundCooldownUntilTimestamp = Time.time + this.turnSoundCooldownDuration;
			this.turnSounds.Play();
		}
	}

	// Token: 0x060024A5 RID: 9381 RVA: 0x00101E3C File Offset: 0x0010003C
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

	// Token: 0x060024A6 RID: 9382 RVA: 0x00101EF8 File Offset: 0x001000F8
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

	// Token: 0x040028C6 RID: 10438
	[SerializeField]
	private AudioSource hum1;

	// Token: 0x040028C7 RID: 10439
	[SerializeField]
	private SoundBankPlayer turnSounds;

	// Token: 0x040028C8 RID: 10440
	private bool didInitHum1BaseVolume;

	// Token: 0x040028C9 RID: 10441
	private float hum1BaseVolume;

	// Token: 0x040028CA RID: 10442
	[SerializeField]
	private float fadeSpeed;

	// Token: 0x040028CB RID: 10443
	[SerializeField]
	private AudioAnimator windRushAnimator;

	// Token: 0x040028CC RID: 10444
	[SerializeField]
	private AudioAnimator motorAnimator;

	// Token: 0x040028CD RID: 10445
	[SerializeField]
	private AudioAnimator grindAnimator;

	// Token: 0x040028CE RID: 10446
	[SerializeField]
	private float turnSoundCooldownDuration;

	// Token: 0x040028CF RID: 10447
	[SerializeField]
	private float minAngleDeltaForTurnSound;

	// Token: 0x040028D0 RID: 10448
	private float turnSoundCooldownUntilTimestamp;
}
