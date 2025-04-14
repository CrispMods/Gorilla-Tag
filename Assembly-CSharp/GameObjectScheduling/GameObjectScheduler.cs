using System;
using System.Collections;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000C88 RID: 3208
	public class GameObjectScheduler : MonoBehaviour
	{
		// Token: 0x060050EB RID: 20715 RVA: 0x00188D70 File Offset: 0x00186F70
		private void Start()
		{
			this.schedule.Validate();
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				list.Add(base.transform.GetChild(i).gameObject);
			}
			this.scheduledGameObject = list.ToArray();
			for (int j = 0; j < this.scheduledGameObject.Length; j++)
			{
				this.scheduledGameObject[j].SetActive(false);
			}
			this.dispatcher = base.GetComponent<GameObjectSchedulerEventDispatcher>();
			this.monitor = base.StartCoroutine(this.MonitorTime());
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x00188E06 File Offset: 0x00187006
		private void OnEnable()
		{
			if (this.monitor == null && this.scheduledGameObject != null)
			{
				this.monitor = base.StartCoroutine(this.MonitorTime());
			}
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x00188E2A File Offset: 0x0018702A
		private void OnDisable()
		{
			if (this.monitor != null)
			{
				base.StopCoroutine(this.monitor);
			}
			this.monitor = null;
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x00188E47 File Offset: 0x00187047
		private IEnumerator MonitorTime()
		{
			while (GorillaComputer.instance == null || GorillaComputer.instance.startupMillis == 0L)
			{
				yield return null;
			}
			bool previousState = this.getActiveState();
			for (int i = 0; i < this.scheduledGameObject.Length; i++)
			{
				this.scheduledGameObject[i].SetActive(previousState);
			}
			for (;;)
			{
				yield return new WaitForSeconds(60f);
				bool activeState = this.getActiveState();
				if (previousState != activeState)
				{
					this.changeActiveState(activeState);
					previousState = activeState;
				}
			}
			yield break;
		}

		// Token: 0x060050EF RID: 20719 RVA: 0x00188E58 File Offset: 0x00187058
		private bool getActiveState()
		{
			this.currentNodeIndex = this.schedule.GetCurrentNodeIndex(this.getServerTime(), 0);
			bool result;
			if (this.currentNodeIndex == -1)
			{
				result = this.schedule.InitialState;
			}
			else if (this.currentNodeIndex < this.schedule.Nodes.Length)
			{
				result = this.schedule.Nodes[this.currentNodeIndex].ActiveState;
			}
			else
			{
				result = this.schedule.Nodes[this.schedule.Nodes.Length - 1].ActiveState;
			}
			return result;
		}

		// Token: 0x060050F0 RID: 20720 RVA: 0x0014F867 File Offset: 0x0014DA67
		private DateTime getServerTime()
		{
			return GorillaComputer.instance.GetServerTime();
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x00188EE8 File Offset: 0x001870E8
		private void changeActiveState(bool state)
		{
			if (state)
			{
				for (int i = 0; i < this.scheduledGameObject.Length; i++)
				{
					this.scheduledGameObject[i].SetActive(true);
				}
				if (this.dispatcher != null && this.dispatcher.OnScheduledActivation != null)
				{
					this.dispatcher.OnScheduledActivation.Invoke();
					return;
				}
			}
			else
			{
				if (this.dispatcher != null && this.dispatcher.OnScheduledDeactivation != null)
				{
					this.dispatcher.OnScheduledActivation.Invoke();
					return;
				}
				for (int j = 0; j < this.scheduledGameObject.Length; j++)
				{
					this.scheduledGameObject[j].SetActive(false);
				}
			}
		}

		// Token: 0x0400534D RID: 21325
		[SerializeField]
		private GameObjectSchedule schedule;

		// Token: 0x0400534E RID: 21326
		private GameObject[] scheduledGameObject;

		// Token: 0x0400534F RID: 21327
		private GameObjectSchedulerEventDispatcher dispatcher;

		// Token: 0x04005350 RID: 21328
		private int currentNodeIndex = -1;

		// Token: 0x04005351 RID: 21329
		private Coroutine monitor;
	}
}
