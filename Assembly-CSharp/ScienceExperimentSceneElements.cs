using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x02000610 RID: 1552
public class ScienceExperimentSceneElements : MonoBehaviour
{
	// Token: 0x06002698 RID: 9880 RVA: 0x0004A576 File Offset: 0x00048776
	private void Awake()
	{
		ScienceExperimentManager.instance.InitElements(this);
	}

	// Token: 0x06002699 RID: 9881 RVA: 0x0004A585 File Offset: 0x00048785
	private void OnDestroy()
	{
		ScienceExperimentManager.instance.DeInitElements();
	}

	// Token: 0x04002A96 RID: 10902
	public List<ScienceExperimentSceneElements.DisableByLiquidData> disableByLiquidList = new List<ScienceExperimentSceneElements.DisableByLiquidData>();

	// Token: 0x04002A97 RID: 10903
	public ParticleSystem sodaFizzParticles;

	// Token: 0x04002A98 RID: 10904
	public ParticleSystem sodaEruptionParticles;

	// Token: 0x02000611 RID: 1553
	[Serializable]
	public struct DisableByLiquidData
	{
		// Token: 0x04002A99 RID: 10905
		public Transform target;

		// Token: 0x04002A9A RID: 10906
		public float heightOffset;
	}
}
