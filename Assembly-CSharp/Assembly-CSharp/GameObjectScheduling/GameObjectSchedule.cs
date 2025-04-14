using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000C88 RID: 3208
	[CreateAssetMenu(fileName = "New Game Object Schedule", menuName = "Game Object Scheduling/Game Object Schedule", order = 0)]
	public class GameObjectSchedule : ScriptableObject
	{
		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x060050E9 RID: 20713 RVA: 0x00189076 File Offset: 0x00187276
		public GameObjectSchedule.GameObjectScheduleNode[] Nodes
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x060050EA RID: 20714 RVA: 0x0018907E File Offset: 0x0018727E
		public bool InitialState
		{
			get
			{
				return this.initialState;
			}
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x00189088 File Offset: 0x00187288
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

		// Token: 0x060050EC RID: 20716 RVA: 0x001890D9 File Offset: 0x001872D9
		public void Validate()
		{
			if (this.validated)
			{
				return;
			}
			this._validate();
			this.validated = true;
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x001890F4 File Offset: 0x001872F4
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

		// Token: 0x060050EE RID: 20718 RVA: 0x00189160 File Offset: 0x00187360
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

		// Token: 0x04005356 RID: 21334
		[SerializeField]
		private bool initialState;

		// Token: 0x04005357 RID: 21335
		[SerializeField]
		private GameObjectSchedule.GameObjectScheduleNode[] nodes;

		// Token: 0x04005358 RID: 21336
		[SerializeField]
		private SchedulingOptions options;

		// Token: 0x04005359 RID: 21337
		private bool validated;

		// Token: 0x02000C89 RID: 3209
		[Serializable]
		public class GameObjectScheduleNode
		{
			// Token: 0x17000830 RID: 2096
			// (get) Token: 0x060050F0 RID: 20720 RVA: 0x00189297 File Offset: 0x00187497
			public bool ActiveState
			{
				get
				{
					return this.activeState;
				}
			}

			// Token: 0x17000831 RID: 2097
			// (get) Token: 0x060050F1 RID: 20721 RVA: 0x0018929F File Offset: 0x0018749F
			public DateTime DateTime
			{
				get
				{
					return this.dateTime;
				}
			}

			// Token: 0x060050F2 RID: 20722 RVA: 0x001892A8 File Offset: 0x001874A8
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

			// Token: 0x0400535A RID: 21338
			[SerializeField]
			public string activeDateTime = "1/1/0001 00:00:00";

			// Token: 0x0400535B RID: 21339
			[SerializeField]
			[Tooltip("Check to turn on. Uncheck to turn off.")]
			public bool activeState = true;

			// Token: 0x0400535C RID: 21340
			private DateTime dateTime;
		}
	}
}
