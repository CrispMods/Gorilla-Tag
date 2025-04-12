using System;
using System.Collections;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000652 RID: 1618
public class BetterDayNightManager : MonoBehaviour, IGorillaSliceableSimple, ITimeOfDaySystem
{
	// Token: 0x06002823 RID: 10275 RVA: 0x0004A73E File Offset: 0x0004893E
	public static void Register(PerSceneRenderData data)
	{
		BetterDayNightManager.allScenesRenderData.Add(data);
	}

	// Token: 0x06002824 RID: 10276 RVA: 0x0004A74B File Offset: 0x0004894B
	public static void Unregister(PerSceneRenderData data)
	{
		BetterDayNightManager.allScenesRenderData.Remove(data);
	}

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x06002825 RID: 10277 RVA: 0x0004A759 File Offset: 0x00048959
	// (set) Token: 0x06002826 RID: 10278 RVA: 0x0004A761 File Offset: 0x00048961
	public string currentTimeOfDay { get; private set; }

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06002827 RID: 10279 RVA: 0x0004A76A File Offset: 0x0004896A
	public float NormalizedTimeOfDay
	{
		get
		{
			return Mathf.Clamp01((float)((this.baseSeconds + (double)Time.realtimeSinceStartup * this.timeMultiplier) % this.totalSeconds / this.totalSeconds));
		}
	}

	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x06002828 RID: 10280 RVA: 0x0004A794 File Offset: 0x00048994
	double ITimeOfDaySystem.currentTimeInSeconds
	{
		get
		{
			return this.currentTime;
		}
	}

	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x06002829 RID: 10281 RVA: 0x0004A79C File Offset: 0x0004899C
	double ITimeOfDaySystem.totalTimeInSeconds
	{
		get
		{
			return this.totalSeconds;
		}
	}

	// Token: 0x0600282A RID: 10282 RVA: 0x0010DB0C File Offset: 0x0010BD0C
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

	// Token: 0x0600282B RID: 10283 RVA: 0x0004A7A4 File Offset: 0x000489A4
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		base.StopAllCoroutines();
	}

	// Token: 0x0600282C RID: 10284 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected void OnDestroy()
	{
	}

	// Token: 0x0600282D RID: 10285 RVA: 0x0010DC00 File Offset: 0x0010BE00
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

	// Token: 0x0600282E RID: 10286 RVA: 0x0004A7B3 File Offset: 0x000489B3
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

	// Token: 0x0600282F RID: 10287 RVA: 0x0010DC84 File Offset: 0x0010BE84
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

	// Token: 0x06002830 RID: 10288 RVA: 0x0010DD64 File Offset: 0x0010BF64
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

	// Token: 0x06002831 RID: 10289 RVA: 0x0010DEEC File Offset: 0x0010C0EC
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

	// Token: 0x06002832 RID: 10290 RVA: 0x0004A7C2 File Offset: 0x000489C2
	public void RequestRepopulateLightmaps()
	{
		this.shouldRepopulate = true;
	}

	// Token: 0x06002833 RID: 10291 RVA: 0x0004A7CB File Offset: 0x000489CB
	public void PopulateAllLightmaps()
	{
		this.PopulateAllLightmaps(this.currentTimeIndex, (this.currentTimeIndex + 1) % this.timeOfDayRange.Length);
	}

	// Token: 0x06002834 RID: 10292 RVA: 0x0010DF64 File Offset: 0x0010C164
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

	// Token: 0x06002835 RID: 10293 RVA: 0x0004A7EA File Offset: 0x000489EA
	public BetterDayNightManager.WeatherType CurrentWeather()
	{
		return this.weatherCycle[this.currentWeatherIndex];
	}

	// Token: 0x06002836 RID: 10294 RVA: 0x0004A7F9 File Offset: 0x000489F9
	public BetterDayNightManager.WeatherType NextWeather()
	{
		return this.weatherCycle[(this.currentWeatherIndex + 1) % this.weatherCycle.Length];
	}

	// Token: 0x06002837 RID: 10295 RVA: 0x0004A813 File Offset: 0x00048A13
	public BetterDayNightManager.WeatherType LastWeather()
	{
		return this.weatherCycle[(this.currentWeatherIndex - 1) % this.weatherCycle.Length];
	}

	// Token: 0x06002838 RID: 10296 RVA: 0x0010E00C File Offset: 0x0010C20C
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

	// Token: 0x06002839 RID: 10297 RVA: 0x0010E0E4 File Offset: 0x0010C2E4
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

	// Token: 0x0600283A RID: 10298 RVA: 0x0004A82D File Offset: 0x00048A2D
	public static void UnregisterScheduledEvent(int id)
	{
		BetterDayNightManager.scheduledEvents.Remove(id);
	}

	// Token: 0x0600283B RID: 10299 RVA: 0x0004A83B File Offset: 0x00048A3B
	public void SetTimeIndexOverrideFunction(Func<int, int> overrideFunction)
	{
		this.timeIndexOverrideFunc = overrideFunction;
	}

	// Token: 0x0600283C RID: 10300 RVA: 0x0004A844 File Offset: 0x00048A44
	public void UnsetTimeIndexOverrideFunction()
	{
		this.timeIndexOverrideFunc = null;
	}

	// Token: 0x0600283D RID: 10301 RVA: 0x0010E144 File Offset: 0x0010C344
	public void SetOverrideIndex(int index)
	{
		this.overrideIndex = index;
		this.currentWeatherIndex = this.overrideIndex;
		this.currentTimeIndex = this.overrideIndex;
		this.currentTimeOfDay = this.dayNightLightmapNames[this.currentTimeIndex];
		this.ChangeMaps(this.currentTimeIndex, (this.currentTimeIndex + 1) % this.timeOfDayRange.Length);
	}

	// Token: 0x0600283E RID: 10302 RVA: 0x0004A84D File Offset: 0x00048A4D
	public void AnimateLightFlash(int index, float fadeInDuration, float holdDuration, float fadeOutDuration)
	{
		if (this.animatingLightFlash != null)
		{
			base.StopCoroutine(this.animatingLightFlash);
		}
		this.animatingLightFlash = base.StartCoroutine(this.AnimateLightFlashCo(index, fadeInDuration, holdDuration, fadeOutDuration));
	}

	// Token: 0x0600283F RID: 10303 RVA: 0x0004A87A File Offset: 0x00048A7A
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

	// Token: 0x06002840 RID: 10304 RVA: 0x0010E1A0 File Offset: 0x0010C3A0
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

	// Token: 0x06002841 RID: 10305 RVA: 0x0004A89F File Offset: 0x00048A9F
	public void FastForward(float seconds)
	{
		this.baseSeconds += (double)seconds;
	}

	// Token: 0x06002844 RID: 10308 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002CAC RID: 11436
	[OnEnterPlay_SetNull]
	public static volatile BetterDayNightManager instance;

	// Token: 0x04002CAD RID: 11437
	[OnEnterPlay_Clear]
	public static List<PerSceneRenderData> allScenesRenderData = new List<PerSceneRenderData>();

	// Token: 0x04002CAE RID: 11438
	public Shader standard;

	// Token: 0x04002CAF RID: 11439
	public Shader standardCutout;

	// Token: 0x04002CB0 RID: 11440
	public Shader gorillaUnlit;

	// Token: 0x04002CB1 RID: 11441
	public Shader gorillaUnlitCutout;

	// Token: 0x04002CB2 RID: 11442
	public Material[] standardMaterialsUnlit;

	// Token: 0x04002CB3 RID: 11443
	public Material[] standardMaterialsUnlitDarker;

	// Token: 0x04002CB4 RID: 11444
	public Material[] dayNightSupportedMaterials;

	// Token: 0x04002CB5 RID: 11445
	public Material[] dayNightSupportedMaterialsCutout;

	// Token: 0x04002CB6 RID: 11446
	public string[] dayNightLightmapNames;

	// Token: 0x04002CB7 RID: 11447
	public string[] dayNightWeatherLightmapNames;

	// Token: 0x04002CB8 RID: 11448
	public Texture2D[] dayNightSkyboxTextures;

	// Token: 0x04002CB9 RID: 11449
	public Texture2D[] cloudsDayNightSkyboxTextures;

	// Token: 0x04002CBA RID: 11450
	public Texture2D[] beachDayNightSkyboxTextures;

	// Token: 0x04002CBB RID: 11451
	public Texture2D[] dayNightWeatherSkyboxTextures;

	// Token: 0x04002CBC RID: 11452
	public float[] standardUnlitColor;

	// Token: 0x04002CBD RID: 11453
	public float[] standardUnlitColorWithPremadeColorDarker;

	// Token: 0x04002CBE RID: 11454
	public float currentLerp;

	// Token: 0x04002CBF RID: 11455
	public float currentTimestep;

	// Token: 0x04002CC0 RID: 11456
	public double[] timeOfDayRange;

	// Token: 0x04002CC1 RID: 11457
	public double timeMultiplier;

	// Token: 0x04002CC2 RID: 11458
	private float lastTime;

	// Token: 0x04002CC3 RID: 11459
	private double currentTime;

	// Token: 0x04002CC4 RID: 11460
	private double totalHours;

	// Token: 0x04002CC5 RID: 11461
	private double totalSeconds;

	// Token: 0x04002CC6 RID: 11462
	private float colorFrom;

	// Token: 0x04002CC7 RID: 11463
	private float colorTo;

	// Token: 0x04002CC8 RID: 11464
	private float colorFromDarker;

	// Token: 0x04002CC9 RID: 11465
	private float colorToDarker;

	// Token: 0x04002CCA RID: 11466
	public int currentTimeIndex;

	// Token: 0x04002CCB RID: 11467
	public int currentWeatherIndex;

	// Token: 0x04002CCC RID: 11468
	private int lastIndex;

	// Token: 0x04002CCD RID: 11469
	private double currentIndexSeconds;

	// Token: 0x04002CCE RID: 11470
	private float tempLerp;

	// Token: 0x04002CCF RID: 11471
	private double baseSeconds;

	// Token: 0x04002CD0 RID: 11472
	private bool computerInit;

	// Token: 0x04002CD1 RID: 11473
	private float h;

	// Token: 0x04002CD2 RID: 11474
	private float s;

	// Token: 0x04002CD3 RID: 11475
	private float v;

	// Token: 0x04002CD4 RID: 11476
	public int mySeed;

	// Token: 0x04002CD5 RID: 11477
	public System.Random randomNumberGenerator = new System.Random();

	// Token: 0x04002CD6 RID: 11478
	public BetterDayNightManager.WeatherType[] weatherCycle;

	// Token: 0x04002CD8 RID: 11480
	public float rainChance = 0.3f;

	// Token: 0x04002CD9 RID: 11481
	public int maxRainDuration = 5;

	// Token: 0x04002CDA RID: 11482
	private int rainDuration;

	// Token: 0x04002CDB RID: 11483
	private float remainingSeconds;

	// Token: 0x04002CDC RID: 11484
	private long initialDayCycles;

	// Token: 0x04002CDD RID: 11485
	private long gameEpochDay;

	// Token: 0x04002CDE RID: 11486
	private int currentWeatherCycle;

	// Token: 0x04002CDF RID: 11487
	private int fromWeatherIndex;

	// Token: 0x04002CE0 RID: 11488
	private int toWeatherIndex;

	// Token: 0x04002CE1 RID: 11489
	private Texture2D fromSky;

	// Token: 0x04002CE2 RID: 11490
	private Texture2D fromSky2;

	// Token: 0x04002CE3 RID: 11491
	private Texture2D fromSky3;

	// Token: 0x04002CE4 RID: 11492
	private Texture2D toSky;

	// Token: 0x04002CE5 RID: 11493
	private Texture2D toSky2;

	// Token: 0x04002CE6 RID: 11494
	private Texture2D toSky3;

	// Token: 0x04002CE7 RID: 11495
	public AddCollidersToParticleSystemTriggers[] weatherSystems;

	// Token: 0x04002CE8 RID: 11496
	public List<Collider> collidersToAddToWeatherSystems = new List<Collider>();

	// Token: 0x04002CE9 RID: 11497
	private Func<int, int> timeIndexOverrideFunc;

	// Token: 0x04002CEA RID: 11498
	public int overrideIndex = -1;

	// Token: 0x04002CEB RID: 11499
	[OnEnterPlay_Clear]
	private static readonly Dictionary<int, BetterDayNightManager.ScheduledEvent> scheduledEvents = new Dictionary<int, BetterDayNightManager.ScheduledEvent>(256);

	// Token: 0x04002CEC RID: 11500
	public TimeSettings currentSetting;

	// Token: 0x04002CED RID: 11501
	private ShaderHashId _Color = "_Color";

	// Token: 0x04002CEE RID: 11502
	private ShaderHashId _GlobalDayNightLerpValue = "_GlobalDayNightLerpValue";

	// Token: 0x04002CEF RID: 11503
	private ShaderHashId _GlobalDayNightSkyTex1 = "_GlobalDayNightSkyTex1";

	// Token: 0x04002CF0 RID: 11504
	private ShaderHashId _GlobalDayNightSkyTex2 = "_GlobalDayNightSkyTex2";

	// Token: 0x04002CF1 RID: 11505
	private ShaderHashId _GlobalDayNightSky2Tex1 = "_GlobalDayNightSky2Tex1";

	// Token: 0x04002CF2 RID: 11506
	private ShaderHashId _GlobalDayNightSky2Tex2 = "_GlobalDayNightSky2Tex2";

	// Token: 0x04002CF3 RID: 11507
	private ShaderHashId _GlobalDayNightSky3Tex1 = "_GlobalDayNightSky3Tex1";

	// Token: 0x04002CF4 RID: 11508
	private ShaderHashId _GlobalDayNightSky3Tex2 = "_GlobalDayNightSky3Tex2";

	// Token: 0x04002CF5 RID: 11509
	private bool shouldRepopulate;

	// Token: 0x04002CF6 RID: 11510
	private Coroutine animatingLightFlash;

	// Token: 0x02000653 RID: 1619
	public enum WeatherType
	{
		// Token: 0x04002CF8 RID: 11512
		None,
		// Token: 0x04002CF9 RID: 11513
		Raining,
		// Token: 0x04002CFA RID: 11514
		All
	}

	// Token: 0x02000654 RID: 1620
	private class ScheduledEvent
	{
		// Token: 0x04002CFB RID: 11515
		public long lastDayCalled;

		// Token: 0x04002CFC RID: 11516
		public int hour;

		// Token: 0x04002CFD RID: 11517
		public Action action;
	}
}
