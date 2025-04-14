using System;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009D8 RID: 2520
	public class ObstacleCourse : MonoBehaviour
	{
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06003ED1 RID: 16081 RVA: 0x00129FF8 File Offset: 0x001281F8
		// (set) Token: 0x06003ED2 RID: 16082 RVA: 0x0012A000 File Offset: 0x00128200
		public int winnerActorNumber { get; private set; }

		// Token: 0x06003ED3 RID: 16083 RVA: 0x0012A00C File Offset: 0x0012820C
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

		// Token: 0x06003ED4 RID: 16084 RVA: 0x0012A080 File Offset: 0x00128280
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

		// Token: 0x06003ED5 RID: 16085 RVA: 0x0012A0ED File Offset: 0x001282ED
		private void Start()
		{
			this.RestartTimer(false);
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x0012A0F8 File Offset: 0x001282F8
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

		// Token: 0x06003ED7 RID: 16087 RVA: 0x0012A174 File Offset: 0x00128374
		public void OnPlayerEnterZone(Collider other)
		{
			if (ObstacleCourseManager.Instance.IsMine)
			{
				this.numPlayersOnCourse++;
			}
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x0012A190 File Offset: 0x00128390
		public void OnPlayerExitZone(Collider other)
		{
			if (ObstacleCourseManager.Instance.IsMine)
			{
				this.numPlayersOnCourse--;
			}
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x0012A1AC File Offset: 0x001283AC
		private void RestartTimer(bool playFx = true)
		{
			this.UpdateState(ObstacleCourse.RaceState.Started, playFx);
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x0012A1B6 File Offset: 0x001283B6
		private void EndRace()
		{
			this.UpdateState(ObstacleCourse.RaceState.Finished, true);
			this.startTime = Time.time;
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x0012A1CC File Offset: 0x001283CC
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

		// Token: 0x06003EDC RID: 16092 RVA: 0x0012A242 File Offset: 0x00128442
		public void OnEndLineTrigger(VRRig rig)
		{
			if (ObstacleCourseManager.Instance.IsMine && this.currentState == ObstacleCourse.RaceState.Started)
			{
				this.winnerActorNumber = rig.creator.ActorNumber;
				this.winnerRig = rig.rigContainer;
				this.EndRace();
			}
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x0012A27B File Offset: 0x0012847B
		public void Deserialize(int _winnerActorNumber, ObstacleCourse.RaceState _currentState)
		{
			if (!ObstacleCourseManager.Instance.IsMine)
			{
				this.winnerActorNumber = _winnerActorNumber;
				VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(this.winnerActorNumber), out this.winnerRig);
				this.UpdateState(_currentState, true);
			}
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x0012A2BC File Offset: 0x001284BC
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

		// Token: 0x06003EDF RID: 16095 RVA: 0x0012A340 File Offset: 0x00128540
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

		// Token: 0x04004019 RID: 16409
		public WinnerScoreboard scoreboard;

		// Token: 0x0400401B RID: 16411
		private RigContainer winnerRig;

		// Token: 0x0400401C RID: 16412
		public ObstacleCourseZoneTrigger[] zoneTriggers;

		// Token: 0x0400401D RID: 16413
		[HideInInspector]
		public ObstacleCourse.RaceState currentState;

		// Token: 0x0400401E RID: 16414
		[SerializeField]
		private ParticleSystem confettiParticle;

		// Token: 0x0400401F RID: 16415
		[SerializeField]
		private Renderer bannerRenderer;

		// Token: 0x04004020 RID: 16416
		[SerializeField]
		private TappableBell TappableBell;

		// Token: 0x04004021 RID: 16417
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04004022 RID: 16418
		[SerializeField]
		private float cooldownTime = 20f;

		// Token: 0x04004023 RID: 16419
		[SerializeField]
		private ZoneBasedObject[] zoneBasedVisuals;

		// Token: 0x04004024 RID: 16420
		public GameObject leftGate;

		// Token: 0x04004025 RID: 16421
		public GameObject rightGate;

		// Token: 0x04004026 RID: 16422
		private int numPlayersOnCourse;

		// Token: 0x04004027 RID: 16423
		private float startTime;

		// Token: 0x020009D9 RID: 2521
		public enum RaceState
		{
			// Token: 0x04004029 RID: 16425
			Started,
			// Token: 0x0400402A RID: 16426
			Waiting,
			// Token: 0x0400402B RID: 16427
			Finished
		}
	}
}
