using System;
using UnityEngine;

namespace com.AnotherAxiom.Paddleball
{
	// Token: 0x02000B54 RID: 2900
	public class PaddleballPaddle : MonoBehaviour
	{
		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x0600486F RID: 18543 RVA: 0x0005F2CC File Offset: 0x0005D4CC
		public bool Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x040049C9 RID: 18889
		[SerializeField]
		private bool right;
	}
}
