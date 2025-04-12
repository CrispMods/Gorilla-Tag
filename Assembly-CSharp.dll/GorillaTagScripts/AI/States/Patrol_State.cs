using System;
using UnityEngine;
using UnityEngine.AI;

namespace GorillaTagScripts.AI.States
{
	// Token: 0x02000A19 RID: 2585
	public class Patrol_State : IState
	{
		// Token: 0x060040AF RID: 16559 RVA: 0x000597E9 File Offset: 0x000579E9
		public Patrol_State(AIEntity entity)
		{
			this.entity = entity;
			this.agent = this.entity.navMeshAgent;
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x0016CD58 File Offset: 0x0016AF58
		public void Tick()
		{
			if (this.agent.remainingDistance <= this.agent.stoppingDistance)
			{
				Vector3 position = this.entity.waypoints[UnityEngine.Random.Range(0, this.entity.waypoints.Count - 1)].transform.position;
				this.agent.SetDestination(position);
			}
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0016CDC0 File Offset: 0x0016AFC0
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

		// Token: 0x060040B2 RID: 16562 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnExit()
		{
		}

		// Token: 0x040041EB RID: 16875
		private AIEntity entity;

		// Token: 0x040041EC RID: 16876
		private NavMeshAgent agent;
	}
}
