using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200082B RID: 2091
public class AverageVector3
{
	// Token: 0x0600331E RID: 13086 RVA: 0x000F4485 File Offset: 0x000F2685
	public AverageVector3(float averagingWindow = 0.1f)
	{
		this.timeWindow = averagingWindow;
	}

	// Token: 0x0600331F RID: 13087 RVA: 0x000F44AC File Offset: 0x000F26AC
	public void AddSample(Vector3 sample, float time)
	{
		this.samples.Add(new AverageVector3.Sample
		{
			timeStamp = time,
			value = sample
		});
		this.RefreshSamples();
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x000F44E4 File Offset: 0x000F26E4
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

	// Token: 0x06003321 RID: 13089 RVA: 0x000F453F File Offset: 0x000F273F
	public void Clear()
	{
		this.samples.Clear();
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x000F454C File Offset: 0x000F274C
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

	// Token: 0x04003683 RID: 13955
	private List<AverageVector3.Sample> samples = new List<AverageVector3.Sample>();

	// Token: 0x04003684 RID: 13956
	private float timeWindow = 0.1f;

	// Token: 0x0200082C RID: 2092
	public struct Sample
	{
		// Token: 0x04003685 RID: 13957
		public float timeStamp;

		// Token: 0x04003686 RID: 13958
		public Vector3 value;
	}
}
