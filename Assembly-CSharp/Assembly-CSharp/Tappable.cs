using System;
using System.Diagnostics;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005EA RID: 1514
public class Tappable : MonoBehaviour
{
	// Token: 0x0600259F RID: 9631 RVA: 0x000BA00A File Offset: 0x000B820A
	public void Validate()
	{
		this.CalculateId(true);
	}

	// Token: 0x060025A0 RID: 9632 RVA: 0x000BA013 File Offset: 0x000B8213
	protected virtual void OnEnable()
	{
		if (!this.useStaticId)
		{
			this.CalculateId(false);
		}
		TappableManager.Register(this);
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x000BA02A File Offset: 0x000B822A
	protected virtual void OnDisable()
	{
		TappableManager.Unregister(this);
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x00044826 File Offset: 0x00042A26
	public virtual bool CanTap(bool isLeftHand)
	{
		return true;
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x000BA034 File Offset: 0x000B8234
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

	// Token: 0x060025A4 RID: 9636 RVA: 0x000BA090 File Offset: 0x000B8290
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

	// Token: 0x060025A5 RID: 9637 RVA: 0x000BA0E4 File Offset: 0x000B82E4
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

	// Token: 0x060025A6 RID: 9638 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnTapLocal(float tapStrength, float tapTime, PhotonMessageInfoWrapped sender)
	{
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnGrabLocal(float tapTime, PhotonMessageInfoWrapped sender)
	{
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnReleaseLocal(float tapTime, PhotonMessageInfoWrapped sender)
	{
	}

	// Token: 0x060025A9 RID: 9641 RVA: 0x000BA00A File Offset: 0x000B820A
	private void EdRecalculateId()
	{
		this.CalculateId(true);
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x000BA138 File Offset: 0x000B8338
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

	// Token: 0x060025AB RID: 9643 RVA: 0x000BA202 File Offset: 0x000B8402
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
