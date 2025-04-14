using System;
using System.Collections;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200071B RID: 1819
public class LightningManager : MonoBehaviour
{
	// Token: 0x06002CF0 RID: 11504 RVA: 0x000DE45E File Offset: 0x000DC65E
	private void Start()
	{
		this.lightningAudio = base.GetComponent<AudioSource>();
		GorillaComputer instance = GorillaComputer.instance;
		instance.OnServerTimeUpdated = (Action)Delegate.Combine(instance.OnServerTimeUpdated, new Action(this.OnTimeChanged));
	}

	// Token: 0x06002CF1 RID: 11505 RVA: 0x000DE494 File Offset: 0x000DC694
	private void OnTimeChanged()
	{
		this.InitializeRng();
		if (this.lightningRunner != null)
		{
			base.StopCoroutine(this.lightningRunner);
		}
		this.lightningRunner = base.StartCoroutine(this.LightningEffectRunner());
	}

	// Token: 0x06002CF2 RID: 11506 RVA: 0x000DE4C4 File Offset: 0x000DC6C4
	private void GetHourStart(out long seed, out float timestampRealtime)
	{
		DateTime serverTime = GorillaComputer.instance.GetServerTime();
		DateTime d = new DateTime(serverTime.Year, serverTime.Month, serverTime.Day, serverTime.Hour, 0, 0);
		timestampRealtime = Time.realtimeSinceStartup - (float)(serverTime - d).TotalSeconds;
		seed = d.Ticks;
	}

	// Token: 0x06002CF3 RID: 11507 RVA: 0x000DE524 File Offset: 0x000DC724
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

	// Token: 0x06002CF4 RID: 11508 RVA: 0x000DE5E8 File Offset: 0x000DC7E8
	private void DoLightningStrike()
	{
		BetterDayNightManager.instance.AnimateLightFlash(this.lightMapIndex, this.flashFadeInDuration, this.flashHoldDuration, this.flashFadeOutDuration);
		this.lightningAudio.clip = (ZoneManagement.IsInZone(GTZone.cave) ? this.muffledLightning : this.regularLightning);
		this.lightningAudio.GTPlay();
	}

	// Token: 0x06002CF5 RID: 11509 RVA: 0x000DE645 File Offset: 0x000DC845
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

	// Token: 0x04003267 RID: 12903
	public int lightMapIndex;

	// Token: 0x04003268 RID: 12904
	public float minTimeBetweenFlashes;

	// Token: 0x04003269 RID: 12905
	public float maxTimeBetweenFlashes;

	// Token: 0x0400326A RID: 12906
	public float flashFadeInDuration;

	// Token: 0x0400326B RID: 12907
	public float flashHoldDuration;

	// Token: 0x0400326C RID: 12908
	public float flashFadeOutDuration;

	// Token: 0x0400326D RID: 12909
	private AudioSource lightningAudio;

	// Token: 0x0400326E RID: 12910
	private SRand rng;

	// Token: 0x0400326F RID: 12911
	private long currentHourlySeed;

	// Token: 0x04003270 RID: 12912
	private List<float> lightningTimestampsRealtime = new List<float>();

	// Token: 0x04003271 RID: 12913
	private int nextLightningTimestampIndex;

	// Token: 0x04003272 RID: 12914
	public AudioClip regularLightning;

	// Token: 0x04003273 RID: 12915
	public AudioClip muffledLightning;

	// Token: 0x04003274 RID: 12916
	private Coroutine lightningRunner;
}
