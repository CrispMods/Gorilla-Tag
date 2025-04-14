using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x02000631 RID: 1585
public class ScienceExperimentSceneElements : MonoBehaviour
{
	// Token: 0x0600276D RID: 10093 RVA: 0x000C1427 File Offset: 0x000BF627
	private void Awake()
	{
		ScienceExperimentManager.instance.InitElements(this);
	}

	// Token: 0x0600276E RID: 10094 RVA: 0x000C1436 File Offset: 0x000BF636
	private void OnDestroy()
	{
		ScienceExperimentManager.instance.DeInitElements();
	}

	// Token: 0x04002B30 RID: 11056
	public List<ScienceExperimentSceneElements.DisableByLiquidData> disableByLiquidList = new List<ScienceExperimentSceneElements.DisableByLiquidData>();

	// Token: 0x04002B31 RID: 11057
	public ParticleSystem sodaFizzParticles;

	// Token: 0x04002B32 RID: 11058
	public ParticleSystem sodaEruptionParticles;

	// Token: 0x02000632 RID: 1586
	[Serializable]
	public struct DisableByLiquidData
	{
		// Token: 0x04002B33 RID: 11059
		public Transform target;

		// Token: 0x04002B34 RID: 11060
		public float heightOffset;
	}
}
