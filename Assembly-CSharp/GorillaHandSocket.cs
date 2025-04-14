using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000564 RID: 1380
[DisallowMultipleComponent]
public class GorillaHandSocket : MonoBehaviour
{
	// Token: 0x17000378 RID: 888
	// (get) Token: 0x0600220A RID: 8714 RVA: 0x000A8728 File Offset: 0x000A6928
	public GorillaHandNode attachedHand
	{
		get
		{
			return this._attachedHand;
		}
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x0600220B RID: 8715 RVA: 0x000A8730 File Offset: 0x000A6930
	public bool inUse
	{
		get
		{
			return this._inUse;
		}
	}

	// Token: 0x0600220C RID: 8716 RVA: 0x000A8738 File Offset: 0x000A6938
	public static bool FetchSocket(Collider collider, out GorillaHandSocket socket)
	{
		return GorillaHandSocket.gColliderToSocket.TryGetValue(collider, out socket);
	}

	// Token: 0x0600220D RID: 8717 RVA: 0x000A8746 File Offset: 0x000A6946
	public bool CanAttach()
	{
		return !this._inUse && this._sinceSocketStateChange.HasElapsed(this.attachCooldown, true);
	}

	// Token: 0x0600220E RID: 8718 RVA: 0x000A8764 File Offset: 0x000A6964
	public void Attach(GorillaHandNode hand)
	{
		if (!this.CanAttach())
		{
			return;
		}
		if (hand == null)
		{
			return;
		}
		hand.attachedToSocket = this;
		this._attachedHand = hand;
		this._inUse = true;
		this.OnHandAttach();
	}

	// Token: 0x0600220F RID: 8719 RVA: 0x000A8794 File Offset: 0x000A6994
	public void Detach()
	{
		GorillaHandNode gorillaHandNode;
		this.Detach(out gorillaHandNode);
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x000A87AC File Offset: 0x000A69AC
	public void Detach(out GorillaHandNode hand)
	{
		if (this._inUse)
		{
			this._inUse = false;
		}
		if (this._attachedHand == null)
		{
			hand = null;
			return;
		}
		hand = this._attachedHand;
		hand.attachedToSocket = null;
		this._attachedHand = null;
		this.OnHandDetach();
		this._sinceSocketStateChange = TimeSince.Now();
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnHandAttach()
	{
	}

	// Token: 0x06002212 RID: 8722 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnHandDetach()
	{
	}

	// Token: 0x06002213 RID: 8723 RVA: 0x000A8802 File Offset: 0x000A6A02
	protected virtual void OnUpdateAttached()
	{
		this._attachedHand.transform.position = base.transform.position;
	}

	// Token: 0x06002214 RID: 8724 RVA: 0x000A881F File Offset: 0x000A6A1F
	private void OnEnable()
	{
		if (this.collider == null)
		{
			return;
		}
		GorillaHandSocket.gColliderToSocket.TryAdd(this.collider, this);
	}

	// Token: 0x06002215 RID: 8725 RVA: 0x000A8842 File Offset: 0x000A6A42
	private void OnDisable()
	{
		if (this.collider == null)
		{
			return;
		}
		GorillaHandSocket.gColliderToSocket.Remove(this.collider);
	}

	// Token: 0x06002216 RID: 8726 RVA: 0x000A8864 File Offset: 0x000A6A64
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06002217 RID: 8727 RVA: 0x000A886C File Offset: 0x000A6A6C
	private void FixedUpdate()
	{
		if (!this._inUse)
		{
			return;
		}
		if (!this._attachedHand)
		{
			return;
		}
		this.OnUpdateAttached();
	}

	// Token: 0x06002218 RID: 8728 RVA: 0x000A888C File Offset: 0x000A6A8C
	private void Setup()
	{
		if (this.collider == null)
		{
			this.collider = base.GetComponent<Collider>();
		}
		int num = 0;
		num |= 1024;
		num |= 2097152;
		num |= 16777216;
		base.gameObject.SetTag(UnityTag.GorillaHandSocket);
		base.gameObject.SetLayer(UnityLayer.GorillaHandSocket);
		this.collider.isTrigger = true;
		this.collider.includeLayers = num;
		this.collider.excludeLayers = ~num;
		this._sinceSocketStateChange = TimeSince.Now();
	}

	// Token: 0x0400258B RID: 9611
	public Collider collider;

	// Token: 0x0400258C RID: 9612
	public float attachCooldown = 0.5f;

	// Token: 0x0400258D RID: 9613
	public HandSocketConstraint constraint;

	// Token: 0x0400258E RID: 9614
	[NonSerialized]
	private GorillaHandNode _attachedHand;

	// Token: 0x0400258F RID: 9615
	[NonSerialized]
	private bool _inUse;

	// Token: 0x04002590 RID: 9616
	[NonSerialized]
	private TimeSince _sinceSocketStateChange;

	// Token: 0x04002591 RID: 9617
	private static readonly Dictionary<Collider, GorillaHandSocket> gColliderToSocket = new Dictionary<Collider, GorillaHandSocket>(64);
}
