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
	// Token: 0x02000A28 RID: 2600
	public class BuilderPieceBallista : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x06004123 RID: 16675 RVA: 0x0016FE44 File Offset: 0x0016E044
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

		// Token: 0x06004124 RID: 16676 RVA: 0x0005A97C File Offset: 0x00058B7C
		private void OnDestroy()
		{
			if (this.handTrigger != null)
			{
				this.handTrigger.TriggeredEvent.RemoveListener(new UnityAction(this.OnHandTriggerPressed));
			}
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x0005A9A8 File Offset: 0x00058BA8
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

		// Token: 0x06004126 RID: 16678 RVA: 0x0016FF0C File Offset: 0x0016E10C
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

		// Token: 0x06004127 RID: 16679 RVA: 0x0005A9D2 File Offset: 0x00058BD2
		private void ResetFlags()
		{
			this.playerLaunched = false;
			this.loadCompleteTime = double.MaxValue;
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x00170290 File Offset: 0x0016E490
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

		// Token: 0x06004129 RID: 16681 RVA: 0x001704AC File Offset: 0x0016E6AC
		private Vector3 GetPlayerBodyCenterPosition(Transform headTransform)
		{
			return headTransform.position + Quaternion.Euler(0f, headTransform.rotation.eulerAngles.y, 0f) * new Vector3(0f, 0f, -0.009f) + Vector3.down * 0.024f;
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x00170514 File Offset: 0x0016E714
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

		// Token: 0x0600412B RID: 16683 RVA: 0x001705AC File Offset: 0x0016E7AC
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

		// Token: 0x0600412C RID: 16684 RVA: 0x0005A9EA File Offset: 0x00058BEA
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.ballistaState = BuilderPieceBallista.BallistaState.Idle;
			this.playerInTrigger = false;
			this.playerRigInTrigger = null;
			this.playerLaunched = false;
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x0005AA08 File Offset: 0x00058C08
		public void OnPieceDestroy()
		{
			this.myPiece.functionalPieceState = 0;
			this.ballistaState = BuilderPieceBallista.BallistaState.Idle;
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x00170644 File Offset: 0x0016E844
		public void OnPiecePlacementDeserialized()
		{
			this.launchDirection = this.launchEnd.position - this.launchStart.position;
			this.launchRampDistance = this.launchDirection.magnitude;
			this.launchDirection /= this.launchRampDistance;
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x0017069C File Offset: 0x0016E89C
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

		// Token: 0x06004130 RID: 16688 RVA: 0x00170764 File Offset: 0x0016E964
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

		// Token: 0x06004131 RID: 16689 RVA: 0x00170808 File Offset: 0x0016EA08
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

		// Token: 0x06004132 RID: 16690 RVA: 0x0017089C File Offset: 0x0016EA9C
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

		// Token: 0x06004133 RID: 16691 RVA: 0x0005AA1D File Offset: 0x00058C1D
		public bool IsStateValid(byte state)
		{
			return state < 8;
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x0005AA23 File Offset: 0x00058C23
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

		// Token: 0x06004135 RID: 16693 RVA: 0x00170B74 File Offset: 0x0016ED74
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

		// Token: 0x06004136 RID: 16694 RVA: 0x0005AA59 File Offset: 0x00058C59
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

		// Token: 0x040041DE RID: 16862
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x040041DF RID: 16863
		[SerializeField]
		private List<Collider> triggers;

		// Token: 0x040041E0 RID: 16864
		[SerializeField]
		private List<Collider> disableWhileLaunching;

		// Token: 0x040041E1 RID: 16865
		[SerializeField]
		private BuilderSmallHandTrigger handTrigger;

		// Token: 0x040041E2 RID: 16866
		[SerializeField]
		private bool autoLaunch;

		// Token: 0x040041E3 RID: 16867
		[SerializeField]
		private float autoLaunchDelay = 0.75f;

		// Token: 0x040041E4 RID: 16868
		private double enteredTriggerTime;

		// Token: 0x040041E5 RID: 16869
		public Animator animator;

		// Token: 0x040041E6 RID: 16870
		public Transform launchStart;

		// Token: 0x040041E7 RID: 16871
		public Transform launchEnd;

		// Token: 0x040041E8 RID: 16872
		public Transform launchBone;

		// Token: 0x040041E9 RID: 16873
		[SerializeField]
		private SoundBankPlayer loadSFX;

		// Token: 0x040041EA RID: 16874
		[SerializeField]
		private SoundBankPlayer launchSFX;

		// Token: 0x040041EB RID: 16875
		[SerializeField]
		private SoundBankPlayer cockSFX;

		// Token: 0x040041EC RID: 16876
		[SerializeField]
		private ParticleSystem launchParticles;

		// Token: 0x040041ED RID: 16877
		private bool hasLaunchParticles;

		// Token: 0x040041EE RID: 16878
		public float reloadDelay = 1f;

		// Token: 0x040041EF RID: 16879
		public float loadTime = 1.933f;

		// Token: 0x040041F0 RID: 16880
		public float slipOverrideDuration = 0.1f;

		// Token: 0x040041F1 RID: 16881
		private double launchedTime;

		// Token: 0x040041F2 RID: 16882
		public float playerMagnetismStrength = 3f;

		// Token: 0x040041F3 RID: 16883
		public float launchSpeed = 20f;

		// Token: 0x040041F4 RID: 16884
		[Range(0f, 1f)]
		public float pitch;

		// Token: 0x040041F5 RID: 16885
		private bool debugDrawTrajectoryOnLaunch;

		// Token: 0x040041F6 RID: 16886
		private int loadTriggerHash = Animator.StringToHash("Load");

		// Token: 0x040041F7 RID: 16887
		private int fireTriggerHash = Animator.StringToHash("Fire");

		// Token: 0x040041F8 RID: 16888
		private int pitchParamHash = Animator.StringToHash("Pitch");

		// Token: 0x040041F9 RID: 16889
		private int idleStateHash = Animator.StringToHash("Idle");

		// Token: 0x040041FA RID: 16890
		private int loadStateHash = Animator.StringToHash("Load");

		// Token: 0x040041FB RID: 16891
		private int fireStateHash = Animator.StringToHash("Fire");

		// Token: 0x040041FC RID: 16892
		private bool playerInTrigger;

		// Token: 0x040041FD RID: 16893
		private VRRig playerRigInTrigger;

		// Token: 0x040041FE RID: 16894
		private bool playerLaunched;

		// Token: 0x040041FF RID: 16895
		private float playerReadyToFireDist = 0.1f;

		// Token: 0x04004200 RID: 16896
		private Vector3 launchDirection;

		// Token: 0x04004201 RID: 16897
		private float launchRampDistance;

		// Token: 0x04004202 RID: 16898
		private float playerPullInRate;

		// Token: 0x04004203 RID: 16899
		private float appliedAnimatorPitch;

		// Token: 0x04004204 RID: 16900
		private double loadCompleteTime;

		// Token: 0x04004205 RID: 16901
		private BuilderPieceBallista.BallistaState ballistaState;

		// Token: 0x04004206 RID: 16902
		private const int predictionLineSamples = 240;

		// Token: 0x04004207 RID: 16903
		private Vector3[] predictionLinePoints = new Vector3[240];

		// Token: 0x02000A29 RID: 2601
		private enum BallistaState
		{
			// Token: 0x04004209 RID: 16905
			Idle,
			// Token: 0x0400420A RID: 16906
			Loading,
			// Token: 0x0400420B RID: 16907
			WaitingForTrigger,
			// Token: 0x0400420C RID: 16908
			PlayerInTrigger,
			// Token: 0x0400420D RID: 16909
			PrepareForLaunch,
			// Token: 0x0400420E RID: 16910
			PrepareForLaunchLocal,
			// Token: 0x0400420F RID: 16911
			Launching,
			// Token: 0x04004210 RID: 16912
			LaunchingLocal,
			// Token: 0x04004211 RID: 16913
			Count
		}
	}
}
