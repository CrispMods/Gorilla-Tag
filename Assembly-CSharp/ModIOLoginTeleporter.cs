using System;
using System.Collections;
using GorillaNetworking;
using GorillaTagScripts.ModIO;
using KID.Model;
using TMPro;
using UnityEngine;

// Token: 0x02000601 RID: 1537
public class ModIOLoginTeleporter : MonoBehaviour, IBuildValidation
{
	// Token: 0x06002622 RID: 9762 RVA: 0x000BC06B File Offset: 0x000BA26B
	public bool BuildValidationCheck()
	{
		if (this.mySerializer == null && base.transform.parent.parent.GetComponentInChildren<VRTeleporterSerializer>() == null)
		{
			Debug.LogError("This teleporter needs a reference to a VRTeleporterSerializer, or to be placed alongside a VRTeleporterSerializer. Check out the arcade or the stump", this);
			return false;
		}
		return true;
	}

	// Token: 0x06002623 RID: 9763 RVA: 0x000BC0A8 File Offset: 0x000BA2A8
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

	// Token: 0x06002624 RID: 9764 RVA: 0x000BC177 File Offset: 0x000BA377
	private void Disable()
	{
		this.HideHandHolds();
		this.disabled = true;
	}

	// Token: 0x06002625 RID: 9765 RVA: 0x000BC188 File Offset: 0x000BA388
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

	// Token: 0x06002626 RID: 9766 RVA: 0x000BC1DC File Offset: 0x000BA3DC
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

	// Token: 0x06002627 RID: 9767 RVA: 0x000BC249 File Offset: 0x000BA449
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

	// Token: 0x06002628 RID: 9768 RVA: 0x000BC284 File Offset: 0x000BA484
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

	// Token: 0x06002629 RID: 9769 RVA: 0x000BC2D9 File Offset: 0x000BA4D9
	private void HideCountdownText()
	{
		if (this.countdownText != null)
		{
			this.countdownText.text = "";
			this.countdownText.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600262A RID: 9770 RVA: 0x000BC30C File Offset: 0x000BA50C
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

	// Token: 0x0600262B RID: 9771 RVA: 0x000BC35F File Offset: 0x000BA55F
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

	// Token: 0x0600262C RID: 9772 RVA: 0x000BC39A File Offset: 0x000BA59A
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

	// Token: 0x0600262D RID: 9773 RVA: 0x000BC3BC File Offset: 0x000BA5BC
	private void HandleTeleport()
	{
		if (this.disabled)
		{
			return;
		}
		base.StartCoroutine(CustomMapManager.TeleportToVirtualStump(this.teleporterIndex, new Action<bool>(this.FinishTeleport), this.entrancePoint, this.mySerializer));
		base.StartCoroutine(this.DelayedFinishTeleport());
	}

	// Token: 0x0600262E RID: 9774 RVA: 0x000BC409 File Offset: 0x000BA609
	private IEnumerator DelayedFinishTeleport()
	{
		yield return new WaitForSecondsRealtime(this.maxTeleportProcessingTime);
		this.FinishTeleport(true);
		yield break;
	}

	// Token: 0x0600262F RID: 9775 RVA: 0x000BC418 File Offset: 0x000BA618
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

	// Token: 0x06002630 RID: 9776 RVA: 0x000BC44C File Offset: 0x000BA64C
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

	// Token: 0x06002631 RID: 9777 RVA: 0x000BC48C File Offset: 0x000BA68C
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

	// Token: 0x04002A22 RID: 10786
	[SerializeField]
	private bool autoLoginWithPlatformCredentials = true;

	// Token: 0x04002A23 RID: 10787
	[SerializeField]
	private short teleporterIndex;

	// Token: 0x04002A24 RID: 10788
	[SerializeField]
	private float stayInTriggerDuration = 3f;

	// Token: 0x04002A25 RID: 10789
	[SerializeField]
	private TMP_Text countdownText;

	// Token: 0x04002A26 RID: 10790
	[SerializeField]
	private float maxTeleportProcessingTime = 15f;

	// Token: 0x04002A27 RID: 10791
	[SerializeField]
	private GameObject[] handHoldObjects;

	// Token: 0x04002A28 RID: 10792
	private VRTeleporterSerializer mySerializer;

	// Token: 0x04002A29 RID: 10793
	private bool disabled;

	// Token: 0x04002A2A RID: 10794
	private bool processing;

	// Token: 0x04002A2B RID: 10795
	private float triggerEntryTime = -1f;

	// Token: 0x04002A2C RID: 10796
	public GTZone entrancePoint = GTZone.arcade;
}
