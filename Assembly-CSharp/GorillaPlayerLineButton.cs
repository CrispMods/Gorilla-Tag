using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000644 RID: 1604
public class GorillaPlayerLineButton : MonoBehaviour
{
	// Token: 0x060027AD RID: 10157 RVA: 0x0004B005 File Offset: 0x00049205
	private void OnEnable()
	{
		if (Application.isEditor)
		{
			base.StartCoroutine(this.TestPressCheck());
		}
	}

	// Token: 0x060027AE RID: 10158 RVA: 0x0004B01B File Offset: 0x0004921B
	private void OnDisable()
	{
		if (Application.isEditor)
		{
			base.StopAllCoroutines();
		}
	}

	// Token: 0x060027AF RID: 10159 RVA: 0x0004B02A File Offset: 0x0004922A
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

	// Token: 0x060027B0 RID: 10160 RVA: 0x0010DC94 File Offset: 0x0010BE94
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

	// Token: 0x060027B1 RID: 10161 RVA: 0x0004B039 File Offset: 0x00049239
	private void OnTriggerExit(Collider other)
	{
		if (this.buttonType != GorillaPlayerLineButton.ButtonType.Mute && other.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
		{
			this.parentLine.canPressNextReportButton = true;
		}
	}

	// Token: 0x060027B2 RID: 10162 RVA: 0x0010DE04 File Offset: 0x0010C004
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

	// Token: 0x04002CD0 RID: 11472
	public GorillaPlayerScoreboardLine parentLine;

	// Token: 0x04002CD1 RID: 11473
	public GorillaPlayerLineButton.ButtonType buttonType;

	// Token: 0x04002CD2 RID: 11474
	public bool isOn;

	// Token: 0x04002CD3 RID: 11475
	public bool isAutoOn;

	// Token: 0x04002CD4 RID: 11476
	public Material offMaterial;

	// Token: 0x04002CD5 RID: 11477
	public Material onMaterial;

	// Token: 0x04002CD6 RID: 11478
	public Material autoOnMaterial;

	// Token: 0x04002CD7 RID: 11479
	public string offText;

	// Token: 0x04002CD8 RID: 11480
	public string onText;

	// Token: 0x04002CD9 RID: 11481
	public string autoOnText;

	// Token: 0x04002CDA RID: 11482
	public Text myText;

	// Token: 0x04002CDB RID: 11483
	public float debounceTime = 0.25f;

	// Token: 0x04002CDC RID: 11484
	public float touchTime;

	// Token: 0x04002CDD RID: 11485
	public bool testPress;

	// Token: 0x02000645 RID: 1605
	public enum ButtonType
	{
		// Token: 0x04002CDF RID: 11487
		HateSpeech,
		// Token: 0x04002CE0 RID: 11488
		Cheating,
		// Token: 0x04002CE1 RID: 11489
		Toxicity,
		// Token: 0x04002CE2 RID: 11490
		Mute,
		// Token: 0x04002CE3 RID: 11491
		Report,
		// Token: 0x04002CE4 RID: 11492
		Cancel
	}
}
