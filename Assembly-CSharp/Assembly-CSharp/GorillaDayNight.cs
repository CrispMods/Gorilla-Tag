using System;
using System.Collections;
using System.Threading;
using UnityEngine;

// Token: 0x02000557 RID: 1367
public class GorillaDayNight : MonoBehaviour
{
	// Token: 0x06002162 RID: 8546 RVA: 0x000A6304 File Offset: 0x000A4504
	public void Awake()
	{
		if (GorillaDayNight.instance == null)
		{
			GorillaDayNight.instance = this;
		}
		else if (GorillaDayNight.instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		this.test = false;
		this.working = false;
		this.lerpValue = 0.5f;
		this.workingLightMapDatas = new LightmapData[3];
		this.workingLightMapData = new LightmapData();
		this.workingLightMapData.lightmapColor = this.lightmapDatas[0].lightTextures[0];
		this.workingLightMapData.lightmapDir = this.lightmapDatas[0].dirTextures[0];
	}

	// Token: 0x06002163 RID: 8547 RVA: 0x000A63A8 File Offset: 0x000A45A8
	public void Update()
	{
		if (this.test)
		{
			this.test = false;
			base.StartCoroutine(this.LightMapSet(this.firstData, this.secondData, this.lerpValue));
		}
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x000A63D8 File Offset: 0x000A45D8
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

	// Token: 0x06002165 RID: 8549 RVA: 0x000A65E4 File Offset: 0x000A47E4
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

	// Token: 0x06002166 RID: 8550 RVA: 0x000A66A8 File Offset: 0x000A48A8
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

	// Token: 0x06002167 RID: 8551 RVA: 0x000A676B File Offset: 0x000A496B
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

	// Token: 0x0400250F RID: 9487
	[OnEnterPlay_SetNull]
	public static volatile GorillaDayNight instance;

	// Token: 0x04002510 RID: 9488
	public GorillaLightmapData[] lightmapDatas;

	// Token: 0x04002511 RID: 9489
	private LightmapData[] workingLightMapDatas;

	// Token: 0x04002512 RID: 9490
	private LightmapData workingLightMapData;

	// Token: 0x04002513 RID: 9491
	public float lerpValue;

	// Token: 0x04002514 RID: 9492
	public bool done;

	// Token: 0x04002515 RID: 9493
	public bool finishedStep;

	// Token: 0x04002516 RID: 9494
	private Color[] fromPixels;

	// Token: 0x04002517 RID: 9495
	private Color[] toPixels;

	// Token: 0x04002518 RID: 9496
	private Color[] mixedPixels;

	// Token: 0x04002519 RID: 9497
	public int firstData;

	// Token: 0x0400251A RID: 9498
	public int secondData;

	// Token: 0x0400251B RID: 9499
	public int i;

	// Token: 0x0400251C RID: 9500
	public int j;

	// Token: 0x0400251D RID: 9501
	public int k;

	// Token: 0x0400251E RID: 9502
	public int l;

	// Token: 0x0400251F RID: 9503
	private Thread lightsThread;

	// Token: 0x04002520 RID: 9504
	private Thread dirsThread;

	// Token: 0x04002521 RID: 9505
	public bool test;

	// Token: 0x04002522 RID: 9506
	public bool working;
}
