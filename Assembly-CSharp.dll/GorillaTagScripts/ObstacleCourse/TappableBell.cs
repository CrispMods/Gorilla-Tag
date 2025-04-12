using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009E3 RID: 2531
	public class TappableBell : Tappable
	{
		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06003F17 RID: 16151 RVA: 0x00165534 File Offset: 0x00163734
		// (remove) Token: 0x06003F18 RID: 16152 RVA: 0x0016556C File Offset: 0x0016376C
		public event TappableBell.ObstacleCourseTriggerEvent OnTapped;

		// Token: 0x06003F19 RID: 16153 RVA: 0x001655A4 File Offset: 0x001637A4
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

		// Token: 0x04004049 RID: 16457
		private VRRig winnerRig;

		// Token: 0x0400404B RID: 16459
		public CallLimiter rpcCooldown;

		// Token: 0x020009E4 RID: 2532
		// (Invoke) Token: 0x06003F1C RID: 16156
		public delegate void ObstacleCourseTriggerEvent(VRRig vrrig);
	}
}
