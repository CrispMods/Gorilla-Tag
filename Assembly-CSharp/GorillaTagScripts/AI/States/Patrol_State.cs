using System;
using UnityEngine;
using UnityEngine.AI;

namespace GorillaTagScripts.AI.States
{
	// Token: 0x02000A43 RID: 2627
	public class Patrol_State : IState
	{
		// Token: 0x060041E8 RID: 16872 RVA: 0x0005B1EB File Offset: 0x000593EB
		public Patrol_State(AIEntity entity)
		{
			this.entity = entity;
			this.agent = this.entity.navMeshAgent;
		}

		// Token: 0x060041E9 RID: 16873 RVA: 0x00173BDC File Offset: 0x00171DDC
		public void Tick()
		{
			if (this.agent.remainingDistance <= this.agent.stoppingDistance)
			{
				Vector3 position = this.entity.waypoints[UnityEngine.Random.Range(0, this.entity.waypoints.Count - 1)].transform.position;
				this.agent.SetDestination(position);
			}
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x00173C44 File Offset: 0x00171E44
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

		// Token: 0x060041EB RID: 16875 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnExit()
		{
		}

		// Token: 0x040042D3 RID: 17107
		private AIEntity entity;

		// Token: 0x040042D4 RID: 17108
		private NavMeshAgent agent;
	}
}
