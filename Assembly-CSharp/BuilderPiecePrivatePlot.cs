using System;
using System.Collections.Generic;
using GorillaTagScripts;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004E1 RID: 1249
public class BuilderPiecePrivatePlot : MonoBehaviour
{
	// Token: 0x06001E7B RID: 7803 RVA: 0x00044BE5 File Offset: 0x00042DE5
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x06001E7C RID: 7804 RVA: 0x000E88A0 File Offset: 0x000E6AA0
	private void Init()
	{
		if (this.initDone)
		{
			return;
		}
		this.materialProps = new MaterialPropertyBlock();
		this.usedResources = new int[3];
		for (int i = 0; i < this.usedResources.Length; i++)
		{
			this.usedResources[i] = 0;
		}
		this.tempResourceCount = new int[3];
		this.piece = base.GetComponent<BuilderPiece>();
		this.SetPlotState(BuilderPiecePrivatePlot.PlotState.Vacant);
		this.piecesToCount = new Queue<BuilderPiece>(1024);
		this.initDone = true;
		this.privatePlotIndex = -1;
	}

	// Token: 0x06001E7D RID: 7805 RVA: 0x000E8928 File Offset: 0x000E6B28
	private void Start()
	{
		if (BuilderTable.instance != null)
		{
			this.doesLocalPlayerOwnAPlot = BuilderTable.instance.DoesPlayerOwnPlot(PhotonNetwork.LocalPlayer.ActorNumber);
			BuilderTable.instance.OnLocalPlayerClaimedPlot.AddListener(new UnityAction<bool>(this.OnLocalPlayerClaimedPlot));
			this.UpdateVisuals();
		}
		this.buildArea.gameObject.SetActive(true);
		this.buildArea.enabled = true;
		this.buildAreaBounds = this.buildArea.bounds;
		this.buildArea.gameObject.SetActive(false);
		this.buildArea.enabled = false;
		this.zoneRenderers.Clear();
		this.zoneRenderers.Add(this.tmpLabel.GetComponent<Renderer>());
		foreach (BuilderResourceMeter builderResourceMeter in this.resourceMeters)
		{
			this.zoneRenderers.AddRange(builderResourceMeter.GetComponentsInChildren<Renderer>());
		}
		this.zoneRenderers.AddRange(this.borderMeshes);
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
		this.inBuilderZone = true;
		this.OnZoneChanged();
	}

	// Token: 0x06001E7E RID: 7806 RVA: 0x000E8A80 File Offset: 0x000E6C80
	private void OnDestroy()
	{
		if (BuilderTable.instance != null)
		{
			BuilderTable.instance.OnLocalPlayerClaimedPlot.RemoveListener(new UnityAction<bool>(this.OnLocalPlayerClaimedPlot));
		}
		if (ZoneManagement.instance != null)
		{
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
		}
	}

	// Token: 0x06001E7F RID: 7807 RVA: 0x000E8AE8 File Offset: 0x000E6CE8
	private void OnZoneChanged()
	{
		bool flag = ZoneManagement.instance.IsZoneActive(GTZone.monkeBlocks);
		if (flag && !this.inBuilderZone)
		{
			using (List<Renderer>.Enumerator enumerator = this.zoneRenderers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Renderer renderer = enumerator.Current;
					renderer.enabled = true;
				}
				goto IL_8B;
			}
		}
		if (!flag && this.inBuilderZone)
		{
			foreach (Renderer renderer2 in this.zoneRenderers)
			{
				renderer2.enabled = false;
			}
		}
		IL_8B:
		this.inBuilderZone = flag;
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x00044BED File Offset: 0x00042DED
	private void OnLocalPlayerClaimedPlot(bool claim)
	{
		this.doesLocalPlayerOwnAPlot = claim;
		this.UpdateVisuals();
	}

	// Token: 0x06001E81 RID: 7809 RVA: 0x000E8BA4 File Offset: 0x000E6DA4
	public void UpdatePlot()
	{
		if (BuilderPieceInteractor.instance == null || BuilderPieceInteractor.instance.heldChainLength == null || BuilderPieceInteractor.instance.heldChainLength.Length < 2)
		{
			return;
		}
		if (!PhotonNetwork.InRoom)
		{
			return;
		}
		if (!this.initDone)
		{
			this.Init();
		}
		if ((this.plotState == BuilderPiecePrivatePlot.PlotState.Occupied && this.owningPlayerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) || (this.plotState == BuilderPiecePrivatePlot.PlotState.Vacant && !this.doesLocalPlayerOwnAPlot))
		{
			BuilderPiece parentPiece = BuilderPieceInteractor.instance.prevPotentialPlacement[0].parentPiece;
			BuilderPiece parentPiece2 = BuilderPieceInteractor.instance.prevPotentialPlacement[1].parentPiece;
			bool flag = false;
			if (parentPiece == null && this.leftPotentialParent != null)
			{
				this.isLeftOverPlot = false;
				this.leftPotentialParent = null;
				flag = true;
			}
			else if ((this.leftPotentialParent == null && parentPiece != null) || (parentPiece != null && !parentPiece.Equals(this.leftPotentialParent)))
			{
				BuilderPiece attachedBuiltInPiece = parentPiece.GetAttachedBuiltInPiece();
				this.isLeftOverPlot = (attachedBuiltInPiece != null && attachedBuiltInPiece.Equals(this.piece));
				this.leftPotentialParent = parentPiece;
				flag = true;
			}
			if (parentPiece2 == null && this.rightPotentialParent != null)
			{
				this.isRightOverPlot = false;
				this.rightPotentialParent = null;
				flag = true;
			}
			else if ((this.rightPotentialParent == null && parentPiece2 != null) || (parentPiece2 != null && !parentPiece2.Equals(this.rightPotentialParent)))
			{
				BuilderPiece attachedBuiltInPiece2 = parentPiece2.GetAttachedBuiltInPiece();
				this.isRightOverPlot = (attachedBuiltInPiece2 != null && attachedBuiltInPiece2.Equals(this.piece));
				this.rightPotentialParent = parentPiece2;
				flag = true;
			}
			if (flag)
			{
				this.UpdateVisuals();
			}
		}
		else if (this.isRightOverPlot || this.isLeftOverPlot)
		{
			this.isRightOverPlot = false;
			this.isLeftOverPlot = false;
			this.UpdateVisuals();
		}
		foreach (BuilderResourceMeter builderResourceMeter in this.resourceMeters)
		{
			builderResourceMeter.UpdateMeterFill();
		}
	}

	// Token: 0x06001E82 RID: 7810 RVA: 0x00044BFC File Offset: 0x00042DFC
	public void RecountPlotCost()
	{
		this.Init();
		this.piece.GetChainCost(this.usedResources);
		this.UpdateVisuals();
	}

	// Token: 0x06001E83 RID: 7811 RVA: 0x00044C1B File Offset: 0x00042E1B
	public void OnPieceAttachedToPlot(BuilderPiece attachPiece)
	{
		this.AddChainResourcesToCount(attachPiece, true);
		this.UpdateVisuals();
	}

	// Token: 0x06001E84 RID: 7812 RVA: 0x00044C2B File Offset: 0x00042E2B
	public void OnPieceDetachedFromPlot(BuilderPiece detachPiece)
	{
		this.AddChainResourcesToCount(detachPiece, false);
		this.UpdateVisuals();
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x00044C3B File Offset: 0x00042E3B
	public void ChangeAttachedPieceCount(int delta)
	{
		this.attachedPieceCount += delta;
		this.UpdateVisuals();
	}

	// Token: 0x06001E86 RID: 7814 RVA: 0x000E8DDC File Offset: 0x000E6FDC
	public void AddChainResourcesToCount(BuilderPiece chain, bool attach)
	{
		if (chain == null)
		{
			return;
		}
		this.piecesToCount.Clear();
		for (int i = 0; i < this.tempResourceCount.Length; i++)
		{
			this.tempResourceCount[i] = 0;
		}
		this.piecesToCount.Enqueue(chain);
		this.AddPieceCostToArray(chain, this.tempResourceCount);
		bool flag = false;
		while (this.piecesToCount.Count > 0 && !flag)
		{
			BuilderPiece builderPiece = this.piecesToCount.Dequeue().firstChildPiece;
			while (builderPiece != null)
			{
				this.piecesToCount.Enqueue(builderPiece);
				if (!this.AddPieceCostToArray(builderPiece, this.tempResourceCount))
				{
					Debug.LogWarning("Builder plot placing pieces over limits");
					flag = true;
					break;
				}
				builderPiece = builderPiece.nextSiblingPiece;
			}
		}
		for (int j = 0; j < this.usedResources.Length; j++)
		{
			if (attach)
			{
				this.usedResources[j] += this.tempResourceCount[j];
			}
			else
			{
				this.usedResources[j] -= this.tempResourceCount[j];
			}
		}
	}

	// Token: 0x06001E87 RID: 7815 RVA: 0x00044C51 File Offset: 0x00042E51
	public void ClaimPlotForPlayerNumber(int player)
	{
		this.owningPlayerActorNumber = player;
		this.SetPlotState(BuilderPiecePrivatePlot.PlotState.Occupied);
	}

	// Token: 0x06001E88 RID: 7816 RVA: 0x00044C61 File Offset: 0x00042E61
	public int GetOwnerActorNumber()
	{
		return this.owningPlayerActorNumber;
	}

	// Token: 0x06001E89 RID: 7817 RVA: 0x000E8EE0 File Offset: 0x000E70E0
	public void ClearPlot()
	{
		this.Init();
		this.attachedPieceCount = 0;
		for (int i = 0; i < this.usedResources.Length; i++)
		{
			this.usedResources[i] = 0;
		}
		this.SetPlotState(BuilderPiecePrivatePlot.PlotState.Vacant);
	}

	// Token: 0x06001E8A RID: 7818 RVA: 0x00044C69 File Offset: 0x00042E69
	public void FreePlot()
	{
		this.SetPlotState(BuilderPiecePrivatePlot.PlotState.Vacant);
	}

	// Token: 0x06001E8B RID: 7819 RVA: 0x00044C72 File Offset: 0x00042E72
	public bool IsPlotClaimed()
	{
		return this.plotState > BuilderPiecePrivatePlot.PlotState.Vacant;
	}

	// Token: 0x06001E8C RID: 7820 RVA: 0x000E8F20 File Offset: 0x000E7120
	public bool IsChainUnderCapacity(BuilderPiece chain)
	{
		if (chain == null)
		{
			return true;
		}
		this.piecesToCount.Clear();
		for (int i = 0; i < this.tempResourceCount.Length; i++)
		{
			this.tempResourceCount[i] = this.usedResources[i];
		}
		this.piecesToCount.Enqueue(chain);
		if (!this.AddPieceCostToArray(chain, this.tempResourceCount))
		{
			return false;
		}
		while (this.piecesToCount.Count > 0)
		{
			BuilderPiece builderPiece = this.piecesToCount.Dequeue().firstChildPiece;
			while (builderPiece != null)
			{
				this.piecesToCount.Enqueue(builderPiece);
				if (!this.AddPieceCostToArray(builderPiece, this.tempResourceCount))
				{
					return false;
				}
				builderPiece = builderPiece.nextSiblingPiece;
			}
		}
		return true;
	}

	// Token: 0x06001E8D RID: 7821 RVA: 0x000E8FD4 File Offset: 0x000E71D4
	public bool AddPieceCostToArray(BuilderPiece addedPiece, int[] array)
	{
		if (addedPiece == null)
		{
			return true;
		}
		if (addedPiece.cost != null)
		{
			foreach (BuilderResourceQuantity builderResourceQuantity in addedPiece.cost.quantities)
			{
				if (builderResourceQuantity.type >= BuilderResourceType.Basic && builderResourceQuantity.type < BuilderResourceType.Count)
				{
					array[(int)builderResourceQuantity.type] += builderResourceQuantity.count;
					if (array[(int)builderResourceQuantity.type] > BuilderTable.instance.GetPrivateResourceLimitForType((int)builderResourceQuantity.type))
					{
						return false;
					}
				}
			}
			return true;
		}
		return true;
	}

	// Token: 0x06001E8E RID: 7822 RVA: 0x00044C7D File Offset: 0x00042E7D
	public bool CanPlayerAttachToPlot(int actorNumber)
	{
		return (this.plotState == BuilderPiecePrivatePlot.PlotState.Occupied && this.owningPlayerActorNumber == actorNumber) || (this.plotState == BuilderPiecePrivatePlot.PlotState.Vacant && !BuilderTable.instance.DoesPlayerOwnPlot(actorNumber));
	}

	// Token: 0x06001E8F RID: 7823 RVA: 0x000E9088 File Offset: 0x000E7288
	public bool CanPlayerGrabFromPlot(int actorNumber, Vector3 worldPosition)
	{
		if (this.owningPlayerActorNumber == actorNumber || this.plotState == BuilderPiecePrivatePlot.PlotState.Vacant)
		{
			return true;
		}
		int pieceId;
		if (BuilderTable.instance.plotOwners.TryGetValue(actorNumber, out pieceId))
		{
			BuilderPiece builderPiece = BuilderTable.instance.GetPiece(pieceId);
			BuilderPiecePrivatePlot builderPiecePrivatePlot;
			if (builderPiece != null && builderPiece.TryGetPlotComponent(out builderPiecePrivatePlot))
			{
				return builderPiecePrivatePlot.IsLocationWithinPlotExtents(worldPosition);
			}
		}
		return false;
	}

	// Token: 0x06001E90 RID: 7824 RVA: 0x000E90E4 File Offset: 0x000E72E4
	private void SetPlotState(BuilderPiecePrivatePlot.PlotState newState)
	{
		this.plotState = newState;
		BuilderPiecePrivatePlot.PlotState plotState = this.plotState;
		if (plotState != BuilderPiecePrivatePlot.PlotState.Vacant)
		{
			if (plotState == BuilderPiecePrivatePlot.PlotState.Occupied)
			{
				if (this.tmpLabel != null && NetworkSystem.Instance != null)
				{
					string text = string.Empty;
					NetPlayer player = NetworkSystem.Instance.GetPlayer(this.owningPlayerActorNumber);
					RigContainer rigContainer;
					if (player != null && VRRigCache.Instance.TryGetVrrig(player, out rigContainer))
					{
						text = rigContainer.Rig.playerNameVisible;
					}
					if (string.IsNullOrEmpty(text) && !this.tmpLabel.text.Equals("OCCUPIED"))
					{
						this.tmpLabel.text = "OCCUPIED";
					}
					else if (!this.tmpLabel.text.Equals(text))
					{
						this.tmpLabel.text = text;
					}
				}
				else if (this.tmpLabel != null && !this.tmpLabel.text.Equals("OCCUPIED"))
				{
					this.tmpLabel.text = "OCCUPIED";
				}
			}
		}
		else
		{
			this.owningPlayerActorNumber = -1;
			if (this.tmpLabel != null && !this.tmpLabel.text.Equals(string.Empty))
			{
				this.tmpLabel.text = string.Empty;
			}
		}
		this.UpdateVisuals();
	}

	// Token: 0x06001E91 RID: 7825 RVA: 0x000E9238 File Offset: 0x000E7438
	public bool IsLocationWithinPlotExtents(Vector3 worldPosition)
	{
		if (!this.buildAreaBounds.Contains(worldPosition))
		{
			return false;
		}
		Vector3 vector = this.buildArea.transform.InverseTransformPoint(worldPosition);
		Vector3 vector2 = this.buildArea.center + this.buildArea.size / 2f;
		Vector3 vector3 = this.buildArea.center - this.buildArea.size / 2f;
		return vector.x >= vector3.x && vector.x <= vector2.x && vector.y >= vector3.y && vector.y <= vector2.y && vector.z >= vector3.z && vector.z <= vector2.z;
	}

	// Token: 0x06001E92 RID: 7826 RVA: 0x00044CAB File Offset: 0x00042EAB
	public void OnAvailableResourceChange()
	{
		this.UpdateVisuals();
	}

	// Token: 0x06001E93 RID: 7827 RVA: 0x000E930C File Offset: 0x000E750C
	private void UpdateVisuals()
	{
		if (this.usedResources == null || BuilderTable.instance == null)
		{
			return;
		}
		BuilderPiecePrivatePlot.PlotState plotState = this.plotState;
		if (plotState != BuilderPiecePrivatePlot.PlotState.Vacant)
		{
			if (plotState != BuilderPiecePrivatePlot.PlotState.Occupied)
			{
				return;
			}
			if (this.owningPlayerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
			{
				this.UpdateVisualsForOwner();
				return;
			}
			this.SetBorderColor(this.placementDisallowedColor);
			int num = 0;
			while (num < this.resourceMeters.Count && num < 3)
			{
				int privateResourceLimitForType = BuilderTable.instance.GetPrivateResourceLimitForType(num);
				if (privateResourceLimitForType != 0)
				{
					this.resourceMeters[num].SetNormalizedFillTarget((float)(privateResourceLimitForType - this.usedResources[num]) / (float)privateResourceLimitForType);
				}
				num++;
			}
		}
		else
		{
			if (!this.doesLocalPlayerOwnAPlot)
			{
				this.UpdateVisualsForOwner();
				return;
			}
			this.SetBorderColor(this.placementDisallowedColor);
			for (int i = 0; i < this.resourceMeters.Count; i++)
			{
				if (i >= 3)
				{
					return;
				}
				int privateResourceLimitForType2 = BuilderTable.instance.GetPrivateResourceLimitForType(i);
				if (privateResourceLimitForType2 != 0)
				{
					this.resourceMeters[i].SetNormalizedFillTarget((float)(privateResourceLimitForType2 - this.usedResources[i]) / (float)privateResourceLimitForType2);
				}
			}
			return;
		}
	}

	// Token: 0x06001E94 RID: 7828 RVA: 0x000E9418 File Offset: 0x000E7618
	private void UpdateVisualsForOwner()
	{
		bool flag = true;
		if (this.usedResources == null)
		{
			return;
		}
		if (BuilderPieceInteractor.instance == null || BuilderPieceInteractor.instance.heldChainCost == null)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			int num = this.usedResources[i];
			if (this.isLeftOverPlot)
			{
				num += BuilderPieceInteractor.instance.heldChainCost[0][i];
			}
			if (this.isRightOverPlot)
			{
				num += BuilderPieceInteractor.instance.heldChainCost[1][i];
			}
			int privateResourceLimitForType = BuilderTable.instance.GetPrivateResourceLimitForType(i);
			if (num < privateResourceLimitForType)
			{
				flag = false;
			}
			if (privateResourceLimitForType != 0 && this.resourceMeters.Count > i)
			{
				this.resourceMeters[i].SetNormalizedFillTarget((float)(privateResourceLimitForType - num) / (float)privateResourceLimitForType);
			}
		}
		if (flag)
		{
			this.SetBorderColor(this.placementDisallowedColor);
			return;
		}
		this.SetBorderColor(this.placementAllowedColor);
	}

	// Token: 0x06001E95 RID: 7829 RVA: 0x000E9500 File Offset: 0x000E7700
	private void SetBorderColor(Color color)
	{
		this.borderMeshes[0].GetPropertyBlock(this.materialProps);
		this.materialProps.SetColor("_BaseColor", color);
		foreach (MeshRenderer meshRenderer in this.borderMeshes)
		{
			meshRenderer.SetPropertyBlock(this.materialProps);
		}
	}

	// Token: 0x040021BB RID: 8635
	[SerializeField]
	private Color placementAllowedColor;

	// Token: 0x040021BC RID: 8636
	[SerializeField]
	private Color placementDisallowedColor;

	// Token: 0x040021BD RID: 8637
	[SerializeField]
	private Color overCapacityColor;

	// Token: 0x040021BE RID: 8638
	public List<MeshRenderer> borderMeshes;

	// Token: 0x040021BF RID: 8639
	public BoxCollider buildArea;

	// Token: 0x040021C0 RID: 8640
	[SerializeField]
	private TMP_Text tmpLabel;

	// Token: 0x040021C1 RID: 8641
	[SerializeField]
	private List<BuilderResourceMeter> resourceMeters;

	// Token: 0x040021C2 RID: 8642
	[NonSerialized]
	public int[] usedResources;

	// Token: 0x040021C3 RID: 8643
	[NonSerialized]
	public int[] tempResourceCount;

	// Token: 0x040021C4 RID: 8644
	[SerializeField]
	private GameObject plotClaimedFX;

	// Token: 0x040021C5 RID: 8645
	private BuilderPiece leftPotentialParent;

	// Token: 0x040021C6 RID: 8646
	private BuilderPiece rightPotentialParent;

	// Token: 0x040021C7 RID: 8647
	private bool isLeftOverPlot;

	// Token: 0x040021C8 RID: 8648
	private bool isRightOverPlot;

	// Token: 0x040021C9 RID: 8649
	private Bounds buildAreaBounds;

	// Token: 0x040021CA RID: 8650
	[HideInInspector]
	public BuilderPiece piece;

	// Token: 0x040021CB RID: 8651
	private int owningPlayerActorNumber;

	// Token: 0x040021CC RID: 8652
	private int attachedPieceCount;

	// Token: 0x040021CD RID: 8653
	[HideInInspector]
	public int privatePlotIndex;

	// Token: 0x040021CE RID: 8654
	[HideInInspector]
	public BuilderPiecePrivatePlot.PlotState plotState;

	// Token: 0x040021CF RID: 8655
	private bool doesLocalPlayerOwnAPlot;

	// Token: 0x040021D0 RID: 8656
	private Queue<BuilderPiece> piecesToCount;

	// Token: 0x040021D1 RID: 8657
	private bool initDone;

	// Token: 0x040021D2 RID: 8658
	private MaterialPropertyBlock materialProps;

	// Token: 0x040021D3 RID: 8659
	private List<Renderer> zoneRenderers = new List<Renderer>(12);

	// Token: 0x040021D4 RID: 8660
	private bool inBuilderZone;

	// Token: 0x020004E2 RID: 1250
	public enum PlotState
	{
		// Token: 0x040021D6 RID: 8662
		Vacant,
		// Token: 0x040021D7 RID: 8663
		Occupied
	}
}
