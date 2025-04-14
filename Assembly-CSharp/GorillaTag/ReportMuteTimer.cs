using System;
using Photon.Realtime;

namespace GorillaTag
{
	// Token: 0x02000BB2 RID: 2994
	internal class ReportMuteTimer : TickSystemTimerAbstract, ObjectPoolEvents
	{
		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06004BB7 RID: 19383 RVA: 0x00170E11 File Offset: 0x0016F011
		// (set) Token: 0x06004BB8 RID: 19384 RVA: 0x00170E19 File Offset: 0x0016F019
		public int Muted { get; set; }

		// Token: 0x06004BB9 RID: 19385 RVA: 0x00170E24 File Offset: 0x0016F024
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

		// Token: 0x06004BBA RID: 19386 RVA: 0x00170EE6 File Offset: 0x0016F0E6
		public void SetReportData(string id, string name, int muted)
		{
			this.Muted = muted;
			this.m_playerID = id;
			this.m_nickName = name;
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x000023F4 File Offset: 0x000005F4
		void ObjectPoolEvents.OnTaken()
		{
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x00170EFD File Offset: 0x0016F0FD
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

		// Token: 0x04004D93 RID: 19859
		private static readonly NetEventOptions netEventOptions = new NetEventOptions
		{
			Flags = new WebFlags(1),
			TargetActors = new int[]
			{
				-1
			}
		};

		// Token: 0x04004D94 RID: 19860
		private static readonly object[] content = new object[6];

		// Token: 0x04004D95 RID: 19861
		private const byte evCode = 51;

		// Token: 0x04004D97 RID: 19863
		private string m_playerID;

		// Token: 0x04004D98 RID: 19864
		private string m_nickName;
	}
}
