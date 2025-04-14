using System;
using System.Collections;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B60 RID: 2912
	public class TestRopePerf : MonoBehaviour
	{
		// Token: 0x060048D9 RID: 18649 RVA: 0x0016169C File Offset: 0x0015F89C
		private IEnumerator Start()
		{
			yield break;
		}

		// Token: 0x04004B7D RID: 19325
		[SerializeField]
		private GameObject ropesOld;

		// Token: 0x04004B7E RID: 19326
		[SerializeField]
		private GameObject ropesCustom;

		// Token: 0x04004B7F RID: 19327
		[SerializeField]
		private GameObject ropesCustomVectorized;
	}
}
