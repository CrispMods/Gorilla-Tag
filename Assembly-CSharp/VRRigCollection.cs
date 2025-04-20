using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008B5 RID: 2229
[RequireComponent(typeof(CompositeTriggerEvents))]
public class VRRigCollection : MonoBehaviour
{
	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x06003614 RID: 13844 RVA: 0x00053951 File Offset: 0x00051B51
	public List<RigContainer> Rigs
	{
		get
		{
			return this.containedRigs;
		}
	}

	// Token: 0x06003615 RID: 13845 RVA: 0x00053959 File Offset: 0x00051B59
	private void OnEnable()
	{
		this.collisionTriggerEvents.CompositeTriggerEnter += this.OnRigTriggerEnter;
		this.collisionTriggerEvents.CompositeTriggerExit += this.OnRigTriggerExit;
	}

	// Token: 0x06003616 RID: 13846 RVA: 0x00144144 File Offset: 0x00142344
	private void OnDisable()
	{
		for (int i = this.containedRigs.Count - 1; i >= 0; i--)
		{
			this.RigDisabled(this.containedRigs[i]);
		}
		this.collisionTriggerEvents.CompositeTriggerEnter -= this.OnRigTriggerEnter;
		this.collisionTriggerEvents.CompositeTriggerExit -= this.OnRigTriggerExit;
	}

	// Token: 0x06003617 RID: 13847 RVA: 0x001441AC File Offset: 0x001423AC
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

	// Token: 0x06003618 RID: 13848 RVA: 0x00144234 File Offset: 0x00142434
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

	// Token: 0x06003619 RID: 13849 RVA: 0x00053989 File Offset: 0x00051B89
	private void RigDisabled(RigContainer rig)
	{
		this.collisionTriggerEvents.ResetColliderMask(rig.HeadCollider);
		this.collisionTriggerEvents.ResetColliderMask(rig.BodyCollider);
	}

	// Token: 0x0600361A RID: 13850 RVA: 0x001442C0 File Offset: 0x001424C0
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

	// Token: 0x0600361B RID: 13851 RVA: 0x00144300 File Offset: 0x00142500
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

	// Token: 0x0400386F RID: 14447
	public readonly List<RigContainer> containedRigs = new List<RigContainer>(10);

	// Token: 0x04003870 RID: 14448
	[SerializeField]
	private CompositeTriggerEvents collisionTriggerEvents;

	// Token: 0x04003871 RID: 14449
	public Action<RigContainer> playerEnteredCollection;

	// Token: 0x04003872 RID: 14450
	public Action<RigContainer> playerLeftCollection;
}
