using System;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009DB RID: 2523
	public class ObstacleCourse : MonoBehaviour
	{
		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06003EDD RID: 16093 RVA: 0x0012A5C0 File Offset: 0x001287C0
		// (set) Token: 0x06003EDE RID: 16094 RVA: 0x0012A5C8 File Offset: 0x001287C8
		public int winnerActorNumber { get; private set; }

		// Token: 0x06003EDF RID: 16095 RVA: 0x0012A5D4 File Offset: 0x001287D4
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

		// Token: 0x06003EE0 RID: 16096 RVA: 0x0012A648 File Offset: 0x00128848
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

		// Token: 0x06003EE1 RID: 16097 RVA: 0x0012A6B5 File Offset: 0x001288B5
		private void Start()
		{
			this.RestartTimer(false);
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x0012A6C0 File Offset: 0x001288C0
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

		// Token: 0x06003EE3 RID: 16099 RVA: 0x0012A73C File Offset: 0x0012893C
		public void OnPlayerEnterZone(Collider other)
		{
			if (ObstacleCourseManager.Instance.IsMine)
			{
				this.numPlayersOnCourse++;
			}
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x0012A758 File Offset: 0x00128958
		public void OnPlayerExitZone(Collider other)
		{
			if (ObstacleCourseManager.Instance.IsMine)
			{
				this.numPlayersOnCourse--;
			}
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x0012A774 File Offset: 0x00128974
		private void RestartTimer(bool playFx = true)
		{
			this.UpdateState(ObstacleCourse.RaceState.Started, playFx);
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x0012A77E File Offset: 0x0012897E
		private void EndRace()
		{
			this.UpdateState(ObstacleCourse.RaceState.Finished, true);
			this.startTime = Time.time;
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x0012A794 File Offset: 0x00128994
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

		// Token: 0x06003EE8 RID: 16104 RVA: 0x0012A80A File Offset: 0x00128A0A
		public void OnEndLineTrigger(VRRig rig)
		{
			if (ObstacleCourseManager.Instance.IsMine && this.currentState == ObstacleCourse.RaceState.Started)
			{
				this.winnerActorNumber = rig.creator.ActorNumber;
				this.winnerRig = rig.rigContainer;
				this.EndRace();
			}
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x0012A843 File Offset: 0x00128A43
		public void Deserialize(int _winnerActorNumber, ObstacleCourse.RaceState _currentState)
		{
			if (!ObstacleCourseManager.Instance.IsMine)
			{
				this.winnerActorNumber = _winnerActorNumber;
				VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(this.winnerActorNumber), out this.winnerRig);
				this.UpdateState(_currentState, true);
			}
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x0012A884 File Offset: 0x00128A84
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

		// Token: 0x06003EEB RID: 16107 RVA: 0x0012A908 File Offset: 0x00128B08
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

		// Token: 0x0400402B RID: 16427
		public WinnerScoreboard scoreboard;

		// Token: 0x0400402D RID: 16429
		private RigContainer winnerRig;

		// Token: 0x0400402E RID: 16430
		public ObstacleCourseZoneTrigger[] zoneTriggers;

		// Token: 0x0400402F RID: 16431
		[HideInInspector]
		public ObstacleCourse.RaceState currentState;

		// Token: 0x04004030 RID: 16432
		[SerializeField]
		private ParticleSystem confettiParticle;

		// Token: 0x04004031 RID: 16433
		[SerializeField]
		private Renderer bannerRenderer;

		// Token: 0x04004032 RID: 16434
		[SerializeField]
		private TappableBell TappableBell;

		// Token: 0x04004033 RID: 16435
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04004034 RID: 16436
		[SerializeField]
		private float cooldownTime = 20f;

		// Token: 0x04004035 RID: 16437
		[SerializeField]
		private ZoneBasedObject[] zoneBasedVisuals;

		// Token: 0x04004036 RID: 16438
		public GameObject leftGate;

		// Token: 0x04004037 RID: 16439
		public GameObject rightGate;

		// Token: 0x04004038 RID: 16440
		private int numPlayersOnCourse;

		// Token: 0x04004039 RID: 16441
		private float startTime;

		// Token: 0x020009DC RID: 2524
		public enum RaceState
		{
			// Token: 0x0400403B RID: 16443
			Started,
			// Token: 0x0400403C RID: 16444
			Waiting,
			// Token: 0x0400403D RID: 16445
			Finished
		}
	}
}
