using System;
using System.Collections.Generic;
using GorillaTagScripts.ModIO;
using GT_CustomMapSupportRuntime;
using UnityEngine;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x020009EB RID: 2539
	public class CustomMapLoadingZone : MonoBehaviour
	{
		// Token: 0x06003F60 RID: 16224 RVA: 0x0012C1B4 File Offset: 0x0012A3B4
		private void Start()
		{
			base.gameObject.layer = UnityLayer.GorillaTrigger.ToLayerIndex();
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x0012C1C8 File Offset: 0x0012A3C8
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

		// Token: 0x06003F62 RID: 16226 RVA: 0x0012C2D4 File Offset: 0x0012A4D4
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

		// Token: 0x06003F63 RID: 16227 RVA: 0x0012C32C File Offset: 0x0012A52C
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

		// Token: 0x06003F64 RID: 16228 RVA: 0x0012C36F File Offset: 0x0012A56F
		private void OnTriggerEnter(Collider other)
		{
			if (!this.ValidateCollider(other))
			{
				return;
			}
			CustomMapManager.LoadZoneTriggered(this.scenesToLoad, this.scenesToUnload, this.lightmapsColor, this.lightmapsDir);
		}

		// Token: 0x06003F65 RID: 16229 RVA: 0x0012C398 File Offset: 0x0012A598
		private string GetSceneNameFromFilePath(string filePath)
		{
			string[] array = filePath.Split("/", StringSplitOptions.None);
			return array[array.Length - 1].Split(".", StringSplitOptions.None)[0];
		}

		// Token: 0x06003F66 RID: 16230 RVA: 0x0012C3BC File Offset: 0x0012A5BC
		private bool ValidateCollider(Collider other)
		{
			GameObject gameObject = other.gameObject;
			bool flag = gameObject == GorillaTagger.Instance.headCollider.gameObject && (this.triggeredBy == TriggerSource.Head || this.triggeredBy == TriggerSource.HeadOrBody);
			bool flag2 = gameObject == GorillaTagger.Instance.bodyCollider.gameObject && (this.triggeredBy == TriggerSource.Body || this.triggeredBy == TriggerSource.HeadOrBody);
			bool flag3 = (gameObject == GorillaTagger.Instance.leftHandTriggerCollider.gameObject || gameObject == GorillaTagger.Instance.rightHandTriggerCollider.gameObject) && this.triggeredBy == TriggerSource.Hands;
			return flag || flag2 || flag3;
		}

		// Token: 0x04004079 RID: 16505
		private int[] scenesToLoad;

		// Token: 0x0400407A RID: 16506
		private int[] scenesToUnload;

		// Token: 0x0400407B RID: 16507
		private Texture2D[] lightmapsColor;

		// Token: 0x0400407C RID: 16508
		private Texture2D[] lightmapsDir;

		// Token: 0x0400407D RID: 16509
		private TriggerSource triggeredBy = TriggerSource.HeadOrBody;
	}
}
