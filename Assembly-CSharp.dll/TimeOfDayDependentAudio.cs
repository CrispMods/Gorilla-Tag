using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000200 RID: 512
public class TimeOfDayDependentAudio : MonoBehaviour, IGorillaSliceableSimple, IBuildValidation
{
	// Token: 0x06000C02 RID: 3074 RVA: 0x0009B3E4 File Offset: 0x000995E4
	private void Awake()
	{
		this.stepTime = 1f;
		if (this.myParticleSystem != null)
		{
			this.myEmissionModule = this.myParticleSystem.emission;
			this.startingEmissionRate = this.myEmissionModule.rateOverTime.constant;
		}
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x000376E4 File Offset: 0x000358E4
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.FixedUpdate);
		base.StopAllCoroutines();
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x000376F3 File Offset: 0x000358F3
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.FixedUpdate);
		base.StartCoroutine(this.UpdateTimeOfDay());
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x00037709 File Offset: 0x00035909
	public void SliceUpdate()
	{
		this.isModified = false;
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x00037712 File Offset: 0x00035912
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

	// Token: 0x06000C07 RID: 3079 RVA: 0x0009B434 File Offset: 0x00099634
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

	// Token: 0x06000C09 RID: 3081 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000E52 RID: 3666
	public AudioSource[] audioSources;

	// Token: 0x04000E53 RID: 3667
	public float[] volumes;

	// Token: 0x04000E54 RID: 3668
	public float currentVolume;

	// Token: 0x04000E55 RID: 3669
	public float stepTime;

	// Token: 0x04000E56 RID: 3670
	public BetterDayNightManager.WeatherType myWeather;

	// Token: 0x04000E57 RID: 3671
	public GameObject dependentStuff;

	// Token: 0x04000E58 RID: 3672
	public GameObject timeOfDayDependent;

	// Token: 0x04000E59 RID: 3673
	public bool includesAudio;

	// Token: 0x04000E5A RID: 3674
	public ParticleSystem myParticleSystem;

	// Token: 0x04000E5B RID: 3675
	private float startingEmissionRate;

	// Token: 0x04000E5C RID: 3676
	private int lastEmission;

	// Token: 0x04000E5D RID: 3677
	private int nextEmission;

	// Token: 0x04000E5E RID: 3678
	private ParticleSystem.MinMaxCurve newCurve;

	// Token: 0x04000E5F RID: 3679
	private ParticleSystem.EmissionModule myEmissionModule;

	// Token: 0x04000E60 RID: 3680
	private float newRate;

	// Token: 0x04000E61 RID: 3681
	public float positionMultiplierSet;

	// Token: 0x04000E62 RID: 3682
	public float positionMultiplier = 1f;

	// Token: 0x04000E63 RID: 3683
	public bool isModified;
}
