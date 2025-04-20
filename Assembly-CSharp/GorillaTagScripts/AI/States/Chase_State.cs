using System;
using UnityEngine;
using UnityEngine.AI;

namespace GorillaTagScripts.AI.States
{
	// Token: 0x02000A41 RID: 2625
	public class Chase_State : IState
	{
		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060041DE RID: 16862 RVA: 0x0005B13C File Offset: 0x0005933C
		// (set) Token: 0x060041DF RID: 16863 RVA: 0x0005B144 File Offset: 0x00059344
		public Transform FollowTarget { get; set; }

		// Token: 0x060041E0 RID: 16864 RVA: 0x0005B14D File Offset: 0x0005934D
		public Chase_State(AIEntity entity)
		{
			this.entity = entity;
			this.agent = this.entity.navMeshAgent;
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x0005B16D File Offset: 0x0005936D
		public void Tick()
		{
			this.agent.SetDestination(this.FollowTarget.position);
			if (this.agent.remainingDistance < this.entity.attackDistance)
			{
				this.chaseOver = true;
			}
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x0005B1A5 File Offset: 0x000593A5
		public void OnEnter()
		{
			this.chaseOver = false;
			string str = "Current State: ";
			Type typeFromHandle = typeof(Chase_State);
			Debug.Log(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x0005B1D3 File Offset: 0x000593D3
		public void OnExit()
		{
			this.chaseOver = true;
		}

		// Token: 0x040042CD RID: 17101
		private AIEntity entity;

		// Token: 0x040042CE RID: 17102
		private NavMeshAgent agent;

		// Token: 0x040042D0 RID: 17104
		public bool chaseOver;
	}
}
