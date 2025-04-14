using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009D4 RID: 2516
	public class SceneBasedObject : MonoBehaviour
	{
		// Token: 0x06003EBE RID: 16062 RVA: 0x0012999C File Offset: 0x00127B9C
		public bool IsLocalPlayerInScene()
		{
			return (ZoneManagement.instance.GetAllLoadedScenes().Count <= 1 || this.zone != GTZone.forest) && ZoneManagement.instance.IsSceneLoaded(this.zone);
		}

		// Token: 0x04004000 RID: 16384
		public GTZone zone;
	}
}
