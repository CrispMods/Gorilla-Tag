using System;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class RaceCheckpointManager : MonoBehaviour
{
	// Token: 0x06000AF5 RID: 2805 RVA: 0x0003B1C8 File Offset: 0x000393C8
	private void Start()
	{
		this.visual = base.GetComponent<RaceVisual>();
		for (int i = 0; i < this.checkpoints.Length; i++)
		{
			this.checkpoints[i].Init(this, i);
		}
		this.OnRaceEnd();
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x0003B20C File Offset: 0x0003940C
	public void OnRaceStart()
	{
		for (int i = 0; i < this.checkpoints.Length; i++)
		{
			this.checkpoints[i].SetIsCorrectCheckpoint(i == 0);
		}
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x0003B240 File Offset: 0x00039440
	public void OnRaceEnd()
	{
		for (int i = 0; i < this.checkpoints.Length; i++)
		{
			this.checkpoints[i].SetIsCorrectCheckpoint(false);
		}
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0003B26E File Offset: 0x0003946E
	public void OnCheckpointReached(int index, SoundBankPlayer checkpointSound)
	{
		this.checkpoints[index].SetIsCorrectCheckpoint(false);
		this.checkpoints[(index + 1) % this.checkpoints.Length].SetIsCorrectCheckpoint(true);
		this.visual.OnCheckpointPassed(index, checkpointSound);
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0003B2A4 File Offset: 0x000394A4
	public bool IsPlayerNearCheckpoint(VRRig player, int checkpointIdx)
	{
		return checkpointIdx >= 0 && checkpointIdx < this.checkpoints.Length && player.IsPositionInRange(this.checkpoints[checkpointIdx].transform.position, 6f);
	}

	// Token: 0x04000D5E RID: 3422
	[SerializeField]
	private RaceCheckpoint[] checkpoints;

	// Token: 0x04000D5F RID: 3423
	private RaceVisual visual;
}
