using System;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x020003A8 RID: 936
public class WorldTargetItem
{
	// Token: 0x060015D4 RID: 5588 RVA: 0x0003EB5A File Offset: 0x0003CD5A
	public bool IsValid()
	{
		return this.itemIdx != -1 && this.owner != null;
	}

	// Token: 0x060015D5 RID: 5589 RVA: 0x000C1518 File Offset: 0x000BF718
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

	// Token: 0x060015D6 RID: 5590 RVA: 0x0003EB70 File Offset: 0x0003CD70
	public static WorldTargetItem GenerateTargetFromWorldSharableItem(NetPlayer owner, int itemIdx, Transform transform)
	{
		return new WorldTargetItem(owner, itemIdx, transform);
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x0003EB7A File Offset: 0x0003CD7A
	private WorldTargetItem(NetPlayer owner, int itemIdx, Transform transform)
	{
		this.owner = owner;
		this.itemIdx = itemIdx;
		this.targetObject = transform;
		this.transferrableObject = transform.GetComponent<TransferrableObject>();
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x0003EBA3 File Offset: 0x0003CDA3
	public override string ToString()
	{
		return string.Format("Id: {0} ({1})", this.itemIdx, this.owner);
	}

	// Token: 0x04001826 RID: 6182
	public readonly NetPlayer owner;

	// Token: 0x04001827 RID: 6183
	public readonly int itemIdx;

	// Token: 0x04001828 RID: 6184
	public readonly Transform targetObject;

	// Token: 0x04001829 RID: 6185
	public readonly TransferrableObject transferrableObject;
}
