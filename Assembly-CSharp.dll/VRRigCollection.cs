using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200089C RID: 2204
[RequireComponent(typeof(CompositeTriggerEvents))]
public class VRRigCollection : MonoBehaviour
{
	// Token: 0x1700056B RID: 1387
	// (get) Token: 0x06003553 RID: 13651 RVA: 0x00052442 File Offset: 0x00050642
	public List<RigContainer> Rigs
	{
		get
		{
			return this.containedRigs;
		}
	}

	// Token: 0x06003554 RID: 13652 RVA: 0x0005244A File Offset: 0x0005064A
	private void OnEnable()
	{
		this.collisionTriggerEvents.CompositeTriggerEnter += this.OnRigTriggerEnter;
		this.collisionTriggerEvents.CompositeTriggerExit += this.OnRigTriggerExit;
	}

	// Token: 0x06003555 RID: 13653 RVA: 0x0013EAFC File Offset: 0x0013CCFC
	private void OnDisable()
	{
		for (int i = this.containedRigs.Count - 1; i >= 0; i--)
		{
			this.RigDisabled(this.containedRigs[i]);
		}
		this.collisionTriggerEvents.CompositeTriggerEnter -= this.OnRigTriggerEnter;
		this.collisionTriggerEvents.CompositeTriggerExit -= this.OnRigTriggerExit;
	}

	// Token: 0x06003556 RID: 13654 RVA: 0x0013EB64 File Offset: 0x0013CD64
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

	// Token: 0x06003557 RID: 13655 RVA: 0x0013EBEC File Offset: 0x0013CDEC
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

	// Token: 0x06003558 RID: 13656 RVA: 0x0005247A File Offset: 0x0005067A
	private void RigDisabled(RigContainer rig)
	{
		this.collisionTriggerEvents.ResetColliderMask(rig.HeadCollider);
		this.collisionTriggerEvents.ResetColliderMask(rig.BodyCollider);
	}

	// Token: 0x06003559 RID: 13657 RVA: 0x0013EC78 File Offset: 0x0013CE78
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

	// Token: 0x0600355A RID: 13658 RVA: 0x0013ECB8 File Offset: 0x0013CEB8
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

	// Token: 0x040037C1 RID: 14273
	public readonly List<RigContainer> containedRigs = new List<RigContainer>(10);

	// Token: 0x040037C2 RID: 14274
	[SerializeField]
	private CompositeTriggerEvents collisionTriggerEvents;

	// Token: 0x040037C3 RID: 14275
	public Action<RigContainer> playerEnteredCollection;

	// Token: 0x040037C4 RID: 14276
	public Action<RigContainer> playerLeftCollection;
}
