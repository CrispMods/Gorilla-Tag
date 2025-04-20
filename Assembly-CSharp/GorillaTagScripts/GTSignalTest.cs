using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009EB RID: 2539
	public class GTSignalTest : GTSignalListener
	{
		// Token: 0x0400405A RID: 16474
		public MeshRenderer[] targets = new MeshRenderer[0];

		// Token: 0x0400405B RID: 16475
		[Space]
		public MeshRenderer target;

		// Token: 0x0400405C RID: 16476
		public List<GTSignalListener> listeners = new List<GTSignalListener>(12);
	}
}
