using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009E0 RID: 2528
	public class TappableBell : Tappable
	{
		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06003F0B RID: 16139 RVA: 0x0012A9C8 File Offset: 0x00128BC8
		// (remove) Token: 0x06003F0C RID: 16140 RVA: 0x0012AA00 File Offset: 0x00128C00
		public event TappableBell.ObstacleCourseTriggerEvent OnTapped;

		// Token: 0x06003F0D RID: 16141 RVA: 0x0012AA38 File Offset: 0x00128C38
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

		// Token: 0x04004037 RID: 16439
		private VRRig winnerRig;

		// Token: 0x04004039 RID: 16441
		public CallLimiter rpcCooldown;

		// Token: 0x020009E1 RID: 2529
		// (Invoke) Token: 0x06003F10 RID: 16144
		public delegate void ObstacleCourseTriggerEvent(VRRig vrrig);
	}
}
