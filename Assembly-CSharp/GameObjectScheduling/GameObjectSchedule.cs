using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000CB6 RID: 3254
	[CreateAssetMenu(fileName = "New Game Object Schedule", menuName = "Game Object Scheduling/Game Object Schedule", order = 0)]
	public class GameObjectSchedule : ScriptableObject
	{
		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x0600523F RID: 21055 RVA: 0x00065595 File Offset: 0x00063795
		public GameObjectSchedule.GameObjectScheduleNode[] Nodes
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06005240 RID: 21056 RVA: 0x0006559D File Offset: 0x0006379D
		public bool InitialState
		{
			get
			{
				return this.initialState;
			}
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x001BFA4C File Offset: 0x001BDC4C
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

		// Token: 0x06005242 RID: 21058 RVA: 0x000655A5 File Offset: 0x000637A5
		public void Validate()
		{
			if (this.validated)
			{
				return;
			}
			this._validate();
			this.validated = true;
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x001BFAA0 File Offset: 0x001BDCA0
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

		// Token: 0x06005244 RID: 21060 RVA: 0x001BFB0C File Offset: 0x001BDD0C
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
				int num3 = UnityEngine.Random.Range(0, schedules.Length - 2);
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

		// Token: 0x04005450 RID: 21584
		[SerializeField]
		private bool initialState;

		// Token: 0x04005451 RID: 21585
		[SerializeField]
		private GameObjectSchedule.GameObjectScheduleNode[] nodes;

		// Token: 0x04005452 RID: 21586
		[SerializeField]
		private SchedulingOptions options;

		// Token: 0x04005453 RID: 21587
		private bool validated;

		// Token: 0x02000CB7 RID: 3255
		[Serializable]
		public class GameObjectScheduleNode
		{
			// Token: 0x1700084D RID: 2125
			// (get) Token: 0x06005246 RID: 21062 RVA: 0x000655BD File Offset: 0x000637BD
			public bool ActiveState
			{
				get
				{
					return this.activeState;
				}
			}

			// Token: 0x1700084E RID: 2126
			// (get) Token: 0x06005247 RID: 21063 RVA: 0x000655C5 File Offset: 0x000637C5
			public DateTime DateTime
			{
				get
				{
					return this.dateTime;
				}
			}

			// Token: 0x06005248 RID: 21064 RVA: 0x001BFC44 File Offset: 0x001BDE44
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

			// Token: 0x04005454 RID: 21588
			[SerializeField]
			public string activeDateTime = "1/1/0001 00:00:00";

			// Token: 0x04005455 RID: 21589
			[SerializeField]
			[Tooltip("Check to turn on. Uncheck to turn off.")]
			public bool activeState = true;

			// Token: 0x04005456 RID: 21590
			private DateTime dateTime;
		}
	}
}
