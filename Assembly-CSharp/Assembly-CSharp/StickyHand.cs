using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class StickyHand : MonoBehaviour, ISpawnable
{
	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000979 RID: 2425 RVA: 0x00032AAF File Offset: 0x00030CAF
	// (set) Token: 0x0600097A RID: 2426 RVA: 0x00032AB7 File Offset: 0x00030CB7
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x0600097B RID: 2427 RVA: 0x00032AC0 File Offset: 0x00030CC0
	// (set) Token: 0x0600097C RID: 2428 RVA: 0x00032AC8 File Offset: 0x00030CC8
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x0600097D RID: 2429 RVA: 0x00032AD4 File Offset: 0x00030CD4
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
		this.isLocal = rig.isLocal;
		this.flatHand.enabled = false;
		this.defaultLocalPosition = this.stringParent.transform.InverseTransformPoint(this.rb.transform.position);
		int num = (this.CosmeticSelectedSide == ECosmeticSelectSide.Left) ? 1 : 2;
		this.stateBitIndex = VRRig.WearablePackedStatesBitWriteInfos[num].index;
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00032B4C File Offset: 0x00030D4C
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

	// Token: 0x06000980 RID: 2432 RVA: 0x00032C7A File Offset: 0x00030E7A
	private void Stick()
	{
		this.thwackSound.Play();
		this.flatHand.enabled = true;
		this.regularHand.enabled = false;
		this.rb.isKinematic = true;
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x00032CAB File Offset: 0x00030EAB
	private void Unstick()
	{
		this.schlupSound.Play();
		this.rb.isKinematic = false;
		this.flatHand.enabled = false;
		this.regularHand.enabled = true;
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00032CDC File Offset: 0x00030EDC
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

	// Token: 0x04000B72 RID: 2930
	[SerializeField]
	private MeshRenderer flatHand;

	// Token: 0x04000B73 RID: 2931
	[SerializeField]
	private MeshRenderer regularHand;

	// Token: 0x04000B74 RID: 2932
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x04000B75 RID: 2933
	[SerializeField]
	private GameObject stringParent;

	// Token: 0x04000B76 RID: 2934
	[SerializeField]
	private float surfaceOffsetDistance;

	// Token: 0x04000B77 RID: 2935
	[SerializeField]
	private float stringMaxAttachLength;

	// Token: 0x04000B78 RID: 2936
	[SerializeField]
	private float stringDetachLength;

	// Token: 0x04000B79 RID: 2937
	[SerializeField]
	private float stringTeleportLength;

	// Token: 0x04000B7A RID: 2938
	[SerializeField]
	private SoundBankPlayer thwackSound;

	// Token: 0x04000B7B RID: 2939
	[SerializeField]
	private SoundBankPlayer schlupSound;

	// Token: 0x04000B7C RID: 2940
	private VRRig myRig;

	// Token: 0x04000B7D RID: 2941
	private bool isLocal;

	// Token: 0x04000B7E RID: 2942
	private int stateBitIndex;

	// Token: 0x04000B7F RID: 2943
	private Vector3 defaultLocalPosition;
}
