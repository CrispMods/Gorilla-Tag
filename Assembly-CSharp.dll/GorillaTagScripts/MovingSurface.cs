using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000984 RID: 2436
	[RequireComponent(typeof(Collider))]
	public class MovingSurface : MonoBehaviour
	{
		// Token: 0x06003B8A RID: 15242 RVA: 0x00056058 File Offset: 0x00054258
		private void Start()
		{
			MovingSurfaceManager.instance == null;
			MovingSurfaceManager.instance.RegisterMovingSurface(this);
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x00056071 File Offset: 0x00054271
		private void OnDestroy()
		{
			if (MovingSurfaceManager.instance != null)
			{
				MovingSurfaceManager.instance.UnregisterMovingSurface(this);
			}
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x0005608B File Offset: 0x0005428B
		public int GetID()
		{
			return this.uniqueId;
		}

		// Token: 0x04003CB9 RID: 15545
		[SerializeField]
		private int uniqueId = -1;
	}
}
