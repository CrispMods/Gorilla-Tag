using System;
using System.Collections.Generic;
using System.Linq;
using GorillaExtensions;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B01 RID: 2817
	public class StoreController : MonoBehaviour
	{
		// Token: 0x06004671 RID: 18033 RVA: 0x0014E97D File Offset: 0x0014CB7D
		public void Awake()
		{
			if (StoreController.instance == null)
			{
				StoreController.instance = this;
				return;
			}
			if (StoreController.instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x0014E9B4 File Offset: 0x0014CBB4
		public void CreateDynamicCosmeticStandsDictionatary()
		{
			this.CosmeticStandsDict = new Dictionary<string, DynamicCosmeticStand>();
			foreach (StoreDepartment storeDepartment in this.Departments)
			{
				if (!storeDepartment.departmentName.IsNullOrEmpty())
				{
					foreach (StoreDisplay storeDisplay in storeDepartment.Displays)
					{
						if (!storeDisplay.displayName.IsNullOrEmpty())
						{
							foreach (DynamicCosmeticStand dynamicCosmeticStand in storeDisplay.Stands)
							{
								if (!dynamicCosmeticStand.StandName.IsNullOrEmpty())
								{
									if (!this.CosmeticStandsDict.ContainsKey(string.Concat(new string[]
									{
										storeDepartment.departmentName,
										"|",
										storeDisplay.displayName,
										"|",
										dynamicCosmeticStand.StandName
									})))
									{
										this.CosmeticStandsDict.Add(string.Concat(new string[]
										{
											storeDepartment.departmentName,
											"|",
											storeDisplay.displayName,
											"|",
											dynamicCosmeticStand.StandName
										}), dynamicCosmeticStand);
									}
									else
									{
										Debug.LogError(string.Concat(new string[]
										{
											"StoreStuff: Duplicate Stand Name: ",
											storeDepartment.departmentName,
											"|",
											storeDisplay.displayName,
											"|",
											dynamicCosmeticStand.StandName,
											" Please Fix Gameobject : ",
											dynamicCosmeticStand.gameObject.GetPath(),
											dynamicCosmeticStand.gameObject.name
										}));
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x0014EB90 File Offset: 0x0014CD90
		private void Create_StandsByPlayfabIDDictionary()
		{
			this.StandsByPlayfabID = new Dictionary<string, List<DynamicCosmeticStand>>();
			foreach (DynamicCosmeticStand dynamicCosmeticStand in this.CosmeticStandsDict.Values)
			{
				if (!dynamicCosmeticStand.StandName.IsNullOrEmpty() && !dynamicCosmeticStand.thisCosmeticName.IsNullOrEmpty())
				{
					if (this.StandsByPlayfabID.ContainsKey(dynamicCosmeticStand.thisCosmeticName))
					{
						this.StandsByPlayfabID[dynamicCosmeticStand.thisCosmeticName].Add(dynamicCosmeticStand);
					}
					else
					{
						this.StandsByPlayfabID.Add(dynamicCosmeticStand.thisCosmeticName, new List<DynamicCosmeticStand>
						{
							dynamicCosmeticStand
						});
					}
				}
			}
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x000023F4 File Offset: 0x000005F4
		public void ExportCosmeticStandLayoutWithItems()
		{
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x000023F4 File Offset: 0x000005F4
		public void ExportCosmeticStandLayoutWITHOUTItems()
		{
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x000023F4 File Offset: 0x000005F4
		public void ImportCosmeticStandLayout()
		{
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x0014EC50 File Offset: 0x0014CE50
		public void InitalizeCosmeticStands()
		{
			this.CreateDynamicCosmeticStandsDictionatary();
			foreach (DynamicCosmeticStand dynamicCosmeticStand in this.CosmeticStandsDict.Values)
			{
				dynamicCosmeticStand.InitializeCosmetic();
			}
			this.Create_StandsByPlayfabIDDictionary();
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x0014ECB4 File Offset: 0x0014CEB4
		public void LoadCosmeticOntoStand(string standID, string playFabId)
		{
			if (this.CosmeticStandsDict.ContainsKey(standID))
			{
				this.CosmeticStandsDict[standID].SpawnItemOntoStand(playFabId);
				Debug.Log("StoreStuff: Cosmetic Loaded Onto Stand: " + standID + " | " + playFabId);
			}
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x0014ECEC File Offset: 0x0014CEEC
		public void ClearCosmetics()
		{
			foreach (StoreDepartment storeDepartment in this.Departments)
			{
				StoreDisplay[] displays = storeDepartment.Displays;
				for (int i = 0; i < displays.Length; i++)
				{
					DynamicCosmeticStand[] stands = displays[i].Stands;
					for (int j = 0; j < stands.Length; j++)
					{
						stands[j].ClearCosmetics();
					}
				}
			}
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x0014ED70 File Offset: 0x0014CF70
		public static CosmeticSO FindCosmeticInAllCosmeticsArraySO(string playfabId)
		{
			if (StoreController.instance == null)
			{
				StoreController.instance = Object.FindObjectOfType<StoreController>();
			}
			return StoreController.instance.AllCosmeticsArraySO.SearchForCosmeticSO(playfabId);
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x0014EDA0 File Offset: 0x0014CFA0
		public DynamicCosmeticStand FindCosmeticStandByCosmeticName(string PlayFabID)
		{
			foreach (DynamicCosmeticStand dynamicCosmeticStand in this.CosmeticStandsDict.Values)
			{
				if (dynamicCosmeticStand.thisCosmeticName == PlayFabID)
				{
					return dynamicCosmeticStand;
				}
			}
			return null;
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x0014EE08 File Offset: 0x0014D008
		public void FindAllDepartments()
		{
			this.Departments = Object.FindObjectsOfType<StoreDepartment>().ToList<StoreDepartment>();
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x0014EE1C File Offset: 0x0014D01C
		public void SaveAllCosmeticsPositions()
		{
			foreach (StoreDepartment storeDepartment in this.Departments)
			{
				foreach (StoreDisplay storeDisplay in storeDepartment.Displays)
				{
					foreach (DynamicCosmeticStand dynamicCosmeticStand in storeDisplay.Stands)
					{
						Debug.Log(string.Concat(new string[]
						{
							"StoreStuff: Saving Items mount transform: ",
							storeDepartment.departmentName,
							"|",
							storeDisplay.displayName,
							"|",
							dynamicCosmeticStand.StandName,
							"|",
							dynamicCosmeticStand.DisplayHeadModel.bustType.ToString(),
							"|",
							dynamicCosmeticStand.thisCosmeticName
						}));
						dynamicCosmeticStand.UpdateCosmeticsMountPositions();
					}
				}
			}
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x0014EF3C File Offset: 0x0014D13C
		public static void SetForGame()
		{
			if (StoreController.instance == null)
			{
				StoreController.instance = Object.FindObjectOfType<StoreController>();
			}
			StoreController.instance.CreateDynamicCosmeticStandsDictionatary();
			foreach (DynamicCosmeticStand dynamicCosmeticStand in StoreController.instance.CosmeticStandsDict.Values)
			{
				dynamicCosmeticStand.SetStandType(dynamicCosmeticStand.DisplayHeadModel.bustType);
				dynamicCosmeticStand.SpawnItemOntoStand(dynamicCosmeticStand.thisCosmeticName);
			}
		}

		// Token: 0x04004802 RID: 18434
		public static volatile StoreController instance;

		// Token: 0x04004803 RID: 18435
		public List<StoreDepartment> Departments;

		// Token: 0x04004804 RID: 18436
		private Dictionary<string, DynamicCosmeticStand> CosmeticStandsDict;

		// Token: 0x04004805 RID: 18437
		public Dictionary<string, List<DynamicCosmeticStand>> StandsByPlayfabID;

		// Token: 0x04004806 RID: 18438
		public AllCosmeticsArraySO AllCosmeticsArraySO;

		// Token: 0x04004807 RID: 18439
		private string exportHeader = "Department ID\tDisplay ID\tStand ID\tStand Type\tPlayFab ID";
	}
}
