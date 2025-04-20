using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200064A RID: 1610
public class GorillaPressableReleaseButton : GorillaPressableButton
{
	// Token: 0x060027D8 RID: 10200 RVA: 0x0010ECB8 File Offset: 0x0010CEB8
	private new void OnTriggerEnter(Collider other)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.touchTime + this.debounceTime >= Time.time)
		{
			return;
		}
		if (this.touchingCollider)
		{
			return;
		}
		GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
		if (component == null)
		{
			return;
		}
		this.touchTime = Time.time;
		this.touchingCollider = other;
		UnityEvent onPressButton = this.onPressButton;
		if (onPressButton != null)
		{
			onPressButton.Invoke();
		}
		this.ButtonActivation();
		this.ButtonActivationWithHand(component.isLeftHand);
		GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(this.pressButtonSoundIndex, component.isLeftHand, 0.05f);
		GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
		if (NetworkSystem.Instance.InRoom && GorillaTagger.Instance.myVRRig != null)
		{
			GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.Others, new object[]
			{
				67,
				component.isLeftHand,
				0.05f
			});
		}
	}

	// Token: 0x060027D9 RID: 10201 RVA: 0x0010EDE0 File Offset: 0x0010CFE0
	private void OnTriggerExit(Collider other)
	{
		if (!base.enabled)
		{
			return;
		}
		if (other != this.touchingCollider)
		{
			return;
		}
		this.touchingCollider = null;
		GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
		if (component == null)
		{
			return;
		}
		UnityEvent unityEvent = this.onReleaseButton;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		this.ButtonDeactivation();
		this.ButtonDeactivationWithHand(component.isLeftHand);
		GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(this.pressButtonSoundIndex, component.isLeftHand, 0.05f);
		GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
		if (NetworkSystem.Instance.InRoom && GorillaTagger.Instance.myVRRig != null)
		{
			GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.Others, new object[]
			{
				67,
				component.isLeftHand,
				0.05f
			});
		}
	}

	// Token: 0x060027DA RID: 10202 RVA: 0x0004B14B File Offset: 0x0004934B
	public override void ResetState()
	{
		base.ResetState();
		this.touchingCollider = null;
	}

	// Token: 0x060027DB RID: 10203 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void ButtonDeactivation()
	{
	}

	// Token: 0x060027DC RID: 10204 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void ButtonDeactivationWithHand(bool isLeftHand)
	{
	}

	// Token: 0x04002D23 RID: 11555
	public UnityEvent onReleaseButton;

	// Token: 0x04002D24 RID: 11556
	private Collider touchingCollider;
}
