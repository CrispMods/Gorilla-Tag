using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B2A RID: 2858
	public class DynamicCosmeticStand : MonoBehaviour, iFlagForBaking
	{
		// Token: 0x0600479C RID: 18332 RVA: 0x0018A718 File Offset: 0x00188918
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

		// Token: 0x0600479D RID: 18333 RVA: 0x0005EA37 File Offset: 0x0005CC37
		public virtual void SetForGame()
		{
			this.DisplayHeadModel.gameObject.SetActive(true);
			this.SetStandType(this.DisplayHeadModel.bustType);
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x0600479E RID: 18334 RVA: 0x0005EA5B File Offset: 0x0005CC5B
		// (set) Token: 0x0600479F RID: 18335 RVA: 0x0005EA63 File Offset: 0x0005CC63
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

		// Token: 0x060047A0 RID: 18336 RVA: 0x0018A780 File Offset: 0x00188980
		public void InitializeCosmetic()
		{
			this.thisCosmeticItem = CosmeticsController.instance.allCosmetics.Find((CosmeticsController.CosmeticItem x) => this.thisCosmeticName == x.displayName || this.thisCosmeticName == x.overrideDisplayName || this.thisCosmeticName == x.itemName);
			if (this.slotPriceText != null)
			{
				this.slotPriceText.text = this.thisCosmeticItem.itemCategory.ToString().ToUpper() + " " + this.thisCosmeticItem.cost.ToString();
			}
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x0018A800 File Offset: 0x00188A00
		public void SpawnItemOntoStand(string PlayFabID)
		{
			this.ClearCosmetics();
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

		// Token: 0x060047A2 RID: 18338 RVA: 0x0005EA6C File Offset: 0x0005CC6C
		public void ClearCosmetics()
		{
			this.thisCosmeticName = "";
			this.DisplayHeadModel.ClearManuallySpawnedCosmeticParts();
			this.DisplayHeadModel.ClearCosmetics();
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x0018A874 File Offset: 0x00188A74
		public void SetStandType(HeadModel_CosmeticStand.BustType newBustType)
		{
			this.DisplayHeadModel.SetStandType(newBustType);
			this.GorillaHeadModel.SetActive(false);
			this.GorillaTorsoModel.SetActive(false);
			this.GorillaTorsoPostModel.SetActive(false);
			this.GorillaMannequinModel.SetActive(false);
			this.GuitarStandModel.SetActive(false);
			this.JeweleryBoxModel.SetActive(false);
			this.AddToCartButton.gameObject.SetActive(true);
			this.slotPriceText.gameObject.SetActive(true);
			this.addToCartText.gameObject.SetActive(true);
			switch (newBustType)
			{
			case HeadModel_CosmeticStand.BustType.Disabled:
				this.ClearCosmetics();
				this.thisCosmeticName = "";
				this.AddToCartButton.gameObject.SetActive(false);
				this.slotPriceText.gameObject.SetActive(false);
				this.addToCartText.gameObject.SetActive(false);
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

		// Token: 0x060047A4 RID: 18340 RVA: 0x0018ACDC File Offset: 0x00188EDC
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

		// Token: 0x060047A5 RID: 18341 RVA: 0x0018AD20 File Offset: 0x00188F20
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

		// Token: 0x060047A6 RID: 18342 RVA: 0x0018AFB0 File Offset: 0x001891B0
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

		// Token: 0x060047A7 RID: 18343 RVA: 0x0005EA8F File Offset: 0x0005CC8F
		public void UpdateCosmeticsMountPositions()
		{
			this.DisplayHeadModel.UpdateCosmeticsMountPositions(StoreController.FindCosmeticInAllCosmeticsArraySO(this.thisCosmeticName));
		}

		// Token: 0x040048D2 RID: 18642
		public HeadModel_CosmeticStand DisplayHeadModel;

		// Token: 0x040048D3 RID: 18643
		public GorillaPressableButton AddToCartButton;

		// Token: 0x040048D4 RID: 18644
		public Text slotPriceText;

		// Token: 0x040048D5 RID: 18645
		public Text addToCartText;

		// Token: 0x040048D6 RID: 18646
		private CosmeticsController.CosmeticItem thisCosmeticItem;

		// Token: 0x040048D7 RID: 18647
		[FormerlySerializedAs("StandID")]
		public string StandName;

		// Token: 0x040048D8 RID: 18648
		public string _thisCosmeticName = "";

		// Token: 0x040048D9 RID: 18649
		public GameObject GorillaHeadModel;

		// Token: 0x040048DA RID: 18650
		public GameObject GorillaTorsoModel;

		// Token: 0x040048DB RID: 18651
		public GameObject GorillaTorsoPostModel;

		// Token: 0x040048DC RID: 18652
		public GameObject GorillaMannequinModel;

		// Token: 0x040048DD RID: 18653
		public GameObject GuitarStandModel;

		// Token: 0x040048DE RID: 18654
		public GameObject GuitarStandMount;

		// Token: 0x040048DF RID: 18655
		public GameObject JeweleryBoxModel;

		// Token: 0x040048E0 RID: 18656
		public GameObject JeweleryBoxMount;

		// Token: 0x040048E1 RID: 18657
		public GameObject TableMount;

		// Token: 0x040048E2 RID: 18658
		[FormerlySerializedAs("PinDisplayMountn")]
		public GameObject PinDisplayMounnt;

		// Token: 0x040048E3 RID: 18659
		public GameObject root;

		// Token: 0x040048E4 RID: 18660
		public GameObject TagEffectDisplayMount;

		// Token: 0x040048E5 RID: 18661
		public GameObject TageEffectDisplayModel;

		// Token: 0x040048E6 RID: 18662
		public GameObject PinDisplayMountn;

		// Token: 0x040048E7 RID: 18663
		private int searchIndex;
	}
}
