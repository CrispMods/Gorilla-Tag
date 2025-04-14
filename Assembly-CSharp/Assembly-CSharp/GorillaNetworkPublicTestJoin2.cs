using System;
using System.Collections;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007A3 RID: 1955
public class GorillaNetworkPublicTestJoin2 : GorillaTriggerBox
{
	// Token: 0x06003040 RID: 12352 RVA: 0x000023F4 File Offset: 0x000005F4
	public void Awake()
	{
	}

	// Token: 0x06003041 RID: 12353 RVA: 0x000E9340 File Offset: 0x000E7540
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

	// Token: 0x06003042 RID: 12354 RVA: 0x000E9474 File Offset: 0x000E7674
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

	// Token: 0x04003431 RID: 13361
	public GameObject[] makeSureThisIsDisabled;

	// Token: 0x04003432 RID: 13362
	public GameObject[] makeSureThisIsEnabled;

	// Token: 0x04003433 RID: 13363
	public string gameModeName;

	// Token: 0x04003434 RID: 13364
	public PhotonNetworkController photonNetworkController;

	// Token: 0x04003435 RID: 13365
	public string componentTypeToAdd;

	// Token: 0x04003436 RID: 13366
	public GameObject componentTarget;

	// Token: 0x04003437 RID: 13367
	public GorillaLevelScreen[] joinScreens;

	// Token: 0x04003438 RID: 13368
	public GorillaLevelScreen[] leaveScreens;

	// Token: 0x04003439 RID: 13369
	private Transform tosPition;

	// Token: 0x0400343A RID: 13370
	private Transform othsTosPosition;

	// Token: 0x0400343B RID: 13371
	private PhotonView fotVew;

	// Token: 0x0400343C RID: 13372
	private bool waiting;

	// Token: 0x0400343D RID: 13373
	private Vector3 lastPosition;

	// Token: 0x0400343E RID: 13374
	private VRRig tempRig;
}
