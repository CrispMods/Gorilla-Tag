using System;
using System.Collections;
using GorillaLocomotion;
using Oculus.Platform;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

// Token: 0x02000375 RID: 885
public class GorillaMetaReport : MonoBehaviour
{
	// Token: 0x17000247 RID: 583
	// (get) Token: 0x0600148D RID: 5261 RVA: 0x0003DDE0 File Offset: 0x0003BFE0
	private GTPlayer localPlayer
	{
		get
		{
			return GTPlayer.Instance;
		}
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x0003DDE7 File Offset: 0x0003BFE7
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

	// Token: 0x0600148F RID: 5263 RVA: 0x0003DE20 File Offset: 0x0003C020
	private void OnDisable()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		this.localPlayer.inOverlay = false;
		base.StopAllCoroutines();
	}

	// Token: 0x06001490 RID: 5264 RVA: 0x000BC478 File Offset: 0x000BA678
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

	// Token: 0x06001491 RID: 5265 RVA: 0x0003DE3C File Offset: 0x0003C03C
	private IEnumerator Submitted()
	{
		yield return new WaitForSeconds(1.5f);
		this.Teardown();
		yield break;
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x000BC4CC File Offset: 0x000BA6CC
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

	// Token: 0x06001493 RID: 5267 RVA: 0x000BC538 File Offset: 0x000BA738
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

	// Token: 0x06001494 RID: 5268 RVA: 0x000BC584 File Offset: 0x000BA784
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

	// Token: 0x06001495 RID: 5269 RVA: 0x000BC660 File Offset: 0x000BA860
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

	// Token: 0x06001496 RID: 5270 RVA: 0x000BC714 File Offset: 0x000BA914
	private void GetIdealScreenPositionRotation(out Vector3 position, out Quaternion rotation, out Vector3 scale)
	{
		GameObject mainCamera = GorillaTagger.Instance.mainCamera;
		rotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
		scale = this.localPlayer.turnParent.transform.localScale;
		position = mainCamera.transform.position + rotation * this.playerLocalScreenPosition * scale.x;
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x000BC7A0 File Offset: 0x000BA9A0
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

	// Token: 0x06001498 RID: 5272 RVA: 0x000BC904 File Offset: 0x000BAB04
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

	// Token: 0x06001499 RID: 5273 RVA: 0x000BCA24 File Offset: 0x000BAC24
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

	// Token: 0x040016AC RID: 5804
	[SerializeField]
	private GameObject occluder;

	// Token: 0x040016AD RID: 5805
	[SerializeField]
	private GameObject reportScoreboard;

	// Token: 0x040016AE RID: 5806
	[SerializeField]
	private GameObject ReportText;

	// Token: 0x040016AF RID: 5807
	[SerializeField]
	private LayerMask visibleLayers;

	// Token: 0x040016B0 RID: 5808
	[SerializeField]
	private GorillaReportButton closeButton;

	// Token: 0x040016B1 RID: 5809
	[SerializeField]
	private GameObject leftHandObject;

	// Token: 0x040016B2 RID: 5810
	[SerializeField]
	private GameObject rightHandObject;

	// Token: 0x040016B3 RID: 5811
	[SerializeField]
	private Vector3 playerLocalScreenPosition;

	// Token: 0x040016B4 RID: 5812
	private float blockButtonsUntilTimestamp;

	// Token: 0x040016B5 RID: 5813
	[SerializeField]
	private GorillaScoreBoard currentScoreboard;

	// Token: 0x040016B6 RID: 5814
	private int savedCullingLayers;

	// Token: 0x040016B7 RID: 5815
	public bool testPress;

	// Token: 0x040016B8 RID: 5816
	public bool isMoving;

	// Token: 0x040016B9 RID: 5817
	private float movementTime;
}
