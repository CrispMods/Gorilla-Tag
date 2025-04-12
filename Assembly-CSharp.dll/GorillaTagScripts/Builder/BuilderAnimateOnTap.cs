using System;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009F5 RID: 2549
	public class BuilderAnimateOnTap : BuilderPieceTappable
	{
		// Token: 0x06003FA1 RID: 16289 RVA: 0x00058C48 File Offset: 0x00056E48
		public override void OnTapReplicated()
		{
			this.anim.Rewind();
			this.anim.Play();
		}

		// Token: 0x040040AD RID: 16557
		[SerializeField]
		private Animation anim;
	}
}
