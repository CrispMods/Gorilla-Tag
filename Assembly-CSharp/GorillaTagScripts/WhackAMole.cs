using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using GorillaExtensions;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;

namespace GorillaTagScripts
{
	// Token: 0x02000975 RID: 2421
	[NetworkBehaviourWeaved(210)]
	public class WhackAMole : NetworkComponent
	{
		// Token: 0x06003B10 RID: 15120 RVA: 0x0010F9FC File Offset: 0x0010DBFC
		private void UpdateMeshRendererList()
		{
			List<MeshRenderer> list = new List<MeshRenderer>();
			ZoneBasedObject[] array = this.zoneBasedVisuals;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (MeshRenderer meshRenderer in array[i].GetComponentsInChildren<MeshRenderer>(true))
				{
					if (meshRenderer.enabled)
					{
						list.Add(meshRenderer);
					}
				}
			}
			this.zoneBasedMeshRenderers = list.ToArray();
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x0010FA64 File Offset: 0x0010DC64
		protected override void Awake()
		{
			base.Awake();
			if (this.molesContainerRight != null)
			{
				this.rightMolesList = new List<Mole>(this.molesContainerRight.GetComponentsInChildren<Mole>());
				if (this.rightMolesList.Count > 0)
				{
					this.molesList.AddRange(this.rightMolesList);
				}
			}
			if (this.molesContainerLeft != null)
			{
				this.leftMolesList = new List<Mole>(this.molesContainerLeft.GetComponentsInChildren<Mole>());
				if (this.leftMolesList.Count > 0)
				{
					this.molesList.AddRange(this.leftMolesList);
					foreach (Mole mole in this.leftMolesList)
					{
						mole.IsLeftSideMole = true;
					}
				}
			}
			this.currentLevelIndex = -1;
			foreach (Mole mole2 in this.molesList)
			{
				mole2.OnTapped += this.OnMoleTapped;
			}
			List<Mole> list = this.leftMolesList;
			bool flag;
			if (list != null && list.Count > 0)
			{
				list = this.rightMolesList;
				flag = (list != null && list.Count > 0);
			}
			else
			{
				flag = false;
			}
			this.isMultiplayer = flag;
			this.welcomeUI.SetActive(false);
			this.ongoingGameUI.SetActive(false);
			this.levelEndedUI.SetActive(false);
			this.ContinuePressedUI.SetActive(false);
			this.multiplyareScoresUI.SetActive(false);
			this.bestScore = 0;
			this.bestScoreText.text = string.Empty;
			this.highScorePlayerName = string.Empty;
			this.victoryParticles = this.victoryFX.GetComponentsInChildren<ParticleSystem>();
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x0010FC34 File Offset: 0x0010DE34
		protected override void Start()
		{
			base.Start();
			this.SwitchState(WhackAMole.GameState.Off);
			if (WhackAMoleManager.instance)
			{
				WhackAMoleManager.instance.Register(this);
			}
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x0010FC5C File Offset: 0x0010DE5C
		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			foreach (Mole mole in this.molesList)
			{
				mole.OnTapped -= this.OnMoleTapped;
			}
			if (WhackAMoleManager.instance)
			{
				WhackAMoleManager.instance.Unregister(this);
			}
			this.molesList.Clear();
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x0010FCE0 File Offset: 0x0010DEE0
		public void InvokeUpdate()
		{
			bool isMasterClient = NetworkSystem.Instance.IsMasterClient;
			bool flag = this.zoneBasedVisuals[0].IsLocalPlayerInZone();
			if (isMasterClient != this.wasMasterClient || flag != this.wasLocalPlayerInZone)
			{
				MeshRenderer[] array = this.zoneBasedMeshRenderers;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].enabled = flag;
				}
				bool active = isMasterClient || flag;
				ZoneBasedObject[] array2 = this.zoneBasedVisuals;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].gameObject.SetActive(active);
				}
				this.wasMasterClient = isMasterClient;
				this.wasLocalPlayerInZone = flag;
			}
			if (!NetworkSystem.Instance.InRoom)
			{
				return;
			}
			foreach (Mole mole in this.molesList)
			{
				mole.InvokeUpdate();
			}
			switch (this.currentState)
			{
			case WhackAMole.GameState.ContinuePressed:
				this.counterText.text = (this.countdownDuration - (int)(Time.time - this.continuePressedTime)).ToString();
				if (this.currentLevel != null)
				{
					this.UpdateLevelUI(this.currentLevel.levelNumber);
				}
				if (base.IsMine && Time.time - this.continuePressedTime > (float)this.countdownDuration)
				{
					this.SwitchState(WhackAMole.GameState.LevelStarted);
				}
				break;
			case WhackAMole.GameState.Ongoing:
				if (!this.audioSource.isPlaying && this.backgroundLoop)
				{
					this.audioSource.GTPlayOneShot(this.backgroundLoop, 1f);
				}
				if (base.IsMine)
				{
					this.UpdateTimerUI((int)this.timer.GetRemainingTime());
					if (Time.time - this.curentTime >= this.currentLevel.pickNextMoleTime)
					{
						this.SwitchState(WhackAMole.GameState.PickMoles);
					}
				}
				break;
			case WhackAMole.GameState.PickMoles:
				if (base.IsMine && this.PickMoles())
				{
					this.SwitchState(WhackAMole.GameState.Ongoing);
				}
				break;
			case WhackAMole.GameState.TimesUp:
				switch (this.curentGameResult)
				{
				case WhackAMole.GameResult.GameOver:
					if (base.IsMine && Time.time - this.gameEndedTime > 10f)
					{
						this.SwitchState(WhackAMole.GameState.Off);
					}
					break;
				case WhackAMole.GameResult.Win:
					if (base.IsMine && Time.time - this.gameEndedTime > 10f)
					{
						this.SwitchState(WhackAMole.GameState.Off);
					}
					break;
				case WhackAMole.GameResult.LevelComplete:
					if (Time.time - this.gameEndedTime > (float)this.betweenLevelPauseDuration && base.IsMine)
					{
						this.SwitchState(WhackAMole.GameState.ContinuePressed);
					}
					break;
				}
				break;
			case WhackAMole.GameState.LevelStarted:
				if (base.IsMine)
				{
					this.SwitchState(WhackAMole.GameState.Ongoing);
				}
				break;
			}
			if (this.arrowRotationNeedsUpdate)
			{
				this.UpdateArrowRotation();
			}
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x0010FFBC File Offset: 0x0010E1BC
		private void SwitchState(WhackAMole.GameState state)
		{
			this.lastState = this.currentState;
			this.currentState = state;
			switch (this.currentState)
			{
			case WhackAMole.GameState.Off:
				this.ResetGame();
				this.currentLevelIndex = -1;
				this.currentLevel = null;
				this.UpdateLevelUI(1);
				break;
			case WhackAMole.GameState.ContinuePressed:
				this.continuePressedTime = Time.time;
				this.audioSource.GTStop();
				this.audioSource.GTPlayOneShot(this.counterClip, 1f);
				if (base.IsMine)
				{
					this.pickedMolesIndex.Clear();
				}
				this.ResetGame();
				if (base.IsMine)
				{
					this.LoadNextLevel();
				}
				break;
			case WhackAMole.GameState.Ongoing:
				this.UpdateScoreUI(this.currentScore, this.leftPlayerScore, this.rightPlayerScore);
				break;
			case WhackAMole.GameState.TimesUp:
				if (this.currentLevel != null)
				{
					foreach (Mole mole in this.molesList)
					{
						mole.HideMole(false);
					}
					this.curentGameResult = this.GetGameResult();
					this.UpdateResultUI(this.curentGameResult);
					this.levelEndedTotalScoreText.text = "SCORE " + this.totalScore.ToString();
					this.levelEndedCurrentScoreText.text = string.Format("{0}/{1}", this.currentScore, this.currentLevel.GetMinScore(this.isMultiplayer));
					if (this.totalScore > this.bestScore)
					{
						this.bestScore = this.totalScore;
						this.highScorePlayerName = this.playerName;
					}
					this.bestScoreText.text = (this.isMultiplayer ? this.bestScore.ToString() : (this.highScorePlayerName + "  " + this.bestScore.ToString()));
					this.audioSource.GTStop();
					if (this.curentGameResult == WhackAMole.GameResult.LevelComplete)
					{
						this.audioSource.GTPlayOneShot(this.levelCompleteClip, 1f);
						if (NetworkSystem.Instance.LocalPlayer.UserId == this.playerId)
						{
							PlayerGameEvents.MiscEvent("WhackComplete" + this.currentLevel.levelNumber.ToString());
						}
					}
					else if (this.curentGameResult == WhackAMole.GameResult.GameOver)
					{
						this.audioSource.GTPlayOneShot(this.gameOverClip, 1f);
					}
					else if (this.curentGameResult == WhackAMole.GameResult.Win)
					{
						this.audioSource.GTPlayOneShot(this.winClip, 1f);
						if (this.victoryFX)
						{
							ParticleSystem[] array = this.victoryParticles;
							for (int i = 0; i < array.Length; i++)
							{
								array[i].Play();
							}
						}
						if (NetworkSystem.Instance.LocalPlayer.UserId == this.playerId)
						{
							PlayerGameEvents.MiscEvent("WhackComplete" + this.currentLevel.levelNumber.ToString());
						}
					}
					int minScore = this.currentLevel.GetMinScore(this.isMultiplayer);
					if (this.levelGoodMolesPicked < minScore)
					{
						GTDev.LogError<string>(string.Format("[WAM] Lvl:{0} Only Picked {1}/{2} good moles!", this.currentLevel.levelNumber, this.levelGoodMolesPicked, minScore), null);
					}
					if (base.IsMine)
					{
						GorillaTelemetry.WamLevelEnd(this.playerId, this.gameId, this.machineId, this.currentLevel.levelNumber, this.levelGoodMolesPicked, this.levelHazardMolesPicked, minScore, this.currentScore, this.levelHazardMolesHit, this.curentGameResult.ToString());
					}
				}
				break;
			}
			this.UpdateScreenData();
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x00110384 File Offset: 0x0010E584
		private void UpdateScreenData()
		{
			switch (this.currentState)
			{
			case WhackAMole.GameState.Off:
				this.welcomeUI.SetActive(true);
				this.ContinuePressedUI.SetActive(false);
				this.ongoingGameUI.SetActive(false);
				this.levelEndedUI.SetActive(false);
				this.multiplyareScoresUI.SetActive(false);
				return;
			case WhackAMole.GameState.ContinuePressed:
				this.levelEndedUI.SetActive(false);
				this.welcomeUI.SetActive(false);
				this.ongoingGameUI.SetActive(false);
				this.multiplyareScoresUI.SetActive(false);
				this.ContinuePressedUI.SetActive(true);
				break;
			case WhackAMole.GameState.Ongoing:
				this.ContinuePressedUI.SetActive(false);
				this.welcomeUI.SetActive(false);
				this.ongoingGameUI.SetActive(true);
				this.levelEndedUI.SetActive(false);
				if (this.isMultiplayer)
				{
					this.multiplyareScoresUI.SetActive(true);
					return;
				}
				break;
			case WhackAMole.GameState.PickMoles:
				break;
			case WhackAMole.GameState.TimesUp:
				this.welcomeUI.SetActive(false);
				this.ongoingGameUI.SetActive(false);
				this.ContinuePressedUI.SetActive(false);
				if (this.isMultiplayer)
				{
					this.multiplyareScoresUI.SetActive(true);
				}
				this.levelEndedUI.SetActive(true);
				return;
			default:
				return;
			}
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x001104BC File Offset: 0x0010E6BC
		public static int CreateNewGameID()
		{
			int num = (int)((DateTime.Now - WhackAMole.epoch).TotalSeconds * 8.0 % 2147483646.0) + 1;
			if (num <= WhackAMole.lastAssignedID)
			{
				WhackAMole.lastAssignedID++;
				return WhackAMole.lastAssignedID;
			}
			WhackAMole.lastAssignedID = num;
			return num;
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x0011051C File Offset: 0x0010E71C
		private void OnMoleTapped(MoleTypes moleType, Vector3 position, bool isLocalTap, bool isLeftHand)
		{
			WhackAMole.GameState gameState = this.currentState;
			if (gameState == WhackAMole.GameState.Off || gameState == WhackAMole.GameState.TimesUp)
			{
				return;
			}
			AudioClip clip = moleType.isHazard ? this.whackHazardClips[Random.Range(0, this.whackHazardClips.Length)] : this.whackMonkeClips[Random.Range(0, this.whackMonkeClips.Length)];
			if (moleType.isHazard)
			{
				this.audioSource.GTPlayOneShot(clip, 1f);
				this.levelHazardMolesHit++;
			}
			else
			{
				this.audioSource.GTPlayOneShot(clip, 1f);
			}
			if (moleType.monkeMoleHitMaterial != null)
			{
				moleType.MeshRenderer.material = moleType.monkeMoleHitMaterial;
			}
			this.currentScore += moleType.scorePoint;
			this.totalScore += moleType.scorePoint;
			if (moleType.IsLeftSideMoleType)
			{
				this.leftPlayerScore += moleType.scorePoint;
			}
			else
			{
				this.rightPlayerScore += moleType.scorePoint;
			}
			this.UpdateScoreUI(this.currentScore, this.leftPlayerScore, this.rightPlayerScore);
			moleType.MoleContainerParent.HideMole(true);
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x00110640 File Offset: 0x0010E840
		public void HandleOnTimerStopped()
		{
			this.gameEndedTime = Time.time;
			this.SwitchState(WhackAMole.GameState.TimesUp);
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x00110654 File Offset: 0x0010E854
		private IEnumerator PlayHazardAudio(AudioClip clip)
		{
			this.audioSource.clip = clip;
			this.audioSource.GTPlay();
			yield return new WaitForSeconds(this.audioSource.clip.length);
			this.audioSource.clip = this.errorClip;
			this.audioSource.GTPlay();
			yield break;
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x0011066C File Offset: 0x0010E86C
		private bool PickMoles()
		{
			WhackAMole.<>c__DisplayClass85_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			this.pickedMolesIndex.Clear();
			float passedTime = this.timer.GetPassedTime();
			if (passedTime > this.currentLevel.levelDuration - this.currentLevel.showMoleDuration)
			{
				return true;
			}
			float t = passedTime / this.currentLevel.levelDuration;
			CS$<>8__locals1.minMoleCount = Mathf.Lerp(this.currentLevel.minimumMoleCount.x, this.currentLevel.minimumMoleCount.y, t);
			CS$<>8__locals1.maxMoleCount = Mathf.Lerp(this.currentLevel.maximumMoleCount.x, this.currentLevel.maximumMoleCount.y, t);
			this.curentTime = Time.time;
			CS$<>8__locals1.hazardMoleChance = Mathf.Lerp(this.currentLevel.hazardMoleChance.x, this.currentLevel.hazardMoleChance.y, t);
			if (this.isMultiplayer)
			{
				this.<PickMoles>g__PickMolesFrom|85_0(this.rightMolesList, ref CS$<>8__locals1);
				this.<PickMoles>g__PickMolesFrom|85_0(this.leftMolesList, ref CS$<>8__locals1);
			}
			else
			{
				this.<PickMoles>g__PickMolesFrom|85_0(this.molesList, ref CS$<>8__locals1);
			}
			return this.pickedMolesIndex.Count != 0;
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x00110798 File Offset: 0x0010E998
		private void LoadNextLevel()
		{
			if (this.currentLevel != null)
			{
				this.resetToFirstLevel = (this.currentScore < this.currentLevel.GetMinScore(this.isMultiplayer));
				if (this.resetToFirstLevel)
				{
					this.currentLevelIndex = 0;
				}
				else
				{
					this.currentLevelIndex++;
				}
				if (this.currentLevelIndex >= this.allLevels.Length)
				{
					this.currentLevelIndex = 0;
				}
			}
			else
			{
				this.currentLevelIndex++;
			}
			this.currentLevel = this.allLevels[this.currentLevelIndex];
			this.timer.SetTimerDuration(this.currentLevel.levelDuration);
			this.timer.RestartTimer();
			this.curentTime = Time.time;
			this.currentScore = 0;
			this.leftPlayerScore = 0;
			this.rightPlayerScore = 0;
			this.levelGoodMolesPicked = (this.levelHazardMolesPicked = 0);
			this.levelHazardMolesHit = 0;
			if (this.currentLevelIndex == 0)
			{
				this.totalScore = 0;
			}
			if (this.currentLevelIndex == 0 && base.IsMine)
			{
				this.gameId = WhackAMole.CreateNewGameID();
				Debug.LogWarning("GAME ID" + this.gameId.ToString());
			}
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x001108C8 File Offset: 0x0010EAC8
		private bool PickSingleMole(int randomMoleIndex, float hazardMoleChance)
		{
			bool flag = hazardMoleChance > 0f && Random.value <= hazardMoleChance;
			int moleTypeIndex = this.molesList[randomMoleIndex].GetMoleTypeIndex(flag);
			this.molesList[randomMoleIndex].ShowMole(this.currentLevel.showMoleDuration, moleTypeIndex);
			this.pickedMolesIndex.Add(randomMoleIndex, moleTypeIndex);
			if (flag)
			{
				this.levelHazardMolesPicked++;
			}
			else
			{
				this.levelGoodMolesPicked++;
			}
			return flag;
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x0011094C File Offset: 0x0010EB4C
		private void ResetGame()
		{
			foreach (Mole mole in this.molesList)
			{
				mole.ResetPosition();
			}
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x0011099C File Offset: 0x0010EB9C
		private void UpdateScoreUI(int totalScore, int _leftPlayerScore, int _rightPlayerScore)
		{
			if (this.currentLevel != null)
			{
				this.scoreText.text = string.Format("SCORE\n{0}/{1}", totalScore, this.currentLevel.GetMinScore(this.isMultiplayer));
				this.leftPlayerScoreText.text = _leftPlayerScore.ToString();
				this.rightPlayerScoreText.text = _rightPlayerScore.ToString();
			}
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x00110A0C File Offset: 0x0010EC0C
		private void UpdateLevelUI(int levelNumber)
		{
			this.arrowTargetRotation = Quaternion.Euler(0f, 0f, (float)(18 * (levelNumber - 1)));
			this.arrowRotationNeedsUpdate = true;
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x00110A34 File Offset: 0x0010EC34
		private void UpdateArrowRotation()
		{
			Quaternion quaternion = Quaternion.Slerp(this.levelArrow.transform.localRotation, this.arrowTargetRotation, Time.deltaTime * 5f);
			if (Quaternion.Angle(quaternion, this.arrowTargetRotation) < 0.1f)
			{
				quaternion = this.arrowTargetRotation;
				this.arrowRotationNeedsUpdate = false;
			}
			this.levelArrow.transform.localRotation = quaternion;
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x00110A9A File Offset: 0x0010EC9A
		private void UpdateTimerUI(int time)
		{
			if (time == this.previousTime)
			{
				return;
			}
			this.timeText.text = "TIME " + time.ToString();
			this.previousTime = time;
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x00110AC9 File Offset: 0x0010ECC9
		private void UpdateResultUI(WhackAMole.GameResult gameResult)
		{
			if (gameResult == WhackAMole.GameResult.LevelComplete)
			{
				this.resultText.text = "LEVEL COMPLETE";
				return;
			}
			if (gameResult == WhackAMole.GameResult.Win)
			{
				this.resultText.text = "YOU WIN!";
				return;
			}
			if (gameResult == WhackAMole.GameResult.GameOver)
			{
				this.resultText.text = "GAME OVER";
			}
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x00110B08 File Offset: 0x0010ED08
		public void OnStartButtonPressed()
		{
			WhackAMole.GameState gameState = this.currentState;
			if (gameState == WhackAMole.GameState.TimesUp || gameState == WhackAMole.GameState.Off)
			{
				base.GetView.RPC("WhackAMoleButtonPressed", RpcTarget.All, Array.Empty<object>());
			}
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x00110B39 File Offset: 0x0010ED39
		[PunRPC]
		private void WhackAMoleButtonPressed(PhotonMessageInfo info)
		{
			this.WhackAMoleButtonPressedShared(info);
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x00110B48 File Offset: 0x0010ED48
		[Rpc]
		private unsafe void RPC_WhackAMoleButtonPressed(RpcInfo info = default(RpcInfo))
		{
			if (!this.InvokeRpc)
			{
				NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
				if (base.Runner.Stage != SimulationStages.Resimulate)
				{
					int localAuthorityMask = base.Object.GetLocalAuthorityMask();
					if ((localAuthorityMask & 7) == 0)
					{
						NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void GorillaTagScripts.WhackAMole::RPC_WhackAMoleButtonPressed(Fusion.RpcInfo)", base.Object, 7);
					}
					else
					{
						if (base.Runner.HasAnyActiveConnections())
						{
							int capacityInBytes = 8;
							SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, capacityInBytes);
							byte* data = SimulationMessage.GetData(ptr);
							int num = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 1), data);
							ptr->Offset = num * 8;
							base.Runner.SendRpc(ptr);
						}
						if ((localAuthorityMask & 7) != 0)
						{
							info = RpcInfo.FromLocal(base.Runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
							goto IL_12;
						}
					}
				}
				return;
			}
			this.InvokeRpc = false;
			IL_12:
			this.WhackAMoleButtonPressedShared(info);
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x00110C68 File Offset: 0x0010EE68
		private void WhackAMoleButtonPressedShared(PhotonMessageInfoWrapped info)
		{
			GorillaNot.IncrementRPCCall(info, "WhackAMoleButtonPressedShared");
			VRRig vrrig = GorillaGameManager.StaticFindRigForPlayer(info.Sender);
			if (vrrig)
			{
				this.playerName = vrrig.playerNameVisible;
				if (this.currentState == WhackAMole.GameState.Off)
				{
					this.playerId = info.Sender.UserId;
					if (NetworkSystem.Instance.LocalPlayer.UserId == this.playerId)
					{
						PlayerGameEvents.MiscEvent("PlayArcadeGame");
					}
				}
			}
			this.SwitchState(WhackAMole.GameState.ContinuePressed);
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x00110CE8 File Offset: 0x0010EEE8
		private WhackAMole.GameResult GetGameResult()
		{
			if (this.currentScore < this.currentLevel.GetMinScore(this.isMultiplayer))
			{
				return WhackAMole.GameResult.GameOver;
			}
			if (this.currentLevelIndex >= this.allLevels.Length - 1)
			{
				return WhackAMole.GameResult.Win;
			}
			return WhackAMole.GameResult.LevelComplete;
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x00110D1A File Offset: 0x0010EF1A
		public int GetCurrentLevel()
		{
			if (this.currentLevel != null)
			{
				return this.currentLevel.levelNumber;
			}
			return 0;
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x00110D37 File Offset: 0x0010EF37
		public int GetTotalLevelNumbers()
		{
			if (this.allLevels != null)
			{
				return this.allLevels.Length;
			}
			return 0;
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06003B2B RID: 15147 RVA: 0x00110D4B File Offset: 0x0010EF4B
		// (set) Token: 0x06003B2C RID: 15148 RVA: 0x00110D75 File Offset: 0x0010EF75
		[Networked]
		[NetworkedWeaved(0, 210)]
		public unsafe WhackAMole.WhackAMoleData Data
		{
			get
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing WhackAMole.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(WhackAMole.WhackAMoleData*)(this.Ptr + 0);
			}
			set
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing WhackAMole.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(WhackAMole.WhackAMoleData*)(this.Ptr + 0) = value;
			}
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x00110DA0 File Offset: 0x0010EFA0
		public override void WriteDataFusion()
		{
			this.Data = new WhackAMole.WhackAMoleData(this.currentState, this.currentLevelIndex, this.currentScore, this.totalScore, this.bestScore, this.rightPlayerScore, this.highScorePlayerName, this.timer.GetRemainingTime(), this.gameEndedTime, this.gameId, this.pickedMolesIndex);
			this.pickedMolesIndex.Clear();
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x00110E0C File Offset: 0x0010F00C
		public override void ReadDataFusion()
		{
			this.ReadDataShared(this.Data.CurrentState, this.Data.CurrentLevelIndex, this.Data.CurrentScore, this.Data.TotalScore, this.Data.BestScore, this.Data.RightPlayerScore, this.Data.HighScorePlayerName.Value, this.Data.RemainingTime, this.Data.GameEndedTime, this.Data.GameId);
			for (int i = 0; i < this.Data.PickedMolesIndexCount; i++)
			{
				int randomMoleTypeIndex = this.Data.PickedMolesIndex[i];
				if (i >= 0 && i < this.molesList.Count && this.currentLevel)
				{
					this.molesList[i].ShowMole(this.currentLevel.showMoleDuration, randomMoleTypeIndex);
				}
			}
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x00110F24 File Offset: 0x0010F124
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != PhotonNetwork.MasterClient)
			{
				return;
			}
			stream.SendNext(this.currentState);
			stream.SendNext(this.currentLevelIndex);
			stream.SendNext(this.currentScore);
			stream.SendNext(this.totalScore);
			stream.SendNext(this.bestScore);
			stream.SendNext(this.rightPlayerScore);
			stream.SendNext(this.highScorePlayerName);
			stream.SendNext(this.timer.GetRemainingTime());
			stream.SendNext(this.gameEndedTime);
			stream.SendNext(this.gameId);
			stream.SendNext(this.pickedMolesIndex.Count);
			foreach (KeyValuePair<int, int> keyValuePair in this.pickedMolesIndex)
			{
				stream.SendNext(keyValuePair.Key);
				stream.SendNext(keyValuePair.Value);
			}
			this.pickedMolesIndex.Clear();
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x00111070 File Offset: 0x0010F270
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != PhotonNetwork.MasterClient)
			{
				return;
			}
			WhackAMole.GameState gameState = (WhackAMole.GameState)stream.ReceiveNext();
			int num = (int)stream.ReceiveNext();
			int cScore = (int)stream.ReceiveNext();
			int tScore = (int)stream.ReceiveNext();
			int bScore = (int)stream.ReceiveNext();
			int rPScore = (int)stream.ReceiveNext();
			string hScorePName = (string)stream.ReceiveNext();
			float num2 = (float)stream.ReceiveNext();
			float endedTime = (float)stream.ReceiveNext();
			int num3 = (int)stream.ReceiveNext();
			int num4 = (int)stream.ReceiveNext();
			this.ReadDataShared(gameState, num, cScore, tScore, bScore, rPScore, hScorePName, num2, endedTime, num3);
			for (int i = 0; i < num4; i++)
			{
				int num5 = (int)stream.ReceiveNext();
				int randomMoleTypeIndex = (int)stream.ReceiveNext();
				if (num5 >= 0 && num5 < this.molesList.Count && this.currentLevel)
				{
					this.molesList[num5].ShowMole(this.currentLevel.showMoleDuration, randomMoleTypeIndex);
				}
			}
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x00111198 File Offset: 0x0010F398
		private void ReadDataShared(WhackAMole.GameState _currentState, int _currentLevelIndex, int cScore, int tScore, int bScore, int rPScore, string hScorePName, float _remainingTime, float endedTime, int _gameId)
		{
			WhackAMole.GameState gameState = this.currentState;
			if (_currentState != gameState)
			{
				this.SwitchState(_currentState);
			}
			this.currentLevelIndex = _currentLevelIndex;
			if (this.currentLevelIndex >= 0 && this.currentLevelIndex < this.allLevels.Length)
			{
				this.currentLevel = this.allLevels[this.currentLevelIndex];
				this.UpdateLevelUI(this.currentLevel.levelNumber);
			}
			this.currentScore = cScore;
			this.totalScore = tScore;
			this.bestScore = bScore;
			this.rightPlayerScore = rPScore;
			this.leftPlayerScore = this.currentScore - this.rightPlayerScore;
			this.highScorePlayerName = hScorePName;
			this.bestScoreText.text = (this.isMultiplayer ? this.bestScore.ToString() : (this.highScorePlayerName + "  " + this.bestScore.ToString()));
			this.remainingTime = _remainingTime;
			if (float.IsFinite(this.remainingTime) && this.currentLevel)
			{
				this.remainingTime = this.remainingTime.ClampSafe(0f, this.currentLevel.levelDuration);
				this.UpdateTimerUI((int)this.remainingTime);
			}
			if (float.IsFinite(endedTime))
			{
				this.gameEndedTime = endedTime.ClampSafe(0f, Time.time);
			}
			this.gameId = _gameId;
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x001112EC File Offset: 0x0010F4EC
		protected override void OnOwnerSwitched(NetPlayer newOwningPlayer)
		{
			base.OnOwnerSwitched(newOwningPlayer);
			if (NetworkSystem.Instance.IsMasterClient)
			{
				this.timer.RestartTimer();
				this.timer.SetTimerDuration(this.remainingTime);
				this.curentTime = Time.time;
				if (this.currentLevelIndex >= 0 && this.currentLevelIndex < this.allLevels.Length)
				{
					this.currentLevel = this.allLevels[this.currentLevelIndex];
				}
				this.SwitchState(this.currentState);
			}
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x001113F0 File Offset: 0x0010F5F0
		[CompilerGenerated]
		private void <PickMoles>g__PickMolesFrom|85_0(List<Mole> moles, ref WhackAMole.<>c__DisplayClass85_0 A_2)
		{
			int a = Mathf.RoundToInt(Random.Range(A_2.minMoleCount, A_2.maxMoleCount));
			this.potentialMoles.Clear();
			foreach (Mole mole in moles)
			{
				if (mole.CanPickMole())
				{
					this.potentialMoles.Add(mole);
				}
			}
			int num = Mathf.Min(a, this.potentialMoles.Count);
			int num2 = Mathf.CeilToInt((float)num * A_2.hazardMoleChance);
			int num3 = 0;
			for (int i = 0; i < num; i++)
			{
				int index = Random.Range(0, this.potentialMoles.Count);
				if (this.PickSingleMole(this.molesList.IndexOf(this.potentialMoles[index]), (num3 < num2) ? A_2.hazardMoleChance : 0f))
				{
					num3++;
				}
				this.potentialMoles.RemoveAt(index);
			}
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x001114FC File Offset: 0x0010F6FC
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x00111514 File Offset: 0x0010F714
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x00111528 File Offset: 0x0010F728
		[NetworkRpcWeavedInvoker(1, 7, 7)]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_WhackAMoleButtonPressed@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
			behaviour.InvokeRpc = true;
			((WhackAMole)behaviour).RPC_WhackAMoleButtonPressed(info);
		}

		// Token: 0x04003C21 RID: 15393
		public string machineId = "default";

		// Token: 0x04003C22 RID: 15394
		public GameObject molesContainerRight;

		// Token: 0x04003C23 RID: 15395
		[Tooltip("Only for co-op version")]
		public GameObject molesContainerLeft;

		// Token: 0x04003C24 RID: 15396
		public int betweenLevelPauseDuration = 3;

		// Token: 0x04003C25 RID: 15397
		public int countdownDuration = 5;

		// Token: 0x04003C26 RID: 15398
		public WhackAMoleLevelSO[] allLevels;

		// Token: 0x04003C27 RID: 15399
		[SerializeField]
		private GorillaTimer timer;

		// Token: 0x04003C28 RID: 15400
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04003C29 RID: 15401
		public GameObject levelArrow;

		// Token: 0x04003C2A RID: 15402
		public GameObject victoryFX;

		// Token: 0x04003C2B RID: 15403
		public ZoneBasedObject[] zoneBasedVisuals;

		// Token: 0x04003C2C RID: 15404
		[SerializeField]
		private MeshRenderer[] zoneBasedMeshRenderers;

		// Token: 0x04003C2D RID: 15405
		[Space]
		public AudioClip backgroundLoop;

		// Token: 0x04003C2E RID: 15406
		public AudioClip errorClip;

		// Token: 0x04003C2F RID: 15407
		public AudioClip counterClip;

		// Token: 0x04003C30 RID: 15408
		public AudioClip levelCompleteClip;

		// Token: 0x04003C31 RID: 15409
		public AudioClip winClip;

		// Token: 0x04003C32 RID: 15410
		public AudioClip gameOverClip;

		// Token: 0x04003C33 RID: 15411
		public AudioClip[] whackHazardClips;

		// Token: 0x04003C34 RID: 15412
		public AudioClip[] whackMonkeClips;

		// Token: 0x04003C35 RID: 15413
		[Space]
		public GameObject welcomeUI;

		// Token: 0x04003C36 RID: 15414
		public GameObject ongoingGameUI;

		// Token: 0x04003C37 RID: 15415
		public GameObject levelEndedUI;

		// Token: 0x04003C38 RID: 15416
		public GameObject ContinuePressedUI;

		// Token: 0x04003C39 RID: 15417
		public GameObject multiplyareScoresUI;

		// Token: 0x04003C3A RID: 15418
		[Space]
		public TextMeshPro scoreText;

		// Token: 0x04003C3B RID: 15419
		public TextMeshPro bestScoreText;

		// Token: 0x04003C3C RID: 15420
		[Tooltip("Only for co-op version")]
		public TextMeshPro rightPlayerScoreText;

		// Token: 0x04003C3D RID: 15421
		[Tooltip("Only for co-op version")]
		public TextMeshPro leftPlayerScoreText;

		// Token: 0x04003C3E RID: 15422
		public TextMeshPro timeText;

		// Token: 0x04003C3F RID: 15423
		public TextMeshPro counterText;

		// Token: 0x04003C40 RID: 15424
		public TextMeshPro resultText;

		// Token: 0x04003C41 RID: 15425
		public TextMeshPro levelEndedOptionsText;

		// Token: 0x04003C42 RID: 15426
		public TextMeshPro levelEndedCountdownText;

		// Token: 0x04003C43 RID: 15427
		public TextMeshPro levelEndedTotalScoreText;

		// Token: 0x04003C44 RID: 15428
		public TextMeshPro levelEndedCurrentScoreText;

		// Token: 0x04003C45 RID: 15429
		private List<Mole> rightMolesList;

		// Token: 0x04003C46 RID: 15430
		private List<Mole> leftMolesList;

		// Token: 0x04003C47 RID: 15431
		private List<Mole> molesList = new List<Mole>();

		// Token: 0x04003C48 RID: 15432
		private WhackAMoleLevelSO currentLevel;

		// Token: 0x04003C49 RID: 15433
		private int currentScore;

		// Token: 0x04003C4A RID: 15434
		private int totalScore;

		// Token: 0x04003C4B RID: 15435
		private int leftPlayerScore;

		// Token: 0x04003C4C RID: 15436
		private int rightPlayerScore;

		// Token: 0x04003C4D RID: 15437
		private int bestScore;

		// Token: 0x04003C4E RID: 15438
		private float curentTime;

		// Token: 0x04003C4F RID: 15439
		private int currentLevelIndex;

		// Token: 0x04003C50 RID: 15440
		private float continuePressedTime;

		// Token: 0x04003C51 RID: 15441
		private bool resetToFirstLevel;

		// Token: 0x04003C52 RID: 15442
		private Quaternion arrowTargetRotation;

		// Token: 0x04003C53 RID: 15443
		private bool arrowRotationNeedsUpdate;

		// Token: 0x04003C54 RID: 15444
		private List<Mole> potentialMoles = new List<Mole>();

		// Token: 0x04003C55 RID: 15445
		private Dictionary<int, int> pickedMolesIndex = new Dictionary<int, int>();

		// Token: 0x04003C56 RID: 15446
		private WhackAMole.GameState currentState;

		// Token: 0x04003C57 RID: 15447
		private WhackAMole.GameState lastState;

		// Token: 0x04003C58 RID: 15448
		private float remainingTime;

		// Token: 0x04003C59 RID: 15449
		private int previousTime = -1;

		// Token: 0x04003C5A RID: 15450
		private bool isMultiplayer;

		// Token: 0x04003C5B RID: 15451
		private float gameEndedTime;

		// Token: 0x04003C5C RID: 15452
		private WhackAMole.GameResult curentGameResult;

		// Token: 0x04003C5D RID: 15453
		private string playerName = string.Empty;

		// Token: 0x04003C5E RID: 15454
		private string highScorePlayerName = string.Empty;

		// Token: 0x04003C5F RID: 15455
		private ParticleSystem[] victoryParticles;

		// Token: 0x04003C60 RID: 15456
		private int levelHazardMolesPicked;

		// Token: 0x04003C61 RID: 15457
		private int levelGoodMolesPicked;

		// Token: 0x04003C62 RID: 15458
		private string playerId;

		// Token: 0x04003C63 RID: 15459
		private int gameId;

		// Token: 0x04003C64 RID: 15460
		private int levelHazardMolesHit;

		// Token: 0x04003C65 RID: 15461
		private static DateTime epoch = new DateTime(2024, 1, 1);

		// Token: 0x04003C66 RID: 15462
		private static int lastAssignedID;

		// Token: 0x04003C67 RID: 15463
		private bool wasMasterClient;

		// Token: 0x04003C68 RID: 15464
		private bool wasLocalPlayerInZone = true;

		// Token: 0x04003C69 RID: 15465
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 210)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private WhackAMole.WhackAMoleData _Data;

		// Token: 0x02000976 RID: 2422
		public enum GameState
		{
			// Token: 0x04003C6B RID: 15467
			Off,
			// Token: 0x04003C6C RID: 15468
			ContinuePressed,
			// Token: 0x04003C6D RID: 15469
			Ongoing,
			// Token: 0x04003C6E RID: 15470
			PickMoles,
			// Token: 0x04003C6F RID: 15471
			TimesUp,
			// Token: 0x04003C70 RID: 15472
			LevelStarted
		}

		// Token: 0x02000977 RID: 2423
		private enum GameResult
		{
			// Token: 0x04003C72 RID: 15474
			GameOver,
			// Token: 0x04003C73 RID: 15475
			Win,
			// Token: 0x04003C74 RID: 15476
			LevelComplete,
			// Token: 0x04003C75 RID: 15477
			Unknown
		}

		// Token: 0x02000978 RID: 2424
		[NetworkStructWeaved(210)]
		[StructLayout(LayoutKind.Explicit, Size = 840)]
		public struct WhackAMoleData : INetworkStruct
		{
			// Token: 0x17000623 RID: 1571
			// (get) Token: 0x06003B39 RID: 15161 RVA: 0x0011157B File Offset: 0x0010F77B
			// (set) Token: 0x06003B3A RID: 15162 RVA: 0x00111583 File Offset: 0x0010F783
			public WhackAMole.GameState CurrentState { readonly get; set; }

			// Token: 0x17000624 RID: 1572
			// (get) Token: 0x06003B3B RID: 15163 RVA: 0x0011158C File Offset: 0x0010F78C
			// (set) Token: 0x06003B3C RID: 15164 RVA: 0x00111594 File Offset: 0x0010F794
			public int CurrentLevelIndex { readonly get; set; }

			// Token: 0x17000625 RID: 1573
			// (get) Token: 0x06003B3D RID: 15165 RVA: 0x0011159D File Offset: 0x0010F79D
			// (set) Token: 0x06003B3E RID: 15166 RVA: 0x001115A5 File Offset: 0x0010F7A5
			public int CurrentScore { readonly get; set; }

			// Token: 0x17000626 RID: 1574
			// (get) Token: 0x06003B3F RID: 15167 RVA: 0x001115AE File Offset: 0x0010F7AE
			// (set) Token: 0x06003B40 RID: 15168 RVA: 0x001115B6 File Offset: 0x0010F7B6
			public int TotalScore { readonly get; set; }

			// Token: 0x17000627 RID: 1575
			// (get) Token: 0x06003B41 RID: 15169 RVA: 0x001115BF File Offset: 0x0010F7BF
			// (set) Token: 0x06003B42 RID: 15170 RVA: 0x001115C7 File Offset: 0x0010F7C7
			public int BestScore { readonly get; set; }

			// Token: 0x17000628 RID: 1576
			// (get) Token: 0x06003B43 RID: 15171 RVA: 0x001115D0 File Offset: 0x0010F7D0
			// (set) Token: 0x06003B44 RID: 15172 RVA: 0x001115D8 File Offset: 0x0010F7D8
			public int RightPlayerScore { readonly get; set; }

			// Token: 0x17000629 RID: 1577
			// (get) Token: 0x06003B45 RID: 15173 RVA: 0x001115E1 File Offset: 0x0010F7E1
			// (set) Token: 0x06003B46 RID: 15174 RVA: 0x001115F3 File Offset: 0x0010F7F3
			[Networked]
			public unsafe NetworkString<_128> HighScorePlayerName
			{
				readonly get
				{
					return *(NetworkString<_128>*)Native.ReferenceToPointer<FixedStorage@129>(ref this._HighScorePlayerName);
				}
				set
				{
					*(NetworkString<_128>*)Native.ReferenceToPointer<FixedStorage@129>(ref this._HighScorePlayerName) = value;
				}
			}

			// Token: 0x1700062A RID: 1578
			// (get) Token: 0x06003B47 RID: 15175 RVA: 0x00111606 File Offset: 0x0010F806
			// (set) Token: 0x06003B48 RID: 15176 RVA: 0x0011160E File Offset: 0x0010F80E
			public float RemainingTime { readonly get; set; }

			// Token: 0x1700062B RID: 1579
			// (get) Token: 0x06003B49 RID: 15177 RVA: 0x00111617 File Offset: 0x0010F817
			// (set) Token: 0x06003B4A RID: 15178 RVA: 0x0011161F File Offset: 0x0010F81F
			public float GameEndedTime { readonly get; set; }

			// Token: 0x1700062C RID: 1580
			// (get) Token: 0x06003B4B RID: 15179 RVA: 0x00111628 File Offset: 0x0010F828
			// (set) Token: 0x06003B4C RID: 15180 RVA: 0x00111630 File Offset: 0x0010F830
			public int GameId { readonly get; set; }

			// Token: 0x1700062D RID: 1581
			// (get) Token: 0x06003B4D RID: 15181 RVA: 0x00111639 File Offset: 0x0010F839
			// (set) Token: 0x06003B4E RID: 15182 RVA: 0x00111641 File Offset: 0x0010F841
			public int PickedMolesIndexCount { readonly get; set; }

			// Token: 0x1700062E RID: 1582
			// (get) Token: 0x06003B4F RID: 15183 RVA: 0x0011164C File Offset: 0x0010F84C
			[Networked]
			[Capacity(10)]
			public unsafe NetworkDictionary<int, int> PickedMolesIndex
			{
				get
				{
					return new NetworkDictionary<int, int>((int*)Native.ReferenceToPointer<FixedStorage@71>(ref this._PickedMolesIndex), 17, ReaderWriter@System_Int32.GetInstance(), ReaderWriter@System_Int32.GetInstance());
				}
			}

			// Token: 0x06003B50 RID: 15184 RVA: 0x00111678 File Offset: 0x0010F878
			public WhackAMoleData(WhackAMole.GameState state, int currentLevelIndex, int cScore, int tScore, int bScore, int rPScore, string hScorePName, float remainingTime, float endedTime, int gameId, Dictionary<int, int> moleIndexs)
			{
				this.CurrentState = state;
				this.CurrentLevelIndex = currentLevelIndex;
				this.CurrentScore = cScore;
				this.TotalScore = tScore;
				this.BestScore = bScore;
				this.RightPlayerScore = rPScore;
				this.HighScorePlayerName = hScorePName;
				this.RemainingTime = remainingTime;
				this.GameEndedTime = endedTime;
				this.GameId = gameId;
				this.PickedMolesIndexCount = moleIndexs.Count;
				foreach (KeyValuePair<int, int> keyValuePair in moleIndexs)
				{
					this.PickedMolesIndex.Set(keyValuePair.Key, keyValuePair.Value);
				}
			}

			// Token: 0x04003C7C RID: 15484
			[FixedBufferProperty(typeof(NetworkString<_128>), typeof(UnityValueSurrogate@ReaderWriter@Fusion_NetworkString), 0, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(24)]
			private FixedStorage@129 _HighScorePlayerName;

			// Token: 0x04003C81 RID: 15489
			[FixedBufferProperty(typeof(NetworkDictionary<int, int>), typeof(UnityDictionarySurrogate@ReaderWriter@System_Int32@ReaderWriter@System_Int32), 17, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(556)]
			private FixedStorage@71 _PickedMolesIndex;
		}
	}
}
