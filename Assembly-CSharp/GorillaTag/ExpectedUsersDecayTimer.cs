using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BB6 RID: 2998
	[Serializable]
	internal class ExpectedUsersDecayTimer : TickSystemTimerAbstract
	{
		// Token: 0x06004BD3 RID: 19411 RVA: 0x00171084 File Offset: 0x0016F284
		public override void OnTimedEvent()
		{
			if (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.IsMasterClient)
			{
				int num = 0;
				if (PhotonNetwork.CurrentRoom.ExpectedUsers != null && PhotonNetwork.CurrentRoom.ExpectedUsers.Length != 0)
				{
					foreach (string key in PhotonNetwork.CurrentRoom.ExpectedUsers)
					{
						float num2;
						if (this.expectedUsers.TryGetValue(key, out num2))
						{
							if (num2 + this.decayTime < Time.time)
							{
								num++;
							}
						}
						else
						{
							this.expectedUsers.Add(key, Time.time);
						}
					}
					if (num >= PhotonNetwork.CurrentRoom.ExpectedUsers.Length && num != 0)
					{
						PhotonNetwork.CurrentRoom.ClearExpectedUsers();
						this.expectedUsers.Clear();
					}
				}
			}
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x0017114B File Offset: 0x0016F34B
		public override void Stop()
		{
			base.Stop();
			this.expectedUsers.Clear();
		}

		// Token: 0x04004D9D RID: 19869
		public float decayTime = 15f;

		// Token: 0x04004D9E RID: 19870
		private Dictionary<string, float> expectedUsers = new Dictionary<string, float>(10);
	}
}
