using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000981 RID: 2433
	[RequireComponent(typeof(Collider))]
	public class MovingSurface : MonoBehaviour
	{
		// Token: 0x06003B7E RID: 15230 RVA: 0x00111C87 File Offset: 0x0010FE87
		private void Start()
		{
			MovingSurfaceManager.instance == null;
			MovingSurfaceManager.instance.RegisterMovingSurface(this);
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x00111CA0 File Offset: 0x0010FEA0
		private void OnDestroy()
		{
			if (MovingSurfaceManager.instance != null)
			{
				MovingSurfaceManager.instance.UnregisterMovingSurface(this);
			}
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x00111CBA File Offset: 0x0010FEBA
		public int GetID()
		{
			return this.uniqueId;
		}

		// Token: 0x04003CA7 RID: 15527
		[SerializeField]
		private int uniqueId = -1;
	}
}
