using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x020000C4 RID: 196
public class GorillaVelocityEstimatorManager : MonoBehaviour
{
	// Token: 0x06000509 RID: 1289 RVA: 0x0001DDD2 File Offset: 0x0001BFD2
	protected void Awake()
	{
		if (GorillaVelocityEstimatorManager.hasInstance && GorillaVelocityEstimatorManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		GorillaVelocityEstimatorManager.SetInstance(this);
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x0001DDF5 File Offset: 0x0001BFF5
	protected void OnDestroy()
	{
		if (GorillaVelocityEstimatorManager.instance == this)
		{
			GorillaVelocityEstimatorManager.hasInstance = false;
			GorillaVelocityEstimatorManager.instance = null;
		}
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x0001DE10 File Offset: 0x0001C010
	protected void LateUpdate()
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		for (int i = 0; i < GorillaVelocityEstimatorManager.estimators.Count; i++)
		{
			if (GorillaVelocityEstimatorManager.estimators[i] != null)
			{
				GorillaVelocityEstimatorManager.estimators[i].TriggeredLateUpdate();
			}
		}
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x0001DE5D File Offset: 0x0001C05D
	public static void CreateManager()
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		GorillaVelocityEstimatorManager.SetInstance(new GameObject("GorillaVelocityEstimatorManager").AddComponent<GorillaVelocityEstimatorManager>());
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x0001DE7B File Offset: 0x0001C07B
	private static void SetInstance(GorillaVelocityEstimatorManager manager)
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		GorillaVelocityEstimatorManager.instance = manager;
		GorillaVelocityEstimatorManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x0001DE9E File Offset: 0x0001C09E
	public static void Register(GorillaVelocityEstimator velEstimator)
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		if (!GorillaVelocityEstimatorManager.hasInstance)
		{
			GorillaVelocityEstimatorManager.CreateManager();
		}
		if (!GorillaVelocityEstimatorManager.estimators.Contains(velEstimator))
		{
			GorillaVelocityEstimatorManager.estimators.Add(velEstimator);
		}
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x0001DECC File Offset: 0x0001C0CC
	public static void Unregister(GorillaVelocityEstimator velEstimator)
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		if (!GorillaVelocityEstimatorManager.hasInstance)
		{
			GorillaVelocityEstimatorManager.CreateManager();
		}
		if (GorillaVelocityEstimatorManager.estimators.Contains(velEstimator))
		{
			GorillaVelocityEstimatorManager.estimators.Remove(velEstimator);
		}
	}

	// Token: 0x040005E3 RID: 1507
	public static GorillaVelocityEstimatorManager instance;

	// Token: 0x040005E4 RID: 1508
	public static bool hasInstance = false;

	// Token: 0x040005E5 RID: 1509
	public static readonly List<GorillaVelocityEstimator> estimators = new List<GorillaVelocityEstimator>(1024);
}
