using System;
using UnityEngine;

// Token: 0x02000577 RID: 1399
public class GorillaHatButtonParent : MonoBehaviour
{
	// Token: 0x06002282 RID: 8834 RVA: 0x000F8ABC File Offset: 0x000F6CBC
	public void Start()
	{
		this.hat = PlayerPrefs.GetString("hatCosmetic", "none");
		this.face = PlayerPrefs.GetString("faceCosmetic", "none");
		this.badge = PlayerPrefs.GetString("badgeCosmetic", "none");
		this.leftHandHold = PlayerPrefs.GetString("leftHandHoldCosmetic", "none");
		this.rightHandHold = PlayerPrefs.GetString("rightHandHoldCosmetic", "none");
	}

	// Token: 0x06002283 RID: 8835 RVA: 0x000F8B34 File Offset: 0x000F6D34
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

	// Token: 0x06002284 RID: 8836 RVA: 0x000F8BDC File Offset: 0x000F6DDC
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

	// Token: 0x06002285 RID: 8837 RVA: 0x000F8CF0 File Offset: 0x000F6EF0
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

	// Token: 0x040025FF RID: 9727
	public GorillaHatButton[] hatButtons;

	// Token: 0x04002600 RID: 9728
	public GameObject[] adminObjects;

	// Token: 0x04002601 RID: 9729
	public string hat;

	// Token: 0x04002602 RID: 9730
	public string face;

	// Token: 0x04002603 RID: 9731
	public string badge;

	// Token: 0x04002604 RID: 9732
	public string leftHandHold;

	// Token: 0x04002605 RID: 9733
	public string rightHandHold;

	// Token: 0x04002606 RID: 9734
	public bool initialized;

	// Token: 0x04002607 RID: 9735
	public GorillaLevelScreen screen;
}
