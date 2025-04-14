using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007D9 RID: 2009
public class RigOwnedPhysicsBody : MonoBehaviour
{
	// Token: 0x060031CD RID: 12749 RVA: 0x000EFA60 File Offset: 0x000EDC60
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

	// Token: 0x060031CE RID: 12750 RVA: 0x000EFAF0 File Offset: 0x000EDCF0
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

	// Token: 0x060031CF RID: 12751 RVA: 0x000EFBB4 File Offset: 0x000EDDB4
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

	// Token: 0x060031D0 RID: 12752 RVA: 0x000EFC30 File Offset: 0x000EDE30
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

	// Token: 0x060031D1 RID: 12753 RVA: 0x000EFD48 File Offset: 0x000EDF48
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

	// Token: 0x0400355D RID: 13661
	private VRRig rig;

	// Token: 0x0400355E RID: 13662
	public RigOwnedTransformView transformView;

	// Token: 0x0400355F RID: 13663
	private bool hasTransformView;

	// Token: 0x04003560 RID: 13664
	public RigOwnedRigidbodyView rigidbodyView;

	// Token: 0x04003561 RID: 13665
	private bool hasRigidbodyView;

	// Token: 0x04003562 RID: 13666
	public MonoBehaviourPun[] otherComponents;

	// Token: 0x04003563 RID: 13667
	private bool hasRig;

	// Token: 0x04003564 RID: 13668
	[Tooltip("To make a rigidbody unaffected by the movement of the holdable part, put this script on the holdable, make the RigOwnedRigidbodyView a child of it, and check this box")]
	[SerializeField]
	private bool detachTransform;
}
