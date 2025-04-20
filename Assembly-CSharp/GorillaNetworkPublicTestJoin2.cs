using System;
using System.Collections;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007BA RID: 1978
public class GorillaNetworkPublicTestJoin2 : GorillaTriggerBox
{
	// Token: 0x060030EA RID: 12522 RVA: 0x00030607 File Offset: 0x0002E807
	public void Awake()
	{
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x0013236C File Offset: 0x0013056C
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

	// Token: 0x060030EC RID: 12524 RVA: 0x00050616 File Offset: 0x0004E816
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

	// Token: 0x040034D5 RID: 13525
	public GameObject[] makeSureThisIsDisabled;

	// Token: 0x040034D6 RID: 13526
	public GameObject[] makeSureThisIsEnabled;

	// Token: 0x040034D7 RID: 13527
	public string gameModeName;

	// Token: 0x040034D8 RID: 13528
	public PhotonNetworkController photonNetworkController;

	// Token: 0x040034D9 RID: 13529
	public string componentTypeToAdd;

	// Token: 0x040034DA RID: 13530
	public GameObject componentTarget;

	// Token: 0x040034DB RID: 13531
	public GorillaLevelScreen[] joinScreens;

	// Token: 0x040034DC RID: 13532
	public GorillaLevelScreen[] leaveScreens;

	// Token: 0x040034DD RID: 13533
	private Transform tosPition;

	// Token: 0x040034DE RID: 13534
	private Transform othsTosPosition;

	// Token: 0x040034DF RID: 13535
	private PhotonView fotVew;

	// Token: 0x040034E0 RID: 13536
	private bool waiting;

	// Token: 0x040034E1 RID: 13537
	private Vector3 lastPosition;

	// Token: 0x040034E2 RID: 13538
	private VRRig tempRig;
}
