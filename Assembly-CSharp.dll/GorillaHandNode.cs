using System;
using UnityEngine;

// Token: 0x02000563 RID: 1379
public class GorillaHandNode : MonoBehaviour
{
	// Token: 0x17000376 RID: 886
	// (get) Token: 0x06002206 RID: 8710 RVA: 0x0004622E File Offset: 0x0004442E
	public bool isGripping
	{
		get
		{
			return this.PollGrip();
		}
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06002207 RID: 8711 RVA: 0x00046236 File Offset: 0x00044436
	public bool isLeftHand
	{
		get
		{
			return this._isLeftHand;
		}
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x06002208 RID: 8712 RVA: 0x0004623E File Offset: 0x0004443E
	public bool isRightHand
	{
		get
		{
			return this._isRightHand;
		}
	}

	// Token: 0x06002209 RID: 8713 RVA: 0x00046246 File Offset: 0x00044446
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x0600220A RID: 8714 RVA: 0x000F57F4 File Offset: 0x000F39F4
	private bool PollGrip()
	{
		if (this.rig == null)
		{
			return false;
		}
		bool flag = this.PollThumb() >= 0.25f;
		bool flag2 = this.PollIndex() >= 0.25f;
		bool flag3 = this.PollMiddle() >= 0.25f;
		return flag && flag2 && flag3;
	}

	// Token: 0x0600220B RID: 8715 RVA: 0x000F5848 File Offset: 0x000F3A48
	private void Setup()
	{
		if (this.rig == null)
		{
			this.rig = base.GetComponentInParent<VRRig>();
		}
		if (this.rigidbody == null)
		{
			this.rigidbody = base.GetComponent<Rigidbody>();
		}
		if (this.collider == null)
		{
			this.collider = base.GetComponent<Collider>();
		}
		if (this.rig)
		{
			this.vrIndex = (this._isLeftHand ? this.rig.leftIndex : this.rig.rightIndex);
			this.vrThumb = (this._isLeftHand ? this.rig.leftThumb : this.rig.rightThumb);
			this.vrMiddle = (this._isLeftHand ? this.rig.leftMiddle : this.rig.rightMiddle);
		}
		this._isLeftHand = base.name.Contains("left", StringComparison.OrdinalIgnoreCase);
		this._isRightHand = base.name.Contains("right", StringComparison.OrdinalIgnoreCase);
		int num = 0;
		num |= 1024;
		num |= 2097152;
		num |= 16777216;
		base.gameObject.SetTag(this._isLeftHand ? UnityTag.GorillaHandLeft : UnityTag.GorillaHandRight);
		base.gameObject.SetLayer(UnityLayer.GorillaHand);
		this.rigidbody.includeLayers = num;
		this.rigidbody.excludeLayers = ~num;
		this.rigidbody.isKinematic = true;
		this.rigidbody.useGravity = false;
		this.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		this.collider.isTrigger = true;
		this.collider.includeLayers = num;
		this.collider.excludeLayers = ~num;
	}

	// Token: 0x0600220C RID: 8716 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void OnTriggerStay(Collider other)
	{
	}

	// Token: 0x0600220D RID: 8717 RVA: 0x0004624E File Offset: 0x0004444E
	private float PollIndex()
	{
		return Mathf.Clamp01(this.vrIndex.calcT / 0.88f);
	}

	// Token: 0x0600220E RID: 8718 RVA: 0x00046266 File Offset: 0x00044466
	private float PollMiddle()
	{
		return this.vrIndex.calcT;
	}

	// Token: 0x0600220F RID: 8719 RVA: 0x00046266 File Offset: 0x00044466
	private float PollThumb()
	{
		return this.vrIndex.calcT;
	}

	// Token: 0x04002587 RID: 9607
	public VRRig rig;

	// Token: 0x04002588 RID: 9608
	public Collider collider;

	// Token: 0x04002589 RID: 9609
	public Rigidbody rigidbody;

	// Token: 0x0400258A RID: 9610
	[Space]
	[NonSerialized]
	public VRMapIndex vrIndex;

	// Token: 0x0400258B RID: 9611
	[NonSerialized]
	public VRMapThumb vrThumb;

	// Token: 0x0400258C RID: 9612
	[NonSerialized]
	public VRMapMiddle vrMiddle;

	// Token: 0x0400258D RID: 9613
	[Space]
	public GorillaHandSocket attachedToSocket;

	// Token: 0x0400258E RID: 9614
	[Space]
	[SerializeField]
	private bool _isLeftHand;

	// Token: 0x0400258F RID: 9615
	[SerializeField]
	private bool _isRightHand;

	// Token: 0x04002590 RID: 9616
	public bool ignoreSockets;
}
