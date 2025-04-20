using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Sports
{
	// Token: 0x02000BEA RID: 3050
	[RequireComponent(typeof(AudioSource))]
	[NetworkBehaviourWeaved(2)]
	public class SportScoreboard : NetworkComponent
	{
		// Token: 0x06004D2E RID: 19758 RVA: 0x001A9FC0 File Offset: 0x001A81C0
		protected override void Awake()
		{
			base.Awake();
			SportScoreboard.Instance = this;
			this.audioSource = base.GetComponent<AudioSource>();
			this.scoreVisuals = new SportScoreboardVisuals[this.teamParameters.Count];
			for (int i = 0; i < this.teamParameters.Count; i++)
			{
				this.teamScores.Add(0);
				this.teamScoresPrev.Add(0);
			}
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x00062AD4 File Offset: 0x00060CD4
		public void RegisterTeamVisual(int TeamIndex, SportScoreboardVisuals visuals)
		{
			this.scoreVisuals[TeamIndex] = visuals;
			this.UpdateScoreboard();
		}

		// Token: 0x06004D30 RID: 19760 RVA: 0x001AA02C File Offset: 0x001A822C
		private void UpdateScoreboard()
		{
			for (int i = 0; i < this.teamParameters.Count; i++)
			{
				if (!(this.scoreVisuals[i] == null))
				{
					int num = this.teamScores[i];
					if (this.scoreVisuals[i].score1s != null)
					{
						this.scoreVisuals[i].score1s.SetUVOffset(num % 10);
					}
					if (this.scoreVisuals[i].score10s != null)
					{
						this.scoreVisuals[i].score10s.SetUVOffset(num / 10 % 10);
					}
				}
			}
		}

		// Token: 0x06004D31 RID: 19761 RVA: 0x001AA0C8 File Offset: 0x001A82C8
		private void OnScoreUpdated()
		{
			for (int i = 0; i < this.teamScores.Count; i++)
			{
				if (this.teamScores[i] > this.teamScoresPrev[i] && this.teamParameters[i].goalScoredAudio != null && this.teamScores[i] < this.matchEndScore)
				{
					this.audioSource.GTPlayOneShot(this.teamParameters[i].goalScoredAudio, 1f);
				}
				this.teamScoresPrev[i] = this.teamScores[i];
			}
			if (!this.runningMatchEndCoroutine)
			{
				for (int j = 0; j < this.teamScores.Count; j++)
				{
					if (this.teamScores[j] >= this.matchEndScore)
					{
						base.StartCoroutine(this.MatchEndCoroutine(j));
						break;
					}
				}
			}
			this.UpdateScoreboard();
		}

		// Token: 0x06004D32 RID: 19762 RVA: 0x001AA1BC File Offset: 0x001A83BC
		public void TeamScored(int team)
		{
			if (base.IsMine && !this.runningMatchEndCoroutine)
			{
				if (team >= 0 && team < this.teamScores.Count)
				{
					this.teamScores[team] = this.teamScores[team] + 1;
				}
				this.OnScoreUpdated();
			}
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x001AA20C File Offset: 0x001A840C
		public void ResetScores()
		{
			if (base.IsMine && !this.runningMatchEndCoroutine)
			{
				for (int i = 0; i < this.teamScores.Count; i++)
				{
					this.teamScores[i] = 0;
				}
				this.OnScoreUpdated();
			}
		}

		// Token: 0x06004D34 RID: 19764 RVA: 0x00062AE5 File Offset: 0x00060CE5
		private IEnumerator MatchEndCoroutine(int winningTeam)
		{
			this.runningMatchEndCoroutine = true;
			if (winningTeam >= 0 && winningTeam < this.teamParameters.Count && this.teamParameters[winningTeam].matchWonAudio != null)
			{
				this.audioSource.GTPlayOneShot(this.teamParameters[winningTeam].matchWonAudio, 1f);
			}
			yield return new WaitForSeconds(this.matchEndScoreResetDelayTime);
			this.runningMatchEndCoroutine = false;
			this.ResetScores();
			yield break;
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06004D35 RID: 19765 RVA: 0x001AA254 File Offset: 0x001A8454
		[Networked]
		[Capacity(2)]
		[NetworkedWeaved(0, 2)]
		public unsafe NetworkArray<int> Data
		{
			get
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SportScoreboard.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkArray<int>((byte*)(this.Ptr + 0), 2, ReaderWriter@System_Int32.GetInstance());
			}
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x001AA290 File Offset: 0x001A8490
		public override void WriteDataFusion()
		{
			this.Data.CopyFrom(this.teamScores, 0, this.teamScores.Count);
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x001AA2C0 File Offset: 0x001A84C0
		public override void ReadDataFusion()
		{
			this.teamScores.Clear();
			this.Data.CopyTo(this.teamScores);
			this.OnScoreUpdated();
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x001AA2F4 File Offset: 0x001A84F4
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			for (int i = 0; i < this.teamScores.Count; i++)
			{
				stream.SendNext(this.teamScores[i]);
			}
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x001AA330 File Offset: 0x001A8530
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			for (int i = 0; i < this.teamScores.Count; i++)
			{
				this.teamScores[i] = (int)stream.ReceiveNext();
			}
			this.OnScoreUpdated();
		}

		// Token: 0x06004D3B RID: 19771 RVA: 0x00062B36 File Offset: 0x00060D36
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			NetworkBehaviourUtils.InitializeNetworkArray<int>(this.Data, this._Data, "Data");
		}

		// Token: 0x06004D3C RID: 19772 RVA: 0x00062B58 File Offset: 0x00060D58
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			NetworkBehaviourUtils.CopyFromNetworkArray<int>(this.Data, ref this._Data);
		}

		// Token: 0x04004E9C RID: 20124
		[OnEnterPlay_SetNull]
		public static SportScoreboard Instance;

		// Token: 0x04004E9D RID: 20125
		[SerializeField]
		private List<SportScoreboard.TeamParameters> teamParameters = new List<SportScoreboard.TeamParameters>();

		// Token: 0x04004E9E RID: 20126
		[SerializeField]
		private int matchEndScore = 3;

		// Token: 0x04004E9F RID: 20127
		[SerializeField]
		private float matchEndScoreResetDelayTime = 3f;

		// Token: 0x04004EA0 RID: 20128
		private List<int> teamScores = new List<int>();

		// Token: 0x04004EA1 RID: 20129
		private List<int> teamScoresPrev = new List<int>();

		// Token: 0x04004EA2 RID: 20130
		private bool runningMatchEndCoroutine;

		// Token: 0x04004EA3 RID: 20131
		private AudioSource audioSource;

		// Token: 0x04004EA4 RID: 20132
		private SportScoreboardVisuals[] scoreVisuals;

		// Token: 0x04004EA5 RID: 20133
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 2)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int[] _Data;

		// Token: 0x02000BEB RID: 3051
		[Serializable]
		private class TeamParameters
		{
			// Token: 0x04004EA6 RID: 20134
			[SerializeField]
			public AudioClip matchWonAudio;

			// Token: 0x04004EA7 RID: 20135
			[SerializeField]
			public AudioClip goalScoredAudio;
		}
	}
}
