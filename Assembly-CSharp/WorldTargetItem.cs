using System;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x0200039D RID: 925
public class WorldTargetItem
{
	// Token: 0x06001588 RID: 5512 RVA: 0x00068FD7 File Offset: 0x000671D7
	public bool IsValid()
	{
		return this.itemIdx != -1 && this.owner != null;
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x00068FF0 File Offset: 0x000671F0
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

	// Token: 0x0600158A RID: 5514 RVA: 0x00069038 File Offset: 0x00067238
	public static WorldTargetItem GenerateTargetFromWorldSharableItem(NetPlayer owner, int itemIdx, Transform transform)
	{
		return new WorldTargetItem(owner, itemIdx, transform);
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x00069042 File Offset: 0x00067242
	private WorldTargetItem(NetPlayer owner, int itemIdx, Transform transform)
	{
		this.owner = owner;
		this.itemIdx = itemIdx;
		this.targetObject = transform;
		this.transferrableObject = transform.GetComponent<TransferrableObject>();
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x0006906B File Offset: 0x0006726B
	public override string ToString()
	{
		return string.Format("Id: {0} ({1})", this.itemIdx, this.owner);
	}

	// Token: 0x040017DF RID: 6111
	public readonly NetPlayer owner;

	// Token: 0x040017E0 RID: 6112
	public readonly int itemIdx;

	// Token: 0x040017E1 RID: 6113
	public readonly Transform targetObject;

	// Token: 0x040017E2 RID: 6114
	public readonly TransferrableObject transferrableObject;
}
