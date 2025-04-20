using System;
using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

// Token: 0x02000637 RID: 1591
public class DayNightCycle : MonoBehaviour
{
	// Token: 0x06002777 RID: 10103 RVA: 0x0010CDFC File Offset: 0x0010AFFC
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

	// Token: 0x06002778 RID: 10104 RVA: 0x0010CF80 File Offset: 0x0010B180
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

	// Token: 0x06002779 RID: 10105 RVA: 0x0004AE8E File Offset: 0x0004908E
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

	// Token: 0x04002C6C RID: 11372
	public Texture2D _dayMap;

	// Token: 0x04002C6D RID: 11373
	private Texture2D fromMap;

	// Token: 0x04002C6E RID: 11374
	public Texture2D _sunriseMap;

	// Token: 0x04002C6F RID: 11375
	private Texture2D toMap;

	// Token: 0x04002C70 RID: 11376
	public DayNightCycle.LerpBakedLightingJob job;

	// Token: 0x04002C71 RID: 11377
	public JobHandle jobHandle;

	// Token: 0x04002C72 RID: 11378
	public bool isComplete;

	// Token: 0x04002C73 RID: 11379
	private float startTime;

	// Token: 0x04002C74 RID: 11380
	public float timeTakenStartingJob;

	// Token: 0x04002C75 RID: 11381
	public float timeTakenPostJob;

	// Token: 0x04002C76 RID: 11382
	public float timeTakenDuringJob;

	// Token: 0x04002C77 RID: 11383
	public LightmapData newData;

	// Token: 0x04002C78 RID: 11384
	private Color[] fromPixels;

	// Token: 0x04002C79 RID: 11385
	private Color[] toPixels;

	// Token: 0x04002C7A RID: 11386
	private Color[] mixedPixels;

	// Token: 0x04002C7B RID: 11387
	private LightmapData[] newDatas;

	// Token: 0x04002C7C RID: 11388
	public Texture2D newTexture;

	// Token: 0x04002C7D RID: 11389
	public int textureWidth;

	// Token: 0x04002C7E RID: 11390
	public int textureHeight;

	// Token: 0x04002C7F RID: 11391
	private Color[] workBlockFrom;

	// Token: 0x04002C80 RID: 11392
	private Color[] workBlockTo;

	// Token: 0x04002C81 RID: 11393
	private Color[] workBlockMix;

	// Token: 0x04002C82 RID: 11394
	public int subTextureSize = 1024;

	// Token: 0x04002C83 RID: 11395
	public Texture2D[] subTextureArray;

	// Token: 0x04002C84 RID: 11396
	public bool startCoroutine;

	// Token: 0x04002C85 RID: 11397
	public bool startedCoroutine;

	// Token: 0x04002C86 RID: 11398
	public bool finishedCoroutine;

	// Token: 0x04002C87 RID: 11399
	public bool startJob;

	// Token: 0x04002C88 RID: 11400
	public float switchTimeTaken;

	// Token: 0x04002C89 RID: 11401
	public bool jobStarted;

	// Token: 0x04002C8A RID: 11402
	public float lerpAmount;

	// Token: 0x04002C8B RID: 11403
	public int currentRow;

	// Token: 0x04002C8C RID: 11404
	public int currentColumn;

	// Token: 0x04002C8D RID: 11405
	public int currentSubTexture;

	// Token: 0x04002C8E RID: 11406
	public int currentRowInSubtexture;

	// Token: 0x02000638 RID: 1592
	public struct LerpBakedLightingJob : IJob
	{
		// Token: 0x0600277B RID: 10107 RVA: 0x0010D170 File Offset: 0x0010B370
		public void Execute()
		{
			for (int i = 0; i < this.fromPixels.Length; i++)
			{
				this.mixedPixels[i] = Color.Lerp(this.fromPixels[i], this.toPixels[i], 0.5f);
			}
		}

		// Token: 0x04002C8F RID: 11407
		public NativeArray<Color> fromPixels;

		// Token: 0x04002C90 RID: 11408
		public NativeArray<Color> toPixels;

		// Token: 0x04002C91 RID: 11409
		public NativeArray<Color> mixedPixels;

		// Token: 0x04002C92 RID: 11410
		public float lerpValue;
	}
}
