using System;
using System.Collections;
using System.Collections.Generic;
using CjLib;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009FE RID: 2558
	public class BuilderPieceBallista : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x06003FEA RID: 16362 RVA: 0x0012F408 File Offset: 0x0012D608
		private void Awake()
		{
			this.animator.SetFloat(this.pitchParamHash, this.pitch);
			this.appliedAnimatorPitch = this.pitch;
			this.launchDirection = this.launchEnd.position - this.launchStart.position;
			this.launchRampDistance = this.launchDirection.magnitude;
			this.launchDirection /= this.launchRampDistance;
			this.playerPullInRate = Mathf.Exp(this.playerMagnetismStrength);
			if (this.handTrigger != null)
			{
				this.handTrigger.TriggeredEvent.AddListener(new UnityAction(this.OnHandTriggerPressed));
			}
			this.hasLaunchParticles = (this.launchParticles != null);
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x0012F4CE File Offset: 0x0012D6CE
		private void OnDestroy()
		{
			if (this.handTrigger != null)
			{
				this.handTrigger.TriggeredEvent.RemoveListener(new UnityAction(this.OnHandTriggerPressed));
			}
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x0012F4FA File Offset: 0x0012D6FA
		private void OnHandTriggerPressed()
		{
			if (this.autoLaunch)
			{
				return;
			}
			if (this.ballistaState == BuilderPieceBallista.BallistaState.PlayerInTrigger)
			{
				BuilderTableNetworking.instance.RequestFunctionalPieceStateChange(this.myPiece.pieceId, 4);
			}
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x0012F524 File Offset: 0x0012D724
		private void UpdateStateMaster()
		{
			if (!NetworkSystem.Instance.InRoom || !NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
			switch (this.ballistaState)
			{
			case BuilderPieceBallista.BallistaState.Idle:
				BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 1, PhotonNetwork.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				return;
			case BuilderPieceBallista.BallistaState.Loading:
				if (currentAnimatorStateInfo.shortNameHash == this.loadStateHash && (double)Time.time > this.loadCompleteTime)
				{
					if (this.playerInTrigger && this.playerRigInTrigger != null && (double)this.playerRigInTrigger.scaleFactor < 0.99)
					{
						BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 3, this.playerRigInTrigger.Creator.GetPlayerRef(), NetworkSystem.Instance.ServerTimestamp);
						return;
					}
					this.playerInTrigger = false;
					this.playerRigInTrigger = null;
					this.ballistaState = BuilderPieceBallista.BallistaState.WaitingForTrigger;
					return;
				}
				break;
			case BuilderPieceBallista.BallistaState.WaitingForTrigger:
				if (this.playerInTrigger && (this.playerRigInTrigger == null || this.playerRigInTrigger.scaleFactor >= 0.99f))
				{
					this.playerInTrigger = false;
					this.playerRigInTrigger = null;
					return;
				}
				if (this.playerInTrigger)
				{
					BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 3, this.playerRigInTrigger.Creator.GetPlayerRef(), NetworkSystem.Instance.ServerTimestamp);
					return;
				}
				break;
			case BuilderPieceBallista.BallistaState.PlayerInTrigger:
				if (!this.playerInTrigger || this.playerRigInTrigger == null || this.playerRigInTrigger.scaleFactor >= 0.99f)
				{
					this.playerInTrigger = false;
					this.playerRigInTrigger = null;
					BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 2, PhotonNetwork.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
					return;
				}
				if (this.autoLaunch && (double)Time.time > this.enteredTriggerTime + (double)this.autoLaunchDelay)
				{
					BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 4, this.playerRigInTrigger.Creator.GetPlayerRef(), NetworkSystem.Instance.ServerTimestamp);
					return;
				}
				break;
			case BuilderPieceBallista.BallistaState.PrepareForLaunch:
			case BuilderPieceBallista.BallistaState.PrepareForLaunchLocal:
			{
				if (!this.playerInTrigger || this.playerRigInTrigger == null || (double)this.playerRigInTrigger.scaleFactor >= 0.99)
				{
					this.playerInTrigger = false;
					this.playerRigInTrigger = null;
					this.ResetFlags();
					this.myPiece.functionalPieceState = 0;
					this.ballistaState = BuilderPieceBallista.BallistaState.Idle;
					return;
				}
				Vector3 playerBodyCenterPosition = this.GetPlayerBodyCenterPosition(this.playerRigInTrigger.transform);
				Vector3 b = Vector3.Dot(playerBodyCenterPosition - this.launchStart.position, this.launchDirection) * this.launchDirection + this.launchStart.position;
				Vector3 b2 = playerBodyCenterPosition - b;
				if (Vector3.Lerp(Vector3.zero, b2, Mathf.Exp(-this.playerPullInRate * Time.deltaTime)).sqrMagnitude < this.playerReadyToFireDist * this.playerReadyToFireDist)
				{
					BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 6, this.playerRigInTrigger.Creator.GetPlayerRef(), NetworkSystem.Instance.ServerTimestamp);
					return;
				}
				break;
			}
			case BuilderPieceBallista.BallistaState.Launching:
			case BuilderPieceBallista.BallistaState.LaunchingLocal:
				if (currentAnimatorStateInfo.shortNameHash == this.idleStateHash)
				{
					BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 1, PhotonNetwork.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0012F8A8 File Offset: 0x0012DAA8
		private void ResetFlags()
		{
			this.playerLaunched = false;
			this.loadCompleteTime = double.MaxValue;
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x0012F8C0 File Offset: 0x0012DAC0
		private void UpdatePlayerPosition()
		{
			if (this.ballistaState != BuilderPieceBallista.BallistaState.PrepareForLaunchLocal && this.ballistaState != BuilderPieceBallista.BallistaState.LaunchingLocal)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			GTPlayer instance = GTPlayer.Instance;
			Vector3 playerBodyCenterPosition = this.GetPlayerBodyCenterPosition(instance.headCollider.transform);
			Vector3 lhs = playerBodyCenterPosition - this.launchStart.position;
			BuilderPieceBallista.BallistaState ballistaState = this.ballistaState;
			if (ballistaState == BuilderPieceBallista.BallistaState.PrepareForLaunchLocal)
			{
				Vector3 b = Vector3.Dot(lhs, this.launchDirection) * this.launchDirection + this.launchStart.position;
				Vector3 b2 = playerBodyCenterPosition - b;
				Vector3 a = Vector3.Lerp(Vector3.zero, b2, Mathf.Exp(-this.playerPullInRate * deltaTime));
				instance.transform.position = instance.transform.position + (a - b2);
				instance.SetPlayerVelocity(Vector3.zero);
				instance.SetMaximumSlipThisFrame();
				return;
			}
			if (ballistaState != BuilderPieceBallista.BallistaState.LaunchingLocal)
			{
				return;
			}
			if (!this.playerLaunched)
			{
				float num = Vector3.Dot(this.launchBone.position - this.launchStart.position, this.launchDirection) / this.launchRampDistance;
				float b3 = Vector3.Dot(lhs, this.launchDirection) / this.launchRampDistance;
				float num2 = 0.015f / this.launchRampDistance;
				float num3 = Mathf.Max(num + num2, b3);
				float d = num3 * this.launchRampDistance;
				Vector3 a2 = this.launchDirection * d + this.launchStart.position;
				instance.transform.position + (a2 - playerBodyCenterPosition);
				instance.transform.position = instance.transform.position + (a2 - playerBodyCenterPosition);
				instance.SetPlayerVelocity(Vector3.zero);
				instance.SetMaximumSlipThisFrame();
				if (num3 >= 1f)
				{
					this.playerLaunched = true;
					this.launchedTime = (double)Time.time;
					instance.SetPlayerVelocity(this.launchSpeed * this.launchDirection);
					instance.SetMaximumSlipThisFrame();
					return;
				}
			}
			else if ((double)Time.time < this.launchedTime + (double)this.slipOverrideDuration)
			{
				instance.SetMaximumSlipThisFrame();
			}
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x0012FADC File Offset: 0x0012DCDC
		private Vector3 GetPlayerBodyCenterPosition(Transform headTransform)
		{
			return headTransform.position + Quaternion.Euler(0f, headTransform.rotation.eulerAngles.y, 0f) * new Vector3(0f, 0f, -0.009f) + Vector3.down * 0.024f;
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x0012FB44 File Offset: 0x0012DD44
		private void OnTriggerEnter(Collider other)
		{
			if (this.playerRigInTrigger != null)
			{
				return;
			}
			if (other.GetComponent<CapsuleCollider>() == null)
			{
				return;
			}
			if (other.attachedRigidbody == null)
			{
				return;
			}
			VRRig vrrig = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
			if (vrrig == null)
			{
				if (!(GTPlayer.Instance.bodyCollider == other))
				{
					return;
				}
				vrrig = GorillaTagger.Instance.offlineVRRig;
			}
			if ((double)vrrig.scaleFactor > 0.99)
			{
				return;
			}
			this.playerRigInTrigger = vrrig;
			this.playerInTrigger = true;
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x0012FBDC File Offset: 0x0012DDDC
		private void OnTriggerExit(Collider other)
		{
			if (this.playerRigInTrigger == null || !this.playerInTrigger)
			{
				return;
			}
			if (other.GetComponent<CapsuleCollider>() == null)
			{
				return;
			}
			if (other.attachedRigidbody == null)
			{
				return;
			}
			VRRig vrrig = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
			if (vrrig == null)
			{
				if (!(GTPlayer.Instance.bodyCollider == other))
				{
					return;
				}
				vrrig = GorillaTagger.Instance.offlineVRRig;
			}
			if (this.playerRigInTrigger.Equals(vrrig))
			{
				this.playerInTrigger = false;
				this.playerRigInTrigger = null;
			}
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x0012FC74 File Offset: 0x0012DE74
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.ballistaState = BuilderPieceBallista.BallistaState.Idle;
			this.playerInTrigger = false;
			this.playerRigInTrigger = null;
			this.playerLaunched = false;
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x0012FC92 File Offset: 0x0012DE92
		public void OnPieceDestroy()
		{
			this.myPiece.functionalPieceState = 0;
			this.ballistaState = BuilderPieceBallista.BallistaState.Idle;
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x0012FCA8 File Offset: 0x0012DEA8
		public void OnPiecePlacementDeserialized()
		{
			this.launchDirection = this.launchEnd.position - this.launchStart.position;
			this.launchRampDistance = this.launchDirection.magnitude;
			this.launchDirection /= this.launchRampDistance;
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x0012FD00 File Offset: 0x0012DF00
		public void OnPieceActivate()
		{
			foreach (Collider collider in this.triggers)
			{
				collider.enabled = true;
			}
			this.animator.SetFloat(this.pitchParamHash, this.pitch);
			this.appliedAnimatorPitch = this.pitch;
			this.launchDirection = this.launchEnd.position - this.launchStart.position;
			this.launchRampDistance = this.launchDirection.magnitude;
			this.launchDirection /= this.launchRampDistance;
			BuilderTable.instance.RegisterFunctionalPiece(this);
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x0012FDC8 File Offset: 0x0012DFC8
		public void OnPieceDeactivate()
		{
			foreach (Collider collider in this.triggers)
			{
				collider.enabled = false;
			}
			if (this.hasLaunchParticles)
			{
				this.launchParticles.Stop();
				this.launchParticles.Clear();
			}
			this.myPiece.functionalPieceState = 0;
			this.ballistaState = BuilderPieceBallista.BallistaState.Idle;
			this.playerInTrigger = false;
			this.playerRigInTrigger = null;
			this.ResetFlags();
			BuilderTable.instance.UnregisterFunctionalPiece(this);
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x0012FE6C File Offset: 0x0012E06C
		public void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp)
		{
			if (!NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			if (!this.IsStateValid(newState) || instigator == null)
			{
				return;
			}
			if ((BuilderPieceBallista.BallistaState)newState == this.ballistaState)
			{
				return;
			}
			if (newState == 4)
			{
				if (this.ballistaState == BuilderPieceBallista.BallistaState.PlayerInTrigger && this.playerInTrigger && this.playerRigInTrigger != null)
				{
					BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 4, this.playerRigInTrigger.Creator.GetPlayerRef(), timeStamp);
					return;
				}
			}
			else
			{
				Debug.LogWarning("BuilderPiece Ballista unexpected state request for " + newState.ToString());
			}
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x0012FF00 File Offset: 0x0012E100
		public void OnStateChanged(byte newState, NetPlayer instigator, int timeStamp)
		{
			if (!this.IsStateValid(newState))
			{
				return;
			}
			BuilderPieceBallista.BallistaState ballistaState = (BuilderPieceBallista.BallistaState)newState;
			if (ballistaState == this.ballistaState)
			{
				return;
			}
			switch (newState)
			{
			case 0:
				this.ResetFlags();
				goto IL_2A7;
			case 1:
				this.ResetFlags();
				foreach (Collider collider in this.disableWhileLaunching)
				{
					collider.enabled = true;
				}
				if (this.ballistaState == BuilderPieceBallista.BallistaState.Launching || this.ballistaState == BuilderPieceBallista.BallistaState.LaunchingLocal)
				{
					this.loadCompleteTime = (double)(Time.time + this.reloadDelay);
					if (this.loadSFX != null)
					{
						this.loadSFX.Play();
					}
				}
				else
				{
					this.loadCompleteTime = (double)(Time.time + this.loadTime);
				}
				this.animator.SetTrigger(this.loadTriggerHash);
				goto IL_2A7;
			case 2:
			case 5:
				goto IL_2A7;
			case 3:
				this.enteredTriggerTime = (double)Time.time;
				if (this.autoLaunch && this.cockSFX != null)
				{
					this.cockSFX.Play();
					goto IL_2A7;
				}
				goto IL_2A7;
			case 4:
			{
				this.playerLaunched = false;
				if (!this.autoLaunch && this.cockSFX != null)
				{
					this.cockSFX.Play();
				}
				if (this.hasLaunchParticles)
				{
					this.launchParticles.Play();
				}
				if (!instigator.IsLocal)
				{
					goto IL_2A7;
				}
				GTPlayer instance = GTPlayer.Instance;
				if (Vector3.Distance(this.GetPlayerBodyCenterPosition(instance.headCollider.transform), this.launchStart.position) > 0.15f || (double)GorillaTagger.Instance.offlineVRRig.scaleFactor >= 0.99)
				{
					goto IL_2A7;
				}
				ballistaState = BuilderPieceBallista.BallistaState.PrepareForLaunchLocal;
				using (List<Collider>.Enumerator enumerator = this.disableWhileLaunching.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Collider collider2 = enumerator.Current;
						collider2.enabled = false;
					}
					goto IL_2A7;
				}
				break;
			}
			case 6:
				break;
			default:
				goto IL_2A7;
			}
			this.playerLaunched = false;
			this.animator.SetTrigger(this.fireTriggerHash);
			if (this.launchSFX != null)
			{
				this.launchSFX.Play();
			}
			if (this.debugDrawTrajectoryOnLaunch)
			{
				base.StartCoroutine(this.DebugDrawTrajectory(8f));
			}
			if (instigator.IsLocal && this.ballistaState == BuilderPieceBallista.BallistaState.PrepareForLaunchLocal)
			{
				ballistaState = BuilderPieceBallista.BallistaState.LaunchingLocal;
				GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength * 2f, GorillaTagger.Instance.tapHapticDuration * 4f);
				GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength * 2f, GorillaTagger.Instance.tapHapticDuration * 4f);
			}
			IL_2A7:
			this.ballistaState = ballistaState;
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x001301D8 File Offset: 0x0012E3D8
		public bool IsStateValid(byte state)
		{
			return state < 8;
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x001301DE File Offset: 0x0012E3DE
		public void FunctionalPieceUpdate()
		{
			if (this.myPiece == null || this.myPiece.state != BuilderPiece.State.AttachedAndPlaced)
			{
				return;
			}
			if (NetworkSystem.Instance.IsMasterClient)
			{
				this.UpdateStateMaster();
			}
			this.UpdatePlayerPosition();
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x00130214 File Offset: 0x0012E414
		private void UpdatePredictionLine()
		{
			float d = 0.033333335f;
			Vector3 vector = this.launchEnd.position;
			Vector3 a = (this.launchEnd.position - this.launchStart.position).normalized * this.launchSpeed;
			for (int i = 0; i < 240; i++)
			{
				this.predictionLinePoints[i] = vector;
				vector += a * d;
				a += Vector3.down * 9.8f * d;
			}
		}

		// Token: 0x06003FFD RID: 16381 RVA: 0x001302AE File Offset: 0x0012E4AE
		private IEnumerator DebugDrawTrajectory(float duration)
		{
			this.UpdatePredictionLine();
			float startTime = Time.time;
			while (Time.time < startTime + duration)
			{
				DebugUtil.DrawLine(this.launchStart.position, this.launchEnd.position, Color.yellow, true);
				DebugUtil.DrawLines(this.predictionLinePoints, Color.yellow, true);
				yield return null;
			}
			yield break;
		}

		// Token: 0x040040F6 RID: 16630
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x040040F7 RID: 16631
		[SerializeField]
		private List<Collider> triggers;

		// Token: 0x040040F8 RID: 16632
		[SerializeField]
		private List<Collider> disableWhileLaunching;

		// Token: 0x040040F9 RID: 16633
		[SerializeField]
		private BuilderSmallHandTrigger handTrigger;

		// Token: 0x040040FA RID: 16634
		[SerializeField]
		private bool autoLaunch;

		// Token: 0x040040FB RID: 16635
		[SerializeField]
		private float autoLaunchDelay = 0.75f;

		// Token: 0x040040FC RID: 16636
		private double enteredTriggerTime;

		// Token: 0x040040FD RID: 16637
		public Animator animator;

		// Token: 0x040040FE RID: 16638
		public Transform launchStart;

		// Token: 0x040040FF RID: 16639
		public Transform launchEnd;

		// Token: 0x04004100 RID: 16640
		public Transform launchBone;

		// Token: 0x04004101 RID: 16641
		[SerializeField]
		private SoundBankPlayer loadSFX;

		// Token: 0x04004102 RID: 16642
		[SerializeField]
		private SoundBankPlayer launchSFX;

		// Token: 0x04004103 RID: 16643
		[SerializeField]
		private SoundBankPlayer cockSFX;

		// Token: 0x04004104 RID: 16644
		[SerializeField]
		private ParticleSystem launchParticles;

		// Token: 0x04004105 RID: 16645
		private bool hasLaunchParticles;

		// Token: 0x04004106 RID: 16646
		public float reloadDelay = 1f;

		// Token: 0x04004107 RID: 16647
		public float loadTime = 1.933f;

		// Token: 0x04004108 RID: 16648
		public float slipOverrideDuration = 0.1f;

		// Token: 0x04004109 RID: 16649
		private double launchedTime;

		// Token: 0x0400410A RID: 16650
		public float playerMagnetismStrength = 3f;

		// Token: 0x0400410B RID: 16651
		public float launchSpeed = 20f;

		// Token: 0x0400410C RID: 16652
		[Range(0f, 1f)]
		public float pitch;

		// Token: 0x0400410D RID: 16653
		private bool debugDrawTrajectoryOnLaunch;

		// Token: 0x0400410E RID: 16654
		private int loadTriggerHash = Animator.StringToHash("Load");

		// Token: 0x0400410F RID: 16655
		private int fireTriggerHash = Animator.StringToHash("Fire");

		// Token: 0x04004110 RID: 16656
		private int pitchParamHash = Animator.StringToHash("Pitch");

		// Token: 0x04004111 RID: 16657
		private int idleStateHash = Animator.StringToHash("Idle");

		// Token: 0x04004112 RID: 16658
		private int loadStateHash = Animator.StringToHash("Load");

		// Token: 0x04004113 RID: 16659
		private int fireStateHash = Animator.StringToHash("Fire");

		// Token: 0x04004114 RID: 16660
		private bool playerInTrigger;

		// Token: 0x04004115 RID: 16661
		private VRRig playerRigInTrigger;

		// Token: 0x04004116 RID: 16662
		private bool playerLaunched;

		// Token: 0x04004117 RID: 16663
		private float playerReadyToFireDist = 0.1f;

		// Token: 0x04004118 RID: 16664
		private Vector3 launchDirection;

		// Token: 0x04004119 RID: 16665
		private float launchRampDistance;

		// Token: 0x0400411A RID: 16666
		private float playerPullInRate;

		// Token: 0x0400411B RID: 16667
		private float appliedAnimatorPitch;

		// Token: 0x0400411C RID: 16668
		private double loadCompleteTime;

		// Token: 0x0400411D RID: 16669
		private BuilderPieceBallista.BallistaState ballistaState;

		// Token: 0x0400411E RID: 16670
		private const int predictionLineSamples = 240;

		// Token: 0x0400411F RID: 16671
		private Vector3[] predictionLinePoints = new Vector3[240];

		// Token: 0x020009FF RID: 2559
		private enum BallistaState
		{
			// Token: 0x04004121 RID: 16673
			Idle,
			// Token: 0x04004122 RID: 16674
			Loading,
			// Token: 0x04004123 RID: 16675
			WaitingForTrigger,
			// Token: 0x04004124 RID: 16676
			PlayerInTrigger,
			// Token: 0x04004125 RID: 16677
			PrepareForLaunch,
			// Token: 0x04004126 RID: 16678
			PrepareForLaunchLocal,
			// Token: 0x04004127 RID: 16679
			Launching,
			// Token: 0x04004128 RID: 16680
			LaunchingLocal,
			// Token: 0x04004129 RID: 16681
			Count
		}
	}
}
