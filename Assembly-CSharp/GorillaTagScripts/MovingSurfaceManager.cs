using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000982 RID: 2434
	public class MovingSurfaceManager : MonoBehaviour
	{
		// Token: 0x06003B82 RID: 15234 RVA: 0x00111CD4 File Offset: 0x0010FED4
		private void Awake()
		{
			if (MovingSurfaceManager.instance != null && MovingSurfaceManager.instance != this)
			{
				GTDev.LogWarning<string>("Instance of MovingSurfaceManager already exists. Destroying.", null);
				Object.Destroy(this);
				return;
			}
			if (MovingSurfaceManager.instance == null)
			{
				MovingSurfaceManager.instance = this;
			}
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x00111D20 File Offset: 0x0010FF20
		public void RegisterMovingSurface(MovingSurface ms)
		{
			this.movingSurfaces.TryAdd(ms.GetID(), ms);
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x00111D35 File Offset: 0x0010FF35
		public void UnregisterMovingSurface(MovingSurface ms)
		{
			this.movingSurfaces.Remove(ms.GetID());
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x00111D49 File Offset: 0x0010FF49
		public void RegisterSurfaceMover(SurfaceMover sm)
		{
			if (!this.surfaceMovers.Contains(sm))
			{
				this.surfaceMovers.Add(sm);
				sm.InitMovingSurface();
			}
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x00111D6B File Offset: 0x0010FF6B
		public void UnregisterSurfaceMover(SurfaceMover sm)
		{
			this.surfaceMovers.Remove(sm);
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x00111D7A File Offset: 0x0010FF7A
		public bool TryGetMovingSurface(int id, out MovingSurface result)
		{
			return this.movingSurfaces.TryGetValue(id, out result) && result != null;
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x00111D98 File Offset: 0x0010FF98
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

		// Token: 0x04003CA8 RID: 15528
		private List<SurfaceMover> surfaceMovers = new List<SurfaceMover>(5);

		// Token: 0x04003CA9 RID: 15529
		private Dictionary<int, MovingSurface> movingSurfaces = new Dictionary<int, MovingSurface>(10);

		// Token: 0x04003CAA RID: 15530
		public static MovingSurfaceManager instance;
	}
}
