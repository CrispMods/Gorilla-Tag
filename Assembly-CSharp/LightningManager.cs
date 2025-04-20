using System;
using System.Collections;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200072F RID: 1839
public class LightningManager : MonoBehaviour
{
	// Token: 0x06002D7E RID: 11646 RVA: 0x0004EE3B File Offset: 0x0004D03B
	private void Start()
	{
		this.lightningAudio = base.GetComponent<AudioSource>();
		GorillaComputer instance = GorillaComputer.instance;
		instance.OnServerTimeUpdated = (Action)Delegate.Combine(instance.OnServerTimeUpdated, new Action(this.OnTimeChanged));
	}

	// Token: 0x06002D7F RID: 11647 RVA: 0x0004EE71 File Offset: 0x0004D071
	private void OnTimeChanged()
	{
		this.InitializeRng();
		if (this.lightningRunner != null)
		{
			base.StopCoroutine(this.lightningRunner);
		}
		this.lightningRunner = base.StartCoroutine(this.LightningEffectRunner());
	}

	// Token: 0x06002D80 RID: 11648 RVA: 0x00128538 File Offset: 0x00126738
	private void GetHourStart(out long seed, out float timestampRealtime)
	{
		DateTime serverTime = GorillaComputer.instance.GetServerTime();
		DateTime d = new DateTime(serverTime.Year, serverTime.Month, serverTime.Day, serverTime.Hour, 0, 0);
		timestampRealtime = Time.realtimeSinceStartup - (float)(serverTime - d).TotalSeconds;
		seed = d.Ticks;
	}

	// Token: 0x06002D81 RID: 11649 RVA: 0x00128598 File Offset: 0x00126798
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

	// Token: 0x06002D82 RID: 11650 RVA: 0x0012865C File Offset: 0x0012685C
	private void DoLightningStrike()
	{
		BetterDayNightManager.instance.AnimateLightFlash(this.lightMapIndex, this.flashFadeInDuration, this.flashHoldDuration, this.flashFadeOutDuration);
		this.lightningAudio.clip = (ZoneManagement.IsInZone(GTZone.cave) ? this.muffledLightning : this.regularLightning);
		this.lightningAudio.GTPlay();
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x0004EE9F File Offset: 0x0004D09F
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

	// Token: 0x040032FE RID: 13054
	public int lightMapIndex;

	// Token: 0x040032FF RID: 13055
	public float minTimeBetweenFlashes;

	// Token: 0x04003300 RID: 13056
	public float maxTimeBetweenFlashes;

	// Token: 0x04003301 RID: 13057
	public float flashFadeInDuration;

	// Token: 0x04003302 RID: 13058
	public float flashHoldDuration;

	// Token: 0x04003303 RID: 13059
	public float flashFadeOutDuration;

	// Token: 0x04003304 RID: 13060
	private AudioSource lightningAudio;

	// Token: 0x04003305 RID: 13061
	private SRand rng;

	// Token: 0x04003306 RID: 13062
	private long currentHourlySeed;

	// Token: 0x04003307 RID: 13063
	private List<float> lightningTimestampsRealtime = new List<float>();

	// Token: 0x04003308 RID: 13064
	private int nextLightningTimestampIndex;

	// Token: 0x04003309 RID: 13065
	public AudioClip regularLightning;

	// Token: 0x0400330A RID: 13066
	public AudioClip muffledLightning;

	// Token: 0x0400330B RID: 13067
	private Coroutine lightningRunner;
}
