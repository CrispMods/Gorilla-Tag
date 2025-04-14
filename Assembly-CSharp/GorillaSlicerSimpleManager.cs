using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200058F RID: 1423
public class GorillaSlicerSimpleManager : MonoBehaviour
{
	// Token: 0x06002338 RID: 9016 RVA: 0x000AE76D File Offset: 0x000AC96D
	protected void Awake()
	{
		if (GorillaSlicerSimpleManager.hasInstance && GorillaSlicerSimpleManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		GorillaSlicerSimpleManager.SetInstance(this);
	}

	// Token: 0x06002339 RID: 9017 RVA: 0x000AE790 File Offset: 0x000AC990
	public static void CreateManager()
	{
		GorillaSlicerSimpleManager gorillaSlicerSimpleManager = new GameObject("GorillaSlicerSimpleManager").AddComponent<GorillaSlicerSimpleManager>();
		gorillaSlicerSimpleManager.fixedUpdateSlice = new List<IGorillaSliceableSimple>();
		gorillaSlicerSimpleManager.updateSlice = new List<IGorillaSliceableSimple>();
		gorillaSlicerSimpleManager.lateUpdateSlice = new List<IGorillaSliceableSimple>();
		gorillaSlicerSimpleManager.sW = new Stopwatch();
		GorillaSlicerSimpleManager.SetInstance(gorillaSlicerSimpleManager);
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x000AE7DD File Offset: 0x000AC9DD
	private static void SetInstance(GorillaSlicerSimpleManager manager)
	{
		GorillaSlicerSimpleManager.instance = manager;
		GorillaSlicerSimpleManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x000AE7F8 File Offset: 0x000AC9F8
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

	// Token: 0x0600233C RID: 9020 RVA: 0x000AE88C File Offset: 0x000ACA8C
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

	// Token: 0x0600233D RID: 9021 RVA: 0x000AE924 File Offset: 0x000ACB24
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
				if (this.fixedUpdateSlice[this.updateIndex].isActiveAndEnabled)
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

	// Token: 0x0600233E RID: 9022 RVA: 0x000AEA34 File Offset: 0x000ACC34
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
				if (this.updateSlice[this.updateIndex - count].isActiveAndEnabled)
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

	// Token: 0x0600233F RID: 9023 RVA: 0x000AEB18 File Offset: 0x000ACD18
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
				if (this.lateUpdateSlice[this.updateIndex - num].isActiveAndEnabled)
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

	// Token: 0x040026CF RID: 9935
	public static GorillaSlicerSimpleManager instance;

	// Token: 0x040026D0 RID: 9936
	public static bool hasInstance;

	// Token: 0x040026D1 RID: 9937
	public List<IGorillaSliceableSimple> fixedUpdateSlice;

	// Token: 0x040026D2 RID: 9938
	public List<IGorillaSliceableSimple> updateSlice;

	// Token: 0x040026D3 RID: 9939
	public List<IGorillaSliceableSimple> lateUpdateSlice;

	// Token: 0x040026D4 RID: 9940
	public long ticksPerFrame = 1500L;

	// Token: 0x040026D5 RID: 9941
	public long ticksThisFrame;

	// Token: 0x040026D6 RID: 9942
	public int checkEveryXUpdateSteps = 10;

	// Token: 0x040026D7 RID: 9943
	public int updateIndex = -1;

	// Token: 0x040026D8 RID: 9944
	public Stopwatch sW;

	// Token: 0x02000590 RID: 1424
	public enum UpdateStep
	{
		// Token: 0x040026DA RID: 9946
		FixedUpdate,
		// Token: 0x040026DB RID: 9947
		Update,
		// Token: 0x040026DC RID: 9948
		LateUpdate
	}
}
