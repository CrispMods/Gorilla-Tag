using System;
using UnityEngine;

// Token: 0x02000569 RID: 1385
public class GorillaHatButtonParent : MonoBehaviour
{
	// Token: 0x06002224 RID: 8740 RVA: 0x000A8B88 File Offset: 0x000A6D88
	public void Start()
	{
		this.hat = PlayerPrefs.GetString("hatCosmetic", "none");
		this.face = PlayerPrefs.GetString("faceCosmetic", "none");
		this.badge = PlayerPrefs.GetString("badgeCosmetic", "none");
		this.leftHandHold = PlayerPrefs.GetString("leftHandHoldCosmetic", "none");
		this.rightHandHold = PlayerPrefs.GetString("rightHandHoldCosmetic", "none");
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x000A8C00 File Offset: 0x000A6E00
	public void LateUpdate()
	{
		if (!this.initialized && GorillaTagger.Instance.offlineVRRig.InitializedCosmetics)
		{
			this.initialized = true;
			if (GorillaTagger.Instance.offlineVRRig.concatStringOfCosmeticsAllowed.Contains("AdministratorBadge"))
			{
				foreach (GameObject gameObject in this.adminObjects)
				{
					Debug.Log("doing this?");
					gameObject.SetActive(true);
				}
			}
			if (GorillaTagger.Instance.offlineVRRig.concatStringOfCosmeticsAllowed.Contains("earlyaccess"))
			{
				this.UpdateButtonState();
				this.screen.UpdateText("WELCOME TO THE HAT ROOM!\nTHANK YOU FOR PURCHASING THE EARLY ACCESS SUPPORTER PACK! PLEASE ENJOY THESE VARIOUS HATS AND NOT-HATS!", true);
			}
		}
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x000A8CA8 File Offset: 0x000A6EA8
	public void PressButton(bool isOn, GorillaHatButton.HatButtonType buttonType, string buttonValue)
	{
		if (this.initialized && GorillaTagger.Instance.offlineVRRig.concatStringOfCosmeticsAllowed.Contains("earlyaccess"))
		{
			switch (buttonType)
			{
			case GorillaHatButton.HatButtonType.Hat:
				if (this.hat != buttonValue)
				{
					this.hat = buttonValue;
					PlayerPrefs.SetString("hatCosmetic", buttonValue);
				}
				else
				{
					this.hat = "none";
					PlayerPrefs.SetString("hatCosmetic", "none");
				}
				break;
			case GorillaHatButton.HatButtonType.Face:
				if (this.face != buttonValue)
				{
					this.face = buttonValue;
					PlayerPrefs.SetString("faceCosmetic", buttonValue);
				}
				else
				{
					this.face = "none";
					PlayerPrefs.SetString("faceCosmetic", "none");
				}
				break;
			case GorillaHatButton.HatButtonType.Badge:
				if (this.badge != buttonValue)
				{
					this.badge = buttonValue;
					PlayerPrefs.SetString("badgeCosmetic", buttonValue);
				}
				else
				{
					this.badge = "none";
					PlayerPrefs.SetString("badgeCosmetic", "none");
				}
				break;
			}
			PlayerPrefs.Save();
			this.UpdateButtonState();
		}
	}

	// Token: 0x06002227 RID: 8743 RVA: 0x000A8DBC File Offset: 0x000A6FBC
	private void UpdateButtonState()
	{
		foreach (GorillaHatButton gorillaHatButton in this.hatButtons)
		{
			switch (gorillaHatButton.buttonType)
			{
			case GorillaHatButton.HatButtonType.Hat:
				gorillaHatButton.isOn = (gorillaHatButton.cosmeticName == this.hat);
				break;
			case GorillaHatButton.HatButtonType.Face:
				gorillaHatButton.isOn = (gorillaHatButton.cosmeticName == this.face);
				break;
			case GorillaHatButton.HatButtonType.Badge:
				gorillaHatButton.isOn = (gorillaHatButton.cosmeticName == this.badge);
				break;
			}
			gorillaHatButton.UpdateColor();
		}
	}

	// Token: 0x040025A7 RID: 9639
	public GorillaHatButton[] hatButtons;

	// Token: 0x040025A8 RID: 9640
	public GameObject[] adminObjects;

	// Token: 0x040025A9 RID: 9641
	public string hat;

	// Token: 0x040025AA RID: 9642
	public string face;

	// Token: 0x040025AB RID: 9643
	public string badge;

	// Token: 0x040025AC RID: 9644
	public string leftHandHold;

	// Token: 0x040025AD RID: 9645
	public string rightHandHold;

	// Token: 0x040025AE RID: 9646
	public bool initialized;

	// Token: 0x040025AF RID: 9647
	public GorillaLevelScreen screen;
}
