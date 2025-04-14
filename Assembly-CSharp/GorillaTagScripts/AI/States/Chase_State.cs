using System;
using UnityEngine;
using UnityEngine.AI;

namespace GorillaTagScripts.AI.States
{
	// Token: 0x02000A14 RID: 2580
	public class Chase_State : IState
	{
		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06004099 RID: 16537 RVA: 0x0013343C File Offset: 0x0013163C
		// (set) Token: 0x0600409A RID: 16538 RVA: 0x00133444 File Offset: 0x00131644
		public Transform FollowTarget { get; set; }

		// Token: 0x0600409B RID: 16539 RVA: 0x0013344D File Offset: 0x0013164D
		public Chase_State(AIEntity entity)
		{
			this.entity = entity;
			this.agent = this.entity.navMeshAgent;
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x0013346D File Offset: 0x0013166D
		public void Tick()
		{
			this.agent.SetDestination(this.FollowTarget.position);
			if (this.agent.remainingDistance < this.entity.attackDistance)
			{
				this.chaseOver = true;
			}
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x001334A5 File Offset: 0x001316A5
		public void OnEnter()
		{
			this.chaseOver = false;
			string str = "Current State: ";
			Type typeFromHandle = typeof(Chase_State);
			Debug.Log(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x001334D3 File Offset: 0x001316D3
		public void OnExit()
		{
			this.chaseOver = true;
		}

		// Token: 0x040041D3 RID: 16851
		private AIEntity entity;

		// Token: 0x040041D4 RID: 16852
		private NavMeshAgent agent;

		// Token: 0x040041D6 RID: 16854
		public bool chaseOver;
	}
}
