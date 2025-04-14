using System;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009F2 RID: 2546
	public class BuilderAnimateOnTap : BuilderPieceTappable
	{
		// Token: 0x06003F95 RID: 16277 RVA: 0x0012D6A4 File Offset: 0x0012B8A4
		public override void OnTapReplicated()
		{
			this.anim.Rewind();
			this.anim.Play();
		}

		// Token: 0x0400409B RID: 16539
		[SerializeField]
		private Animation anim;
	}
}
