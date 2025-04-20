using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000572 RID: 1394
[DisallowMultipleComponent]
public class GorillaHandSocket : MonoBehaviour
{
	// Token: 0x17000380 RID: 896
	// (get) Token: 0x06002268 RID: 8808 RVA: 0x00047618 File Offset: 0x00045818
	public GorillaHandNode attachedHand
	{
		get
		{
			return this._attachedHand;
		}
	}

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06002269 RID: 8809 RVA: 0x00047620 File Offset: 0x00045820
	public bool inUse
	{
		get
		{
			return this._inUse;
		}
	}

	// Token: 0x0600226A RID: 8810 RVA: 0x00047628 File Offset: 0x00045828
	public static bool FetchSocket(Collider collider, out GorillaHandSocket socket)
	{
		return GorillaHandSocket.gColliderToSocket.TryGetValue(collider, out socket);
	}

	// Token: 0x0600226B RID: 8811 RVA: 0x00047636 File Offset: 0x00045836
	public bool CanAttach()
	{
		return !this._inUse && this._sinceSocketStateChange.HasElapsed(this.attachCooldown, true);
	}

	// Token: 0x0600226C RID: 8812 RVA: 0x00047654 File Offset: 0x00045854
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

	// Token: 0x0600226D RID: 8813 RVA: 0x000F8784 File Offset: 0x000F6984
	public void Detach()
	{
		GorillaHandNode gorillaHandNode;
		this.Detach(out gorillaHandNode);
	}

	// Token: 0x0600226E RID: 8814 RVA: 0x000F879C File Offset: 0x000F699C
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

	// Token: 0x0600226F RID: 8815 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnHandAttach()
	{
	}

	// Token: 0x06002270 RID: 8816 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnHandDetach()
	{
	}

	// Token: 0x06002271 RID: 8817 RVA: 0x00047684 File Offset: 0x00045884
	protected virtual void OnUpdateAttached()
	{
		this._attachedHand.transform.position = base.transform.position;
	}

	// Token: 0x06002272 RID: 8818 RVA: 0x000476A1 File Offset: 0x000458A1
	private void OnEnable()
	{
		if (this.collider == null)
		{
			return;
		}
		GorillaHandSocket.gColliderToSocket.TryAdd(this.collider, this);
	}

	// Token: 0x06002273 RID: 8819 RVA: 0x000476C4 File Offset: 0x000458C4
	private void OnDisable()
	{
		if (this.collider == null)
		{
			return;
		}
		GorillaHandSocket.gColliderToSocket.Remove(this.collider);
	}

	// Token: 0x06002274 RID: 8820 RVA: 0x000476E6 File Offset: 0x000458E6
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06002275 RID: 8821 RVA: 0x000476EE File Offset: 0x000458EE
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

	// Token: 0x06002276 RID: 8822 RVA: 0x000F87F4 File Offset: 0x000F69F4
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

	// Token: 0x040025E3 RID: 9699
	public Collider collider;

	// Token: 0x040025E4 RID: 9700
	public float attachCooldown = 0.5f;

	// Token: 0x040025E5 RID: 9701
	public HandSocketConstraint constraint;

	// Token: 0x040025E6 RID: 9702
	[NonSerialized]
	private GorillaHandNode _attachedHand;

	// Token: 0x040025E7 RID: 9703
	[NonSerialized]
	private bool _inUse;

	// Token: 0x040025E8 RID: 9704
	[NonSerialized]
	private TimeSince _sinceSocketStateChange;

	// Token: 0x040025E9 RID: 9705
	private static readonly Dictionary<Collider, GorillaHandSocket> gColliderToSocket = new Dictionary<Collider, GorillaHandSocket>(64);
}
