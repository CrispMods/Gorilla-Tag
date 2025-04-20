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
	// Token: 0x0200099B RID: 2459
	[NetworkBehaviourWeaved(210)]
	public class WhackAMole : NetworkComponent
	{
		// Token: 0x06003C28 RID: 15400 RVA: 0x00153058 File Offset: 0x00151258
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

		// Token: 0x06003C29 RID: 15401 RVA: 0x001530C0 File Offset: 0x001512C0
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

		// Token: 0x06003C2A RID: 15402 RVA: 0x0005744A File Offset: 0x0005564A
		protected override void Start()
		{
			base.Start();
			this.SwitchState(WhackAMole.GameState.Off);
			if (WhackAMoleManager.instance)
			{
				WhackAMoleManager.instance.Register(this);
			}
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x00153290 File Offset: 0x00151490
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

		// Token: 0x06003C2C RID: 15404 RVA: 0x00153314 File Offset: 0x00151514
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

		// Token: 0x06003C2D RID: 15405 RVA: 0x001535F0 File Offset: 0x001517F0
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

		// Token: 0x06003C2E RID: 15406 RVA: 0x001539B8 File Offset: 0x00151BB8
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

		// Token: 0x06003C2F RID: 15407 RVA: 0x00153AF0 File Offset: 0x00151CF0
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

		// Token: 0x06003C30 RID: 15408 RVA: 0x00153B50 File Offset: 0x00151D50
		private void OnMoleTapped(MoleTypes moleType, Vector3 position, bool isLocalTap, bool isLeftHand)
		{
			WhackAMole.GameState gameState = this.currentState;
			if (gameState == WhackAMole.GameState.Off || gameState == WhackAMole.GameState.TimesUp)
			{
				return;
			}
			AudioClip clip = moleType.isHazard ? this.whackHazardClips[UnityEngine.Random.Range(0, this.whackHazardClips.Length)] : this.whackMonkeClips[UnityEngine.Random.Range(0, this.whackMonkeClips.Length)];
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

		// Token: 0x06003C31 RID: 15409 RVA: 0x00057470 File Offset: 0x00055670
		public void HandleOnTimerStopped()
		{
			this.gameEndedTime = Time.time;
			this.SwitchState(WhackAMole.GameState.TimesUp);
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x00057484 File Offset: 0x00055684
		private IEnumerator PlayHazardAudio(AudioClip clip)
		{
			this.audioSource.clip = clip;
			this.audioSource.GTPlay();
			yield return new WaitForSeconds(this.audioSource.clip.length);
			this.audioSource.clip = this.errorClip;
			this.audioSource.GTPlay();
			yield break;
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x00153C74 File Offset: 0x00151E74
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

		// Token: 0x06003C34 RID: 15412 RVA: 0x00153DA0 File Offset: 0x00151FA0
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

		// Token: 0x06003C35 RID: 15413 RVA: 0x00153ED0 File Offset: 0x001520D0
		private bool PickSingleMole(int randomMoleIndex, float hazardMoleChance)
		{
			bool flag = hazardMoleChance > 0f && UnityEngine.Random.value <= hazardMoleChance;
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

		// Token: 0x06003C36 RID: 15414 RVA: 0x00153F54 File Offset: 0x00152154
		private void ResetGame()
		{
			foreach (Mole mole in this.molesList)
			{
				mole.ResetPosition();
			}
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x00153FA4 File Offset: 0x001521A4
		private void UpdateScoreUI(int totalScore, int _leftPlayerScore, int _rightPlayerScore)
		{
			if (this.currentLevel != null)
			{
				this.scoreText.text = string.Format("SCORE\n{0}/{1}", totalScore, this.currentLevel.GetMinScore(this.isMultiplayer));
				this.leftPlayerScoreText.text = _leftPlayerScore.ToString();
				this.rightPlayerScoreText.text = _rightPlayerScore.ToString();
			}
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x0005749A File Offset: 0x0005569A
		private void UpdateLevelUI(int levelNumber)
		{
			this.arrowTargetRotation = Quaternion.Euler(0f, 0f, (float)(18 * (levelNumber - 1)));
			this.arrowRotationNeedsUpdate = true;
		}

		// Token: 0x06003C39 RID: 15417 RVA: 0x00154014 File Offset: 0x00152214
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

		// Token: 0x06003C3A RID: 15418 RVA: 0x000574BF File Offset: 0x000556BF
		private void UpdateTimerUI(int time)
		{
			if (time == this.previousTime)
			{
				return;
			}
			this.timeText.text = "TIME " + time.ToString();
			this.previousTime = time;
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x000574EE File Offset: 0x000556EE
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

		// Token: 0x06003C3C RID: 15420 RVA: 0x0015407C File Offset: 0x0015227C
		public void OnStartButtonPressed()
		{
			WhackAMole.GameState gameState = this.currentState;
			if (gameState == WhackAMole.GameState.TimesUp || gameState == WhackAMole.GameState.Off)
			{
				base.GetView.RPC("WhackAMoleButtonPressed", RpcTarget.All, Array.Empty<object>());
			}
		}

		// Token: 0x06003C3D RID: 15421 RVA: 0x0005752D File Offset: 0x0005572D
		[PunRPC]
		private void WhackAMoleButtonPressed(PhotonMessageInfo info)
		{
			this.WhackAMoleButtonPressedShared(info);
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x001540B0 File Offset: 0x001522B0
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

		// Token: 0x06003C3F RID: 15423 RVA: 0x001541D0 File Offset: 0x001523D0
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

		// Token: 0x06003C40 RID: 15424 RVA: 0x0005753B File Offset: 0x0005573B
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

		// Token: 0x06003C41 RID: 15425 RVA: 0x0005756D File Offset: 0x0005576D
		public int GetCurrentLevel()
		{
			if (this.currentLevel != null)
			{
				return this.currentLevel.levelNumber;
			}
			return 0;
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x0005758A File Offset: 0x0005578A
		public int GetTotalLevelNumbers()
		{
			if (this.allLevels != null)
			{
				return this.allLevels.Length;
			}
			return 0;
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06003C43 RID: 15427 RVA: 0x0005759E File Offset: 0x0005579E
		// (set) Token: 0x06003C44 RID: 15428 RVA: 0x000575C8 File Offset: 0x000557C8
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

		// Token: 0x06003C45 RID: 15429 RVA: 0x00154250 File Offset: 0x00152450
		public override void WriteDataFusion()
		{
			this.Data = new WhackAMole.WhackAMoleData(this.currentState, this.currentLevelIndex, this.currentScore, this.totalScore, this.bestScore, this.rightPlayerScore, this.highScorePlayerName, this.timer.GetRemainingTime(), this.gameEndedTime, this.gameId, this.pickedMolesIndex);
			this.pickedMolesIndex.Clear();
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x001542BC File Offset: 0x001524BC
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

		// Token: 0x06003C47 RID: 15431 RVA: 0x001543D4 File Offset: 0x001525D4
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

		// Token: 0x06003C48 RID: 15432 RVA: 0x00154520 File Offset: 0x00152720
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

		// Token: 0x06003C49 RID: 15433 RVA: 0x00154648 File Offset: 0x00152848
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

		// Token: 0x06003C4A RID: 15434 RVA: 0x0015479C File Offset: 0x0015299C
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

		// Token: 0x06003C4D RID: 15437 RVA: 0x00154890 File Offset: 0x00152A90
		[CompilerGenerated]
		private void <PickMoles>g__PickMolesFrom|85_0(List<Mole> moles, ref WhackAMole.<>c__DisplayClass85_0 A_2)
		{
			int a = Mathf.RoundToInt(UnityEngine.Random.Range(A_2.minMoleCount, A_2.maxMoleCount));
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
				int index = UnityEngine.Random.Range(0, this.potentialMoles.Count);
				if (this.PickSingleMole(this.molesList.IndexOf(this.potentialMoles[index]), (num3 < num2) ? A_2.hazardMoleChance : 0f))
				{
					num3++;
				}
				this.potentialMoles.RemoveAt(index);
			}
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x00057606 File Offset: 0x00055806
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x0005761E File Offset: 0x0005581E
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x0015499C File Offset: 0x00152B9C
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

		// Token: 0x04003CFB RID: 15611
		public string machineId = "default";

		// Token: 0x04003CFC RID: 15612
		public GameObject molesContainerRight;

		// Token: 0x04003CFD RID: 15613
		[Tooltip("Only for co-op version")]
		public GameObject molesContainerLeft;

		// Token: 0x04003CFE RID: 15614
		public int betweenLevelPauseDuration = 3;

		// Token: 0x04003CFF RID: 15615
		public int countdownDuration = 5;

		// Token: 0x04003D00 RID: 15616
		public WhackAMoleLevelSO[] allLevels;

		// Token: 0x04003D01 RID: 15617
		[SerializeField]
		private GorillaTimer timer;

		// Token: 0x04003D02 RID: 15618
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04003D03 RID: 15619
		public GameObject levelArrow;

		// Token: 0x04003D04 RID: 15620
		public GameObject victoryFX;

		// Token: 0x04003D05 RID: 15621
		public ZoneBasedObject[] zoneBasedVisuals;

		// Token: 0x04003D06 RID: 15622
		[SerializeField]
		private MeshRenderer[] zoneBasedMeshRenderers;

		// Token: 0x04003D07 RID: 15623
		[Space]
		public AudioClip backgroundLoop;

		// Token: 0x04003D08 RID: 15624
		public AudioClip errorClip;

		// Token: 0x04003D09 RID: 15625
		public AudioClip counterClip;

		// Token: 0x04003D0A RID: 15626
		public AudioClip levelCompleteClip;

		// Token: 0x04003D0B RID: 15627
		public AudioClip winClip;

		// Token: 0x04003D0C RID: 15628
		public AudioClip gameOverClip;

		// Token: 0x04003D0D RID: 15629
		public AudioClip[] whackHazardClips;

		// Token: 0x04003D0E RID: 15630
		public AudioClip[] whackMonkeClips;

		// Token: 0x04003D0F RID: 15631
		[Space]
		public GameObject welcomeUI;

		// Token: 0x04003D10 RID: 15632
		public GameObject ongoingGameUI;

		// Token: 0x04003D11 RID: 15633
		public GameObject levelEndedUI;

		// Token: 0x04003D12 RID: 15634
		public GameObject ContinuePressedUI;

		// Token: 0x04003D13 RID: 15635
		public GameObject multiplyareScoresUI;

		// Token: 0x04003D14 RID: 15636
		[Space]
		public TextMeshPro scoreText;

		// Token: 0x04003D15 RID: 15637
		public TextMeshPro bestScoreText;

		// Token: 0x04003D16 RID: 15638
		[Tooltip("Only for co-op version")]
		public TextMeshPro rightPlayerScoreText;

		// Token: 0x04003D17 RID: 15639
		[Tooltip("Only for co-op version")]
		public TextMeshPro leftPlayerScoreText;

		// Token: 0x04003D18 RID: 15640
		public TextMeshPro timeText;

		// Token: 0x04003D19 RID: 15641
		public TextMeshPro counterText;

		// Token: 0x04003D1A RID: 15642
		public TextMeshPro resultText;

		// Token: 0x04003D1B RID: 15643
		public TextMeshPro levelEndedOptionsText;

		// Token: 0x04003D1C RID: 15644
		public TextMeshPro levelEndedCountdownText;

		// Token: 0x04003D1D RID: 15645
		public TextMeshPro levelEndedTotalScoreText;

		// Token: 0x04003D1E RID: 15646
		public TextMeshPro levelEndedCurrentScoreText;

		// Token: 0x04003D1F RID: 15647
		private List<Mole> rightMolesList;

		// Token: 0x04003D20 RID: 15648
		private List<Mole> leftMolesList;

		// Token: 0x04003D21 RID: 15649
		private List<Mole> molesList = new List<Mole>();

		// Token: 0x04003D22 RID: 15650
		private WhackAMoleLevelSO currentLevel;

		// Token: 0x04003D23 RID: 15651
		private int currentScore;

		// Token: 0x04003D24 RID: 15652
		private int totalScore;

		// Token: 0x04003D25 RID: 15653
		private int leftPlayerScore;

		// Token: 0x04003D26 RID: 15654
		private int rightPlayerScore;

		// Token: 0x04003D27 RID: 15655
		private int bestScore;

		// Token: 0x04003D28 RID: 15656
		private float curentTime;

		// Token: 0x04003D29 RID: 15657
		private int currentLevelIndex;

		// Token: 0x04003D2A RID: 15658
		private float continuePressedTime;

		// Token: 0x04003D2B RID: 15659
		private bool resetToFirstLevel;

		// Token: 0x04003D2C RID: 15660
		private Quaternion arrowTargetRotation;

		// Token: 0x04003D2D RID: 15661
		private bool arrowRotationNeedsUpdate;

		// Token: 0x04003D2E RID: 15662
		private List<Mole> potentialMoles = new List<Mole>();

		// Token: 0x04003D2F RID: 15663
		private Dictionary<int, int> pickedMolesIndex = new Dictionary<int, int>();

		// Token: 0x04003D30 RID: 15664
		private WhackAMole.GameState currentState;

		// Token: 0x04003D31 RID: 15665
		private WhackAMole.GameState lastState;

		// Token: 0x04003D32 RID: 15666
		private float remainingTime;

		// Token: 0x04003D33 RID: 15667
		private int previousTime = -1;

		// Token: 0x04003D34 RID: 15668
		private bool isMultiplayer;

		// Token: 0x04003D35 RID: 15669
		private float gameEndedTime;

		// Token: 0x04003D36 RID: 15670
		private WhackAMole.GameResult curentGameResult;

		// Token: 0x04003D37 RID: 15671
		private string playerName = string.Empty;

		// Token: 0x04003D38 RID: 15672
		private string highScorePlayerName = string.Empty;

		// Token: 0x04003D39 RID: 15673
		private ParticleSystem[] victoryParticles;

		// Token: 0x04003D3A RID: 15674
		private int levelHazardMolesPicked;

		// Token: 0x04003D3B RID: 15675
		private int levelGoodMolesPicked;

		// Token: 0x04003D3C RID: 15676
		private string playerId;

		// Token: 0x04003D3D RID: 15677
		private int gameId;

		// Token: 0x04003D3E RID: 15678
		private int levelHazardMolesHit;

		// Token: 0x04003D3F RID: 15679
		private static DateTime epoch = new DateTime(2024, 1, 1);

		// Token: 0x04003D40 RID: 15680
		private static int lastAssignedID;

		// Token: 0x04003D41 RID: 15681
		private bool wasMasterClient;

		// Token: 0x04003D42 RID: 15682
		private bool wasLocalPlayerInZone = true;

		// Token: 0x04003D43 RID: 15683
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 210)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private WhackAMole.WhackAMoleData _Data;

		// Token: 0x0200099C RID: 2460
		public enum GameState
		{
			// Token: 0x04003D45 RID: 15685
			Off,
			// Token: 0x04003D46 RID: 15686
			ContinuePressed,
			// Token: 0x04003D47 RID: 15687
			Ongoing,
			// Token: 0x04003D48 RID: 15688
			PickMoles,
			// Token: 0x04003D49 RID: 15689
			TimesUp,
			// Token: 0x04003D4A RID: 15690
			LevelStarted
		}

		// Token: 0x0200099D RID: 2461
		private enum GameResult
		{
			// Token: 0x04003D4C RID: 15692
			GameOver,
			// Token: 0x04003D4D RID: 15693
			Win,
			// Token: 0x04003D4E RID: 15694
			LevelComplete,
			// Token: 0x04003D4F RID: 15695
			Unknown
		}

		// Token: 0x0200099E RID: 2462
		[NetworkStructWeaved(210)]
		[StructLayout(LayoutKind.Explicit, Size = 840)]
		public struct WhackAMoleData : INetworkStruct
		{
			// Token: 0x1700063B RID: 1595
			// (get) Token: 0x06003C51 RID: 15441 RVA: 0x00057632 File Offset: 0x00055832
			// (set) Token: 0x06003C52 RID: 15442 RVA: 0x0005763A File Offset: 0x0005583A
			public WhackAMole.GameState CurrentState { readonly get; set; }

			// Token: 0x1700063C RID: 1596
			// (get) Token: 0x06003C53 RID: 15443 RVA: 0x00057643 File Offset: 0x00055843
			// (set) Token: 0x06003C54 RID: 15444 RVA: 0x0005764B File Offset: 0x0005584B
			public int CurrentLevelIndex { readonly get; set; }

			// Token: 0x1700063D RID: 1597
			// (get) Token: 0x06003C55 RID: 15445 RVA: 0x00057654 File Offset: 0x00055854
			// (set) Token: 0x06003C56 RID: 15446 RVA: 0x0005765C File Offset: 0x0005585C
			public int CurrentScore { readonly get; set; }

			// Token: 0x1700063E RID: 1598
			// (get) Token: 0x06003C57 RID: 15447 RVA: 0x00057665 File Offset: 0x00055865
			// (set) Token: 0x06003C58 RID: 15448 RVA: 0x0005766D File Offset: 0x0005586D
			public int TotalScore { readonly get; set; }

			// Token: 0x1700063F RID: 1599
			// (get) Token: 0x06003C59 RID: 15449 RVA: 0x00057676 File Offset: 0x00055876
			// (set) Token: 0x06003C5A RID: 15450 RVA: 0x0005767E File Offset: 0x0005587E
			public int BestScore { readonly get; set; }

			// Token: 0x17000640 RID: 1600
			// (get) Token: 0x06003C5B RID: 15451 RVA: 0x00057687 File Offset: 0x00055887
			// (set) Token: 0x06003C5C RID: 15452 RVA: 0x0005768F File Offset: 0x0005588F
			public int RightPlayerScore { readonly get; set; }

			// Token: 0x17000641 RID: 1601
			// (get) Token: 0x06003C5D RID: 15453 RVA: 0x00057698 File Offset: 0x00055898
			// (set) Token: 0x06003C5E RID: 15454 RVA: 0x000576AA File Offset: 0x000558AA
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

			// Token: 0x17000642 RID: 1602
			// (get) Token: 0x06003C5F RID: 15455 RVA: 0x000576BD File Offset: 0x000558BD
			// (set) Token: 0x06003C60 RID: 15456 RVA: 0x000576C5 File Offset: 0x000558C5
			public float RemainingTime { readonly get; set; }

			// Token: 0x17000643 RID: 1603
			// (get) Token: 0x06003C61 RID: 15457 RVA: 0x000576CE File Offset: 0x000558CE
			// (set) Token: 0x06003C62 RID: 15458 RVA: 0x000576D6 File Offset: 0x000558D6
			public float GameEndedTime { readonly get; set; }

			// Token: 0x17000644 RID: 1604
			// (get) Token: 0x06003C63 RID: 15459 RVA: 0x000576DF File Offset: 0x000558DF
			// (set) Token: 0x06003C64 RID: 15460 RVA: 0x000576E7 File Offset: 0x000558E7
			public int GameId { readonly get; set; }

			// Token: 0x17000645 RID: 1605
			// (get) Token: 0x06003C65 RID: 15461 RVA: 0x000576F0 File Offset: 0x000558F0
			// (set) Token: 0x06003C66 RID: 15462 RVA: 0x000576F8 File Offset: 0x000558F8
			public int PickedMolesIndexCount { readonly get; set; }

			// Token: 0x17000646 RID: 1606
			// (get) Token: 0x06003C67 RID: 15463 RVA: 0x00057701 File Offset: 0x00055901
			[Networked]
			[Capacity(10)]
			public unsafe NetworkDictionary<int, int> PickedMolesIndex
			{
				get
				{
					return new NetworkDictionary<int, int>((int*)Native.ReferenceToPointer<FixedStorage@71>(ref this._PickedMolesIndex), 17, ReaderWriter@System_Int32.GetInstance(), ReaderWriter@System_Int32.GetInstance());
				}
			}

			// Token: 0x06003C68 RID: 15464 RVA: 0x001549F0 File Offset: 0x00152BF0
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

			// Token: 0x04003D56 RID: 15702
			[FixedBufferProperty(typeof(NetworkString<_128>), typeof(UnityValueSurrogate@ReaderWriter@Fusion_NetworkString), 0, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(24)]
			private FixedStorage@129 _HighScorePlayerName;

			// Token: 0x04003D5B RID: 15707
			[FixedBufferProperty(typeof(NetworkDictionary<int, int>), typeof(UnityDictionarySurrogate@ReaderWriter@System_Int32@ReaderWriter@System_Int32), 17, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(556)]
			private FixedStorage@71 _PickedMolesIndex;
		}
	}
}
