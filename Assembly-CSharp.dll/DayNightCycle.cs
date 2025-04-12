using System;
using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

// Token: 0x02000659 RID: 1625
public class DayNightCycle : MonoBehaviour
{
	// Token: 0x06002854 RID: 10324 RVA: 0x0010E9D4 File Offset: 0x0010CBD4
	public void Awake()
	{
		this.fromMap = new Texture2D(this._sunriseMap.width, this._sunriseMap.height);
		this.fromMap = LightmapSettings.lightmaps[0].lightmapColor;
		this.toMap = new Texture2D(this._dayMap.width, this._dayMap.height);
		this.toMap.SetPixels(this._dayMap.GetPixels());
		this.toMap.Apply();
		this.workBlockMix = new Color[this.subTextureSize * this.subTextureSize];
		this.newTexture = new Texture2D(this.fromMap.width, this.fromMap.height, this.fromMap.graphicsFormat, TextureCreationFlags.None);
		this.newData = new LightmapData();
		this.textureHeight = this.fromMap.height;
		this.textureWidth = this.fromMap.width;
		this.subTextureArray = new Texture2D[(int)Mathf.Pow((float)(this.textureHeight / this.subTextureSize), 2f)];
		Debug.Log("aaaa " + this.fromMap.format.ToString());
		Debug.Log("aaaa " + this.fromMap.graphicsFormat.ToString());
		this.startJob = false;
		this.startCoroutine = false;
		this.startedCoroutine = false;
		this.finishedCoroutine = false;
	}

	// Token: 0x06002855 RID: 10325 RVA: 0x0010EB58 File Offset: 0x0010CD58
	public void Update()
	{
		if (this.startJob)
		{
			this.startJob = false;
			this.startTime = Time.realtimeSinceStartup;
			base.StartCoroutine(this.UpdateWork());
			this.timeTakenStartingJob = Time.realtimeSinceStartup - this.startTime;
			this.startTime = Time.realtimeSinceStartup;
		}
		if (this.jobStarted && this.jobHandle.IsCompleted)
		{
			this.timeTakenDuringJob = Time.realtimeSinceStartup - this.startTime;
			this.startTime = Time.realtimeSinceStartup;
			this.jobHandle.Complete();
			this.jobStarted = false;
			this.newTexture.SetPixels(this.job.mixedPixels.ToArray());
			this.newData.lightmapDir = LightmapSettings.lightmaps[0].lightmapDir;
			LightmapSettings.lightmaps = new LightmapData[]
			{
				this.newData
			};
			this.job.fromPixels.Dispose();
			this.job.toPixels.Dispose();
			this.job.mixedPixels.Dispose();
			this.timeTakenPostJob = Time.realtimeSinceStartup - this.startTime;
		}
		if (this.startCoroutine)
		{
			this.startCoroutine = false;
			this.startTime = Time.realtimeSinceStartup;
			this.newTexture = new Texture2D(this.fromMap.width, this.fromMap.height);
			base.StartCoroutine(this.UpdateWork());
		}
		if (this.startedCoroutine && this.finishedCoroutine)
		{
			this.startedCoroutine = false;
			this.finishedCoroutine = false;
			this.timeTakenDuringJob = Time.realtimeSinceStartup - this.startTime;
			this.startTime = Time.realtimeSinceStartup;
			this.newData = LightmapSettings.lightmaps[0];
			this.newData.lightmapColor = this.fromMap;
			LightmapData[] lightmaps = LightmapSettings.lightmaps;
			lightmaps[0].lightmapColor = this.fromMap;
			LightmapSettings.lightmaps = lightmaps;
			this.timeTakenPostJob = Time.realtimeSinceStartup - this.startTime;
		}
	}

	// Token: 0x06002856 RID: 10326 RVA: 0x0004A8F9 File Offset: 0x00048AF9
	public IEnumerator UpdateWork()
	{
		yield return 0;
		this.timeTakenStartingJob = Time.realtimeSinceStartup - this.startTime;
		this.startTime = Time.realtimeSinceStartup;
		this.startedCoroutine = true;
		this.currentSubTexture = 0;
		int num;
		for (int i = 0; i < this.subTextureArray.Length; i = num + 1)
		{
			this.subTextureArray[i] = new Texture2D(this.subTextureSize, this.subTextureSize, this.fromMap.graphicsFormat, TextureCreationFlags.None);
			yield return 0;
			num = i;
		}
		for (int i = 0; i < this.textureWidth / this.subTextureSize; i = num + 1)
		{
			this.currentColumn = i;
			for (int j = 0; j < this.textureHeight / this.subTextureSize; j = num + 1)
			{
				this.currentRow = j;
				this.workBlockFrom = this.fromMap.GetPixels(i * this.subTextureSize, j * this.subTextureSize, this.subTextureSize, this.subTextureSize);
				this.workBlockTo = this.toMap.GetPixels(i * this.subTextureSize, j * this.subTextureSize, this.subTextureSize, this.subTextureSize);
				for (int k = 0; k < this.subTextureSize * this.subTextureSize - 1; k++)
				{
					this.workBlockMix[k] = Color.Lerp(this.workBlockFrom[k], this.workBlockTo[k], this.lerpAmount);
				}
				this.subTextureArray[j * (this.textureWidth / this.subTextureSize) + i].SetPixels(0, 0, this.subTextureSize, this.subTextureSize, this.workBlockMix);
				yield return 0;
				num = j;
			}
			num = i;
		}
		for (int i = 0; i < this.subTextureArray.Length; i = num + 1)
		{
			this.currentSubTexture = i;
			this.subTextureArray[i].Apply();
			yield return 0;
			Graphics.CopyTexture(this.subTextureArray[i], 0, 0, 0, 0, this.subTextureSize, this.subTextureSize, this.newTexture, 0, 0, i * this.subTextureSize % this.textureHeight, (int)Mathf.Floor((float)(this.subTextureSize * i / this.textureHeight)) * this.subTextureSize);
			yield return 0;
			num = i;
		}
		this.finishedCoroutine = true;
		yield break;
	}

	// Token: 0x04002D0C RID: 11532
	public Texture2D _dayMap;

	// Token: 0x04002D0D RID: 11533
	private Texture2D fromMap;

	// Token: 0x04002D0E RID: 11534
	public Texture2D _sunriseMap;

	// Token: 0x04002D0F RID: 11535
	private Texture2D toMap;

	// Token: 0x04002D10 RID: 11536
	public DayNightCycle.LerpBakedLightingJob job;

	// Token: 0x04002D11 RID: 11537
	public JobHandle jobHandle;

	// Token: 0x04002D12 RID: 11538
	public bool isComplete;

	// Token: 0x04002D13 RID: 11539
	private float startTime;

	// Token: 0x04002D14 RID: 11540
	public float timeTakenStartingJob;

	// Token: 0x04002D15 RID: 11541
	public float timeTakenPostJob;

	// Token: 0x04002D16 RID: 11542
	public float timeTakenDuringJob;

	// Token: 0x04002D17 RID: 11543
	public LightmapData newData;

	// Token: 0x04002D18 RID: 11544
	private Color[] fromPixels;

	// Token: 0x04002D19 RID: 11545
	private Color[] toPixels;

	// Token: 0x04002D1A RID: 11546
	private Color[] mixedPixels;

	// Token: 0x04002D1B RID: 11547
	private LightmapData[] newDatas;

	// Token: 0x04002D1C RID: 11548
	public Texture2D newTexture;

	// Token: 0x04002D1D RID: 11549
	public int textureWidth;

	// Token: 0x04002D1E RID: 11550
	public int textureHeight;

	// Token: 0x04002D1F RID: 11551
	private Color[] workBlockFrom;

	// Token: 0x04002D20 RID: 11552
	private Color[] workBlockTo;

	// Token: 0x04002D21 RID: 11553
	private Color[] workBlockMix;

	// Token: 0x04002D22 RID: 11554
	public int subTextureSize = 1024;

	// Token: 0x04002D23 RID: 11555
	public Texture2D[] subTextureArray;

	// Token: 0x04002D24 RID: 11556
	public bool startCoroutine;

	// Token: 0x04002D25 RID: 11557
	public bool startedCoroutine;

	// Token: 0x04002D26 RID: 11558
	public bool finishedCoroutine;

	// Token: 0x04002D27 RID: 11559
	public bool startJob;

	// Token: 0x04002D28 RID: 11560
	public float switchTimeTaken;

	// Token: 0x04002D29 RID: 11561
	public bool jobStarted;

	// Token: 0x04002D2A RID: 11562
	public float lerpAmount;

	// Token: 0x04002D2B RID: 11563
	public int currentRow;

	// Token: 0x04002D2C RID: 11564
	public int currentColumn;

	// Token: 0x04002D2D RID: 11565
	public int currentSubTexture;

	// Token: 0x04002D2E RID: 11566
	public int currentRowInSubtexture;

	// Token: 0x0200065A RID: 1626
	public struct LerpBakedLightingJob : IJob
	{
		// Token: 0x06002858 RID: 10328 RVA: 0x0010ED48 File Offset: 0x0010CF48
		public void Execute()
		{
			for (int i = 0; i < this.fromPixels.Length; i++)
			{
				this.mixedPixels[i] = Color.Lerp(this.fromPixels[i], this.toPixels[i], 0.5f);
			}
		}

		// Token: 0x04002D2F RID: 11567
		public NativeArray<Color> fromPixels;

		// Token: 0x04002D30 RID: 11568
		public NativeArray<Color> toPixels;

		// Token: 0x04002D31 RID: 11569
		public NativeArray<Color> mixedPixels;

		// Token: 0x04002D32 RID: 11570
		public float lerpValue;
	}
}
