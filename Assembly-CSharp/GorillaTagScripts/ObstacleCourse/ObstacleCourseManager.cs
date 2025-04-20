using System;
using System.Collections.Generic;
using Fusion;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x02000A16 RID: 2582
	[NetworkBehaviourWeaved(9)]
	public class ObstacleCourseManager : NetworkComponent, ITickSystemTick
	{
		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060040A6 RID: 16550 RVA: 0x0005A46B File Offset: 0x0005866B
		// (set) Token: 0x060040A7 RID: 16551 RVA: 0x0005A472 File Offset: 0x00058672
		public static ObstacleCourseManager Instance { get; private set; }

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060040A8 RID: 16552 RVA: 0x0005A47A File Offset: 0x0005867A
		// (set) Token: 0x060040A9 RID: 16553 RVA: 0x0005A482 File Offset: 0x00058682
		public bool TickRunning { get; set; }

		// Token: 0x060040AA RID: 16554 RVA: 0x0005A48B File Offset: 0x0005868B
		protected override void Awake()
		{
			base.Awake();
			ObstacleCourseManager.Instance = this;
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x0005A499 File Offset: 0x00058699
		internal override void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			base.OnEnable();
			TickSystem<object>.AddCallbackTarget(this);
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x0005A4AD File Offset: 0x000586AD
		internal override void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			base.OnEnable();
			TickSystem<object>.RemoveCallbackTarget(this);
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x0016E4E0 File Offset: 0x0016C6E0
		public void Tick()
		{
			foreach (ObstacleCourse obstacleCourse in this.allObstaclesCourses)
			{
				obstacleCourse.InvokeUpdate();
			}
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x0005A4C1 File Offset: 0x000586C1
		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			this.allObstaclesCourses.Clear();
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060040AF RID: 16559 RVA: 0x0005A4D4 File Offset: 0x000586D4
		// (set) Token: 0x060040B0 RID: 16560 RVA: 0x0005A4FE File Offset: 0x000586FE
		[Networked]
		[NetworkedWeaved(0, 9)]
		public unsafe ObstacleCourseData Data
		{
			get
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ObstacleCourseManager.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(ObstacleCourseData*)(this.Ptr + 0);
			}
			set
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ObstacleCourseManager.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(ObstacleCourseData*)(this.Ptr + 0) = value;
			}
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0005A529 File Offset: 0x00058729
		public override void WriteDataFusion()
		{
			this.Data = new ObstacleCourseData(this.allObstaclesCourses);
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x0016E530 File Offset: 0x0016C730
		public override void ReadDataFusion()
		{
			for (int i = 0; i < this.Data.ObstacleCourseCount; i++)
			{
				int winnerActorNumber = this.Data.WinnerActorNumber[i];
				ObstacleCourse.RaceState raceState = (ObstacleCourse.RaceState)this.Data.CurrentRaceState[i];
				if (this.allObstaclesCourses[i].currentState != raceState)
				{
					this.allObstaclesCourses[i].Deserialize(winnerActorNumber, raceState);
				}
			}
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x0016E5B0 File Offset: 0x0016C7B0
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != PhotonNetwork.MasterClient)
			{
				return;
			}
			stream.SendNext(this.allObstaclesCourses.Count);
			for (int i = 0; i < this.allObstaclesCourses.Count; i++)
			{
				stream.SendNext(this.allObstaclesCourses[i].winnerActorNumber);
				stream.SendNext(this.allObstaclesCourses[i].currentState);
			}
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x0016E630 File Offset: 0x0016C830
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != PhotonNetwork.MasterClient)
			{
				return;
			}
			int num = (int)stream.ReceiveNext();
			for (int i = 0; i < num; i++)
			{
				int winnerActorNumber = (int)stream.ReceiveNext();
				ObstacleCourse.RaceState raceState = (ObstacleCourse.RaceState)stream.ReceiveNext();
				if (this.allObstaclesCourses[i].currentState != raceState)
				{
					this.allObstaclesCourses[i].Deserialize(winnerActorNumber, raceState);
				}
			}
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x0005A54F File Offset: 0x0005874F
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x0005A567 File Offset: 0x00058767
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x04004185 RID: 16773
		public List<ObstacleCourse> allObstaclesCourses = new List<ObstacleCourse>();

		// Token: 0x04004187 RID: 16775
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 9)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ObstacleCourseData _Data;
	}
}
