using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000590 RID: 1424
public class GorillaSlicerSimpleManager : MonoBehaviour
{
	// Token: 0x06002340 RID: 9024 RVA: 0x00046CC7 File Offset: 0x00044EC7
	protected void Awake()
	{
		if (GorillaSlicerSimpleManager.hasInstance && GorillaSlicerSimpleManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		GorillaSlicerSimpleManager.SetInstance(this);
	}

	// Token: 0x06002341 RID: 9025 RVA: 0x000FAFF0 File Offset: 0x000F91F0
	public static void CreateManager()
	{
		GorillaSlicerSimpleManager gorillaSlicerSimpleManager = new GameObject("GorillaSlicerSimpleManager").AddComponent<GorillaSlicerSimpleManager>();
		gorillaSlicerSimpleManager.fixedUpdateSlice = new List<IGorillaSliceableSimple>();
		gorillaSlicerSimpleManager.updateSlice = new List<IGorillaSliceableSimple>();
		gorillaSlicerSimpleManager.lateUpdateSlice = new List<IGorillaSliceableSimple>();
		gorillaSlicerSimpleManager.sW = new Stopwatch();
		GorillaSlicerSimpleManager.SetInstance(gorillaSlicerSimpleManager);
	}

	// Token: 0x06002342 RID: 9026 RVA: 0x00046CEA File Offset: 0x00044EEA
	private static void SetInstance(GorillaSlicerSimpleManager manager)
	{
		GorillaSlicerSimpleManager.instance = manager;
		GorillaSlicerSimpleManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x000FB040 File Offset: 0x000F9240
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

	// Token: 0x06002344 RID: 9028 RVA: 0x000FB0D4 File Offset: 0x000F92D4
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

	// Token: 0x06002345 RID: 9029 RVA: 0x000FB16C File Offset: 0x000F936C
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

	// Token: 0x06002346 RID: 9030 RVA: 0x000FB27C File Offset: 0x000F947C
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

	// Token: 0x06002347 RID: 9031 RVA: 0x000FB360 File Offset: 0x000F9560
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

	// Token: 0x040026D5 RID: 9941
	public static GorillaSlicerSimpleManager instance;

	// Token: 0x040026D6 RID: 9942
	public static bool hasInstance;

	// Token: 0x040026D7 RID: 9943
	public List<IGorillaSliceableSimple> fixedUpdateSlice;

	// Token: 0x040026D8 RID: 9944
	public List<IGorillaSliceableSimple> updateSlice;

	// Token: 0x040026D9 RID: 9945
	public List<IGorillaSliceableSimple> lateUpdateSlice;

	// Token: 0x040026DA RID: 9946
	public long ticksPerFrame = 1500L;

	// Token: 0x040026DB RID: 9947
	public long ticksThisFrame;

	// Token: 0x040026DC RID: 9948
	public int checkEveryXUpdateSteps = 10;

	// Token: 0x040026DD RID: 9949
	public int updateIndex = -1;

	// Token: 0x040026DE RID: 9950
	public Stopwatch sW;

	// Token: 0x02000591 RID: 1425
	public enum UpdateStep
	{
		// Token: 0x040026E0 RID: 9952
		FixedUpdate,
		// Token: 0x040026E1 RID: 9953
		Update,
		// Token: 0x040026E2 RID: 9954
		LateUpdate
	}
}
