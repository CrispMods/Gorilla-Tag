using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class GorillaVelocityEstimatorManager : MonoBehaviour
{
	// Token: 0x06000545 RID: 1349 RVA: 0x00033E56 File Offset: 0x00032056
	protected void Awake()
	{
		if (GorillaVelocityEstimatorManager.hasInstance && GorillaVelocityEstimatorManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		GorillaVelocityEstimatorManager.SetInstance(this);
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x00033E79 File Offset: 0x00032079
	protected void OnDestroy()
	{
		if (GorillaVelocityEstimatorManager.instance == this)
		{
			GorillaVelocityEstimatorManager.hasInstance = false;
			GorillaVelocityEstimatorManager.instance = null;
		}
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00081014 File Offset: 0x0007F214
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

	// Token: 0x06000548 RID: 1352 RVA: 0x00033E94 File Offset: 0x00032094
	public static void CreateManager()
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		GorillaVelocityEstimatorManager.SetInstance(new GameObject("GorillaVelocityEstimatorManager").AddComponent<GorillaVelocityEstimatorManager>());
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x00033EB2 File Offset: 0x000320B2
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
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x00033ED5 File Offset: 0x000320D5
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

	// Token: 0x0600054B RID: 1355 RVA: 0x00033F03 File Offset: 0x00032103
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

	// Token: 0x04000623 RID: 1571
	public static GorillaVelocityEstimatorManager instance;

	// Token: 0x04000624 RID: 1572
	public static bool hasInstance = false;

	// Token: 0x04000625 RID: 1573
	public static readonly List<GorillaVelocityEstimator> estimators = new List<GorillaVelocityEstimator>(1024);
}
