using System;
using UnityEngine;

// Token: 0x0200056A RID: 1386
public class GorillaHatButtonParent : MonoBehaviour
{
	// Token: 0x0600222C RID: 8748 RVA: 0x000F5D40 File Offset: 0x000F3F40
	public void Start()
	{
		this.hat = PlayerPrefs.GetString("hatCosmetic", "none");
		this.face = PlayerPrefs.GetString("faceCosmetic", "none");
		this.badge = PlayerPrefs.GetString("badgeCosmetic", "none");
		this.leftHandHold = PlayerPrefs.GetString("leftHandHoldCosmetic", "none");
		this.rightHandHold = PlayerPrefs.GetString("rightHandHoldCosmetic", "none");
	}

	// Token: 0x0600222D RID: 8749 RVA: 0x000F5DB8 File Offset: 0x000F3FB8
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

	// Token: 0x0600222E RID: 8750 RVA: 0x000F5E60 File Offset: 0x000F4060
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

	// Token: 0x0600222F RID: 8751 RVA: 0x000F5F74 File Offset: 0x000F4174
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

	// Token: 0x040025AD RID: 9645
	public GorillaHatButton[] hatButtons;

	// Token: 0x040025AE RID: 9646
	public GameObject[] adminObjects;

	// Token: 0x040025AF RID: 9647
	public string hat;

	// Token: 0x040025B0 RID: 9648
	public string face;

	// Token: 0x040025B1 RID: 9649
	public string badge;

	// Token: 0x040025B2 RID: 9650
	public string leftHandHold;

	// Token: 0x040025B3 RID: 9651
	public string rightHandHold;

	// Token: 0x040025B4 RID: 9652
	public bool initialized;

	// Token: 0x040025B5 RID: 9653
	public GorillaLevelScreen screen;
}
