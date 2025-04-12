using System;
using System.Runtime.CompilerServices;
using GorillaTagScripts.AI.States;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.AI.Entities
{
	// Token: 0x02000A1A RID: 2586
	public class TestShark : AIEntity
	{
		// Token: 0x060040B3 RID: 16563 RVA: 0x0016CE2C File Offset: 0x0016B02C
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

		// Token: 0x060040B4 RID: 16564 RVA: 0x0016CEC4 File Offset: 0x0016B0C4
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

		// Token: 0x060040B6 RID: 16566 RVA: 0x0005981C File Offset: 0x00057A1C
		[CompilerGenerated]
		private Func<bool> <Awake>g__ShouldChase|7_0()
		{
			return () => this.shouldChase && PhotonNetwork.InRoom;
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x0005983B File Offset: 0x00057A3B
		[CompilerGenerated]
		private Func<bool> <Awake>g__ShouldPatrol|7_1()
		{
			return () => this.chase.chaseOver;
		}

		// Token: 0x040041ED RID: 16877
		public float nextTimeToChasePlayer = 30f;

		// Token: 0x040041EE RID: 16878
		private float chasingTimer;

		// Token: 0x040041EF RID: 16879
		private bool shouldChase;

		// Token: 0x040041F0 RID: 16880
		private StateMachine _stateMachine;

		// Token: 0x040041F1 RID: 16881
		private CircularPatrol_State circularPatrol;

		// Token: 0x040041F2 RID: 16882
		private Patrol_State patrol;

		// Token: 0x040041F3 RID: 16883
		private Chase_State chase;
	}
}
