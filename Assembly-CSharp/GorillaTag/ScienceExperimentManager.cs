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
	// Token: 0x02000BA2 RID: 2978
	[NetworkBehaviourWeaved(76)]
	public class ScienceExperimentManager : NetworkComponent, ITickSystemTick
	{
		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06004B30 RID: 19248 RVA: 0x0016C184 File Offset: 0x0016A384
		private bool RefreshWaterAvailable
		{
			get
			{
				return this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Drained || this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Erupting || (this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Rising && this.riseProgress < this.lavaProgressToDisableRefreshWater) || (this.reliableState.state == ScienceExperimentManager.RisingLiquidState.Draining && this.riseProgress < this.lavaProgressToEnableRefreshWater);
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06004B31 RID: 19249 RVA: 0x0016C1E8 File Offset: 0x0016A3E8
		public ScienceExperimentManager.RisingLiquidState GameState
		{
			get
			{
				return this.reliableState.state;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06004B32 RID: 19250 RVA: 0x0016C1F5 File Offset: 0x0016A3F5
		public float RiseProgress
		{
			get
			{
				return this.riseProgress;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06004B33 RID: 19251 RVA: 0x0016C1FD File Offset: 0x0016A3FD
		public float RiseProgressLinear
		{
			get
			{
				return this.riseProgressLinear;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06004B34 RID: 19252 RVA: 0x0016C208 File Offset: 0x0016A408
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

		// Token: 0x06004B35 RID: 19253 RVA: 0x0016C238 File Offset: 0x0016A438
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

		// Token: 0x06004B36 RID: 19254 RVA: 0x0016C4E2 File Offset: 0x0016A6E2
		internal override void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			base.OnEnable();
			TickSystem<object>.AddTickCallback(this);
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x0016C4F6 File Offset: 0x0016A6F6
		internal override void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			base.OnDisable();
			TickSystem<object>.RemoveTickCallback(this);
		}

		// Token: 0x06004B38 RID: 19256 RVA: 0x0016C50C File Offset: 0x0016A70C
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

		// Token: 0x06004B39 RID: 19257 RVA: 0x0016C684 File Offset: 0x0016A884
		public void InitElements(ScienceExperimentSceneElements elements)
		{
			this.elements = elements;
			this.fizzParticleEmission = elements.sodaFizzParticles.emission;
			elements.sodaFizzParticles.gameObject.SetActive(false);
			elements.sodaEruptionParticles.gameObject.SetActive(false);
			RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
		}

		// Token: 0x06004B3A RID: 19258 RVA: 0x0016C6EB File Offset: 0x0016A8EB
		public void DeInitElements()
		{
			this.elements = null;
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x0016C6F4 File Offset: 0x0016A8F4
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

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06004B3C RID: 19260 RVA: 0x0016C7B9 File Offset: 0x0016A9B9
		// (set) Token: 0x06004B3D RID: 19261 RVA: 0x0016C7C1 File Offset: 0x0016A9C1
		bool ITickSystemTick.TickRunning { get; set; }

		// Token: 0x06004B3E RID: 19262 RVA: 0x0016C7CC File Offset: 0x0016A9CC
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

		// Token: 0x06004B3F RID: 19263 RVA: 0x0016C9EC File Offset: 0x0016ABEC
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

		// Token: 0x06004B40 RID: 19264 RVA: 0x0016CB38 File Offset: 0x0016AD38
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

		// Token: 0x06004B41 RID: 19265 RVA: 0x0016CB74 File Offset: 0x0016AD74
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
						float num3 = Random.Range(this.rotatingRingRandomAngleRange.x, this.rotatingRingRandomAngleRange.y);
						float num4 = (Random.Range(0f, 1f) > 0.5f) ? 1f : -1f;
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

		// Token: 0x06004B42 RID: 19266 RVA: 0x0016CE50 File Offset: 0x0016B050
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

		// Token: 0x06004B43 RID: 19267 RVA: 0x0016CFBC File Offset: 0x0016B1BC
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

		// Token: 0x06004B44 RID: 19268 RVA: 0x0016D1E0 File Offset: 0x0016B3E0
		private void UpdateRotatingRings(float rotationProgress)
		{
			for (int i = 0; i < this.rotatingRings.Length; i++)
			{
				float angle = Mathf.Lerp(this.rotatingRings[i].initialAngle, this.rotatingRings[i].resultingAngle, rotationProgress);
				this.rotatingRings[i].ringTransform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
			}
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x0016D24C File Offset: 0x0016B44C
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

		// Token: 0x06004B46 RID: 19270 RVA: 0x0016D320 File Offset: 0x0016B520
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

		// Token: 0x06004B47 RID: 19271 RVA: 0x0016D5BC File Offset: 0x0016B7BC
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

		// Token: 0x06004B48 RID: 19272 RVA: 0x0016D728 File Offset: 0x0016B928
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

		// Token: 0x06004B49 RID: 19273 RVA: 0x0016D7BC File Offset: 0x0016B9BC
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

		// Token: 0x06004B4A RID: 19274 RVA: 0x0016D7F1 File Offset: 0x0016B9F1
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

		// Token: 0x06004B4B RID: 19275 RVA: 0x0016D82C File Offset: 0x0016BA2C
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

		// Token: 0x06004B4C RID: 19276 RVA: 0x0016D8A0 File Offset: 0x0016BAA0
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

		// Token: 0x06004B4D RID: 19277 RVA: 0x0016D8EC File Offset: 0x0016BAEC
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

		// Token: 0x06004B4E RID: 19278 RVA: 0x0016D974 File Offset: 0x0016BB74
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

		// Token: 0x06004B4F RID: 19279 RVA: 0x0016D9F8 File Offset: 0x0016BBF8
		public void RandomizeRings()
		{
			for (int i = 0; i < this.rotatingRings.Length; i++)
			{
				float num = Mathf.Repeat(this.rotatingRings[i].resultingAngle, 360f);
				float num2 = Random.Range(this.rotatingRingRandomAngleRange.x, this.rotatingRingRandomAngleRange.y);
				float num3 = (Random.Range(0f, 1f) > 0.5f) ? 1f : -1f;
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

		// Token: 0x06004B50 RID: 19280 RVA: 0x0016DAEA File Offset: 0x0016BCEA
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

		// Token: 0x06004B51 RID: 19281 RVA: 0x0016DAFC File Offset: 0x0016BCFC
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

		// Token: 0x06004B52 RID: 19282 RVA: 0x0016DB50 File Offset: 0x0016BD50
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

		// Token: 0x06004B53 RID: 19283 RVA: 0x0016DC94 File Offset: 0x0016BE94
		private void OnColliderEnteredVolume(Collider collider)
		{
			VRRig component = collider.attachedRigidbody.gameObject.GetComponent<VRRig>();
			if (component != null && component.creator != null)
			{
				this.PlayerEnteredGameArea(component.creator.ActorNumber);
			}
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x0016DCD4 File Offset: 0x0016BED4
		private void OnColliderExitedVolume(Collider collider)
		{
			VRRig component = collider.attachedRigidbody.gameObject.GetComponent<VRRig>();
			if (component != null && component.creator != null)
			{
				this.PlayerExitedGameArea(component.creator.ActorNumber);
			}
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x0016DD14 File Offset: 0x0016BF14
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

		// Token: 0x06004B56 RID: 19286 RVA: 0x000023F4 File Offset: 0x000005F4
		private void OnColliderExitedSoda(WaterVolume volume, Collider collider)
		{
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x0016DD68 File Offset: 0x0016BF68
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

		// Token: 0x06004B58 RID: 19288 RVA: 0x000023F4 File Offset: 0x000005F4
		private void OnColliderExitedRefreshWater(WaterVolume volume, Collider collider)
		{
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x0016DDBB File Offset: 0x0016BFBB
		private void OnProjectileEnteredSodaWater(SlingshotProjectile projectile, Collider collider)
		{
			if (projectile.gameObject.CompareTag(this.mentoProjectileTag))
			{
				this.AddLavaRock(projectile.projectileOwner.ActorNumber);
			}
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x0016DDE4 File Offset: 0x0016BFE4
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

		// Token: 0x06004B5B RID: 19291 RVA: 0x0016DE58 File Offset: 0x0016C058
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

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06004B5C RID: 19292 RVA: 0x0016DED5 File Offset: 0x0016C0D5
		// (set) Token: 0x06004B5D RID: 19293 RVA: 0x0016DEFF File Offset: 0x0016C0FF
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

		// Token: 0x06004B5E RID: 19294 RVA: 0x0016DF2C File Offset: 0x0016C12C
		public override void WriteDataFusion()
		{
			ScienceExperimentManager.ScienceManagerData data = new ScienceExperimentManager.ScienceManagerData((int)this.reliableState.state, this.reliableState.stateStartTime, this.reliableState.stateStartLiquidProgressLinear, this.reliableState.activationProgress, (int)this.nextRoundRiseSpeed, this.riseTime, this.lastWinnerId, this.inGamePlayerCount, this.inGamePlayerStates, this.rotatingRings);
			this.Data = data;
		}

		// Token: 0x06004B5F RID: 19295 RVA: 0x0016DF98 File Offset: 0x0016C198
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

		// Token: 0x06004B60 RID: 19296 RVA: 0x0016E1C0 File Offset: 0x0016C3C0
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

		// Token: 0x06004B61 RID: 19297 RVA: 0x0016E318 File Offset: 0x0016C518
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

		// Token: 0x06004B62 RID: 19298 RVA: 0x0016E500 File Offset: 0x0016C700
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

		// Token: 0x06004B63 RID: 19299 RVA: 0x0016E5A4 File Offset: 0x0016C7A4
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

		// Token: 0x06004B64 RID: 19300 RVA: 0x0016E61A File Offset: 0x0016C81A
		[PunRPC]
		public void PlayerTouchedLavaRPC(PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PlayerTouchedLavaRPC");
			this.PlayerTouchedLava(info.Sender.ActorNumber);
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x0016E638 File Offset: 0x0016C838
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

		// Token: 0x06004B66 RID: 19302 RVA: 0x0016E77C File Offset: 0x0016C97C
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

		// Token: 0x06004B67 RID: 19303 RVA: 0x0016E7EE File Offset: 0x0016C9EE
		[PunRPC]
		private void PlayerTouchedRefreshWaterRPC(PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PlayerTouchedRefreshWaterRPC");
			this.PlayerTouchedRefreshWater(info.Sender.ActorNumber);
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x0016E80C File Offset: 0x0016CA0C
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

		// Token: 0x06004B69 RID: 19305 RVA: 0x0016E950 File Offset: 0x0016CB50
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

		// Token: 0x06004B6A RID: 19306 RVA: 0x0016E9C1 File Offset: 0x0016CBC1
		[PunRPC]
		private void ValidateLocalPlayerWaterBalloonHitRPC(int playerId, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "ValidateLocalPlayerWaterBalloonHitRPC");
			if (playerId == NetworkSystem.Instance.LocalPlayer.ActorNumber)
			{
				this.ValidateLocalPlayerWaterBalloonHit(playerId);
			}
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x0016E9E8 File Offset: 0x0016CBE8
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

		// Token: 0x06004B6C RID: 19308 RVA: 0x0016EB24 File Offset: 0x0016CD24
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

		// Token: 0x06004B6D RID: 19309 RVA: 0x0016EB96 File Offset: 0x0016CD96
		[PunRPC]
		private void PlayerHitByWaterBalloonRPC(int playerId, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PlayerHitByWaterBalloonRPC");
			this.PlayerHitByWaterBalloon(playerId);
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x0016EBAC File Offset: 0x0016CDAC
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

		// Token: 0x06004B6F RID: 19311 RVA: 0x0016ED04 File Offset: 0x0016CF04
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

		// Token: 0x06004B70 RID: 19312 RVA: 0x0016ED6D File Offset: 0x0016CF6D
		public void OnPlayerLeftRoom(NetPlayer otherPlayer)
		{
			this.PlayerExitedGameArea(otherPlayer.ActorNumber);
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x0016ED7C File Offset: 0x0016CF7C
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

		// Token: 0x06004B72 RID: 19314 RVA: 0x0016EDE4 File Offset: 0x0016CFE4
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

		// Token: 0x06004B74 RID: 19316 RVA: 0x0016F060 File Offset: 0x0016D260
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

		// Token: 0x06004B75 RID: 19317 RVA: 0x0016F098 File Offset: 0x0016D298
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x0016F0B0 File Offset: 0x0016D2B0
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x0016F0C4 File Offset: 0x0016D2C4
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

		// Token: 0x06004B78 RID: 19320 RVA: 0x0016F118 File Offset: 0x0016D318
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

		// Token: 0x06004B79 RID: 19321 RVA: 0x0016F16C File Offset: 0x0016D36C
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

		// Token: 0x06004B7A RID: 19322 RVA: 0x0016F1DC File Offset: 0x0016D3DC
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

		// Token: 0x04004CAF RID: 19631
		public static volatile ScienceExperimentManager instance;

		// Token: 0x04004CB0 RID: 19632
		[SerializeField]
		private ScienceExperimentManager.TagBehavior tagBehavior = ScienceExperimentManager.TagBehavior.Infect;

		// Token: 0x04004CB1 RID: 19633
		[SerializeField]
		private float minScale = 1f;

		// Token: 0x04004CB2 RID: 19634
		[SerializeField]
		private float maxScale = 10f;

		// Token: 0x04004CB3 RID: 19635
		[SerializeField]
		private float riseTimeFast = 30f;

		// Token: 0x04004CB4 RID: 19636
		[SerializeField]
		private float riseTimeMedium = 60f;

		// Token: 0x04004CB5 RID: 19637
		[SerializeField]
		private float riseTimeSlow = 120f;

		// Token: 0x04004CB6 RID: 19638
		[SerializeField]
		private float riseTimeExtraSlow = 240f;

		// Token: 0x04004CB7 RID: 19639
		[SerializeField]
		private float preDrainWaitTime = 3f;

		// Token: 0x04004CB8 RID: 19640
		[SerializeField]
		private float maxFullTime = 5f;

		// Token: 0x04004CB9 RID: 19641
		[SerializeField]
		private float drainTime = 10f;

		// Token: 0x04004CBA RID: 19642
		[SerializeField]
		private float fullyDrainedWaitTime = 3f;

		// Token: 0x04004CBB RID: 19643
		[SerializeField]
		private float lagResolutionLavaProgressPerSecond = 0.2f;

		// Token: 0x04004CBC RID: 19644
		[SerializeField]
		private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004CBD RID: 19645
		[SerializeField]
		private float lavaProgressToDisableRefreshWater = 0.18f;

		// Token: 0x04004CBE RID: 19646
		[SerializeField]
		private float lavaProgressToEnableRefreshWater = 0.08f;

		// Token: 0x04004CBF RID: 19647
		[SerializeField]
		private float entryLiquidMaxScale = 5f;

		// Token: 0x04004CC0 RID: 19648
		[SerializeField]
		private Vector2 entryLiquidScaleSyncOpeningTop = Vector2.zero;

		// Token: 0x04004CC1 RID: 19649
		[SerializeField]
		private Vector2 entryLiquidScaleSyncOpeningBottom = Vector2.zero;

		// Token: 0x04004CC2 RID: 19650
		[SerializeField]
		private float entryBridgeQuadMaxScaleY = 0.0915f;

		// Token: 0x04004CC3 RID: 19651
		[SerializeField]
		private Vector2 entryBridgeQuadMinMaxZHeight = new Vector2(0.245f, 0.337f);

		// Token: 0x04004CC4 RID: 19652
		[SerializeField]
		private AnimationCurve lavaActivationRockProgressVsPlayerCount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004CC5 RID: 19653
		[SerializeField]
		private AnimationCurve lavaActivationDrainRateVsPlayerCount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004CC6 RID: 19654
		[SerializeField]
		public GameObject waterBalloonPrefab;

		// Token: 0x04004CC7 RID: 19655
		[SerializeField]
		private Vector2 rotatingRingRandomAngleRange = Vector2.zero;

		// Token: 0x04004CC8 RID: 19656
		[SerializeField]
		private bool rotatingRingQuantizeAngles;

		// Token: 0x04004CC9 RID: 19657
		[SerializeField]
		private float rotatingRingAngleSnapDegrees = 9f;

		// Token: 0x04004CCA RID: 19658
		[SerializeField]
		private float drainBlockerSlideTime = 4f;

		// Token: 0x04004CCB RID: 19659
		[SerializeField]
		private Vector2 sodaFizzParticleEmissionMinMax = new Vector2(30f, 100f);

		// Token: 0x04004CCC RID: 19660
		[SerializeField]
		private float infrequentUpdatePeriod = 3f;

		// Token: 0x04004CCD RID: 19661
		[SerializeField]
		private bool optPlayersOutOfRoomGameMode;

		// Token: 0x04004CCE RID: 19662
		[SerializeField]
		private bool debugDrawPlayerGameState;

		// Token: 0x04004CCF RID: 19663
		private ScienceExperimentSceneElements elements;

		// Token: 0x04004CD0 RID: 19664
		private NetPlayer[] allPlayersInRoom;

		// Token: 0x04004CD1 RID: 19665
		private ScienceExperimentManager.RotatingRingState[] rotatingRings = new ScienceExperimentManager.RotatingRingState[0];

		// Token: 0x04004CD2 RID: 19666
		private const int maxPlayerCount = 10;

		// Token: 0x04004CD3 RID: 19667
		private ScienceExperimentManager.PlayerGameState[] inGamePlayerStates = new ScienceExperimentManager.PlayerGameState[10];

		// Token: 0x04004CD4 RID: 19668
		private int inGamePlayerCount;

		// Token: 0x04004CD5 RID: 19669
		private int lastWinnerId = -1;

		// Token: 0x04004CD6 RID: 19670
		private string lastWinnerName = "None";

		// Token: 0x04004CD7 RID: 19671
		private List<ScienceExperimentManager.PlayerGameState> sortedPlayerStates = new List<ScienceExperimentManager.PlayerGameState>();

		// Token: 0x04004CD8 RID: 19672
		private ScienceExperimentManager.SyncData reliableState;

		// Token: 0x04004CD9 RID: 19673
		private ScienceExperimentManager.RiseSpeed nextRoundRiseSpeed = ScienceExperimentManager.RiseSpeed.Slow;

		// Token: 0x04004CDA RID: 19674
		private float riseTime = 120f;

		// Token: 0x04004CDB RID: 19675
		private float riseProgress;

		// Token: 0x04004CDC RID: 19676
		private float riseProgressLinear;

		// Token: 0x04004CDD RID: 19677
		private float localLagRiseProgressOffset;

		// Token: 0x04004CDE RID: 19678
		private double lastInfrequentUpdateTime = -10.0;

		// Token: 0x04004CDF RID: 19679
		private string mentoProjectileTag = "ScienceCandyProjectile";

		// Token: 0x04004CE0 RID: 19680
		private double currentTime;

		// Token: 0x04004CE1 RID: 19681
		private double prevTime;

		// Token: 0x04004CE2 RID: 19682
		private float ringRotationProgress = 1f;

		// Token: 0x04004CE3 RID: 19683
		private float drainBlockerSlideSpeed;

		// Token: 0x04004CE4 RID: 19684
		private float[] riseTimeLookup;

		// Token: 0x04004CE5 RID: 19685
		[Header("Scene References")]
		public Transform ringParent;

		// Token: 0x04004CE6 RID: 19686
		public Transform liquidMeshTransform;

		// Token: 0x04004CE7 RID: 19687
		public Transform liquidSurfacePlane;

		// Token: 0x04004CE8 RID: 19688
		public Transform entryWayLiquidMeshTransform;

		// Token: 0x04004CE9 RID: 19689
		public Transform entryWayBridgeQuadTransform;

		// Token: 0x04004CEA RID: 19690
		public Transform drainBlocker;

		// Token: 0x04004CEB RID: 19691
		public Transform drainBlockerClosedPosition;

		// Token: 0x04004CEC RID: 19692
		public Transform drainBlockerOpenPosition;

		// Token: 0x04004CED RID: 19693
		public WaterVolume liquidVolume;

		// Token: 0x04004CEE RID: 19694
		public WaterVolume entryLiquidVolume;

		// Token: 0x04004CEF RID: 19695
		public WaterVolume bottleLiquidVolume;

		// Token: 0x04004CF0 RID: 19696
		public WaterVolume refreshWaterVolume;

		// Token: 0x04004CF1 RID: 19697
		public CompositeTriggerEvents gameAreaTriggerNotifier;

		// Token: 0x04004CF2 RID: 19698
		public SlingshotProjectileHitNotifier sodaWaterProjectileTriggerNotifier;

		// Token: 0x04004CF3 RID: 19699
		public AudioSource eruptionAudioSource;

		// Token: 0x04004CF4 RID: 19700
		public AudioSource drainAudioSource;

		// Token: 0x04004CF5 RID: 19701
		public AudioSource rotatingRingsAudioSource;

		// Token: 0x04004CF6 RID: 19702
		private ParticleSystem.EmissionModule fizzParticleEmission;

		// Token: 0x04004CF7 RID: 19703
		private bool hasPlayedEruptionEffects;

		// Token: 0x04004CF8 RID: 19704
		private bool hasPlayedDrainEffects;

		// Token: 0x04004CFA RID: 19706
		[SerializeField]
		private float debugRotateRingsTime = 10f;

		// Token: 0x04004CFB RID: 19707
		private Coroutine rotateRingsCoroutine;

		// Token: 0x04004CFC RID: 19708
		private bool debugRandomizingRings;

		// Token: 0x04004CFD RID: 19709
		[WeaverGenerated]
		[DefaultForProperty("Data", 0, 76)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ScienceExperimentManager.ScienceManagerData _Data;

		// Token: 0x02000BA3 RID: 2979
		public enum RisingLiquidState
		{
			// Token: 0x04004CFF RID: 19711
			Drained,
			// Token: 0x04004D00 RID: 19712
			Erupting,
			// Token: 0x04004D01 RID: 19713
			Rising,
			// Token: 0x04004D02 RID: 19714
			Full,
			// Token: 0x04004D03 RID: 19715
			PreDrainDelay,
			// Token: 0x04004D04 RID: 19716
			Draining
		}

		// Token: 0x02000BA4 RID: 2980
		private enum RiseSpeed
		{
			// Token: 0x04004D06 RID: 19718
			Fast,
			// Token: 0x04004D07 RID: 19719
			Medium,
			// Token: 0x04004D08 RID: 19720
			Slow,
			// Token: 0x04004D09 RID: 19721
			ExtraSlow
		}

		// Token: 0x02000BA5 RID: 2981
		private enum TagBehavior
		{
			// Token: 0x04004D0B RID: 19723
			None,
			// Token: 0x04004D0C RID: 19724
			Infect,
			// Token: 0x04004D0D RID: 19725
			Revive
		}

		// Token: 0x02000BA6 RID: 2982
		[Serializable]
		public struct PlayerGameState
		{
			// Token: 0x04004D0E RID: 19726
			public int playerId;

			// Token: 0x04004D0F RID: 19727
			public bool touchedLiquid;

			// Token: 0x04004D10 RID: 19728
			public float touchedLiquidAtProgress;
		}

		// Token: 0x02000BA7 RID: 2983
		private struct SyncData
		{
			// Token: 0x04004D11 RID: 19729
			public ScienceExperimentManager.RisingLiquidState state;

			// Token: 0x04004D12 RID: 19730
			public double stateStartTime;

			// Token: 0x04004D13 RID: 19731
			public float stateStartLiquidProgressLinear;

			// Token: 0x04004D14 RID: 19732
			public double activationProgress;
		}

		// Token: 0x02000BA8 RID: 2984
		private struct RotatingRingState
		{
			// Token: 0x04004D15 RID: 19733
			public Transform ringTransform;

			// Token: 0x04004D16 RID: 19734
			public float initialAngle;

			// Token: 0x04004D17 RID: 19735
			public float resultingAngle;
		}

		// Token: 0x02000BA9 RID: 2985
		[Serializable]
		private struct DisableByLiquidData
		{
			// Token: 0x04004D18 RID: 19736
			public Transform target;

			// Token: 0x04004D19 RID: 19737
			public float heightOffset;
		}

		// Token: 0x02000BAA RID: 2986
		[NetworkStructWeaved(76)]
		[StructLayout(LayoutKind.Explicit, Size = 304)]
		private struct ScienceManagerData : INetworkStruct
		{
			// Token: 0x170007CE RID: 1998
			// (get) Token: 0x06004B7B RID: 19323 RVA: 0x0016F24C File Offset: 0x0016D44C
			[Networked]
			[Capacity(10)]
			public NetworkArray<int> playerIdArray
			{
				get
				{
					return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@10>(ref this._playerIdArray), 10, ReaderWriter@System_Int32.GetInstance());
				}
			}

			// Token: 0x170007CF RID: 1999
			// (get) Token: 0x06004B7C RID: 19324 RVA: 0x0016F274 File Offset: 0x0016D474
			[Networked]
			[Capacity(10)]
			public NetworkArray<bool> touchedLiquidArray
			{
				get
				{
					return new NetworkArray<bool>(Native.ReferenceToPointer<FixedStorage@10>(ref this._touchedLiquidArray), 10, ReaderWriter@System_Boolean.GetInstance());
				}
			}

			// Token: 0x170007D0 RID: 2000
			// (get) Token: 0x06004B7D RID: 19325 RVA: 0x0016F29C File Offset: 0x0016D49C
			[Networked]
			[Capacity(10)]
			public NetworkArray<float> touchedLiquidAtProgressArray
			{
				get
				{
					return new NetworkArray<float>(Native.ReferenceToPointer<FixedStorage@10>(ref this._touchedLiquidAtProgressArray), 10, ReaderWriter@System_Single.GetInstance());
				}
			}

			// Token: 0x170007D1 RID: 2001
			// (get) Token: 0x06004B7E RID: 19326 RVA: 0x0016F2C4 File Offset: 0x0016D4C4
			[Networked]
			[Capacity(5)]
			public NetworkLinkedList<float> initialAngleArray
			{
				get
				{
					return new NetworkLinkedList<float>(Native.ReferenceToPointer<FixedStorage@18>(ref this._initialAngleArray), 5, ReaderWriter@System_Single.GetInstance());
				}
			}

			// Token: 0x170007D2 RID: 2002
			// (get) Token: 0x06004B7F RID: 19327 RVA: 0x0016F2E8 File Offset: 0x0016D4E8
			[Networked]
			[Capacity(5)]
			public NetworkLinkedList<float> resultingAngleArray
			{
				get
				{
					return new NetworkLinkedList<float>(Native.ReferenceToPointer<FixedStorage@18>(ref this._resultingAngleArray), 5, ReaderWriter@System_Single.GetInstance());
				}
			}

			// Token: 0x06004B80 RID: 19328 RVA: 0x0016F30C File Offset: 0x0016D50C
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

			// Token: 0x04004D1A RID: 19738
			[FieldOffset(0)]
			public int reliableState;

			// Token: 0x04004D1B RID: 19739
			[FieldOffset(4)]
			public double stateStartTime;

			// Token: 0x04004D1C RID: 19740
			[FieldOffset(12)]
			public float stateStartLiquidProgressLinear;

			// Token: 0x04004D1D RID: 19741
			[FieldOffset(16)]
			public double activationProgress;

			// Token: 0x04004D1E RID: 19742
			[FieldOffset(24)]
			public int nextRoundRiseSpeed;

			// Token: 0x04004D1F RID: 19743
			[FieldOffset(28)]
			public float riseTime;

			// Token: 0x04004D20 RID: 19744
			[FieldOffset(32)]
			public int lastWinnerId;

			// Token: 0x04004D21 RID: 19745
			[FieldOffset(36)]
			public int inGamePlayerCount;

			// Token: 0x04004D22 RID: 19746
			[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(40)]
			private FixedStorage@10 _playerIdArray;

			// Token: 0x04004D23 RID: 19747
			[FixedBufferProperty(typeof(NetworkArray<bool>), typeof(UnityArraySurrogate@ReaderWriter@System_Boolean), 10, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(80)]
			private FixedStorage@10 _touchedLiquidArray;

			// Token: 0x04004D24 RID: 19748
			[FixedBufferProperty(typeof(NetworkArray<float>), typeof(UnityArraySurrogate@ReaderWriter@System_Single), 10, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(120)]
			private FixedStorage@10 _touchedLiquidAtProgressArray;

			// Token: 0x04004D25 RID: 19749
			[FixedBufferProperty(typeof(NetworkLinkedList<float>), typeof(UnityLinkedListSurrogate@ReaderWriter@System_Single), 5, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(160)]
			private FixedStorage@18 _initialAngleArray;

			// Token: 0x04004D26 RID: 19750
			[FixedBufferProperty(typeof(NetworkLinkedList<float>), typeof(UnityLinkedListSurrogate@ReaderWriter@System_Single), 5, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(232)]
			private FixedStorage@18 _resultingAngleArray;
		}
	}
}
