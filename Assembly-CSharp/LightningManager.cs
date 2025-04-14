using System;
using System.Collections;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200071A RID: 1818
public class LightningManager : MonoBehaviour
{
	// Token: 0x06002CE8 RID: 11496 RVA: 0x000DDFDE File Offset: 0x000DC1DE
	private void Start()
	{
		this.lightningAudio = base.GetComponent<AudioSource>();
		GorillaComputer instance = GorillaComputer.instance;
		instance.OnServerTimeUpdated = (Action)Delegate.Combine(instance.OnServerTimeUpdated, new Action(this.OnTimeChanged));
	}

	// Token: 0x06002CE9 RID: 11497 RVA: 0x000DE014 File Offset: 0x000DC214
	private void OnTimeChanged()
	{
		this.InitializeRng();
		if (this.lightningRunner != null)
		{
			base.StopCoroutine(this.lightningRunner);
		}
		this.lightningRunner = base.StartCoroutine(this.LightningEffectRunner());
	}

	// Token: 0x06002CEA RID: 11498 RVA: 0x000DE044 File Offset: 0x000DC244
	private void GetHourStart(out long seed, out float timestampRealtime)
	{
		DateTime serverTime = GorillaComputer.instance.GetServerTime();
		DateTime d = new DateTime(serverTime.Year, serverTime.Month, serverTime.Day, serverTime.Hour, 0, 0);
		timestampRealtime = Time.realtimeSinceStartup - (float)(serverTime - d).TotalSeconds;
		seed = d.Ticks;
	}

	// Token: 0x06002CEB RID: 11499 RVA: 0x000DE0A4 File Offset: 0x000DC2A4
	private void InitializeRng()
	{
		long seed;
		float num;
		this.GetHourStart(out seed, out num);
		this.currentHourlySeed = seed;
		this.rng = new SRand(seed);
		this.lightningTimestampsRealtime.Clear();
		this.nextLightningTimestampIndex = -1;
		float num2 = num;
		float num3 = 0f;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		while (num3 < 3600f)
		{
			float num4 = this.rng.NextFloat(this.minTimeBetweenFlashes, this.maxTimeBetweenFlashes);
			num3 += num4;
			num2 += num4;
			if (this.nextLightningTimestampIndex == -1 && num2 > realtimeSinceStartup)
			{
				this.nextLightningTimestampIndex = this.lightningTimestampsRealtime.Count;
			}
			this.lightningTimestampsRealtime.Add(num2);
		}
		this.lightningTimestampsRealtime[this.lightningTimestampsRealtime.Count - 1] = num + 3605f;
	}

	// Token: 0x06002CEC RID: 11500 RVA: 0x000DE168 File Offset: 0x000DC368
	private void DoLightningStrike()
	{
		BetterDayNightManager.instance.AnimateLightFlash(this.lightMapIndex, this.flashFadeInDuration, this.flashHoldDuration, this.flashFadeOutDuration);
		this.lightningAudio.clip = (ZoneManagement.IsInZone(GTZone.cave) ? this.muffledLightning : this.regularLightning);
		this.lightningAudio.GTPlay();
	}

	// Token: 0x06002CED RID: 11501 RVA: 0x000DE1C5 File Offset: 0x000DC3C5
	private IEnumerator LightningEffectRunner()
	{
		for (;;)
		{
			if (this.lightningTimestampsRealtime.Count <= this.nextLightningTimestampIndex)
			{
				this.InitializeRng();
			}
			if (this.lightningTimestampsRealtime.Count > this.nextLightningTimestampIndex)
			{
				yield return new WaitForSecondsRealtime(this.lightningTimestampsRealtime[this.nextLightningTimestampIndex] - Time.realtimeSinceStartup);
				float num = this.lightningTimestampsRealtime[this.nextLightningTimestampIndex];
				this.nextLightningTimestampIndex++;
				if (Time.realtimeSinceStartup - num < 1f && this.lightningTimestampsRealtime.Count > this.nextLightningTimestampIndex)
				{
					this.DoLightningStrike();
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04003261 RID: 12897
	public int lightMapIndex;

	// Token: 0x04003262 RID: 12898
	public float minTimeBetweenFlashes;

	// Token: 0x04003263 RID: 12899
	public float maxTimeBetweenFlashes;

	// Token: 0x04003264 RID: 12900
	public float flashFadeInDuration;

	// Token: 0x04003265 RID: 12901
	public float flashHoldDuration;

	// Token: 0x04003266 RID: 12902
	public float flashFadeOutDuration;

	// Token: 0x04003267 RID: 12903
	private AudioSource lightningAudio;

	// Token: 0x04003268 RID: 12904
	private SRand rng;

	// Token: 0x04003269 RID: 12905
	private long currentHourlySeed;

	// Token: 0x0400326A RID: 12906
	private List<float> lightningTimestampsRealtime = new List<float>();

	// Token: 0x0400326B RID: 12907
	private int nextLightningTimestampIndex;

	// Token: 0x0400326C RID: 12908
	public AudioClip regularLightning;

	// Token: 0x0400326D RID: 12909
	public AudioClip muffledLightning;

	// Token: 0x0400326E RID: 12910
	private Coroutine lightningRunner;
}
