using System;
using System.Collections;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000CB9 RID: 3257
	public class GameObjectScheduler : MonoBehaviour
	{
		// Token: 0x0600524D RID: 21069 RVA: 0x001BFCAC File Offset: 0x001BDEAC
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

		// Token: 0x0600524E RID: 21070 RVA: 0x000655F3 File Offset: 0x000637F3
		private void OnEnable()
		{
			if (this.monitor == null && this.scheduledGameObject != null)
			{
				this.monitor = base.StartCoroutine(this.MonitorTime());
			}
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x00065617 File Offset: 0x00063817
		private void OnDisable()
		{
			if (this.monitor != null)
			{
				base.StopCoroutine(this.monitor);
			}
			this.monitor = null;
		}

		// Token: 0x06005250 RID: 21072 RVA: 0x00065634 File Offset: 0x00063834
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

		// Token: 0x06005251 RID: 21073 RVA: 0x001BFD44 File Offset: 0x001BDF44
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

		// Token: 0x06005252 RID: 21074 RVA: 0x0005EE1B File Offset: 0x0005D01B
		private DateTime getServerTime()
		{
			return GorillaComputer.instance.GetServerTime();
		}

		// Token: 0x06005253 RID: 21075 RVA: 0x001BFDD4 File Offset: 0x001BDFD4
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

		// Token: 0x04005459 RID: 21593
		[SerializeField]
		private GameObjectSchedule schedule;

		// Token: 0x0400545A RID: 21594
		private GameObject[] scheduledGameObject;

		// Token: 0x0400545B RID: 21595
		private GameObjectSchedulerEventDispatcher dispatcher;

		// Token: 0x0400545C RID: 21596
		private int currentNodeIndex = -1;

		// Token: 0x0400545D RID: 21597
		private Coroutine monitor;
	}
}
