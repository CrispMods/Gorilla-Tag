using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000C85 RID: 3205
	[CreateAssetMenu(fileName = "New Game Object Schedule", menuName = "Game Object Scheduling/Game Object Schedule", order = 0)]
	public class GameObjectSchedule : ScriptableObject
	{
		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x060050DD RID: 20701 RVA: 0x00188AAE File Offset: 0x00186CAE
		public GameObjectSchedule.GameObjectScheduleNode[] Nodes
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x060050DE RID: 20702 RVA: 0x00188AB6 File Offset: 0x00186CB6
		public bool InitialState
		{
			get
			{
				return this.initialState;
			}
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x00188AC0 File Offset: 0x00186CC0
		public int GetCurrentNodeIndex(DateTime currentDate, int startFrom = 0)
		{
			if (startFrom >= this.nodes.Length)
			{
				return int.MaxValue;
			}
			for (int i = -1; i < this.nodes.Length - 1; i++)
			{
				if (currentDate < this.nodes[i + 1].DateTime)
				{
					return i;
				}
			}
			return int.MaxValue;
		}

		// Token: 0x060050E0 RID: 20704 RVA: 0x00188B11 File Offset: 0x00186D11
		public void Validate()
		{
			if (this.validated)
			{
				return;
			}
			this._validate();
			this.validated = true;
		}

		// Token: 0x060050E1 RID: 20705 RVA: 0x00188B2C File Offset: 0x00186D2C
		private void _validate()
		{
			for (int i = 0; i < this.nodes.Length; i++)
			{
				this.nodes[i].Validate();
			}
			List<GameObjectSchedule.GameObjectScheduleNode> list = new List<GameObjectSchedule.GameObjectScheduleNode>(this.nodes);
			list.Sort((GameObjectSchedule.GameObjectScheduleNode e1, GameObjectSchedule.GameObjectScheduleNode e2) => e1.DateTime.CompareTo(e2.DateTime));
			this.nodes = list.ToArray();
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x00188B98 File Offset: 0x00186D98
		public static void GenerateDailyShuffle(DateTime startDate, DateTime endDate, GameObjectSchedule[] schedules)
		{
			TimeSpan t = TimeSpan.FromDays(1.0);
			int num = schedules.Length - 1;
			int num2 = schedules.Length - 2;
			DateTime dateTime = startDate;
			List<GameObjectSchedule.GameObjectScheduleNode>[] array = new List<GameObjectSchedule.GameObjectScheduleNode>[schedules.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new List<GameObjectSchedule.GameObjectScheduleNode>();
			}
			while (dateTime < endDate)
			{
				int num3 = Random.Range(0, schedules.Length - 2);
				if (num <= num3)
				{
					num3++;
					if (num2 <= num3)
					{
						num3++;
					}
				}
				else if (num2 <= num3)
				{
					num3++;
					if (num <= num3)
					{
						num3++;
					}
				}
				array[num].Add(new GameObjectSchedule.GameObjectScheduleNode
				{
					activeDateTime = dateTime.ToString(),
					activeState = false
				});
				array[num3].Add(new GameObjectSchedule.GameObjectScheduleNode
				{
					activeDateTime = dateTime.ToString(),
					activeState = true
				});
				dateTime += t;
				num2 = num;
				num = num3;
			}
			array[num].Add(new GameObjectSchedule.GameObjectScheduleNode
			{
				activeDateTime = dateTime.ToString(),
				activeState = false
			});
			for (int j = 0; j < array.Length; j++)
			{
				schedules[j].nodes = array[j].ToArray();
			}
		}

		// Token: 0x04005344 RID: 21316
		[SerializeField]
		private bool initialState;

		// Token: 0x04005345 RID: 21317
		[SerializeField]
		private GameObjectSchedule.GameObjectScheduleNode[] nodes;

		// Token: 0x04005346 RID: 21318
		[SerializeField]
		private SchedulingOptions options;

		// Token: 0x04005347 RID: 21319
		private bool validated;

		// Token: 0x02000C86 RID: 3206
		[Serializable]
		public class GameObjectScheduleNode
		{
			// Token: 0x1700082F RID: 2095
			// (get) Token: 0x060050E4 RID: 20708 RVA: 0x00188CCF File Offset: 0x00186ECF
			public bool ActiveState
			{
				get
				{
					return this.activeState;
				}
			}

			// Token: 0x17000830 RID: 2096
			// (get) Token: 0x060050E5 RID: 20709 RVA: 0x00188CD7 File Offset: 0x00186ED7
			public DateTime DateTime
			{
				get
				{
					return this.dateTime;
				}
			}

			// Token: 0x060050E6 RID: 20710 RVA: 0x00188CE0 File Offset: 0x00186EE0
			public void Validate()
			{
				try
				{
					this.dateTime = DateTime.Parse(this.activeDateTime, CultureInfo.InvariantCulture);
				}
				catch
				{
					this.dateTime = DateTime.MinValue;
				}
			}

			// Token: 0x04005348 RID: 21320
			[SerializeField]
			public string activeDateTime = "1/1/0001 00:00:00";

			// Token: 0x04005349 RID: 21321
			[SerializeField]
			[Tooltip("Check to turn on. Uncheck to turn off.")]
			public bool activeState = true;

			// Token: 0x0400534A RID: 21322
			private DateTime dateTime;
		}
	}
}
