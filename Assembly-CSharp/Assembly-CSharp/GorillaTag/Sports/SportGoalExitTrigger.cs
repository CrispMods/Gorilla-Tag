using System;
using UnityEngine;

namespace GorillaTag.Sports
{
	// Token: 0x02000BBD RID: 3005
	public class SportGoalExitTrigger : MonoBehaviour
	{
		// Token: 0x06004BE8 RID: 19432 RVA: 0x00171808 File Offset: 0x0016FA08
		private void OnTriggerExit(Collider other)
		{
			SportBall componentInParent = other.GetComponentInParent<SportBall>();
			if (componentInParent != null && this.goalTrigger != null)
			{
				this.goalTrigger.BallExitedGoalTrigger(componentInParent);
			}
		}

		// Token: 0x04004DB3 RID: 19891
		[SerializeField]
		private SportGoalTrigger goalTrigger;
	}
}
