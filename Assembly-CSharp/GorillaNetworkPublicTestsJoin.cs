using System;
using System.Collections;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007BC RID: 1980
public class GorillaNetworkPublicTestsJoin : GorillaTriggerBox
{
	// Token: 0x060030F4 RID: 12532 RVA: 0x00030607 File Offset: 0x0002E807
	public void Awake()
	{
	}

	// Token: 0x060030F5 RID: 12533 RVA: 0x0013265C File Offset: 0x0013085C
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

	// Token: 0x060030F6 RID: 12534 RVA: 0x0005063C File Offset: 0x0004E83C
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

	// Token: 0x040034E6 RID: 13542
	public GameObject[] makeSureThisIsDisabled;

	// Token: 0x040034E7 RID: 13543
	public GameObject[] makeSureThisIsEnabled;

	// Token: 0x040034E8 RID: 13544
	public string gameModeName;

	// Token: 0x040034E9 RID: 13545
	public PhotonNetworkController photonNetworkController;

	// Token: 0x040034EA RID: 13546
	public string componentTypeToAdd;

	// Token: 0x040034EB RID: 13547
	public GameObject componentTarget;

	// Token: 0x040034EC RID: 13548
	public GorillaLevelScreen[] joinScreens;

	// Token: 0x040034ED RID: 13549
	public GorillaLevelScreen[] leaveScreens;

	// Token: 0x040034EE RID: 13550
	private Transform tosPition;

	// Token: 0x040034EF RID: 13551
	private Transform othsTosPosition;

	// Token: 0x040034F0 RID: 13552
	private PhotonView fotVew;

	// Token: 0x040034F1 RID: 13553
	private bool waiting;

	// Token: 0x040034F2 RID: 13554
	private Vector3 lastPosition;

	// Token: 0x040034F3 RID: 13555
	private VRRig tempRig;
}
