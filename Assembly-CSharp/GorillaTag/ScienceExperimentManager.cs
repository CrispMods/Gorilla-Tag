using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CjLib;
using Fusion;
using Fusion.CodeGen;
using GorillaExtensions;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Scripting;

namespace GorillaTag
{
	// Token: 0x02000BCE RID: 3022
	[NetworkBehaviourWeaved(76)]
	public class ScienceExperimentManager : NetworkComponent, ITickSystemTick
	{
		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06004C70 RID: 19568 RVA: 0x001A53DC File Offset: 0x001A35DC
		private bool RefreshWaterAvailable
		{
			get
			{
				return this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Drained || this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Erupting || (this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Rising && this.riseProgress < this.lavaProgressToDisableRefreshWater) || (this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Draining && this.riseProgress < this.lavaProgressToEnableRefreshWater);
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06004C71 RID: 19569 RVA: 0x0006244E File Offset: 0x0006064E
		public ScienceExperimentManager.RisingLiquidState GameState
		{
			get
			{
				return this.reliableState.state;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06004C72 RID: 19570 RVA: 0x0006245B File Offset: 0x0006065B
		public float RiseProgress
		{
			get
			{
				return this.riseProgress;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06004C73 RID: 19571 RVA: 0x00062463 File Offset: 0x00060663
		public float RiseProgressLinear
		{
			get
			{
				return this.riseProgressLinear;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06004C74 RID: 19572 RVA: 0x001A4284 File Offset: 0x001A2484
		private int PlayerCount
		{
			get
			{
				int result = 1;
				GorillaGameManager gorillaGameManager = GorillaGameManager.instance;
				if (gorillaGameManager != null && gorillaGameManager.currentNetPlayerArray != null)
				{
					result = gorillaGameManager.currentNetPlayerArray.Length;
				}
				return result;
			}
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x001A5440 File Offset: 0x001A3640
		protected override void Awake()
		{
			base.Awake();
			if (ScienceExperimentManager.instance == null)
			{
				ScienceExperimentManager.instance = this;
				NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
				this.riseTimeLookup = new float[]
				{
					this.riseTimeFast,
					this.riseTimeMedium,
					this.riseTimeSlow,
					this.riseTimeExtraSlow
				};
				this.riseTime = this.riseTimeLookup[(int)this.nextRoundRiseSpeed];
				this.allPlayersInRoom = RoomSystem.PlayersInRoom.ToArray();
				GorillaGameManager.OnTouch += this.OnPlayerTagged;
				RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
				RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
				this.rotatingRings = new ScienceExperimentManager.RotatingRingState[this.ringParent.childCount];
				for (int i = 0; i < this.rotatingRings.Length; i++)
				{
					this.rotatingRings[i].ringTransform = this.ringParent.GetChild(i);
					this.rotatingRings[i].initialAngle = 0f;
					this.rotatingRings[i].resultingAngle = 0f;
				}
				this.gameAreaTriggerNotifier.CompositeTriggerEnter += this.OnColliderEnteredVolume;
				this.gameAreaTriggerNotifier.CompositeTriggerExit += this.OnColliderExitedVolume;
				this.liquidVolume.ColliderEnteredWater += this.OnColliderEnteredSoda;
				this.liquidVolume.ColliderExitedWater += this.OnColliderExitedSoda;
				this.entryLiquidVolume.ColliderEnteredWater += this.OnColliderEnteredSoda;
				this.entryLiquidVolume.ColliderExitedWater += this.OnColliderExitedSoda;
				if (this.bottleLiquidVolume != null)
				{
					this.bottleLiquidVolume.ColliderEnteredWater += this.OnColliderEnteredSoda;
					this.bottleLiquidVolume.ColliderExitedWater += this.OnColliderExitedSoda;
				}
				if (this.refreshWaterVolume != null)
				{
					this.refreshWaterVolume.ColliderEnteredWater += this.OnColliderEnteredRefreshWater;
					this.refreshWaterVolume.ColliderExitedWater += this.OnColliderExitedRefreshWater;
				}
				if (this.sodaWaterProjectileTriggerNotifier != null)
				{
					this.sodaWaterProjectileTriggerNotifier.OnProjectileTriggerEnter += this.OnProjectileEnteredSodaWater;
				}
				float num = Vector3.Distance(this.drainBlockerClosedPosition.position, this.drainBlockerOpenPosition.position);
				this.drainBlockerSlideSpeed = num / this.drainBlockerSlideTime;
				return;
			}
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x0006246B File Offset: 0x0006066B
		internal override void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			base.OnEnable();
			TickSystem<object>.AddTickCallback(this);
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x0006247F File Offset: 0x0006067F
		internal override void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			base.OnDisable();
			TickSystem<object>.RemoveTickCallback(this);
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x001A56EC File Offset: 0x001A38EC
		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			GorillaGameManager.OnTouch -= this.OnPlayerTagged;
			if (this.gameAreaTriggerNotifier != null)
			{
				this.gameAreaTriggerNotifier.CompositeTriggerEnter -= this.OnColliderEnteredVolume;
				this.gameAreaTriggerNotifier.CompositeTriggerExit -= this.OnColliderExitedVolume;
			}
			if (this.liquidVolume != null)
			{
				this.liquidVolume.ColliderEnteredWater -= this.OnColliderEnteredSoda;
				this.liquidVolume.ColliderExitedWater -= this.OnColliderExitedSoda;
			}
			if (this.entryLiquidVolume != null)
			{
				this.entryLiquidVolume.ColliderEnteredWater -= this.OnColliderEnteredSoda;
				this.entryLiquidVolume.ColliderExitedWater -= this.OnColliderExitedSoda;
			}
			if (this.bottleLiquidVolume != null)
			{
				this.bottleLiquidVolume.ColliderEnteredWater -= this.OnColliderEnteredSoda;
				this.bottleLiquidVolume.ColliderExitedWater -= this.OnColliderExitedSoda;
			}
			if (this.refreshWaterVolume != null)
			{
				this.refreshWaterVolume.ColliderEnteredWater -= this.OnColliderEnteredRefreshWater;
				this.refreshWaterVolume.ColliderExitedWater -= this.OnColliderExitedRefreshWater;
			}
			if (this.sodaWaterProjectileTriggerNotifier != null)
			{
				this.sodaWaterProjectileTriggerNotifier.OnProjectileTriggerEnter -= this.OnProjectileEnteredSodaWater;
			}
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x001A5864 File Offset: 0x001A3A64
		public void InitElements(ScienceExperimentSceneElements elements)
		{
			this.elements = elements;
			this.fizzParticleEmission = elements.sodaFizzParticles.emission;
			elements.sodaFizzParticles.gameObject.SetActive(false);
			elements.sodaEruptionParticles.gameObject.SetActive(false);
			RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x00062493 File Offset: 0x00060693
		public void DeInitElements()
		{
			this.elements = null;
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x001A58CC File Offset: 0x001A3ACC
		public Transform GetElement(ScienceExperimentElementID elementID)
		{
			switch (elementID)
			{
			case ScienceExperimentElementID.Platform1:
				return this.rotatingRings[0].ringTransform;
			case ScienceExperimentElementID.Platform2:
				return this.rotatingRings[1].ringTransform;
			case ScienceExperimentElementID.Platform3:
				return this.rotatingRings[2].ringTransform;
			case ScienceExperimentElementID.Platform4:
				return this.rotatingRings[3].ringTransform;
			case ScienceExperimentElementID.Platform5:
				return this.rotatingRings[4].ringTransform;
			case ScienceExperimentElementID.LiquidMesh:
				return this.liquidMeshTransform;
			case ScienceExperimentElementID.EntryChamberLiquidMesh:
				return this.entryWayLiquidMeshTransform;
			case ScienceExperimentElementID.EntryChamberBridgeQuad:
				return this.entryWayBridgeQuadTransform;
			case ScienceExperimentElementID.DrainBlocker:
				return this.drainBlocker;
			default:
				Debug.LogError(string.Format("Unhandled ScienceExperiment element ID! {0}", elementID));
				return null;
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06004C7C RID: 19580 RVA: 0x0006249C File Offset: 0x0006069C
		// (set) Token: 0x06004C7D RID: 19581 RVA: 0x000624A4 File Offset: 0x000606A4
		bool ITickSystemTick.TickRunning { get; set; }

		// Token: 0x06004C7E RID: 19582 RVA: 0x001A5994 File Offset: 0x001A3B94
		void ITickSystemTick.Tick()
		{
			this.prevTime = this.currentTime;
			this.currentTime = (NetworkSystem.Instance.InRoom ? NetworkSystem.Instance.SimTime : Time.unscaledTimeAsDouble);
			this.lastInfrequentUpdateTime = ((this.lastInfrequentUpdateTime > this.currentTime) ? this.currentTime : this.lastInfrequentUpdateTime);
			if (this.currentTime > this.lastInfrequentUpdateTime + (double)this.infrequentUpdatePeriod)
			{
				this.InfrequentUpdate();
				this.lastInfrequentUpdateTime = (double)((float)this.currentTime);
			}
			if (base.IsMine)
			{
				this.UpdateReliableState(this.currentTime, ref this.reliableState);
			}
			this.UpdateLocalState(this.currentTime, this.reliableState);
			this.localLagRiseProgressOffset = Mathf.MoveTowards(this.localLagRiseProgressOffset, 0f, this.lagResolutionLavaProgressPerSecond * Time.deltaTime);
			this.UpdateLiquid(this.riseProgress + this.localLagRiseProgressOffset);
			this.UpdateRotatingRings(this.ringRotationProgress);
			this.UpdateRefreshWater();
			this.UpdateDrainBlocker(this.currentTime);
			this.DisableObjectsInContactWithLava(this.liquidMeshTransform.localScale.z);
			this.UpdateEffects();
			if (this.debugDrawPlayerGameState)
			{
				for (int i = 0; i < this.inGamePlayerCount; i++)
				{
					NetPlayer netPlayer = null;
					if (NetworkSystem.Instance.InRoom)
					{
						netPlayer = NetworkSystem.Instance.GetPlayer(this.inGamePlayerStates[i].playerId);
					}
					else if (this.inGamePlayerStates[i].playerId == NetworkSystem.Instance.LocalPlayer.ActorNumber)
					{
						netPlayer = NetworkSystem.Instance.LocalPlayer;
					}
					RigContainer rigContainer;
					if (netPlayer != null && VRRigCache.Instance.TryGetVrrig(netPlayer, out rigContainer) && rigContainer.Rig != null)
					{
						float num = 0.03f;
						DebugUtil.DrawSphere(rigContainer.Rig.transform.position + Vector3.up * 0.5f * num, 0.16f * num, 12, 12, this.inGamePlayerStates[i].touchedLiquid ? Color.red : Color.green, true, DebugUtil.Style.SolidColor);
					}
				}
			}
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x001A5BB4 File Offset: 0x001A3DB4
		private void InfrequentUpdate()
		{
			this.allPlayersInRoom = RoomSystem.PlayersInRoom.ToArray();
			if (base.IsMine)
			{
				for (int i = this.inGamePlayerCount - 1; i >= 0; i--)
				{
					int playerId = this.inGamePlayerStates[i].playerId;
					bool flag = false;
					for (int j = 0; j < this.allPlayersInRoom.Length; j++)
					{
						if (this.allPlayersInRoom[j].ActorNumber == playerId)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						if (i < this.inGamePlayerCount - 1)
						{
							this.inGamePlayerStates[i] = this.inGamePlayerStates[this.inGamePlayerCount - 1];
						}
						this.inGamePlayerStates[this.inGamePlayerCount - 1] = default(ScienceExperimentManager.PlayerGameState);
						this.inGamePlayerCount--;
					}
				}
			}
			if (this.optPlayersOutOfRoomGameMode)
			{
				for (int k = 0; k < this.allPlayersInRoom.Length; k++)
				{
					bool flag2 = false;
					for (int l = 0; l < this.inGamePlayerCount; l++)
					{
						if (this.allPlayersInRoom[k].ActorNumber == this.inGamePlayerStates[l].playerId)
						{
							flag2 = true;
						}
					}
					if (flag2)
					{
						GorillaGameModes.GameMode.OptOut(this.allPlayersInRoom[k]);
					}
					else
					{
						GorillaGameModes.GameMode.OptIn(this.allPlayersInRoom[k]);
					}
				}
			}
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x001A5D00 File Offset: 0x001A3F00
		private bool PlayerInGame(Player player)
		{
			for (int i = 0; i < this.inGamePlayerCount; i++)
			{
				if (this.inGamePlayerStates[i].playerId == player.ActorNumber)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x001A5D3C File Offset: 0x001A3F3C
		private void UpdateReliableState(double currentTime, ref ScienceExperimentManager.SyncData syncData)
		{
			if (currentTime < syncData.stateStartTime)
			{
				syncData.stateStartTime = currentTime;
			}
			switch (syncData.state)
			{
			default:
			{
				if (this.<UpdateReliableState>g__GetAlivePlayerCount|105_0() > 0 && syncData.activationProgress > 1.0)
				{
					syncData.state = ScienceExperimentManager.RisingLiquidState.Erupting;
					syncData.stateStartTime = currentTime;
					syncData.stateStartLiquidProgressLinear = 0f;
					syncData.activationProgress = 1.0;
					return;
				}
				float num = Mathf.Clamp((float)(currentTime - this.prevTime), 0f, 0.1f);
				syncData.activationProgress = (double)Mathf.MoveTowards((float)syncData.activationProgress, 0f, this.lavaActivationDrainRateVsPlayerCount.Evaluate((float)this.PlayerCount) * num);
				return;
			}
			case ScienceExperimentManager.RisingLiquidState.Erupting:
				if (currentTime > syncData.stateStartTime + (double)this.fullyDrainedWaitTime)
				{
					this.riseTime = this.riseTimeLookup[(int)this.nextRoundRiseSpeed];
					syncData.stateStartLiquidProgressLinear = 0f;
					syncData.state = ScienceExperimentManager.RisingLiquidState.Rising;
					syncData.stateStartTime = currentTime;
					return;
				}
				break;
			case ScienceExperimentManager.RisingLiquidState.Rising:
				if (this.<UpdateReliableState>g__GetAlivePlayerCount|105_0() <= 0)
				{
					this.UpdateWinner();
					syncData.stateStartLiquidProgressLinear = Mathf.Clamp01((float)((currentTime - syncData.stateStartTime) / (double)this.riseTime));
					syncData.state = ScienceExperimentManager.RisingLiquidState.PreDrainDelay;
					syncData.stateStartTime = currentTime;
					return;
				}
				if (currentTime > syncData.stateStartTime + (double)this.riseTime)
				{
					syncData.stateStartLiquidProgressLinear = 1f;
					syncData.state = ScienceExperimentManager.RisingLiquidState.Full;
					syncData.stateStartTime = currentTime;
					return;
				}
				break;
			case ScienceExperimentManager.RisingLiquidState.Full:
				if (this.<UpdateReliableState>g__GetAlivePlayerCount|105_0() <= 0 || currentTime > syncData.stateStartTime + (double)this.maxFullTime)
				{
					this.UpdateWinner();
					syncData.stateStartLiquidProgressLinear = 1f;
					syncData.state = ScienceExperimentManager.RisingLiquidState.PreDrainDelay;
					syncData.stateStartTime = currentTime;
					return;
				}
				break;
			case ScienceExperimentManager.RisingLiquidState.PreDrainDelay:
				if (currentTime > syncData.stateStartTime + (double)this.preDrainWaitTime)
				{
					syncData.state = ScienceExperimentManager.RisingLiquidState.Draining;
					syncData.stateStartTime = currentTime;
					syncData.activationProgress = 0.0;
					for (int i = 0; i < this.rotatingRings.Length; i++)
					{
						float num2 = Mathf.Repeat(this.rotatingRings[i].resultingAngle, 360f);
						float num3 = UnityEngine.Random.Range(this.rotatingRingRandomAngleRange.x, this.rotatingRingRandomAngleRange.y);
						float num4 = (UnityEngine.Random.Range(0f, 1f) > 0.5f) ? 1f : -1f;
						this.rotatingRings[i].initialAngle = num2;
						this.rotatingRings[i].resultingAngle = num2 + num4 * num3;
					}
					return;
				}
				break;
			case ScienceExperimentManager.RisingLiquidState.Draining:
			{
				double num5 = (1.0 - (double)syncData.stateStartLiquidProgressLinear) * (double)this.drainTime;
				if (currentTime + num5 > syncData.stateStartTime + (double)this.drainTime)
				{
					syncData.stateStartLiquidProgressLinear = 0f;
					syncData.state = ScienceExperimentManager.RisingLiquidState.Drained;
					syncData.stateStartTime = currentTime;
					syncData.activationProgress = 0.0;
				}
				break;
			}
			}
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x001A6018 File Offset: 0x001A4218
		private void UpdateLocalState(double currentTime, ScienceExperimentManager.SyncData syncData)
		{
			switch (syncData.state)
			{
			default:
				this.riseProgressLinear = 0f;
				this.riseProgress = 0f;
				if (!this.debugRandomizingRings)
				{
					this.ringRotationProgress = 1f;
					return;
				}
				break;
			case ScienceExperimentManager.RisingLiquidState.Rising:
			{
				double num = (currentTime - syncData.stateStartTime) / (double)this.riseTime;
				this.riseProgressLinear = Mathf.Clamp01((float)num);
				this.riseProgress = this.animationCurve.Evaluate(this.riseProgressLinear);
				this.ringRotationProgress = 1f;
				return;
			}
			case ScienceExperimentManager.RisingLiquidState.Full:
				this.riseProgressLinear = 1f;
				this.riseProgress = 1f;
				this.ringRotationProgress = 1f;
				return;
			case ScienceExperimentManager.RisingLiquidState.PreDrainDelay:
				this.riseProgressLinear = syncData.stateStartLiquidProgressLinear;
				this.riseProgress = this.animationCurve.Evaluate(this.riseProgressLinear);
				this.ringRotationProgress = 1f;
				return;
			case ScienceExperimentManager.RisingLiquidState.Draining:
			{
				double num2 = (1.0 - (double)syncData.stateStartLiquidProgressLinear) * (double)this.drainTime;
				double num3 = (currentTime + num2 - syncData.stateStartTime) / (double)this.drainTime;
				this.riseProgressLinear = Mathf.Clamp01((float)(1.0 - num3));
				this.riseProgress = this.animationCurve.Evaluate(this.riseProgressLinear);
				this.ringRotationProgress = (float)(currentTime - syncData.stateStartTime) / (this.drainTime * syncData.stateStartLiquidProgressLinear);
				break;
			}
			}
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x001A6184 File Offset: 0x001A4384
		private void UpdateLiquid(float fillProgress)
		{
			float num = Mathf.Lerp(this.minScale, this.maxScale, fillProgress);
			this.liquidMeshTransform.localScale = new Vector3(1f, 1f, num);
			bool active = this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Rising || this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Full || this.reliableState.state == ScienceExperimentManager.RisingLiquidState.PreDrainDelay || this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Draining;
			this.liquidMeshTransform.gameObject.SetActive(active);
			if (this.entryWayLiquidMeshTransform != null)
			{
				float y = 0f;
				float z;
				float z2;
				if (num < this.entryLiquidScaleSyncOpeningBottom.y)
				{
					z = this.entryLiquidScaleSyncOpeningBottom.x;
					z2 = this.entryBridgeQuadMinMaxZHeight.x;
				}
				else if (num < this.entryLiquidScaleSyncOpeningTop.y)
				{
					float num2 = Mathf.InverseLerp(this.entryLiquidScaleSyncOpeningBottom.y, this.entryLiquidScaleSyncOpeningTop.y, num);
					z = Mathf.Lerp(this.entryLiquidScaleSyncOpeningBottom.x, this.entryLiquidScaleSyncOpeningTop.x, num2);
					z2 = Mathf.Lerp(this.entryBridgeQuadMinMaxZHeight.x, this.entryBridgeQuadMinMaxZHeight.y, num2);
					y = this.entryBridgeQuadMaxScaleY * Mathf.Sin(num2 * 3.1415927f);
				}
				else
				{
					float t = Mathf.InverseLerp(this.entryLiquidScaleSyncOpeningTop.y, 0.6f * this.maxScale, num);
					z = Mathf.Lerp(this.entryLiquidScaleSyncOpeningTop.x, this.entryLiquidMaxScale, t);
					z2 = this.entryBridgeQuadMinMaxZHeight.y;
				}
				this.entryWayLiquidMeshTransform.localScale = new Vector3(this.entryWayLiquidMeshTransform.localScale.x, this.entryWayLiquidMeshTransform.localScale.y, z);
				this.entryWayBridgeQuadTransform.localScale = new Vector3(this.entryWayBridgeQuadTransform.localScale.x, y, this.entryWayBridgeQuadTransform.localScale.z);
				this.entryWayBridgeQuadTransform.localPosition = new Vector3(this.entryWayBridgeQuadTransform.localPosition.x, this.entryWayBridgeQuadTransform.localPosition.y, z2);
			}
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x001A63A8 File Offset: 0x001A45A8
		private void UpdateRotatingRings(float rotationProgress)
		{
			for (int i = 0; i < this.rotatingRings.Length; i++)
			{
				float angle = Mathf.Lerp(this.rotatingRings[i].initialAngle, this.rotatingRings[i].resultingAngle, rotationProgress);
				this.rotatingRings[i].ringTransform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
			}
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x001A6414 File Offset: 0x001A4614
		private void UpdateDrainBlocker(double currentTime)
		{
			if (this.reliableState.state != ScienceExperimentManager.RisingLiquidState.Draining)
			{
				this.drainBlocker.position = this.drainBlockerClosedPosition.position;
				return;
			}
			float num = (float)(currentTime - this.reliableState.stateStartTime);
			float num2 = (1f - this.reliableState.stateStartLiquidProgressLinear) * this.drainTime;
			if (this.drainTime - (num + num2) < this.drainBlockerSlideTime)
			{
				this.drainBlocker.position = Vector3.MoveTowards(this.drainBlocker.position, this.drainBlockerClosedPosition.position, this.drainBlockerSlideSpeed * Time.deltaTime);
				return;
			}
			this.drainBlocker.position = Vector3.MoveTowards(this.drainBlocker.position, this.drainBlockerOpenPosition.position, this.drainBlockerSlideSpeed * Time.deltaTime);
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x001A64E8 File Offset: 0x001A46E8
		private void UpdateEffects()
		{
			switch (this.reliableState.state)
			{
			case ScienceExperimentManager.RisingLiquidState.Drained:
				this.hasPlayedEruptionEffects = false;
				this.hasPlayedDrainEffects = false;
				this.eruptionAudioSource.GTStop();
				this.drainAudioSource.GTStop();
				this.rotatingRingsAudioSource.GTStop();
				if (this.elements != null)
				{
					this.elements.sodaEruptionParticles.gameObject.SetActive(false);
					this.elements.sodaFizzParticles.gameObject.SetActive(true);
					if (this.reliableState.activationProgress > 0.0010000000474974513)
					{
						this.fizzParticleEmission.rateOverTimeMultiplier = Mathf.Lerp(this.sodaFizzParticleEmissionMinMax.x, this.sodaFizzParticleEmissionMinMax.y, (float)this.reliableState.activationProgress);
						return;
					}
					this.fizzParticleEmission.rateOverTimeMultiplier = 0f;
					return;
				}
				break;
			case ScienceExperimentManager.RisingLiquidState.Erupting:
				if (!this.hasPlayedEruptionEffects)
				{
					this.eruptionAudioSource.loop = true;
					this.eruptionAudioSource.GTPlay();
					this.hasPlayedEruptionEffects = true;
					if (this.elements != null)
					{
						this.elements.sodaEruptionParticles.gameObject.SetActive(true);
						this.fizzParticleEmission.rateOverTimeMultiplier = this.sodaFizzParticleEmissionMinMax.y;
						return;
					}
				}
				break;
			case ScienceExperimentManager.RisingLiquidState.Rising:
				if (this.elements != null)
				{
					this.fizzParticleEmission.rateOverTimeMultiplier = 0f;
					return;
				}
				break;
			default:
				if (this.elements != null)
				{
					this.elements.sodaFizzParticles.gameObject.SetActive(false);
					this.elements.sodaEruptionParticles.gameObject.SetActive(false);
					this.fizzParticleEmission.rateOverTimeMultiplier = 0f;
				}
				this.hasPlayedEruptionEffects = false;
				this.hasPlayedDrainEffects = false;
				this.eruptionAudioSource.GTStop();
				this.drainAudioSource.GTStop();
				this.rotatingRingsAudioSource.GTStop();
				return;
			case ScienceExperimentManager.RisingLiquidState.Draining:
				this.hasPlayedEruptionEffects = false;
				this.eruptionAudioSource.GTStop();
				if (this.elements != null)
				{
					this.elements.sodaFizzParticles.gameObject.SetActive(false);
					this.elements.sodaEruptionParticles.gameObject.SetActive(false);
					this.fizzParticleEmission.rateOverTimeMultiplier = 0f;
				}
				if (!this.hasPlayedDrainEffects)
				{
					this.drainAudioSource.loop = true;
					this.drainAudioSource.GTPlay();
					this.rotatingRingsAudioSource.loop = true;
					this.rotatingRingsAudioSource.GTPlay();
					this.hasPlayedDrainEffects = true;
				}
				break;
			}
		}

		// Token: 0x06004C87 RID: 19591 RVA: 0x001A6784 File Offset: 0x001A4984
		private void DisableObjectsInContactWithLava(float lavaScale)
		{
			if (this.elements == null)
			{
				return;
			}
			Plane plane = new Plane(this.liquidSurfacePlane.up, this.liquidSurfacePlane.position);
			if (this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Rising)
			{
				for (int i = 0; i < this.elements.disableByLiquidList.Count; i++)
				{
					if (!plane.GetSide(this.elements.disableByLiquidList[i].target.position + this.elements.disableByLiquidList[i].heightOffset * Vector3.up))
					{
						this.elements.disableByLiquidList[i].target.gameObject.SetActive(false);
					}
				}
				return;
			}
			if (this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Draining)
			{
				for (int j = 0; j < this.elements.disableByLiquidList.Count; j++)
				{
					if (plane.GetSide(this.elements.disableByLiquidList[j].target.position + this.elements.disableByLiquidList[j].heightOffset * Vector3.up))
					{
						this.elements.disableByLiquidList[j].target.gameObject.SetActive(true);
					}
				}
			}
		}

		// Token: 0x06004C88 RID: 19592 RVA: 0x001A68F0 File Offset: 0x001A4AF0
		private void UpdateWinner()
		{
			float num = -1f;
			for (int i = 0; i < this.inGamePlayerCount; i++)
			{
				if (!this.inGamePlayerStates[i].touchedLiquid)
				{
					this.lastWinnerId = this.inGamePlayerStates[i].playerId;
					break;
				}
				if (this.inGamePlayerStates[i].touchedLiquidAtProgress > num)
				{
					num = this.inGamePlayerStates[i].touchedLiquidAtProgress;
					this.lastWinnerId = this.inGamePlayerStates[i].playerId;
				}
			}
			this.RefreshWinnerName();
		}

		// Token: 0x06004C89 RID: 19593 RVA: 0x001A6984 File Offset: 0x001A4B84
		private void RefreshWinnerName()
		{
			NetPlayer playerFromId = this.GetPlayerFromId(this.lastWinnerId);
			if (playerFromId != null)
			{
				this.lastWinnerName = playerFromId.NickName;
				return;
			}
			this.lastWinnerName = "None";
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x000624AD File Offset: 0x000606AD
		private NetPlayer GetPlayerFromId(int id)
		{
			if (NetworkSystem.Instance.InRoom)
			{
				return NetworkSystem.Instance.GetPlayer(id);
			}
			if (id == NetworkSystem.Instance.LocalPlayer.ActorNumber)
			{
				return NetworkSystem.Instance.LocalPlayer;
			}
			return null;
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x001A69BC File Offset: 0x001A4BBC
		private void UpdateRefreshWater()
		{
			if (this.refreshWaterVolume != null)
			{
				if (this.RefreshWaterAvailable && !this.refreshWaterVolume.gameObject.activeSelf)
				{
					this.refreshWaterVolume.gameObject.SetActive(true);
					return;
				}
				if (!this.RefreshWaterAvailable && this.refreshWaterVolume.gameObject.activeSelf)
				{
					this.refreshWaterVolume.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06004C8C RID: 19596 RVA: 0x001A6A30 File Offset: 0x001A4C30
		private void ResetGame()
		{
			for (int i = 0; i < this.inGamePlayerCount; i++)
			{
				ScienceExperimentManager.PlayerGameState playerGameState = this.inGamePlayerStates[i];
				playerGameState.touchedLiquid = false;
				playerGameState.touchedLiquidAtProgress = -1f;
				this.inGamePlayerStates[i] = playerGameState;
			}
		}

		// Token: 0x06004C8D RID: 19597 RVA: 0x001A6A7C File Offset: 0x001A4C7C
		public void RestartGame()
		{
			if (base.IsMine)
			{
				this.riseTime = this.riseTimeLookup[(int)this.nextRoundRiseSpeed];
				this.reliableState.state = ScienceExperimentManager.RisingLiquidState.Erupting;
				this.reliableState.stateStartTime = (NetworkSystem.Instance.InRoom ? NetworkSystem.Instance.SimTime : ((double)Time.time));
				this.reliableState.stateStartLiquidProgressLinear = 0f;
				this.reliableState.activationProgress = 1.0;
				this.ResetGame();
			}
		}

		// Token: 0x06004C8E RID: 19598 RVA: 0x001A6B04 File Offset: 0x001A4D04
		public void DebugErupt()
		{
			if (base.IsMine)
			{
				this.riseTime = this.riseTimeLookup[(int)this.nextRoundRiseSpeed];
				this.reliableState.state = ScienceExperimentManager.RisingLiquidState.Erupting;
				this.reliableState.stateStartTime = (NetworkSystem.Instance.InRoom ? NetworkSystem.Instance.SimTime : ((double)Time.time));
				this.reliableState.stateStartLiquidProgressLinear = 0f;
				this.reliableState.activationProgress = 1.0;
			}
		}

		// Token: 0x06004C8F RID: 19599 RVA: 0x001A6B88 File Offset: 0x001A4D88
		public void RandomizeRings()
		{
			for (int i = 0; i < this.rotatingRings.Length; i++)
			{
				float num = Mathf.Repeat(this.rotatingRings[i].resultingAngle, 360f);
				float num2 = UnityEngine.Random.Range(this.rotatingRingRandomAngleRange.x, this.rotatingRingRandomAngleRange.y);
				float num3 = (UnityEngine.Random.Range(0f, 1f) > 0.5f) ? 1f : -1f;
				this.rotatingRings[i].initialAngle = num;
				float num4 = num + num3 * num2;
				if (this.rotatingRingQuantizeAngles)
				{
					num4 = Mathf.Round(num4 / this.rotatingRingAngleSnapDegrees) * this.rotatingRingAngleSnapDegrees;
				}
				this.rotatingRings[i].resultingAngle = num4;
			}
			if (this.rotateRingsCoroutine != null)
			{
				base.StopCoroutine(this.rotateRingsCoroutine);
			}
			this.rotateRingsCoroutine = base.StartCoroutine(this.RotateRingsCoroutine());
		}

		// Token: 0x06004C90 RID: 19600 RVA: 0x000624E5 File Offset: 0x000606E5
		private IEnumerator RotateRingsCoroutine()
		{
			if (this.debugRotateRingsTime > 0.01f)
			{
				float routineStartTime = Time.time;
				this.ringRotationProgress = 0f;
				this.debugRandomizingRings = true;
				while (this.ringRotationProgress < 1f)
				{
					this.ringRotationProgress = (Time.time - routineStartTime) / this.debugRotateRingsTime;
					yield return null;
				}
			}
			this.debugRandomizingRings = false;
			this.ringRotationProgress = 1f;
			yield break;
		}

		// Token: 0x06004C91 RID: 19601 RVA: 0x001A6C7C File Offset: 0x001A4E7C
		public bool GetMaterialIfPlayerInGame(int playerActorNumber, out int materialIndex)
		{
			int i = 0;
			while (i < this.inGamePlayerCount)
			{
				if (this.inGamePlayerStates[i].playerId == playerActorNumber)
				{
					if (this.inGamePlayerStates[i].touchedLiquid)
					{
						materialIndex = 12;
						return true;
					}
					materialIndex = 0;
					return true;
				}
				else
				{
					i++;
				}
			}
			materialIndex = 0;
			return false;
		}

		// Token: 0x06004C92 RID: 19602 RVA: 0x001A6CD0 File Offset: 0x001A4ED0
		private void OnPlayerTagged(NetPlayer taggedPlayer, NetPlayer taggingPlayer)
		{
			if (base.IsMine)
			{
				int num = -1;
				int num2 = -1;
				for (int i = 0; i < this.inGamePlayerCount; i++)
				{
					if (this.inGamePlayerStates[i].playerId == taggedPlayer.ActorNumber)
					{
						num = i;
					}
					else if (this.inGamePlayerStates[i].playerId == taggingPlayer.ActorNumber)
					{
						num2 = i;
					}
					if (num != -1 && num2 != -1)
					{
						break;
					}
				}
				if (num == -1 || num2 == -1)
				{
					return;
				}
				switch (this.tagBehavior)
				{
				case ScienceExperimentManager.TagBehavior.None:
					break;
				case ScienceExperimentManager.TagBehavior.Infect:
					if (this.inGamePlayerStates[num2].touchedLiquid && !this.inGamePlayerStates[num].touchedLiquid)
					{
						ScienceExperimentManager.PlayerGameState playerGameState = this.inGamePlayerStates[num];
						playerGameState.touchedLiquid = true;
						playerGameState.touchedLiquidAtProgress = this.riseProgressLinear;
						this.inGamePlayerStates[num] = playerGameState;
						return;
					}
					break;
				case ScienceExperimentManager.TagBehavior.Revive:
					if (!this.inGamePlayerStates[num2].touchedLiquid && this.inGamePlayerStates[num].touchedLiquid)
					{
						ScienceExperimentManager.PlayerGameState playerGameState2 = this.inGamePlayerStates[num];
						playerGameState2.touchedLiquid = false;
						playerGameState2.touchedLiquidAtProgress = -1f;
						this.inGamePlayerStates[num] = playerGameState2;
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06004C93 RID: 19603 RVA: 0x001A6E14 File Offset: 0x001A5014
		private void OnColliderEnteredVolume(Collider collider)
		{
			VRRig component = collider.attachedRigidbody.gameObject.GetComponent<VRRig>();
			if (component != null && component.creator != null)
			{
				this.PlayerEnteredGameArea(component.creator.ActorNumber);
			}
		}

		// Token: 0x06004C94 RID: 19604 RVA: 0x001A6E54 File Offset: 0x001A5054
		private void OnColliderExitedVolume(Collider collider)
		{
			VRRig component = collider.attachedRigidbody.gameObject.GetComponent<VRRig>();
			if (component != null && component.creator != null)
			{
				this.PlayerExitedGameArea(component.creator.ActorNumber);
			}
		}

		// Token: 0x06004C95 RID: 19605 RVA: 0x001A6E94 File Offset: 0x001A5094
		private void OnColliderEnteredSoda(WaterVolume volume, Collider collider)
		{
			if (collider == GTPlayer.Instance.bodyCollider)
			{
				if (base.IsMine)
				{
					this.PlayerTouchedLava(NetworkSystem.Instance.LocalPlayer.ActorNumber);
					return;
				}
				base.GetView.RPC("PlayerTouchedLavaRPC", RpcTarget.MasterClient, Array.Empty<object>());
			}
		}

		// Token: 0x06004C96 RID: 19606 RVA: 0x00030607 File Offset: 0x0002E807
		private void OnColliderExitedSoda(WaterVolume volume, Collider collider)
		{
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x001A6EE8 File Offset: 0x001A50E8
		private void OnColliderEnteredRefreshWater(WaterVolume volume, Collider collider)
		{
			if (collider == GTPlayer.Instance.bodyCollider)
			{
				if (base.IsMine)
				{
					this.PlayerTouchedRefreshWater(NetworkSystem.Instance.LocalPlayer.ActorNumber);
					return;
				}
				base.GetView.RPC("PlayerTouchedRefreshWaterRPC", RpcTarget.MasterClient, Array.Empty<object>());
			}
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x00030607 File Offset: 0x0002E807
		private void OnColliderExitedRefreshWater(WaterVolume volume, Collider collider)
		{
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x000624F4 File Offset: 0x000606F4
		private void OnProjectileEnteredSodaWater(SlingshotProjectile projectile, Collider collider)
		{
			if (projectile.gameObject.CompareTag(this.mentoProjectileTag))
			{
				this.AddLavaRock(projectile.projectileOwner.ActorNumber);
			}
		}

		// Token: 0x06004C9A RID: 19610 RVA: 0x001A6F3C File Offset: 0x001A513C
		private void AddLavaRock(int playerId)
		{
			if (base.IsMine && this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Drained)
			{
				bool flag = false;
				for (int i = 0; i < this.inGamePlayerCount; i++)
				{
					if (!this.inGamePlayerStates[i].touchedLiquid)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					float num = this.lavaActivationRockProgressVsPlayerCount.Evaluate((float)this.PlayerCount);
					this.reliableState.activationProgress = this.reliableState.activationProgress + (double)num;
				}
			}
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x001A6FB0 File Offset: 0x001A51B0
		public void OnWaterBalloonHitPlayer(NetPlayer hitPlayer)
		{
			bool flag = false;
			for (int i = 0; i < this.inGamePlayerCount; i++)
			{
				if (this.inGamePlayerStates[i].playerId == hitPlayer.ActorNumber)
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (hitPlayer == NetworkSystem.Instance.LocalPlayer)
				{
					this.ValidateLocalPlayerWaterBalloonHit(hitPlayer.ActorNumber);
					return;
				}
				base.GetView.RPC("ValidateLocalPlayerWaterBalloonHitRPC", RpcTarget.Others, new object[]
				{
					hitPlayer.ActorNumber
				});
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06004C9C RID: 19612 RVA: 0x0006251A File Offset: 0x0006071A
		// (set) Token: 0x06004C9D RID: 19613 RVA: 0x00062544 File Offset: 0x00060744
		[Networked]
		[NetworkedWeaved(0, 76)]
		private unsafe ScienceExperimentManager.ScienceManagerData Data
		{
			get
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ScienceExperimentManager.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(ScienceExperimentManager.ScienceManagerData*)(this.Ptr + 0);
			}
			set
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ScienceExperimentManager.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(ScienceExperimentManager.ScienceManagerData*)(this.Ptr + 0) = value;
			}
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x001A7030 File Offset: 0x001A5230
		public override void WriteDataFusion()
		{
			ScienceExperimentManager.ScienceManagerData data = new ScienceExperimentManager.ScienceManagerData((int)this.reliableState.state, this.reliableState.stateStartTime, this.reliableState.stateStartLiquidProgressLinear, this.reliableState.activationProgress, (int)this.nextRoundRiseSpeed, this.riseTime, this.lastWinnerId, this.inGamePlayerCount, this.inGamePlayerStates, this.rotatingRings);
			this.Data = data;
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x001A709C File Offset: 0x001A529C
		public override void ReadDataFusion()
		{
			int num = this.lastWinnerId;
			ScienceExperimentManager.RiseSpeed riseSpeed = this.nextRoundRiseSpeed;
			this.reliableState.state = (ScienceExperimentManager.RisingLiquidState)this.Data.reliableState;
			this.reliableState.stateStartTime = this.Data.stateStartTime;
			this.reliableState.stateStartLiquidProgressLinear = this.Data.stateStartLiquidProgressLinear.ClampSafe(0f, 1f);
			this.reliableState.activationProgress = this.Data.activationProgress.GetFinite();
			this.nextRoundRiseSpeed = (ScienceExperimentManager.RiseSpeed)this.Data.nextRoundRiseSpeed;
			this.riseTime = this.Data.riseTime.GetFinite();
			this.lastWinnerId = this.Data.lastWinnerId;
			this.inGamePlayerCount = Mathf.Clamp(this.Data.inGamePlayerCount, 0, 10);
			for (int i = 0; i < 10; i++)
			{
				this.inGamePlayerStates[i].playerId = this.Data.playerIdArray[i];
				this.inGamePlayerStates[i].touchedLiquid = this.Data.touchedLiquidArray[i];
				this.inGamePlayerStates[i].touchedLiquidAtProgress = this.Data.touchedLiquidAtProgressArray[i].ClampSafe(0f, 1f);
			}
			for (int j = 0; j < this.rotatingRings.Length; j++)
			{
				this.rotatingRings[j].initialAngle = this.Data.initialAngleArray[j].GetFinite();
				this.rotatingRings[j].resultingAngle = this.Data.resultingAngleArray[j].GetFinite();
			}
			float num2 = this.riseProgress;
			this.UpdateLocalState(NetworkSystem.Instance.SimTime, this.reliableState);
			this.localLagRiseProgressOffset = num2 - this.riseProgress;
			if (num != this.lastWinnerId)
			{
				this.RefreshWinnerName();
			}
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x001A72C4 File Offset: 0x001A54C4
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			stream.SendNext((int)this.reliableState.state);
			stream.SendNext(this.reliableState.stateStartTime);
			stream.SendNext(this.reliableState.stateStartLiquidProgressLinear);
			stream.SendNext(this.reliableState.activationProgress);
			stream.SendNext((int)this.nextRoundRiseSpeed);
			stream.SendNext(this.riseTime);
			stream.SendNext(this.lastWinnerId);
			stream.SendNext(this.inGamePlayerCount);
			for (int i = 0; i < 10; i++)
			{
				stream.SendNext(this.inGamePlayerStates[i].playerId);
				stream.SendNext(this.inGamePlayerStates[i].touchedLiquid);
				stream.SendNext(this.inGamePlayerStates[i].touchedLiquidAtProgress);
			}
			for (int j = 0; j < this.rotatingRings.Length; j++)
			{
				stream.SendNext(this.rotatingRings[j].initialAngle);
				stream.SendNext(this.rotatingRings[j].resultingAngle);
			}
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x001A741C File Offset: 0x001A561C
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			int num = this.lastWinnerId;
			ScienceExperimentManager.RiseSpeed riseSpeed = this.nextRoundRiseSpeed;
			this.reliableState.state = (ScienceExperimentManager.RisingLiquidState)((int)stream.ReceiveNext());
			this.reliableState.stateStartTime = ((double)stream.ReceiveNext()).GetFinite();
			this.reliableState.stateStartLiquidProgressLinear = ((float)stream.ReceiveNext()).ClampSafe(0f, 1f);
			this.reliableState.activationProgress = ((double)stream.ReceiveNext()).GetFinite();
			this.nextRoundRiseSpeed = (ScienceExperimentManager.RiseSpeed)((int)stream.ReceiveNext());
			this.riseTime = ((float)stream.ReceiveNext()).GetFinite();
			this.lastWinnerId = (int)stream.ReceiveNext();
			this.inGamePlayerCount = (int)stream.ReceiveNext();
			this.inGamePlayerCount = Mathf.Clamp(this.inGamePlayerCount, 0, 10);
			for (int i = 0; i < 10; i++)
			{
				this.inGamePlayerStates[i].playerId = (int)stream.ReceiveNext();
				this.inGamePlayerStates[i].touchedLiquid = (bool)stream.ReceiveNext();
				this.inGamePlayerStates[i].touchedLiquidAtProgress = ((float)stream.ReceiveNext()).ClampSafe(0f, 1f);
			}
			for (int j = 0; j < this.rotatingRings.Length; j++)
			{
				this.rotatingRings[j].initialAngle = ((float)stream.ReceiveNext()).GetFinite();
				this.rotatingRings[j].resultingAngle = ((float)stream.ReceiveNext()).GetFinite();
			}
			float num2 = this.riseProgress;
			this.UpdateLocalState(NetworkSystem.Instance.SimTime, this.reliableState);
			this.localLagRiseProgressOffset = num2 - this.riseProgress;
			if (num != this.lastWinnerId)
			{
				this.RefreshWinnerName();
			}
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x001A7604 File Offset: 0x001A5804
		private void PlayerEnteredGameArea(int pId)
		{
			if (base.IsMine)
			{
				bool flag = false;
				for (int i = 0; i < this.inGamePlayerCount; i++)
				{
					if (this.inGamePlayerStates[i].playerId == pId)
					{
						flag = true;
						break;
					}
				}
				if (!flag && this.inGamePlayerCount < 10)
				{
					bool touchedLiquid = false;
					this.inGamePlayerStates[this.inGamePlayerCount] = new ScienceExperimentManager.PlayerGameState
					{
						playerId = pId,
						touchedLiquid = touchedLiquid,
						touchedLiquidAtProgress = -1f
					};
					this.inGamePlayerCount++;
					if (this.optPlayersOutOfRoomGameMode)
					{
						GorillaGameModes.GameMode.OptOut(pId);
					}
				}
			}
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x001A76A8 File Offset: 0x001A58A8
		private void PlayerExitedGameArea(int playerId)
		{
			if (base.IsMine)
			{
				int i = 0;
				while (i < this.inGamePlayerCount)
				{
					if (this.inGamePlayerStates[i].playerId == playerId)
					{
						this.inGamePlayerStates[i] = this.inGamePlayerStates[this.inGamePlayerCount - 1];
						this.inGamePlayerCount--;
						if (this.optPlayersOutOfRoomGameMode)
						{
							GorillaGameModes.GameMode.OptIn(playerId);
							return;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x0006256F File Offset: 0x0006076F
		[PunRPC]
		public void PlayerTouchedLavaRPC(PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PlayerTouchedLavaRPC");
			this.PlayerTouchedLava(info.Sender.ActorNumber);
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x001A7720 File Offset: 0x001A5920
		[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
		public unsafe void RPC_PlayerTouchedLava(RpcInfo info = default(RpcInfo))
		{
			if (!this.InvokeRpc)
			{
				NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
				if (base.Runner.Stage != SimulationStages.Resimulate)
				{
					int localAuthorityMask = base.Object.GetLocalAuthorityMask();
					if ((localAuthorityMask & 7) != 0)
					{
						if ((localAuthorityMask & 1) != 1)
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
							if ((localAuthorityMask & 1) == 0)
							{
								return;
							}
						}
						info = RpcInfo.FromLocal(base.Runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
						goto IL_12;
					}
					NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void GorillaTag.ScienceExperimentManager::RPC_PlayerTouchedLava(Fusion.RpcInfo)", base.Object, 7);
				}
				return;
			}
			this.InvokeRpc = false;
			IL_12:
			PhotonMessageInfoWrapped infoWrapped = new PhotonMessageInfoWrapped(info);
			GorillaNot.IncrementRPCCall(infoWrapped, "PlayerTouchedLavaRPC");
			this.PlayerTouchedLava(infoWrapped.Sender.ActorNumber);
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x001A7864 File Offset: 0x001A5A64
		private void PlayerTouchedLava(int playerId)
		{
			if (base.IsMine)
			{
				for (int i = 0; i < this.inGamePlayerCount; i++)
				{
					if (this.inGamePlayerStates[i].playerId == playerId)
					{
						ScienceExperimentManager.PlayerGameState playerGameState = this.inGamePlayerStates[i];
						if (!playerGameState.touchedLiquid)
						{
							playerGameState.touchedLiquidAtProgress = this.riseProgressLinear;
						}
						playerGameState.touchedLiquid = true;
						this.inGamePlayerStates[i] = playerGameState;
						return;
					}
				}
			}
		}

		// Token: 0x06004CA7 RID: 19623 RVA: 0x0006258D File Offset: 0x0006078D
		[PunRPC]
		private void PlayerTouchedRefreshWaterRPC(PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PlayerTouchedRefreshWaterRPC");
			this.PlayerTouchedRefreshWater(info.Sender.ActorNumber);
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x001A78D8 File Offset: 0x001A5AD8
		[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
		private unsafe void RPC_PlayerTouchedRefreshWater(RpcInfo info = default(RpcInfo))
		{
			if (!this.InvokeRpc)
			{
				NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
				if (base.Runner.Stage != SimulationStages.Resimulate)
				{
					int localAuthorityMask = base.Object.GetLocalAuthorityMask();
					if ((localAuthorityMask & 7) != 0)
					{
						if ((localAuthorityMask & 1) != 1)
						{
							if (base.Runner.HasAnyActiveConnections())
							{
								int capacityInBytes = 8;
								SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, capacityInBytes);
								byte* data = SimulationMessage.GetData(ptr);
								int num = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 2), data);
								ptr->Offset = num * 8;
								base.Runner.SendRpc(ptr);
							}
							if ((localAuthorityMask & 1) == 0)
							{
								return;
							}
						}
						info = RpcInfo.FromLocal(base.Runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
						goto IL_12;
					}
					NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void GorillaTag.ScienceExperimentManager::RPC_PlayerTouchedRefreshWater(Fusion.RpcInfo)", base.Object, 7);
				}
				return;
			}
			this.InvokeRpc = false;
			IL_12:
			PhotonMessageInfoWrapped infoWrapped = new PhotonMessageInfoWrapped(info);
			GorillaNot.IncrementRPCCall(infoWrapped, "PlayerTouchedRefreshWaterRPC");
			this.PlayerTouchedRefreshWater(infoWrapped.Sender.ActorNumber);
		}

		// Token: 0x06004CA9 RID: 19625 RVA: 0x001A7A1C File Offset: 0x001A5C1C
		private void PlayerTouchedRefreshWater(int playerId)
		{
			if (base.IsMine && this.RefreshWaterAvailable)
			{
				for (int i = 0; i < this.inGamePlayerCount; i++)
				{
					if (this.inGamePlayerStates[i].playerId == playerId)
					{
						ScienceExperimentManager.PlayerGameState playerGameState = this.inGamePlayerStates[i];
						playerGameState.touchedLiquid = false;
						playerGameState.touchedLiquidAtProgress = -1f;
						this.inGamePlayerStates[i] = playerGameState;
						return;
					}
				}
			}
		}

		// Token: 0x06004CAA RID: 19626 RVA: 0x000625AB File Offset: 0x000607AB
		[PunRPC]
		private void ValidateLocalPlayerWaterBalloonHitRPC(int playerId, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "ValidateLocalPlayerWaterBalloonHitRPC");
			if (playerId == NetworkSystem.Instance.LocalPlayer.ActorNumber)
			{
				this.ValidateLocalPlayerWaterBalloonHit(playerId);
			}
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x001A7A90 File Offset: 0x001A5C90
		[Rpc(InvokeLocal = false)]
		private unsafe void RPC_ValidateLocalPlayerWaterBalloonHit(int playerId, RpcInfo info = default(RpcInfo))
		{
			if (this.InvokeRpc)
			{
				this.InvokeRpc = false;
				GorillaNot.IncrementRPCCall(new PhotonMessageInfoWrapped(info), "ValidateLocalPlayerWaterBalloonHitRPC");
				if (playerId == NetworkSystem.Instance.LocalPlayer.ActorNumber)
				{
					this.ValidateLocalPlayerWaterBalloonHit(playerId);
				}
				return;
			}
			NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
			if (base.Runner.Stage != SimulationStages.Resimulate)
			{
				int localAuthorityMask = base.Object.GetLocalAuthorityMask();
				if ((localAuthorityMask & 7) == 0)
				{
					NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void GorillaTag.ScienceExperimentManager::RPC_ValidateLocalPlayerWaterBalloonHit(System.Int32,Fusion.RpcInfo)", base.Object, 7);
				}
				else if (base.Runner.HasAnyActiveConnections())
				{
					int num = 8;
					num += 4;
					SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, num);
					byte* data = SimulationMessage.GetData(ptr);
					int num2 = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 3), data);
					*(int*)(data + num2) = playerId;
					num2 += 4;
					ptr->Offset = num2 * 8;
					base.Runner.SendRpc(ptr);
				}
			}
		}

		// Token: 0x06004CAC RID: 19628 RVA: 0x001A7BCC File Offset: 0x001A5DCC
		private void ValidateLocalPlayerWaterBalloonHit(int playerId)
		{
			if (playerId == NetworkSystem.Instance.LocalPlayer.ActorNumber && !GTPlayer.Instance.InWater)
			{
				if (base.IsMine)
				{
					this.PlayerHitByWaterBalloon(NetworkSystem.Instance.LocalPlayer.ActorNumber);
					return;
				}
				base.GetView.RPC("PlayerHitByWaterBalloonRPC", RpcTarget.MasterClient, new object[]
				{
					PhotonNetwork.LocalPlayer.ActorNumber
				});
			}
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x000625D1 File Offset: 0x000607D1
		[PunRPC]
		private void PlayerHitByWaterBalloonRPC(int playerId, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PlayerHitByWaterBalloonRPC");
			this.PlayerHitByWaterBalloon(playerId);
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x001A7C40 File Offset: 0x001A5E40
		[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
		private unsafe void RPC_PlayerHitByWaterBalloon(int playerId, RpcInfo info = default(RpcInfo))
		{
			if (!this.InvokeRpc)
			{
				NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
				if (base.Runner.Stage != SimulationStages.Resimulate)
				{
					int localAuthorityMask = base.Object.GetLocalAuthorityMask();
					if ((localAuthorityMask & 7) != 0)
					{
						if ((localAuthorityMask & 1) != 1)
						{
							if (base.Runner.HasAnyActiveConnections())
							{
								int num = 8;
								num += 4;
								SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, num);
								byte* data = SimulationMessage.GetData(ptr);
								int num2 = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 4), data);
								*(int*)(data + num2) = playerId;
								num2 += 4;
								ptr->Offset = num2 * 8;
								base.Runner.SendRpc(ptr);
							}
							if ((localAuthorityMask & 1) == 0)
							{
								return;
							}
						}
						info = RpcInfo.FromLocal(base.Runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
						goto IL_12;
					}
					NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void GorillaTag.ScienceExperimentManager::RPC_PlayerHitByWaterBalloon(System.Int32,Fusion.RpcInfo)", base.Object, 7);
				}
				return;
			}
			this.InvokeRpc = false;
			IL_12:
			GorillaNot.IncrementRPCCall(new PhotonMessageInfoWrapped(info), "PlayerHitByWaterBalloonRPC");
			this.PlayerHitByWaterBalloon(playerId);
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x001A7D98 File Offset: 0x001A5F98
		private void PlayerHitByWaterBalloon(int playerId)
		{
			if (base.IsMine)
			{
				for (int i = 0; i < this.inGamePlayerCount; i++)
				{
					if (this.inGamePlayerStates[i].playerId == playerId)
					{
						ScienceExperimentManager.PlayerGameState playerGameState = this.inGamePlayerStates[i];
						playerGameState.touchedLiquid = false;
						playerGameState.touchedLiquidAtProgress = -1f;
						this.inGamePlayerStates[i] = playerGameState;
						return;
					}
				}
			}
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x000625E5 File Offset: 0x000607E5
		public void OnPlayerLeftRoom(NetPlayer otherPlayer)
		{
			this.PlayerExitedGameArea(otherPlayer.ActorNumber);
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x001A7E04 File Offset: 0x001A6004
		public void OnLeftRoom()
		{
			this.inGamePlayerCount = 0;
			for (int i = 0; i < this.inGamePlayerCount; i++)
			{
				if (this.inGamePlayerStates[i].playerId == NetworkSystem.Instance.LocalPlayer.ActorNumber)
				{
					this.inGamePlayerStates[0] = this.inGamePlayerStates[i];
					this.inGamePlayerCount = 1;
					return;
				}
			}
		}

		// Token: 0x06004CB2 RID: 19634 RVA: 0x001A7E6C File Offset: 0x001A606C
		protected override void OnOwnerSwitched(NetPlayer newOwningPlayer)
		{
			base.OnOwnerSwitched(newOwningPlayer);
			if (!NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			for (int i = 0; i < this.inGamePlayerCount; i++)
			{
				if (!Utils.PlayerInRoom(this.inGamePlayerStates[i].playerId))
				{
					this.inGamePlayerStates[i] = this.inGamePlayerStates[this.inGamePlayerCount - 1];
					this.inGamePlayerCount--;
					i--;
				}
			}
		}

		// Token: 0x06004CB4 RID: 19636 RVA: 0x001A80E8 File Offset: 0x001A62E8
		[CompilerGenerated]
		private int <UpdateReliableState>g__GetAlivePlayerCount|105_0()
		{
			int num = 0;
			for (int i = 0; i < this.inGamePlayerCount; i++)
			{
				if (!this.inGamePlayerStates[i].touchedLiquid)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06004CB5 RID: 19637 RVA: 0x000625F3 File Offset: 0x000607F3
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06004CB6 RID: 19638 RVA: 0x0006260B File Offset: 0x0006080B
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x06004CB7 RID: 19639 RVA: 0x001A8120 File Offset: 0x001A6320
		[NetworkRpcWeavedInvoker(1, 7, 1)]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_PlayerTouchedLava@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
			behaviour.InvokeRpc = true;
			((ScienceExperimentManager)behaviour).RPC_PlayerTouchedLava(info);
		}

		// Token: 0x06004CB8 RID: 19640 RVA: 0x001A8174 File Offset: 0x001A6374
		[NetworkRpcWeavedInvoker(2, 7, 1)]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_PlayerTouchedRefreshWater@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
			behaviour.InvokeRpc = true;
			((ScienceExperimentManager)behaviour).RPC_PlayerTouchedRefreshWater(info);
		}

		// Token: 0x06004CB9 RID: 19641 RVA: 0x001A81C8 File Offset: 0x001A63C8
		[NetworkRpcWeavedInvoker(3, 7, 7)]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_ValidateLocalPlayerWaterBalloonHit@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			int num2 = *(int*)(data + num);
			num += 4;
			int playerId = num2;
			RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
			behaviour.InvokeRpc = true;
			((ScienceExperimentManager)behaviour).RPC_ValidateLocalPlayerWaterBalloonHit(playerId, info);
		}

		// Token: 0x06004CBA RID: 19642 RVA: 0x001A8238 File Offset: 0x001A6438
		[NetworkRpcWeavedInvoker(4, 7, 1)]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_PlayerHitByWaterBalloon@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			int num2 = *(int*)(data + num);
			num += 4;
			int playerId = num2;
			RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
			behaviour.InvokeRpc = true;
			((ScienceExperimentManager)behaviour).RPC_PlayerHitByWaterBalloon(playerId, info);
		}

		// Token: 0x04004DA2 RID: 19874
		public static volatile ScienceExperimentManager instance;

		// Token: 0x04004DA3 RID: 19875
		[SerializeField]
		private ScienceExperimentManager.TagBehavior tagBehavior = ScienceExperimentManager.TagBehavior.Infect;

		// Token: 0x04004DA4 RID: 19876
		[SerializeField]
		private float minScale = 1f;

		// Token: 0x04004DA5 RID: 19877
		[SerializeField]
		private float maxScale = 10f;

		// Token: 0x04004DA6 RID: 19878
		[SerializeField]
		private float riseTimeFast = 30f;

		// Token: 0x04004DA7 RID: 19879
		[SerializeField]
		private float riseTimeMedium = 60f;

		// Token: 0x04004DA8 RID: 19880
		[SerializeField]
		private float riseTimeSlow = 120f;

		// Token: 0x04004DA9 RID: 19881
		[SerializeField]
		private float riseTimeExtraSlow = 240f;

		// Token: 0x04004DAA RID: 19882
		[SerializeField]
		private float preDrainWaitTime = 3f;

		// Token: 0x04004DAB RID: 19883
		[SerializeField]
		private float maxFullTime = 5f;

		// Token: 0x04004DAC RID: 19884
		[SerializeField]
		private float drainTime = 10f;

		// Token: 0x04004DAD RID: 19885
		[SerializeField]
		private float fullyDrainedWaitTime = 3f;

		// Token: 0x04004DAE RID: 19886
		[SerializeField]
		private float lagResolutionLavaProgressPerSecond = 0.2f;

		// Token: 0x04004DAF RID: 19887
		[SerializeField]
		private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004DB0 RID: 19888
		[SerializeField]
		private float lavaProgressToDisableRefreshWater = 0.18f;

		// Token: 0x04004DB1 RID: 19889
		[SerializeField]
		private float lavaProgressToEnableRefreshWater = 0.08f;

		// Token: 0x04004DB2 RID: 19890
		[SerializeField]
		private float entryLiquidMaxScale = 5f;

		// Token: 0x04004DB3 RID: 19891
		[SerializeField]
		private Vector2 entryLiquidScaleSyncOpeningTop = Vector2.zero;

		// Token: 0x04004DB4 RID: 19892
		[SerializeField]
		private Vector2 entryLiquidScaleSyncOpeningBottom = Vector2.zero;

		// Token: 0x04004DB5 RID: 19893
		[SerializeField]
		private float entryBridgeQuadMaxScaleY = 0.0915f;

		// Token: 0x04004DB6 RID: 19894
		[SerializeField]
		private Vector2 entryBridgeQuadMinMaxZHeight = new Vector2(0.245f, 0.337f);

		// Token: 0x04004DB7 RID: 19895
		[SerializeField]
		private AnimationCurve lavaActivationRockProgressVsPlayerCount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004DB8 RID: 19896
		[SerializeField]
		private AnimationCurve lavaActivationDrainRateVsPlayerCount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004DB9 RID: 19897
		[SerializeField]
		public GameObject waterBalloonPrefab;

		// Token: 0x04004DBA RID: 19898
		[SerializeField]
		private Vector2 rotatingRingRandomAngleRange = Vector2.zero;

		// Token: 0x04004DBB RID: 19899
		[SerializeField]
		private bool rotatingRingQuantizeAngles;

		// Token: 0x04004DBC RID: 19900
		[SerializeField]
		private float rotatingRingAngleSnapDegrees = 9f;

		// Token: 0x04004DBD RID: 19901
		[SerializeField]
		private float drainBlockerSlideTime = 4f;

		// Token: 0x04004DBE RID: 19902
		[SerializeField]
		private Vector2 sodaFizzParticleEmissionMinMax = new Vector2(30f, 100f);

		// Token: 0x04004DBF RID: 19903
		[SerializeField]
		private float infrequentUpdatePeriod = 3f;

		// Token: 0x04004DC0 RID: 19904
		[SerializeField]
		private bool optPlayersOutOfRoomGameMode;

		// Token: 0x04004DC1 RID: 19905
		[SerializeField]
		private bool debugDrawPlayerGameState;

		// Token: 0x04004DC2 RID: 19906
		private ScienceExperimentSceneElements elements;

		// Token: 0x04004DC3 RID: 19907
		private NetPlayer[] allPlayersInRoom;

		// Token: 0x04004DC4 RID: 19908
		private ScienceExperimentManager.RotatingRingState[] rotatingRings = new ScienceExperimentManager.RotatingRingState[0];

		// Token: 0x04004DC5 RID: 19909
		private const int maxPlayerCount = 10;

		// Token: 0x04004DC6 RID: 19910
		private ScienceExperimentManager.PlayerGameState[] inGamePlayerStates = new ScienceExperimentManager.PlayerGameState[10];

		// Token: 0x04004DC7 RID: 19911
		private int inGamePlayerCount;

		// Token: 0x04004DC8 RID: 19912
		private int lastWinnerId = -1;

		// Token: 0x04004DC9 RID: 19913
		private string lastWinnerName = "None";

		// Token: 0x04004DCA RID: 19914
		private List<ScienceExperimentManager.PlayerGameState> sortedPlayerStates = new List<ScienceExperimentManager.PlayerGameState>();

		// Token: 0x04004DCB RID: 19915
		private ScienceExperimentManager.SyncData reliableState;

		// Token: 0x04004DCC RID: 19916
		private ScienceExperimentManager.RiseSpeed nextRoundRiseSpeed = ScienceExperimentManager.RiseSpeed.Slow;

		// Token: 0x04004DCD RID: 19917
		private float riseTime = 120f;

		// Token: 0x04004DCE RID: 19918
		private float riseProgress;

		// Token: 0x04004DCF RID: 19919
		private float riseProgressLinear;

		// Token: 0x04004DD0 RID: 19920
		private float localLagRiseProgressOffset;

		// Token: 0x04004DD1 RID: 19921
		private double lastInfrequentUpdateTime = -10.0;

		// Token: 0x04004DD2 RID: 19922
		private string mentoProjectileTag = "ScienceCandyProjectile";

		// Token: 0x04004DD3 RID: 19923
		private double currentTime;

		// Token: 0x04004DD4 RID: 19924
		private double prevTime;

		// Token: 0x04004DD5 RID: 19925
		private float ringRotationProgress = 1f;

		// Token: 0x04004DD6 RID: 19926
		private float drainBlockerSlideSpeed;

		// Token: 0x04004DD7 RID: 19927
		private float[] riseTimeLookup;

		// Token: 0x04004DD8 RID: 19928
		[Header("Scene References")]
		public Transform ringParent;

		// Token: 0x04004DD9 RID: 19929
		public Transform liquidMeshTransform;

		// Token: 0x04004DDA RID: 19930
		public Transform liquidSurfacePlane;

		// Token: 0x04004DDB RID: 19931
		public Transform entryWayLiquidMeshTransform;

		// Token: 0x04004DDC RID: 19932
		public Transform entryWayBridgeQuadTransform;

		// Token: 0x04004DDD RID: 19933
		public Transform drainBlocker;

		// Token: 0x04004DDE RID: 19934
		public Transform drainBlockerClosedPosition;

		// Token: 0x04004DDF RID: 19935
		public Transform drainBlockerOpenPosition;

		// Token: 0x04004DE0 RID: 19936
		public WaterVolume liquidVolume;

		// Token: 0x04004DE1 RID: 19937
		public WaterVolume entryLiquidVolume;

		// Token: 0x04004DE2 RID: 19938
		public WaterVolume bottleLiquidVolume;

		// Token: 0x04004DE3 RID: 19939
		public WaterVolume refreshWaterVolume;

		// Token: 0x04004DE4 RID: 19940
		public CompositeTriggerEvents gameAreaTriggerNotifier;

		// Token: 0x04004DE5 RID: 19941
		public SlingshotProjectileHitNotifier sodaWaterProjectileTriggerNotifier;

		// Token: 0x04004DE6 RID: 19942
		public AudioSource eruptionAudioSource;

		// Token: 0x04004DE7 RID: 19943
		public AudioSource drainAudioSource;

		// Token: 0x04004DE8 RID: 19944
		public AudioSource rotatingRingsAudioSource;

		// Token: 0x04004DE9 RID: 19945
		private ParticleSystem.EmissionModule fizzParticleEmission;

		// Token: 0x04004DEA RID: 19946
		private bool hasPlayedEruptionEffects;

		// Token: 0x04004DEB RID: 19947
		private bool hasPlayedDrainEffects;

		// Token: 0x04004DED RID: 19949
		[SerializeField]
		private float debugRotateRingsTime = 10f;

		// Token: 0x04004DEE RID: 19950
		private Coroutine rotateRingsCoroutine;

		// Token: 0x04004DEF RID: 19951
		private bool debugRandomizingRings;

		// Token: 0x04004DF0 RID: 19952
		[WeaverGenerated]
		[DefaultForProperty("Data", 0, 76)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ScienceExperimentManager.ScienceManagerData _Data;

		// Token: 0x02000BCF RID: 3023
		public enum RisingLiquidState
		{
			// Token: 0x04004DF2 RID: 19954
			Drained,
			// Token: 0x04004DF3 RID: 19955
			Erupting,
			// Token: 0x04004DF4 RID: 19956
			Rising,
			// Token: 0x04004DF5 RID: 19957
			Full,
			// Token: 0x04004DF6 RID: 19958
			PreDrainDelay,
			// Token: 0x04004DF7 RID: 19959
			Draining
		}

		// Token: 0x02000BD0 RID: 3024
		private enum RiseSpeed
		{
			// Token: 0x04004DF9 RID: 19961
			Fast,
			// Token: 0x04004DFA RID: 19962
			Medium,
			// Token: 0x04004DFB RID: 19963
			Slow,
			// Token: 0x04004DFC RID: 19964
			ExtraSlow
		}

		// Token: 0x02000BD1 RID: 3025
		private enum TagBehavior
		{
			// Token: 0x04004DFE RID: 19966
			None,
			// Token: 0x04004DFF RID: 19967
			Infect,
			// Token: 0x04004E00 RID: 19968
			Revive
		}

		// Token: 0x02000BD2 RID: 3026
		[Serializable]
		public struct PlayerGameState
		{
			// Token: 0x04004E01 RID: 19969
			public int playerId;

			// Token: 0x04004E02 RID: 19970
			public bool touchedLiquid;

			// Token: 0x04004E03 RID: 19971
			public float touchedLiquidAtProgress;
		}

		// Token: 0x02000BD3 RID: 3027
		private struct SyncData
		{
			// Token: 0x04004E04 RID: 19972
			public ScienceExperimentManager.RisingLiquidState state;

			// Token: 0x04004E05 RID: 19973
			public double stateStartTime;

			// Token: 0x04004E06 RID: 19974
			public float stateStartLiquidProgressLinear;

			// Token: 0x04004E07 RID: 19975
			public double activationProgress;
		}

		// Token: 0x02000BD4 RID: 3028
		private struct RotatingRingState
		{
			// Token: 0x04004E08 RID: 19976
			public Transform ringTransform;

			// Token: 0x04004E09 RID: 19977
			public float initialAngle;

			// Token: 0x04004E0A RID: 19978
			public float resultingAngle;
		}

		// Token: 0x02000BD5 RID: 3029
		[Serializable]
		private struct DisableByLiquidData
		{
			// Token: 0x04004E0B RID: 19979
			public Transform target;

			// Token: 0x04004E0C RID: 19980
			public float heightOffset;
		}

		// Token: 0x02000BD6 RID: 3030
		[NetworkStructWeaved(76)]
		[StructLayout(LayoutKind.Explicit, Size = 304)]
		private struct ScienceManagerData : INetworkStruct
		{
			// Token: 0x170007EA RID: 2026
			// (get) Token: 0x06004CBB RID: 19643 RVA: 0x0006261F File Offset: 0x0006081F
			[Networked]
			[Capacity(10)]
			public NetworkArray<int> playerIdArray
			{
				get
				{
					return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@10>(ref this._playerIdArray), 10, ReaderWriter@System_Int32.GetInstance());
				}
			}

			// Token: 0x170007EB RID: 2027
			// (get) Token: 0x06004CBC RID: 19644 RVA: 0x0006263B File Offset: 0x0006083B
			[Networked]
			[Capacity(10)]
			public NetworkArray<bool> touchedLiquidArray
			{
				get
				{
					return new NetworkArray<bool>(Native.ReferenceToPointer<FixedStorage@10>(ref this._touchedLiquidArray), 10, ReaderWriter@System_Boolean.GetInstance());
				}
			}

			// Token: 0x170007EC RID: 2028
			// (get) Token: 0x06004CBD RID: 19645 RVA: 0x00062657 File Offset: 0x00060857
			[Networked]
			[Capacity(10)]
			public NetworkArray<float> touchedLiquidAtProgressArray
			{
				get
				{
					return new NetworkArray<float>(Native.ReferenceToPointer<FixedStorage@10>(ref this._touchedLiquidAtProgressArray), 10, ReaderWriter@System_Single.GetInstance());
				}
			}

			// Token: 0x170007ED RID: 2029
			// (get) Token: 0x06004CBE RID: 19646 RVA: 0x00062673 File Offset: 0x00060873
			[Networked]
			[Capacity(5)]
			public NetworkLinkedList<float> initialAngleArray
			{
				get
				{
					return new NetworkLinkedList<float>(Native.ReferenceToPointer<FixedStorage@18>(ref this._initialAngleArray), 5, ReaderWriter@System_Single.GetInstance());
				}
			}

			// Token: 0x170007EE RID: 2030
			// (get) Token: 0x06004CBF RID: 19647 RVA: 0x0006268B File Offset: 0x0006088B
			[Networked]
			[Capacity(5)]
			public NetworkLinkedList<float> resultingAngleArray
			{
				get
				{
					return new NetworkLinkedList<float>(Native.ReferenceToPointer<FixedStorage@18>(ref this._resultingAngleArray), 5, ReaderWriter@System_Single.GetInstance());
				}
			}

			// Token: 0x06004CC0 RID: 19648 RVA: 0x001A82A8 File Offset: 0x001A64A8
			public ScienceManagerData(int reliableState, double stateStartTime, float stateStartLiquidProgressLinear, double activationProgress, int nextRoundRiseSpeed, float riseTime, int lastWinnerId, int inGamePlayerCount, ScienceExperimentManager.PlayerGameState[] playerStates, ScienceExperimentManager.RotatingRingState[] rings)
			{
				this.reliableState = reliableState;
				this.stateStartTime = stateStartTime;
				this.stateStartLiquidProgressLinear = stateStartLiquidProgressLinear;
				this.activationProgress = activationProgress;
				this.nextRoundRiseSpeed = nextRoundRiseSpeed;
				this.riseTime = riseTime;
				this.lastWinnerId = lastWinnerId;
				this.inGamePlayerCount = inGamePlayerCount;
				foreach (ScienceExperimentManager.RotatingRingState rotatingRingState in rings)
				{
					this.initialAngleArray.Add(rotatingRingState.initialAngle);
					this.resultingAngleArray.Add(rotatingRingState.resultingAngle);
				}
				int[] array = new int[10];
				bool[] array2 = new bool[10];
				float[] array3 = new float[10];
				for (int j = 0; j < 10; j++)
				{
					array[j] = playerStates[j].playerId;
					array2[j] = playerStates[j].touchedLiquid;
					array3[j] = playerStates[j].touchedLiquidAtProgress;
				}
				this.playerIdArray.CopyFrom(array, 0, array.Length);
				this.touchedLiquidArray.CopyFrom(array2, 0, array2.Length);
				this.touchedLiquidAtProgressArray.CopyFrom(array3, 0, array3.Length);
			}

			// Token: 0x04004E0D RID: 19981
			[FieldOffset(0)]
			public int reliableState;

			// Token: 0x04004E0E RID: 19982
			[FieldOffset(4)]
			public double stateStartTime;

			// Token: 0x04004E0F RID: 19983
			[FieldOffset(12)]
			public float stateStartLiquidProgressLinear;

			// Token: 0x04004E10 RID: 19984
			[FieldOffset(16)]
			public double activationProgress;

			// Token: 0x04004E11 RID: 19985
			[FieldOffset(24)]
			public int nextRoundRiseSpeed;

			// Token: 0x04004E12 RID: 19986
			[FieldOffset(28)]
			public float riseTime;

			// Token: 0x04004E13 RID: 19987
			[FieldOffset(32)]
			public int lastWinnerId;

			// Token: 0x04004E14 RID: 19988
			[FieldOffset(36)]
			public int inGamePlayerCount;

			// Token: 0x04004E15 RID: 19989
			[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(40)]
			private FixedStorage@10 _playerIdArray;

			// Token: 0x04004E16 RID: 19990
			[FixedBufferProperty(typeof(NetworkArray<bool>), typeof(UnityArraySurrogate@ReaderWriter@System_Boolean), 10, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(80)]
			private FixedStorage@10 _touchedLiquidArray;

			// Token: 0x04004E17 RID: 19991
			[FixedBufferProperty(typeof(NetworkArray<float>), typeof(UnityArraySurrogate@ReaderWriter@System_Single), 10, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(120)]
			private FixedStorage@10 _touchedLiquidAtProgressArray;

			// Token: 0x04004E18 RID: 19992
			[FixedBufferProperty(typeof(NetworkLinkedList<float>), typeof(UnityLinkedListSurrogate@ReaderWriter@System_Single), 5, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(160)]
			private FixedStorage@18 _initialAngleArray;

			// Token: 0x04004E19 RID: 19993
			[FixedBufferProperty(typeof(NetworkLinkedList<float>), typeof(UnityLinkedListSurrogate@ReaderWriter@System_Single), 5, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(232)]
			private FixedStorage@18 _resultingAngleArray;
		}
	}
}
