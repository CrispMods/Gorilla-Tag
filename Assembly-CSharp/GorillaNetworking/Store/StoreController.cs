using System;
using System.Collections.Generic;
using System.Linq;
using GorillaExtensions;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B2E RID: 2862
	public class StoreController : MonoBehaviour
	{
		// Token: 0x060047BA RID: 18362 RVA: 0x0005EB6B File Offset: 0x0005CD6B
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

		// Token: 0x060047BB RID: 18363 RVA: 0x0018B620 File Offset: 0x00189820
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

		// Token: 0x060047BC RID: 18364 RVA: 0x0018B7FC File Offset: 0x001899FC
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

		// Token: 0x060047BD RID: 18365 RVA: 0x00030607 File Offset: 0x0002E807
		public void ExportCosmeticStandLayoutWithItems()
		{
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x00030607 File Offset: 0x0002E807
		public void ExportCosmeticStandLayoutWITHOUTItems()
		{
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x00030607 File Offset: 0x0002E807
		public void ImportCosmeticStandLayout()
		{
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x0018B8BC File Offset: 0x00189ABC
		public void InitalizeCosmeticStands()
		{
			this.CreateDynamicCosmeticStandsDictionatary();
			foreach (DynamicCosmeticStand dynamicCosmeticStand in this.CosmeticStandsDict.Values)
			{
				dynamicCosmeticStand.InitializeCosmetic();
			}
			this.Create_StandsByPlayfabIDDictionary();
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x0005EBA0 File Offset: 0x0005CDA0
		public void LoadCosmeticOntoStand(string standID, string playFabId)
		{
			if (this.CosmeticStandsDict.ContainsKey(standID))
			{
				this.CosmeticStandsDict[standID].SpawnItemOntoStand(playFabId);
				Debug.Log("StoreStuff: Cosmetic Loaded Onto Stand: " + standID + " | " + playFabId);
			}
		}

		// Token: 0x060047C2 RID: 18370 RVA: 0x0018B920 File Offset: 0x00189B20
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

		// Token: 0x060047C3 RID: 18371 RVA: 0x0005EBD8 File Offset: 0x0005CDD8
		public static CosmeticSO FindCosmeticInAllCosmeticsArraySO(string playfabId)
		{
			if (StoreController.instance == null)
			{
				StoreController.instance = UnityEngine.Object.FindObjectOfType<StoreController>();
			}
			return StoreController.instance.AllCosmeticsArraySO.SearchForCosmeticSO(playfabId);
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x0018B9A4 File Offset: 0x00189BA4
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

		// Token: 0x060047C5 RID: 18373 RVA: 0x0005EC07 File Offset: 0x0005CE07
		public void FindAllDepartments()
		{
			this.Departments = UnityEngine.Object.FindObjectsOfType<StoreDepartment>().ToList<StoreDepartment>();
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x0018BA0C File Offset: 0x00189C0C
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

		// Token: 0x060047C7 RID: 18375 RVA: 0x0018BB2C File Offset: 0x00189D2C
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

		// Token: 0x040048F7 RID: 18679
		public static volatile StoreController instance;

		// Token: 0x040048F8 RID: 18680
		public List<StoreDepartment> Departments;

		// Token: 0x040048F9 RID: 18681
		private Dictionary<string, DynamicCosmeticStand> CosmeticStandsDict;

		// Token: 0x040048FA RID: 18682
		public Dictionary<string, List<DynamicCosmeticStand>> StandsByPlayfabID;

		// Token: 0x040048FB RID: 18683
		public AllCosmeticsArraySO AllCosmeticsArraySO;

		// Token: 0x040048FC RID: 18684
		private string exportHeader = "Department ID\tDisplay ID\tStand ID\tStand Type\tPlayFab ID";
	}
}
