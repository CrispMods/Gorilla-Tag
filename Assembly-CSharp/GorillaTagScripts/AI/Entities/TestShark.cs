using System;
using System.Runtime.CompilerServices;
using GorillaTagScripts.AI.States;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.AI.Entities
{
	// Token: 0x02000A17 RID: 2583
	public class TestShark : AIEntity
	{
		// Token: 0x060040A7 RID: 16551 RVA: 0x00133678 File Offset: 0x00131878
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

		// Token: 0x060040A8 RID: 16552 RVA: 0x00133710 File Offset: 0x00131910
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

		// Token: 0x060040AA RID: 16554 RVA: 0x00133799 File Offset: 0x00131999
		[CompilerGenerated]
		private Func<bool> <Awake>g__ShouldChase|7_0()
		{
			return () => this.shouldChase && PhotonNetwork.InRoom;
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x001337B8 File Offset: 0x001319B8
		[CompilerGenerated]
		private Func<bool> <Awake>g__ShouldPatrol|7_1()
		{
			return () => this.chase.chaseOver;
		}

		// Token: 0x040041DB RID: 16859
		public float nextTimeToChasePlayer = 30f;

		// Token: 0x040041DC RID: 16860
		private float chasingTimer;

		// Token: 0x040041DD RID: 16861
		private bool shouldChase;

		// Token: 0x040041DE RID: 16862
		private StateMachine _stateMachine;

		// Token: 0x040041DF RID: 16863
		private CircularPatrol_State circularPatrol;

		// Token: 0x040041E0 RID: 16864
		private Patrol_State patrol;

		// Token: 0x040041E1 RID: 16865
		private Chase_State chase;
	}
}
