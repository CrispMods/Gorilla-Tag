using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTag.Sports
{
	// Token: 0x02000BE9 RID: 3049
	public class SportGoalTrigger : MonoBehaviour
	{
		// Token: 0x06004D2A RID: 19754 RVA: 0x00062A92 File Offset: 0x00060C92
		public void BallExitedGoalTrigger(SportBall ball)
		{
			if (this.ballsPendingTriggerExit.Contains(ball))
			{
				this.ballsPendingTriggerExit.Remove(ball);
			}
		}

		// Token: 0x06004D2B RID: 19755 RVA: 0x001A9ED4 File Offset: 0x001A80D4
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

		// Token: 0x06004D2C RID: 19756 RVA: 0x001A9F60 File Offset: 0x001A8160
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

		// Token: 0x04004E98 RID: 20120
		[SerializeField]
		private SportScoreboard scoreboard;

		// Token: 0x04004E99 RID: 20121
		[SerializeField]
		private int teamScoringOnThisGoal = 1;

		// Token: 0x04004E9A RID: 20122
		[SerializeField]
		private float ballTriggerExitDistanceFallback = 3f;

		// Token: 0x04004E9B RID: 20123
		private HashSet<SportBall> ballsPendingTriggerExit = new HashSet<SportBall>();
	}
}
