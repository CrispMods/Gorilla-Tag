using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005E2 RID: 1506
public class SizeLayerChanger : MonoBehaviour
{
	// Token: 0x170003DE RID: 990
	// (get) Token: 0x06002571 RID: 9585 RVA: 0x000B9138 File Offset: 0x000B7338
	public int SizeLayerMask
	{
		get
		{
			int num = 0;
			if (this.affectLayerA)
			{
				num |= 1;
			}
			if (this.affectLayerB)
			{
				num |= 2;
			}
			if (this.affectLayerC)
			{
				num |= 4;
			}
			if (this.affectLayerD)
			{
				num |= 8;
			}
			return num;
		}
	}

	// Token: 0x06002572 RID: 9586 RVA: 0x000B9178 File Offset: 0x000B7378
	private void Awake()
	{
		this.minScale = Mathf.Max(this.minScale, 0.01f);
	}

	// Token: 0x06002573 RID: 9587 RVA: 0x000B9190 File Offset: 0x000B7390
	public void OnTriggerEnter(Collider other)
	{
		if (!this.triggerWithBodyCollider && !other.GetComponent<SphereCollider>())
		{
			return;
		}
		VRRig vrrig;
		if (this.triggerWithBodyCollider)
		{
			if (other != GTPlayer.Instance.bodyCollider)
			{
				return;
			}
			vrrig = GorillaTagger.Instance.offlineVRRig;
		}
		else
		{
			vrrig = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		}
		if (vrrig == null)
		{
			return;
		}
		if (this.applyOnTriggerEnter)
		{
			vrrig.sizeManager.currentSizeLayerMaskValue = this.SizeLayerMask;
		}
	}

	// Token: 0x06002574 RID: 9588 RVA: 0x000B9210 File Offset: 0x000B7410
	public void OnTriggerExit(Collider other)
	{
		if (!this.triggerWithBodyCollider && !other.GetComponent<SphereCollider>())
		{
			return;
		}
		VRRig vrrig;
		if (this.triggerWithBodyCollider)
		{
			if (other != GTPlayer.Instance.bodyCollider)
			{
				return;
			}
			vrrig = GorillaTagger.Instance.offlineVRRig;
		}
		else
		{
			vrrig = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		}
		if (vrrig == null)
		{
			return;
		}
		if (this.applyOnTriggerExit)
		{
			vrrig.sizeManager.currentSizeLayerMaskValue = this.SizeLayerMask;
		}
	}

	// Token: 0x040029A2 RID: 10658
	public float maxScale;

	// Token: 0x040029A3 RID: 10659
	public float minScale;

	// Token: 0x040029A4 RID: 10660
	public bool isAssurance;

	// Token: 0x040029A5 RID: 10661
	public bool affectLayerA = true;

	// Token: 0x040029A6 RID: 10662
	public bool affectLayerB = true;

	// Token: 0x040029A7 RID: 10663
	public bool affectLayerC = true;

	// Token: 0x040029A8 RID: 10664
	public bool affectLayerD = true;

	// Token: 0x040029A9 RID: 10665
	[SerializeField]
	private bool applyOnTriggerEnter = true;

	// Token: 0x040029AA RID: 10666
	[SerializeField]
	private bool applyOnTriggerExit;

	// Token: 0x040029AB RID: 10667
	[SerializeField]
	private bool triggerWithBodyCollider;
}
