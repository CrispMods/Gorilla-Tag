using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000666 RID: 1638
public class GorillaPlayerLineButton : MonoBehaviour
{
	// Token: 0x0600288A RID: 10378 RVA: 0x000C7222 File Offset: 0x000C5422
	private void OnEnable()
	{
		if (Application.isEditor)
		{
			base.StartCoroutine(this.TestPressCheck());
		}
	}

	// Token: 0x0600288B RID: 10379 RVA: 0x000C7238 File Offset: 0x000C5438
	private void OnDisable()
	{
		if (Application.isEditor)
		{
			base.StopAllCoroutines();
		}
	}

	// Token: 0x0600288C RID: 10380 RVA: 0x000C7247 File Offset: 0x000C5447
	private IEnumerator TestPressCheck()
	{
		for (;;)
		{
			if (this.testPress)
			{
				this.testPress = false;
				if (this.buttonType == GorillaPlayerLineButton.ButtonType.Mute)
				{
					this.isOn = !this.isOn;
				}
				this.parentLine.PressButton(this.isOn, this.buttonType);
			}
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	// Token: 0x0600288D RID: 10381 RVA: 0x000C7258 File Offset: 0x000C5458
	private void OnTriggerEnter(Collider collider)
	{
		if (base.enabled && this.touchTime + this.debounceTime < Time.time && collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
		{
			this.touchTime = Time.time;
			GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
			if (this.buttonType == GorillaPlayerLineButton.ButtonType.Mute)
			{
				if (this.isAutoOn)
				{
					this.isOn = false;
				}
				else
				{
					this.isOn = !this.isOn;
				}
			}
			if (this.buttonType == GorillaPlayerLineButton.ButtonType.Mute || this.buttonType == GorillaPlayerLineButton.ButtonType.HateSpeech || this.buttonType == GorillaPlayerLineButton.ButtonType.Cheating || this.buttonType == GorillaPlayerLineButton.ButtonType.Cancel || this.parentLine.canPressNextReportButton)
			{
				this.parentLine.PressButton(this.isOn, this.buttonType);
				if (component != null)
				{
					GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
					GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, component.isLeftHand, 0.05f);
					if (PhotonNetwork.InRoom && GorillaTagger.Instance.myVRRig != null)
					{
						GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.Others, new object[]
						{
							67,
							component.isLeftHand,
							0.05f
						});
					}
				}
			}
		}
	}

	// Token: 0x0600288E RID: 10382 RVA: 0x000C73C8 File Offset: 0x000C55C8
	private void OnTriggerExit(Collider other)
	{
		if (this.buttonType != GorillaPlayerLineButton.ButtonType.Mute && other.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
		{
			this.parentLine.canPressNextReportButton = true;
		}
	}

	// Token: 0x0600288F RID: 10383 RVA: 0x000C73F0 File Offset: 0x000C55F0
	public void UpdateColor()
	{
		if (this.isOn)
		{
			base.GetComponent<MeshRenderer>().material = this.onMaterial;
			this.myText.text = this.onText;
			return;
		}
		if (this.isAutoOn)
		{
			base.GetComponent<MeshRenderer>().material = this.autoOnMaterial;
			this.myText.text = this.autoOnText;
			return;
		}
		base.GetComponent<MeshRenderer>().material = this.offMaterial;
		this.myText.text = this.offText;
	}

	// Token: 0x04002D70 RID: 11632
	public GorillaPlayerScoreboardLine parentLine;

	// Token: 0x04002D71 RID: 11633
	public GorillaPlayerLineButton.ButtonType buttonType;

	// Token: 0x04002D72 RID: 11634
	public bool isOn;

	// Token: 0x04002D73 RID: 11635
	public bool isAutoOn;

	// Token: 0x04002D74 RID: 11636
	public Material offMaterial;

	// Token: 0x04002D75 RID: 11637
	public Material onMaterial;

	// Token: 0x04002D76 RID: 11638
	public Material autoOnMaterial;

	// Token: 0x04002D77 RID: 11639
	public string offText;

	// Token: 0x04002D78 RID: 11640
	public string onText;

	// Token: 0x04002D79 RID: 11641
	public string autoOnText;

	// Token: 0x04002D7A RID: 11642
	public Text myText;

	// Token: 0x04002D7B RID: 11643
	public float debounceTime = 0.25f;

	// Token: 0x04002D7C RID: 11644
	public float touchTime;

	// Token: 0x04002D7D RID: 11645
	public bool testPress;

	// Token: 0x02000667 RID: 1639
	public enum ButtonType
	{
		// Token: 0x04002D7F RID: 11647
		HateSpeech,
		// Token: 0x04002D80 RID: 11648
		Cheating,
		// Token: 0x04002D81 RID: 11649
		Toxicity,
		// Token: 0x04002D82 RID: 11650
		Mute,
		// Token: 0x04002D83 RID: 11651
		Report,
		// Token: 0x04002D84 RID: 11652
		Cancel
	}
}
