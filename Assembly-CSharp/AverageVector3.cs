using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000845 RID: 2117
public class AverageVector3
{
	// Token: 0x060033D9 RID: 13273 RVA: 0x00052155 File Offset: 0x00050355
	public AverageVector3(float averagingWindow = 0.1f)
	{
		this.timeWindow = averagingWindow;
	}

	// Token: 0x060033DA RID: 13274 RVA: 0x0013C274 File Offset: 0x0013A474
	public void AddSample(Vector3 sample, float time)
	{
		this.samples.Add(new AverageVector3.Sample
		{
			timeStamp = time,
			value = sample
		});
		this.RefreshSamples();
	}

	// Token: 0x060033DB RID: 13275 RVA: 0x0013C2AC File Offset: 0x0013A4AC
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

	// Token: 0x060033DC RID: 13276 RVA: 0x0005217A File Offset: 0x0005037A
	public void Clear()
	{
		this.samples.Clear();
	}

	// Token: 0x060033DD RID: 13277 RVA: 0x0013C308 File Offset: 0x0013A508
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

	// Token: 0x0400373F RID: 14143
	private List<AverageVector3.Sample> samples = new List<AverageVector3.Sample>();

	// Token: 0x04003740 RID: 14144
	private float timeWindow = 0.1f;

	// Token: 0x02000846 RID: 2118
	public struct Sample
	{
		// Token: 0x04003741 RID: 14145
		public float timeStamp;

		// Token: 0x04003742 RID: 14146
		public Vector3 value;
	}
}
