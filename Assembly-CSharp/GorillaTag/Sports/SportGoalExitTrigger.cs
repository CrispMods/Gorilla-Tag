using System;
using UnityEngine;

namespace GorillaTag.Sports
{
	// Token: 0x02000BBA RID: 3002
	public class SportGoalExitTrigger : MonoBehaviour
	{
		// Token: 0x06004BDC RID: 19420 RVA: 0x00171240 File Offset: 0x0016F440
		private void OnTriggerExit(Collider other)
		{
			SportBall componentInParent = other.GetComponentInParent<SportBall>();
			if (componentInParent != null && this.goalTrigger != null)
			{
				this.goalTrigger.BallExitedGoalTrigger(componentInParent);
			}
		}

		// Token: 0x04004DA1 RID: 19873
		[SerializeField]
		private SportGoalTrigger goalTrigger;
	}
}
