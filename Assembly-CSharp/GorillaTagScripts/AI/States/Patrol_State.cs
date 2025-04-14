using System;
using UnityEngine;
using UnityEngine.AI;

namespace GorillaTagScripts.AI.States
{
	// Token: 0x02000A16 RID: 2582
	public class Patrol_State : IState
	{
		// Token: 0x060040A3 RID: 16547 RVA: 0x00133583 File Offset: 0x00131783
		public Patrol_State(AIEntity entity)
		{
			this.entity = entity;
			this.agent = this.entity.navMeshAgent;
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x001335A4 File Offset: 0x001317A4
		public void Tick()
		{
			if (this.agent.remainingDistance <= this.agent.stoppingDistance)
			{
				Vector3 position = this.entity.waypoints[Random.Range(0, this.entity.waypoints.Count - 1)].transform.position;
				this.agent.SetDestination(position);
			}
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x0013360C File Offset: 0x0013180C
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

		// Token: 0x060040A6 RID: 16550 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnExit()
		{
		}

		// Token: 0x040041D9 RID: 16857
		private AIEntity entity;

		// Token: 0x040041DA RID: 16858
		private NavMeshAgent agent;
	}
}
