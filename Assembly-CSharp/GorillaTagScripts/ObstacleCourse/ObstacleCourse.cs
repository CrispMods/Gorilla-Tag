using System;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x02000A14 RID: 2580
	public class ObstacleCourse : MonoBehaviour
	{
		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06004096 RID: 16534 RVA: 0x0005A370 File Offset: 0x00058570
		// (set) Token: 0x06004097 RID: 16535 RVA: 0x0005A378 File Offset: 0x00058578
		public int winnerActorNumber { get; private set; }

		// Token: 0x06004098 RID: 16536 RVA: 0x0016E1BC File Offset: 0x0016C3BC
		private void Awake()
		{
			this.numPlayersOnCourse = 0;
			for (int i = 0; i < this.zoneTriggers.Length; i++)
			{
				ObstacleCourseZoneTrigger obstacleCourseZoneTrigger = this.zoneTriggers[i];
				if (!(obstacleCourseZoneTrigger == null))
				{
					obstacleCourseZoneTrigger.OnPlayerTriggerEnter += this.OnPlayerEnterZone;
					obstacleCourseZoneTrigger.OnPlayerTriggerExit += this.OnPlayerExitZone;
				}
			}
			this.TappableBell.OnTapped += this.OnEndLineTrigger;
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x0016E230 File Offset: 0x0016C430
		private void OnDestroy()
		{
			for (int i = 0; i < this.zoneTriggers.Length; i++)
			{
				ObstacleCourseZoneTrigger obstacleCourseZoneTrigger = this.zoneTriggers[i];
				if (!(obstacleCourseZoneTrigger == null))
				{
					obstacleCourseZoneTrigger.OnPlayerTriggerEnter -= this.OnPlayerEnterZone;
					obstacleCourseZoneTrigger.OnPlayerTriggerExit -= this.OnPlayerExitZone;
				}
			}
			this.TappableBell.OnTapped -= this.OnEndLineTrigger;
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x0005A381 File Offset: 0x00058581
		private void Start()
		{
			this.RestartTimer(false);
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x0016E2A0 File Offset: 0x0016C4A0
		public void InvokeUpdate()
		{
			foreach (ZoneBasedObject zoneBasedObject in this.zoneBasedVisuals)
			{
				if (zoneBasedObject != null)
				{
					zoneBasedObject.gameObject.SetActive(zoneBasedObject.IsLocalPlayerInZone());
				}
			}
			if (NetworkSystem.Instance.InRoom && ObstacleCourseManager.Instance.IsMine && this.currentState == ObstacleCourse.RaceState.Finished && Time.time - this.startTime >= this.cooldownTime)
			{
				this.RestartTimer(true);
			}
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x0005A38A File Offset: 0x0005858A
		public void OnPlayerEnterZone(Collider other)
		{
			if (ObstacleCourseManager.Instance.IsMine)
			{
				this.numPlayersOnCourse++;
			}
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0005A3A6 File Offset: 0x000585A6
		public void OnPlayerExitZone(Collider other)
		{
			if (ObstacleCourseManager.Instance.IsMine)
			{
				this.numPlayersOnCourse--;
			}
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x0005A3C2 File Offset: 0x000585C2
		private void RestartTimer(bool playFx = true)
		{
			this.UpdateState(ObstacleCourse.RaceState.Started, playFx);
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x0005A3CC File Offset: 0x000585CC
		private void EndRace()
		{
			this.UpdateState(ObstacleCourse.RaceState.Finished, true);
			this.startTime = Time.time;
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x0016E31C File Offset: 0x0016C51C
		public void PlayWinningEffects()
		{
			if (this.confettiParticle)
			{
				this.confettiParticle.Play();
			}
			if (this.bannerRenderer)
			{
				UberShaderProperty baseColor = UberShader.BaseColor;
				Material material = this.bannerRenderer.material;
				RigContainer rigContainer = this.winnerRig;
				baseColor.SetValue<Color?>(material, (rigContainer != null) ? new Color?(rigContainer.Rig.playerColor) : null);
			}
			this.audioSource.GTPlay();
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x0005A3E1 File Offset: 0x000585E1
		public void OnEndLineTrigger(VRRig rig)
		{
			if (ObstacleCourseManager.Instance.IsMine && this.currentState == ObstacleCourse.RaceState.Started)
			{
				this.winnerActorNumber = rig.creator.ActorNumber;
				this.winnerRig = rig.rigContainer;
				this.EndRace();
			}
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x0005A41A File Offset: 0x0005861A
		public void Deserialize(int _winnerActorNumber, ObstacleCourse.RaceState _currentState)
		{
			if (!ObstacleCourseManager.Instance.IsMine)
			{
				this.winnerActorNumber = _winnerActorNumber;
				VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(this.winnerActorNumber), out this.winnerRig);
				this.UpdateState(_currentState, true);
			}
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x0016E394 File Offset: 0x0016C594
		private void UpdateState(ObstacleCourse.RaceState state, bool playFX = true)
		{
			this.currentState = state;
			WinnerScoreboard winnerScoreboard = this.scoreboard;
			RigContainer rigContainer = this.winnerRig;
			winnerScoreboard.UpdateBoard((rigContainer != null) ? rigContainer.Rig.playerNameVisible : null, this.currentState);
			if (this.currentState == ObstacleCourse.RaceState.Finished)
			{
				this.PlayWinningEffects();
			}
			else if (this.currentState == ObstacleCourse.RaceState.Started && this.bannerRenderer)
			{
				UberShader.BaseColor.SetValue<Color>(this.bannerRenderer.material, Color.white);
			}
			this.UpdateStartingGate();
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x0016E418 File Offset: 0x0016C618
		private void UpdateStartingGate()
		{
			if (this.currentState == ObstacleCourse.RaceState.Finished)
			{
				this.leftGate.transform.RotateAround(this.leftGate.transform.position, Vector3.up, 90f);
				this.rightGate.transform.RotateAround(this.rightGate.transform.position, Vector3.up, -90f);
				return;
			}
			if (this.currentState == ObstacleCourse.RaceState.Started)
			{
				this.leftGate.transform.RotateAround(this.leftGate.transform.position, Vector3.up, -90f);
				this.rightGate.transform.RotateAround(this.rightGate.transform.position, Vector3.up, 90f);
			}
		}

		// Token: 0x04004171 RID: 16753
		public WinnerScoreboard scoreboard;

		// Token: 0x04004173 RID: 16755
		private RigContainer winnerRig;

		// Token: 0x04004174 RID: 16756
		public ObstacleCourseZoneTrigger[] zoneTriggers;

		// Token: 0x04004175 RID: 16757
		[HideInInspector]
		public ObstacleCourse.RaceState currentState;

		// Token: 0x04004176 RID: 16758
		[SerializeField]
		private ParticleSystem confettiParticle;

		// Token: 0x04004177 RID: 16759
		[SerializeField]
		private Renderer bannerRenderer;

		// Token: 0x04004178 RID: 16760
		[SerializeField]
		private TappableBell TappableBell;

		// Token: 0x04004179 RID: 16761
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400417A RID: 16762
		[SerializeField]
		private float cooldownTime = 20f;

		// Token: 0x0400417B RID: 16763
		[SerializeField]
		private ZoneBasedObject[] zoneBasedVisuals;

		// Token: 0x0400417C RID: 16764
		public GameObject leftGate;

		// Token: 0x0400417D RID: 16765
		public GameObject rightGate;

		// Token: 0x0400417E RID: 16766
		private int numPlayersOnCourse;

		// Token: 0x0400417F RID: 16767
		private float startTime;

		// Token: 0x02000A15 RID: 2581
		public enum RaceState
		{
			// Token: 0x04004181 RID: 16769
			Started,
			// Token: 0x04004182 RID: 16770
			Waiting,
			// Token: 0x04004183 RID: 16771
			Finished
		}
	}
}
