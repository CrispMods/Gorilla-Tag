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
	// Token: 0x02000978 RID: 2424
	[NetworkBehaviourWeaved(210)]
	public class WhackAMole : NetworkComponent
	{
		// Token: 0x06003B1C RID: 15132 RVA: 0x0014D070 File Offset: 0x0014B270
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

		// Token: 0x06003B1D RID: 15133 RVA: 0x0014D0D8 File Offset: 0x0014B2D8
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

		// Token: 0x06003B1E RID: 15134 RVA: 0x00055BB3 File Offset: 0x00053DB3
		protected override void Start()
		{
			base.Start();
			this.SwitchState(WhackAMole.GameState.Off);
			if (WhackAMoleManager.instance)
			{
				WhackAMoleManager.instance.Register(this);
			}
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x0014D2A8 File Offset: 0x0014B4A8
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

		// Token: 0x06003B20 RID: 15136 RVA: 0x0014D32C File Offset: 0x0014B52C
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

		// Token: 0x06003B21 RID: 15137 RVA: 0x0014D608 File Offset: 0x0014B808
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

		// Token: 0x06003B22 RID: 15138 RVA: 0x0014D9D0 File Offset: 0x0014BBD0
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

		// Token: 0x06003B23 RID: 15139 RVA: 0x0014DB08 File Offset: 0x0014BD08
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

		// Token: 0x06003B24 RID: 15140 RVA: 0x0014DB68 File Offset: 0x0014BD68
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

		// Token: 0x06003B25 RID: 15141 RVA: 0x00055BD9 File Offset: 0x00053DD9
		public void HandleOnTimerStopped()
		{
			this.gameEndedTime = Time.time;
			this.SwitchState(WhackAMole.GameState.TimesUp);
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x00055BED File Offset: 0x00053DED
		private IEnumerator PlayHazardAudio(AudioClip clip)
		{
			this.audioSource.clip = clip;
			this.audioSource.GTPlay();
			yield return new WaitForSeconds(this.audioSource.clip.length);
			this.audioSource.clip = this.errorClip;
			this.audioSource.GTPlay();
			yield break;
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x0014DC8C File Offset: 0x0014BE8C
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

		// Token: 0x06003B28 RID: 15144 RVA: 0x0014DDB8 File Offset: 0x0014BFB8
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

		// Token: 0x06003B29 RID: 15145 RVA: 0x0014DEE8 File Offset: 0x0014C0E8
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

		// Token: 0x06003B2A RID: 15146 RVA: 0x0014DF6C File Offset: 0x0014C16C
		private void ResetGame()
		{
			foreach (Mole mole in this.molesList)
			{
				mole.ResetPosition();
			}
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x0014DFBC File Offset: 0x0014C1BC
		private void UpdateScoreUI(int totalScore, int _leftPlayerScore, int _rightPlayerScore)
		{
			if (this.currentLevel != null)
			{
				this.scoreText.text = string.Format("SCORE\n{0}/{1}", totalScore, this.currentLevel.GetMinScore(this.isMultiplayer));
				this.leftPlayerScoreText.text = _leftPlayerScore.ToString();
				this.rightPlayerScoreText.text = _rightPlayerScore.ToString();
			}
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x00055C03 File Offset: 0x00053E03
		private void UpdateLevelUI(int levelNumber)
		{
			this.arrowTargetRotation = Quaternion.Euler(0f, 0f, (float)(18 * (levelNumber - 1)));
			this.arrowRotationNeedsUpdate = true;
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x0014E02C File Offset: 0x0014C22C
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

		// Token: 0x06003B2E RID: 15150 RVA: 0x00055C28 File Offset: 0x00053E28
		private void UpdateTimerUI(int time)
		{
			if (time == this.previousTime)
			{
				return;
			}
			this.timeText.text = "TIME " + time.ToString();
			this.previousTime = time;
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x00055C57 File Offset: 0x00053E57
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

		// Token: 0x06003B30 RID: 15152 RVA: 0x0014E094 File Offset: 0x0014C294
		public void OnStartButtonPressed()
		{
			WhackAMole.GameState gameState = this.currentState;
			if (gameState == WhackAMole.GameState.TimesUp || gameState == WhackAMole.GameState.Off)
			{
				base.GetView.RPC("WhackAMoleButtonPressed", RpcTarget.All, Array.Empty<object>());
			}
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x00055C96 File Offset: 0x00053E96
		[PunRPC]
		private void WhackAMoleButtonPressed(PhotonMessageInfo info)
		{
			this.WhackAMoleButtonPressedShared(info);
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x0014E0C8 File Offset: 0x0014C2C8
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

		// Token: 0x06003B33 RID: 15155 RVA: 0x0014E1E8 File Offset: 0x0014C3E8
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

		// Token: 0x06003B34 RID: 15156 RVA: 0x00055CA4 File Offset: 0x00053EA4
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

		// Token: 0x06003B35 RID: 15157 RVA: 0x00055CD6 File Offset: 0x00053ED6
		public int GetCurrentLevel()
		{
			if (this.currentLevel != null)
			{
				return this.currentLevel.levelNumber;
			}
			return 0;
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x00055CF3 File Offset: 0x00053EF3
		public int GetTotalLevelNumbers()
		{
			if (this.allLevels != null)
			{
				return this.allLevels.Length;
			}
			return 0;
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06003B37 RID: 15159 RVA: 0x00055D07 File Offset: 0x00053F07
		// (set) Token: 0x06003B38 RID: 15160 RVA: 0x00055D31 File Offset: 0x00053F31
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

		// Token: 0x06003B39 RID: 15161 RVA: 0x0014E268 File Offset: 0x0014C468
		public override void WriteDataFusion()
		{
			this.Data = new WhackAMole.WhackAMoleData(this.currentState, this.currentLevelIndex, this.currentScore, this.totalScore, this.bestScore, this.rightPlayerScore, this.highScorePlayerName, this.timer.GetRemainingTime(), this.gameEndedTime, this.gameId, this.pickedMolesIndex);
			this.pickedMolesIndex.Clear();
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x0014E2D4 File Offset: 0x0014C4D4
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

		// Token: 0x06003B3B RID: 15163 RVA: 0x0014E3EC File Offset: 0x0014C5EC
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

		// Token: 0x06003B3C RID: 15164 RVA: 0x0014E538 File Offset: 0x0014C738
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

		// Token: 0x06003B3D RID: 15165 RVA: 0x0014E660 File Offset: 0x0014C860
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

		// Token: 0x06003B3E RID: 15166 RVA: 0x0014E7B4 File Offset: 0x0014C9B4
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

		// Token: 0x06003B41 RID: 15169 RVA: 0x0014E8A8 File Offset: 0x0014CAA8
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

		// Token: 0x06003B42 RID: 15170 RVA: 0x00055D6F File Offset: 0x00053F6F
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x00055D87 File Offset: 0x00053F87
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x0014E9B4 File Offset: 0x0014CBB4
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

		// Token: 0x04003C33 RID: 15411
		public string machineId = "default";

		// Token: 0x04003C34 RID: 15412
		public GameObject molesContainerRight;

		// Token: 0x04003C35 RID: 15413
		[Tooltip("Only for co-op version")]
		public GameObject molesContainerLeft;

		// Token: 0x04003C36 RID: 15414
		public int betweenLevelPauseDuration = 3;

		// Token: 0x04003C37 RID: 15415
		public int countdownDuration = 5;

		// Token: 0x04003C38 RID: 15416
		public WhackAMoleLevelSO[] allLevels;

		// Token: 0x04003C39 RID: 15417
		[SerializeField]
		private GorillaTimer timer;

		// Token: 0x04003C3A RID: 15418
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04003C3B RID: 15419
		public GameObject levelArrow;

		// Token: 0x04003C3C RID: 15420
		public GameObject victoryFX;

		// Token: 0x04003C3D RID: 15421
		public ZoneBasedObject[] zoneBasedVisuals;

		// Token: 0x04003C3E RID: 15422
		[SerializeField]
		private MeshRenderer[] zoneBasedMeshRenderers;

		// Token: 0x04003C3F RID: 15423
		[Space]
		public AudioClip backgroundLoop;

		// Token: 0x04003C40 RID: 15424
		public AudioClip errorClip;

		// Token: 0x04003C41 RID: 15425
		public AudioClip counterClip;

		// Token: 0x04003C42 RID: 15426
		public AudioClip levelCompleteClip;

		// Token: 0x04003C43 RID: 15427
		public AudioClip winClip;

		// Token: 0x04003C44 RID: 15428
		public AudioClip gameOverClip;

		// Token: 0x04003C45 RID: 15429
		public AudioClip[] whackHazardClips;

		// Token: 0x04003C46 RID: 15430
		public AudioClip[] whackMonkeClips;

		// Token: 0x04003C47 RID: 15431
		[Space]
		public GameObject welcomeUI;

		// Token: 0x04003C48 RID: 15432
		public GameObject ongoingGameUI;

		// Token: 0x04003C49 RID: 15433
		public GameObject levelEndedUI;

		// Token: 0x04003C4A RID: 15434
		public GameObject ContinuePressedUI;

		// Token: 0x04003C4B RID: 15435
		public GameObject multiplyareScoresUI;

		// Token: 0x04003C4C RID: 15436
		[Space]
		public TextMeshPro scoreText;

		// Token: 0x04003C4D RID: 15437
		public TextMeshPro bestScoreText;

		// Token: 0x04003C4E RID: 15438
		[Tooltip("Only for co-op version")]
		public TextMeshPro rightPlayerScoreText;

		// Token: 0x04003C4F RID: 15439
		[Tooltip("Only for co-op version")]
		public TextMeshPro leftPlayerScoreText;

		// Token: 0x04003C50 RID: 15440
		public TextMeshPro timeText;

		// Token: 0x04003C51 RID: 15441
		public TextMeshPro counterText;

		// Token: 0x04003C52 RID: 15442
		public TextMeshPro resultText;

		// Token: 0x04003C53 RID: 15443
		public TextMeshPro levelEndedOptionsText;

		// Token: 0x04003C54 RID: 15444
		public TextMeshPro levelEndedCountdownText;

		// Token: 0x04003C55 RID: 15445
		public TextMeshPro levelEndedTotalScoreText;

		// Token: 0x04003C56 RID: 15446
		public TextMeshPro levelEndedCurrentScoreText;

		// Token: 0x04003C57 RID: 15447
		private List<Mole> rightMolesList;

		// Token: 0x04003C58 RID: 15448
		private List<Mole> leftMolesList;

		// Token: 0x04003C59 RID: 15449
		private List<Mole> molesList = new List<Mole>();

		// Token: 0x04003C5A RID: 15450
		private WhackAMoleLevelSO currentLevel;

		// Token: 0x04003C5B RID: 15451
		private int currentScore;

		// Token: 0x04003C5C RID: 15452
		private int totalScore;

		// Token: 0x04003C5D RID: 15453
		private int leftPlayerScore;

		// Token: 0x04003C5E RID: 15454
		private int rightPlayerScore;

		// Token: 0x04003C5F RID: 15455
		private int bestScore;

		// Token: 0x04003C60 RID: 15456
		private float curentTime;

		// Token: 0x04003C61 RID: 15457
		private int currentLevelIndex;

		// Token: 0x04003C62 RID: 15458
		private float continuePressedTime;

		// Token: 0x04003C63 RID: 15459
		private bool resetToFirstLevel;

		// Token: 0x04003C64 RID: 15460
		private Quaternion arrowTargetRotation;

		// Token: 0x04003C65 RID: 15461
		private bool arrowRotationNeedsUpdate;

		// Token: 0x04003C66 RID: 15462
		private List<Mole> potentialMoles = new List<Mole>();

		// Token: 0x04003C67 RID: 15463
		private Dictionary<int, int> pickedMolesIndex = new Dictionary<int, int>();

		// Token: 0x04003C68 RID: 15464
		private WhackAMole.GameState currentState;

		// Token: 0x04003C69 RID: 15465
		private WhackAMole.GameState lastState;

		// Token: 0x04003C6A RID: 15466
		private float remainingTime;

		// Token: 0x04003C6B RID: 15467
		private int previousTime = -1;

		// Token: 0x04003C6C RID: 15468
		private bool isMultiplayer;

		// Token: 0x04003C6D RID: 15469
		private float gameEndedTime;

		// Token: 0x04003C6E RID: 15470
		private WhackAMole.GameResult curentGameResult;

		// Token: 0x04003C6F RID: 15471
		private string playerName = string.Empty;

		// Token: 0x04003C70 RID: 15472
		private string highScorePlayerName = string.Empty;

		// Token: 0x04003C71 RID: 15473
		private ParticleSystem[] victoryParticles;

		// Token: 0x04003C72 RID: 15474
		private int levelHazardMolesPicked;

		// Token: 0x04003C73 RID: 15475
		private int levelGoodMolesPicked;

		// Token: 0x04003C74 RID: 15476
		private string playerId;

		// Token: 0x04003C75 RID: 15477
		private int gameId;

		// Token: 0x04003C76 RID: 15478
		private int levelHazardMolesHit;

		// Token: 0x04003C77 RID: 15479
		private static DateTime epoch = new DateTime(2024, 1, 1);

		// Token: 0x04003C78 RID: 15480
		private static int lastAssignedID;

		// Token: 0x04003C79 RID: 15481
		private bool wasMasterClient;

		// Token: 0x04003C7A RID: 15482
		private bool wasLocalPlayerInZone = true;

		// Token: 0x04003C7B RID: 15483
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 210)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private WhackAMole.WhackAMoleData _Data;

		// Token: 0x02000979 RID: 2425
		public enum GameState
		{
			// Token: 0x04003C7D RID: 15485
			Off,
			// Token: 0x04003C7E RID: 15486
			ContinuePressed,
			// Token: 0x04003C7F RID: 15487
			Ongoing,
			// Token: 0x04003C80 RID: 15488
			PickMoles,
			// Token: 0x04003C81 RID: 15489
			TimesUp,
			// Token: 0x04003C82 RID: 15490
			LevelStarted
		}

		// Token: 0x0200097A RID: 2426
		private enum GameResult
		{
			// Token: 0x04003C84 RID: 15492
			GameOver,
			// Token: 0x04003C85 RID: 15493
			Win,
			// Token: 0x04003C86 RID: 15494
			LevelComplete,
			// Token: 0x04003C87 RID: 15495
			Unknown
		}

		// Token: 0x0200097B RID: 2427
		[NetworkStructWeaved(210)]
		[StructLayout(LayoutKind.Explicit, Size = 840)]
		public struct WhackAMoleData : INetworkStruct
		{
			// Token: 0x17000624 RID: 1572
			// (get) Token: 0x06003B45 RID: 15173 RVA: 0x00055D9B File Offset: 0x00053F9B
			// (set) Token: 0x06003B46 RID: 15174 RVA: 0x00055DA3 File Offset: 0x00053FA3
			public WhackAMole.GameState CurrentState { readonly get; set; }

			// Token: 0x17000625 RID: 1573
			// (get) Token: 0x06003B47 RID: 15175 RVA: 0x00055DAC File Offset: 0x00053FAC
			// (set) Token: 0x06003B48 RID: 15176 RVA: 0x00055DB4 File Offset: 0x00053FB4
			public int CurrentLevelIndex { readonly get; set; }

			// Token: 0x17000626 RID: 1574
			// (get) Token: 0x06003B49 RID: 15177 RVA: 0x00055DBD File Offset: 0x00053FBD
			// (set) Token: 0x06003B4A RID: 15178 RVA: 0x00055DC5 File Offset: 0x00053FC5
			public int CurrentScore { readonly get; set; }

			// Token: 0x17000627 RID: 1575
			// (get) Token: 0x06003B4B RID: 15179 RVA: 0x00055DCE File Offset: 0x00053FCE
			// (set) Token: 0x06003B4C RID: 15180 RVA: 0x00055DD6 File Offset: 0x00053FD6
			public int TotalScore { readonly get; set; }

			// Token: 0x17000628 RID: 1576
			// (get) Token: 0x06003B4D RID: 15181 RVA: 0x00055DDF File Offset: 0x00053FDF
			// (set) Token: 0x06003B4E RID: 15182 RVA: 0x00055DE7 File Offset: 0x00053FE7
			public int BestScore { readonly get; set; }

			// Token: 0x17000629 RID: 1577
			// (get) Token: 0x06003B4F RID: 15183 RVA: 0x00055DF0 File Offset: 0x00053FF0
			// (set) Token: 0x06003B50 RID: 15184 RVA: 0x00055DF8 File Offset: 0x00053FF8
			public int RightPlayerScore { readonly get; set; }

			// Token: 0x1700062A RID: 1578
			// (get) Token: 0x06003B51 RID: 15185 RVA: 0x00055E01 File Offset: 0x00054001
			// (set) Token: 0x06003B52 RID: 15186 RVA: 0x00055E13 File Offset: 0x00054013
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

			// Token: 0x1700062B RID: 1579
			// (get) Token: 0x06003B53 RID: 15187 RVA: 0x00055E26 File Offset: 0x00054026
			// (set) Token: 0x06003B54 RID: 15188 RVA: 0x00055E2E File Offset: 0x0005402E
			public float RemainingTime { readonly get; set; }

			// Token: 0x1700062C RID: 1580
			// (get) Token: 0x06003B55 RID: 15189 RVA: 0x00055E37 File Offset: 0x00054037
			// (set) Token: 0x06003B56 RID: 15190 RVA: 0x00055E3F File Offset: 0x0005403F
			public float GameEndedTime { readonly get; set; }

			// Token: 0x1700062D RID: 1581
			// (get) Token: 0x06003B57 RID: 15191 RVA: 0x00055E48 File Offset: 0x00054048
			// (set) Token: 0x06003B58 RID: 15192 RVA: 0x00055E50 File Offset: 0x00054050
			public int GameId { readonly get; set; }

			// Token: 0x1700062E RID: 1582
			// (get) Token: 0x06003B59 RID: 15193 RVA: 0x00055E59 File Offset: 0x00054059
			// (set) Token: 0x06003B5A RID: 15194 RVA: 0x00055E61 File Offset: 0x00054061
			public int PickedMolesIndexCount { readonly get; set; }

			// Token: 0x1700062F RID: 1583
			// (get) Token: 0x06003B5B RID: 15195 RVA: 0x00055E6A File Offset: 0x0005406A
			[Networked]
			[Capacity(10)]
			public unsafe NetworkDictionary<int, int> PickedMolesIndex
			{
				get
				{
					return new NetworkDictionary<int, int>((int*)Native.ReferenceToPointer<FixedStorage@71>(ref this._PickedMolesIndex), 17, ReaderWriter@System_Int32.GetInstance(), ReaderWriter@System_Int32.GetInstance());
				}
			}

			// Token: 0x06003B5C RID: 15196 RVA: 0x0014EA08 File Offset: 0x0014CC08
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

			// Token: 0x04003C8E RID: 15502
			[FixedBufferProperty(typeof(NetworkString<_128>), typeof(UnityValueSurrogate@ReaderWriter@Fusion_NetworkString), 0, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(24)]
			private FixedStorage@129 _HighScorePlayerName;

			// Token: 0x04003C93 RID: 15507
			[FixedBufferProperty(typeof(NetworkDictionary<int, int>), typeof(UnityDictionarySurrogate@ReaderWriter@System_Int32@ReaderWriter@System_Int32), 17, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(556)]
			private FixedStorage@71 _PickedMolesIndex;
		}
	}
}
