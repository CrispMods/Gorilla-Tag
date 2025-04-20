using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000AA3 RID: 2723
	public class Pose
	{
		// Token: 0x0600440C RID: 17420 RVA: 0x0005C5EA File Offset: 0x0005A7EA
		public Pose()
		{
			this.Position = Vector3.zero;
			this.Rotation = Quaternion.identity;
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x0005C608 File Offset: 0x0005A808
		public Pose(Vector3 position, Quaternion rotation)
		{
			this.Position = position;
			this.Rotation = rotation;
		}

		// Token: 0x040044D9 RID: 17625
		public Vector3 Position;

		// Token: 0x040044DA RID: 17626
		public Quaternion Rotation;
	}
}
