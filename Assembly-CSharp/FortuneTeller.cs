using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000536 RID: 1334
public class FortuneTeller : MonoBehaviourPunCallbacks
{
	// Token: 0x0600204B RID: 8267 RVA: 0x000F28D0 File Offset: 0x000F0AD0
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

	// Token: 0x0600204C RID: 8268 RVA: 0x000F2944 File Offset: 0x000F0B44
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

	// Token: 0x0600204D RID: 8269 RVA: 0x00045F61 File Offset: 0x00044161
	public override void OnEnable()
	{
		base.OnEnable();
		this.nextAttractAnimTimestamp = Time.time + this.waitDurationBeforeAttractAnim;
		if (this.button)
		{
			this.button.onPressed += this.HandlePressedButton;
		}
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x00045F9F File Offset: 0x0004419F
	public override void OnDisable()
	{
		base.OnDisable();
		if (this.button)
		{
			this.button.onPressed -= this.HandlePressedButton;
		}
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x00045FCB File Offset: 0x000441CB
	private void GreyZoneActivated()
	{
		this.boothRenderer.material = this.boothGreyZoneMaterial;
		this.beardRenderer.material = this.beardGreyZoneMaterial;
		this.tellerRenderer.SetMaterials(this.tellerGreyZoneMaterials);
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x00046000 File Offset: 0x00044200
	private void GreyZoneDeactivated()
	{
		this.boothRenderer.material = this.boothDefaultMaterial;
		this.beardRenderer.material = this.beardDefaultMaterial;
		this.tellerRenderer.SetMaterials(this.tellerDefaultMaterials);
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x000F29B0 File Offset: 0x000F0BB0
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

	// Token: 0x06002052 RID: 8274 RVA: 0x00046035 File Offset: 0x00044235
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.StartAttractModeMonitor();
		}
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x00046035 File Offset: 0x00044235
	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.StartAttractModeMonitor();
		}
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x00046044 File Offset: 0x00044244
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

	// Token: 0x06002055 RID: 8277 RVA: 0x000F2A14 File Offset: 0x000F0C14
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

	// Token: 0x06002056 RID: 8278 RVA: 0x000F2A94 File Offset: 0x000F0C94
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

	// Token: 0x06002057 RID: 8279 RVA: 0x000F2B34 File Offset: 0x000F0D34
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

	// Token: 0x06002058 RID: 8280 RVA: 0x000F2BB0 File Offset: 0x000F0DB0
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

	// Token: 0x06002059 RID: 8281 RVA: 0x00046077 File Offset: 0x00044277
	private void StartAttractModeMonitor()
	{
		if (this.attractModeMonitor == null)
		{
			this.attractModeMonitor = base.StartCoroutine(this.AttractModeMonitor());
		}
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x00046093 File Offset: 0x00044293
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

	// Token: 0x0600205B RID: 8283 RVA: 0x000460A2 File Offset: 0x000442A2
	private void SendAttractAnim()
	{
		if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
		{
			base.photonView.RPC("TriggerAttractAnimRPC", RpcTarget.All, Array.Empty<object>());
		}
	}

	// Token: 0x0600205C RID: 8284 RVA: 0x000F2C3C File Offset: 0x000F0E3C
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

	// Token: 0x0600205D RID: 8285 RVA: 0x000F2CB4 File Offset: 0x000F0EB4
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

	// Token: 0x0600205E RID: 8286 RVA: 0x000460C8 File Offset: 0x000442C8
	public void ApplyFortuneText()
	{
		this.text.text = this.results.GetResultText(this.latestFortune).ToUpper();
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x000F2D38 File Offset: 0x000F0F38
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

	// Token: 0x04002455 RID: 9301
	[SerializeField]
	private FXType limiterType;

	// Token: 0x04002456 RID: 9302
	[SerializeField]
	private FortuneTellerButton button;

	// Token: 0x04002457 RID: 9303
	[SerializeField]
	private TextMeshPro text;

	// Token: 0x04002458 RID: 9304
	[SerializeField]
	private FortuneResults results;

	// Token: 0x04002459 RID: 9305
	[SerializeField]
	private PlayableDirector playable;

	// Token: 0x0400245A RID: 9306
	[SerializeField]
	private Animator animator;

	// Token: 0x0400245B RID: 9307
	[SerializeField]
	private float waitDurationBeforeAttractAnim;

	// Token: 0x0400245C RID: 9308
	[SerializeField]
	private FortuneTeller.FortuneTellerResultFanfare[] resultFanfares;

	// Token: 0x0400245D RID: 9309
	[Header("Grey Zone Visuals")]
	[SerializeField]
	private bool changeMaterialsInGreyZone;

	// Token: 0x0400245E RID: 9310
	[SerializeField]
	private MeshRenderer boothRenderer;

	// Token: 0x0400245F RID: 9311
	[SerializeField]
	private Material boothDefaultMaterial;

	// Token: 0x04002460 RID: 9312
	[SerializeField]
	private Material boothGreyZoneMaterial;

	// Token: 0x04002461 RID: 9313
	[SerializeField]
	private MeshRenderer beardRenderer;

	// Token: 0x04002462 RID: 9314
	[SerializeField]
	private Material beardDefaultMaterial;

	// Token: 0x04002463 RID: 9315
	[SerializeField]
	private Material beardGreyZoneMaterial;

	// Token: 0x04002464 RID: 9316
	[SerializeField]
	private SkinnedMeshRenderer tellerRenderer;

	// Token: 0x04002465 RID: 9317
	[SerializeField]
	private List<Material> tellerDefaultMaterials;

	// Token: 0x04002466 RID: 9318
	[SerializeField]
	private List<Material> tellerGreyZoneMaterials;

	// Token: 0x04002467 RID: 9319
	private FortuneResults.FortuneResult latestFortune;

	// Token: 0x04002468 RID: 9320
	private CallLimiter triggerNewFortuneLimiter = new CallLimiter(10, 1f, 0.5f);

	// Token: 0x04002469 RID: 9321
	private CallLimiter triggerUpdateFortuneLimiter = new CallLimiter(10, 1f, 0.5f);

	// Token: 0x0400246A RID: 9322
	private AnimHashId trigger_attract = "Attract";

	// Token: 0x0400246B RID: 9323
	private AnimHashId trigger_prediction = "Prediction";

	// Token: 0x0400246C RID: 9324
	private float nextAttractAnimTimestamp;

	// Token: 0x0400246D RID: 9325
	private Coroutine attractModeMonitor;

	// Token: 0x02000537 RID: 1335
	[Serializable]
	public struct FortuneTellerResultFanfare
	{
		// Token: 0x0400246E RID: 9326
		public FortuneResults.FortuneCategoryType type;

		// Token: 0x0400246F RID: 9327
		public PlayableAsset fanfare;
	}
}
