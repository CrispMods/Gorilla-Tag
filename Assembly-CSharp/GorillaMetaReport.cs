using System;
using System.Collections;
using GorillaLocomotion;
using Oculus.Platform;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

// Token: 0x0200036A RID: 874
public class GorillaMetaReport : MonoBehaviour
{
	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06001441 RID: 5185 RVA: 0x00063159 File Offset: 0x00061359
	private GTPlayer localPlayer
	{
		get
		{
			return GTPlayer.Instance;
		}
	}

	// Token: 0x06001442 RID: 5186 RVA: 0x00063160 File Offset: 0x00061360
	private void Start()
	{
		this.localPlayer.inOverlay = false;
		if (!Core.IsInitialized())
		{
			Core.AsyncInitialize(null);
		}
		AbuseReport.SetReportButtonPressedNotificationCallback(new Message<string>.Callback(this.OnReportButtonIntentNotif));
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001443 RID: 5187 RVA: 0x00063199 File Offset: 0x00061399
	private void OnDisable()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		this.localPlayer.inOverlay = false;
		base.StopAllCoroutines();
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x000631B8 File Offset: 0x000613B8
	private void OnReportButtonIntentNotif(Message<string> message)
	{
		if (message.IsError)
		{
			AbuseReport.ReportRequestHandled(ReportRequestResponse.Unhandled);
			return;
		}
		if (!PhotonNetwork.InRoom)
		{
			this.ReportText.SetActive(true);
			AbuseReport.ReportRequestHandled(ReportRequestResponse.Handled);
			this.StartOverlay();
			return;
		}
		if (!message.IsError)
		{
			AbuseReport.ReportRequestHandled(ReportRequestResponse.Handled);
			this.StartOverlay();
		}
	}

	// Token: 0x06001445 RID: 5189 RVA: 0x0006320B File Offset: 0x0006140B
	private IEnumerator Submitted()
	{
		yield return new WaitForSeconds(1.5f);
		this.Teardown();
		yield break;
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x0006321C File Offset: 0x0006141C
	private void DuplicateScoreboard()
	{
		this.currentScoreboard.gameObject.SetActive(true);
		if (GorillaScoreboardTotalUpdater.instance != null)
		{
			GorillaScoreboardTotalUpdater.instance.UpdateScoreboard(this.currentScoreboard);
		}
		Vector3 position;
		Quaternion rotation;
		Vector3 vector;
		this.GetIdealScreenPositionRotation(out position, out rotation, out vector);
		this.currentScoreboard.transform.SetPositionAndRotation(position, rotation);
		this.reportScoreboard.transform.SetPositionAndRotation(position, rotation);
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x00063288 File Offset: 0x00061488
	private void ToggleLevelVisibility(bool state)
	{
		Camera component = GorillaTagger.Instance.mainCamera.GetComponent<Camera>();
		if (state)
		{
			component.cullingMask = this.savedCullingLayers;
			return;
		}
		this.savedCullingLayers = component.cullingMask;
		component.cullingMask = this.visibleLayers;
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x000632D4 File Offset: 0x000614D4
	private void Teardown()
	{
		this.ReportText.GetComponent<Text>().text = "NOT CURRENTLY CONNECTED TO A ROOM";
		this.ReportText.SetActive(false);
		this.localPlayer.inOverlay = false;
		this.localPlayer.disableMovement = false;
		this.closeButton.selected = false;
		this.closeButton.isOn = false;
		this.closeButton.UpdateColor();
		this.localPlayer.InReportMenu = false;
		this.ToggleLevelVisibility(true);
		base.gameObject.SetActive(false);
		foreach (GorillaPlayerScoreboardLine gorillaPlayerScoreboardLine in this.currentScoreboard.lines)
		{
			gorillaPlayerScoreboardLine.doneReporting = false;
		}
		GorillaScoreboardTotalUpdater.instance.UpdateActiveScoreboards();
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x000633B0 File Offset: 0x000615B0
	private void CheckReportSubmit()
	{
		if (this.currentScoreboard == null)
		{
			return;
		}
		foreach (GorillaPlayerScoreboardLine gorillaPlayerScoreboardLine in this.currentScoreboard.lines)
		{
			if (gorillaPlayerScoreboardLine.doneReporting)
			{
				this.ReportText.SetActive(true);
				this.ReportText.GetComponent<Text>().text = "REPORTED " + gorillaPlayerScoreboardLine.playerNameVisible;
				this.currentScoreboard.gameObject.SetActive(false);
				base.StartCoroutine(this.Submitted());
			}
		}
	}

	// Token: 0x0600144A RID: 5194 RVA: 0x00063464 File Offset: 0x00061664
	private void GetIdealScreenPositionRotation(out Vector3 position, out Quaternion rotation, out Vector3 scale)
	{
		GameObject mainCamera = GorillaTagger.Instance.mainCamera;
		rotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
		scale = this.localPlayer.turnParent.transform.localScale;
		position = mainCamera.transform.position + rotation * this.playerLocalScreenPosition * scale.x;
	}

	// Token: 0x0600144B RID: 5195 RVA: 0x000634F0 File Offset: 0x000616F0
	private void StartOverlay()
	{
		Vector3 position;
		Quaternion rotation;
		Vector3 vector;
		this.GetIdealScreenPositionRotation(out position, out rotation, out vector);
		this.currentScoreboard.transform.localScale = vector * 2f;
		this.reportScoreboard.transform.localScale = vector;
		this.leftHandObject.transform.localScale = vector;
		this.rightHandObject.transform.localScale = vector;
		this.occluder.transform.localScale = vector;
		if (this.localPlayer.InReportMenu && !PhotonNetwork.InRoom)
		{
			return;
		}
		this.localPlayer.InReportMenu = true;
		this.localPlayer.disableMovement = true;
		this.localPlayer.inOverlay = true;
		base.gameObject.SetActive(true);
		if (PhotonNetwork.InRoom)
		{
			this.DuplicateScoreboard();
		}
		else
		{
			this.ReportText.SetActive(true);
			this.reportScoreboard.transform.SetPositionAndRotation(position, rotation);
			this.currentScoreboard.transform.SetPositionAndRotation(position, rotation);
		}
		this.ToggleLevelVisibility(false);
		this.rightHandObject.transform.SetPositionAndRotation(this.localPlayer.rightControllerTransform.position, this.localPlayer.rightControllerTransform.rotation);
		this.leftHandObject.transform.SetPositionAndRotation(this.localPlayer.leftControllerTransform.position, this.localPlayer.leftControllerTransform.rotation);
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x00063654 File Offset: 0x00061854
	private void CheckDistance()
	{
		Vector3 b;
		Quaternion b2;
		Vector3 vector;
		this.GetIdealScreenPositionRotation(out b, out b2, out vector);
		float num = Vector3.Distance(this.reportScoreboard.transform.position, b);
		float num2 = 1f;
		if (num > num2 && !this.isMoving)
		{
			this.isMoving = true;
			this.movementTime = 0f;
		}
		if (this.isMoving)
		{
			this.movementTime += Time.deltaTime;
			float num3 = this.movementTime;
			this.reportScoreboard.transform.SetPositionAndRotation(Vector3.Lerp(this.reportScoreboard.transform.position, b, num3), Quaternion.Lerp(this.reportScoreboard.transform.rotation, b2, num3));
			if (this.currentScoreboard != null)
			{
				this.currentScoreboard.transform.SetPositionAndRotation(Vector3.Lerp(this.currentScoreboard.transform.position, b, num3), Quaternion.Lerp(this.currentScoreboard.transform.rotation, b2, num3));
			}
			if (num3 >= 1f)
			{
				this.isMoving = false;
				this.movementTime = 0f;
			}
		}
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x00063774 File Offset: 0x00061974
	private void Update()
	{
		if (this.blockButtonsUntilTimestamp > Time.time)
		{
			return;
		}
		if (SteamVR_Actions.gorillaTag_System.GetState(SteamVR_Input_Sources.LeftHand) && this.localPlayer.InReportMenu)
		{
			this.Teardown();
			this.blockButtonsUntilTimestamp = Time.time + 0.75f;
		}
		if (this.localPlayer.InReportMenu)
		{
			this.localPlayer.inOverlay = true;
			this.occluder.transform.position = GorillaTagger.Instance.mainCamera.transform.position;
			this.rightHandObject.transform.SetPositionAndRotation(this.localPlayer.rightControllerTransform.position, this.localPlayer.rightControllerTransform.rotation);
			this.leftHandObject.transform.SetPositionAndRotation(this.localPlayer.leftControllerTransform.position, this.localPlayer.leftControllerTransform.rotation);
			this.CheckDistance();
			this.CheckReportSubmit();
		}
		if (this.closeButton.selected)
		{
			this.Teardown();
		}
		if (this.testPress)
		{
			this.testPress = false;
			this.StartOverlay();
		}
	}

	// Token: 0x04001664 RID: 5732
	[SerializeField]
	private GameObject occluder;

	// Token: 0x04001665 RID: 5733
	[SerializeField]
	private GameObject reportScoreboard;

	// Token: 0x04001666 RID: 5734
	[SerializeField]
	private GameObject ReportText;

	// Token: 0x04001667 RID: 5735
	[SerializeField]
	private LayerMask visibleLayers;

	// Token: 0x04001668 RID: 5736
	[SerializeField]
	private GorillaReportButton closeButton;

	// Token: 0x04001669 RID: 5737
	[SerializeField]
	private GameObject leftHandObject;

	// Token: 0x0400166A RID: 5738
	[SerializeField]
	private GameObject rightHandObject;

	// Token: 0x0400166B RID: 5739
	[SerializeField]
	private Vector3 playerLocalScreenPosition;

	// Token: 0x0400166C RID: 5740
	private float blockButtonsUntilTimestamp;

	// Token: 0x0400166D RID: 5741
	[SerializeField]
	private GorillaScoreBoard currentScoreboard;

	// Token: 0x0400166E RID: 5742
	private int savedCullingLayers;

	// Token: 0x0400166F RID: 5743
	public bool testPress;

	// Token: 0x04001670 RID: 5744
	public bool isMoving;

	// Token: 0x04001671 RID: 5745
	private float movementTime;
}
