using System;
using System.Runtime.CompilerServices;
using GorillaTagScripts.AI.States;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.AI.Entities
{
	// Token: 0x02000A44 RID: 2628
	public class TestShark : AIEntity
	{
		// Token: 0x060041EC RID: 16876 RVA: 0x00173CB0 File Offset: 0x00171EB0
		private new void Awake()
		{
			base.Awake();
			this.chasingTimer = 0f;
			this._stateMachine = new StateMachine();
			this.circularPatrol = new CircularPatrol_State(this);
			this.patrol = new Patrol_State(this);
			this.chase = new Chase_State(this);
			this._stateMachine.AddTransition(this.patrol, this.chase, this.<Awake>g__ShouldChase|7_0());
			this._stateMachine.AddTransition(this.chase, this.patrol, this.<Awake>g__ShouldPatrol|7_1());
			this._stateMachine.SetState(this.patrol);
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x00173D48 File Offset: 0x00171F48
		private void Update()
		{
			this._stateMachine.Tick();
			this.shouldChase = false;
			this.chasingTimer += Time.deltaTime;
			if (this.chasingTimer >= this.nextTimeToChasePlayer)
			{
				base.ChooseClosestTarget();
				if (this.followTarget != null)
				{
					this.chase.FollowTarget = this.followTarget;
					this.shouldChase = true;
				}
				this.chasingTimer = 0f;
			}
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x0005B21E File Offset: 0x0005941E
		[CompilerGenerated]
		private Func<bool> <Awake>g__ShouldChase|7_0()
		{
			return () => this.shouldChase && PhotonNetwork.InRoom;
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x0005B23D File Offset: 0x0005943D
		[CompilerGenerated]
		private Func<bool> <Awake>g__ShouldPatrol|7_1()
		{
			return () => this.chase.chaseOver;
		}

		// Token: 0x040042D5 RID: 17109
		public float nextTimeToChasePlayer = 30f;

		// Token: 0x040042D6 RID: 17110
		private float chasingTimer;

		// Token: 0x040042D7 RID: 17111
		private bool shouldChase;

		// Token: 0x040042D8 RID: 17112
		private StateMachine _stateMachine;

		// Token: 0x040042D9 RID: 17113
		private CircularPatrol_State circularPatrol;

		// Token: 0x040042DA RID: 17114
		private Patrol_State patrol;

		// Token: 0x040042DB RID: 17115
		private Chase_State chase;
	}
}
