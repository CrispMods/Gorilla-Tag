using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000899 RID: 2201
[RequireComponent(typeof(CompositeTriggerEvents))]
public class VRRigCollection : MonoBehaviour
{
	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x06003547 RID: 13639 RVA: 0x000FDCCE File Offset: 0x000FBECE
	public List<RigContainer> Rigs
	{
		get
		{
			return this.containedRigs;
		}
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x000FDCD6 File Offset: 0x000FBED6
	private void OnEnable()
	{
		this.collisionTriggerEvents.CompositeTriggerEnter += this.OnRigTriggerEnter;
		this.collisionTriggerEvents.CompositeTriggerExit += this.OnRigTriggerExit;
	}

	// Token: 0x06003549 RID: 13641 RVA: 0x000FDD08 File Offset: 0x000FBF08
	private void OnDisable()
	{
		for (int i = this.containedRigs.Count - 1; i >= 0; i--)
		{
			this.RigDisabled(this.containedRigs[i]);
		}
		this.collisionTriggerEvents.CompositeTriggerEnter -= this.OnRigTriggerEnter;
		this.collisionTriggerEvents.CompositeTriggerExit -= this.OnRigTriggerExit;
	}

	// Token: 0x0600354A RID: 13642 RVA: 0x000FDD70 File Offset: 0x000FBF70
	private void OnRigTriggerEnter(Collider other)
	{
		Rigidbody attachedRigidbody = other.attachedRigidbody;
		RigContainer rigContainer;
		if (attachedRigidbody == null || !attachedRigidbody.TryGetComponent<RigContainer>(out rigContainer) || other != rigContainer.HeadCollider || this.containedRigs.Contains(rigContainer))
		{
			return;
		}
		VRRigEvents rigEvents = rigContainer.RigEvents;
		rigEvents.disableEvent = (Action<RigContainer>)Delegate.Combine(rigEvents.disableEvent, new Action<RigContainer>(this.RigDisabled));
		this.containedRigs.Add(rigContainer);
		Action<RigContainer> action = this.playerEnteredCollection;
		if (action == null)
		{
			return;
		}
		action(rigContainer);
	}

	// Token: 0x0600354B RID: 13643 RVA: 0x000FDDF8 File Offset: 0x000FBFF8
	private void OnRigTriggerExit(Collider other)
	{
		Rigidbody attachedRigidbody = other.attachedRigidbody;
		RigContainer rigContainer;
		if (attachedRigidbody == null || !attachedRigidbody.TryGetComponent<RigContainer>(out rigContainer) || other != rigContainer.HeadCollider || !this.containedRigs.Contains(rigContainer))
		{
			return;
		}
		VRRigEvents rigEvents = rigContainer.RigEvents;
		rigEvents.disableEvent = (Action<RigContainer>)Delegate.Remove(rigEvents.disableEvent, new Action<RigContainer>(this.RigDisabled));
		this.containedRigs.Remove(rigContainer);
		Action<RigContainer> action = this.playerLeftCollection;
		if (action == null)
		{
			return;
		}
		action(rigContainer);
	}

	// Token: 0x0600354C RID: 13644 RVA: 0x000FDE81 File Offset: 0x000FC081
	private void RigDisabled(RigContainer rig)
	{
		this.collisionTriggerEvents.ResetColliderMask(rig.HeadCollider);
		this.collisionTriggerEvents.ResetColliderMask(rig.BodyCollider);
	}

	// Token: 0x0600354D RID: 13645 RVA: 0x000FDEA8 File Offset: 0x000FC0A8
	private bool HasRig(VRRig rig)
	{
		for (int i = 0; i < this.containedRigs.Count; i++)
		{
			if (this.containedRigs[i].Rig == rig)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600354E RID: 13646 RVA: 0x000FDEE8 File Offset: 0x000FC0E8
	private bool HasRig(NetPlayer player)
	{
		for (int i = 0; i < this.containedRigs.Count; i++)
		{
			if (this.containedRigs[i].Creator == player)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040037AF RID: 14255
	public readonly List<RigContainer> containedRigs = new List<RigContainer>(10);

	// Token: 0x040037B0 RID: 14256
	[SerializeField]
	private CompositeTriggerEvents collisionTriggerEvents;

	// Token: 0x040037B1 RID: 14257
	public Action<RigContainer> playerEnteredCollection;

	// Token: 0x040037B2 RID: 14258
	public Action<RigContainer> playerLeftCollection;
}
