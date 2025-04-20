using System;
using UnityEngine;

// Token: 0x02000570 RID: 1392
public class GorillaHandNode : MonoBehaviour
{
	// Token: 0x1700037D RID: 893
	// (get) Token: 0x0600225C RID: 8796 RVA: 0x000475D3 File Offset: 0x000457D3
	public bool isGripping
	{
		get
		{
			return this.PollGrip();
		}
	}

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x0600225D RID: 8797 RVA: 0x000475DB File Offset: 0x000457DB
	public bool isLeftHand
	{
		get
		{
			return this._isLeftHand;
		}
	}

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x0600225E RID: 8798 RVA: 0x000475E3 File Offset: 0x000457E3
	public bool isRightHand
	{
		get
		{
			return this._isRightHand;
		}
	}

	// Token: 0x0600225F RID: 8799 RVA: 0x000475EB File Offset: 0x000457EB
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06002260 RID: 8800 RVA: 0x000F8570 File Offset: 0x000F6770
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

	// Token: 0x06002261 RID: 8801 RVA: 0x000F85C4 File Offset: 0x000F67C4
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

	// Token: 0x06002262 RID: 8802 RVA: 0x00030607 File Offset: 0x0002E807
	private void OnTriggerStay(Collider other)
	{
	}

	// Token: 0x06002263 RID: 8803 RVA: 0x000475F3 File Offset: 0x000457F3
	private float PollIndex()
	{
		return Mathf.Clamp01(this.vrIndex.calcT / 0.88f);
	}

	// Token: 0x06002264 RID: 8804 RVA: 0x0004760B File Offset: 0x0004580B
	private float PollMiddle()
	{
		return this.vrIndex.calcT;
	}

	// Token: 0x06002265 RID: 8805 RVA: 0x0004760B File Offset: 0x0004580B
	private float PollThumb()
	{
		return this.vrIndex.calcT;
	}

	// Token: 0x040025D9 RID: 9689
	public VRRig rig;

	// Token: 0x040025DA RID: 9690
	public Collider collider;

	// Token: 0x040025DB RID: 9691
	public Rigidbody rigidbody;

	// Token: 0x040025DC RID: 9692
	[Space]
	[NonSerialized]
	public VRMapIndex vrIndex;

	// Token: 0x040025DD RID: 9693
	[NonSerialized]
	public VRMapThumb vrThumb;

	// Token: 0x040025DE RID: 9694
	[NonSerialized]
	public VRMapMiddle vrMiddle;

	// Token: 0x040025DF RID: 9695
	[Space]
	public GorillaHandSocket attachedToSocket;

	// Token: 0x040025E0 RID: 9696
	[Space]
	[SerializeField]
	private bool _isLeftHand;

	// Token: 0x040025E1 RID: 9697
	[SerializeField]
	private bool _isRightHand;

	// Token: 0x040025E2 RID: 9698
	public bool ignoreSockets;
}
