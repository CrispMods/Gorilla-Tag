using System;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A1F RID: 2591
	public class BuilderAnimateOnTap : BuilderPieceTappable
	{
		// Token: 0x060040DA RID: 16602 RVA: 0x0005A64A File Offset: 0x0005884A
		public override void OnTapReplicated()
		{
			this.anim.Rewind();
			this.anim.Play();
		}

		// Token: 0x04004195 RID: 16789
		[SerializeField]
		private Animation anim;
	}
}
