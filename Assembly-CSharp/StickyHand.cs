using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class StickyHand : MonoBehaviour, ISpawnable
{
	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x060009C3 RID: 2499 RVA: 0x00036E73 File Offset: 0x00035073
	// (set) Token: 0x060009C4 RID: 2500 RVA: 0x00036E7B File Offset: 0x0003507B
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x060009C5 RID: 2501 RVA: 0x00036E84 File Offset: 0x00035084
	// (set) Token: 0x060009C6 RID: 2502 RVA: 0x00036E8C File Offset: 0x0003508C
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x060009C7 RID: 2503 RVA: 0x00092AE4 File Offset: 0x00090CE4
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
		this.isLocal = rig.isLocal;
		this.flatHand.enabled = false;
		this.defaultLocalPosition = this.stringParent.transform.InverseTransformPoint(this.rb.transform.position);
		int num = (this.CosmeticSelectedSide == ECosmeticSelectSide.Left) ? 1 : 2;
		this.stateBitIndex = VRRig.WearablePackedStatesBitWriteInfos[num].index;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00092B5C File Offset: 0x00090D5C
	private void Update()
	{
		if (this.isLocal)
		{
			if (this.rb.isKinematic && (this.rb.transform.position - this.stringParent.transform.position).IsLongerThan(this.stringDetachLength))
			{
				this.Unstick();
			}
			else if (!this.rb.isKinematic && (this.rb.transform.position - this.stringParent.transform.position).IsLongerThan(this.stringTeleportLength))
			{
				this.rb.transform.position = this.stringParent.transform.TransformPoint(this.defaultLocalPosition);
			}
			this.myRig.WearablePackedStates = GTBitOps.WriteBit(this.myRig.WearablePackedStates, this.stateBitIndex, this.rb.isKinematic);
			return;
		}
		if (GTBitOps.ReadBit(this.myRig.WearablePackedStates, this.stateBitIndex) != this.rb.isKinematic)
		{
			if (this.rb.isKinematic)
			{
				this.Unstick();
				return;
			}
			this.Stick();
		}
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x00036E95 File Offset: 0x00035095
	private void Stick()
	{
		this.thwackSound.Play();
		this.flatHand.enabled = true;
		this.regularHand.enabled = false;
		this.rb.isKinematic = true;
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x00036EC6 File Offset: 0x000350C6
	private void Unstick()
	{
		this.schlupSound.Play();
		this.rb.isKinematic = false;
		this.flatHand.enabled = false;
		this.regularHand.enabled = true;
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x00092C8C File Offset: 0x00090E8C
	private void OnCollisionStay(Collision collision)
	{
		if (!this.isLocal || this.rb.isKinematic)
		{
			return;
		}
		if ((this.rb.transform.position - this.stringParent.transform.position).IsLongerThan(this.stringMaxAttachLength))
		{
			return;
		}
		this.Stick();
		Vector3 point = collision.contacts[0].point;
		Vector3 normal = collision.contacts[0].normal;
		this.rb.transform.rotation = Quaternion.LookRotation(normal, this.rb.transform.up);
		Vector3 vector = this.rb.transform.position - point;
		vector -= Vector3.Dot(vector, normal) * normal;
		this.rb.transform.position = point + vector + this.surfaceOffsetDistance * normal;
	}

	// Token: 0x04000BB7 RID: 2999
	[SerializeField]
	private MeshRenderer flatHand;

	// Token: 0x04000BB8 RID: 3000
	[SerializeField]
	private MeshRenderer regularHand;

	// Token: 0x04000BB9 RID: 3001
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x04000BBA RID: 3002
	[SerializeField]
	private GameObject stringParent;

	// Token: 0x04000BBB RID: 3003
	[SerializeField]
	private float surfaceOffsetDistance;

	// Token: 0x04000BBC RID: 3004
	[SerializeField]
	private float stringMaxAttachLength;

	// Token: 0x04000BBD RID: 3005
	[SerializeField]
	private float stringDetachLength;

	// Token: 0x04000BBE RID: 3006
	[SerializeField]
	private float stringTeleportLength;

	// Token: 0x04000BBF RID: 3007
	[SerializeField]
	private SoundBankPlayer thwackSound;

	// Token: 0x04000BC0 RID: 3008
	[SerializeField]
	private SoundBankPlayer schlupSound;

	// Token: 0x04000BC1 RID: 3009
	private VRRig myRig;

	// Token: 0x04000BC2 RID: 3010
	private bool isLocal;

	// Token: 0x04000BC3 RID: 3011
	private int stateBitIndex;

	// Token: 0x04000BC4 RID: 3012
	private Vector3 defaultLocalPosition;
}
