using System;
using Photon.Realtime;

namespace GorillaTag
{
	// Token: 0x02000BDE RID: 3038
	internal class ReportMuteTimer : TickSystemTimerAbstract, ObjectPoolEvents
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06004CF7 RID: 19703 RVA: 0x0006286C File Offset: 0x00060A6C
		// (set) Token: 0x06004CF8 RID: 19704 RVA: 0x00062874 File Offset: 0x00060A74
		public int Muted { get; set; }

		// Token: 0x06004CF9 RID: 19705 RVA: 0x001A9BE8 File Offset: 0x001A7DE8
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

		// Token: 0x06004CFA RID: 19706 RVA: 0x0006287D File Offset: 0x00060A7D
		public void SetReportData(string id, string name, int muted)
		{
			this.Muted = muted;
			this.m_playerID = id;
			this.m_nickName = name;
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x00030607 File Offset: 0x0002E807
		void ObjectPoolEvents.OnTaken()
		{
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x00062894 File Offset: 0x00060A94
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

		// Token: 0x04004E86 RID: 20102
		private static readonly NetEventOptions netEventOptions = new NetEventOptions
		{
			Flags = new WebFlags(1),
			TargetActors = new int[]
			{
				-1
			}
		};

		// Token: 0x04004E87 RID: 20103
		private static readonly object[] content = new object[6];

		// Token: 0x04004E88 RID: 20104
		private const byte evCode = 51;

		// Token: 0x04004E8A RID: 20106
		private string m_playerID;

		// Token: 0x04004E8B RID: 20107
		private string m_nickName;
	}
}
