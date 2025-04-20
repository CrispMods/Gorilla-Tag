using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009A8 RID: 2472
	public class MovingSurfaceManager : MonoBehaviour
	{
		// Token: 0x06003C9A RID: 15514 RVA: 0x00154E30 File Offset: 0x00153030
		private void Awake()
		{
			if (MovingSurfaceManager.instance != null && MovingSurfaceManager.instance != this)
			{
				GTDev.LogWarning<string>("Instance of MovingSurfaceManager already exists. Destroying.", null);
				UnityEngine.Object.Destroy(this);
				return;
			}
			if (MovingSurfaceManager.instance == null)
			{
				MovingSurfaceManager.instance = this;
			}
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x00057939 File Offset: 0x00055B39
		public void RegisterMovingSurface(MovingSurface ms)
		{
			this.movingSurfaces.TryAdd(ms.GetID(), ms);
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x0005794E File Offset: 0x00055B4E
		public void UnregisterMovingSurface(MovingSurface ms)
		{
			this.movingSurfaces.Remove(ms.GetID());
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x00057962 File Offset: 0x00055B62
		public void RegisterSurfaceMover(SurfaceMover sm)
		{
			if (!this.surfaceMovers.Contains(sm))
			{
				this.surfaceMovers.Add(sm);
				sm.InitMovingSurface();
			}
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x00057984 File Offset: 0x00055B84
		public void UnregisterSurfaceMover(SurfaceMover sm)
		{
			this.surfaceMovers.Remove(sm);
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x00057993 File Offset: 0x00055B93
		public bool TryGetMovingSurface(int id, out MovingSurface result)
		{
			return this.movingSurfaces.TryGetValue(id, out result) && result != null;
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x00154E7C File Offset: 0x0015307C
		private void FixedUpdate()
		{
			foreach (SurfaceMover surfaceMover in this.surfaceMovers)
			{
				if (surfaceMover.isActiveAndEnabled)
				{
					surfaceMover.Move();
				}
			}
		}

		// Token: 0x04003D82 RID: 15746
		private List<SurfaceMover> surfaceMovers = new List<SurfaceMover>(5);

		// Token: 0x04003D83 RID: 15747
		private Dictionary<int, MovingSurface> movingSurfaces = new Dictionary<int, MovingSurface>(10);

		// Token: 0x04003D84 RID: 15748
		public static MovingSurfaceManager instance;
	}
}
