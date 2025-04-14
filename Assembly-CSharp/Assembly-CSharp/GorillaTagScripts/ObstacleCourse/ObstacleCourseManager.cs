using System;
using System.Collections.Generic;
using Fusion;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009DD RID: 2525
	[NetworkBehaviourWeaved(9)]
	public class ObstacleCourseManager : NetworkComponent, ITickSystemTick
	{
		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06003EED RID: 16109 RVA: 0x0012A9E2 File Offset: 0x00128BE2
		// (set) Token: 0x06003EEE RID: 16110 RVA: 0x0012A9E9 File Offset: 0x00128BE9
		public static ObstacleCourseManager Instance { get; private set; }

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06003EEF RID: 16111 RVA: 0x0012A9F1 File Offset: 0x00128BF1
		// (set) Token: 0x06003EF0 RID: 16112 RVA: 0x0012A9F9 File Offset: 0x00128BF9
		public bool TickRunning { get; set; }

		// Token: 0x06003EF1 RID: 16113 RVA: 0x0012AA02 File Offset: 0x00128C02
		protected override void Awake()
		{
			base.Awake();
			ObstacleCourseManager.Instance = this;
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x0012AA10 File Offset: 0x00128C10
		internal override void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			base.OnEnable();
			TickSystem<object>.AddCallbackTarget(this);
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x0012AA24 File Offset: 0x00128C24
		internal override void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			base.OnEnable();
			TickSystem<object>.RemoveCallbackTarget(this);
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x0012AA38 File Offset: 0x00128C38
		public void Tick()
		{
			foreach (ObstacleCourse obstacleCourse in this.allObstaclesCourses)
			{
				obstacleCourse.InvokeUpdate();
			}
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x0012AA88 File Offset: 0x00128C88
		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			this.allObstaclesCourses.Clear();
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06003EF6 RID: 16118 RVA: 0x0012AA9B File Offset: 0x00128C9B
		// (set) Token: 0x06003EF7 RID: 16119 RVA: 0x0012AAC5 File Offset: 0x00128CC5
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

		// Token: 0x06003EF8 RID: 16120 RVA: 0x0012AAF0 File Offset: 0x00128CF0
		public override void WriteDataFusion()
		{
			this.Data = new ObstacleCourseData(this.allObstaclesCourses);
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x0012AB04 File Offset: 0x00128D04
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

		// Token: 0x06003EFA RID: 16122 RVA: 0x0012AB84 File Offset: 0x00128D84
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

		// Token: 0x06003EFB RID: 16123 RVA: 0x0012AC04 File Offset: 0x00128E04
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

		// Token: 0x06003EFD RID: 16125 RVA: 0x0012AC89 File Offset: 0x00128E89
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x0012ACA1 File Offset: 0x00128EA1
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x0400403F RID: 16447
		public List<ObstacleCourse> allObstaclesCourses = new List<ObstacleCourse>();

		// Token: 0x04004041 RID: 16449
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 9)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ObstacleCourseData _Data;
	}
}
