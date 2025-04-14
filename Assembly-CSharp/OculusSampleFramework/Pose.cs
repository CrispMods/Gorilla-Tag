using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A76 RID: 2678
	public class Pose
	{
		// Token: 0x060042C7 RID: 17095 RVA: 0x0013AD62 File Offset: 0x00138F62
		public Pose()
		{
			this.Position = Vector3.zero;
			this.Rotation = Quaternion.identity;
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x0013AD80 File Offset: 0x00138F80
		public Pose(Vector3 position, Quaternion rotation)
		{
			this.Position = position;
			this.Rotation = rotation;
		}

		// Token: 0x040043DF RID: 17375
		public Vector3 Position;

		// Token: 0x040043E0 RID: 17376
		public Quaternion Rotation;
	}
}
