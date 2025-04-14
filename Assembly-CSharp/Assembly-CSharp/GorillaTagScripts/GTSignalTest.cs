using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009C8 RID: 2504
	public class GTSignalTest : GTSignalListener
	{
		// Token: 0x04003F92 RID: 16274
		public MeshRenderer[] targets = new MeshRenderer[0];

		// Token: 0x04003F93 RID: 16275
		[Space]
		public MeshRenderer target;

		// Token: 0x04003F94 RID: 16276
		public List<GTSignalListener> listeners = new List<GTSignalListener>(12);
	}
}
