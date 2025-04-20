using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class GhostLab : MonoBehaviour, IBuildValidation
{
	// Token: 0x06000493 RID: 1171 RVA: 0x00033693 File Offset: 0x00031893
	private void Awake()
	{
		this.relState = UnityEngine.Object.FindFirstObjectByType<GhostLabReliableState>();
		this.doorState = GhostLab.EntranceDoorsState.BothClosed;
		this.doorOpen = new bool[this.slidingDoor.Length];
		this.toggleDoorsParent.GetComponentsInChildren<GhostLabButton>();
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x0007D5FC File Offset: 0x0007B7FC
	public bool BuildValidationCheck()
	{
		if (this.entranceDoorScanner == null)
		{
			Debug.LogError("door scanner missing", base.gameObject);
			return false;
		}
		if (this.outerDoor == null || this.innerDoor == null)
		{
			Debug.LogError("sliding doors missing", base.gameObject);
			return false;
		}
		if (this.toggleDoorsParent == null)
		{
			Debug.LogError("missing reference to parent of toggleable doors", base.gameObject);
			return false;
		}
		List<int> list = new List<int>();
		GhostLabButton[] componentsInChildren = this.toggleDoorsParent.GetComponentsInChildren<GhostLabButton>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!list.Contains(componentsInChildren[i].buttonIndex))
			{
				list.Add(componentsInChildren[i].buttonIndex);
			}
		}
		if (list.Count != this.slidingDoor.Length)
		{
			Debug.LogError("slidingDoor array not set to the correct length", base.gameObject);
			return false;
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (!list.Contains(j))
			{
				Debug.LogError("door indices not continuous", base.gameObject);
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x000336C6 File Offset: 0x000318C6
	public void DoorButtonPress(int buttonIndex, bool forSingleDoor)
	{
		if (!forSingleDoor)
		{
			this.UpdateEntranceDoorsState(buttonIndex);
			return;
		}
		this.UpdateDoorState(buttonIndex);
		this.relState.UpdateSingleDoorState(buttonIndex);
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x0007D704 File Offset: 0x0007B904
	public void UpdateDoorState(int buttonIndex)
	{
		if ((this.doorOpen[buttonIndex] && this.slidingDoor[buttonIndex].localPosition == this.singleDoorTravelDistance) || (!this.doorOpen[buttonIndex] && this.slidingDoor[buttonIndex].localPosition == Vector3.zero))
		{
			this.doorOpen[buttonIndex] = !this.doorOpen[buttonIndex];
		}
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x0007D76C File Offset: 0x0007B96C
	public void UpdateEntranceDoorsState(int buttonIndex)
	{
		if (this.doorState == GhostLab.EntranceDoorsState.BothClosed)
		{
			if (!(this.innerDoor.localPosition != Vector3.zero) && !(this.outerDoor.localPosition != Vector3.zero))
			{
				if (buttonIndex == 0 || buttonIndex == 1)
				{
					this.doorState = GhostLab.EntranceDoorsState.OuterDoorOpen;
				}
				if (buttonIndex == 2 || buttonIndex == 3)
				{
					this.doorState = GhostLab.EntranceDoorsState.InnerDoorOpen;
				}
			}
		}
		else if (this.innerDoor.localPosition == this.doorTravelDistance || this.outerDoor.localPosition == this.doorTravelDistance)
		{
			this.doorState = GhostLab.EntranceDoorsState.BothClosed;
		}
		this.relState.UpdateEntranceDoorsState(this.doorState);
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x0007D818 File Offset: 0x0007BA18
	public void Update()
	{
		this.SynchStates();
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		switch (this.doorState)
		{
		case GhostLab.EntranceDoorsState.InnerDoorOpen:
			zero2 = this.doorTravelDistance;
			break;
		case GhostLab.EntranceDoorsState.OuterDoorOpen:
			zero = this.doorTravelDistance;
			break;
		}
		this.outerDoor.localPosition = Vector3.MoveTowards(this.outerDoor.localPosition, zero, this.doorMoveSpeed * Time.deltaTime);
		this.innerDoor.localPosition = Vector3.MoveTowards(this.innerDoor.localPosition, zero2, this.doorMoveSpeed * Time.deltaTime);
		Vector3 zero3 = Vector3.zero;
		for (int i = 0; i < this.slidingDoor.Length; i++)
		{
			if (this.doorOpen[i])
			{
				zero3 = this.singleDoorTravelDistance;
			}
			else
			{
				zero3 = Vector3.zero;
			}
			this.slidingDoor[i].localPosition = Vector3.MoveTowards(this.slidingDoor[i].localPosition, zero3, this.singleDoorMoveSpeed * Time.deltaTime);
		}
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x0007D918 File Offset: 0x0007BB18
	private void SynchStates()
	{
		this.doorState = this.relState.doorState;
		for (int i = 0; i < this.doorOpen.Length; i++)
		{
			this.doorOpen[i] = this.relState.singleDoorOpen[i];
		}
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x0007D960 File Offset: 0x0007BB60
	public bool IsDoorMoving(bool singleDoor, int index)
	{
		if (singleDoor)
		{
			return (this.doorOpen[index] && this.slidingDoor[index].localPosition != this.singleDoorTravelDistance) || (!this.doorOpen[index] && this.slidingDoor[index].localPosition != Vector3.zero);
		}
		if (index == 0 || index == 1)
		{
			return (this.doorState == GhostLab.EntranceDoorsState.OuterDoorOpen && this.outerDoor.localPosition != this.doorTravelDistance) || (this.doorState != GhostLab.EntranceDoorsState.OuterDoorOpen && this.outerDoor.localPosition != Vector3.zero);
		}
		return (this.doorState == GhostLab.EntranceDoorsState.InnerDoorOpen && this.innerDoor.localPosition != this.doorTravelDistance) || (this.doorState != GhostLab.EntranceDoorsState.InnerDoorOpen && this.innerDoor.localPosition != Vector3.zero);
	}

	// Token: 0x04000546 RID: 1350
	public IDCardScanner entranceDoorScanner;

	// Token: 0x04000547 RID: 1351
	public Transform outerDoor;

	// Token: 0x04000548 RID: 1352
	public Transform innerDoor;

	// Token: 0x04000549 RID: 1353
	public Vector3 doorTravelDistance;

	// Token: 0x0400054A RID: 1354
	public float doorMoveSpeed;

	// Token: 0x0400054B RID: 1355
	public float singleDoorMoveSpeed;

	// Token: 0x0400054C RID: 1356
	public GhostLab.EntranceDoorsState doorState;

	// Token: 0x0400054D RID: 1357
	public GhostLabReliableState relState;

	// Token: 0x0400054E RID: 1358
	public Transform toggleDoorsParent;

	// Token: 0x0400054F RID: 1359
	public Transform[] slidingDoor;

	// Token: 0x04000550 RID: 1360
	public Vector3 singleDoorTravelDistance;

	// Token: 0x04000551 RID: 1361
	private bool[] doorOpen;

	// Token: 0x020000B6 RID: 182
	public enum EntranceDoorsState
	{
		// Token: 0x04000553 RID: 1363
		BothClosed,
		// Token: 0x04000554 RID: 1364
		InnerDoorOpen,
		// Token: 0x04000555 RID: 1365
		OuterDoorOpen
	}
}
