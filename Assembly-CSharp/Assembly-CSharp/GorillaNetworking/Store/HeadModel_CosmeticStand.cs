using System;
using System.Collections.Generic;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B02 RID: 2818
	public class HeadModel_CosmeticStand : HeadModel
	{
		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06004672 RID: 18034 RVA: 0x0014EA45 File Offset: 0x0014CC45
		private string mountID
		{
			get
			{
				return "Mount_" + this.bustType.ToString();
			}
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x0014EA64 File Offset: 0x0014CC64
		public void LoadCosmeticParts(CosmeticSO cosmeticInfo, bool forRightSide = false)
		{
			this.ClearManuallySpawnedCosmeticParts();
			this.ClearCosmetics();
			if (cosmeticInfo == null)
			{
				Debug.LogWarning("Dynamic Cosmetics - LoadWardRobeParts -  No Cosmetic Info");
				return;
			}
			Debug.Log("Dynamic Cosmetics - Loading Wardrobe Parts for " + cosmeticInfo.info.playFabID);
			this.HandleLoadCosmeticParts(cosmeticInfo, forRightSide);
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x0014EAB4 File Offset: 0x0014CCB4
		private void HandleLoadCosmeticParts(CosmeticSO cosmeticInfo, bool forRightSide)
		{
			if (cosmeticInfo.info.category == CosmeticsController.CosmeticCategory.Set)
			{
				foreach (CosmeticSO cosmeticInfo2 in cosmeticInfo.info.setCosmetics)
				{
					this.HandleLoadCosmeticParts(cosmeticInfo2, forRightSide);
				}
				return;
			}
			if (cosmeticInfo.info.category == CosmeticsController.CosmeticCategory.Fur)
			{
				CosmeticPart[] array = cosmeticInfo.info.functionalParts;
				int i = 0;
				if (i < array.Length)
				{
					CosmeticPart cosmeticPart = array[i];
					GameObject gameObject = this.LoadAndInstantiatePrefab(cosmeticPart.prefabAssetRef, base.transform);
					gameObject.GetComponent<GorillaSkinToggle>().ApplyToMannequin(this.mannequin);
					Object.DestroyImmediate(gameObject);
					return;
				}
			}
			CosmeticPart[] array2;
			if (cosmeticInfo.info.storeParts.Length != 0)
			{
				array2 = cosmeticInfo.info.storeParts;
			}
			else
			{
				array2 = cosmeticInfo.info.wardrobeParts;
			}
			foreach (CosmeticPart cosmeticPart2 in array2)
			{
				foreach (CosmeticAttachInfo cosmeticAttachInfo in cosmeticPart2.attachAnchors)
				{
					if ((!forRightSide || !(cosmeticAttachInfo.selectSide == ECosmeticSelectSide.Left)) && (forRightSide || !(cosmeticAttachInfo.selectSide == ECosmeticSelectSide.Right)))
					{
						HeadModel._CosmeticPartLoadInfo partLoadInfo = new HeadModel._CosmeticPartLoadInfo
						{
							playFabId = cosmeticInfo.info.playFabID,
							prefabAssetRef = cosmeticPart2.prefabAssetRef,
							attachInfo = cosmeticAttachInfo,
							xform = null
						};
						GameObject gameObject2 = this.LoadAndInstantiatePrefab(cosmeticPart2.prefabAssetRef, base.transform);
						partLoadInfo.xform = gameObject2.transform;
						this._manuallySpawnedCosmeticParts.Add(gameObject2);
						gameObject2.SetActive(true);
						switch (this.bustType)
						{
						case HeadModel_CosmeticStand.BustType.Disabled:
							this.PositionWithWardRobeOffsets(partLoadInfo);
							break;
						case HeadModel_CosmeticStand.BustType.GorillaHead:
							this.PositionWithWardRobeOffsets(partLoadInfo);
							break;
						case HeadModel_CosmeticStand.BustType.GorillaTorso:
							this.PositionWithWardRobeOffsets(partLoadInfo);
							break;
						case HeadModel_CosmeticStand.BustType.GorillaTorsoPost:
							this.PositionWithWardRobeOffsets(partLoadInfo);
							break;
						case HeadModel_CosmeticStand.BustType.GorillaMannequin:
							this._manuallySpawnedCosmeticParts.Remove(gameObject2);
							Object.DestroyImmediate(gameObject2);
							break;
						case HeadModel_CosmeticStand.BustType.GuitarStand:
							this.PositionWardRobeItems(gameObject2, partLoadInfo);
							break;
						case HeadModel_CosmeticStand.BustType.JewelryBox:
							this.PositionWardRobeItems(gameObject2, partLoadInfo);
							break;
						case HeadModel_CosmeticStand.BustType.Table:
							this.PositionWardRobeItems(gameObject2, partLoadInfo);
							break;
						case HeadModel_CosmeticStand.BustType.PinDisplay:
							this.PositionWardRobeItems(gameObject2, partLoadInfo);
							break;
						case HeadModel_CosmeticStand.BustType.TagEffectDisplay:
							this.PositionWardRobeItems(gameObject2, partLoadInfo);
							break;
						default:
							this.PositionWithWardRobeOffsets(partLoadInfo);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x0014ED41 File Offset: 0x0014CF41
		public void SetStandType(HeadModel_CosmeticStand.BustType newBustType)
		{
			this.bustType = newBustType;
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x0014ED4C File Offset: 0x0014CF4C
		private void PositionWardRobeItems(GameObject instantiateEdObject, HeadModel._CosmeticPartLoadInfo partLoadInfo)
		{
			Transform transform = instantiateEdObject.transform.FindChildRecursive(this.mountID);
			if (transform != null)
			{
				Debug.Log("Dynamic Cosmetics - Mount Found: " + this.mountID);
				instantiateEdObject.transform.position = base.transform.position;
				instantiateEdObject.transform.rotation = base.transform.rotation;
				instantiateEdObject.transform.localPosition = transform.localPosition;
				instantiateEdObject.transform.localRotation = transform.localRotation;
				return;
			}
			HeadModel_CosmeticStand.BustType bustType = this.bustType;
			if (bustType - HeadModel_CosmeticStand.BustType.GuitarStand <= 2 || bustType == HeadModel_CosmeticStand.BustType.TagEffectDisplay)
			{
				instantiateEdObject.transform.position = base.transform.position;
				instantiateEdObject.transform.rotation = base.transform.rotation;
				return;
			}
			this.PositionWithWardRobeOffsets(partLoadInfo);
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x0014EE20 File Offset: 0x0014D020
		private void PositionWithWardRobeOffsets(HeadModel._CosmeticPartLoadInfo partLoadInfo)
		{
			Debug.Log("Dynamic Cosmetics - Mount Not Found: " + this.mountID);
			partLoadInfo.xform.localPosition = partLoadInfo.attachInfo.offset.pos;
			partLoadInfo.xform.localRotation = partLoadInfo.attachInfo.offset.rot;
			partLoadInfo.xform.localScale = partLoadInfo.attachInfo.offset.scale;
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x0014EE94 File Offset: 0x0014D094
		public void ClearManuallySpawnedCosmeticParts()
		{
			foreach (GameObject obj in this._manuallySpawnedCosmeticParts)
			{
				Object.DestroyImmediate(obj);
			}
			this._manuallySpawnedCosmeticParts.Clear();
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x0014EEF0 File Offset: 0x0014D0F0
		public void ClearCosmetics()
		{
			for (int i = base.transform.childCount - 1; i >= 0; i--)
			{
				Object.DestroyImmediate(base.transform.GetChild(i).gameObject);
			}
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x00043175 File Offset: 0x00041375
		private GameObject LoadAndInstantiatePrefab(GTAssetRef<GameObject> prefabAssetRef, Transform parent)
		{
			return null;
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x000023F4 File Offset: 0x000005F4
		public void UpdateCosmeticsMountPositions(CosmeticSO findCosmeticInAllCosmeticsArraySO)
		{
		}

		// Token: 0x04004806 RID: 18438
		public HeadModel_CosmeticStand.BustType bustType = HeadModel_CosmeticStand.BustType.JewelryBox;

		// Token: 0x04004807 RID: 18439
		[SerializeField]
		private List<GameObject> _manuallySpawnedCosmeticParts = new List<GameObject>();

		// Token: 0x04004808 RID: 18440
		public GameObject mannequin;

		// Token: 0x02000B03 RID: 2819
		public enum BustType
		{
			// Token: 0x0400480A RID: 18442
			Disabled,
			// Token: 0x0400480B RID: 18443
			GorillaHead,
			// Token: 0x0400480C RID: 18444
			GorillaTorso,
			// Token: 0x0400480D RID: 18445
			GorillaTorsoPost,
			// Token: 0x0400480E RID: 18446
			GorillaMannequin,
			// Token: 0x0400480F RID: 18447
			GuitarStand,
			// Token: 0x04004810 RID: 18448
			JewelryBox,
			// Token: 0x04004811 RID: 18449
			Table,
			// Token: 0x04004812 RID: 18450
			PinDisplay,
			// Token: 0x04004813 RID: 18451
			TagEffectDisplay
		}
	}
}
