using System;
using UnityEngine;
using UnityEngine.AI;

namespace GorillaTagScripts.AI.States
{
	// Token: 0x02000A19 RID: 2585
	public class Patrol_State : IState
	{
		// Token: 0x060040AF RID: 16559 RVA: 0x00133B4B File Offset: 0x00131D4B
		public Patrol_State(AIEntity entity)
		{
			this.entity = entity;
			this.agent = this.entity.navMeshAgent;
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x00133B6C File Offset: 0x00131D6C
		public void Tick()
		{
			if (this.agent.remainingDistance <= this.agent.stoppingDistance)
			{
				Vector3 position = this.entity.waypoints[Random.Range(0, this.entity.waypoints.Count - 1)].transform.position;
				this.agent.SetDestination(position);
			}
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x00133BD4 File Offset: 0x00131DD4
		public void OnEnter()
		{
			string str = "Current State: ";
			Type typeFromHandle = typeof(Patrol_State);
			Debug.Log(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
			if (this.entity.waypoints.Count > 0)
			{
				this.agent.SetDestination(this.entity.waypoints[0].transform.position);
			}
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnExit()
		{
		}

		// Token: 0x040041EB RID: 16875
		private AIEntity entity;

		// Token: 0x040041EC RID: 16876
		private NavMeshAgent agent;
	}
}
