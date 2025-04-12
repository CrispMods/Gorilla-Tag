using System;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x0200039D RID: 925
public class WorldTargetItem
{
	// Token: 0x0600158B RID: 5515 RVA: 0x0003D89A File Offset: 0x0003BA9A
	public bool IsValid()
	{
		return this.itemIdx != -1 && this.owner != null;
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x000BECF0 File Offset: 0x000BCEF0
	[CanBeNull]
	public static WorldTargetItem GenerateTargetFromPlayerAndID(NetPlayer owner, int itemIdx)
	{
		VRRig vrrig = GorillaGameManager.StaticFindRigForPlayer(owner);
		if (vrrig == null)
		{
			Debug.LogError("Tried to setup a sharable object but the target rig is null...");
			return null;
		}
		Transform component = vrrig.myBodyDockPositions.TransferrableItem(itemIdx).gameObject.GetComponent<Transform>();
		return new WorldTargetItem(owner, itemIdx, component);
	}

	// Token: 0x0600158D RID: 5517 RVA: 0x0003D8B0 File Offset: 0x0003BAB0
	public static WorldTargetItem GenerateTargetFromWorldSharableItem(NetPlayer owner, int itemIdx, Transform transform)
	{
		return new WorldTargetItem(owner, itemIdx, transform);
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x0003D8BA File Offset: 0x0003BABA
	private WorldTargetItem(NetPlayer owner, int itemIdx, Transform transform)
	{
		this.owner = owner;
		this.itemIdx = itemIdx;
		this.targetObject = transform;
		this.transferrableObject = transform.GetComponent<TransferrableObject>();
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x0003D8E3 File Offset: 0x0003BAE3
	public override string ToString()
	{
		return string.Format("Id: {0} ({1})", this.itemIdx, this.owner);
	}

	// Token: 0x040017E0 RID: 6112
	public readonly NetPlayer owner;

	// Token: 0x040017E1 RID: 6113
	public readonly int itemIdx;

	// Token: 0x040017E2 RID: 6114
	public readonly Transform targetObject;

	// Token: 0x040017E3 RID: 6115
	public readonly TransferrableObject transferrableObject;
}
