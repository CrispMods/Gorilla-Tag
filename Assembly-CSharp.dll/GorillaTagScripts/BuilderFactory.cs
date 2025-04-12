using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200098B RID: 2443
	public class BuilderFactory : MonoBehaviour
	{
		// Token: 0x06003BBA RID: 15290 RVA: 0x00056294 File Offset: 0x00054494
		private void Awake()
		{
			this.InitIfNeeded();
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x0014FB80 File Offset: 0x0014DD80
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

		// Token: 0x06003BBC RID: 15292 RVA: 0x0014FC78 File Offset: 0x0014DE78
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

		// Token: 0x06003BBD RID: 15293 RVA: 0x0005629C File Offset: 0x0005449C
		public void Show()
		{
			this.RefreshUI();
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x0014FED4 File Offset: 0x0014E0D4
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

		// Token: 0x06003BBF RID: 15295 RVA: 0x0014FF18 File Offset: 0x0014E118
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

		// Token: 0x06003BC0 RID: 15296 RVA: 0x0014FFAC File Offset: 0x0014E1AC
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

		// Token: 0x06003BC1 RID: 15297 RVA: 0x0015002C File Offset: 0x0014E22C
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

		// Token: 0x06003BC2 RID: 15298 RVA: 0x001500AC File Offset: 0x0014E2AC
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

		// Token: 0x06003BC3 RID: 15299 RVA: 0x0015017C File Offset: 0x0014E37C
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

		// Token: 0x06003BC4 RID: 15300 RVA: 0x0015024C File Offset: 0x0014E44C
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

		// Token: 0x06003BC5 RID: 15301 RVA: 0x001502D0 File Offset: 0x0014E4D0
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

		// Token: 0x06003BC6 RID: 15302 RVA: 0x00150350 File Offset: 0x0014E550
		public bool CanBuildPieceType(int pieceType)
		{
			BuilderPiece piecePrefab = this.GetPiecePrefab(pieceType);
			return !(piecePrefab == null) && !piecePrefab.isBuiltIntoTable;
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x00038586 File Offset: 0x00036786
		public bool CanUseMaterialType(int materalType)
		{
			return true;
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x0015037C File Offset: 0x0014E57C
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

		// Token: 0x06003BC9 RID: 15305 RVA: 0x001504F4 File Offset: 0x0014E6F4
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

		// Token: 0x06003BCA RID: 15306 RVA: 0x000562A4 File Offset: 0x000544A4
		public void OnAvailableResourcesChange()
		{
			this.RefreshCostUI();
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x000562AC File Offset: 0x000544AC
		public void CreateRandomPiece()
		{
			Debug.LogError("Create Random Piece No longer implemented");
		}

		// Token: 0x04003CE9 RID: 15593
		public Transform spawnLocation;

		// Token: 0x04003CEA RID: 15594
		private List<int> pieceTypes;

		// Token: 0x04003CEB RID: 15595
		public List<GameObject> itemList;

		// Token: 0x04003CEC RID: 15596
		[HideInInspector]
		public List<BuilderPiece> pieceList;

		// Token: 0x04003CED RID: 15597
		public BuilderOptionButton buildItemButton;

		// Token: 0x04003CEE RID: 15598
		public TextMeshPro itemLabel;

		// Token: 0x04003CEF RID: 15599
		public BuilderOptionButton prevItemButton;

		// Token: 0x04003CF0 RID: 15600
		public BuilderOptionButton nextItemButton;

		// Token: 0x04003CF1 RID: 15601
		public TextMeshPro materialLabel;

		// Token: 0x04003CF2 RID: 15602
		public BuilderOptionButton prevMaterialButton;

		// Token: 0x04003CF3 RID: 15603
		public BuilderOptionButton nextMaterialButton;

		// Token: 0x04003CF4 RID: 15604
		public AudioSource audioSource;

		// Token: 0x04003CF5 RID: 15605
		public AudioClip buildPieceSound;

		// Token: 0x04003CF6 RID: 15606
		public Transform previewMarker;

		// Token: 0x04003CF7 RID: 15607
		public List<BuilderUIResource> resourceCostUIs;

		// Token: 0x04003CF8 RID: 15608
		private BuilderPiece previewPiece;

		// Token: 0x04003CF9 RID: 15609
		private int currPieceTypeIndex;

		// Token: 0x04003CFA RID: 15610
		private int currPieceMaterialIndex;

		// Token: 0x04003CFB RID: 15611
		private Dictionary<int, int> pieceTypeToIndex;

		// Token: 0x04003CFC RID: 15612
		private bool initialized;
	}
}
