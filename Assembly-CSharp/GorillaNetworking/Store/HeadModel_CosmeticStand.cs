using System;
using System.Collections.Generic;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000AFF RID: 2815
	public class HeadModel_CosmeticStand : HeadModel
	{
		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06004666 RID: 18022 RVA: 0x0014E47D File Offset: 0x0014C67D
		private string mountID
		{
			get
			{
				return "Mount_" + this.bustType.ToString();
			}
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x0014E49C File Offset: 0x0014C69C
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

		// Token: 0x06004668 RID: 18024 RVA: 0x0014E4EC File Offset: 0x0014C6EC
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

		// Token: 0x06004669 RID: 18025 RVA: 0x0014E779 File Offset: 0x0014C979
		public void SetStandType(HeadModel_CosmeticStand.BustType newBustType)
		{
			this.bustType = newBustType;
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x0014E784 File Offset: 0x0014C984
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

		// Token: 0x0600466B RID: 18027 RVA: 0x0014E858 File Offset: 0x0014CA58
		private void PositionWithWardRobeOffsets(HeadModel._CosmeticPartLoadInfo partLoadInfo)
		{
			Debug.Log("Dynamic Cosmetics - Mount Not Found: " + this.mountID);
			partLoadInfo.xform.localPosition = partLoadInfo.attachInfo.offset.pos;
			partLoadInfo.xform.localRotation = partLoadInfo.attachInfo.offset.rot;
			partLoadInfo.xform.localScale = partLoadInfo.attachInfo.offset.scale;
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x0014E8CC File Offset: 0x0014CACC
		public void ClearManuallySpawnedCosmeticParts()
		{
			foreach (GameObject obj in this._manuallySpawnedCosmeticParts)
			{
				Object.DestroyImmediate(obj);
			}
			this._manuallySpawnedCosmeticParts.Clear();
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x0014E928 File Offset: 0x0014CB28
		public void ClearCosmetics()
		{
			for (int i = base.transform.childCount - 1; i >= 0; i--)
			{
				Object.DestroyImmediate(base.transform.GetChild(i).gameObject);
			}
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x00042E31 File Offset: 0x00041031
		private GameObject LoadAndInstantiatePrefab(GTAssetRef<GameObject> prefabAssetRef, Transform parent)
		{
			return null;
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x000023F4 File Offset: 0x000005F4
		public void UpdateCosmeticsMountPositions(CosmeticSO findCosmeticInAllCosmeticsArraySO)
		{
		}

		// Token: 0x040047F4 RID: 18420
		public HeadModel_CosmeticStand.BustType bustType = HeadModel_CosmeticStand.BustType.JewelryBox;

		// Token: 0x040047F5 RID: 18421
		[SerializeField]
		private List<GameObject> _manuallySpawnedCosmeticParts = new List<GameObject>();

		// Token: 0x040047F6 RID: 18422
		public GameObject mannequin;

		// Token: 0x02000B00 RID: 2816
		public enum BustType
		{
			// Token: 0x040047F8 RID: 18424
			Disabled,
			// Token: 0x040047F9 RID: 18425
			GorillaHead,
			// Token: 0x040047FA RID: 18426
			GorillaTorso,
			// Token: 0x040047FB RID: 18427
			GorillaTorsoPost,
			// Token: 0x040047FC RID: 18428
			GorillaMannequin,
			// Token: 0x040047FD RID: 18429
			GuitarStand,
			// Token: 0x040047FE RID: 18430
			JewelryBox,
			// Token: 0x040047FF RID: 18431
			Table,
			// Token: 0x04004800 RID: 18432
			PinDisplay,
			// Token: 0x04004801 RID: 18433
			TagEffectDisplay
		}
	}
}
