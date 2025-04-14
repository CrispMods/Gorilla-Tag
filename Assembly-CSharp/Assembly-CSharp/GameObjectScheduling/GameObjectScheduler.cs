using System;
using System.Collections;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000C8B RID: 3211
	public class GameObjectScheduler : MonoBehaviour
	{
		// Token: 0x060050F7 RID: 20727 RVA: 0x00189338 File Offset: 0x00187538
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

		// Token: 0x060050F8 RID: 20728 RVA: 0x001893CE File Offset: 0x001875CE
		private void OnEnable()
		{
			if (this.monitor == null && this.scheduledGameObject != null)
			{
				this.monitor = base.StartCoroutine(this.MonitorTime());
			}
		}

		// Token: 0x060050F9 RID: 20729 RVA: 0x001893F2 File Offset: 0x001875F2
		private void OnDisable()
		{
			if (this.monitor != null)
			{
				base.StopCoroutine(this.monitor);
			}
			this.monitor = null;
		}

		// Token: 0x060050FA RID: 20730 RVA: 0x0018940F File Offset: 0x0018760F
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

		// Token: 0x060050FB RID: 20731 RVA: 0x00189420 File Offset: 0x00187620
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

		// Token: 0x060050FC RID: 20732 RVA: 0x0014FE2F File Offset: 0x0014E02F
		private DateTime getServerTime()
		{
			return GorillaComputer.instance.GetServerTime();
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x001894B0 File Offset: 0x001876B0
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

		// Token: 0x0400535F RID: 21343
		[SerializeField]
		private GameObjectSchedule schedule;

		// Token: 0x04005360 RID: 21344
		private GameObject[] scheduledGameObject;

		// Token: 0x04005361 RID: 21345
		private GameObjectSchedulerEventDispatcher dispatcher;

		// Token: 0x04005362 RID: 21346
		private int currentNodeIndex = -1;

		// Token: 0x04005363 RID: 21347
		private Coroutine monitor;
	}
}
