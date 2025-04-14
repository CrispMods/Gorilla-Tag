using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A79 RID: 2681
	public class Pose
	{
		// Token: 0x060042D3 RID: 17107 RVA: 0x0013B32A File Offset: 0x0013952A
		public Pose()
		{
			this.Position = Vector3.zero;
			this.Rotation = Quaternion.identity;
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x0013B348 File Offset: 0x00139548
		public Pose(Vector3 position, Quaternion rotation)
		{
			this.Position = position;
			this.Rotation = rotation;
		}

		// Token: 0x040043F1 RID: 17393
		public Vector3 Position;

		// Token: 0x040043F2 RID: 17394
		public Quaternion Rotation;
	}
}
