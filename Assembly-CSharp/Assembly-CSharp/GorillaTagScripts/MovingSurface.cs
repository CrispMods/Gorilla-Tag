using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000984 RID: 2436
	[RequireComponent(typeof(Collider))]
	public class MovingSurface : MonoBehaviour
	{
		// Token: 0x06003B8A RID: 15242 RVA: 0x0011224F File Offset: 0x0011044F
		private void Start()
		{
			MovingSurfaceManager.instance == null;
			MovingSurfaceManager.instance.RegisterMovingSurface(this);
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x00112268 File Offset: 0x00110468
		private void OnDestroy()
		{
			if (MovingSurfaceManager.instance != null)
			{
				MovingSurfaceManager.instance.UnregisterMovingSurface(this);
			}
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x00112282 File Offset: 0x00110482
		public int GetID()
		{
			return this.uniqueId;
		}

		// Token: 0x04003CB9 RID: 15545
		[SerializeField]
		private int uniqueId = -1;
	}
}
