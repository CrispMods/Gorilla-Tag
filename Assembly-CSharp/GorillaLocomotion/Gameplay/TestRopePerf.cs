using System;
using System.Collections;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B8A RID: 2954
	public class TestRopePerf : MonoBehaviour
	{
		// Token: 0x06004A18 RID: 18968 RVA: 0x00060381 File Offset: 0x0005E581
		private IEnumerator Start()
		{
			yield break;
		}

		// Token: 0x04004C61 RID: 19553
		[SerializeField]
		private GameObject ropesOld;

		// Token: 0x04004C62 RID: 19554
		[SerializeField]
		private GameObject ropesCustom;

		// Token: 0x04004C63 RID: 19555
		[SerializeField]
		private GameObject ropesCustomVectorized;
	}
}
