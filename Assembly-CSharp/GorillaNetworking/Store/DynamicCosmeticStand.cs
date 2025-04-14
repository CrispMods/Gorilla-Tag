using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GorillaNetworking.Store
{
	// Token: 0x02000AFD RID: 2813
	public class DynamicCosmeticStand : MonoBehaviour, iFlagForBaking
	{
		// Token: 0x06004654 RID: 18004 RVA: 0x0014D9D8 File Offset: 0x0014BBD8
		public virtual void SetForBaking()
		{
			this.GorillaHeadModel.SetActive(true);
			this.GorillaTorsoModel.SetActive(true);
			this.GorillaTorsoPostModel.SetActive(true);
			this.GorillaMannequinModel.SetActive(true);
			this.JeweleryBoxModel.SetActive(true);
			this.root.SetActive(true);
			this.DisplayHeadModel.gameObject.SetActive(false);
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x0014DA3E File Offset: 0x0014BC3E
		public virtual void SetForGame()
		{
			this.DisplayHeadModel.gameObject.SetActive(true);
			this.SetStandType(this.DisplayHeadModel.bustType);
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06004656 RID: 18006 RVA: 0x0014DA62 File Offset: 0x0014BC62
		// (set) Token: 0x06004657 RID: 18007 RVA: 0x0014DA6A File Offset: 0x0014BC6A
		public string thisCosmeticName
		{
			get
			{
				return this._thisCosmeticName;
			}
			set
			{
				this._thisCosmeticName = value;
			}
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x0014DA74 File Offset: 0x0014BC74
		public void InitializeCosmetic()
		{
			this.thisCosmeticItem = CosmeticsController.instance.allCosmetics.Find((CosmeticsController.CosmeticItem x) => this.thisCosmeticName == x.displayName || this.thisCosmeticName == x.overrideDisplayName || this.thisCosmeticName == x.itemName);
			if (this.slotPriceText != null)
			{
				this.slotPriceText.text = this.thisCosmeticItem.itemCategory.ToString().ToUpper() + " " + this.thisCosmeticItem.cost.ToString();
			}
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x0014DAF4 File Offset: 0x0014BCF4
		public void SpawnItemOntoStand(string PlayFabID)
		{
			if (PlayFabID.IsNullOrEmpty())
			{
				Debug.LogWarning("ManuallyInitialize: PlayFabID is null or empty for " + this.StandName);
				return;
			}
			this.thisCosmeticName = PlayFabID;
			if (this.thisCosmeticName.Length == 5)
			{
				this.thisCosmeticName += ".";
			}
			this.DisplayHeadModel.LoadCosmeticParts(StoreController.FindCosmeticInAllCosmeticsArraySO(this.thisCosmeticName), false);
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x0014DB61 File Offset: 0x0014BD61
		public void ClearCosmetics()
		{
			this.DisplayHeadModel.ClearManuallySpawnedCosmeticParts();
			this.DisplayHeadModel.ClearCosmetics();
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x0014DB7C File Offset: 0x0014BD7C
		public void SetStandType(HeadModel_CosmeticStand.BustType newBustType)
		{
			this.DisplayHeadModel.SetStandType(newBustType);
			this.GorillaHeadModel.SetActive(false);
			this.GorillaTorsoModel.SetActive(false);
			this.GorillaTorsoPostModel.SetActive(false);
			this.GorillaMannequinModel.SetActive(false);
			this.GuitarStandModel.SetActive(false);
			this.JeweleryBoxModel.SetActive(false);
			switch (newBustType)
			{
			case HeadModel_CosmeticStand.BustType.Disabled:
				this.DisplayHeadModel.transform.localPosition = Vector3.zero;
				this.DisplayHeadModel.transform.localRotation = Quaternion.identity;
				this.root.SetActive(false);
				break;
			case HeadModel_CosmeticStand.BustType.GorillaHead:
				this.root.SetActive(true);
				this.GorillaHeadModel.SetActive(true);
				this.DisplayHeadModel.transform.localPosition = this.GorillaHeadModel.transform.localPosition;
				this.DisplayHeadModel.transform.localRotation = this.GorillaHeadModel.transform.localRotation;
				break;
			case HeadModel_CosmeticStand.BustType.GorillaTorso:
				this.root.SetActive(true);
				this.GorillaTorsoModel.SetActive(true);
				this.DisplayHeadModel.transform.localPosition = this.GorillaTorsoModel.transform.localPosition;
				this.DisplayHeadModel.transform.localRotation = this.GorillaTorsoModel.transform.localRotation;
				break;
			case HeadModel_CosmeticStand.BustType.GorillaTorsoPost:
				this.root.SetActive(true);
				this.GorillaTorsoPostModel.SetActive(true);
				this.DisplayHeadModel.transform.localPosition = this.GorillaTorsoPostModel.transform.localPosition;
				this.DisplayHeadModel.transform.localRotation = this.GorillaTorsoPostModel.transform.localRotation;
				break;
			case HeadModel_CosmeticStand.BustType.GorillaMannequin:
				this.root.SetActive(true);
				this.GorillaMannequinModel.SetActive(true);
				this.DisplayHeadModel.transform.localPosition = this.GorillaMannequinModel.transform.localPosition;
				this.DisplayHeadModel.transform.localRotation = this.GorillaMannequinModel.transform.localRotation;
				break;
			case HeadModel_CosmeticStand.BustType.GuitarStand:
				this.root.SetActive(true);
				this.GuitarStandModel.SetActive(true);
				this.DisplayHeadModel.transform.localPosition = this.GuitarStandMount.transform.localPosition;
				this.DisplayHeadModel.transform.localRotation = this.GuitarStandMount.transform.localRotation;
				break;
			case HeadModel_CosmeticStand.BustType.JewelryBox:
				this.root.SetActive(true);
				this.JeweleryBoxModel.SetActive(true);
				this.DisplayHeadModel.transform.localPosition = this.JeweleryBoxMount.transform.localPosition;
				this.DisplayHeadModel.transform.localRotation = this.JeweleryBoxMount.transform.localRotation;
				break;
			case HeadModel_CosmeticStand.BustType.Table:
				this.root.SetActive(true);
				this.DisplayHeadModel.transform.localPosition = this.TableMount.transform.localPosition;
				this.DisplayHeadModel.transform.localRotation = this.TableMount.transform.localRotation;
				break;
			case HeadModel_CosmeticStand.BustType.PinDisplay:
				this.root.SetActive(true);
				this.DisplayHeadModel.transform.localPosition = this.PinDisplayMounnt.transform.localPosition;
				this.DisplayHeadModel.transform.localRotation = this.PinDisplayMounnt.transform.localRotation;
				break;
			case HeadModel_CosmeticStand.BustType.TagEffectDisplay:
				this.root.SetActive(true);
				break;
			default:
				this.root.SetActive(true);
				this.DisplayHeadModel.transform.localPosition = Vector3.zero;
				this.DisplayHeadModel.transform.localRotation = Quaternion.identity;
				break;
			}
			this.SpawnItemOntoStand(this.thisCosmeticName);
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x0014DF6C File Offset: 0x0014C16C
		public void CopyChildsName()
		{
			foreach (DynamicCosmeticStand dynamicCosmeticStand in base.gameObject.GetComponentsInChildren<DynamicCosmeticStand>(true))
			{
				if (dynamicCosmeticStand != this)
				{
					this.StandName = dynamicCosmeticStand.StandName;
				}
			}
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x0014DFB0 File Offset: 0x0014C1B0
		public void PressCosmeticStandButton()
		{
			this.searchIndex = CosmeticsController.instance.currentCart.IndexOf(this.thisCosmeticItem);
			if (this.searchIndex != -1)
			{
				GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.cart_item_remove, this.thisCosmeticItem);
				CosmeticsController.instance.currentCart.RemoveAt(this.searchIndex);
				foreach (DynamicCosmeticStand dynamicCosmeticStand in StoreController.instance.StandsByPlayfabID[this.thisCosmeticItem.itemName])
				{
					dynamicCosmeticStand.AddToCartButton.isOn = false;
					dynamicCosmeticStand.AddToCartButton.UpdateColor();
				}
				for (int i = 0; i < 16; i++)
				{
					if (this.thisCosmeticItem.itemName == CosmeticsController.instance.tryOnSet.items[i].itemName)
					{
						CosmeticsController.instance.tryOnSet.items[i] = CosmeticsController.instance.nullItem;
					}
				}
			}
			else
			{
				GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.cart_item_add, this.thisCosmeticItem);
				CosmeticsController.instance.currentCart.Insert(0, this.thisCosmeticItem);
				foreach (DynamicCosmeticStand dynamicCosmeticStand2 in StoreController.instance.StandsByPlayfabID[this.thisCosmeticName])
				{
					dynamicCosmeticStand2.AddToCartButton.isOn = true;
					dynamicCosmeticStand2.AddToCartButton.UpdateColor();
				}
				if (CosmeticsController.instance.currentCart.Count > CosmeticsController.instance.fittingRoomButtons.Length)
				{
					foreach (DynamicCosmeticStand dynamicCosmeticStand3 in StoreController.instance.StandsByPlayfabID[CosmeticsController.instance.currentCart[CosmeticsController.instance.fittingRoomButtons.Length].itemName])
					{
						dynamicCosmeticStand3.AddToCartButton.isOn = false;
						dynamicCosmeticStand3.AddToCartButton.UpdateColor();
					}
					CosmeticsController.instance.currentCart.RemoveAt(CosmeticsController.instance.fittingRoomButtons.Length);
				}
			}
			CosmeticsController.instance.UpdateShoppingCart();
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x0014E240 File Offset: 0x0014C440
		public void SetStandTypeString(string bustTypeString)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(bustTypeString);
			if (num <= 1590453963U)
			{
				if (num <= 1121133049U)
				{
					if (num != 214514339U)
					{
						if (num == 1121133049U)
						{
							if (bustTypeString == "GuitarStand")
							{
								this.SetStandType(HeadModel_CosmeticStand.BustType.GuitarStand);
								return;
							}
						}
					}
					else if (bustTypeString == "GorillaHead")
					{
						this.SetStandType(HeadModel_CosmeticStand.BustType.GorillaHead);
						return;
					}
				}
				else if (num != 1364530810U)
				{
					if (num != 1520673798U)
					{
						if (num == 1590453963U)
						{
							if (bustTypeString == "GorillaMannequin")
							{
								this.SetStandType(HeadModel_CosmeticStand.BustType.GorillaMannequin);
								return;
							}
						}
					}
					else if (bustTypeString == "JewelryBox")
					{
						this.SetStandType(HeadModel_CosmeticStand.BustType.JewelryBox);
						return;
					}
				}
				else if (bustTypeString == "PinDisplay")
				{
					this.SetStandType(HeadModel_CosmeticStand.BustType.PinDisplay);
					return;
				}
			}
			else if (num <= 2111326094U)
			{
				if (num != 1952506660U)
				{
					if (num == 2111326094U)
					{
						if (bustTypeString == "GorillaTorsoPost")
						{
							this.SetStandType(HeadModel_CosmeticStand.BustType.GorillaTorsoPost);
							return;
						}
					}
				}
				else if (bustTypeString == "GorillaTorso")
				{
					this.SetStandType(HeadModel_CosmeticStand.BustType.GorillaTorso);
					return;
				}
			}
			else if (num != 3217987877U)
			{
				if (num != 3607948159U)
				{
					if (num == 3845287012U)
					{
						if (bustTypeString == "TagEffectDisplay")
						{
							this.SetStandType(HeadModel_CosmeticStand.BustType.TagEffectDisplay);
							return;
						}
					}
				}
				else if (bustTypeString == "Table")
				{
					this.SetStandType(HeadModel_CosmeticStand.BustType.Table);
					return;
				}
			}
			else if (bustTypeString == "Disabled")
			{
				this.SetStandType(HeadModel_CosmeticStand.BustType.Disabled);
				return;
			}
			this.SetStandType(HeadModel_CosmeticStand.BustType.Table);
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x0014E3EE File Offset: 0x0014C5EE
		public void UpdateCosmeticsMountPositions()
		{
			this.DisplayHeadModel.UpdateCosmeticsMountPositions(StoreController.FindCosmeticInAllCosmeticsArraySO(this.thisCosmeticName));
		}

		// Token: 0x040047DD RID: 18397
		public HeadModel_CosmeticStand DisplayHeadModel;

		// Token: 0x040047DE RID: 18398
		public GorillaPressableButton AddToCartButton;

		// Token: 0x040047DF RID: 18399
		public Text slotPriceText;

		// Token: 0x040047E0 RID: 18400
		public Text addToCartText;

		// Token: 0x040047E1 RID: 18401
		private CosmeticsController.CosmeticItem thisCosmeticItem;

		// Token: 0x040047E2 RID: 18402
		[FormerlySerializedAs("StandID")]
		public string StandName;

		// Token: 0x040047E3 RID: 18403
		public string _thisCosmeticName = "";

		// Token: 0x040047E4 RID: 18404
		public GameObject GorillaHeadModel;

		// Token: 0x040047E5 RID: 18405
		public GameObject GorillaTorsoModel;

		// Token: 0x040047E6 RID: 18406
		public GameObject GorillaTorsoPostModel;

		// Token: 0x040047E7 RID: 18407
		public GameObject GorillaMannequinModel;

		// Token: 0x040047E8 RID: 18408
		public GameObject GuitarStandModel;

		// Token: 0x040047E9 RID: 18409
		public GameObject GuitarStandMount;

		// Token: 0x040047EA RID: 18410
		public GameObject JeweleryBoxModel;

		// Token: 0x040047EB RID: 18411
		public GameObject JeweleryBoxMount;

		// Token: 0x040047EC RID: 18412
		public GameObject TableMount;

		// Token: 0x040047ED RID: 18413
		[FormerlySerializedAs("PinDisplayMountn")]
		public GameObject PinDisplayMounnt;

		// Token: 0x040047EE RID: 18414
		public GameObject root;

		// Token: 0x040047EF RID: 18415
		public GameObject TagEffectDisplayMount;

		// Token: 0x040047F0 RID: 18416
		public GameObject TageEffectDisplayModel;

		// Token: 0x040047F1 RID: 18417
		public GameObject PinDisplayMountn;

		// Token: 0x040047F2 RID: 18418
		private int searchIndex;
	}
}
