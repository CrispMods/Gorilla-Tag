using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Sports
{
	// Token: 0x02000BBC RID: 3004
	[RequireComponent(typeof(AudioSource))]
	[NetworkBehaviourWeaved(2)]
	public class SportScoreboard : NetworkComponent
	{
		// Token: 0x06004BE2 RID: 19426 RVA: 0x001713A4 File Offset: 0x0016F5A4
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

		// Token: 0x06004BE3 RID: 19427 RVA: 0x0017140D File Offset: 0x0016F60D
		public void RegisterTeamVisual(int TeamIndex, SportScoreboardVisuals visuals)
		{
			this.scoreVisuals[TeamIndex] = visuals;
			this.UpdateScoreboard();
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x00171420 File Offset: 0x0016F620
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

		// Token: 0x06004BE5 RID: 19429 RVA: 0x001714BC File Offset: 0x0016F6BC
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

		// Token: 0x06004BE6 RID: 19430 RVA: 0x001715B0 File Offset: 0x0016F7B0
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

		// Token: 0x06004BE7 RID: 19431 RVA: 0x00171600 File Offset: 0x0016F800
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

		// Token: 0x06004BE8 RID: 19432 RVA: 0x00171646 File Offset: 0x0016F846
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

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06004BE9 RID: 19433 RVA: 0x0017165C File Offset: 0x0016F85C
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

		// Token: 0x06004BEA RID: 19434 RVA: 0x00171698 File Offset: 0x0016F898
		public override void WriteDataFusion()
		{
			this.Data.CopyFrom(this.teamScores, 0, this.teamScores.Count);
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x001716C8 File Offset: 0x0016F8C8
		public override void ReadDataFusion()
		{
			this.teamScores.Clear();
			this.Data.CopyTo(this.teamScores);
			this.OnScoreUpdated();
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x001716FC File Offset: 0x0016F8FC
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			for (int i = 0; i < this.teamScores.Count; i++)
			{
				stream.SendNext(this.teamScores[i]);
			}
		}

		// Token: 0x06004BED RID: 19437 RVA: 0x00171738 File Offset: 0x0016F938
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			for (int i = 0; i < this.teamScores.Count; i++)
			{
				this.teamScores[i] = (int)stream.ReceiveNext();
			}
			this.OnScoreUpdated();
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x001717B3 File Offset: 0x0016F9B3
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			NetworkBehaviourUtils.InitializeNetworkArray<int>(this.Data, this._Data, "Data");
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x001717D5 File Offset: 0x0016F9D5
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			NetworkBehaviourUtils.CopyFromNetworkArray<int>(this.Data, ref this._Data);
		}

		// Token: 0x04004DA6 RID: 19878
		[OnEnterPlay_SetNull]
		public static SportScoreboard Instance;

		// Token: 0x04004DA7 RID: 19879
		[SerializeField]
		private List<SportScoreboard.TeamParameters> teamParameters = new List<SportScoreboard.TeamParameters>();

		// Token: 0x04004DA8 RID: 19880
		[SerializeField]
		private int matchEndScore = 3;

		// Token: 0x04004DA9 RID: 19881
		[SerializeField]
		private float matchEndScoreResetDelayTime = 3f;

		// Token: 0x04004DAA RID: 19882
		private List<int> teamScores = new List<int>();

		// Token: 0x04004DAB RID: 19883
		private List<int> teamScoresPrev = new List<int>();

		// Token: 0x04004DAC RID: 19884
		private bool runningMatchEndCoroutine;

		// Token: 0x04004DAD RID: 19885
		private AudioSource audioSource;

		// Token: 0x04004DAE RID: 19886
		private SportScoreboardVisuals[] scoreVisuals;

		// Token: 0x04004DAF RID: 19887
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 2)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int[] _Data;

		// Token: 0x02000BBD RID: 3005
		[Serializable]
		private class TeamParameters
		{
			// Token: 0x04004DB0 RID: 19888
			[SerializeField]
			public AudioClip matchWonAudio;

			// Token: 0x04004DB1 RID: 19889
			[SerializeField]
			public AudioClip goalScoredAudio;
		}
	}
}
