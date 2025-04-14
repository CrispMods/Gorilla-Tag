using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007DA RID: 2010
public class RigOwnedPhysicsBody : MonoBehaviour
{
	// Token: 0x060031D5 RID: 12757 RVA: 0x000EFEE0 File Offset: 0x000EE0E0
	private void Awake()
	{
		this.hasTransformView = (this.transformView != null);
		this.hasRigidbodyView = (this.rigidbodyView != null);
		if (!this.hasTransformView && !this.hasRigidbodyView && this.otherComponents.Length == 0)
		{
			GTDev.LogError<string>("RigOwnedPhysicsBody has nothing to do! No TransformView, RigidbodyView, or otherComponents", null);
		}
		if (this.detachTransform)
		{
			if (this.hasTransformView)
			{
				this.transformView.transform.parent = null;
				return;
			}
			if (this.hasRigidbodyView)
			{
				this.rigidbodyView.transform.parent = null;
			}
		}
	}

	// Token: 0x060031D6 RID: 12758 RVA: 0x000EFF70 File Offset: 0x000EE170
	private void OnEnable()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		NetworkSystem.Instance.OnJoinedRoomEvent += this.OnNetConnect;
		NetworkSystem.Instance.OnReturnedToSinglePlayer += this.OnNetDisconnect;
		if (!this.hasRig)
		{
			this.rig = base.GetComponentInParent<VRRig>();
			this.hasRig = (this.rig != null);
		}
		if (this.detachTransform)
		{
			if (this.hasTransformView)
			{
				this.transformView.gameObject.SetActive(true);
			}
			else if (this.hasRigidbodyView)
			{
				this.rigidbodyView.gameObject.SetActive(true);
			}
		}
		if (NetworkSystem.Instance.InRoom)
		{
			this.OnNetConnect();
			return;
		}
		this.OnNetDisconnect();
	}

	// Token: 0x060031D7 RID: 12759 RVA: 0x000F0034 File Offset: 0x000EE234
	private void OnDisable()
	{
		NetworkSystem.Instance.OnJoinedRoomEvent -= this.OnNetConnect;
		NetworkSystem.Instance.OnReturnedToSinglePlayer -= this.OnNetDisconnect;
		if (this.detachTransform)
		{
			if (this.hasTransformView)
			{
				this.transformView.gameObject.SetActive(false);
			}
			else if (this.hasRigidbodyView)
			{
				this.rigidbodyView.gameObject.SetActive(false);
			}
		}
		this.OnNetDisconnect();
	}

	// Token: 0x060031D8 RID: 12760 RVA: 0x000F00B0 File Offset: 0x000EE2B0
	private void OnNetConnect()
	{
		if (this.hasTransformView)
		{
			this.transformView.enabled = this.hasRig;
		}
		if (this.hasRigidbodyView)
		{
			this.rigidbodyView.enabled = this.hasRig;
		}
		MonoBehaviourPun[] array = this.otherComponents;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = this.hasRig;
		}
		if (!this.hasRig)
		{
			return;
		}
		PhotonView getView = this.rig.netView.GetView;
		List<Component> observedComponents = getView.ObservedComponents;
		if (this.hasTransformView)
		{
			this.transformView.SetIsMine(getView.IsMine);
			if (!observedComponents.Contains(this.transformView))
			{
				observedComponents.Add(this.transformView);
			}
		}
		if (this.hasRigidbodyView)
		{
			this.rigidbodyView.SetIsMine(getView.IsMine);
			if (!observedComponents.Contains(this.rigidbodyView))
			{
				observedComponents.Add(this.rigidbodyView);
			}
		}
		foreach (MonoBehaviourPun item in this.otherComponents)
		{
			if (!observedComponents.Contains(item))
			{
				observedComponents.Add(item);
			}
		}
	}

	// Token: 0x060031D9 RID: 12761 RVA: 0x000F01C8 File Offset: 0x000EE3C8
	private void OnNetDisconnect()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		if (this.hasTransformView)
		{
			this.transformView.enabled = false;
		}
		if (this.hasRigidbodyView)
		{
			this.rigidbodyView.enabled = false;
		}
		MonoBehaviourPun[] array = this.otherComponents;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		if (!this.hasRig || !NetworkSystem.Instance.InRoom)
		{
			return;
		}
		List<Component> observedComponents = this.rig.netView.GetView.ObservedComponents;
		if (this.hasTransformView)
		{
			observedComponents.Remove(this.transformView);
		}
		if (this.hasRigidbodyView)
		{
			observedComponents.Remove(this.rigidbodyView);
		}
		foreach (MonoBehaviourPun item in this.otherComponents)
		{
			observedComponents.Remove(item);
		}
	}

	// Token: 0x04003563 RID: 13667
	private VRRig rig;

	// Token: 0x04003564 RID: 13668
	public RigOwnedTransformView transformView;

	// Token: 0x04003565 RID: 13669
	private bool hasTransformView;

	// Token: 0x04003566 RID: 13670
	public RigOwnedRigidbodyView rigidbodyView;

	// Token: 0x04003567 RID: 13671
	private bool hasRigidbodyView;

	// Token: 0x04003568 RID: 13672
	public MonoBehaviourPun[] otherComponents;

	// Token: 0x04003569 RID: 13673
	private bool hasRig;

	// Token: 0x0400356A RID: 13674
	[Tooltip("To make a rigidbody unaffected by the movement of the holdable part, put this script on the holdable, make the RigOwnedRigidbodyView a child of it, and check this box")]
	[SerializeField]
	private bool detachTransform;
}
