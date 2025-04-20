using System;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaTagScripts.ModIO;
using GT_CustomMapSupportRuntime;
using UnityEngine;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x02000A01 RID: 2561
	public class CMSLoadingZone : MonoBehaviour
	{
		// Token: 0x06003FFD RID: 16381 RVA: 0x00059D19 File Offset: 0x00057F19
		private void Start()
		{
			base.gameObject.layer = UnityLayer.GorillaTrigger.ToLayerIndex();
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x0016B298 File Offset: 0x00169498
		public void SetupLoadingZone(LoadZoneSettings settings, in string[] assetBundleSceneFilePaths)
		{
			this.scenesToLoad = this.GetSceneIndexes(settings.scenesToLoad, assetBundleSceneFilePaths);
			this.scenesToUnload = this.CleanSceneUnloadArray(settings.scenesToUnload, settings.scenesToLoad, assetBundleSceneFilePaths);
			this.triggeredBy = TriggerSource.Body;
			base.gameObject.layer = UnityLayer.GorillaBoundary.ToLayerIndex();
			Collider[] components = base.gameObject.GetComponents<Collider>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].isTrigger = true;
			}
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x0016B310 File Offset: 0x00169510
		private int[] GetSceneIndexes(List<string> sceneNames, in string[] assetBundleSceneFilePaths)
		{
			int[] array = new int[sceneNames.Count];
			for (int i = 0; i < sceneNames.Count; i++)
			{
				for (int j = 0; j < assetBundleSceneFilePaths.Length; j++)
				{
					if (string.Equals(sceneNames[i], this.GetSceneNameFromFilePath(assetBundleSceneFilePaths[j])))
					{
						array[i] = j;
						break;
					}
				}
			}
			return array;
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x0016B368 File Offset: 0x00169568
		private int[] CleanSceneUnloadArray(List<string> unload, List<string> load, in string[] assetBundleSceneFilePaths)
		{
			for (int i = 0; i < load.Count; i++)
			{
				if (unload.Contains(load[i]))
				{
					unload.Remove(load[i]);
				}
			}
			return this.GetSceneIndexes(unload, assetBundleSceneFilePaths);
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x00059D2D File Offset: 0x00057F2D
		public void OnTriggerEnter(Collider other)
		{
			if (other == GTPlayer.Instance.bodyCollider)
			{
				CustomMapManager.LoadZoneTriggered(this.scenesToLoad, this.scenesToUnload);
			}
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x00059D52 File Offset: 0x00057F52
		private string GetSceneNameFromFilePath(string filePath)
		{
			string[] array = filePath.Split("/", StringSplitOptions.None);
			return array[array.Length - 1].Split(".", StringSplitOptions.None)[0];
		}

		// Token: 0x040040FD RID: 16637
		private int[] scenesToLoad;

		// Token: 0x040040FE RID: 16638
		private int[] scenesToUnload;

		// Token: 0x040040FF RID: 16639
		private TriggerSource triggeredBy = TriggerSource.HeadOrBody;
	}
}
