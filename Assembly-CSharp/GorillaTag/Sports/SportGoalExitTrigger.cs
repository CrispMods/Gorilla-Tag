using System;
using UnityEngine;

namespace GorillaTag.Sports
{
	// Token: 0x02000BE8 RID: 3048
	public class SportGoalExitTrigger : MonoBehaviour
	{
		// Token: 0x06004D28 RID: 19752 RVA: 0x001A9E9C File Offset: 0x001A809C
		private void OnTriggerExit(Collider other)
		{
			SportBall componentInParent = other.GetComponentInParent<SportBall>();
			if (componentInParent != null && this.goalTrigger != null)
			{
				this.goalTrigger.BallExitedGoalTrigger(componentInParent);
			}
		}

		// Token: 0x04004E97 RID: 20119
		[SerializeField]
		private SportGoalTrigger goalTrigger;
	}
}
