using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000988 RID: 2440
	public class BuilderFactory : MonoBehaviour
	{
		// Token: 0x06003BAE RID: 15278 RVA: 0x00112BFD File Offset: 0x00110DFD
		private void Awake()
		{
			this.InitIfNeeded();
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x00112C08 File Offset: 0x00110E08
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

		// Token: 0x06003BB0 RID: 15280 RVA: 0x00112D00 File Offset: 0x00110F00
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

		// Token: 0x06003BB1 RID: 15281 RVA: 0x00112F5C File Offset: 0x0011115C
		public void Show()
		{
			this.RefreshUI();
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x00112F64 File Offset: 0x00111164
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

		// Token: 0x06003BB3 RID: 15283 RVA: 0x00112FA8 File Offset: 0x001111A8
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

		// Token: 0x06003BB4 RID: 15284 RVA: 0x0011303C File Offset: 0x0011123C
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

		// Token: 0x06003BB5 RID: 15285 RVA: 0x001130BC File Offset: 0x001112BC
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

		// Token: 0x06003BB6 RID: 15286 RVA: 0x0011313C File Offset: 0x0011133C
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

		// Token: 0x06003BB7 RID: 15287 RVA: 0x0011320C File Offset: 0x0011140C
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

		// Token: 0x06003BB8 RID: 15288 RVA: 0x001132DC File Offset: 0x001114DC
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

		// Token: 0x06003BB9 RID: 15289 RVA: 0x00113360 File Offset: 0x00111560
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

		// Token: 0x06003BBA RID: 15290 RVA: 0x001133E0 File Offset: 0x001115E0
		public bool CanBuildPieceType(int pieceType)
		{
			BuilderPiece piecePrefab = this.GetPiecePrefab(pieceType);
			return !(piecePrefab == null) && !piecePrefab.isBuiltIntoTable;
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x000444E2 File Offset: 0x000426E2
		public bool CanUseMaterialType(int materalType)
		{
			return true;
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x0011340C File Offset: 0x0011160C
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

		// Token: 0x06003BBD RID: 15293 RVA: 0x00113584 File Offset: 0x00111784
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

		// Token: 0x06003BBE RID: 15294 RVA: 0x0011361E File Offset: 0x0011181E
		public void OnAvailableResourcesChange()
		{
			this.RefreshCostUI();
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x00113626 File Offset: 0x00111826
		public void CreateRandomPiece()
		{
			Debug.LogError("Create Random Piece No longer implemented");
		}

		// Token: 0x04003CD7 RID: 15575
		public Transform spawnLocation;

		// Token: 0x04003CD8 RID: 15576
		private List<int> pieceTypes;

		// Token: 0x04003CD9 RID: 15577
		public List<GameObject> itemList;

		// Token: 0x04003CDA RID: 15578
		[HideInInspector]
		public List<BuilderPiece> pieceList;

		// Token: 0x04003CDB RID: 15579
		public BuilderOptionButton buildItemButton;

		// Token: 0x04003CDC RID: 15580
		public TextMeshPro itemLabel;

		// Token: 0x04003CDD RID: 15581
		public BuilderOptionButton prevItemButton;

		// Token: 0x04003CDE RID: 15582
		public BuilderOptionButton nextItemButton;

		// Token: 0x04003CDF RID: 15583
		public TextMeshPro materialLabel;

		// Token: 0x04003CE0 RID: 15584
		public BuilderOptionButton prevMaterialButton;

		// Token: 0x04003CE1 RID: 15585
		public BuilderOptionButton nextMaterialButton;

		// Token: 0x04003CE2 RID: 15586
		public AudioSource audioSource;

		// Token: 0x04003CE3 RID: 15587
		public AudioClip buildPieceSound;

		// Token: 0x04003CE4 RID: 15588
		public Transform previewMarker;

		// Token: 0x04003CE5 RID: 15589
		public List<BuilderUIResource> resourceCostUIs;

		// Token: 0x04003CE6 RID: 15590
		private BuilderPiece previewPiece;

		// Token: 0x04003CE7 RID: 15591
		private int currPieceTypeIndex;

		// Token: 0x04003CE8 RID: 15592
		private int currPieceMaterialIndex;

		// Token: 0x04003CE9 RID: 15593
		private Dictionary<int, int> pieceTypeToIndex;

		// Token: 0x04003CEA RID: 15594
		private bool initialized;
	}
}
