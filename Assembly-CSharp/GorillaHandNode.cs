using System;
using UnityEngine;

// Token: 0x02000562 RID: 1378
public class GorillaHandNode : MonoBehaviour
{
	// Token: 0x17000375 RID: 885
	// (get) Token: 0x060021FE RID: 8702 RVA: 0x000A84D0 File Offset: 0x000A66D0
	public bool isGripping
	{
		get
		{
			return this.PollGrip();
		}
	}

	// Token: 0x17000376 RID: 886
	// (get) Token: 0x060021FF RID: 8703 RVA: 0x000A84D8 File Offset: 0x000A66D8
	public bool isLeftHand
	{
		get
		{
			return this._isLeftHand;
		}
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06002200 RID: 8704 RVA: 0x000A84E0 File Offset: 0x000A66E0
	public bool isRightHand
	{
		get
		{
			return this._isRightHand;
		}
	}

	// Token: 0x06002201 RID: 8705 RVA: 0x000A84E8 File Offset: 0x000A66E8
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06002202 RID: 8706 RVA: 0x000A84F0 File Offset: 0x000A66F0
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

	// Token: 0x06002203 RID: 8707 RVA: 0x000A8544 File Offset: 0x000A6744
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

	// Token: 0x06002204 RID: 8708 RVA: 0x000023F4 File Offset: 0x000005F4
	private void OnTriggerStay(Collider other)
	{
	}

	// Token: 0x06002205 RID: 8709 RVA: 0x000A8703 File Offset: 0x000A6903
	private float PollIndex()
	{
		return Mathf.Clamp01(this.vrIndex.calcT / 0.88f);
	}

	// Token: 0x06002206 RID: 8710 RVA: 0x000A871B File Offset: 0x000A691B
	private float PollMiddle()
	{
		return this.vrIndex.calcT;
	}

	// Token: 0x06002207 RID: 8711 RVA: 0x000A871B File Offset: 0x000A691B
	private float PollThumb()
	{
		return this.vrIndex.calcT;
	}

	// Token: 0x04002581 RID: 9601
	public VRRig rig;

	// Token: 0x04002582 RID: 9602
	public Collider collider;

	// Token: 0x04002583 RID: 9603
	public Rigidbody rigidbody;

	// Token: 0x04002584 RID: 9604
	[Space]
	[NonSerialized]
	public VRMapIndex vrIndex;

	// Token: 0x04002585 RID: 9605
	[NonSerialized]
	public VRMapThumb vrThumb;

	// Token: 0x04002586 RID: 9606
	[NonSerialized]
	public VRMapMiddle vrMiddle;

	// Token: 0x04002587 RID: 9607
	[Space]
	public GorillaHandSocket attachedToSocket;

	// Token: 0x04002588 RID: 9608
	[Space]
	[SerializeField]
	private bool _isLeftHand;

	// Token: 0x04002589 RID: 9609
	[SerializeField]
	private bool _isRightHand;

	// Token: 0x0400258A RID: 9610
	public bool ignoreSockets;
}
