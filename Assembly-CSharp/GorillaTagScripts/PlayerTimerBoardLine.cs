using System;
using GorillaExtensions;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009F3 RID: 2547
	public class PlayerTimerBoardLine : MonoBehaviour
	{
		// Token: 0x06003FAC RID: 16300 RVA: 0x00059A38 File Offset: 0x00057C38
		public void ResetData()
		{
			this.linePlayer = null;
			this.currentNickname = string.Empty;
			this.playerTimeStr = string.Empty;
			this.playerTimeSeconds = 0f;
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x001698A0 File Offset: 0x00167AA0
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

		// Token: 0x06003FAE RID: 16302 RVA: 0x00059A62 File Offset: 0x00057C62
		public void InitializeLine()
		{
			this.currentNickname = string.Empty;
			this.UpdatePlayerText();
			this.UpdateTimeText();
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x001698F0 File Offset: 0x00167AF0
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

		// Token: 0x06003FB0 RID: 16304 RVA: 0x00169958 File Offset: 0x00167B58
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

		// Token: 0x06003FB1 RID: 16305 RVA: 0x00169A8C File Offset: 0x00167C8C
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

		// Token: 0x06003FB2 RID: 16306 RVA: 0x00169B10 File Offset: 0x00167D10
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

		// Token: 0x06003FB3 RID: 16307 RVA: 0x00169BB4 File Offset: 0x00167DB4
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

		// Token: 0x040040B5 RID: 16565
		public string playerNameVisible;

		// Token: 0x040040B6 RID: 16566
		public string playerTimeStr;

		// Token: 0x040040B7 RID: 16567
		private float playerTimeSeconds;

		// Token: 0x040040B8 RID: 16568
		public NetPlayer linePlayer;

		// Token: 0x040040B9 RID: 16569
		public VRRig playerVRRig;

		// Token: 0x040040BA RID: 16570
		public PlayerTimerBoard parentBoard;

		// Token: 0x040040BB RID: 16571
		internal RigContainer rigContainer;

		// Token: 0x040040BC RID: 16572
		private string currentNickname;
	}
}
