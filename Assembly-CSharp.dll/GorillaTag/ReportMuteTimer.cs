using System;
using Photon.Realtime;

namespace GorillaTag
{
	// Token: 0x02000BB5 RID: 2997
	internal class ReportMuteTimer : TickSystemTimerAbstract, ObjectPoolEvents
	{
		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06004BC3 RID: 19395 RVA: 0x00060F05 File Offset: 0x0005F105
		// (set) Token: 0x06004BC4 RID: 19396 RVA: 0x00060F0D File Offset: 0x0005F10D
		public int Muted { get; set; }

		// Token: 0x06004BC5 RID: 19397 RVA: 0x001A2C70 File Offset: 0x001A0E70
		public override void OnTimedEvent()
		{
			if (!NetworkSystem.Instance.InRoom)
			{
				this.Stop();
				return;
			}
			ReportMuteTimer.content[0] = this.m_playerID;
			ReportMuteTimer.content[1] = this.Muted;
			ReportMuteTimer.content[2] = ((this.m_nickName.Length > 12) ? this.m_nickName.Remove(12) : this.m_nickName);
			ReportMuteTimer.content[3] = NetworkSystem.Instance.LocalPlayer.NickName;
			ReportMuteTimer.content[4] = !NetworkSystem.Instance.SessionIsPrivate;
			ReportMuteTimer.content[5] = NetworkSystem.Instance.RoomStringStripped();
			NetworkSystemRaiseEvent.RaiseEvent(51, ReportMuteTimer.content, ReportMuteTimer.netEventOptions, true);
			this.Stop();
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x00060F16 File Offset: 0x0005F116
		public void SetReportData(string id, string name, int muted)
		{
			this.Muted = muted;
			this.m_playerID = id;
			this.m_nickName = name;
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x0002F75F File Offset: 0x0002D95F
		void ObjectPoolEvents.OnTaken()
		{
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x00060F2D File Offset: 0x0005F12D
		void ObjectPoolEvents.OnReturned()
		{
			if (base.Running)
			{
				this.OnTimedEvent();
			}
			this.m_playerID = string.Empty;
			this.m_nickName = string.Empty;
			this.Muted = 0;
		}

		// Token: 0x04004DA5 RID: 19877
		private static readonly NetEventOptions netEventOptions = new NetEventOptions
		{
			Flags = new WebFlags(1),
			TargetActors = new int[]
			{
				-1
			}
		};

		// Token: 0x04004DA6 RID: 19878
		private static readonly object[] content = new object[6];

		// Token: 0x04004DA7 RID: 19879
		private const byte evCode = 51;

		// Token: 0x04004DA9 RID: 19881
		private string m_playerID;

		// Token: 0x04004DAA RID: 19882
		private string m_nickName;
	}
}
