using System;
using System.Diagnostics;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005F7 RID: 1527
public class Tappable : MonoBehaviour
{
	// Token: 0x060025F9 RID: 9721 RVA: 0x00049CCA File Offset: 0x00047ECA
	public void Validate()
	{
		this.CalculateId(true);
	}

	// Token: 0x060025FA RID: 9722 RVA: 0x00049CD3 File Offset: 0x00047ED3
	protected virtual void OnEnable()
	{
		if (!this.useStaticId)
		{
			this.CalculateId(false);
		}
		TappableManager.Register(this);
	}

	// Token: 0x060025FB RID: 9723 RVA: 0x00049CEA File Offset: 0x00047EEA
	protected virtual void OnDisable()
	{
		TappableManager.Unregister(this);
	}

	// Token: 0x060025FC RID: 9724 RVA: 0x00039846 File Offset: 0x00037A46
	public virtual bool CanTap(bool isLeftHand)
	{
		return true;
	}

	// Token: 0x060025FD RID: 9725 RVA: 0x00107724 File Offset: 0x00105924
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

	// Token: 0x060025FE RID: 9726 RVA: 0x00107780 File Offset: 0x00105980
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

	// Token: 0x060025FF RID: 9727 RVA: 0x001077D4 File Offset: 0x001059D4
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

	// Token: 0x06002600 RID: 9728 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void OnTapLocal(float tapStrength, float tapTime, PhotonMessageInfoWrapped sender)
	{
	}

	// Token: 0x06002601 RID: 9729 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void OnGrabLocal(float tapTime, PhotonMessageInfoWrapped sender)
	{
	}

	// Token: 0x06002602 RID: 9730 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void OnReleaseLocal(float tapTime, PhotonMessageInfoWrapped sender)
	{
	}

	// Token: 0x06002603 RID: 9731 RVA: 0x00049CCA File Offset: 0x00047ECA
	private void EdRecalculateId()
	{
		this.CalculateId(true);
	}

	// Token: 0x06002604 RID: 9732 RVA: 0x00107828 File Offset: 0x00105A28
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

	// Token: 0x06002605 RID: 9733 RVA: 0x00049CF2 File Offset: 0x00047EF2
	[Conditional("UNITY_EDITOR")]
	private void OnValidate()
	{
		this.CalculateId(false);
	}

	// Token: 0x04002A2E RID: 10798
	public int tappableId;

	// Token: 0x04002A2F RID: 10799
	public string staticId;

	// Token: 0x04002A30 RID: 10800
	public bool useStaticId;

	// Token: 0x04002A31 RID: 10801
	[Tooltip("If true, tap cooldown will be ignored.  Tapping will be allowed/disallowed based on result of CanTap()")]
	public bool overrideTapCooldown;

	// Token: 0x04002A32 RID: 10802
	[Space]
	public TappableManager manager;

	// Token: 0x04002A33 RID: 10803
	public RpcTarget rpcTarget;
}
