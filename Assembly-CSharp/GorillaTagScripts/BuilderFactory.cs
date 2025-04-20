using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009AE RID: 2478
	public class BuilderFactory : MonoBehaviour
	{
		// Token: 0x06003CC6 RID: 15558 RVA: 0x00057B2B File Offset: 0x00055D2B
		private void Awake()
		{
			this.InitIfNeeded();
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x00155B68 File Offset: 0x00153D68
		public void InitIfNeeded()
		{
			if (this.initialized)
			{
				return;
			}
			this.buildItemButton.Setup(new Action<BuilderOptionButton, bool>(this.OnBuildItem));
			this.currPieceTypeIndex = 0;
			this.prevItemButton.Setup(new Action<BuilderOptionButton, bool>(this.OnPrevItem));
			this.nextItemButton.Setup(new Action<BuilderOptionButton, bool>(this.OnNextItem));
			this.currPieceMaterialIndex = 0;
			this.prevMaterialButton.Setup(new Action<BuilderOptionButton, bool>(this.OnPrevMaterial));
			this.nextMaterialButton.Setup(new Action<BuilderOptionButton, bool>(this.OnNextMaterial));
			this.pieceTypeToIndex = new Dictionary<int, int>(256);
			this.initialized = true;
			if (this.resourceCostUIs != null)
			{
				for (int i = 0; i < this.resourceCostUIs.Count; i++)
				{
					if (this.resourceCostUIs[i] != null)
					{
						this.resourceCostUIs[i].gameObject.SetActive(false);
					}
				}
			}
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x00155C60 File Offset: 0x00153E60
		public void Setup()
		{
			this.InitIfNeeded();
			List<BuilderPiece> list = this.pieceList;
			this.pieceTypes = new List<int>(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				string name = list[i].name;
				int staticHash = name.GetStaticHash();
				int num;
				if (this.pieceTypeToIndex.TryAdd(staticHash, i))
				{
					this.pieceTypes.Add(staticHash);
				}
				else if (this.pieceTypeToIndex.TryGetValue(staticHash, out num))
				{
					string text = "BuilderFactory: ERROR!! " + string.Format("Could not add pieceType \"{0}\" with hash {1} ", name, staticHash) + "to 'pieceTypeToIndex' Dictionary because because it was already added!";
					if (num < 0 || num >= list.Count)
					{
						text += " Also the index to the conflicting piece is out of range of the pieceList!";
					}
					else
					{
						BuilderPiece builderPiece = list[num];
						if (builderPiece != null)
						{
							if (name == builderPiece.name)
							{
								text += "The conflicting piece has the same name (as expected).";
							}
							else
							{
								text = text + "Also the conflicting pieceType has the same hash but different name \"" + builderPiece.name + "\"!";
							}
						}
						else
						{
							text += "And (should never happen) the piece at that slot is null!";
						}
					}
					Debug.LogError(text, this);
				}
			}
			int num2 = this.pieceTypes.Count;
			foreach (BuilderPieceSet builderPieceSet in BuilderSetManager.instance.GetAllPieceSets())
			{
				foreach (BuilderPieceSet.BuilderPieceSubset builderPieceSubset in builderPieceSet.subsets)
				{
					foreach (BuilderPieceSet.PieceInfo pieceInfo in builderPieceSubset.pieceInfos)
					{
						int staticHash2 = pieceInfo.piecePrefab.name.GetStaticHash();
						if (!this.pieceTypeToIndex.ContainsKey(staticHash2))
						{
							this.pieceList.Add(pieceInfo.piecePrefab);
							this.pieceTypes.Add(staticHash2);
							this.pieceTypeToIndex.Add(staticHash2, num2);
							num2++;
						}
					}
				}
			}
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x00057B33 File Offset: 0x00055D33
		public void Show()
		{
			this.RefreshUI();
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x00155EBC File Offset: 0x001540BC
		public BuilderPiece GetPiecePrefab(int pieceType)
		{
			int index;
			if (this.pieceTypeToIndex.TryGetValue(pieceType, out index))
			{
				return this.pieceList[index];
			}
			Debug.LogErrorFormat("No Prefab found for type {0}", new object[]
			{
				pieceType
			});
			return null;
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x00155F00 File Offset: 0x00154100
		public void OnBuildItem(BuilderOptionButton button, bool isLeftHand)
		{
			if (this.pieceTypes != null && this.pieceTypes.Count > this.currPieceTypeIndex)
			{
				int selectedMaterialType = this.GetSelectedMaterialType();
				BuilderTable.instance.RequestCreatePiece(this.pieceTypes[this.currPieceTypeIndex], this.spawnLocation.position, this.spawnLocation.rotation, selectedMaterialType);
				if (this.audioSource != null && this.buildPieceSound != null)
				{
					this.audioSource.GTPlayOneShot(this.buildPieceSound, 1f);
				}
			}
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x00155F94 File Offset: 0x00154194
		public void OnPrevItem(BuilderOptionButton button, bool isLeftHand)
		{
			if (this.pieceTypes != null && this.pieceTypes.Count > 0)
			{
				for (int i = 0; i < this.pieceTypes.Count; i++)
				{
					this.currPieceTypeIndex = (this.currPieceTypeIndex - 1 + this.pieceTypes.Count) % this.pieceTypes.Count;
					if (this.CanBuildPieceType(this.pieceTypes[this.currPieceTypeIndex]))
					{
						break;
					}
				}
				this.RefreshUI();
			}
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x00156014 File Offset: 0x00154214
		public void OnNextItem(BuilderOptionButton button, bool isLeftHand)
		{
			if (this.pieceTypes != null && this.pieceTypes.Count > 0)
			{
				for (int i = 0; i < this.pieceTypes.Count; i++)
				{
					this.currPieceTypeIndex = (this.currPieceTypeIndex + 1 + this.pieceTypes.Count) % this.pieceTypes.Count;
					if (this.CanBuildPieceType(this.pieceTypes[this.currPieceTypeIndex]))
					{
						break;
					}
				}
				this.RefreshUI();
			}
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x00156094 File Offset: 0x00154294
		public void OnPrevMaterial(BuilderOptionButton button, bool isLeftHand)
		{
			if (this.pieceTypes != null && this.pieceTypes.Count > 0)
			{
				BuilderPiece piecePrefab = this.GetPiecePrefab(this.pieceTypes[this.currPieceTypeIndex]);
				if (piecePrefab != null)
				{
					BuilderMaterialOptions materialOptions = piecePrefab.materialOptions;
					if (materialOptions != null && materialOptions.options.Count > 0)
					{
						for (int i = 0; i < materialOptions.options.Count; i++)
						{
							this.currPieceMaterialIndex = (this.currPieceMaterialIndex - 1 + materialOptions.options.Count) % materialOptions.options.Count;
							if (this.CanUseMaterialType(materialOptions.options[this.currPieceMaterialIndex].materialId.GetHashCode()))
							{
								break;
							}
						}
					}
					this.RefreshUI();
				}
			}
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x00156164 File Offset: 0x00154364
		public void OnNextMaterial(BuilderOptionButton button, bool isLeftHand)
		{
			if (this.pieceTypes != null && this.pieceTypes.Count > 0)
			{
				BuilderPiece piecePrefab = this.GetPiecePrefab(this.pieceTypes[this.currPieceTypeIndex]);
				if (piecePrefab != null)
				{
					BuilderMaterialOptions materialOptions = piecePrefab.materialOptions;
					if (materialOptions != null && materialOptions.options.Count > 0)
					{
						for (int i = 0; i < materialOptions.options.Count; i++)
						{
							this.currPieceMaterialIndex = (this.currPieceMaterialIndex + 1 + materialOptions.options.Count) % materialOptions.options.Count;
							if (this.CanUseMaterialType(materialOptions.options[this.currPieceMaterialIndex].materialId.GetHashCode()))
							{
								break;
							}
						}
					}
					this.RefreshUI();
				}
			}
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x00156234 File Offset: 0x00154434
		private int GetSelectedMaterialType()
		{
			int result = -1;
			BuilderPiece piecePrefab = this.GetPiecePrefab(this.pieceTypes[this.currPieceTypeIndex]);
			if (piecePrefab != null)
			{
				BuilderMaterialOptions materialOptions = piecePrefab.materialOptions;
				if (materialOptions != null && materialOptions.options != null && this.currPieceMaterialIndex >= 0 && this.currPieceMaterialIndex < materialOptions.options.Count)
				{
					result = materialOptions.options[this.currPieceMaterialIndex].materialId.GetHashCode();
				}
			}
			return result;
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x001562B8 File Offset: 0x001544B8
		private string GetSelectedMaterialName()
		{
			string result = "DEFAULT";
			BuilderPiece piecePrefab = this.GetPiecePrefab(this.pieceTypes[this.currPieceTypeIndex]);
			if (piecePrefab != null)
			{
				BuilderMaterialOptions materialOptions = piecePrefab.materialOptions;
				if (materialOptions != null && materialOptions.options != null && this.currPieceMaterialIndex >= 0 && this.currPieceMaterialIndex < materialOptions.options.Count)
				{
					result = materialOptions.options[this.currPieceMaterialIndex].materialId;
				}
			}
			return result;
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x00156338 File Offset: 0x00154538
		public bool CanBuildPieceType(int pieceType)
		{
			BuilderPiece piecePrefab = this.GetPiecePrefab(pieceType);
			return !(piecePrefab == null) && !piecePrefab.isBuiltIntoTable;
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x00039846 File Offset: 0x00037A46
		public bool CanUseMaterialType(int materalType)
		{
			return true;
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x00156364 File Offset: 0x00154564
		public void RefreshUI()
		{
			if (this.pieceList != null && this.pieceList.Count > this.currPieceTypeIndex)
			{
				this.itemLabel.SetText(this.pieceList[this.currPieceTypeIndex].displayName, true);
			}
			else
			{
				this.itemLabel.SetText("No Items", true);
			}
			if (this.previewPiece != null)
			{
				BuilderTable.instance.builderPool.DestroyPiece(this.previewPiece);
				this.previewPiece = null;
			}
			if (this.currPieceTypeIndex < 0 || this.currPieceTypeIndex >= this.pieceTypes.Count)
			{
				return;
			}
			int pieceType = this.pieceTypes[this.currPieceTypeIndex];
			this.previewPiece = BuilderTable.instance.builderPool.CreatePiece(pieceType, false);
			this.previewPiece.pieceType = pieceType;
			string selectedMaterialName = this.GetSelectedMaterialName();
			this.materialLabel.SetText(selectedMaterialName, true);
			this.previewPiece.SetScale(BuilderTable.instance.pieceScale * 0.75f);
			this.previewPiece.SetupPiece(BuilderTable.instance.gridSize);
			int selectedMaterialType = this.GetSelectedMaterialType();
			this.previewPiece.SetMaterial(selectedMaterialType, true);
			this.previewPiece.transform.SetPositionAndRotation(this.previewMarker.position, this.previewMarker.rotation);
			this.previewPiece.SetState(BuilderPiece.State.Displayed, false);
			this.previewPiece.enabled = false;
			this.RefreshCostUI();
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x001564DC File Offset: 0x001546DC
		private void RefreshCostUI()
		{
			List<BuilderResourceQuantity> list = null;
			if (this.previewPiece != null)
			{
				list = this.previewPiece.cost.quantities;
			}
			for (int i = 0; i < this.resourceCostUIs.Count; i++)
			{
				if (!(this.resourceCostUIs[i] == null))
				{
					bool flag = list != null && i < list.Count;
					this.resourceCostUIs[i].gameObject.SetActive(flag);
					if (flag)
					{
						this.resourceCostUIs[i].SetResourceCost(list[i]);
					}
				}
			}
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x00057B3B File Offset: 0x00055D3B
		public void OnAvailableResourcesChange()
		{
			this.RefreshCostUI();
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x00057B43 File Offset: 0x00055D43
		public void CreateRandomPiece()
		{
			Debug.LogError("Create Random Piece No longer implemented");
		}

		// Token: 0x04003DB1 RID: 15793
		public Transform spawnLocation;

		// Token: 0x04003DB2 RID: 15794
		private List<int> pieceTypes;

		// Token: 0x04003DB3 RID: 15795
		public List<GameObject> itemList;

		// Token: 0x04003DB4 RID: 15796
		[HideInInspector]
		public List<BuilderPiece> pieceList;

		// Token: 0x04003DB5 RID: 15797
		public BuilderOptionButton buildItemButton;

		// Token: 0x04003DB6 RID: 15798
		public TextMeshPro itemLabel;

		// Token: 0x04003DB7 RID: 15799
		public BuilderOptionButton prevItemButton;

		// Token: 0x04003DB8 RID: 15800
		public BuilderOptionButton nextItemButton;

		// Token: 0x04003DB9 RID: 15801
		public TextMeshPro materialLabel;

		// Token: 0x04003DBA RID: 15802
		public BuilderOptionButton prevMaterialButton;

		// Token: 0x04003DBB RID: 15803
		public BuilderOptionButton nextMaterialButton;

		// Token: 0x04003DBC RID: 15804
		public AudioSource audioSource;

		// Token: 0x04003DBD RID: 15805
		public AudioClip buildPieceSound;

		// Token: 0x04003DBE RID: 15806
		public Transform previewMarker;

		// Token: 0x04003DBF RID: 15807
		public List<BuilderUIResource> resourceCostUIs;

		// Token: 0x04003DC0 RID: 15808
		private BuilderPiece previewPiece;

		// Token: 0x04003DC1 RID: 15809
		private int currPieceTypeIndex;

		// Token: 0x04003DC2 RID: 15810
		private int currPieceMaterialIndex;

		// Token: 0x04003DC3 RID: 15811
		private Dictionary<int, int> pieceTypeToIndex;

		// Token: 0x04003DC4 RID: 15812
		private bool initialized;
	}
}
