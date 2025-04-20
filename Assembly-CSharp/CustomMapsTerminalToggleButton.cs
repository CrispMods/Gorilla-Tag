using System;
using Photon.Pun;

// Token: 0x020006A5 RID: 1701
public class CustomMapsTerminalToggleButton : CustomMapsTerminalButton
{
	// Token: 0x06002A48 RID: 10824 RVA: 0x0011BED4 File Offset: 0x0011A0D4
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		GorillaTagger.Instance.StartVibration(isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
		GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, isLeftHand, 0.1f);
		if (NetworkSystem.Instance.InRoom && GorillaTagger.Instance.myVRRig != null)
		{
			GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.Others, new object[]
			{
				66,
				isLeftHand,
				0.1f
			});
		}
		if (CustomMapsTerminal.IsDriver)
		{
			GameEvents.OnModIOKeyboardButtonPressedEvent.Invoke(this.modIOBinding);
		}
	}

	// Token: 0x06002A49 RID: 10825 RVA: 0x0004C879 File Offset: 0x0004AA79
	public void SetButtonStatus(bool isPressed)
	{
		this.isOn = isPressed;
		this.UpdateColor();
	}
}
