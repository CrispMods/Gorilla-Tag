using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class GhostLab : MonoBehaviour, IBuildValidation
{
	// Token: 0x06000457 RID: 1111 RVA: 0x00019BF2 File Offset: 0x00017DF2
	private void Awake()
	{
		this.relState = Object.FindFirstObjectByType<GhostLabReliableState>();
		this.doorState = GhostLab.EntranceDoorsState.BothClosed;
		this.doorOpen = new bool[this.slidingDoor.Length];
		this.toggleDoorsParent.GetComponentsInChildren<GhostLabButton>();
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x00019C28 File Offset: 0x00017E28
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

	// Token: 0x06000459 RID: 1113 RVA: 0x00019D30 File Offset: 0x00017F30
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

	// Token: 0x0600045A RID: 1114 RVA: 0x00019D50 File Offset: 0x00017F50
	public void UpdateDoorState(int buttonIndex)
	{
		if ((this.doorOpen[buttonIndex] && this.slidingDoor[buttonIndex].localPosition == this.singleDoorTravelDistance) || (!this.doorOpen[buttonIndex] && this.slidingDoor[buttonIndex].localPosition == Vector3.zero))
		{
			this.doorOpen[buttonIndex] = !this.doorOpen[buttonIndex];
		}
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x00019DB8 File Offset: 0x00017FB8
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

	// Token: 0x0600045C RID: 1116 RVA: 0x00019E64 File Offset: 0x00018064
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

	// Token: 0x0600045D RID: 1117 RVA: 0x00019F64 File Offset: 0x00018164
	private void SynchStates()
	{
		this.doorState = this.relState.doorState;
		for (int i = 0; i < this.doorOpen.Length; i++)
		{
			this.doorOpen[i] = this.relState.singleDoorOpen[i];
		}
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x00019FAC File Offset: 0x000181AC
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

	// Token: 0x04000506 RID: 1286
	public IDCardScanner entranceDoorScanner;

	// Token: 0x04000507 RID: 1287
	public Transform outerDoor;

	// Token: 0x04000508 RID: 1288
	public Transform innerDoor;

	// Token: 0x04000509 RID: 1289
	public Vector3 doorTravelDistance;

	// Token: 0x0400050A RID: 1290
	public float doorMoveSpeed;

	// Token: 0x0400050B RID: 1291
	public float singleDoorMoveSpeed;

	// Token: 0x0400050C RID: 1292
	public GhostLab.EntranceDoorsState doorState;

	// Token: 0x0400050D RID: 1293
	public GhostLabReliableState relState;

	// Token: 0x0400050E RID: 1294
	public Transform toggleDoorsParent;

	// Token: 0x0400050F RID: 1295
	public Transform[] slidingDoor;

	// Token: 0x04000510 RID: 1296
	public Vector3 singleDoorTravelDistance;

	// Token: 0x04000511 RID: 1297
	private bool[] doorOpen;

	// Token: 0x020000AC RID: 172
	public enum EntranceDoorsState
	{
		// Token: 0x04000513 RID: 1299
		BothClosed,
		// Token: 0x04000514 RID: 1300
		InnerDoorOpen,
		// Token: 0x04000515 RID: 1301
		OuterDoorOpen
	}
}
