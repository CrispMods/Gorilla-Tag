using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200020B RID: 523
public class TimeOfDayDependentAudio : MonoBehaviour, IGorillaSliceableSimple, IBuildValidation
{
	// Token: 0x06000C4B RID: 3147 RVA: 0x0009DC70 File Offset: 0x0009BE70
	private void Awake()
	{
		this.stepTime = 1f;
		if (this.myParticleSystem != null)
		{
			this.myEmissionModule = this.myParticleSystem.emission;
			this.startingEmissionRate = this.myEmissionModule.rateOverTime.constant;
		}
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x000389A4 File Offset: 0x00036BA4
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.FixedUpdate);
		base.StopAllCoroutines();
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x000389B3 File Offset: 0x00036BB3
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.FixedUpdate);
		base.StartCoroutine(this.UpdateTimeOfDay());
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x000389C9 File Offset: 0x00036BC9
	public void SliceUpdate()
	{
		this.isModified = false;
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x000389D2 File Offset: 0x00036BD2
	private IEnumerator UpdateTimeOfDay()
	{
		yield return 0;
		for (;;)
		{
			if (BetterDayNightManager.instance != null)
			{
				if (this.isModified)
				{
					this.positionMultiplier = this.positionMultiplierSet;
				}
				else
				{
					this.positionMultiplier = 1f;
				}
				if (this.myWeather == BetterDayNightManager.WeatherType.All || BetterDayNightManager.instance.CurrentWeather() == this.myWeather || BetterDayNightManager.instance.NextWeather() == this.myWeather)
				{
					if (!this.dependentStuff.activeSelf && (!this.includesAudio || this.dependentStuff != this.timeOfDayDependent))
					{
						this.dependentStuff.SetActive(true);
					}
					if (this.includesAudio)
					{
						if (this.timeOfDayDependent != null)
						{
							if (this.volumes[BetterDayNightManager.instance.currentTimeIndex] == 0f)
							{
								if (this.timeOfDayDependent.activeSelf)
								{
									this.timeOfDayDependent.SetActive(false);
								}
							}
							else if (!this.timeOfDayDependent.activeSelf)
							{
								this.timeOfDayDependent.SetActive(true);
							}
						}
						if (this.volumes[BetterDayNightManager.instance.currentTimeIndex] != this.audioSources[0].volume)
						{
							if (BetterDayNightManager.instance.currentLerp < 0.05f)
							{
								this.currentVolume = Mathf.Lerp(this.currentVolume, this.volumes[BetterDayNightManager.instance.currentTimeIndex], BetterDayNightManager.instance.currentLerp * 20f);
							}
							else
							{
								this.currentVolume = this.volumes[BetterDayNightManager.instance.currentTimeIndex];
							}
						}
					}
					if (this.myWeather == BetterDayNightManager.WeatherType.All || BetterDayNightManager.instance.CurrentWeather() == this.myWeather)
					{
						if (this.myWeather == BetterDayNightManager.WeatherType.All || BetterDayNightManager.instance.NextWeather() == this.myWeather)
						{
							if (this.myParticleSystem != null)
							{
								this.newRate = this.startingEmissionRate;
							}
							if (this.includesAudio && this.myParticleSystem != null)
							{
								this.currentVolume = Mathf.Lerp(this.volumes[BetterDayNightManager.instance.currentTimeIndex], this.volumes[(BetterDayNightManager.instance.currentTimeIndex + 1) % this.volumes.Length], BetterDayNightManager.instance.currentLerp);
							}
							else if (this.includesAudio)
							{
								if (BetterDayNightManager.instance.currentLerp < 0.05f)
								{
									this.currentVolume = Mathf.Lerp(this.currentVolume, this.volumes[BetterDayNightManager.instance.currentTimeIndex], BetterDayNightManager.instance.currentLerp * 20f);
								}
								else
								{
									this.currentVolume = this.volumes[BetterDayNightManager.instance.currentTimeIndex];
								}
							}
						}
						else
						{
							if (this.myParticleSystem != null)
							{
								this.newRate = ((BetterDayNightManager.instance.currentLerp < 0.5f) ? Mathf.Lerp(this.startingEmissionRate, 0f, BetterDayNightManager.instance.currentLerp * 2f) : 0f);
							}
							if (this.includesAudio)
							{
								this.currentVolume = ((BetterDayNightManager.instance.currentLerp < 0.5f) ? Mathf.Lerp(this.volumes[BetterDayNightManager.instance.currentTimeIndex], 0f, BetterDayNightManager.instance.currentLerp * 2f) : 0f);
							}
						}
					}
					else
					{
						if (this.myParticleSystem != null)
						{
							this.newRate = ((BetterDayNightManager.instance.currentLerp > 0.5f) ? Mathf.Lerp(0f, this.startingEmissionRate, (BetterDayNightManager.instance.currentLerp - 0.5f) * 2f) : 0f);
						}
						if (this.includesAudio)
						{
							this.currentVolume = ((BetterDayNightManager.instance.currentLerp > 0.5f) ? Mathf.Lerp(0f, this.volumes[(BetterDayNightManager.instance.currentTimeIndex + 1) % this.volumes.Length], (BetterDayNightManager.instance.currentLerp - 0.5f) * 2f) : 0f);
						}
					}
					if (this.myParticleSystem != null)
					{
						this.myEmissionModule = this.myParticleSystem.emission;
						this.myEmissionModule.rateOverTime = this.newRate;
					}
					if (this.includesAudio)
					{
						for (int i = 0; i < this.audioSources.Length; i++)
						{
							MusicSource component = this.audioSources[i].gameObject.GetComponent<MusicSource>();
							if (!(component != null) || !component.VolumeOverridden)
							{
								this.audioSources[i].volume = this.currentVolume * this.positionMultiplier;
								this.audioSources[i].enabled = (this.currentVolume != 0f);
							}
						}
					}
				}
				else if (this.dependentStuff.activeSelf)
				{
					this.dependentStuff.SetActive(false);
				}
			}
			yield return new WaitForSeconds(this.stepTime);
		}
		yield break;
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x0009DCC0 File Offset: 0x0009BEC0
	public bool BuildValidationCheck()
	{
		for (int i = 0; i < this.audioSources.Length; i++)
		{
			if (this.audioSources[i] == null)
			{
				Debug.LogError("audio source array contains null references", this);
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000E97 RID: 3735
	public AudioSource[] audioSources;

	// Token: 0x04000E98 RID: 3736
	public float[] volumes;

	// Token: 0x04000E99 RID: 3737
	public float currentVolume;

	// Token: 0x04000E9A RID: 3738
	public float stepTime;

	// Token: 0x04000E9B RID: 3739
	public BetterDayNightManager.WeatherType myWeather;

	// Token: 0x04000E9C RID: 3740
	public GameObject dependentStuff;

	// Token: 0x04000E9D RID: 3741
	public GameObject timeOfDayDependent;

	// Token: 0x04000E9E RID: 3742
	public bool includesAudio;

	// Token: 0x04000E9F RID: 3743
	public ParticleSystem myParticleSystem;

	// Token: 0x04000EA0 RID: 3744
	private float startingEmissionRate;

	// Token: 0x04000EA1 RID: 3745
	private int lastEmission;

	// Token: 0x04000EA2 RID: 3746
	private int nextEmission;

	// Token: 0x04000EA3 RID: 3747
	private ParticleSystem.MinMaxCurve newCurve;

	// Token: 0x04000EA4 RID: 3748
	private ParticleSystem.EmissionModule myEmissionModule;

	// Token: 0x04000EA5 RID: 3749
	private float newRate;

	// Token: 0x04000EA6 RID: 3750
	public float positionMultiplierSet;

	// Token: 0x04000EA7 RID: 3751
	public float positionMultiplier = 1f;

	// Token: 0x04000EA8 RID: 3752
	public bool isModified;
}
