using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200059D RID: 1437
public class GorillaSlicerSimpleManager : MonoBehaviour
{
	// Token: 0x06002398 RID: 9112 RVA: 0x000480C5 File Offset: 0x000462C5
	protected void Awake()
	{
		if (GorillaSlicerSimpleManager.hasInstance && GorillaSlicerSimpleManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		GorillaSlicerSimpleManager.SetInstance(this);
	}

	// Token: 0x06002399 RID: 9113 RVA: 0x000FDD6C File Offset: 0x000FBF6C
	public static void CreateManager()
	{
		GorillaSlicerSimpleManager gorillaSlicerSimpleManager = new GameObject("GorillaSlicerSimpleManager").AddComponent<GorillaSlicerSimpleManager>();
		gorillaSlicerSimpleManager.fixedUpdateSlice = new List<IGorillaSliceableSimple>();
		gorillaSlicerSimpleManager.updateSlice = new List<IGorillaSliceableSimple>();
		gorillaSlicerSimpleManager.lateUpdateSlice = new List<IGorillaSliceableSimple>();
		gorillaSlicerSimpleManager.sW = new Stopwatch();
		GorillaSlicerSimpleManager.SetInstance(gorillaSlicerSimpleManager);
	}

	// Token: 0x0600239A RID: 9114 RVA: 0x000480E8 File Offset: 0x000462E8
	private static void SetInstance(GorillaSlicerSimpleManager manager)
	{
		GorillaSlicerSimpleManager.instance = manager;
		GorillaSlicerSimpleManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x0600239B RID: 9115 RVA: 0x000FDDBC File Offset: 0x000FBFBC
	public static void RegisterSliceable(IGorillaSliceableSimple gSS, GorillaSlicerSimpleManager.UpdateStep step)
	{
		if (!GorillaSlicerSimpleManager.hasInstance)
		{
			GorillaSlicerSimpleManager.CreateManager();
		}
		switch (step)
		{
		case GorillaSlicerSimpleManager.UpdateStep.FixedUpdate:
			if (!GorillaSlicerSimpleManager.instance.fixedUpdateSlice.Contains(gSS))
			{
				GorillaSlicerSimpleManager.instance.fixedUpdateSlice.Add(gSS);
				return;
			}
			break;
		case GorillaSlicerSimpleManager.UpdateStep.Update:
			if (!GorillaSlicerSimpleManager.instance.updateSlice.Contains(gSS))
			{
				GorillaSlicerSimpleManager.instance.updateSlice.Add(gSS);
				return;
			}
			break;
		case GorillaSlicerSimpleManager.UpdateStep.LateUpdate:
			if (!GorillaSlicerSimpleManager.instance.lateUpdateSlice.Contains(gSS))
			{
				GorillaSlicerSimpleManager.instance.lateUpdateSlice.Add(gSS);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600239C RID: 9116 RVA: 0x000FDE50 File Offset: 0x000FC050
	public static void UnregisterSliceable(IGorillaSliceableSimple gSS, GorillaSlicerSimpleManager.UpdateStep step)
	{
		if (!GorillaSlicerSimpleManager.hasInstance)
		{
			GorillaSlicerSimpleManager.CreateManager();
		}
		switch (step)
		{
		case GorillaSlicerSimpleManager.UpdateStep.FixedUpdate:
			if (GorillaSlicerSimpleManager.instance.fixedUpdateSlice.Contains(gSS))
			{
				GorillaSlicerSimpleManager.instance.fixedUpdateSlice.Remove(gSS);
				return;
			}
			break;
		case GorillaSlicerSimpleManager.UpdateStep.Update:
			if (GorillaSlicerSimpleManager.instance.updateSlice.Contains(gSS))
			{
				GorillaSlicerSimpleManager.instance.updateSlice.Remove(gSS);
				return;
			}
			break;
		case GorillaSlicerSimpleManager.UpdateStep.LateUpdate:
			if (GorillaSlicerSimpleManager.instance.lateUpdateSlice.Contains(gSS))
			{
				GorillaSlicerSimpleManager.instance.lateUpdateSlice.Remove(gSS);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600239D RID: 9117 RVA: 0x000FDEE8 File Offset: 0x000FC0E8
	public void FixedUpdate()
	{
		if (this.updateIndex < 0 || this.updateIndex >= this.fixedUpdateSlice.Count + this.updateSlice.Count + this.lateUpdateSlice.Count)
		{
			this.updateIndex = 0;
		}
		this.ticksThisFrame = 0L;
		this.sW.Restart();
		while (this.ticksThisFrame + this.sW.ElapsedTicks < this.ticksPerFrame && this.updateIndex < this.fixedUpdateSlice.Count)
		{
			int num = 0;
			while (num < this.checkEveryXUpdateSteps && this.updateIndex < this.fixedUpdateSlice.Count)
			{
				if (0 <= this.updateIndex && this.updateIndex < this.fixedUpdateSlice.Count && this.fixedUpdateSlice[this.updateIndex].isActiveAndEnabled)
				{
					this.fixedUpdateSlice[this.updateIndex].SliceUpdate();
				}
				this.updateIndex++;
				num++;
			}
		}
		this.ticksThisFrame += this.sW.ElapsedTicks;
		this.sW.Stop();
	}

	// Token: 0x0600239E RID: 9118 RVA: 0x000FE014 File Offset: 0x000FC214
	public void Update()
	{
		int count = this.fixedUpdateSlice.Count;
		int count2 = this.updateSlice.Count;
		int num = count + count2;
		this.sW.Restart();
		while (this.ticksThisFrame + this.sW.ElapsedTicks < this.ticksPerFrame && count <= this.updateIndex && this.updateIndex < num)
		{
			int num2 = 0;
			while (num2 < this.checkEveryXUpdateSteps && this.updateIndex < num)
			{
				if (0 <= this.updateIndex - count && this.updateIndex - count < this.updateSlice.Count && this.updateSlice[this.updateIndex - count].isActiveAndEnabled)
				{
					this.updateSlice[this.updateIndex - count].SliceUpdate();
				}
				this.updateIndex++;
				num2++;
			}
		}
		this.ticksThisFrame += this.sW.ElapsedTicks;
		this.sW.Stop();
	}

	// Token: 0x0600239F RID: 9119 RVA: 0x000FE118 File Offset: 0x000FC318
	public void LateUpdate()
	{
		int count = this.fixedUpdateSlice.Count;
		int count2 = this.updateSlice.Count;
		int count3 = this.lateUpdateSlice.Count;
		int num = count + count2;
		int num2 = num + count3;
		this.sW.Restart();
		while (this.ticksThisFrame + this.sW.ElapsedTicks < this.ticksPerFrame && num <= this.updateIndex && this.updateIndex < num2)
		{
			int num3 = 0;
			while (num3 < this.checkEveryXUpdateSteps && this.updateIndex < num2)
			{
				if (0 <= this.updateIndex - num && this.updateIndex - num < this.lateUpdateSlice.Count && this.lateUpdateSlice[this.updateIndex - num].isActiveAndEnabled)
				{
					this.lateUpdateSlice[this.updateIndex - num].SliceUpdate();
				}
				this.updateIndex++;
				num3++;
			}
		}
		this.ticksThisFrame += this.sW.ElapsedTicks;
		this.sW.Stop();
		if (this.updateIndex >= num2)
		{
			this.updateIndex = -1;
		}
	}

	// Token: 0x0400272A RID: 10026
	public static GorillaSlicerSimpleManager instance;

	// Token: 0x0400272B RID: 10027
	public static bool hasInstance;

	// Token: 0x0400272C RID: 10028
	public List<IGorillaSliceableSimple> fixedUpdateSlice;

	// Token: 0x0400272D RID: 10029
	public List<IGorillaSliceableSimple> updateSlice;

	// Token: 0x0400272E RID: 10030
	public List<IGorillaSliceableSimple> lateUpdateSlice;

	// Token: 0x0400272F RID: 10031
	public long ticksPerFrame = 1500L;

	// Token: 0x04002730 RID: 10032
	public long ticksThisFrame;

	// Token: 0x04002731 RID: 10033
	public int checkEveryXUpdateSteps = 10;

	// Token: 0x04002732 RID: 10034
	public int updateIndex = -1;

	// Token: 0x04002733 RID: 10035
	public Stopwatch sW;

	// Token: 0x0200059E RID: 1438
	public enum UpdateStep
	{
		// Token: 0x04002735 RID: 10037
		FixedUpdate,
		// Token: 0x04002736 RID: 10038
		Update,
		// Token: 0x04002737 RID: 10039
		LateUpdate
	}
}
