using System;
using System.Collections.Generic;
using System.Linq;
using GorillaExtensions;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B04 RID: 2820
	public class StoreController : MonoBehaviour
	{
		// Token: 0x0600467D RID: 18045 RVA: 0x0005D154 File Offset: 0x0005B354
		public void Awake()
		{
			if (StoreController.instance == null)
			{
				StoreController.instance = this;
				return;
			}
			if (StoreController.instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x001846AC File Offset: 0x001828AC
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

		// Token: 0x0600467F RID: 18047 RVA: 0x00184888 File Offset: 0x00182A88
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

		// Token: 0x06004680 RID: 18048 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void ExportCosmeticStandLayoutWithItems()
		{
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void ExportCosmeticStandLayoutWITHOUTItems()
		{
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void ImportCosmeticStandLayout()
		{
		}

		// Token: 0x06004683 RID: 18051 RVA: 0x00184948 File Offset: 0x00182B48
		public void InitalizeCosmeticStands()
		{
			this.CreateDynamicCosmeticStandsDictionatary();
			foreach (DynamicCosmeticStand dynamicCosmeticStand in this.CosmeticStandsDict.Values)
			{
				dynamicCosmeticStand.InitializeCosmetic();
			}
			this.Create_StandsByPlayfabIDDictionary();
		}

		// Token: 0x06004684 RID: 18052 RVA: 0x0005D189 File Offset: 0x0005B389
		public void LoadCosmeticOntoStand(string standID, string playFabId)
		{
			if (this.CosmeticStandsDict.ContainsKey(standID))
			{
				this.CosmeticStandsDict[standID].SpawnItemOntoStand(playFabId);
				Debug.Log("StoreStuff: Cosmetic Loaded Onto Stand: " + standID + " | " + playFabId);
			}
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x001849AC File Offset: 0x00182BAC
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

		// Token: 0x06004686 RID: 18054 RVA: 0x0005D1C1 File Offset: 0x0005B3C1
		public static CosmeticSO FindCosmeticInAllCosmeticsArraySO(string playfabId)
		{
			if (StoreController.instance == null)
			{
				StoreController.instance = UnityEngine.Object.FindObjectOfType<StoreController>();
			}
			return StoreController.instance.AllCosmeticsArraySO.SearchForCosmeticSO(playfabId);
		}

		// Token: 0x06004687 RID: 18055 RVA: 0x00184A30 File Offset: 0x00182C30
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

		// Token: 0x06004688 RID: 18056 RVA: 0x0005D1F0 File Offset: 0x0005B3F0
		public void FindAllDepartments()
		{
			this.Departments = UnityEngine.Object.FindObjectsOfType<StoreDepartment>().ToList<StoreDepartment>();
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x00184A98 File Offset: 0x00182C98
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

		// Token: 0x0600468A RID: 18058 RVA: 0x00184BB8 File Offset: 0x00182DB8
		public static void SetForGame()
		{
			if (StoreController.instance == null)
			{
				StoreController.instance = UnityEngine.Object.FindObjectOfType<StoreController>();
			}
			StoreController.instance.CreateDynamicCosmeticStandsDictionatary();
			foreach (DynamicCosmeticStand dynamicCosmeticStand in StoreController.instance.CosmeticStandsDict.Values)
			{
				dynamicCosmeticStand.SetStandType(dynamicCosmeticStand.DisplayHeadModel.bustType);
				dynamicCosmeticStand.SpawnItemOntoStand(dynamicCosmeticStand.thisCosmeticName);
			}
		}

		// Token: 0x04004814 RID: 18452
		public static volatile StoreController instance;

		// Token: 0x04004815 RID: 18453
		public List<StoreDepartment> Departments;

		// Token: 0x04004816 RID: 18454
		private Dictionary<string, DynamicCosmeticStand> CosmeticStandsDict;

		// Token: 0x04004817 RID: 18455
		public Dictionary<string, List<DynamicCosmeticStand>> StandsByPlayfabID;

		// Token: 0x04004818 RID: 18456
		public AllCosmeticsArraySO AllCosmeticsArraySO;

		// Token: 0x04004819 RID: 18457
		private string exportHeader = "Department ID\tDisplay ID\tStand ID\tStand Type\tPlayFab ID";
	}
}
