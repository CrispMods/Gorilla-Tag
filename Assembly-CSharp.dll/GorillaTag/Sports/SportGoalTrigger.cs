using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTag.Sports
{
	// Token: 0x02000BBE RID: 3006
	public class SportGoalTrigger : MonoBehaviour
	{
		// Token: 0x06004BEA RID: 19434 RVA: 0x000610D1 File Offset: 0x0005F2D1
		public void BallExitedGoalTrigger(SportBall ball)
		{
			if (this.ballsPendingTriggerExit.Contains(ball))
			{
				this.ballsPendingTriggerExit.Remove(ball);
			}
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x001A2F08 File Offset: 0x001A1108
		private void PruneBallsPendingTriggerExitByDistance()
		{
			foreach (SportBall sportBall in this.ballsPendingTriggerExit)
			{
				if ((sportBall.transform.position - base.transform.position).sqrMagnitude > this.ballTriggerExitDistanceFallback * this.ballTriggerExitDistanceFallback)
				{
					this.ballsPendingTriggerExit.Remove(sportBall);
				}
			}
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x001A2F94 File Offset: 0x001A1194
		private void OnTriggerEnter(Collider other)
		{
			SportBall componentInParent = other.GetComponentInParent<SportBall>();
			if (componentInParent != null && this.scoreboard != null)
			{
				this.PruneBallsPendingTriggerExitByDistance();
				if (!this.ballsPendingTriggerExit.Contains(componentInParent))
				{
					this.scoreboard.TeamScored(this.teamScoringOnThisGoal);
					this.ballsPendingTriggerExit.Add(componentInParent);
				}
			}
		}

		// Token: 0x04004DB4 RID: 19892
		[SerializeField]
		private SportScoreboard scoreboard;

		// Token: 0x04004DB5 RID: 19893
		[SerializeField]
		private int teamScoringOnThisGoal = 1;

		// Token: 0x04004DB6 RID: 19894
		[SerializeField]
		private float ballTriggerExitDistanceFallback = 3f;

		// Token: 0x04004DB7 RID: 19895
		private HashSet<SportBall> ballsPendingTriggerExit = new HashSet<SportBall>();
	}
}
