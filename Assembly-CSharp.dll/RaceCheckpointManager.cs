using System;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class RaceCheckpointManager : MonoBehaviour
{
	// Token: 0x06000AF7 RID: 2807 RVA: 0x00097C78 File Offset: 0x00095E78
	private void Start()
	{
		this.visual = base.GetComponent<RaceVisual>();
		for (int i = 0; i < this.checkpoints.Length; i++)
		{
			this.checkpoints[i].Init(this, i);
		}
		this.OnRaceEnd();
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x00097CBC File Offset: 0x00095EBC
	public void OnRaceStart()
	{
		for (int i = 0; i < this.checkpoints.Length; i++)
		{
			this.checkpoints[i].SetIsCorrectCheckpoint(i == 0);
		}
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x00097CF0 File Offset: 0x00095EF0
	public void OnRaceEnd()
	{
		for (int i = 0; i < this.checkpoints.Length; i++)
		{
			this.checkpoints[i].SetIsCorrectCheckpoint(false);
		}
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x00036B6B File Offset: 0x00034D6B
	public void OnCheckpointReached(int index, SoundBankPlayer checkpointSound)
	{
		this.checkpoints[index].SetIsCorrectCheckpoint(false);
		this.checkpoints[(index + 1) % this.checkpoints.Length].SetIsCorrectCheckpoint(true);
		this.visual.OnCheckpointPassed(index, checkpointSound);
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x00036BA1 File Offset: 0x00034DA1
	public bool IsPlayerNearCheckpoint(VRRig player, int checkpointIdx)
	{
		return checkpointIdx >= 0 && checkpointIdx < this.checkpoints.Length && player.IsPositionInRange(this.checkpoints[checkpointIdx].transform.position, 6f);
	}

	// Token: 0x04000D5F RID: 3423
	[SerializeField]
	private RaceCheckpoint[] checkpoints;

	// Token: 0x04000D60 RID: 3424
	private RaceVisual visual;
}
