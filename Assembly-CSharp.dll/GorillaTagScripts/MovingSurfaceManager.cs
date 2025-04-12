using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000985 RID: 2437
	public class MovingSurfaceManager : MonoBehaviour
	{
		// Token: 0x06003B8E RID: 15246 RVA: 0x0014EE48 File Offset: 0x0014D048
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

		// Token: 0x06003B8F RID: 15247 RVA: 0x000560A2 File Offset: 0x000542A2
		public void RegisterMovingSurface(MovingSurface ms)
		{
			this.movingSurfaces.TryAdd(ms.GetID(), ms);
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x000560B7 File Offset: 0x000542B7
		public void UnregisterMovingSurface(MovingSurface ms)
		{
			this.movingSurfaces.Remove(ms.GetID());
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x000560CB File Offset: 0x000542CB
		public void RegisterSurfaceMover(SurfaceMover sm)
		{
			if (!this.surfaceMovers.Contains(sm))
			{
				this.surfaceMovers.Add(sm);
				sm.InitMovingSurface();
			}
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x000560ED File Offset: 0x000542ED
		public void UnregisterSurfaceMover(SurfaceMover sm)
		{
			this.surfaceMovers.Remove(sm);
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x000560FC File Offset: 0x000542FC
		public bool TryGetMovingSurface(int id, out MovingSurface result)
		{
			return this.movingSurfaces.TryGetValue(id, out result) && result != null;
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x0014EE94 File Offset: 0x0014D094
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

		// Token: 0x04003CBA RID: 15546
		private List<SurfaceMover> surfaceMovers = new List<SurfaceMover>(5);

		// Token: 0x04003CBB RID: 15547
		private Dictionary<int, MovingSurface> movingSurfaces = new Dictionary<int, MovingSurface>(10);

		// Token: 0x04003CBC RID: 15548
		public static MovingSurfaceManager instance;
	}
}
