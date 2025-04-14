using System;
using UnityEngine;
using UnityEngine.AI;

namespace GorillaTagScripts.AI.States
{
	// Token: 0x02000A17 RID: 2583
	public class Chase_State : IState
	{
		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060040A5 RID: 16549 RVA: 0x00133A04 File Offset: 0x00131C04
		// (set) Token: 0x060040A6 RID: 16550 RVA: 0x00133A0C File Offset: 0x00131C0C
		public Transform FollowTarget { get; set; }

		// Token: 0x060040A7 RID: 16551 RVA: 0x00133A15 File Offset: 0x00131C15
		public Chase_State(AIEntity entity)
		{
			this.entity = entity;
			this.agent = this.entity.navMeshAgent;
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x00133A35 File Offset: 0x00131C35
		public void Tick()
		{
			this.agent.SetDestination(this.FollowTarget.position);
			if (this.agent.remainingDistance < this.entity.attackDistance)
			{
				this.chaseOver = true;
			}
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x00133A6D File Offset: 0x00131C6D
		public void OnEnter()
		{
			this.chaseOver = false;
			string str = "Current State: ";
			Type typeFromHandle = typeof(Chase_State);
			Debug.Log(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x00133A9B File Offset: 0x00131C9B
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
