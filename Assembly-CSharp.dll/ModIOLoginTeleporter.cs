using System;
using System.Collections;
using GorillaNetworking;
using GorillaTagScripts.ModIO;
using KID.Model;
using TMPro;
using UnityEngine;

// Token: 0x02000602 RID: 1538
public class ModIOLoginTeleporter : MonoBehaviour, IBuildValidation
{
	// Token: 0x0600262A RID: 9770 RVA: 0x00048F34 File Offset: 0x00047134
	public bool BuildValidationCheck()
	{
		if (this.mySerializer == null && base.transform.parent.parent.GetComponentInChildren<VRTeleporterSerializer>() == null)
		{
			Debug.LogError("This teleporter needs a reference to a VRTeleporterSerializer, or to be placed alongside a VRTeleporterSerializer. Check out the arcade or the stump", this);
			return false;
		}
		return true;
	}

	// Token: 0x0600262B RID: 9771 RVA: 0x0010667C File Offset: 0x0010487C
	public void OnEnable()
	{
		this.disabled = false;
		this.ShowHandHolds();
		if (this.mySerializer == null)
		{
			this.mySerializer = base.transform.parent.parent.GetComponentInChildren<VRTeleporterSerializer>();
		}
		bool flag = false;
		if (GorillaServer.Instance == null || !GorillaServer.Instance.FeatureFlagsReady)
		{
			return;
		}
		if (!GorillaServer.Instance.UseKID())
		{
			flag = PlayFabAuthenticator.instance.GetSafety();
		}
		else if (KIDIntegration.IsReady)
		{
			AgeStatusType activeAccountStatus = KIDManager.Instance.GetActiveAccountStatus();
			if (activeAccountStatus != AgeStatusType.LEGALADULT)
			{
				if (activeAccountStatus == (AgeStatusType)0)
				{
					bool safety = PlayFabAuthenticator.instance.GetSafety();
					int userAge = KIDAgeGate.UserAge;
					if (safety || userAge < 13)
					{
						flag = true;
					}
				}
				else if (KIDManager.KIDIntegration.GetAgeFromDateOfBirth() < 13)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.Disable();
		}
	}

	// Token: 0x0600262C RID: 9772 RVA: 0x00048F6F File Offset: 0x0004716F
	private void Disable()
	{
		this.HideHandHolds();
		this.disabled = true;
	}

	// Token: 0x0600262D RID: 9773 RVA: 0x0010674C File Offset: 0x0010494C
	public void OnTriggerEnter(Collider other)
	{
		if (this.disabled || CustomMapManager.WaitingForRoomJoin || CustomMapManager.WaitingForDisconnect)
		{
			return;
		}
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject)
		{
			this.triggerEntryTime = Time.time;
			this.ShowCountdownText();
		}
	}

	// Token: 0x0600262E RID: 9774 RVA: 0x001067A0 File Offset: 0x001049A0
	public void OnTriggerStay(Collider other)
	{
		if (this.disabled)
		{
			return;
		}
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject && this.triggerEntryTime >= 0f)
		{
			this.UpdateCountdownText();
			if (!this.processing && this.triggerEntryTime + this.stayInTriggerDuration <= Time.time)
			{
				this.LoginAndTeleport();
				this.HideCountdownText();
			}
		}
	}

	// Token: 0x0600262F RID: 9775 RVA: 0x00048F7E File Offset: 0x0004717E
	public void OnTriggerExit(Collider other)
	{
		if (this.disabled)
		{
			return;
		}
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject)
		{
			this.triggerEntryTime = -1f;
			this.HideCountdownText();
		}
	}

	// Token: 0x06002630 RID: 9776 RVA: 0x00106810 File Offset: 0x00104A10
	private void ShowCountdownText()
	{
		if (this.disabled)
		{
			return;
		}
		if (this.countdownText != null)
		{
			int num = 1 + Mathf.FloorToInt(this.stayInTriggerDuration);
			this.countdownText.text = num.ToString();
			this.countdownText.gameObject.SetActive(true);
		}
	}

	// Token: 0x06002631 RID: 9777 RVA: 0x00048FB6 File Offset: 0x000471B6
	private void HideCountdownText()
	{
		if (this.countdownText != null)
		{
			this.countdownText.text = "";
			this.countdownText.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002632 RID: 9778 RVA: 0x00106868 File Offset: 0x00104A68
	private void UpdateCountdownText()
	{
		if (this.disabled)
		{
			return;
		}
		if (this.countdownText != null)
		{
			float f = this.stayInTriggerDuration - (Time.time - this.triggerEntryTime);
			int num = 1 + Mathf.FloorToInt(f);
			this.countdownText.text = num.ToString();
		}
	}

	// Token: 0x06002633 RID: 9779 RVA: 0x00048FE7 File Offset: 0x000471E7
	public void LoginAndTeleport()
	{
		if (this.disabled)
		{
			return;
		}
		if (!this.processing)
		{
			this.processing = true;
			PrivateUIRoom.ForceStartOverlay();
			GorillaTagger.Instance.overrideNotInFocus = true;
			ModIODataStore.Initialize(delegate(ModIORequestResult result)
			{
				if (result.success && this.autoLoginWithPlatformCredentials)
				{
					ModIODataStore.RequestPlatformLogin(new Action<ModIORequestResult>(this.OnPlatformLoginComplete));
					return;
				}
				this.HandleTeleport();
			});
		}
	}

	// Token: 0x06002634 RID: 9780 RVA: 0x00049022 File Offset: 0x00047222
	private void OnPlatformLoginComplete(ModIORequestResult result)
	{
		if (this.disabled)
		{
			return;
		}
		if (result.success)
		{
			this.HandleTeleport();
			return;
		}
		this.HandleTeleport();
	}

	// Token: 0x06002635 RID: 9781 RVA: 0x001068BC File Offset: 0x00104ABC
	private void HandleTeleport()
	{
		if (this.disabled)
		{
			return;
		}
		base.StartCoroutine(CustomMapManager.TeleportToVirtualStump(this.teleporterIndex, new Action<bool>(this.FinishTeleport), this.entrancePoint, this.mySerializer));
		base.StartCoroutine(this.DelayedFinishTeleport());
	}

	// Token: 0x06002636 RID: 9782 RVA: 0x00049042 File Offset: 0x00047242
	private IEnumerator DelayedFinishTeleport()
	{
		yield return new WaitForSecondsRealtime(this.maxTeleportProcessingTime);
		this.FinishTeleport(true);
		yield break;
	}

	// Token: 0x06002637 RID: 9783 RVA: 0x00049051 File Offset: 0x00047251
	private void FinishTeleport(bool success = true)
	{
		if (this.processing)
		{
			this.processing = false;
			this.triggerEntryTime = -1f;
			CustomMapManager.DisableTeleportHUD();
			GorillaTagger.Instance.overrideNotInFocus = false;
			PrivateUIRoom.StopForcedOverlay();
		}
	}

	// Token: 0x06002638 RID: 9784 RVA: 0x0010690C File Offset: 0x00104B0C
	private void HideHandHolds()
	{
		if (this.disabled)
		{
			return;
		}
		foreach (GameObject gameObject in this.handHoldObjects)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06002639 RID: 9785 RVA: 0x0010694C File Offset: 0x00104B4C
	private void ShowHandHolds()
	{
		if (this.disabled)
		{
			return;
		}
		foreach (GameObject gameObject in this.handHoldObjects)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04002A28 RID: 10792
	[SerializeField]
	private bool autoLoginWithPlatformCredentials = true;

	// Token: 0x04002A29 RID: 10793
	[SerializeField]
	private short teleporterIndex;

	// Token: 0x04002A2A RID: 10794
	[SerializeField]
	private float stayInTriggerDuration = 3f;

	// Token: 0x04002A2B RID: 10795
	[SerializeField]
	private TMP_Text countdownText;

	// Token: 0x04002A2C RID: 10796
	[SerializeField]
	private float maxTeleportProcessingTime = 15f;

	// Token: 0x04002A2D RID: 10797
	[SerializeField]
	private GameObject[] handHoldObjects;

	// Token: 0x04002A2E RID: 10798
	private VRTeleporterSerializer mySerializer;

	// Token: 0x04002A2F RID: 10799
	private bool disabled;

	// Token: 0x04002A30 RID: 10800
	private bool processing;

	// Token: 0x04002A31 RID: 10801
	private float triggerEntryTime = -1f;

	// Token: 0x04002A32 RID: 10802
	public GTZone entrancePoint = GTZone.arcade;
}
