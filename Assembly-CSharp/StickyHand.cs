using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class StickyHand : MonoBehaviour, ISpawnable
{
	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000977 RID: 2423 RVA: 0x0003278B File Offset: 0x0003098B
	// (set) Token: 0x06000978 RID: 2424 RVA: 0x00032793 File Offset: 0x00030993
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000979 RID: 2425 RVA: 0x0003279C File Offset: 0x0003099C
	// (set) Token: 0x0600097A RID: 2426 RVA: 0x000327A4 File Offset: 0x000309A4
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x0600097B RID: 2427 RVA: 0x000327B0 File Offset: 0x000309B0
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
		this.isLocal = rig.isLocal;
		this.flatHand.enabled = false;
		this.defaultLocalPosition = this.stringParent.transform.InverseTransformPoint(this.rb.transform.position);
		int num = (this.CosmeticSelectedSide == ECosmeticSelectSide.Left) ? 1 : 2;
		this.stateBitIndex = VRRig.WearablePackedStatesBitWriteInfos[num].index;
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00032828 File Offset: 0x00030A28
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

	// Token: 0x0600097E RID: 2430 RVA: 0x00032956 File Offset: 0x00030B56
	private void Stick()
	{
		this.thwackSound.Play();
		this.flatHand.enabled = true;
		this.regularHand.enabled = false;
		this.rb.isKinematic = true;
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00032987 File Offset: 0x00030B87
	private void Unstick()
	{
		this.schlupSound.Play();
		this.rb.isKinematic = false;
		this.flatHand.enabled = false;
		this.regularHand.enabled = true;
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x000329B8 File Offset: 0x00030BB8
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

	// Token: 0x04000B71 RID: 2929
	[SerializeField]
	private MeshRenderer flatHand;

	// Token: 0x04000B72 RID: 2930
	[SerializeField]
	private MeshRenderer regularHand;

	// Token: 0x04000B73 RID: 2931
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x04000B74 RID: 2932
	[SerializeField]
	private GameObject stringParent;

	// Token: 0x04000B75 RID: 2933
	[SerializeField]
	private float surfaceOffsetDistance;

	// Token: 0x04000B76 RID: 2934
	[SerializeField]
	private float stringMaxAttachLength;

	// Token: 0x04000B77 RID: 2935
	[SerializeField]
	private float stringDetachLength;

	// Token: 0x04000B78 RID: 2936
	[SerializeField]
	private float stringTeleportLength;

	// Token: 0x04000B79 RID: 2937
	[SerializeField]
	private SoundBankPlayer thwackSound;

	// Token: 0x04000B7A RID: 2938
	[SerializeField]
	private SoundBankPlayer schlupSound;

	// Token: 0x04000B7B RID: 2939
	private VRRig myRig;

	// Token: 0x04000B7C RID: 2940
	private bool isLocal;

	// Token: 0x04000B7D RID: 2941
	private int stateBitIndex;

	// Token: 0x04000B7E RID: 2942
	private Vector3 defaultLocalPosition;
}
