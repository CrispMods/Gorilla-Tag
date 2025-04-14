using System;
using System.Diagnostics;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005E9 RID: 1513
public class Tappable : MonoBehaviour
{
	// Token: 0x06002597 RID: 9623 RVA: 0x000B9B8A File Offset: 0x000B7D8A
	public void Validate()
	{
		this.CalculateId(true);
	}

	// Token: 0x06002598 RID: 9624 RVA: 0x000B9B93 File Offset: 0x000B7D93
	protected virtual void OnEnable()
	{
		if (!this.useStaticId)
		{
			this.CalculateId(false);
		}
		TappableManager.Register(this);
	}

	// Token: 0x06002599 RID: 9625 RVA: 0x000B9BAA File Offset: 0x000B7DAA
	protected virtual void OnDisable()
	{
		TappableManager.Unregister(this);
	}

	// Token: 0x0600259A RID: 9626 RVA: 0x000444E2 File Offset: 0x000426E2
	public virtual bool CanTap(bool isLeftHand)
	{
		return true;
	}

	// Token: 0x0600259B RID: 9627 RVA: 0x000B9BB4 File Offset: 0x000B7DB4
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

	// Token: 0x0600259C RID: 9628 RVA: 0x000B9C10 File Offset: 0x000B7E10
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

	// Token: 0x0600259D RID: 9629 RVA: 0x000B9C64 File Offset: 0x000B7E64
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

	// Token: 0x0600259E RID: 9630 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnTapLocal(float tapStrength, float tapTime, PhotonMessageInfoWrapped sender)
	{
	}

	// Token: 0x0600259F RID: 9631 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnGrabLocal(float tapTime, PhotonMessageInfoWrapped sender)
	{
	}

	// Token: 0x060025A0 RID: 9632 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnReleaseLocal(float tapTime, PhotonMessageInfoWrapped sender)
	{
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x000B9B8A File Offset: 0x000B7D8A
	private void EdRecalculateId()
	{
		this.CalculateId(true);
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x000B9CB8 File Offset: 0x000B7EB8
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

	// Token: 0x060025A3 RID: 9635 RVA: 0x000B9D82 File Offset: 0x000B7F82
	[Conditional("UNITY_EDITOR")]
	private void OnValidate()
	{
		this.CalculateId(false);
	}

	// Token: 0x040029CF RID: 10703
	public int tappableId;

	// Token: 0x040029D0 RID: 10704
	public string staticId;

	// Token: 0x040029D1 RID: 10705
	public bool useStaticId;

	// Token: 0x040029D2 RID: 10706
	[Tooltip("If true, tap cooldown will be ignored.  Tapping will be allowed/disallowed based on result of CanTap()")]
	public bool overrideTapCooldown;

	// Token: 0x040029D3 RID: 10707
	[Space]
	public TappableManager manager;

	// Token: 0x040029D4 RID: 10708
	public RpcTarget rpcTarget;
}
