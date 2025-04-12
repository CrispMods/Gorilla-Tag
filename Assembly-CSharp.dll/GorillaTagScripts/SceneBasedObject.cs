using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009D7 RID: 2519
	public class SceneBasedObject : MonoBehaviour
	{
		// Token: 0x06003ECA RID: 16074 RVA: 0x00058299 File Offset: 0x00056499
		public bool IsLocalPlayerInScene()
		{
			return (ZoneManagement.instance.GetAllLoadedScenes().Count <= 1 || this.zone != GTZone.forest) && ZoneManagement.instance.IsSceneLoaded(this.zone);
		}

		// Token: 0x04004012 RID: 16402
		public GTZone zone;
	}
}
