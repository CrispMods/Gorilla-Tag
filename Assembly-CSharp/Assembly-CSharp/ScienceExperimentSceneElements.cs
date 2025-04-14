using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x02000632 RID: 1586
public class ScienceExperimentSceneElements : MonoBehaviour
{
	// Token: 0x06002775 RID: 10101 RVA: 0x000C18A7 File Offset: 0x000BFAA7
	private void Awake()
	{
		ScienceExperimentManager.instance.InitElements(this);
	}

	// Token: 0x06002776 RID: 10102 RVA: 0x000C18B6 File Offset: 0x000BFAB6
	private void OnDestroy()
	{
		ScienceExperimentManager.instance.DeInitElements();
	}

	// Token: 0x04002B36 RID: 11062
	public List<ScienceExperimentSceneElements.DisableByLiquidData> disableByLiquidList = new List<ScienceExperimentSceneElements.DisableByLiquidData>();

	// Token: 0x04002B37 RID: 11063
	public ParticleSystem sodaFizzParticles;

	// Token: 0x04002B38 RID: 11064
	public ParticleSystem sodaEruptionParticles;

	// Token: 0x02000633 RID: 1587
	[Serializable]
	public struct DisableByLiquidData
	{
		// Token: 0x04002B39 RID: 11065
		public Transform target;

		// Token: 0x04002B3A RID: 11066
		public float heightOffset;
	}
}
