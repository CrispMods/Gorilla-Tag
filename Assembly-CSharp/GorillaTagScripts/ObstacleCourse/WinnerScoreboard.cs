using System;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x02000A1E RID: 2590
	public class WinnerScoreboard : MonoBehaviour
	{
		// Token: 0x060040D8 RID: 16600 RVA: 0x0016E984 File Offset: 0x0016CB84
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

		// Token: 0x04004192 RID: 16786
		public string raceStarted = "RACE STARTED!";

		// Token: 0x04004193 RID: 16787
		public string raceLoading = "RACE LOADING...";

		// Token: 0x04004194 RID: 16788
		[SerializeField]
		private TextMeshPro output;
	}
}
