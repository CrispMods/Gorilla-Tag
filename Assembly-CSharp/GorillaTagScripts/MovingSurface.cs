using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009A7 RID: 2471
	[RequireComponent(typeof(Collider))]
	public class MovingSurface : MonoBehaviour
	{
		// Token: 0x06003C96 RID: 15510 RVA: 0x000578EF File Offset: 0x00055AEF
		private void Start()
		{
			MovingSurfaceManager.instance == null;
			MovingSurfaceManager.instance.RegisterMovingSurface(this);
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x00057908 File Offset: 0x00055B08
		private void OnDestroy()
		{
			if (MovingSurfaceManager.instance != null)
			{
				MovingSurfaceManager.instance.UnregisterMovingSurface(this);
			}
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x00057922 File Offset: 0x00055B22
		public int GetID()
		{
			return this.uniqueId;
		}

		// Token: 0x04003D81 RID: 15745
		[SerializeField]
		private int uniqueId = -1;
	}
}
