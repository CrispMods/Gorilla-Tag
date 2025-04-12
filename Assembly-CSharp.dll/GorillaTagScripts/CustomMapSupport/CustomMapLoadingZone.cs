using System;
using System.Collections.Generic;
using GorillaTagScripts.ModIO;
using GT_CustomMapSupportRuntime;
using UnityEngine;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x020009EE RID: 2542
	public class CustomMapLoadingZone : MonoBehaviour
	{
		// Token: 0x06003F6C RID: 16236 RVA: 0x000589EE File Offset: 0x00056BEE
		private void Start()
		{
			base.gameObject.layer = UnityLayer.GorillaTrigger.ToLayerIndex();
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x0016698C File Offset: 0x00164B8C
		public void SetupLoadingZone(LoadZoneSettings settings, in string[] assetBundleSceneFilePaths)
		{
			this.scenesToLoad = this.GetSceneIndexes(settings.scenesToLoad, assetBundleSceneFilePaths);
			this.scenesToUnload = this.CleanSceneUnloadArray(settings.scenesToUnload, settings.scenesToLoad, assetBundleSceneFilePaths);
			this.lightmapsColor = Array.Empty<Texture2D>();
			this.lightmapsDir = Array.Empty<Texture2D>();
			if (this.triggeredBy == TriggerSource.None)
			{
				if (settings.triggeredByHead && !settings.triggeredByBody)
				{
					this.triggeredBy = TriggerSource.Head;
				}
				else if (settings.triggeredByBody && !settings.triggeredByHead)
				{
					this.triggeredBy = TriggerSource.Body;
				}
				else if (settings.triggeredByHands && !settings.triggeredByHead && !settings.triggeredByBody)
				{
					this.triggeredBy = TriggerSource.Hands;
				}
				else
				{
					this.triggeredBy = TriggerSource.HeadOrBody;
				}
			}
			TriggerSource triggerSource = this.triggeredBy;
			if (triggerSource != TriggerSource.Hands)
			{
				if (triggerSource - TriggerSource.Head <= 2)
				{
					base.gameObject.layer = UnityLayer.GorillaTrigger.ToLayerIndex();
				}
			}
			else
			{
				base.gameObject.layer = UnityLayer.GorillaInteractable.ToLayerIndex();
			}
			Collider[] components = base.gameObject.GetComponents<Collider>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].isTrigger = true;
			}
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x00166A98 File Offset: 0x00164C98
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

		// Token: 0x06003F6F RID: 16239 RVA: 0x00166AF0 File Offset: 0x00164CF0
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

		// Token: 0x06003F70 RID: 16240 RVA: 0x00058A02 File Offset: 0x00056C02
		private void OnTriggerEnter(Collider other)
		{
			if (!this.ValidateCollider(other))
			{
				return;
			}
			CustomMapManager.LoadZoneTriggered(this.scenesToLoad, this.scenesToUnload, this.lightmapsColor, this.lightmapsDir);
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x00058A2B File Offset: 0x00056C2B
		private string GetSceneNameFromFilePath(string filePath)
		{
			string[] array = filePath.Split("/", StringSplitOptions.None);
			return array[array.Length - 1].Split(".", StringSplitOptions.None)[0];
		}

		// Token: 0x06003F72 RID: 16242 RVA: 0x00166B34 File Offset: 0x00164D34
		private bool ValidateCollider(Collider other)
		{
			GameObject gameObject = other.gameObject;
			bool flag = gameObject == GorillaTagger.Instance.headCollider.gameObject && (this.triggeredBy == TriggerSource.Head || this.triggeredBy == TriggerSource.HeadOrBody);
			bool flag2 = gameObject == GorillaTagger.Instance.bodyCollider.gameObject && (this.triggeredBy == TriggerSource.Body || this.triggeredBy == TriggerSource.HeadOrBody);
			bool flag3 = (gameObject == GorillaTagger.Instance.leftHandTriggerCollider.gameObject || gameObject == GorillaTagger.Instance.rightHandTriggerCollider.gameObject) && this.triggeredBy == TriggerSource.Hands;
			return flag || flag2 || flag3;
		}

		// Token: 0x0400408B RID: 16523
		private int[] scenesToLoad;

		// Token: 0x0400408C RID: 16524
		private int[] scenesToUnload;

		// Token: 0x0400408D RID: 16525
		private Texture2D[] lightmapsColor;

		// Token: 0x0400408E RID: 16526
		private Texture2D[] lightmapsDir;

		// Token: 0x0400408F RID: 16527
		private TriggerSource triggeredBy = TriggerSource.HeadOrBody;
	}
}
