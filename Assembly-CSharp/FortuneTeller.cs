using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000529 RID: 1321
public class FortuneTeller : MonoBehaviourPunCallbacks
{
	// Token: 0x06001FF2 RID: 8178 RVA: 0x000A12A8 File Offset: 0x0009F4A8
	private void Awake()
	{
		if (this.changeMaterialsInGreyZone && GreyZoneManager.Instance != null)
		{
			GreyZoneManager instance = GreyZoneManager.Instance;
			instance.OnGreyZoneActivated = (Action)Delegate.Combine(instance.OnGreyZoneActivated, new Action(this.GreyZoneActivated));
			GreyZoneManager instance2 = GreyZoneManager.Instance;
			instance2.OnGreyZoneDeactivated = (Action)Delegate.Combine(instance2.OnGreyZoneDeactivated, new Action(this.GreyZoneDeactivated));
		}
	}

	// Token: 0x06001FF3 RID: 8179 RVA: 0x000A131C File Offset: 0x0009F51C
	private void OnDestroy()
	{
		if (GreyZoneManager.Instance != null)
		{
			GreyZoneManager instance = GreyZoneManager.Instance;
			instance.OnGreyZoneActivated = (Action)Delegate.Remove(instance.OnGreyZoneActivated, new Action(this.GreyZoneActivated));
			GreyZoneManager instance2 = GreyZoneManager.Instance;
			instance2.OnGreyZoneDeactivated = (Action)Delegate.Remove(instance2.OnGreyZoneDeactivated, new Action(this.GreyZoneDeactivated));
		}
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x000A1388 File Offset: 0x0009F588
	public override void OnEnable()
	{
		base.OnEnable();
		this.nextAttractAnimTimestamp = Time.time + this.waitDurationBeforeAttractAnim;
		if (this.button)
		{
			this.button.onPressed += this.HandlePressedButton;
		}
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x000A13C6 File Offset: 0x0009F5C6
	public override void OnDisable()
	{
		base.OnDisable();
		if (this.button)
		{
			this.button.onPressed -= this.HandlePressedButton;
		}
	}

	// Token: 0x06001FF6 RID: 8182 RVA: 0x000A13F2 File Offset: 0x0009F5F2
	private void GreyZoneActivated()
	{
		this.boothRenderer.material = this.boothGreyZoneMaterial;
		this.beardRenderer.material = this.beardGreyZoneMaterial;
		this.tellerRenderer.SetMaterials(this.tellerGreyZoneMaterials);
	}

	// Token: 0x06001FF7 RID: 8183 RVA: 0x000A1427 File Offset: 0x0009F627
	private void GreyZoneDeactivated()
	{
		this.boothRenderer.material = this.boothDefaultMaterial;
		this.beardRenderer.material = this.beardDefaultMaterial;
		this.tellerRenderer.SetMaterials(this.tellerDefaultMaterials);
	}

	// Token: 0x06001FF8 RID: 8184 RVA: 0x000A145C File Offset: 0x0009F65C
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);
		if (PhotonNetwork.InRoom && PhotonNetwork.LocalPlayer.IsMasterClient)
		{
			base.photonView.RPC("TriggerUpdateFortuneRPC", newPlayer, new object[]
			{
				(int)this.latestFortune.fortuneType,
				this.latestFortune.resultIndex
			});
		}
	}

	// Token: 0x06001FF9 RID: 8185 RVA: 0x000A14C0 File Offset: 0x0009F6C0
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.StartAttractModeMonitor();
		}
	}

	// Token: 0x06001FFA RID: 8186 RVA: 0x000A14C0 File Offset: 0x0009F6C0
	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.StartAttractModeMonitor();
		}
	}

	// Token: 0x06001FFB RID: 8187 RVA: 0x000A14CF File Offset: 0x0009F6CF
	private void HandlePressedButton(GorillaPressableButton button, bool isLeft)
	{
		if (base.photonView.IsMine)
		{
			this.SendNewFortune();
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			base.photonView.RPC("RequestFortuneRPC", RpcTarget.MasterClient, Array.Empty<object>());
		}
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x000A1504 File Offset: 0x0009F704
	[PunRPC]
	private void RequestFortuneRPC(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RequestFortune");
		RigContainer rigContainer;
		if (info.Sender != null && VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
		{
			CallLimitType<CallLimiter> callLimitType = rigContainer.Rig.fxSettings.callSettings[(int)this.limiterType];
			if (callLimitType.UseNetWorkTime ? callLimitType.CallLimitSettings.CheckCallServerTime(info.SentServerTime) : callLimitType.CallLimitSettings.CheckCallTime(Time.time))
			{
				this.SendNewFortune();
			}
		}
	}

	// Token: 0x06001FFD RID: 8189 RVA: 0x000A1584 File Offset: 0x0009F784
	private void SendNewFortune()
	{
		if (this.playable.time > 0.0 && this.playable.time < this.playable.duration)
		{
			return;
		}
		this.latestFortune = this.results.GetResult();
		this.UpdateFortune(this.latestFortune, true);
		if (PhotonNetwork.InRoom)
		{
			base.photonView.RPC("TriggerNewFortuneRPC", RpcTarget.Others, new object[]
			{
				(int)this.latestFortune.fortuneType,
				this.latestFortune.resultIndex
			});
		}
	}

	// Token: 0x06001FFE RID: 8190 RVA: 0x000A1624 File Offset: 0x0009F824
	[PunRPC]
	private void TriggerUpdateFortuneRPC(int fortuneType, int resultIndex, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "TriggerUpdateFortune");
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			GorillaNot.instance.SendReport("Sent TriggerUpdateFortune when they weren't the master client", info.Sender.UserId, info.Sender.NickName);
			return;
		}
		if (!this.triggerUpdateFortuneLimiter.CheckCallTime(Time.time))
		{
			return;
		}
		this.latestFortune = new FortuneResults.FortuneResult((FortuneResults.FortuneCategoryType)fortuneType, resultIndex);
		this.UpdateFortune(this.latestFortune, false);
	}

	// Token: 0x06001FFF RID: 8191 RVA: 0x000A16A0 File Offset: 0x0009F8A0
	[PunRPC]
	private void TriggerNewFortuneRPC(int fortuneType, int resultIndex, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "TriggerNewFortune");
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			GorillaNot.instance.SendReport("Sent TriggerNewFortune when they weren't the master client", info.Sender.UserId, info.Sender.NickName);
			return;
		}
		if (!this.triggerNewFortuneLimiter.CheckCallTime(Time.time))
		{
			return;
		}
		this.latestFortune = new FortuneResults.FortuneResult((FortuneResults.FortuneCategoryType)fortuneType, resultIndex);
		this.nextAttractAnimTimestamp = Time.time + this.waitDurationBeforeAttractAnim;
		this.UpdateFortune(this.latestFortune, true);
	}

	// Token: 0x06002000 RID: 8192 RVA: 0x000A172C File Offset: 0x0009F92C
	private void StartAttractModeMonitor()
	{
		if (this.attractModeMonitor == null)
		{
			this.attractModeMonitor = base.StartCoroutine(this.AttractModeMonitor());
		}
	}

	// Token: 0x06002001 RID: 8193 RVA: 0x000A1748 File Offset: 0x0009F948
	private IEnumerator AttractModeMonitor()
	{
		while (PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom)
		{
			if (Time.time >= this.nextAttractAnimTimestamp)
			{
				this.SendAttractAnim();
			}
			yield return new WaitForSeconds(this.nextAttractAnimTimestamp - Time.time);
		}
		this.attractModeMonitor = null;
		yield break;
	}

	// Token: 0x06002002 RID: 8194 RVA: 0x000A1757 File Offset: 0x0009F957
	private void SendAttractAnim()
	{
		if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
		{
			base.photonView.RPC("TriggerAttractAnimRPC", RpcTarget.All, Array.Empty<object>());
		}
	}

	// Token: 0x06002003 RID: 8195 RVA: 0x000A1780 File Offset: 0x0009F980
	[PunRPC]
	private void TriggerAttractAnimRPC(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "TriggerAttractAnim");
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			GorillaNot.instance.SendReport("Sent TriggerAttractAnim when they weren't the master client", info.Sender.UserId, info.Sender.NickName);
			return;
		}
		this.animator.SetTrigger(this.trigger_attract);
		this.nextAttractAnimTimestamp = Time.time + this.waitDurationBeforeAttractAnim;
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x000A17F8 File Offset: 0x0009F9F8
	private void UpdateFortune(FortuneResults.FortuneResult result, bool newFortune)
	{
		if (this.results)
		{
			PlayableAsset resultFanfare = this.GetResultFanfare(result.fortuneType);
			if (resultFanfare)
			{
				this.playable.initialTime = (newFortune ? 0.0 : resultFanfare.duration);
				this.playable.Play(resultFanfare, DirectorWrapMode.Hold);
				this.animator.SetTrigger(this.trigger_prediction);
				this.nextAttractAnimTimestamp = Time.time + this.waitDurationBeforeAttractAnim;
			}
		}
	}

	// Token: 0x06002005 RID: 8197 RVA: 0x000A187B File Offset: 0x0009FA7B
	public void ApplyFortuneText()
	{
		this.text.text = this.results.GetResultText(this.latestFortune).ToUpper();
	}

	// Token: 0x06002006 RID: 8198 RVA: 0x000A18A0 File Offset: 0x0009FAA0
	private PlayableAsset GetResultFanfare(FortuneResults.FortuneCategoryType fortuneType)
	{
		foreach (FortuneTeller.FortuneTellerResultFanfare fortuneTellerResultFanfare in this.resultFanfares)
		{
			if (fortuneTellerResultFanfare.type == fortuneType)
			{
				return fortuneTellerResultFanfare.fanfare;
			}
		}
		return null;
	}

	// Token: 0x04002402 RID: 9218
	[SerializeField]
	private FXType limiterType;

	// Token: 0x04002403 RID: 9219
	[SerializeField]
	private FortuneTellerButton button;

	// Token: 0x04002404 RID: 9220
	[SerializeField]
	private TextMeshPro text;

	// Token: 0x04002405 RID: 9221
	[SerializeField]
	private FortuneResults results;

	// Token: 0x04002406 RID: 9222
	[SerializeField]
	private PlayableDirector playable;

	// Token: 0x04002407 RID: 9223
	[SerializeField]
	private Animator animator;

	// Token: 0x04002408 RID: 9224
	[SerializeField]
	private float waitDurationBeforeAttractAnim;

	// Token: 0x04002409 RID: 9225
	[SerializeField]
	private FortuneTeller.FortuneTellerResultFanfare[] resultFanfares;

	// Token: 0x0400240A RID: 9226
	[Header("Grey Zone Visuals")]
	[SerializeField]
	private bool changeMaterialsInGreyZone;

	// Token: 0x0400240B RID: 9227
	[SerializeField]
	private MeshRenderer boothRenderer;

	// Token: 0x0400240C RID: 9228
	[SerializeField]
	private Material boothDefaultMaterial;

	// Token: 0x0400240D RID: 9229
	[SerializeField]
	private Material boothGreyZoneMaterial;

	// Token: 0x0400240E RID: 9230
	[SerializeField]
	private MeshRenderer beardRenderer;

	// Token: 0x0400240F RID: 9231
	[SerializeField]
	private Material beardDefaultMaterial;

	// Token: 0x04002410 RID: 9232
	[SerializeField]
	private Material beardGreyZoneMaterial;

	// Token: 0x04002411 RID: 9233
	[SerializeField]
	private SkinnedMeshRenderer tellerRenderer;

	// Token: 0x04002412 RID: 9234
	[SerializeField]
	private List<Material> tellerDefaultMaterials;

	// Token: 0x04002413 RID: 9235
	[SerializeField]
	private List<Material> tellerGreyZoneMaterials;

	// Token: 0x04002414 RID: 9236
	private FortuneResults.FortuneResult latestFortune;

	// Token: 0x04002415 RID: 9237
	private CallLimiter triggerNewFortuneLimiter = new CallLimiter(10, 1f, 0.5f);

	// Token: 0x04002416 RID: 9238
	private CallLimiter triggerUpdateFortuneLimiter = new CallLimiter(10, 1f, 0.5f);

	// Token: 0x04002417 RID: 9239
	private AnimHashId trigger_attract = "Attract";

	// Token: 0x04002418 RID: 9240
	private AnimHashId trigger_prediction = "Prediction";

	// Token: 0x04002419 RID: 9241
	private float nextAttractAnimTimestamp;

	// Token: 0x0400241A RID: 9242
	private Coroutine attractModeMonitor;

	// Token: 0x0200052A RID: 1322
	[Serializable]
	public struct FortuneTellerResultFanfare
	{
		// Token: 0x0400241B RID: 9243
		public FortuneResults.FortuneCategoryType type;

		// Token: 0x0400241C RID: 9244
		public PlayableAsset fanfare;
	}
}
