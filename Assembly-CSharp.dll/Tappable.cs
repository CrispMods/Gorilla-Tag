using System;
using System.Diagnostics;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005EA RID: 1514
public class Tappable : MonoBehaviour
{
	// Token: 0x0600259F RID: 9631 RVA: 0x000488F3 File Offset: 0x00046AF3
	public void Validate()
	{
		this.CalculateId(true);
	}

	// Token: 0x060025A0 RID: 9632 RVA: 0x000488FC File Offset: 0x00046AFC
	protected virtual void OnEnable()
	{
		if (!this.useStaticId)
		{
			this.CalculateId(false);
		}
		TappableManager.Register(this);
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x00048913 File Offset: 0x00046B13
	protected virtual void OnDisable()
	{
		TappableManager.Unregister(this);
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x00038586 File Offset: 0x00036786
	public virtual bool CanTap(bool isLeftHand)
	{
		return true;
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x001047E8 File Offset: 0x001029E8
	public void OnTap(float tapStrength)
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			return;
		}
		if (!this.manager)
		{
			return;
		}
		this.manager.photonView.RPC("SendOnTapRPC", RpcTarget.All, new object[]
		{
			this.tappableId,
			tapStrength
		});
	}

	// Token: 0x060025A4 RID: 9636 RVA: 0x00104844 File Offset: 0x00102A44
	public void OnGrab()
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			return;
		}
		if (!this.manager)
		{
			return;
		}
		this.manager.photonView.RPC("SendOnGrabRPC", RpcTarget.All, new object[]
		{
			this.tappableId
		});
	}

	// Token: 0x060025A5 RID: 9637 RVA: 0x00104898 File Offset: 0x00102A98
	public void OnRelease()
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			return;
		}
		if (!this.manager)
		{
			return;
		}
		this.manager.photonView.RPC("SendOnReleaseRPC", RpcTarget.All, new object[]
		{
			this.tappableId
		});
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void OnTapLocal(float tapStrength, float tapTime, PhotonMessageInfoWrapped sender)
	{
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void OnGrabLocal(float tapTime, PhotonMessageInfoWrapped sender)
	{
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void OnReleaseLocal(float tapTime, PhotonMessageInfoWrapped sender)
	{
	}

	// Token: 0x060025A9 RID: 9641 RVA: 0x000488F3 File Offset: 0x00046AF3
	private void EdRecalculateId()
	{
		this.CalculateId(true);
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x001048EC File Offset: 0x00102AEC
	private void CalculateId(bool force = false)
	{
		Transform transform = base.transform;
		int hashCode = TransformUtils.ComputePathHash(transform).ToId128().GetHashCode();
		int staticHash = base.GetType().Name.GetStaticHash();
		int hashCode2 = transform.position.QuantizedId128().GetHashCode();
		int num = StaticHash.Compute(hashCode, staticHash, hashCode2);
		if (this.useStaticId)
		{
			if (string.IsNullOrEmpty(this.staticId) || force)
			{
				int instanceID = transform.GetInstanceID();
				int num2 = StaticHash.Compute(num, instanceID);
				this.staticId = string.Format("#ID_{0:X8}", num2);
			}
			this.tappableId = this.staticId.GetStaticHash();
			return;
		}
		this.tappableId = (Application.isPlaying ? num : 0);
	}

	// Token: 0x060025AB RID: 9643 RVA: 0x0004891B File Offset: 0x00046B1B
	[Conditional("UNITY_EDITOR")]
	private void OnValidate()
	{
		this.CalculateId(false);
	}

	// Token: 0x040029D5 RID: 10709
	public int tappableId;

	// Token: 0x040029D6 RID: 10710
	public string staticId;

	// Token: 0x040029D7 RID: 10711
	public bool useStaticId;

	// Token: 0x040029D8 RID: 10712
	[Tooltip("If true, tap cooldown will be ignored.  Tapping will be allowed/disallowed based on result of CanTap()")]
	public bool overrideTapCooldown;

	// Token: 0x040029D9 RID: 10713
	[Space]
	public TappableManager manager;

	// Token: 0x040029DA RID: 10714
	public RpcTarget rpcTarget;
}
