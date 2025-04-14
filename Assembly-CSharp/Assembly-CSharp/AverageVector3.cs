using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200082E RID: 2094
public class AverageVector3
{
	// Token: 0x0600332A RID: 13098 RVA: 0x000F4A4D File Offset: 0x000F2C4D
	public AverageVector3(float averagingWindow = 0.1f)
	{
		this.timeWindow = averagingWindow;
	}

	// Token: 0x0600332B RID: 13099 RVA: 0x000F4A74 File Offset: 0x000F2C74
	public void AddSample(Vector3 sample, float time)
	{
		this.samples.Add(new AverageVector3.Sample
		{
			timeStamp = time,
			value = sample
		});
		this.RefreshSamples();
	}

	// Token: 0x0600332C RID: 13100 RVA: 0x000F4AAC File Offset: 0x000F2CAC
	public Vector3 GetAverage()
	{
		this.RefreshSamples();
		Vector3 a = Vector3.zero;
		for (int i = 0; i < this.samples.Count; i++)
		{
			a += this.samples[i].value;
		}
		return a / (float)this.samples.Count;
	}

	// Token: 0x0600332D RID: 13101 RVA: 0x000F4B07 File Offset: 0x000F2D07
	public void Clear()
	{
		this.samples.Clear();
	}

	// Token: 0x0600332E RID: 13102 RVA: 0x000F4B14 File Offset: 0x000F2D14
	private void RefreshSamples()
	{
		float num = Time.time - this.timeWindow;
		for (int i = this.samples.Count - 1; i >= 0; i--)
		{
			if (this.samples[i].timeStamp < num)
			{
				this.samples.RemoveAt(i);
			}
		}
	}

	// Token: 0x04003695 RID: 13973
	private List<AverageVector3.Sample> samples = new List<AverageVector3.Sample>();

	// Token: 0x04003696 RID: 13974
	private float timeWindow = 0.1f;

	// Token: 0x0200082F RID: 2095
	public struct Sample
	{
		// Token: 0x04003697 RID: 13975
		public float timeStamp;

		// Token: 0x04003698 RID: 13976
		public Vector3 value;
	}
}
