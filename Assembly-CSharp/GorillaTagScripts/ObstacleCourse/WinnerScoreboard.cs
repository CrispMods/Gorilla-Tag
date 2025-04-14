using System;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009E2 RID: 2530
	public class WinnerScoreboard : MonoBehaviour
	{
		// Token: 0x06003F13 RID: 16147 RVA: 0x0012AA98 File Offset: 0x00128C98
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

		// Token: 0x0400403A RID: 16442
		public string raceStarted = "RACE STARTED!";

		// Token: 0x0400403B RID: 16443
		public string raceLoading = "RACE LOADING...";

		// Token: 0x0400403C RID: 16444
		[SerializeField]
		private TextMeshPro output;
	}
}
