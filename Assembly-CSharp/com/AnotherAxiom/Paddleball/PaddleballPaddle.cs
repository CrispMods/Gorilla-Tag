using System;
using UnityEngine;

namespace com.AnotherAxiom.Paddleball
{
	// Token: 0x02000B27 RID: 2855
	public class PaddleballPaddle : MonoBehaviour
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06004726 RID: 18214 RVA: 0x00152F96 File Offset: 0x00151196
		public bool Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x040048D4 RID: 18644
		[SerializeField]
		private bool right;
	}
}
