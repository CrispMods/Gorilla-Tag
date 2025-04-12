using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005E3 RID: 1507
public class SizeLayerChanger : MonoBehaviour
{
	// Token: 0x170003DF RID: 991
	// (get) Token: 0x06002579 RID: 9593 RVA: 0x00104004 File Offset: 0x00102204
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

	// Token: 0x0600257A RID: 9594 RVA: 0x00048681 File Offset: 0x00046881
	private void Awake()
	{
		this.minScale = Mathf.Max(this.minScale, 0.01f);
	}

	// Token: 0x0600257B RID: 9595 RVA: 0x00104044 File Offset: 0x00102244
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

	// Token: 0x0600257C RID: 9596 RVA: 0x001040C4 File Offset: 0x001022C4
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

	// Token: 0x040029A8 RID: 10664
	public float maxScale;

	// Token: 0x040029A9 RID: 10665
	public float minScale;

	// Token: 0x040029AA RID: 10666
	public bool isAssurance;

	// Token: 0x040029AB RID: 10667
	public bool affectLayerA = true;

	// Token: 0x040029AC RID: 10668
	public bool affectLayerB = true;

	// Token: 0x040029AD RID: 10669
	public bool affectLayerC = true;

	// Token: 0x040029AE RID: 10670
	public bool affectLayerD = true;

	// Token: 0x040029AF RID: 10671
	[SerializeField]
	private bool applyOnTriggerEnter = true;

	// Token: 0x040029B0 RID: 10672
	[SerializeField]
	private bool applyOnTriggerExit;

	// Token: 0x040029B1 RID: 10673
	[SerializeField]
	private bool triggerWithBodyCollider;
}
