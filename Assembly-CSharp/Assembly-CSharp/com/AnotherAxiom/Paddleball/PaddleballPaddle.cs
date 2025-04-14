using System;
using UnityEngine;

namespace com.AnotherAxiom.Paddleball
{
	// Token: 0x02000B2A RID: 2858
	public class PaddleballPaddle : MonoBehaviour
	{
		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06004732 RID: 18226 RVA: 0x0015355E File Offset: 0x0015175E
		public bool Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x040048E6 RID: 18662
		[SerializeField]
		private bool right;
	}
}
