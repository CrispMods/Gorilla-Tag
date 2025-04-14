using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000665 RID: 1637
public class GorillaPlayerLineButton : MonoBehaviour
{
	// Token: 0x06002882 RID: 10370 RVA: 0x000C6DA2 File Offset: 0x000C4FA2
	private void OnEnable()
	{
		if (Application.isEditor)
		{
			base.StartCoroutine(this.TestPressCheck());
		}
	}

	// Token: 0x06002883 RID: 10371 RVA: 0x000C6DB8 File Offset: 0x000C4FB8
	private void OnDisable()
	{
		if (Application.isEditor)
		{
			base.StopAllCoroutines();
		}
	}

	// Token: 0x06002884 RID: 10372 RVA: 0x000C6DC7 File Offset: 0x000C4FC7
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

	// Token: 0x06002885 RID: 10373 RVA: 0x000C6DD8 File Offset: 0x000C4FD8
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

	// Token: 0x06002886 RID: 10374 RVA: 0x000C6F48 File Offset: 0x000C5148
	private void OnTriggerExit(Collider other)
	{
		if (this.buttonType != GorillaPlayerLineButton.ButtonType.Mute && other.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
		{
			this.parentLine.canPressNextReportButton = true;
		}
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x000C6F70 File Offset: 0x000C5170
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

	// Token: 0x04002D6A RID: 11626
	public GorillaPlayerScoreboardLine parentLine;

	// Token: 0x04002D6B RID: 11627
	public GorillaPlayerLineButton.ButtonType buttonType;

	// Token: 0x04002D6C RID: 11628
	public bool isOn;

	// Token: 0x04002D6D RID: 11629
	public bool isAutoOn;

	// Token: 0x04002D6E RID: 11630
	public Material offMaterial;

	// Token: 0x04002D6F RID: 11631
	public Material onMaterial;

	// Token: 0x04002D70 RID: 11632
	public Material autoOnMaterial;

	// Token: 0x04002D71 RID: 11633
	public string offText;

	// Token: 0x04002D72 RID: 11634
	public string onText;

	// Token: 0x04002D73 RID: 11635
	public string autoOnText;

	// Token: 0x04002D74 RID: 11636
	public Text myText;

	// Token: 0x04002D75 RID: 11637
	public float debounceTime = 0.25f;

	// Token: 0x04002D76 RID: 11638
	public float touchTime;

	// Token: 0x04002D77 RID: 11639
	public bool testPress;

	// Token: 0x02000666 RID: 1638
	public enum ButtonType
	{
		// Token: 0x04002D79 RID: 11641
		HateSpeech,
		// Token: 0x04002D7A RID: 11642
		Cheating,
		// Token: 0x04002D7B RID: 11643
		Toxicity,
		// Token: 0x04002D7C RID: 11644
		Mute,
		// Token: 0x04002D7D RID: 11645
		Report,
		// Token: 0x04002D7E RID: 11646
		Cancel
	}
}
