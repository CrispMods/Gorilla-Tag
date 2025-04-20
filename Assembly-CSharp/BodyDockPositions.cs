using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020003DE RID: 990
public class BodyDockPositions : MonoBehaviour
{
	// Token: 0x170002AB RID: 683
	// (get) Token: 0x060017D2 RID: 6098 RVA: 0x000401ED File Offset: 0x0003E3ED
	// (set) Token: 0x060017D3 RID: 6099 RVA: 0x000401F5 File Offset: 0x0003E3F5
	public TransferrableObject[] allObjects
	{
		get
		{
			return this._allObjects;
		}
		set
		{
			this._allObjects = value;
		}
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x000C98DC File Offset: 0x000C7ADC
	public void Awake()
	{
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
		RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x000401FE File Offset: 0x0003E3FE
	public void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		if (object.Equals(this.myRig.creator, otherPlayer))
		{
			this.DeallocateSharableInstances();
		}
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x00040219 File Offset: 0x0003E419
	public void OnLeftRoom()
	{
		this.DeallocateSharableInstances();
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x000C992C File Offset: 0x000C7B2C
	public WorldShareableItem AllocateSharableInstance(BodyDockPositions.DropPositions position, NetPlayer owner)
	{
		switch (position)
		{
		case BodyDockPositions.DropPositions.None:
		case BodyDockPositions.DropPositions.LeftArm:
		case BodyDockPositions.DropPositions.RightArm:
		case BodyDockPositions.DropPositions.LeftArm | BodyDockPositions.DropPositions.RightArm:
		case BodyDockPositions.DropPositions.Chest:
		case BodyDockPositions.DropPositions.MaxDropPostions:
		case BodyDockPositions.DropPositions.RightArm | BodyDockPositions.DropPositions.Chest:
		case BodyDockPositions.DropPositions.LeftArm | BodyDockPositions.DropPositions.RightArm | BodyDockPositions.DropPositions.Chest:
			break;
		case BodyDockPositions.DropPositions.LeftBack:
			if (this.leftBackSharableItem == null)
			{
				this.leftBackSharableItem = ObjectPools.instance.Instantiate(this.SharableItemInstance).GetComponent<WorldShareableItem>();
				this.leftBackSharableItem.GetComponent<RequestableOwnershipGuard>().SetOwnership(owner, false, true);
				this.leftBackSharableItem.GetComponent<WorldShareableItem>().SetupSharableViewIDs(owner, 3);
			}
			return this.leftBackSharableItem;
		default:
			if (position == BodyDockPositions.DropPositions.RightBack)
			{
				if (this.rightBackShareableItem == null)
				{
					this.rightBackShareableItem = ObjectPools.instance.Instantiate(this.SharableItemInstance).GetComponent<WorldShareableItem>();
					this.rightBackShareableItem.GetComponent<RequestableOwnershipGuard>().SetOwnership(owner, false, true);
					this.rightBackShareableItem.GetComponent<WorldShareableItem>().SetupSharableViewIDs(owner, 4);
				}
				return this.rightBackShareableItem;
			}
			if (position != BodyDockPositions.DropPositions.All)
			{
			}
			break;
		}
		throw new ArgumentOutOfRangeException("position", position, null);
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x000C9A24 File Offset: 0x000C7C24
	public void DeallocateSharableInstance(WorldShareableItem worldShareable)
	{
		if (worldShareable == null)
		{
			return;
		}
		if (worldShareable == this.leftBackSharableItem)
		{
			if (this.leftBackSharableItem == null)
			{
				return;
			}
			this.leftBackSharableItem.ResetViews();
			ObjectPools.instance.Destroy(this.leftBackSharableItem.gameObject);
			this.leftBackSharableItem = null;
		}
		if (worldShareable == this.rightBackShareableItem)
		{
			if (this.rightBackShareableItem == null)
			{
				return;
			}
			this.rightBackShareableItem.ResetViews();
			ObjectPools.instance.Destroy(this.rightBackShareableItem.gameObject);
			this.rightBackShareableItem = null;
		}
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x000C9AB8 File Offset: 0x000C7CB8
	public void DeallocateSharableInstances()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		if (this.rightBackShareableItem != null)
		{
			this.rightBackShareableItem.ResetViews();
			ObjectPools.instance.Destroy(this.rightBackShareableItem.gameObject);
		}
		if (this.leftBackSharableItem != null)
		{
			this.leftBackSharableItem.ResetViews();
			ObjectPools.instance.Destroy(this.leftBackSharableItem.gameObject);
		}
		this.leftBackSharableItem = null;
		this.rightBackShareableItem = null;
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x00040221 File Offset: 0x0003E421
	public static bool IsPositionLeft(BodyDockPositions.DropPositions pos)
	{
		return pos == BodyDockPositions.DropPositions.LeftArm || pos == BodyDockPositions.DropPositions.LeftBack;
	}

	// Token: 0x060017DB RID: 6107 RVA: 0x000C9B2C File Offset: 0x000C7D2C
	public int DropZoneStorageUsed(BodyDockPositions.DropPositions dropPosition)
	{
		if (this.myRig == null)
		{
			Debug.Log("BodyDockPositions lost reference to VR Rig, resetting it now", this);
			this.myRig = base.GetComponent<VRRig>();
		}
		if (this.myRig == null)
		{
			Debug.Log("Unable to reset reference");
			return -1;
		}
		for (int i = 0; i < this.myRig.ActiveTransferrableObjectIndexLength(); i++)
		{
			if (this.myRig.ActiveTransferrableObjectIndex(i) >= 0 && this.allObjects[this.myRig.ActiveTransferrableObjectIndex(i)].gameObject.activeInHierarchy && this.allObjects[this.myRig.ActiveTransferrableObjectIndex(i)].storedZone == dropPosition)
			{
				return this.myRig.ActiveTransferrableObjectIndex(i);
			}
		}
		return -1;
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x000C9BE8 File Offset: 0x000C7DE8
	public TransferrableObject ItemPositionInUse(BodyDockPositions.DropPositions dropPosition)
	{
		TransferrableObject.PositionState positionState = this.MapDropPositionToState(dropPosition);
		if (this.myRig == null)
		{
			Debug.Log("BodyDockPositions lost reference to VR Rig, resetting it now", this);
			this.myRig = base.GetComponent<VRRig>();
		}
		if (this.myRig == null)
		{
			Debug.Log("Unable to reset reference");
			return null;
		}
		for (int i = 0; i < this.myRig.ActiveTransferrableObjectIndexLength(); i++)
		{
			if (this.myRig.ActiveTransferrableObjectIndex(i) != -1 && this.allObjects[this.myRig.ActiveTransferrableObjectIndex(i)].gameObject.activeInHierarchy && this.allObjects[this.myRig.ActiveTransferrableObjectIndex(i)].currentState == positionState)
			{
				return this.allObjects[this.myRig.ActiveTransferrableObjectIndex(i)];
			}
		}
		return null;
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x000C9CB0 File Offset: 0x000C7EB0
	private int EnableTransferrableItem(int allItemsIndex, BodyDockPositions.DropPositions startingPosition, TransferrableObject.PositionState startingState)
	{
		if (allItemsIndex < 0 || allItemsIndex >= this.allObjects.Length)
		{
			return -1;
		}
		if (this.myRig != null && this.myRig.isOfflineVRRig)
		{
			for (int i = 0; i < this.myRig.ActiveTransferrableObjectIndexLength(); i++)
			{
				if (this.myRig.ActiveTransferrableObjectIndex(i) == allItemsIndex)
				{
					this.DisableTransferrableItem(allItemsIndex);
				}
			}
			for (int j = 0; j < this.myRig.ActiveTransferrableObjectIndexLength(); j++)
			{
				if (this.myRig.ActiveTransferrableObjectIndex(j) == -1)
				{
					string itemNameFromDisplayName = CosmeticsController.instance.GetItemNameFromDisplayName(this.allObjects[allItemsIndex].gameObject.name);
					if (this.myRig.IsItemAllowed(itemNameFromDisplayName))
					{
						this.myRig.SetActiveTransferrableObjectIndex(j, allItemsIndex);
						this.myRig.SetTransferrablePosStates(j, startingState);
						this.myRig.SetTransferrableItemStates(j, (TransferrableObject.ItemStates)0);
						this.myRig.SetTransferrableDockPosition(j, startingPosition);
						this.EnableTransferrableGameObject(allItemsIndex, startingPosition, startingState);
						return j;
					}
				}
			}
		}
		return -1;
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x0004022D File Offset: 0x0003E42D
	public BodyDockPositions.DropPositions ItemActive(int allItemsIndex)
	{
		if (!this.allObjects[allItemsIndex].gameObject.activeSelf)
		{
			return BodyDockPositions.DropPositions.None;
		}
		return this.allObjects[allItemsIndex].storedZone;
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x000C9DB0 File Offset: 0x000C7FB0
	public static BodyDockPositions.DropPositions OfflineItemActive(int allItemsIndex)
	{
		if (GorillaTagger.Instance == null || GorillaTagger.Instance.offlineVRRig == null)
		{
			return BodyDockPositions.DropPositions.None;
		}
		BodyDockPositions component = GorillaTagger.Instance.offlineVRRig.GetComponent<BodyDockPositions>();
		if (component == null)
		{
			return BodyDockPositions.DropPositions.None;
		}
		if (!component.allObjects[allItemsIndex].gameObject.activeSelf)
		{
			return BodyDockPositions.DropPositions.None;
		}
		return component.allObjects[allItemsIndex].storedZone;
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x000C9E20 File Offset: 0x000C8020
	public void DisableTransferrableItem(int index)
	{
		TransferrableObject transferrableObject = this.allObjects[index];
		if (transferrableObject.gameObject.activeSelf)
		{
			transferrableObject.gameObject.Disable();
			transferrableObject.storedZone = BodyDockPositions.DropPositions.None;
		}
		if (this.myRig.isOfflineVRRig)
		{
			for (int i = 0; i < this.myRig.ActiveTransferrableObjectIndexLength(); i++)
			{
				if (this.myRig.ActiveTransferrableObjectIndex(i) == index)
				{
					this.myRig.SetActiveTransferrableObjectIndex(i, -1);
				}
			}
		}
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x000C9E94 File Offset: 0x000C8094
	public void DisableAllTransferableItems()
	{
		if (!CosmeticsV2Spawner_Dirty.allPartsInstantiated)
		{
			return;
		}
		for (int i = 0; i < this.myRig.ActiveTransferrableObjectIndexLength(); i++)
		{
			int num = this.myRig.ActiveTransferrableObjectIndex(i);
			if (num >= 0 && num < this.allObjects.Length)
			{
				TransferrableObject transferrableObject = this.allObjects[num];
				transferrableObject.gameObject.Disable();
				transferrableObject.storedZone = BodyDockPositions.DropPositions.None;
				this.myRig.SetActiveTransferrableObjectIndex(i, -1);
				this.myRig.SetTransferrableItemStates(i, (TransferrableObject.ItemStates)0);
				this.myRig.SetTransferrablePosStates(i, TransferrableObject.PositionState.None);
			}
		}
		this.DeallocateSharableInstances();
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x00040252 File Offset: 0x0003E452
	private bool AllItemsIndexValid(int allItemsIndex)
	{
		return allItemsIndex != -1 && allItemsIndex < this.allObjects.Length;
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x00040265 File Offset: 0x0003E465
	public bool PositionAvailable(int allItemIndex, BodyDockPositions.DropPositions startPos)
	{
		return (this.allObjects[allItemIndex].dockPositions & startPos) > BodyDockPositions.DropPositions.None;
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x000C9F24 File Offset: 0x000C8124
	public BodyDockPositions.DropPositions FirstAvailablePosition(int allItemIndex)
	{
		for (int i = 0; i < 5; i++)
		{
			BodyDockPositions.DropPositions dropPositions = (BodyDockPositions.DropPositions)(1 << i);
			if ((this.allObjects[allItemIndex].dockPositions & dropPositions) != BodyDockPositions.DropPositions.None)
			{
				return dropPositions;
			}
		}
		return BodyDockPositions.DropPositions.None;
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x00040279 File Offset: 0x0003E479
	public int TransferrableItemDisable(int allItemsIndex)
	{
		if (BodyDockPositions.OfflineItemActive(allItemsIndex) != BodyDockPositions.DropPositions.None)
		{
			this.DisableTransferrableItem(allItemsIndex);
		}
		return 0;
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x000C9F58 File Offset: 0x000C8158
	public void TransferrableItemDisableAtPosition(BodyDockPositions.DropPositions dropPositions)
	{
		int num = this.DropZoneStorageUsed(dropPositions);
		if (num >= 0)
		{
			this.TransferrableItemDisable(num);
		}
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x000C9F7C File Offset: 0x000C817C
	public void TransferrableItemEnableAtPosition(string itemName, BodyDockPositions.DropPositions dropPosition)
	{
		if (this.DropZoneStorageUsed(dropPosition) >= 0)
		{
			return;
		}
		List<int> list = this.TransferrableObjectIndexFromName(itemName);
		if (list.Count == 0)
		{
			return;
		}
		TransferrableObject.PositionState startingState = this.MapDropPositionToState(dropPosition);
		if (list.Count == 1)
		{
			this.EnableTransferrableItem(list[0], dropPosition, startingState);
			return;
		}
		int allItemsIndex = BodyDockPositions.IsPositionLeft(dropPosition) ? list[0] : list[1];
		this.EnableTransferrableItem(allItemsIndex, dropPosition, startingState);
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x000C9FEC File Offset: 0x000C81EC
	public bool TransferrableItemActive(string transferrableItemName)
	{
		List<int> list = this.TransferrableObjectIndexFromName(transferrableItemName);
		if (list.Count == 0)
		{
			return false;
		}
		foreach (int allItemsIndex in list)
		{
			if (this.TransferrableItemActive(allItemsIndex))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x000CA058 File Offset: 0x000C8258
	public bool TransferrableItemActiveAtPos(string transferrableItemName, BodyDockPositions.DropPositions dropPosition)
	{
		List<int> list = this.TransferrableObjectIndexFromName(transferrableItemName);
		if (list.Count == 0)
		{
			return false;
		}
		foreach (int allItemsIndex in list)
		{
			BodyDockPositions.DropPositions dropPositions = this.TransferrableItemPosition(allItemsIndex);
			if (dropPositions != BodyDockPositions.DropPositions.None && dropPositions == dropPosition)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060017EA RID: 6122 RVA: 0x0004028B File Offset: 0x0003E48B
	public bool TransferrableItemActive(int allItemsIndex)
	{
		return this.ItemActive(allItemsIndex) > BodyDockPositions.DropPositions.None;
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x00040297 File Offset: 0x0003E497
	public TransferrableObject TransferrableItem(int allItemsIndex)
	{
		return this.allObjects[allItemsIndex];
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x000402A1 File Offset: 0x0003E4A1
	public BodyDockPositions.DropPositions TransferrableItemPosition(int allItemsIndex)
	{
		return this.ItemActive(allItemsIndex);
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x000CA0CC File Offset: 0x000C82CC
	public bool DisableTransferrableItem(string transferrableItemName)
	{
		List<int> list = this.TransferrableObjectIndexFromName(transferrableItemName);
		if (list.Count == 0)
		{
			return false;
		}
		foreach (int index in list)
		{
			this.DisableTransferrableItem(index);
		}
		return true;
	}

	// Token: 0x060017EE RID: 6126 RVA: 0x000402AA File Offset: 0x0003E4AA
	public BodyDockPositions.DropPositions OppositePosition(BodyDockPositions.DropPositions pos)
	{
		if (pos == BodyDockPositions.DropPositions.LeftArm)
		{
			return BodyDockPositions.DropPositions.RightArm;
		}
		if (pos == BodyDockPositions.DropPositions.RightArm)
		{
			return BodyDockPositions.DropPositions.LeftArm;
		}
		if (pos == BodyDockPositions.DropPositions.LeftBack)
		{
			return BodyDockPositions.DropPositions.RightBack;
		}
		if (pos == BodyDockPositions.DropPositions.RightBack)
		{
			return BodyDockPositions.DropPositions.LeftBack;
		}
		return pos;
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x000CA130 File Offset: 0x000C8330
	public BodyDockPositions.DockingResult ToggleWithHandedness(string transferrableItemName, bool isLeftHand, bool bothHands)
	{
		List<int> list = this.TransferrableObjectIndexFromName(transferrableItemName);
		if (list.Count == 0)
		{
			return new BodyDockPositions.DockingResult();
		}
		if (!this.AllItemsIndexValid(list[0]))
		{
			return new BodyDockPositions.DockingResult();
		}
		BodyDockPositions.DropPositions startingPos;
		if (isLeftHand)
		{
			startingPos = (((this.allObjects[list[0]].dockPositions & BodyDockPositions.DropPositions.RightArm) != BodyDockPositions.DropPositions.None) ? BodyDockPositions.DropPositions.RightArm : BodyDockPositions.DropPositions.LeftBack);
		}
		else
		{
			startingPos = (((this.allObjects[list[0]].dockPositions & BodyDockPositions.DropPositions.LeftArm) != BodyDockPositions.DropPositions.None) ? BodyDockPositions.DropPositions.LeftArm : BodyDockPositions.DropPositions.RightBack);
		}
		return this.ToggleTransferrableItem(transferrableItemName, startingPos, bothHands);
	}

	// Token: 0x060017F0 RID: 6128 RVA: 0x000CA1B0 File Offset: 0x000C83B0
	public BodyDockPositions.DockingResult ToggleTransferrableItem(string transferrableItemName, BodyDockPositions.DropPositions startingPos, bool bothHands)
	{
		BodyDockPositions.DockingResult dockingResult = new BodyDockPositions.DockingResult();
		List<int> list = this.TransferrableObjectIndexFromName(transferrableItemName);
		if (list.Count == 0)
		{
			return dockingResult;
		}
		if (bothHands && list.Count == 2)
		{
			for (int i = 0; i < list.Count; i++)
			{
				int allItemsIndex = list[i];
				BodyDockPositions.DropPositions dropPositions = BodyDockPositions.OfflineItemActive(allItemsIndex);
				if (dropPositions != BodyDockPositions.DropPositions.None)
				{
					this.TransferrableItemDisable(allItemsIndex);
					dockingResult.positionsDisabled.Add(dropPositions);
				}
			}
			if (dockingResult.positionsDisabled.Count >= 1)
			{
				return dockingResult;
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			int num = list[j];
			BodyDockPositions.DropPositions dropPositions2 = startingPos;
			if (bothHands && j != 0)
			{
				dropPositions2 = this.OppositePosition(dropPositions2);
			}
			if (!this.PositionAvailable(num, dropPositions2))
			{
				dropPositions2 = this.FirstAvailablePosition(num);
				if (dropPositions2 == BodyDockPositions.DropPositions.None)
				{
					return dockingResult;
				}
			}
			if (BodyDockPositions.OfflineItemActive(num) == dropPositions2)
			{
				this.TransferrableItemDisable(num);
				dockingResult.positionsDisabled.Add(dropPositions2);
			}
			else
			{
				this.TransferrableItemDisableAtPosition(dropPositions2);
				dockingResult.dockedPosition.Add(dropPositions2);
				TransferrableObject.PositionState positionState = this.MapDropPositionToState(dropPositions2);
				if (this.TransferrableItemActive(num))
				{
					BodyDockPositions.DropPositions item = this.TransferrableItemPosition(num);
					dockingResult.positionsDisabled.Add(item);
					this.MoveTransferableItem(num, dropPositions2, positionState);
				}
				else
				{
					this.EnableTransferrableItem(num, dropPositions2, positionState);
				}
			}
		}
		return dockingResult;
	}

	// Token: 0x060017F1 RID: 6129 RVA: 0x000402C7 File Offset: 0x0003E4C7
	private void MoveTransferableItem(int allItemsIndex, BodyDockPositions.DropPositions newPosition, TransferrableObject.PositionState newPositionState)
	{
		this.allObjects[allItemsIndex].storedZone = newPosition;
		this.allObjects[allItemsIndex].currentState = newPositionState;
		this.allObjects[allItemsIndex].ResetToDefaultState();
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x000CA2F8 File Offset: 0x000C84F8
	public void EnableTransferrableGameObject(int allItemsIndex, BodyDockPositions.DropPositions dropZone, TransferrableObject.PositionState startingPosition)
	{
		GameObject gameObject = this.allObjects[allItemsIndex].gameObject;
		TransferrableObject component = gameObject.GetComponent<TransferrableObject>();
		if ((component.dockPositions & dropZone) == BodyDockPositions.DropPositions.None || !component.ValidateState(startingPosition))
		{
			gameObject.Disable();
			return;
		}
		this.MoveTransferableItem(allItemsIndex, dropZone, startingPosition);
		gameObject.SetActive(true);
		ProjectileWeapon component2;
		if ((component2 = gameObject.GetComponent<ProjectileWeapon>()) != null)
		{
			component2.enabled = true;
		}
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x000CA35C File Offset: 0x000C855C
	public void RefreshTransferrableItems()
	{
		if (!this.myRig)
		{
			this.myRig = base.GetComponentInParent<VRRig>(true);
			if (!this.myRig)
			{
				Debug.LogError("BodyDockPositions.RefreshTransferrableItems: (should never happen) myRig is null and could not be found on same GameObject or parents. Path: " + base.transform.GetPathQ(), this);
			}
		}
		this.objectsToEnable.Clear();
		this.objectsToDisable.Clear();
		for (int i = 0; i < this.myRig.ActiveTransferrableObjectIndexLength(); i++)
		{
			bool flag = true;
			int num = this.myRig.ActiveTransferrableObjectIndex(i);
			if (num != -1)
			{
				if (num < 0 || num >= this.allObjects.Length)
				{
					Debug.LogError(string.Format("Transferrable object index {0} out of range, expected [0..{1})", num, this.allObjects.Length));
				}
				else if (this.myRig.IsItemAllowed(CosmeticsController.instance.GetItemNameFromDisplayName(this.allObjects[num].gameObject.name)))
				{
					for (int j = 0; j < this.allObjects.Length; j++)
					{
						if (j == this.myRig.ActiveTransferrableObjectIndex(i) && this.allObjects[j].gameObject.activeSelf)
						{
							this.allObjects[j].objectIndex = i;
							flag = false;
						}
					}
					if (flag)
					{
						this.objectsToEnable.Add(i);
					}
				}
			}
		}
		for (int k = 0; k < this.allObjects.Length; k++)
		{
			if (this.allObjects[k] != null && this.allObjects[k].gameObject.activeSelf)
			{
				bool flag2 = true;
				for (int l = 0; l < this.myRig.ActiveTransferrableObjectIndexLength(); l++)
				{
					if (this.myRig.ActiveTransferrableObjectIndex(l) == k && this.myRig.IsItemAllowed(CosmeticsController.instance.GetItemNameFromDisplayName(this.allObjects[this.myRig.ActiveTransferrableObjectIndex(l)].gameObject.name)))
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					this.objectsToDisable.Add(k);
				}
			}
		}
		foreach (int index in this.objectsToDisable)
		{
			this.DisableTransferrableItem(index);
		}
		foreach (int idx in this.objectsToEnable)
		{
			this.EnableTransferrableGameObject(this.myRig.ActiveTransferrableObjectIndex(idx), this.myRig.TransferrableDockPosition(idx), this.myRig.TransferrablePosStates(idx));
		}
		this.UpdateHandState();
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x000CA628 File Offset: 0x000C8828
	public int ReturnTransferrableItemIndex(int allItemsIndex)
	{
		for (int i = 0; i < this.myRig.ActiveTransferrableObjectIndexLength(); i++)
		{
			if (this.myRig.ActiveTransferrableObjectIndex(i) == allItemsIndex)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x000CA660 File Offset: 0x000C8860
	public List<int> TransferrableObjectIndexFromName(string transObjectName)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.allObjects.Length; i++)
		{
			if (!(this.allObjects[i] == null) && this.allObjects[i].gameObject.name == transObjectName)
			{
				list.Add(i);
			}
		}
		return list;
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x000402F2 File Offset: 0x0003E4F2
	private TransferrableObject.PositionState MapDropPositionToState(BodyDockPositions.DropPositions pos)
	{
		if (pos == BodyDockPositions.DropPositions.RightArm)
		{
			return TransferrableObject.PositionState.OnRightArm;
		}
		if (pos == BodyDockPositions.DropPositions.LeftArm)
		{
			return TransferrableObject.PositionState.OnLeftArm;
		}
		if (pos == BodyDockPositions.DropPositions.LeftBack)
		{
			return TransferrableObject.PositionState.OnLeftShoulder;
		}
		if (pos == BodyDockPositions.DropPositions.RightBack)
		{
			return TransferrableObject.PositionState.OnRightShoulder;
		}
		return TransferrableObject.PositionState.OnChest;
	}

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x060017F7 RID: 6135 RVA: 0x00040311 File Offset: 0x0003E511
	internal int PreviousLeftHandThrowableIndex
	{
		get
		{
			return this.throwableDisabledIndex[0];
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x060017F8 RID: 6136 RVA: 0x0004031B File Offset: 0x0003E51B
	internal int PreviousRightHandThrowableIndex
	{
		get
		{
			return this.throwableDisabledIndex[1];
		}
	}

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x060017F9 RID: 6137 RVA: 0x00040325 File Offset: 0x0003E525
	internal float PreviousLeftHandThrowableDisabledTime
	{
		get
		{
			return this.throwableDisabledTime[0];
		}
	}

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x060017FA RID: 6138 RVA: 0x0004032F File Offset: 0x0003E52F
	internal float PreviousRightHandThrowableDisabledTime
	{
		get
		{
			return this.throwableDisabledTime[1];
		}
	}

	// Token: 0x060017FB RID: 6139 RVA: 0x000CA6B8 File Offset: 0x000C88B8
	private void UpdateHandState()
	{
		for (int i = 0; i < 2; i++)
		{
			GameObject[] array = (i == 0) ? this.leftHandThrowables : this.rightHandThrowables;
			int num = (i == 0) ? this.myRig.LeftThrowableProjectileIndex : this.myRig.RightThrowableProjectileIndex;
			for (int j = 0; j < array.Length; j++)
			{
				bool activeSelf = array[j].activeSelf;
				bool flag = j == num;
				array[j].SetActive(flag);
				if (activeSelf && !flag)
				{
					this.throwableDisabledIndex[i] = j;
					this.throwableDisabledTime[i] = Time.time + 0.02f;
				}
			}
		}
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x00040339 File Offset: 0x0003E539
	internal GameObject GetLeftHandThrowable()
	{
		return this.GetLeftHandThrowable(this.myRig.LeftThrowableProjectileIndex);
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x0004034C File Offset: 0x0003E54C
	internal GameObject GetLeftHandThrowable(int throwableIndex)
	{
		if (throwableIndex < 0 || throwableIndex >= this.leftHandThrowables.Length)
		{
			throwableIndex = this.PreviousLeftHandThrowableIndex;
			if (throwableIndex < 0 || throwableIndex >= this.leftHandThrowables.Length || this.PreviousLeftHandThrowableDisabledTime < Time.time)
			{
				return null;
			}
		}
		return this.leftHandThrowables[throwableIndex];
	}

	// Token: 0x060017FE RID: 6142 RVA: 0x0004038B File Offset: 0x0003E58B
	internal GameObject GetRightHandThrowable()
	{
		return this.GetRightHandThrowable(this.myRig.RightThrowableProjectileIndex);
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x0004039E File Offset: 0x0003E59E
	internal GameObject GetRightHandThrowable(int throwableIndex)
	{
		if (throwableIndex < 0 || throwableIndex >= this.rightHandThrowables.Length)
		{
			throwableIndex = this.PreviousRightHandThrowableIndex;
			if (throwableIndex < 0 || throwableIndex >= this.rightHandThrowables.Length || this.PreviousRightHandThrowableDisabledTime < Time.time)
			{
				return null;
			}
		}
		return this.rightHandThrowables[throwableIndex];
	}

	// Token: 0x04001A82 RID: 6786
	public VRRig myRig;

	// Token: 0x04001A83 RID: 6787
	public GameObject[] leftHandThrowables;

	// Token: 0x04001A84 RID: 6788
	public GameObject[] rightHandThrowables;

	// Token: 0x04001A85 RID: 6789
	[FormerlySerializedAs("allObjects")]
	public TransferrableObject[] _allObjects;

	// Token: 0x04001A86 RID: 6790
	private List<int> objectsToEnable = new List<int>();

	// Token: 0x04001A87 RID: 6791
	private List<int> objectsToDisable = new List<int>();

	// Token: 0x04001A88 RID: 6792
	public Transform leftHandTransform;

	// Token: 0x04001A89 RID: 6793
	public Transform rightHandTransform;

	// Token: 0x04001A8A RID: 6794
	public Transform chestTransform;

	// Token: 0x04001A8B RID: 6795
	public Transform leftArmTransform;

	// Token: 0x04001A8C RID: 6796
	public Transform rightArmTransform;

	// Token: 0x04001A8D RID: 6797
	public Transform leftBackTransform;

	// Token: 0x04001A8E RID: 6798
	public Transform rightBackTransform;

	// Token: 0x04001A8F RID: 6799
	public WorldShareableItem leftBackSharableItem;

	// Token: 0x04001A90 RID: 6800
	public WorldShareableItem rightBackShareableItem;

	// Token: 0x04001A91 RID: 6801
	public GameObject SharableItemInstance;

	// Token: 0x04001A92 RID: 6802
	private int[] throwableDisabledIndex = new int[]
	{
		-1,
		-1
	};

	// Token: 0x04001A93 RID: 6803
	private float[] throwableDisabledTime = new float[2];

	// Token: 0x020003DF RID: 991
	[Flags]
	public enum DropPositions
	{
		// Token: 0x04001A95 RID: 6805
		LeftArm = 1,
		// Token: 0x04001A96 RID: 6806
		RightArm = 2,
		// Token: 0x04001A97 RID: 6807
		Chest = 4,
		// Token: 0x04001A98 RID: 6808
		LeftBack = 8,
		// Token: 0x04001A99 RID: 6809
		RightBack = 16,
		// Token: 0x04001A9A RID: 6810
		MaxDropPostions = 5,
		// Token: 0x04001A9B RID: 6811
		All = 31,
		// Token: 0x04001A9C RID: 6812
		None = 0
	}

	// Token: 0x020003E0 RID: 992
	public class DockingResult
	{
		// Token: 0x06001801 RID: 6145 RVA: 0x0004041B File Offset: 0x0003E61B
		public DockingResult()
		{
			this.dockedPosition = new List<BodyDockPositions.DropPositions>(2);
			this.positionsDisabled = new List<BodyDockPositions.DropPositions>(2);
		}

		// Token: 0x04001A9D RID: 6813
		public List<BodyDockPositions.DropPositions> positionsDisabled;

		// Token: 0x04001A9E RID: 6814
		public List<BodyDockPositions.DropPositions> dockedPosition;
	}
}
