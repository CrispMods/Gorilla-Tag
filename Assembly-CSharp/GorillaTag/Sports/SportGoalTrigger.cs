using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTag.Sports
{
	// Token: 0x02000BBB RID: 3003
	public class SportGoalTrigger : MonoBehaviour
	{
		// Token: 0x06004BDE RID: 19422 RVA: 0x00171277 File Offset: 0x0016F477
		public void BallExitedGoalTrigger(SportBall ball)
		{
			if (this.ballsPendingTriggerExit.Contains(ball))
			{
				this.ballsPendingTriggerExit.Remove(ball);
			}
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x00171294 File Offset: 0x0016F494
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

		// Token: 0x06004BE0 RID: 19424 RVA: 0x00171320 File Offset: 0x0016F520
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

		// Token: 0x04004DA2 RID: 19874
		[SerializeField]
		private SportScoreboard scoreboard;

		// Token: 0x04004DA3 RID: 19875
		[SerializeField]
		private int teamScoringOnThisGoal = 1;

		// Token: 0x04004DA4 RID: 19876
		[SerializeField]
		private float ballTriggerExitDistanceFallback = 3f;

		// Token: 0x04004DA5 RID: 19877
		private HashSet<SportBall> ballsPendingTriggerExit = new HashSet<SportBall>();
	}
}
