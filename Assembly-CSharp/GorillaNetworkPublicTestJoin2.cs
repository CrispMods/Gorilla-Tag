using System;
using System.Collections;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007A2 RID: 1954
public class GorillaNetworkPublicTestJoin2 : GorillaTriggerBox
{
	// Token: 0x06003038 RID: 12344 RVA: 0x000023F4 File Offset: 0x000005F4
	public void Awake()
	{
	}

	// Token: 0x06003039 RID: 12345 RVA: 0x000E8EC0 File Offset: 0x000E70C0
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

	// Token: 0x0600303A RID: 12346 RVA: 0x000E8FF4 File Offset: 0x000E71F4
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

	// Token: 0x0400342B RID: 13355
	public GameObject[] makeSureThisIsDisabled;

	// Token: 0x0400342C RID: 13356
	public GameObject[] makeSureThisIsEnabled;

	// Token: 0x0400342D RID: 13357
	public string gameModeName;

	// Token: 0x0400342E RID: 13358
	public PhotonNetworkController photonNetworkController;

	// Token: 0x0400342F RID: 13359
	public string componentTypeToAdd;

	// Token: 0x04003430 RID: 13360
	public GameObject componentTarget;

	// Token: 0x04003431 RID: 13361
	public GorillaLevelScreen[] joinScreens;

	// Token: 0x04003432 RID: 13362
	public GorillaLevelScreen[] leaveScreens;

	// Token: 0x04003433 RID: 13363
	private Transform tosPition;

	// Token: 0x04003434 RID: 13364
	private Transform othsTosPosition;

	// Token: 0x04003435 RID: 13365
	private PhotonView fotVew;

	// Token: 0x04003436 RID: 13366
	private bool waiting;

	// Token: 0x04003437 RID: 13367
	private Vector3 lastPosition;

	// Token: 0x04003438 RID: 13368
	private VRRig tempRig;
}
