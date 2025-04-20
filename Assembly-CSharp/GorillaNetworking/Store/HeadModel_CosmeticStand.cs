using System;
using System.Collections.Generic;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B2C RID: 2860
	public class HeadModel_CosmeticStand : HeadModel
	{
		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x060047AF RID: 18351 RVA: 0x0005EB2B File Offset: 0x0005CD2B
		private string mountID
		{
			get
			{
				return "Mount_" + this.bustType.ToString();
			}
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x0018B160 File Offset: 0x00189360
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

		// Token: 0x060047B1 RID: 18353 RVA: 0x0018B1B0 File Offset: 0x001893B0
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
					UnityEngine.Object.DestroyImmediate(gameObject);
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
							UnityEngine.Object.DestroyImmediate(gameObject2);
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

		// Token: 0x060047B2 RID: 18354 RVA: 0x0005EB48 File Offset: 0x0005CD48
		public void SetStandType(HeadModel_CosmeticStand.BustType newBustType)
		{
			this.bustType = newBustType;
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x0018B440 File Offset: 0x00189640
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

		// Token: 0x060047B4 RID: 18356 RVA: 0x0018B514 File Offset: 0x00189714
		private void PositionWithWardRobeOffsets(HeadModel._CosmeticPartLoadInfo partLoadInfo)
		{
			Debug.Log("Dynamic Cosmetics - Mount Not Found: " + this.mountID);
			partLoadInfo.xform.localPosition = partLoadInfo.attachInfo.offset.pos;
			partLoadInfo.xform.localRotation = partLoadInfo.attachInfo.offset.rot;
			partLoadInfo.xform.localScale = partLoadInfo.attachInfo.offset.scale;
		}

		// Token: 0x060047B5 RID: 18357 RVA: 0x0018B588 File Offset: 0x00189788
		public void ClearManuallySpawnedCosmeticParts()
		{
			foreach (GameObject obj in this._manuallySpawnedCosmeticParts)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			this._manuallySpawnedCosmeticParts.Clear();
		}

		// Token: 0x060047B6 RID: 18358 RVA: 0x0018B5E4 File Offset: 0x001897E4
		public void ClearCosmetics()
		{
			for (int i = base.transform.childCount - 1; i >= 0; i--)
			{
				UnityEngine.Object.DestroyImmediate(base.transform.GetChild(i).gameObject);
			}
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x0003924B File Offset: 0x0003744B
		private GameObject LoadAndInstantiatePrefab(GTAssetRef<GameObject> prefabAssetRef, Transform parent)
		{
			return null;
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x00030607 File Offset: 0x0002E807
		public void UpdateCosmeticsMountPositions(CosmeticSO findCosmeticInAllCosmeticsArraySO)
		{
		}

		// Token: 0x040048E9 RID: 18665
		public HeadModel_CosmeticStand.BustType bustType = HeadModel_CosmeticStand.BustType.JewelryBox;

		// Token: 0x040048EA RID: 18666
		[SerializeField]
		private List<GameObject> _manuallySpawnedCosmeticParts = new List<GameObject>();

		// Token: 0x040048EB RID: 18667
		public GameObject mannequin;

		// Token: 0x02000B2D RID: 2861
		public enum BustType
		{
			// Token: 0x040048ED RID: 18669
			Disabled,
			// Token: 0x040048EE RID: 18670
			GorillaHead,
			// Token: 0x040048EF RID: 18671
			GorillaTorso,
			// Token: 0x040048F0 RID: 18672
			GorillaTorsoPost,
			// Token: 0x040048F1 RID: 18673
			GorillaMannequin,
			// Token: 0x040048F2 RID: 18674
			GuitarStand,
			// Token: 0x040048F3 RID: 18675
			JewelryBox,
			// Token: 0x040048F4 RID: 18676
			Table,
			// Token: 0x040048F5 RID: 18677
			PinDisplay,
			// Token: 0x040048F6 RID: 18678
			TagEffectDisplay
		}
	}
}
