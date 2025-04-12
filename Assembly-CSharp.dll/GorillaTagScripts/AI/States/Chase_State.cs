using System;
using UnityEngine;
using UnityEngine.AI;

namespace GorillaTagScripts.AI.States
{
	// Token: 0x02000A17 RID: 2583
	public class Chase_State : IState
	{
		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060040A5 RID: 16549 RVA: 0x0005973A File Offset: 0x0005793A
		// (set) Token: 0x060040A6 RID: 16550 RVA: 0x00059742 File Offset: 0x00057942
		public Transform FollowTarget { get; set; }

		// Token: 0x060040A7 RID: 16551 RVA: 0x0005974B File Offset: 0x0005794B
		public Chase_State(AIEntity entity)
		{
			this.entity = entity;
			this.agent = this.entity.navMeshAgent;
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x0005976B File Offset: 0x0005796B
		public void Tick()
		{
			this.agent.SetDestination(this.FollowTarget.position);
			if (this.agent.remainingDistance < this.entity.attackDistance)
			{
				this.chaseOver = true;
			}
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x000597A3 File Offset: 0x000579A3
		public void OnEnter()
		{
			this.chaseOver = false;
			string str = "Current State: ";
			Type typeFromHandle = typeof(Chase_State);
			Debug.Log(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x000597D1 File Offset: 0x000579D1
		public void OnExit()
		{
			this.chaseOver = true;
		}

		// Token: 0x040041E5 RID: 16869
		private AIEntity entity;

		// Token: 0x040041E6 RID: 16870
		private NavMeshAgent agent;

		// Token: 0x040041E8 RID: 16872
		public bool chaseOver;
	}
}
