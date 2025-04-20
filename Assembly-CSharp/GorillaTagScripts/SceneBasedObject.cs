using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009FA RID: 2554
	public class SceneBasedObject : MonoBehaviour
	{
		// Token: 0x06003FD6 RID: 16342 RVA: 0x00059B30 File Offset: 0x00057D30
		public bool IsLocalPlayerInScene()
		{
			return (ZoneManagement.instance.GetAllLoadedScenes().Count <= 1 || this.zone != GTZone.forest) && ZoneManagement.instance.IsSceneLoaded(this.zone);
		}

		// Token: 0x040040DA RID: 16602
		public GTZone zone;
	}
}
