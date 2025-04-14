using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009C5 RID: 2501
	public class GTSignalTest : GTSignalListener
	{
		// Token: 0x04003F80 RID: 16256
		public MeshRenderer[] targets = new MeshRenderer[0];

		// Token: 0x04003F81 RID: 16257
		[Space]
		public MeshRenderer target;

		// Token: 0x04003F82 RID: 16258
		public List<GTSignalListener> listeners = new List<GTSignalListener>(12);
	}
}
