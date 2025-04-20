using System;
using System.Collections;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000630 RID: 1584
public class BetterDayNightManager : MonoBehaviour, IGorillaSliceableSimple, ITimeOfDaySystem
{
	// Token: 0x06002746 RID: 10054 RVA: 0x0004ACD3 File Offset: 0x00048ED3
	public static void Register(PerSceneRenderData data)
	{
		BetterDayNightManager.allScenesRenderData.Add(data);
	}

	// Token: 0x06002747 RID: 10055 RVA: 0x0004ACE0 File Offset: 0x00048EE0
	public static void Unregister(PerSceneRenderData data)
	{
		BetterDayNightManager.allScenesRenderData.Remove(data);
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06002748 RID: 10056 RVA: 0x0004ACEE File Offset: 0x00048EEE
	// (set) Token: 0x06002749 RID: 10057 RVA: 0x0004ACF6 File Offset: 0x00048EF6
	public string currentTimeOfDay { get; private set; }

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x0600274A RID: 10058 RVA: 0x0004ACFF File Offset: 0x00048EFF
	public float NormalizedTimeOfDay
	{
		get
		{
			return Mathf.Clamp01((float)((this.baseSeconds + (double)Time.realtimeSinceStartup * this.timeMultiplier) % this.totalSeconds / this.totalSeconds));
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x0600274B RID: 10059 RVA: 0x0004AD29 File Offset: 0x00048F29
	double ITimeOfDaySystem.currentTimeInSeconds
	{
		get
		{
			return this.currentTime;
		}
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x0600274C RID: 10060 RVA: 0x0004AD31 File Offset: 0x00048F31
	double ITimeOfDaySystem.totalTimeInSeconds
	{
		get
		{
			return this.totalSeconds;
		}
	}

	// Token: 0x0600274D RID: 10061 RVA: 0x0010BF34 File Offset: 0x0010A134
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		if (BetterDayNightManager.instance == null)
		{
			BetterDayNightManager.instance = this;
		}
		else if (BetterDayNightManager.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.currentLerp = 0f;
		this.totalHours = 0.0;
		for (int i = 0; i < this.timeOfDayRange.Length; i++)
		{
			this.totalHours += this.timeOfDayRange[i];
		}
		this.totalSeconds = this.totalHours * 60.0 * 60.0;
		this.currentTimeIndex = 0;
		this.baseSeconds = 0.0;
		this.computerInit = false;
		this.randomNumberGenerator = new System.Random(this.mySeed);
		this.GenerateWeatherEventTimes();
		this.ChangeMaps(0, 1);
		base.StartCoroutine(this.UpdateTimeOfDay());
	}

	// Token: 0x0600274E RID: 10062 RVA: 0x0004AD39 File Offset: 0x00048F39
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		base.StopAllCoroutines();
	}

	// Token: 0x0600274F RID: 10063 RVA: 0x00030607 File Offset: 0x0002E807
	protected void OnDestroy()
	{
	}

	// Token: 0x06002750 RID: 10064 RVA: 0x0010C028 File Offset: 0x0010A228
	private Vector4 MaterialColorCorrection(Vector4 color)
	{
		if (color.x < 0.5f)
		{
			color.x += 3E-08f;
		}
		if (color.y < 0.5f)
		{
			color.y += 3E-08f;
		}
		if (color.z < 0.5f)
		{
			color.z += 3E-08f;
		}
		if (color.w < 0.5f)
		{
			color.w += 3E-08f;
		}
		return color;
	}

	// Token: 0x06002751 RID: 10065 RVA: 0x0004AD48 File Offset: 0x00048F48
	private IEnumerator UpdateTimeOfDay()
	{
		yield return 0;
		for (;;)
		{
			if (this.animatingLightFlash != null)
			{
				yield return new WaitForSeconds(this.currentTimestep);
			}
			else
			{
				try
				{
					if (!this.computerInit && GorillaComputer.instance != null && GorillaComputer.instance.startupMillis != 0L)
					{
						this.computerInit = true;
						this.initialDayCycles = (long)(TimeSpan.FromMilliseconds((double)GorillaComputer.instance.startupMillis).TotalSeconds * this.timeMultiplier / this.totalSeconds);
						this.currentWeatherIndex = (int)(this.initialDayCycles * (long)this.dayNightLightmapNames.Length) % this.weatherCycle.Length;
						this.baseSeconds = TimeSpan.FromMilliseconds((double)GorillaComputer.instance.startupMillis).TotalSeconds * this.timeMultiplier % this.totalSeconds;
						this.currentTime = (this.baseSeconds + (double)Time.realtimeSinceStartup * this.timeMultiplier) % this.totalSeconds;
						this.currentIndexSeconds = 0.0;
						for (int i = 0; i < this.timeOfDayRange.Length; i++)
						{
							this.currentIndexSeconds += this.timeOfDayRange[i] * 3600.0;
							if (this.currentIndexSeconds > this.currentTime)
							{
								this.currentTimeIndex = i;
								break;
							}
						}
						this.currentWeatherIndex += this.currentTimeIndex;
					}
					else if (!this.computerInit && this.baseSeconds == 0.0)
					{
						this.initialDayCycles = (long)(TimeSpan.FromTicks(DateTime.UtcNow.Ticks).TotalSeconds * this.timeMultiplier / this.totalSeconds);
						this.currentWeatherIndex = (int)(this.initialDayCycles * (long)this.dayNightLightmapNames.Length) % this.weatherCycle.Length;
						this.baseSeconds = TimeSpan.FromTicks(DateTime.UtcNow.Ticks).TotalSeconds * this.timeMultiplier % this.totalSeconds;
						this.currentTime = this.baseSeconds % this.totalSeconds;
						this.currentIndexSeconds = 0.0;
						for (int j = 0; j < this.timeOfDayRange.Length; j++)
						{
							this.currentIndexSeconds += this.timeOfDayRange[j] * 3600.0;
							if (this.currentIndexSeconds > this.currentTime)
							{
								this.currentTimeIndex = j;
								break;
							}
						}
						this.currentWeatherIndex += this.currentTimeIndex - 1;
						if (this.currentWeatherIndex < 0)
						{
							this.currentWeatherIndex = this.weatherCycle.Length - 1;
						}
					}
					this.currentTime = ((this.currentSetting == TimeSettings.Normal) ? ((this.baseSeconds + (double)Time.realtimeSinceStartup * this.timeMultiplier) % this.totalSeconds) : this.currentTime);
					this.currentIndexSeconds = 0.0;
					for (int k = 0; k < this.timeOfDayRange.Length; k++)
					{
						this.currentIndexSeconds += this.timeOfDayRange[k] * 3600.0;
						if (this.currentIndexSeconds > this.currentTime)
						{
							this.currentTimeIndex = k;
							break;
						}
					}
					if (this.timeIndexOverrideFunc != null)
					{
						this.currentTimeIndex = this.timeIndexOverrideFunc(this.currentTimeIndex);
					}
					if (this.currentTimeIndex != this.lastIndex)
					{
						this.currentWeatherIndex = (this.currentWeatherIndex + 1) % this.weatherCycle.Length;
						this.ChangeMaps(this.currentTimeIndex, (this.currentTimeIndex + 1) % this.timeOfDayRange.Length);
					}
					this.currentLerp = (float)(1.0 - (this.currentIndexSeconds - this.currentTime) / (this.timeOfDayRange[this.currentTimeIndex] * 3600.0));
					this.ChangeLerps(this.currentLerp);
					this.lastIndex = this.currentTimeIndex;
					this.currentTimeOfDay = this.dayNightLightmapNames[this.currentTimeIndex];
				}
				catch (Exception ex)
				{
					string str = "Error in BetterDayNightManager: ";
					Exception ex2 = ex;
					Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null), this);
				}
				this.gameEpochDay = (long)((this.baseSeconds + (double)Time.realtimeSinceStartup * this.timeMultiplier) / this.totalSeconds + (double)this.initialDayCycles);
				foreach (BetterDayNightManager.ScheduledEvent scheduledEvent in BetterDayNightManager.scheduledEvents.Values)
				{
					if (scheduledEvent.lastDayCalled != this.gameEpochDay && scheduledEvent.hour == this.currentTimeIndex)
					{
						scheduledEvent.lastDayCalled = this.gameEpochDay;
						scheduledEvent.action();
					}
				}
				yield return new WaitForSeconds(this.currentTimestep);
			}
		}
		yield break;
	}

	// Token: 0x06002752 RID: 10066 RVA: 0x0010C0AC File Offset: 0x0010A2AC
	private void ChangeLerps(float newLerp)
	{
		Shader.SetGlobalFloat(this._GlobalDayNightLerpValue, newLerp);
		for (int i = 0; i < this.standardMaterialsUnlit.Length; i++)
		{
			this.tempLerp = Mathf.Lerp(this.colorFrom, this.colorTo, newLerp);
			this.standardMaterialsUnlit[i].color = new Color(this.tempLerp, this.tempLerp, this.tempLerp);
		}
		for (int j = 0; j < this.standardMaterialsUnlitDarker.Length; j++)
		{
			this.tempLerp = Mathf.Lerp(this.colorFromDarker, this.colorToDarker, newLerp);
			Color.RGBToHSV(this.standardMaterialsUnlitDarker[j].color, out this.h, out this.s, out this.v);
			this.standardMaterialsUnlitDarker[j].color = Color.HSVToRGB(this.h, this.s, this.tempLerp);
		}
	}

	// Token: 0x06002753 RID: 10067 RVA: 0x0010C18C File Offset: 0x0010A38C
	private void ChangeMaps(int fromIndex, int toIndex)
	{
		this.fromWeatherIndex = this.currentWeatherIndex;
		this.toWeatherIndex = (this.currentWeatherIndex + 1) % this.weatherCycle.Length;
		if (this.weatherCycle[this.fromWeatherIndex] == BetterDayNightManager.WeatherType.Raining)
		{
			this.fromSky = this.dayNightWeatherSkyboxTextures[fromIndex];
		}
		else
		{
			this.fromSky = this.dayNightSkyboxTextures[fromIndex];
		}
		this.fromSky2 = this.cloudsDayNightSkyboxTextures[fromIndex];
		this.fromSky3 = this.beachDayNightSkyboxTextures[fromIndex];
		if (this.weatherCycle[this.toWeatherIndex] == BetterDayNightManager.WeatherType.Raining)
		{
			this.toSky = this.dayNightWeatherSkyboxTextures[toIndex];
		}
		else
		{
			this.toSky = this.dayNightSkyboxTextures[toIndex];
		}
		this.toSky2 = this.cloudsDayNightSkyboxTextures[toIndex];
		this.toSky3 = this.beachDayNightSkyboxTextures[toIndex];
		this.PopulateAllLightmaps(fromIndex, toIndex);
		Shader.SetGlobalTexture(this._GlobalDayNightSkyTex1, this.fromSky);
		Shader.SetGlobalTexture(this._GlobalDayNightSkyTex2, this.toSky);
		Shader.SetGlobalTexture(this._GlobalDayNightSky2Tex1, this.fromSky2);
		Shader.SetGlobalTexture(this._GlobalDayNightSky2Tex2, this.toSky2);
		Shader.SetGlobalTexture(this._GlobalDayNightSky3Tex1, this.fromSky3);
		Shader.SetGlobalTexture(this._GlobalDayNightSky3Tex2, this.toSky3);
		this.colorFrom = this.standardUnlitColor[fromIndex];
		this.colorTo = this.standardUnlitColor[toIndex];
		this.colorFromDarker = this.standardUnlitColorWithPremadeColorDarker[fromIndex];
		this.colorToDarker = this.standardUnlitColorWithPremadeColorDarker[toIndex];
	}

	// Token: 0x06002754 RID: 10068 RVA: 0x0010C314 File Offset: 0x0010A514
	public void SliceUpdate()
	{
		if (!this.shouldRepopulate)
		{
			using (List<PerSceneRenderData>.Enumerator enumerator = BetterDayNightManager.allScenesRenderData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CheckShouldRepopulate())
					{
						this.shouldRepopulate = true;
					}
				}
			}
		}
		if (this.shouldRepopulate)
		{
			this.PopulateAllLightmaps();
			this.shouldRepopulate = false;
		}
	}

	// Token: 0x06002755 RID: 10069 RVA: 0x0004AD57 File Offset: 0x00048F57
	public void RequestRepopulateLightmaps()
	{
		this.shouldRepopulate = true;
	}

	// Token: 0x06002756 RID: 10070 RVA: 0x0004AD60 File Offset: 0x00048F60
	public void PopulateAllLightmaps()
	{
		this.PopulateAllLightmaps(this.currentTimeIndex, (this.currentTimeIndex + 1) % this.timeOfDayRange.Length);
	}

	// Token: 0x06002757 RID: 10071 RVA: 0x0010C38C File Offset: 0x0010A58C
	public void PopulateAllLightmaps(int fromIndex, int toIndex)
	{
		string fromTimeOfDay;
		if (this.weatherCycle[this.fromWeatherIndex] == BetterDayNightManager.WeatherType.Raining)
		{
			fromTimeOfDay = this.dayNightWeatherLightmapNames[fromIndex];
		}
		else
		{
			fromTimeOfDay = this.dayNightLightmapNames[fromIndex];
		}
		string toTimeOfDay;
		if (this.weatherCycle[this.toWeatherIndex] == BetterDayNightManager.WeatherType.Raining)
		{
			toTimeOfDay = this.dayNightWeatherLightmapNames[toIndex];
		}
		else
		{
			toTimeOfDay = this.dayNightLightmapNames[toIndex];
		}
		LightmapData[] lightmaps = LightmapSettings.lightmaps;
		foreach (PerSceneRenderData perSceneRenderData in BetterDayNightManager.allScenesRenderData)
		{
			perSceneRenderData.PopulateLightmaps(fromTimeOfDay, toTimeOfDay, lightmaps);
		}
		LightmapSettings.lightmaps = lightmaps;
	}

	// Token: 0x06002758 RID: 10072 RVA: 0x0004AD7F File Offset: 0x00048F7F
	public BetterDayNightManager.WeatherType CurrentWeather()
	{
		return this.weatherCycle[this.currentWeatherIndex];
	}

	// Token: 0x06002759 RID: 10073 RVA: 0x0004AD8E File Offset: 0x00048F8E
	public BetterDayNightManager.WeatherType NextWeather()
	{
		return this.weatherCycle[(this.currentWeatherIndex + 1) % this.weatherCycle.Length];
	}

	// Token: 0x0600275A RID: 10074 RVA: 0x0004ADA8 File Offset: 0x00048FA8
	public BetterDayNightManager.WeatherType LastWeather()
	{
		return this.weatherCycle[(this.currentWeatherIndex - 1) % this.weatherCycle.Length];
	}

	// Token: 0x0600275B RID: 10075 RVA: 0x0010C434 File Offset: 0x0010A634
	private void GenerateWeatherEventTimes()
	{
		this.weatherCycle = new BetterDayNightManager.WeatherType[100 * this.dayNightLightmapNames.Length];
		this.rainChance = this.rainChance * 2f / (float)this.maxRainDuration;
		for (int i = 1; i < this.weatherCycle.Length; i++)
		{
			this.weatherCycle[i] = (((float)this.randomNumberGenerator.Next(100) < this.rainChance * 100f) ? BetterDayNightManager.WeatherType.Raining : BetterDayNightManager.WeatherType.None);
			if (this.weatherCycle[i] == BetterDayNightManager.WeatherType.Raining)
			{
				this.rainDuration = this.randomNumberGenerator.Next(1, this.maxRainDuration + 1);
				for (int j = 1; j < this.rainDuration; j++)
				{
					if (i + j < this.weatherCycle.Length)
					{
						this.weatherCycle[i + j] = BetterDayNightManager.WeatherType.Raining;
					}
				}
				i += this.rainDuration - 1;
			}
		}
	}

	// Token: 0x0600275C RID: 10076 RVA: 0x0010C50C File Offset: 0x0010A70C
	public static int RegisterScheduledEvent(int hour, Action action)
	{
		int num = (int)(DateTime.Now.Ticks % 2147483647L);
		while (BetterDayNightManager.scheduledEvents.ContainsKey(num))
		{
			num++;
		}
		BetterDayNightManager.scheduledEvents.Add(num, new BetterDayNightManager.ScheduledEvent
		{
			lastDayCalled = -1L,
			hour = hour,
			action = action
		});
		return num;
	}

	// Token: 0x0600275D RID: 10077 RVA: 0x0004ADC2 File Offset: 0x00048FC2
	public static void UnregisterScheduledEvent(int id)
	{
		BetterDayNightManager.scheduledEvents.Remove(id);
	}

	// Token: 0x0600275E RID: 10078 RVA: 0x0004ADD0 File Offset: 0x00048FD0
	public void SetTimeIndexOverrideFunction(Func<int, int> overrideFunction)
	{
		this.timeIndexOverrideFunc = overrideFunction;
	}

	// Token: 0x0600275F RID: 10079 RVA: 0x0004ADD9 File Offset: 0x00048FD9
	public void UnsetTimeIndexOverrideFunction()
	{
		this.timeIndexOverrideFunc = null;
	}

	// Token: 0x06002760 RID: 10080 RVA: 0x0010C56C File Offset: 0x0010A76C
	public void SetOverrideIndex(int index)
	{
		this.overrideIndex = index;
		this.currentWeatherIndex = this.overrideIndex;
		this.currentTimeIndex = this.overrideIndex;
		this.currentTimeOfDay = this.dayNightLightmapNames[this.currentTimeIndex];
		this.ChangeMaps(this.currentTimeIndex, (this.currentTimeIndex + 1) % this.timeOfDayRange.Length);
	}

	// Token: 0x06002761 RID: 10081 RVA: 0x0004ADE2 File Offset: 0x00048FE2
	public void AnimateLightFlash(int index, float fadeInDuration, float holdDuration, float fadeOutDuration)
	{
		if (this.animatingLightFlash != null)
		{
			base.StopCoroutine(this.animatingLightFlash);
		}
		this.animatingLightFlash = base.StartCoroutine(this.AnimateLightFlashCo(index, fadeInDuration, holdDuration, fadeOutDuration));
	}

	// Token: 0x06002762 RID: 10082 RVA: 0x0004AE0F File Offset: 0x0004900F
	private IEnumerator AnimateLightFlashCo(int index, float fadeInDuration, float holdDuration, float fadeOutDuration)
	{
		int startMap = (this.currentLerp < 0.5f) ? this.currentTimeIndex : ((this.currentTimeIndex + 1) % this.timeOfDayRange.Length);
		this.ChangeMaps(startMap, index);
		float endTimestamp = Time.time + fadeInDuration;
		while (Time.time < endTimestamp)
		{
			this.ChangeLerps(1f - (endTimestamp - Time.time) / fadeInDuration);
			yield return null;
		}
		this.ChangeMaps(index, index);
		this.ChangeLerps(0f);
		endTimestamp = Time.time + fadeInDuration;
		while (Time.time < endTimestamp)
		{
			yield return null;
		}
		this.ChangeMaps(index, startMap);
		endTimestamp = Time.time + fadeOutDuration;
		while (Time.time < endTimestamp)
		{
			this.ChangeLerps(1f - (endTimestamp - Time.time) / fadeInDuration);
			yield return null;
		}
		this.ChangeMaps(this.currentTimeIndex, (this.currentTimeIndex + 1) % this.timeOfDayRange.Length);
		this.ChangeLerps(this.currentLerp);
		this.animatingLightFlash = null;
		yield break;
	}

	// Token: 0x06002763 RID: 10083 RVA: 0x0010C5C8 File Offset: 0x0010A7C8
	public void SetTimeOfDay(int timeIndex)
	{
		double num = 0.0;
		for (int i = 0; i < timeIndex; i++)
		{
			num += this.timeOfDayRange[i];
		}
		this.currentTime = num * 3600.0;
		this.currentSetting = TimeSettings.Static;
	}

	// Token: 0x06002764 RID: 10084 RVA: 0x0004AE34 File Offset: 0x00049034
	public void FastForward(float seconds)
	{
		this.baseSeconds += (double)seconds;
	}

	// Token: 0x06002767 RID: 10087 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002C0C RID: 11276
	[OnEnterPlay_SetNull]
	public static volatile BetterDayNightManager instance;

	// Token: 0x04002C0D RID: 11277
	[OnEnterPlay_Clear]
	public static List<PerSceneRenderData> allScenesRenderData = new List<PerSceneRenderData>();

	// Token: 0x04002C0E RID: 11278
	public Shader standard;

	// Token: 0x04002C0F RID: 11279
	public Shader standardCutout;

	// Token: 0x04002C10 RID: 11280
	public Shader gorillaUnlit;

	// Token: 0x04002C11 RID: 11281
	public Shader gorillaUnlitCutout;

	// Token: 0x04002C12 RID: 11282
	public Material[] standardMaterialsUnlit;

	// Token: 0x04002C13 RID: 11283
	public Material[] standardMaterialsUnlitDarker;

	// Token: 0x04002C14 RID: 11284
	public Material[] dayNightSupportedMaterials;

	// Token: 0x04002C15 RID: 11285
	public Material[] dayNightSupportedMaterialsCutout;

	// Token: 0x04002C16 RID: 11286
	public string[] dayNightLightmapNames;

	// Token: 0x04002C17 RID: 11287
	public string[] dayNightWeatherLightmapNames;

	// Token: 0x04002C18 RID: 11288
	public Texture2D[] dayNightSkyboxTextures;

	// Token: 0x04002C19 RID: 11289
	public Texture2D[] cloudsDayNightSkyboxTextures;

	// Token: 0x04002C1A RID: 11290
	public Texture2D[] beachDayNightSkyboxTextures;

	// Token: 0x04002C1B RID: 11291
	public Texture2D[] dayNightWeatherSkyboxTextures;

	// Token: 0x04002C1C RID: 11292
	public float[] standardUnlitColor;

	// Token: 0x04002C1D RID: 11293
	public float[] standardUnlitColorWithPremadeColorDarker;

	// Token: 0x04002C1E RID: 11294
	public float currentLerp;

	// Token: 0x04002C1F RID: 11295
	public float currentTimestep;

	// Token: 0x04002C20 RID: 11296
	public double[] timeOfDayRange;

	// Token: 0x04002C21 RID: 11297
	public double timeMultiplier;

	// Token: 0x04002C22 RID: 11298
	private float lastTime;

	// Token: 0x04002C23 RID: 11299
	private double currentTime;

	// Token: 0x04002C24 RID: 11300
	private double totalHours;

	// Token: 0x04002C25 RID: 11301
	private double totalSeconds;

	// Token: 0x04002C26 RID: 11302
	private float colorFrom;

	// Token: 0x04002C27 RID: 11303
	private float colorTo;

	// Token: 0x04002C28 RID: 11304
	private float colorFromDarker;

	// Token: 0x04002C29 RID: 11305
	private float colorToDarker;

	// Token: 0x04002C2A RID: 11306
	public int currentTimeIndex;

	// Token: 0x04002C2B RID: 11307
	public int currentWeatherIndex;

	// Token: 0x04002C2C RID: 11308
	private int lastIndex;

	// Token: 0x04002C2D RID: 11309
	private double currentIndexSeconds;

	// Token: 0x04002C2E RID: 11310
	private float tempLerp;

	// Token: 0x04002C2F RID: 11311
	private double baseSeconds;

	// Token: 0x04002C30 RID: 11312
	private bool computerInit;

	// Token: 0x04002C31 RID: 11313
	private float h;

	// Token: 0x04002C32 RID: 11314
	private float s;

	// Token: 0x04002C33 RID: 11315
	private float v;

	// Token: 0x04002C34 RID: 11316
	public int mySeed;

	// Token: 0x04002C35 RID: 11317
	public System.Random randomNumberGenerator = new System.Random();

	// Token: 0x04002C36 RID: 11318
	public BetterDayNightManager.WeatherType[] weatherCycle;

	// Token: 0x04002C38 RID: 11320
	public float rainChance = 0.3f;

	// Token: 0x04002C39 RID: 11321
	public int maxRainDuration = 5;

	// Token: 0x04002C3A RID: 11322
	private int rainDuration;

	// Token: 0x04002C3B RID: 11323
	private float remainingSeconds;

	// Token: 0x04002C3C RID: 11324
	private long initialDayCycles;

	// Token: 0x04002C3D RID: 11325
	private long gameEpochDay;

	// Token: 0x04002C3E RID: 11326
	private int currentWeatherCycle;

	// Token: 0x04002C3F RID: 11327
	private int fromWeatherIndex;

	// Token: 0x04002C40 RID: 11328
	private int toWeatherIndex;

	// Token: 0x04002C41 RID: 11329
	private Texture2D fromSky;

	// Token: 0x04002C42 RID: 11330
	private Texture2D fromSky2;

	// Token: 0x04002C43 RID: 11331
	private Texture2D fromSky3;

	// Token: 0x04002C44 RID: 11332
	private Texture2D toSky;

	// Token: 0x04002C45 RID: 11333
	private Texture2D toSky2;

	// Token: 0x04002C46 RID: 11334
	private Texture2D toSky3;

	// Token: 0x04002C47 RID: 11335
	public AddCollidersToParticleSystemTriggers[] weatherSystems;

	// Token: 0x04002C48 RID: 11336
	public List<Collider> collidersToAddToWeatherSystems = new List<Collider>();

	// Token: 0x04002C49 RID: 11337
	private Func<int, int> timeIndexOverrideFunc;

	// Token: 0x04002C4A RID: 11338
	public int overrideIndex = -1;

	// Token: 0x04002C4B RID: 11339
	[OnEnterPlay_Clear]
	private static readonly Dictionary<int, BetterDayNightManager.ScheduledEvent> scheduledEvents = new Dictionary<int, BetterDayNightManager.ScheduledEvent>(256);

	// Token: 0x04002C4C RID: 11340
	public TimeSettings currentSetting;

	// Token: 0x04002C4D RID: 11341
	private ShaderHashId _Color = "_Color";

	// Token: 0x04002C4E RID: 11342
	private ShaderHashId _GlobalDayNightLerpValue = "_GlobalDayNightLerpValue";

	// Token: 0x04002C4F RID: 11343
	private ShaderHashId _GlobalDayNightSkyTex1 = "_GlobalDayNightSkyTex1";

	// Token: 0x04002C50 RID: 11344
	private ShaderHashId _GlobalDayNightSkyTex2 = "_GlobalDayNightSkyTex2";

	// Token: 0x04002C51 RID: 11345
	private ShaderHashId _GlobalDayNightSky2Tex1 = "_GlobalDayNightSky2Tex1";

	// Token: 0x04002C52 RID: 11346
	private ShaderHashId _GlobalDayNightSky2Tex2 = "_GlobalDayNightSky2Tex2";

	// Token: 0x04002C53 RID: 11347
	private ShaderHashId _GlobalDayNightSky3Tex1 = "_GlobalDayNightSky3Tex1";

	// Token: 0x04002C54 RID: 11348
	private ShaderHashId _GlobalDayNightSky3Tex2 = "_GlobalDayNightSky3Tex2";

	// Token: 0x04002C55 RID: 11349
	private bool shouldRepopulate;

	// Token: 0x04002C56 RID: 11350
	private Coroutine animatingLightFlash;

	// Token: 0x02000631 RID: 1585
	public enum WeatherType
	{
		// Token: 0x04002C58 RID: 11352
		None,
		// Token: 0x04002C59 RID: 11353
		Raining,
		// Token: 0x04002C5A RID: 11354
		All
	}

	// Token: 0x02000632 RID: 1586
	private class ScheduledEvent
	{
		// Token: 0x04002C5B RID: 11355
		public long lastDayCalled;

		// Token: 0x04002C5C RID: 11356
		public int hour;

		// Token: 0x04002C5D RID: 11357
		public Action action;
	}
}
