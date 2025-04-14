using System;
using GorillaExtensions;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009CD RID: 2509
	public class PlayerTimerBoardLine : MonoBehaviour
	{
		// Token: 0x06003E94 RID: 16020 RVA: 0x00128843 File Offset: 0x00126A43
		public void ResetData()
		{
			this.linePlayer = null;
			this.currentNickname = string.Empty;
			this.playerTimeStr = string.Empty;
			this.playerTimeSeconds = 0f;
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x00128870 File Offset: 0x00126A70
		public void SetLineData(NetPlayer netPlayer)
		{
			if (!netPlayer.InRoom || netPlayer == this.linePlayer)
			{
				return;
			}
			this.linePlayer = netPlayer;
			RigContainer rigContainer;
			if (VRRigCache.Instance.TryGetVrrig(netPlayer, out rigContainer))
			{
				this.rigContainer = rigContainer;
				this.playerVRRig = rigContainer.Rig;
			}
			this.InitializeLine();
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x001288BE File Offset: 0x00126ABE
		public void InitializeLine()
		{
			this.currentNickname = string.Empty;
			this.UpdatePlayerText();
			this.UpdateTimeText();
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x001288D8 File Offset: 0x00126AD8
		public void UpdateLine()
		{
			if (this.linePlayer != null)
			{
				if (this.playerNameVisible != this.playerVRRig.playerNameVisible)
				{
					this.UpdatePlayerText();
					this.parentBoard.IsDirty = true;
				}
				string value = this.playerTimeStr;
				this.UpdateTimeText();
				if (!this.playerTimeStr.Equals(value))
				{
					this.parentBoard.IsDirty = true;
				}
			}
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x00128940 File Offset: 0x00126B40
		private void UpdatePlayerText()
		{
			try
			{
				if (this.rigContainer.IsNull() || this.playerVRRig.IsNull())
				{
					this.playerNameVisible = this.NormalizeName(this.linePlayer.NickName != this.currentNickname, this.linePlayer.NickName);
					this.currentNickname = this.linePlayer.NickName;
				}
				else if (this.rigContainer.Initialized)
				{
					this.playerNameVisible = this.playerVRRig.playerNameVisible;
				}
				else if (this.currentNickname.IsNullOrEmpty() || GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(this.linePlayer.UserId))
				{
					this.playerNameVisible = this.NormalizeName(this.linePlayer.NickName != this.currentNickname, this.linePlayer.NickName);
				}
			}
			catch (Exception)
			{
				this.playerNameVisible = this.linePlayer.DefaultName;
				GorillaNot.instance.SendReport("NmError", this.linePlayer.UserId, this.linePlayer.NickName);
			}
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x00128A74 File Offset: 0x00126C74
		private void UpdateTimeText()
		{
			if (this.linePlayer == null || !(PlayerTimerManager.instance != null))
			{
				this.playerTimeStr = "--:--:--";
				return;
			}
			this.playerTimeSeconds = PlayerTimerManager.instance.GetLastDurationForPlayer(this.linePlayer.ActorNumber);
			if (this.playerTimeSeconds > 0f)
			{
				this.playerTimeStr = TimeSpan.FromSeconds((double)this.playerTimeSeconds).ToString("mm\\:ss\\:ff");
				return;
			}
			this.playerTimeStr = "--:--:--";
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x00128AF8 File Offset: 0x00126CF8
		public string NormalizeName(bool doIt, string text)
		{
			if (doIt)
			{
				if (GorillaComputer.instance.CheckAutoBanListForName(text))
				{
					text = new string(Array.FindAll<char>(text.ToCharArray(), (char c) => Utils.IsASCIILetterOrDigit(c)));
					if (text.Length > 12)
					{
						text = text.Substring(0, 11);
					}
					text = text.ToUpper();
				}
				else
				{
					text = "BADGORILLA";
					GorillaNot.instance.SendReport("evading the name ban", this.linePlayer.UserId, this.linePlayer.NickName);
				}
			}
			return text;
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x00128B9C File Offset: 0x00126D9C
		public static int CompareByTotalTime(PlayerTimerBoardLine lineA, PlayerTimerBoardLine lineB)
		{
			if (lineA.playerTimeSeconds > 0f && lineB.playerTimeSeconds > 0f)
			{
				return lineA.playerTimeSeconds.CompareTo(lineB.playerTimeSeconds);
			}
			if (lineA.playerTimeSeconds <= 0f)
			{
				return 1;
			}
			if (lineB.playerTimeSeconds <= 0f)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x04003FDB RID: 16347
		public string playerNameVisible;

		// Token: 0x04003FDC RID: 16348
		public string playerTimeStr;

		// Token: 0x04003FDD RID: 16349
		private float playerTimeSeconds;

		// Token: 0x04003FDE RID: 16350
		public NetPlayer linePlayer;

		// Token: 0x04003FDF RID: 16351
		public VRRig playerVRRig;

		// Token: 0x04003FE0 RID: 16352
		public PlayerTimerBoard parentBoard;

		// Token: 0x04003FE1 RID: 16353
		internal RigContainer rigContainer;

		// Token: 0x04003FE2 RID: 16354
		private string currentNickname;
	}
}
