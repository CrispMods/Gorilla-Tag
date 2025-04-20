using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x02000A1C RID: 2588
	public class TappableBell : Tappable
	{
		// Token: 0x14000076 RID: 118
		// (add) Token: 0x060040D0 RID: 16592 RVA: 0x0016E8B4 File Offset: 0x0016CAB4
		// (remove) Token: 0x060040D1 RID: 16593 RVA: 0x0016E8EC File Offset: 0x0016CAEC
		public event TappableBell.ObstacleCourseTriggerEvent OnTapped;

		// Token: 0x060040D2 RID: 16594 RVA: 0x0016E924 File Offset: 0x0016CB24
		public override void OnTapLocal(float tapStrength, float tapTime, PhotonMessageInfoWrapped info)
		{
			if (!PhotonNetwork.LocalPlayer.IsMasterClient)
			{
				return;
			}
			if (!this.rpcCooldown.CheckCallTime(Time.time))
			{
				return;
			}
			this.winnerRig = GorillaGameManager.StaticFindRigForPlayer(info.Sender);
			if (this.winnerRig != null)
			{
				TappableBell.ObstacleCourseTriggerEvent onTapped = this.OnTapped;
				if (onTapped == null)
				{
					return;
				}
				onTapped(this.winnerRig);
			}
		}

		// Token: 0x0400418F RID: 16783
		private VRRig winnerRig;

		// Token: 0x04004191 RID: 16785
		public CallLimiter rpcCooldown;

		// Token: 0x02000A1D RID: 2589
		// (Invoke) Token: 0x060040D5 RID: 16597
		public delegate void ObstacleCourseTriggerEvent(VRRig vrrig);
	}
}
