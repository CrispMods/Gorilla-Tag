using System;
using System.Collections;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B5D RID: 2909
	public class TestRopePerf : MonoBehaviour
	{
		// Token: 0x060048CD RID: 18637 RVA: 0x001610D4 File Offset: 0x0015F2D4
		private IEnumerator Start()
		{
			yield break;
		}

		// Token: 0x04004B6B RID: 19307
		[SerializeField]
		private GameObject ropesOld;

		// Token: 0x04004B6C RID: 19308
		[SerializeField]
		private GameObject ropesCustom;

		// Token: 0x04004B6D RID: 19309
		[SerializeField]
		private GameObject ropesCustomVectorized;
	}
}
