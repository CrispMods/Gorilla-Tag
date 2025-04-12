using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Sports
{
	// Token: 0x02000BBF RID: 3007
	[RequireComponent(typeof(AudioSource))]
	[NetworkBehaviourWeaved(2)]
	public class SportScoreboard : NetworkComponent
	{
		// Token: 0x06004BEE RID: 19438 RVA: 0x001A2FF4 File Offset: 0x001A11F4
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

		// Token: 0x06004BEF RID: 19439 RVA: 0x00061113 File Offset: 0x0005F313
		public void RegisterTeamVisual(int TeamIndex, SportScoreboardVisuals visuals)
		{
			this.scoreVisuals[TeamIndex] = visuals;
			this.UpdateScoreboard();
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x001A3060 File Offset: 0x001A1260
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

		// Token: 0x06004BF1 RID: 19441 RVA: 0x001A30FC File Offset: 0x001A12FC
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

		// Token: 0x06004BF2 RID: 19442 RVA: 0x001A31F0 File Offset: 0x001A13F0
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

		// Token: 0x06004BF3 RID: 19443 RVA: 0x001A3240 File Offset: 0x001A1440
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

		// Token: 0x06004BF4 RID: 19444 RVA: 0x00061124 File Offset: 0x0005F324
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

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06004BF5 RID: 19445 RVA: 0x001A3288 File Offset: 0x001A1488
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

		// Token: 0x06004BF6 RID: 19446 RVA: 0x001A32C4 File Offset: 0x001A14C4
		public override void WriteDataFusion()
		{
			this.Data.CopyFrom(this.teamScores, 0, this.teamScores.Count);
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x001A32F4 File Offset: 0x001A14F4
		public override void ReadDataFusion()
		{
			this.teamScores.Clear();
			this.Data.CopyTo(this.teamScores);
			this.OnScoreUpdated();
		}

		// Token: 0x06004BF8 RID: 19448 RVA: 0x001A3328 File Offset: 0x001A1528
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			for (int i = 0; i < this.teamScores.Count; i++)
			{
				stream.SendNext(this.teamScores[i]);
			}
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x001A3364 File Offset: 0x001A1564
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			for (int i = 0; i < this.teamScores.Count; i++)
			{
				this.teamScores[i] = (int)stream.ReceiveNext();
			}
			this.OnScoreUpdated();
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x00061175 File Offset: 0x0005F375
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			NetworkBehaviourUtils.InitializeNetworkArray<int>(this.Data, this._Data, "Data");
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x00061197 File Offset: 0x0005F397
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			NetworkBehaviourUtils.CopyFromNetworkArray<int>(this.Data, ref this._Data);
		}

		// Token: 0x04004DB8 RID: 19896
		[OnEnterPlay_SetNull]
		public static SportScoreboard Instance;

		// Token: 0x04004DB9 RID: 19897
		[SerializeField]
		private List<SportScoreboard.TeamParameters> teamParameters = new List<SportScoreboard.TeamParameters>();

		// Token: 0x04004DBA RID: 19898
		[SerializeField]
		private int matchEndScore = 3;

		// Token: 0x04004DBB RID: 19899
		[SerializeField]
		private float matchEndScoreResetDelayTime = 3f;

		// Token: 0x04004DBC RID: 19900
		private List<int> teamScores = new List<int>();

		// Token: 0x04004DBD RID: 19901
		private List<int> teamScoresPrev = new List<int>();

		// Token: 0x04004DBE RID: 19902
		private bool runningMatchEndCoroutine;

		// Token: 0x04004DBF RID: 19903
		private AudioSource audioSource;

		// Token: 0x04004DC0 RID: 19904
		private SportScoreboardVisuals[] scoreVisuals;

		// Token: 0x04004DC1 RID: 19905
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 2)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int[] _Data;

		// Token: 0x02000BC0 RID: 3008
		[Serializable]
		private class TeamParameters
		{
			// Token: 0x04004DC2 RID: 19906
			[SerializeField]
			public AudioClip matchWonAudio;

			// Token: 0x04004DC3 RID: 19907
			[SerializeField]
			public AudioClip goalScoredAudio;
		}
	}
}
