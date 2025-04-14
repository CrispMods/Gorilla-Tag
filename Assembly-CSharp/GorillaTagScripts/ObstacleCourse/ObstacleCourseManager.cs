using System;
using System.Collections.Generic;
using Fusion;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009DA RID: 2522
	[NetworkBehaviourWeaved(9)]
	public class ObstacleCourseManager : NetworkComponent, ITickSystemTick
	{
		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06003EE1 RID: 16097 RVA: 0x0012A41A File Offset: 0x0012861A
		// (set) Token: 0x06003EE2 RID: 16098 RVA: 0x0012A421 File Offset: 0x00128621
		public static ObstacleCourseManager Instance { get; private set; }

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06003EE3 RID: 16099 RVA: 0x0012A429 File Offset: 0x00128629
		// (set) Token: 0x06003EE4 RID: 16100 RVA: 0x0012A431 File Offset: 0x00128631
		public bool TickRunning { get; set; }

		// Token: 0x06003EE5 RID: 16101 RVA: 0x0012A43A File Offset: 0x0012863A
		protected override void Awake()
		{
			base.Awake();
			ObstacleCourseManager.Instance = this;
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x0012A448 File Offset: 0x00128648
		internal override void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			base.OnEnable();
			TickSystem<object>.AddCallbackTarget(this);
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x0012A45C File Offset: 0x0012865C
		internal override void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			base.OnEnable();
			TickSystem<object>.RemoveCallbackTarget(this);
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x0012A470 File Offset: 0x00128670
		public void Tick()
		{
			foreach (ObstacleCourse obstacleCourse in this.allObstaclesCourses)
			{
				obstacleCourse.InvokeUpdate();
			}
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x0012A4C0 File Offset: 0x001286C0
		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			this.allObstaclesCourses.Clear();
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06003EEA RID: 16106 RVA: 0x0012A4D3 File Offset: 0x001286D3
		// (set) Token: 0x06003EEB RID: 16107 RVA: 0x0012A4FD File Offset: 0x001286FD
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

		// Token: 0x06003EEC RID: 16108 RVA: 0x0012A528 File Offset: 0x00128728
		public override void WriteDataFusion()
		{
			this.Data = new ObstacleCourseData(this.allObstaclesCourses);
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x0012A53C File Offset: 0x0012873C
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

		// Token: 0x06003EEE RID: 16110 RVA: 0x0012A5BC File Offset: 0x001287BC
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

		// Token: 0x06003EEF RID: 16111 RVA: 0x0012A63C File Offset: 0x0012883C
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

		// Token: 0x06003EF1 RID: 16113 RVA: 0x0012A6C1 File Offset: 0x001288C1
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x0012A6D9 File Offset: 0x001288D9
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x0400402D RID: 16429
		public List<ObstacleCourse> allObstaclesCourses = new List<ObstacleCourse>();

		// Token: 0x0400402F RID: 16431
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 9)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ObstacleCourseData _Data;
	}
}
