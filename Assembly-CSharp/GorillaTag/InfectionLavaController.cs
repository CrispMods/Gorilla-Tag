using System;
using GorillaExtensions;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using GorillaTag.GuidedRefs;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B9F RID: 2975
	public class InfectionLavaController : MonoBehaviour, IGorillaSerializeableScene, IGorillaSerializeable, ITickSystemPost, IGuidedRefReceiverMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06004B04 RID: 19204 RVA: 0x0016AEBA File Offset: 0x001690BA
		public static InfectionLavaController Instance
		{
			get
			{
				return InfectionLavaController.instance;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06004B05 RID: 19205 RVA: 0x0016AEC1 File Offset: 0x001690C1
		public bool LavaCurrentlyActivated
		{
			get
			{
				return this.reliableState.state > InfectionLavaController.RisingLavaState.Drained;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06004B06 RID: 19206 RVA: 0x0016AED1 File Offset: 0x001690D1
		public Plane LavaPlane
		{
			get
			{
				return new Plane(this.lavaSurfacePlaneTransform.up, this.lavaSurfacePlaneTransform.position);
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06004B07 RID: 19207 RVA: 0x0016AEEE File Offset: 0x001690EE
		public Vector3 SurfaceCenter
		{
			get
			{
				return this.lavaSurfacePlaneTransform.position;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06004B08 RID: 19208 RVA: 0x0016AEFC File Offset: 0x001690FC
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

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06004B09 RID: 19209 RVA: 0x0016AF2C File Offset: 0x0016912C
		private bool InCompetitiveQueue
		{
			get
			{
				return NetworkSystem.Instance.InRoom && NetworkSystem.Instance.GameModeString.Contains("COMPETITIVE");
			}
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x0016AF50 File Offset: 0x00169150
		private void Awake()
		{
			if (InfectionLavaController.instance.IsNotNull())
			{
				Object.Destroy(base.gameObject);
				return;
			}
			InfectionLavaController.instance = this;
			RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
			RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
			((IGuidedRefObject)this).GuidedRefInitialize();
			if (this.lavaVolume != null)
			{
				this.lavaVolume.ColliderEnteredWater += this.OnColliderEnteredLava;
			}
			if (this.lavaActivationProjectileHitNotifier != null)
			{
				this.lavaActivationProjectileHitNotifier.OnProjectileHit += this.OnActivationLavaProjectileHit;
			}
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x0016B00B File Offset: 0x0016920B
		protected void OnEnable()
		{
			if (!this.guidedRefsFullyResolved)
			{
				return;
			}
			this.VerifyReferences();
			TickSystem<object>.AddPostTickCallback(this);
		}

		// Token: 0x06004B0C RID: 19212 RVA: 0x0016B022 File Offset: 0x00169222
		void IGorillaSerializeableScene.OnSceneLinking(GorillaSerializerScene netObj)
		{
			this.networkObject = netObj;
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x000C141F File Offset: 0x000BF61F
		protected void OnDisable()
		{
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x0016B02C File Offset: 0x0016922C
		private void VerifyReferences()
		{
			this.IfNullThenLogAndDisableSelf(this.lavaMeshTransform, "lavaMeshTransform", -1);
			this.IfNullThenLogAndDisableSelf(this.lavaSurfacePlaneTransform, "lavaSurfacePlaneTransform", -1);
			this.IfNullThenLogAndDisableSelf(this.lavaVolume, "lavaVolume", -1);
			this.IfNullThenLogAndDisableSelf(this.lavaActivationRenderer, "lavaActivationRenderer", -1);
			this.IfNullThenLogAndDisableSelf(this.lavaActivationStartPos, "lavaActivationStartPos", -1);
			this.IfNullThenLogAndDisableSelf(this.lavaActivationEndPos, "lavaActivationEndPos", -1);
			this.IfNullThenLogAndDisableSelf(this.lavaActivationProjectileHitNotifier, "lavaActivationProjectileHitNotifier", -1);
			for (int i = 0; i < this.volcanoEffects.Length; i++)
			{
				this.IfNullThenLogAndDisableSelf(this.volcanoEffects[i], "volcanoEffects", i);
			}
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x0016B0E0 File Offset: 0x001692E0
		private void IfNullThenLogAndDisableSelf(Object obj, string fieldName, int index = -1)
		{
			if (obj != null)
			{
				return;
			}
			fieldName = ((index != -1) ? string.Format("{0}[{1}]", fieldName, index) : fieldName);
			Debug.LogError("InfectionLavaController: Disabling self because reference `" + fieldName + "` is null.", this);
			base.enabled = false;
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x0016B130 File Offset: 0x00169330
		private void OnDestroy()
		{
			if (InfectionLavaController.instance == this)
			{
				InfectionLavaController.instance = null;
			}
			TickSystem<object>.RemovePostTickCallback(this);
			this.UpdateLava(0f);
			if (this.lavaVolume != null)
			{
				this.lavaVolume.ColliderEnteredWater -= this.OnColliderEnteredLava;
			}
			if (this.lavaActivationProjectileHitNotifier != null)
			{
				this.lavaActivationProjectileHitNotifier.OnProjectileHit -= this.OnActivationLavaProjectileHit;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06004B11 RID: 19217 RVA: 0x0016B1AB File Offset: 0x001693AB
		// (set) Token: 0x06004B12 RID: 19218 RVA: 0x0016B1B3 File Offset: 0x001693B3
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004B13 RID: 19219 RVA: 0x0016B1BC File Offset: 0x001693BC
		void ITickSystemPost.PostTick()
		{
			this.prevTime = this.currentTime;
			this.currentTime = (NetworkSystem.Instance.InRoom ? NetworkSystem.Instance.SimTime : Time.timeAsDouble);
			if (this.networkObject.HasAuthority)
			{
				this.UpdateReliableState(this.currentTime, ref this.reliableState);
			}
			this.UpdateLocalState(this.currentTime, this.reliableState);
			this.localLagLavaProgressOffset = Mathf.MoveTowards(this.localLagLavaProgressOffset, 0f, this.lagResolutionLavaProgressPerSecond * Time.deltaTime);
			this.UpdateLava(this.lavaProgressSmooth + this.localLagLavaProgressOffset);
			this.UpdateVolcanoActivationLava((float)this.reliableState.activationProgress);
			this.CheckLocalPlayerAgainstLava(this.currentTime);
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x0016B27C File Offset: 0x0016947C
		private void JumpToState(InfectionLavaController.RisingLavaState state)
		{
			this.reliableState.state = state;
			switch (state)
			{
			case InfectionLavaController.RisingLavaState.Drained:
				for (int i = 0; i < this.volcanoEffects.Length; i++)
				{
					VolcanoEffects volcanoEffects = this.volcanoEffects[i];
					if (volcanoEffects != null)
					{
						volcanoEffects.SetDrainedState();
					}
				}
				return;
			case InfectionLavaController.RisingLavaState.Erupting:
				for (int j = 0; j < this.volcanoEffects.Length; j++)
				{
					VolcanoEffects volcanoEffects2 = this.volcanoEffects[j];
					if (volcanoEffects2 != null)
					{
						volcanoEffects2.SetEruptingState();
					}
				}
				return;
			case InfectionLavaController.RisingLavaState.Rising:
				for (int k = 0; k < this.volcanoEffects.Length; k++)
				{
					VolcanoEffects volcanoEffects3 = this.volcanoEffects[k];
					if (volcanoEffects3 != null)
					{
						volcanoEffects3.SetRisingState();
					}
				}
				return;
			case InfectionLavaController.RisingLavaState.Full:
				for (int l = 0; l < this.volcanoEffects.Length; l++)
				{
					VolcanoEffects volcanoEffects4 = this.volcanoEffects[l];
					if (volcanoEffects4 != null)
					{
						volcanoEffects4.SetFullState();
					}
				}
				return;
			case InfectionLavaController.RisingLavaState.Draining:
				for (int m = 0; m < this.volcanoEffects.Length; m++)
				{
					VolcanoEffects volcanoEffects5 = this.volcanoEffects[m];
					if (volcanoEffects5 != null)
					{
						volcanoEffects5.SetDrainingState();
					}
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06004B15 RID: 19221 RVA: 0x0016B378 File Offset: 0x00169578
		private void UpdateReliableState(double currentTime, ref InfectionLavaController.LavaSyncData syncData)
		{
			if (currentTime < syncData.stateStartTime)
			{
				syncData.stateStartTime = currentTime;
			}
			switch (syncData.state)
			{
			default:
				if (syncData.activationProgress > 1.0)
				{
					float playerCount = (float)this.PlayerCount;
					float num = this.InCompetitiveQueue ? this.activationVotePercentageCompetitiveQueue : this.activationVotePercentageDefaultQueue;
					int num2 = Mathf.RoundToInt(playerCount * num);
					if (this.lavaActivationVoteCount >= num2)
					{
						for (int i = 0; i < this.lavaActivationVoteCount; i++)
						{
							this.lavaActivationVotePlayerIds[i] = 0;
						}
						this.lavaActivationVoteCount = 0;
						syncData.state = InfectionLavaController.RisingLavaState.Erupting;
						syncData.stateStartTime = currentTime;
						syncData.activationProgress = 1.0;
						for (int j = 0; j < this.volcanoEffects.Length; j++)
						{
							VolcanoEffects volcanoEffects = this.volcanoEffects[j];
							if (volcanoEffects != null)
							{
								volcanoEffects.SetEruptingState();
							}
						}
						return;
					}
				}
				else
				{
					float num3 = Mathf.Clamp((float)(currentTime - this.prevTime), 0f, 0.1f);
					double activationProgress = syncData.activationProgress;
					syncData.activationProgress = (double)Mathf.MoveTowards((float)syncData.activationProgress, 0f, this.lavaActivationDrainRateVsPlayerCount.Evaluate((float)this.PlayerCount) * num3);
					if (activationProgress > 0.0 && syncData.activationProgress <= 5E-324)
					{
						VolcanoEffects[] array = this.volcanoEffects;
						for (int k = 0; k < array.Length; k++)
						{
							array[k].OnVolcanoBellyEmpty();
						}
						return;
					}
				}
				break;
			case InfectionLavaController.RisingLavaState.Erupting:
				if (currentTime > syncData.stateStartTime + (double)this.eruptTime)
				{
					syncData.state = InfectionLavaController.RisingLavaState.Rising;
					syncData.stateStartTime = currentTime;
					for (int l = 0; l < this.volcanoEffects.Length; l++)
					{
						VolcanoEffects volcanoEffects2 = this.volcanoEffects[l];
						if (volcanoEffects2 != null)
						{
							volcanoEffects2.SetRisingState();
						}
					}
					return;
				}
				break;
			case InfectionLavaController.RisingLavaState.Rising:
				if (currentTime > syncData.stateStartTime + (double)this.riseTime)
				{
					syncData.state = InfectionLavaController.RisingLavaState.Full;
					syncData.stateStartTime = currentTime;
					for (int m = 0; m < this.volcanoEffects.Length; m++)
					{
						VolcanoEffects volcanoEffects3 = this.volcanoEffects[m];
						if (volcanoEffects3 != null)
						{
							volcanoEffects3.SetFullState();
						}
					}
					return;
				}
				break;
			case InfectionLavaController.RisingLavaState.Full:
				if (currentTime > syncData.stateStartTime + (double)this.fullTime)
				{
					syncData.state = InfectionLavaController.RisingLavaState.Draining;
					syncData.stateStartTime = currentTime;
					for (int n = 0; n < this.volcanoEffects.Length; n++)
					{
						VolcanoEffects volcanoEffects4 = this.volcanoEffects[n];
						if (volcanoEffects4 != null)
						{
							volcanoEffects4.SetDrainingState();
						}
					}
					return;
				}
				break;
			case InfectionLavaController.RisingLavaState.Draining:
				syncData.activationProgress = (double)Mathf.MoveTowards((float)syncData.activationProgress, 0f, this.lavaActivationDrainRateVsPlayerCount.Evaluate((float)this.PlayerCount) * Time.deltaTime);
				if (currentTime > syncData.stateStartTime + (double)this.drainTime)
				{
					syncData.state = InfectionLavaController.RisingLavaState.Drained;
					syncData.stateStartTime = currentTime;
					for (int num4 = 0; num4 < this.volcanoEffects.Length; num4++)
					{
						VolcanoEffects volcanoEffects5 = this.volcanoEffects[num4];
						if (volcanoEffects5 != null)
						{
							volcanoEffects5.SetDrainedState();
						}
					}
				}
				break;
			}
		}

		// Token: 0x06004B16 RID: 19222 RVA: 0x0016B664 File Offset: 0x00169864
		private void UpdateLocalState(double currentTime, InfectionLavaController.LavaSyncData syncData)
		{
			switch (syncData.state)
			{
			default:
			{
				this.lavaProgressLinear = 0f;
				this.lavaProgressSmooth = 0f;
				float time = (float)(currentTime - syncData.stateStartTime);
				foreach (VolcanoEffects volcanoEffects in this.volcanoEffects)
				{
					if (volcanoEffects != null)
					{
						volcanoEffects.UpdateDrainedState(time);
					}
				}
				return;
			}
			case InfectionLavaController.RisingLavaState.Erupting:
			{
				this.lavaProgressLinear = 0f;
				this.lavaProgressSmooth = 0f;
				float num = (float)(currentTime - syncData.stateStartTime);
				float progress = Mathf.Clamp01(num / this.eruptTime);
				foreach (VolcanoEffects volcanoEffects2 in this.volcanoEffects)
				{
					if (volcanoEffects2 != null)
					{
						volcanoEffects2.UpdateEruptingState(num, this.eruptTime - num, progress);
					}
				}
				return;
			}
			case InfectionLavaController.RisingLavaState.Rising:
			{
				float value = (float)(currentTime - syncData.stateStartTime) / this.riseTime;
				this.lavaProgressLinear = Mathf.Clamp01(value);
				this.lavaProgressSmooth = this.lavaProgressAnimationCurve.Evaluate(this.lavaProgressLinear);
				float num2 = (float)(currentTime - syncData.stateStartTime);
				foreach (VolcanoEffects volcanoEffects3 in this.volcanoEffects)
				{
					if (volcanoEffects3 != null)
					{
						volcanoEffects3.UpdateRisingState(num2, this.riseTime - num2, this.lavaProgressLinear);
					}
				}
				return;
			}
			case InfectionLavaController.RisingLavaState.Full:
			{
				this.lavaProgressLinear = 1f;
				this.lavaProgressSmooth = 1f;
				float num3 = (float)(currentTime - syncData.stateStartTime);
				float progress2 = Mathf.Clamp01(this.fullTime / num3);
				foreach (VolcanoEffects volcanoEffects4 in this.volcanoEffects)
				{
					if (volcanoEffects4 != null)
					{
						volcanoEffects4.UpdateFullState(num3, this.fullTime - num3, progress2);
					}
				}
				return;
			}
			case InfectionLavaController.RisingLavaState.Draining:
			{
				float num4 = (float)(currentTime - syncData.stateStartTime);
				float num5 = Mathf.Clamp01(num4 / this.drainTime);
				this.lavaProgressLinear = 1f - num5;
				this.lavaProgressSmooth = this.lavaProgressAnimationCurve.Evaluate(this.lavaProgressLinear);
				foreach (VolcanoEffects volcanoEffects5 in this.volcanoEffects)
				{
					if (volcanoEffects5 != null)
					{
						volcanoEffects5.UpdateDrainingState(num4, this.riseTime - num4, num5);
					}
				}
				return;
			}
			}
		}

		// Token: 0x06004B17 RID: 19223 RVA: 0x0016B8AC File Offset: 0x00169AAC
		private void UpdateLava(float fillProgress)
		{
			this.lavaScale = Mathf.Lerp(this.lavaMeshMinScale, this.lavaMeshMaxScale, fillProgress);
			if (this.lavaMeshTransform != null)
			{
				this.lavaMeshTransform.localScale = new Vector3(this.lavaMeshTransform.localScale.x, this.lavaMeshTransform.localScale.y, this.lavaScale);
			}
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x0016B918 File Offset: 0x00169B18
		private void UpdateVolcanoActivationLava(float activationProgress)
		{
			this.activationProgessSmooth = Mathf.MoveTowards(this.activationProgessSmooth, activationProgress, this.lavaActivationVisualMovementProgressPerSecond * Time.deltaTime);
			this.lavaActivationRenderer.material.SetColor(InfectionLavaController.shaderProp_BaseColor, this.lavaActivationGradient.Evaluate(activationProgress));
			this.lavaActivationRenderer.transform.position = Vector3.Lerp(this.lavaActivationStartPos.position, this.lavaActivationEndPos.position, this.activationProgessSmooth);
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x0016B995 File Offset: 0x00169B95
		private void CheckLocalPlayerAgainstLava(double currentTime)
		{
			if (GTPlayer.Instance.InWater && GTPlayer.Instance.CurrentWaterVolume == this.lavaVolume)
			{
				this.LocalPlayerInLava(currentTime, false);
			}
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x0016B9C2 File Offset: 0x00169BC2
		private void OnColliderEnteredLava(WaterVolume volume, Collider collider)
		{
			if (collider == GTPlayer.Instance.bodyCollider)
			{
				this.LocalPlayerInLava(NetworkSystem.Instance.InRoom ? NetworkSystem.Instance.SimTime : Time.timeAsDouble, true);
			}
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x0016B9FC File Offset: 0x00169BFC
		private void LocalPlayerInLava(double currentTime, bool enteredLavaThisFrame)
		{
			GorillaGameManager gorillaGameManager = GorillaGameManager.instance;
			if (gorillaGameManager != null && gorillaGameManager.CanAffectPlayer(NetworkSystem.Instance.LocalPlayer, enteredLavaThisFrame) && (currentTime - this.lastTagSelfRPCTime > 0.5 || enteredLavaThisFrame))
			{
				this.lastTagSelfRPCTime = currentTime;
				GameMode.ReportHit();
			}
		}

		// Token: 0x06004B1C RID: 19228 RVA: 0x0016BA4E File Offset: 0x00169C4E
		public void OnActivationLavaProjectileHit(SlingshotProjectile projectile, Collision collision)
		{
			if (projectile.gameObject.CompareTag("LavaRockProjectile"))
			{
				this.AddLavaRock(projectile.projectileOwner.ActorNumber);
			}
		}

		// Token: 0x06004B1D RID: 19229 RVA: 0x0016BA74 File Offset: 0x00169C74
		private void AddLavaRock(int playerId)
		{
			if (this.networkObject.HasAuthority && this.reliableState.state == InfectionLavaController.RisingLavaState.Drained)
			{
				float num = this.lavaActivationRockProgressVsPlayerCount.Evaluate((float)this.PlayerCount);
				this.reliableState.activationProgress = this.reliableState.activationProgress + (double)num;
				this.AddVoteForVolcanoActivation(playerId);
				VolcanoEffects[] array = this.volcanoEffects;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].OnStoneAccepted(this.reliableState.activationProgress);
				}
			}
		}

		// Token: 0x06004B1E RID: 19230 RVA: 0x0016BAF0 File Offset: 0x00169CF0
		private void AddVoteForVolcanoActivation(int playerId)
		{
			if (this.networkObject.HasAuthority && this.lavaActivationVoteCount < 10)
			{
				bool flag = false;
				for (int i = 0; i < this.lavaActivationVoteCount; i++)
				{
					if (this.lavaActivationVotePlayerIds[i] == playerId)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					this.lavaActivationVotePlayerIds[this.lavaActivationVoteCount] = playerId;
					this.lavaActivationVoteCount++;
				}
			}
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x0016BB54 File Offset: 0x00169D54
		private void RemoveVoteForVolcanoActivation(int playerId)
		{
			if (this.networkObject.HasAuthority)
			{
				for (int i = 0; i < this.lavaActivationVoteCount; i++)
				{
					if (this.lavaActivationVotePlayerIds[i] == playerId)
					{
						this.lavaActivationVotePlayerIds[i] = this.lavaActivationVotePlayerIds[this.lavaActivationVoteCount - 1];
						this.lavaActivationVoteCount--;
						return;
					}
				}
			}
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x0016BBB0 File Offset: 0x00169DB0
		void IGorillaSerializeable.OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
		{
			stream.SendNext((int)this.reliableState.state);
			stream.SendNext(this.reliableState.stateStartTime);
			stream.SendNext(this.reliableState.activationProgress);
			stream.SendNext(this.lavaActivationVoteCount);
			stream.SendNext(this.lavaActivationVotePlayerIds[0]);
			stream.SendNext(this.lavaActivationVotePlayerIds[1]);
			stream.SendNext(this.lavaActivationVotePlayerIds[2]);
			stream.SendNext(this.lavaActivationVotePlayerIds[3]);
			stream.SendNext(this.lavaActivationVotePlayerIds[4]);
			stream.SendNext(this.lavaActivationVotePlayerIds[5]);
			stream.SendNext(this.lavaActivationVotePlayerIds[6]);
			stream.SendNext(this.lavaActivationVotePlayerIds[7]);
			stream.SendNext(this.lavaActivationVotePlayerIds[8]);
			stream.SendNext(this.lavaActivationVotePlayerIds[9]);
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x0016BCD0 File Offset: 0x00169ED0
		void IGorillaSerializeable.OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
		{
			InfectionLavaController.RisingLavaState risingLavaState = (InfectionLavaController.RisingLavaState)((int)stream.ReceiveNext());
			this.reliableState.stateStartTime = ((double)stream.ReceiveNext()).GetFinite();
			this.reliableState.activationProgress = ((double)stream.ReceiveNext()).ClampSafe(0.0, 2.0);
			this.lavaActivationVoteCount = (int)stream.ReceiveNext();
			this.lavaActivationVotePlayerIds[0] = (int)stream.ReceiveNext();
			this.lavaActivationVotePlayerIds[1] = (int)stream.ReceiveNext();
			this.lavaActivationVotePlayerIds[2] = (int)stream.ReceiveNext();
			this.lavaActivationVotePlayerIds[3] = (int)stream.ReceiveNext();
			this.lavaActivationVotePlayerIds[4] = (int)stream.ReceiveNext();
			this.lavaActivationVotePlayerIds[5] = (int)stream.ReceiveNext();
			this.lavaActivationVotePlayerIds[6] = (int)stream.ReceiveNext();
			this.lavaActivationVotePlayerIds[7] = (int)stream.ReceiveNext();
			this.lavaActivationVotePlayerIds[8] = (int)stream.ReceiveNext();
			this.lavaActivationVotePlayerIds[9] = (int)stream.ReceiveNext();
			float num = this.lavaProgressSmooth;
			if (risingLavaState != this.reliableState.state)
			{
				this.JumpToState(risingLavaState);
			}
			this.UpdateLocalState((double)((float)NetworkSystem.Instance.SimTime), this.reliableState);
			this.localLagLavaProgressOffset = num - this.lavaProgressSmooth;
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x0016BE43 File Offset: 0x0016A043
		public void OnPlayerLeftRoom(NetPlayer otherNetPlayer)
		{
			this.RemoveVoteForVolcanoActivation(otherNetPlayer.ActorNumber);
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x0016BE54 File Offset: 0x0016A054
		private void OnLeftRoom()
		{
			for (int i = 0; i < this.lavaActivationVotePlayerIds.Length; i++)
			{
				if (this.lavaActivationVotePlayerIds[i] != NetworkSystem.Instance.LocalPlayerID)
				{
					this.RemoveVoteForVolcanoActivation(this.lavaActivationVotePlayerIds[i]);
				}
			}
		}

		// Token: 0x06004B24 RID: 19236 RVA: 0x000023F4 File Offset: 0x000005F4
		void IGorillaSerializeableScene.OnNetworkObjectDisable()
		{
		}

		// Token: 0x06004B25 RID: 19237 RVA: 0x000023F4 File Offset: 0x000005F4
		void IGorillaSerializeableScene.OnNetworkObjectEnable()
		{
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06004B26 RID: 19238 RVA: 0x0016BE96 File Offset: 0x0016A096
		// (set) Token: 0x06004B27 RID: 19239 RVA: 0x0016BE9E File Offset: 0x0016A09E
		int IGuidedRefReceiverMono.GuidedRefsWaitingToResolveCount { get; set; }

		// Token: 0x06004B28 RID: 19240 RVA: 0x0016BEA7 File Offset: 0x0016A0A7
		void IGuidedRefReceiverMono.OnAllGuidedRefsResolved()
		{
			this.guidedRefsFullyResolved = true;
			this.VerifyReferences();
			TickSystem<object>.AddPostTickCallback(this);
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x0016BEBC File Offset: 0x0016A0BC
		public void OnGuidedRefTargetDestroyed(int fieldId)
		{
			this.guidedRefsFullyResolved = false;
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004B2A RID: 19242 RVA: 0x0016BECC File Offset: 0x0016A0CC
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterReceiverField<InfectionLavaController>(this, "lavaMeshTransform_gRef", ref this.lavaMeshTransform_gRef);
			GuidedRefHub.RegisterReceiverField<InfectionLavaController>(this, "lavaSurfacePlaneTransform_gRef", ref this.lavaSurfacePlaneTransform_gRef);
			GuidedRefHub.RegisterReceiverField<InfectionLavaController>(this, "lavaVolume_gRef", ref this.lavaVolume_gRef);
			GuidedRefHub.RegisterReceiverField<InfectionLavaController>(this, "lavaActivationRenderer_gRef", ref this.lavaActivationRenderer_gRef);
			GuidedRefHub.RegisterReceiverField<InfectionLavaController>(this, "lavaActivationStartPos_gRef", ref this.lavaActivationStartPos_gRef);
			GuidedRefHub.RegisterReceiverField<InfectionLavaController>(this, "lavaActivationEndPos_gRef", ref this.lavaActivationEndPos_gRef);
			GuidedRefHub.RegisterReceiverField<InfectionLavaController>(this, "lavaActivationProjectileHitNotifier_gRef", ref this.lavaActivationProjectileHitNotifier_gRef);
			GuidedRefHub.RegisterReceiverArray<InfectionLavaController, VolcanoEffects>(this, "volcanoEffects_gRefs", ref this.volcanoEffects, ref this.volcanoEffects_gRefs);
			GuidedRefHub.ReceiverFullyRegistered<InfectionLavaController>(this);
		}

		// Token: 0x06004B2B RID: 19243 RVA: 0x0016BF70 File Offset: 0x0016A170
		bool IGuidedRefReceiverMono.GuidedRefTryResolveReference(GuidedRefTryResolveInfo target)
		{
			return GuidedRefHub.TryResolveField<InfectionLavaController, Transform>(this, ref this.lavaMeshTransform, this.lavaMeshTransform_gRef, target) || GuidedRefHub.TryResolveField<InfectionLavaController, Transform>(this, ref this.lavaSurfacePlaneTransform, this.lavaSurfacePlaneTransform_gRef, target) || GuidedRefHub.TryResolveField<InfectionLavaController, WaterVolume>(this, ref this.lavaVolume, this.lavaVolume_gRef, target) || GuidedRefHub.TryResolveField<InfectionLavaController, MeshRenderer>(this, ref this.lavaActivationRenderer, this.lavaActivationRenderer_gRef, target) || GuidedRefHub.TryResolveField<InfectionLavaController, Transform>(this, ref this.lavaActivationStartPos, this.lavaActivationStartPos_gRef, target) || GuidedRefHub.TryResolveField<InfectionLavaController, Transform>(this, ref this.lavaActivationEndPos, this.lavaActivationEndPos_gRef, target) || GuidedRefHub.TryResolveField<InfectionLavaController, SlingshotProjectileHitNotifier>(this, ref this.lavaActivationProjectileHitNotifier, this.lavaActivationProjectileHitNotifier_gRef, target) || GuidedRefHub.TryResolveArrayItem<InfectionLavaController, VolcanoEffects>(this, this.volcanoEffects, this.volcanoEffects_gRefs, target);
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x00042E29 File Offset: 0x00041029
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x00015AA9 File Offset: 0x00013CA9
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004C75 RID: 19573
		[OnEnterPlay_SetNull]
		private static InfectionLavaController instance;

		// Token: 0x04004C76 RID: 19574
		[SerializeField]
		private float lavaMeshMinScale = 3.17f;

		// Token: 0x04004C77 RID: 19575
		[Tooltip("If you throw rocks into the volcano quickly enough, then it will raise to this height.")]
		[SerializeField]
		private float lavaMeshMaxScale = 8.941086f;

		// Token: 0x04004C78 RID: 19576
		[SerializeField]
		private float eruptTime = 3f;

		// Token: 0x04004C79 RID: 19577
		[SerializeField]
		private float riseTime = 10f;

		// Token: 0x04004C7A RID: 19578
		[SerializeField]
		private float fullTime = 240f;

		// Token: 0x04004C7B RID: 19579
		[SerializeField]
		private float drainTime = 10f;

		// Token: 0x04004C7C RID: 19580
		[SerializeField]
		private float lagResolutionLavaProgressPerSecond = 0.2f;

		// Token: 0x04004C7D RID: 19581
		[SerializeField]
		private AnimationCurve lavaProgressAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004C7E RID: 19582
		[Header("Volcano Activation")]
		[SerializeField]
		[Range(0f, 1f)]
		private float activationVotePercentageDefaultQueue = 0.42f;

		// Token: 0x04004C7F RID: 19583
		[SerializeField]
		[Range(0f, 1f)]
		private float activationVotePercentageCompetitiveQueue = 0.6f;

		// Token: 0x04004C80 RID: 19584
		[SerializeField]
		private Gradient lavaActivationGradient;

		// Token: 0x04004C81 RID: 19585
		[SerializeField]
		private AnimationCurve lavaActivationRockProgressVsPlayerCount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004C82 RID: 19586
		[SerializeField]
		private AnimationCurve lavaActivationDrainRateVsPlayerCount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004C83 RID: 19587
		[SerializeField]
		private float lavaActivationVisualMovementProgressPerSecond = 1f;

		// Token: 0x04004C84 RID: 19588
		[SerializeField]
		private bool debugLavaActivationVotes;

		// Token: 0x04004C85 RID: 19589
		[Header("Scene References")]
		[SerializeField]
		private Transform lavaMeshTransform;

		// Token: 0x04004C86 RID: 19590
		[SerializeField]
		private GuidedRefReceiverFieldInfo lavaMeshTransform_gRef = new GuidedRefReceiverFieldInfo(true);

		// Token: 0x04004C87 RID: 19591
		[SerializeField]
		private Transform lavaSurfacePlaneTransform;

		// Token: 0x04004C88 RID: 19592
		[SerializeField]
		private GuidedRefReceiverFieldInfo lavaSurfacePlaneTransform_gRef = new GuidedRefReceiverFieldInfo(true);

		// Token: 0x04004C89 RID: 19593
		[SerializeField]
		private WaterVolume lavaVolume;

		// Token: 0x04004C8A RID: 19594
		[SerializeField]
		private GuidedRefReceiverFieldInfo lavaVolume_gRef = new GuidedRefReceiverFieldInfo(true);

		// Token: 0x04004C8B RID: 19595
		[SerializeField]
		private MeshRenderer lavaActivationRenderer;

		// Token: 0x04004C8C RID: 19596
		[SerializeField]
		private GuidedRefReceiverFieldInfo lavaActivationRenderer_gRef = new GuidedRefReceiverFieldInfo(true);

		// Token: 0x04004C8D RID: 19597
		[SerializeField]
		private Transform lavaActivationStartPos;

		// Token: 0x04004C8E RID: 19598
		[SerializeField]
		private GuidedRefReceiverFieldInfo lavaActivationStartPos_gRef = new GuidedRefReceiverFieldInfo(true);

		// Token: 0x04004C8F RID: 19599
		[SerializeField]
		private Transform lavaActivationEndPos;

		// Token: 0x04004C90 RID: 19600
		[SerializeField]
		private GuidedRefReceiverFieldInfo lavaActivationEndPos_gRef = new GuidedRefReceiverFieldInfo(true);

		// Token: 0x04004C91 RID: 19601
		[SerializeField]
		private SlingshotProjectileHitNotifier lavaActivationProjectileHitNotifier;

		// Token: 0x04004C92 RID: 19602
		[SerializeField]
		private GuidedRefReceiverFieldInfo lavaActivationProjectileHitNotifier_gRef = new GuidedRefReceiverFieldInfo(true);

		// Token: 0x04004C93 RID: 19603
		[SerializeField]
		private VolcanoEffects[] volcanoEffects;

		// Token: 0x04004C94 RID: 19604
		[SerializeField]
		private GuidedRefReceiverArrayInfo volcanoEffects_gRefs = new GuidedRefReceiverArrayInfo(true);

		// Token: 0x04004C95 RID: 19605
		[DebugReadout]
		private InfectionLavaController.LavaSyncData reliableState;

		// Token: 0x04004C96 RID: 19606
		private int[] lavaActivationVotePlayerIds = new int[10];

		// Token: 0x04004C97 RID: 19607
		private int lavaActivationVoteCount;

		// Token: 0x04004C98 RID: 19608
		private float localLagLavaProgressOffset;

		// Token: 0x04004C99 RID: 19609
		[DebugReadout]
		private float lavaProgressLinear;

		// Token: 0x04004C9A RID: 19610
		[DebugReadout]
		private float lavaProgressSmooth;

		// Token: 0x04004C9B RID: 19611
		private double lastTagSelfRPCTime;

		// Token: 0x04004C9C RID: 19612
		private const string lavaRockProjectileTag = "LavaRockProjectile";

		// Token: 0x04004C9D RID: 19613
		private double currentTime;

		// Token: 0x04004C9E RID: 19614
		private double prevTime;

		// Token: 0x04004C9F RID: 19615
		private float activationProgessSmooth;

		// Token: 0x04004CA0 RID: 19616
		private float lavaScale;

		// Token: 0x04004CA1 RID: 19617
		private static readonly int shaderProp_BaseColor = Shader.PropertyToID("_BaseColor");

		// Token: 0x04004CA2 RID: 19618
		private GorillaSerializerScene networkObject;

		// Token: 0x04004CA4 RID: 19620
		private bool guidedRefsFullyResolved;

		// Token: 0x02000BA0 RID: 2976
		public enum RisingLavaState
		{
			// Token: 0x04004CA7 RID: 19623
			Drained,
			// Token: 0x04004CA8 RID: 19624
			Erupting,
			// Token: 0x04004CA9 RID: 19625
			Rising,
			// Token: 0x04004CAA RID: 19626
			Full,
			// Token: 0x04004CAB RID: 19627
			Draining
		}

		// Token: 0x02000BA1 RID: 2977
		private struct LavaSyncData
		{
			// Token: 0x04004CAC RID: 19628
			public InfectionLavaController.RisingLavaState state;

			// Token: 0x04004CAD RID: 19629
			public double stateStartTime;

			// Token: 0x04004CAE RID: 19630
			public double activationProgress;
		}
	}
}
