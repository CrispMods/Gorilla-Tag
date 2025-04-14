using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200066B RID: 1643
public class GorillaPressableReleaseButton : GorillaPressableButton
{
	// Token: 0x060028AD RID: 10413 RVA: 0x000C7F0C File Offset: 0x000C610C
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

	// Token: 0x060028AE RID: 10414 RVA: 0x000C8034 File Offset: 0x000C6234
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

	// Token: 0x060028AF RID: 10415 RVA: 0x000C813C File Offset: 0x000C633C
	public override void ResetState()
	{
		base.ResetState();
		this.touchingCollider = null;
	}

	// Token: 0x060028B0 RID: 10416 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void ButtonDeactivation()
	{
	}

	// Token: 0x060028B1 RID: 10417 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void ButtonDeactivationWithHand(bool isLeftHand)
	{
	}

	// Token: 0x04002DBD RID: 11709
	public UnityEvent onReleaseButton;

	// Token: 0x04002DBE RID: 11710
	private Collider touchingCollider;
}
