using System;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009E5 RID: 2533
	public class WinnerScoreboard : MonoBehaviour
	{
		// Token: 0x06003F1F RID: 16159 RVA: 0x00165604 File Offset: 0x00163804
		public void UpdateBoard(string winner, ObstacleCourse.RaceState _currentState)
		{
			if (this.output == null)
			{
				return;
			}
			switch (_currentState)
			{
			case ObstacleCourse.RaceState.Started:
				Debug.Log(this.raceStarted);
				this.output.text = this.raceStarted;
				return;
			case ObstacleCourse.RaceState.Waiting:
				Debug.Log(this.raceLoading);
				this.output.text = this.raceLoading;
				return;
			case ObstacleCourse.RaceState.Finished:
				Debug.Log(winner + " WON!!");
				this.output.text = winner + " WON!!";
				return;
			default:
				return;
			}
		}

		// Token: 0x0400404C RID: 16460
		public string raceStarted = "RACE STARTED!";

		// Token: 0x0400404D RID: 16461
		public string raceLoading = "RACE LOADING...";

		// Token: 0x0400404E RID: 16462
		[SerializeField]
		private TextMeshPro output;
	}
}
