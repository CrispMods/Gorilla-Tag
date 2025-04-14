using System;
using System.Collections;
using System.Threading;
using UnityEngine;

// Token: 0x02000556 RID: 1366
public class GorillaDayNight : MonoBehaviour
{
	// Token: 0x0600215A RID: 8538 RVA: 0x000A5E84 File Offset: 0x000A4084
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

	// Token: 0x0600215B RID: 8539 RVA: 0x000A5F28 File Offset: 0x000A4128
	public void Update()
	{
		if (this.test)
		{
			this.test = false;
			base.StartCoroutine(this.LightMapSet(this.firstData, this.secondData, this.lerpValue));
		}
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x000A5F58 File Offset: 0x000A4158
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

	// Token: 0x0600215D RID: 8541 RVA: 0x000A6164 File Offset: 0x000A4364
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

	// Token: 0x0600215E RID: 8542 RVA: 0x000A6228 File Offset: 0x000A4428
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

	// Token: 0x0600215F RID: 8543 RVA: 0x000A62EB File Offset: 0x000A44EB
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

	// Token: 0x04002509 RID: 9481
	[OnEnterPlay_SetNull]
	public static volatile GorillaDayNight instance;

	// Token: 0x0400250A RID: 9482
	public GorillaLightmapData[] lightmapDatas;

	// Token: 0x0400250B RID: 9483
	private LightmapData[] workingLightMapDatas;

	// Token: 0x0400250C RID: 9484
	private LightmapData workingLightMapData;

	// Token: 0x0400250D RID: 9485
	public float lerpValue;

	// Token: 0x0400250E RID: 9486
	public bool done;

	// Token: 0x0400250F RID: 9487
	public bool finishedStep;

	// Token: 0x04002510 RID: 9488
	private Color[] fromPixels;

	// Token: 0x04002511 RID: 9489
	private Color[] toPixels;

	// Token: 0x04002512 RID: 9490
	private Color[] mixedPixels;

	// Token: 0x04002513 RID: 9491
	public int firstData;

	// Token: 0x04002514 RID: 9492
	public int secondData;

	// Token: 0x04002515 RID: 9493
	public int i;

	// Token: 0x04002516 RID: 9494
	public int j;

	// Token: 0x04002517 RID: 9495
	public int k;

	// Token: 0x04002518 RID: 9496
	public int l;

	// Token: 0x04002519 RID: 9497
	private Thread lightsThread;

	// Token: 0x0400251A RID: 9498
	private Thread dirsThread;

	// Token: 0x0400251B RID: 9499
	public bool test;

	// Token: 0x0400251C RID: 9500
	public bool working;
}
