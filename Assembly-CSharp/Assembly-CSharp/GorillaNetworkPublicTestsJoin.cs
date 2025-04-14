using System;
using System.Collections;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007A5 RID: 1957
public class GorillaNetworkPublicTestsJoin : GorillaTriggerBox
{
	// Token: 0x0600304A RID: 12362 RVA: 0x000023F4 File Offset: 0x000005F4
	public void Awake()
	{
	}

	// Token: 0x0600304B RID: 12363 RVA: 0x000E9658 File Offset: 0x000E7858
	public void LateUpdate()
	{
		try
		{
			if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.IsVisible)
			{
				if (GTPlayer.Instance.GetComponent<Rigidbody>().isKinematic && !this.waiting && !GorillaNot.instance.reportedPlayers.Contains(PhotonNetwork.LocalPlayer.UserId))
				{
					base.StartCoroutine(this.GracePeriod());
				}
				if ((GTPlayer.Instance.jumpMultiplier > GorillaGameManager.instance.fastJumpMultiplier * 2f || GTPlayer.Instance.maxJumpSpeed > GorillaGameManager.instance.fastJumpLimit * 2f) && !this.waiting && !GorillaNot.instance.reportedPlayers.Contains(PhotonNetwork.LocalPlayer.UserId))
				{
					base.StartCoroutine(this.GracePeriod());
				}
				float magnitude = (GTPlayer.Instance.transform.position - this.lastPosition).magnitude;
			}
			this.lastPosition = GTPlayer.Instance.transform.position;
		}
		catch
		{
		}
	}

	// Token: 0x0600304C RID: 12364 RVA: 0x000E978C File Offset: 0x000E798C
	private IEnumerator GracePeriod()
	{
		this.waiting = true;
		yield return new WaitForSeconds(30f);
		try
		{
			if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.IsVisible)
			{
				if (GTPlayer.Instance.GetComponent<Rigidbody>().isKinematic)
				{
					GorillaNot.instance.SendReport("gorvity bisdabled", PhotonNetwork.LocalPlayer.UserId, PhotonNetwork.LocalPlayer.NickName);
				}
				if (GTPlayer.Instance.jumpMultiplier > GorillaGameManager.instance.fastJumpMultiplier * 2f || GTPlayer.Instance.maxJumpSpeed > GorillaGameManager.instance.fastJumpLimit * 2f)
				{
					GorillaNot.instance.SendReport(string.Concat(new string[]
					{
						"jimp 2mcuh.",
						GTPlayer.Instance.jumpMultiplier.ToString(),
						".",
						GTPlayer.Instance.maxJumpSpeed.ToString(),
						"."
					}), PhotonNetwork.LocalPlayer.UserId, PhotonNetwork.LocalPlayer.NickName);
				}
				if (GorillaTagger.Instance.sphereCastRadius > 0.04f)
				{
					GorillaNot.instance.SendReport("wack rad. " + GorillaTagger.Instance.sphereCastRadius.ToString(), PhotonNetwork.LocalPlayer.UserId, PhotonNetwork.LocalPlayer.NickName);
				}
			}
			this.waiting = false;
			yield break;
		}
		catch
		{
			yield break;
		}
		yield break;
	}

	// Token: 0x04003442 RID: 13378
	public GameObject[] makeSureThisIsDisabled;

	// Token: 0x04003443 RID: 13379
	public GameObject[] makeSureThisIsEnabled;

	// Token: 0x04003444 RID: 13380
	public string gameModeName;

	// Token: 0x04003445 RID: 13381
	public PhotonNetworkController photonNetworkController;

	// Token: 0x04003446 RID: 13382
	public string componentTypeToAdd;

	// Token: 0x04003447 RID: 13383
	public GameObject componentTarget;

	// Token: 0x04003448 RID: 13384
	public GorillaLevelScreen[] joinScreens;

	// Token: 0x04003449 RID: 13385
	public GorillaLevelScreen[] leaveScreens;

	// Token: 0x0400344A RID: 13386
	private Transform tosPition;

	// Token: 0x0400344B RID: 13387
	private Transform othsTosPosition;

	// Token: 0x0400344C RID: 13388
	private PhotonView fotVew;

	// Token: 0x0400344D RID: 13389
	private bool waiting;

	// Token: 0x0400344E RID: 13390
	private Vector3 lastPosition;

	// Token: 0x0400344F RID: 13391
	private VRRig tempRig;
}
