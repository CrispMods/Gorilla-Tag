using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000565 RID: 1381
[DisallowMultipleComponent]
public class GorillaHandSocket : MonoBehaviour
{
	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06002212 RID: 8722 RVA: 0x00046273 File Offset: 0x00044473
	public GorillaHandNode attachedHand
	{
		get
		{
			return this._attachedHand;
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06002213 RID: 8723 RVA: 0x0004627B File Offset: 0x0004447B
	public bool inUse
	{
		get
		{
			return this._inUse;
		}
	}

	// Token: 0x06002214 RID: 8724 RVA: 0x00046283 File Offset: 0x00044483
	public static bool FetchSocket(Collider collider, out GorillaHandSocket socket)
	{
		return GorillaHandSocket.gColliderToSocket.TryGetValue(collider, out socket);
	}

	// Token: 0x06002215 RID: 8725 RVA: 0x00046291 File Offset: 0x00044491
	public bool CanAttach()
	{
		return !this._inUse && this._sinceSocketStateChange.HasElapsed(this.attachCooldown, true);
	}

	// Token: 0x06002216 RID: 8726 RVA: 0x000462AF File Offset: 0x000444AF
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

	// Token: 0x06002217 RID: 8727 RVA: 0x000F5A08 File Offset: 0x000F3C08
	public void Detach()
	{
		GorillaHandNode gorillaHandNode;
		this.Detach(out gorillaHandNode);
	}

	// Token: 0x06002218 RID: 8728 RVA: 0x000F5A20 File Offset: 0x000F3C20
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

	// Token: 0x06002219 RID: 8729 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnHandAttach()
	{
	}

	// Token: 0x0600221A RID: 8730 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnHandDetach()
	{
	}

	// Token: 0x0600221B RID: 8731 RVA: 0x000462DF File Offset: 0x000444DF
	protected virtual void OnUpdateAttached()
	{
		this._attachedHand.transform.position = base.transform.position;
	}

	// Token: 0x0600221C RID: 8732 RVA: 0x000462FC File Offset: 0x000444FC
	private void OnEnable()
	{
		if (this.collider == null)
		{
			return;
		}
		GorillaHandSocket.gColliderToSocket.TryAdd(this.collider, this);
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x0004631F File Offset: 0x0004451F
	private void OnDisable()
	{
		if (this.collider == null)
		{
			return;
		}
		GorillaHandSocket.gColliderToSocket.Remove(this.collider);
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x00046341 File Offset: 0x00044541
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x00046349 File Offset: 0x00044549
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

	// Token: 0x06002220 RID: 8736 RVA: 0x000F5A78 File Offset: 0x000F3C78
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

	// Token: 0x04002591 RID: 9617
	public Collider collider;

	// Token: 0x04002592 RID: 9618
	public float attachCooldown = 0.5f;

	// Token: 0x04002593 RID: 9619
	public HandSocketConstraint constraint;

	// Token: 0x04002594 RID: 9620
	[NonSerialized]
	private GorillaHandNode _attachedHand;

	// Token: 0x04002595 RID: 9621
	[NonSerialized]
	private bool _inUse;

	// Token: 0x04002596 RID: 9622
	[NonSerialized]
	private TimeSince _sinceSocketStateChange;

	// Token: 0x04002597 RID: 9623
	private static readonly Dictionary<Collider, GorillaHandSocket> gColliderToSocket = new Dictionary<Collider, GorillaHandSocket>(64);
}
