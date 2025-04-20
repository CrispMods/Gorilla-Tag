using System;
using System.Collections;
using System.Threading;
using UnityEngine;

// Token: 0x02000564 RID: 1380
public class GorillaDayNight : MonoBehaviour
{
	// Token: 0x060021B8 RID: 8632 RVA: 0x000F6598 File Offset: 0x000F4798
	public void Awake()
	{
		if (GorillaDayNight.instance == null)
		{
			GorillaDayNight.instance = this;
		}
		else if (GorillaDayNight.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.test = false;
		this.working = false;
		this.lerpValue = 0.5f;
		this.workingLightMapDatas = new LightmapData[3];
		this.workingLightMapData = new LightmapData();
		this.workingLightMapData.lightmapColor = this.lightmapDatas[0].lightTextures[0];
		this.workingLightMapData.lightmapDir = this.lightmapDatas[0].dirTextures[0];
	}

	// Token: 0x060021B9 RID: 8633 RVA: 0x00046F64 File Offset: 0x00045164
	public void Update()
	{
		if (this.test)
		{
			this.test = false;
			base.StartCoroutine(this.LightMapSet(this.firstData, this.secondData, this.lerpValue));
		}
	}

	// Token: 0x060021BA RID: 8634 RVA: 0x000F663C File Offset: 0x000F483C
	public void DoWork()
	{
		this.k = 0;
		while (this.k < this.lightmapDatas[this.firstData].lights.Length)
		{
			this.fromPixels = this.lightmapDatas[this.firstData].lights[this.k];
			this.toPixels = this.lightmapDatas[this.secondData].lights[this.k];
			this.mixedPixels = this.fromPixels;
			this.j = 0;
			while (this.j < this.mixedPixels.Length)
			{
				this.mixedPixels[this.j] = Color.Lerp(this.fromPixels[this.j], this.toPixels[this.j], this.lerpValue);
				this.j++;
			}
			this.workingLightMapData.lightmapColor.SetPixels(this.mixedPixels);
			this.workingLightMapData.lightmapDir.Apply(false);
			this.fromPixels = this.lightmapDatas[this.firstData].dirs[this.k];
			this.toPixels = this.lightmapDatas[this.secondData].dirs[this.k];
			this.mixedPixels = this.fromPixels;
			this.j = 0;
			while (this.j < this.mixedPixels.Length)
			{
				this.mixedPixels[this.j] = Color.Lerp(this.fromPixels[this.j], this.toPixels[this.j], this.lerpValue);
				this.j++;
			}
			this.workingLightMapData.lightmapDir.SetPixels(this.mixedPixels);
			this.workingLightMapData.lightmapDir.Apply(false);
			this.workingLightMapDatas[this.k] = this.workingLightMapData;
			this.k++;
		}
		this.done = true;
	}

	// Token: 0x060021BB RID: 8635 RVA: 0x000F6848 File Offset: 0x000F4A48
	public void DoLightsStep()
	{
		this.fromPixels = this.lightmapDatas[this.firstData].lights[this.k];
		this.toPixels = this.lightmapDatas[this.secondData].lights[this.k];
		this.mixedPixels = this.fromPixels;
		this.j = 0;
		while (this.j < this.mixedPixels.Length)
		{
			this.mixedPixels[this.j] = Color.Lerp(this.fromPixels[this.j], this.toPixels[this.j], this.lerpValue);
			this.j++;
		}
		this.finishedStep = true;
	}

	// Token: 0x060021BC RID: 8636 RVA: 0x000F690C File Offset: 0x000F4B0C
	public void DoDirsStep()
	{
		this.fromPixels = this.lightmapDatas[this.firstData].dirs[this.k];
		this.toPixels = this.lightmapDatas[this.secondData].dirs[this.k];
		this.mixedPixels = this.fromPixels;
		this.j = 0;
		while (this.j < this.mixedPixels.Length)
		{
			this.mixedPixels[this.j] = Color.Lerp(this.fromPixels[this.j], this.toPixels[this.j], this.lerpValue);
			this.j++;
		}
		this.finishedStep = true;
	}

	// Token: 0x060021BD RID: 8637 RVA: 0x00046F94 File Offset: 0x00045194
	private IEnumerator LightMapSet(int setFirstData, int setSecondData, float setLerp)
	{
		this.working = true;
		this.firstData = setFirstData;
		this.secondData = setSecondData;
		this.lerpValue = setLerp;
		this.k = 0;
		while (this.k < this.lightmapDatas[this.firstData].lights.Length)
		{
			this.lightsThread = new Thread(new ThreadStart(this.DoLightsStep));
			this.lightsThread.Start();
			yield return new WaitUntil(() => this.finishedStep);
			this.finishedStep = false;
			this.workingLightMapData.lightmapColor.SetPixels(this.mixedPixels);
			this.workingLightMapData.lightmapColor.Apply(false);
			this.dirsThread = new Thread(new ThreadStart(this.DoDirsStep));
			this.dirsThread.Start();
			yield return new WaitUntil(() => this.finishedStep);
			this.finishedStep = false;
			this.workingLightMapData.lightmapDir.SetPixels(this.mixedPixels);
			this.workingLightMapData.lightmapDir.Apply(false);
			this.workingLightMapDatas[this.k] = this.workingLightMapData;
			this.k++;
		}
		LightmapSettings.lightmaps = this.workingLightMapDatas;
		this.working = false;
		this.done = true;
		yield break;
	}

	// Token: 0x04002561 RID: 9569
	[OnEnterPlay_SetNull]
	public static volatile GorillaDayNight instance;

	// Token: 0x04002562 RID: 9570
	public GorillaLightmapData[] lightmapDatas;

	// Token: 0x04002563 RID: 9571
	private LightmapData[] workingLightMapDatas;

	// Token: 0x04002564 RID: 9572
	private LightmapData workingLightMapData;

	// Token: 0x04002565 RID: 9573
	public float lerpValue;

	// Token: 0x04002566 RID: 9574
	public bool done;

	// Token: 0x04002567 RID: 9575
	public bool finishedStep;

	// Token: 0x04002568 RID: 9576
	private Color[] fromPixels;

	// Token: 0x04002569 RID: 9577
	private Color[] toPixels;

	// Token: 0x0400256A RID: 9578
	private Color[] mixedPixels;

	// Token: 0x0400256B RID: 9579
	public int firstData;

	// Token: 0x0400256C RID: 9580
	public int secondData;

	// Token: 0x0400256D RID: 9581
	public int i;

	// Token: 0x0400256E RID: 9582
	public int j;

	// Token: 0x0400256F RID: 9583
	public int k;

	// Token: 0x04002570 RID: 9584
	public int l;

	// Token: 0x04002571 RID: 9585
	private Thread lightsThread;

	// Token: 0x04002572 RID: 9586
	private Thread dirsThread;

	// Token: 0x04002573 RID: 9587
	public bool test;

	// Token: 0x04002574 RID: 9588
	public bool working;
}
