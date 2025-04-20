using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BE4 RID: 3044
	[Serializable]
	internal class ExpectedUsersDecayTimer : TickSystemTimerAbstract
	{
		// Token: 0x06004D1C RID: 19740 RVA: 0x001A9D54 File Offset: 0x001A7F54
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

		// Token: 0x06004D1D RID: 19741 RVA: 0x00062A11 File Offset: 0x00060C11
		public override void Stop()
		{
			base.Stop();
			this.expectedUsers.Clear();
		}

		// Token: 0x04004E93 RID: 20115
		public float decayTime = 15f;

		// Token: 0x04004E94 RID: 20116
		private Dictionary<string, float> expectedUsers = new Dictionary<string, float>(10);
	}
}
