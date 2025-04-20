using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005F0 RID: 1520
public class SizeLayerChanger : MonoBehaviour
{
	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x060025D3 RID: 9683 RVA: 0x00106F40 File Offset: 0x00105140
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

	// Token: 0x060025D4 RID: 9684 RVA: 0x00049A58 File Offset: 0x00047C58
	private void Awake()
	{
		this.minScale = Mathf.Max(this.minScale, 0.01f);
	}

	// Token: 0x060025D5 RID: 9685 RVA: 0x00106F80 File Offset: 0x00105180
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

	// Token: 0x060025D6 RID: 9686 RVA: 0x00107000 File Offset: 0x00105200
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

	// Token: 0x04002A01 RID: 10753
	public float maxScale;

	// Token: 0x04002A02 RID: 10754
	public float minScale;

	// Token: 0x04002A03 RID: 10755
	public bool isAssurance;

	// Token: 0x04002A04 RID: 10756
	public bool affectLayerA = true;

	// Token: 0x04002A05 RID: 10757
	public bool affectLayerB = true;

	// Token: 0x04002A06 RID: 10758
	public bool affectLayerC = true;

	// Token: 0x04002A07 RID: 10759
	public bool affectLayerD = true;

	// Token: 0x04002A08 RID: 10760
	[SerializeField]
	private bool applyOnTriggerEnter = true;

	// Token: 0x04002A09 RID: 10761
	[SerializeField]
	private bool applyOnTriggerExit;

	// Token: 0x04002A0A RID: 10762
	[SerializeField]
	private bool triggerWithBodyCollider;
}
