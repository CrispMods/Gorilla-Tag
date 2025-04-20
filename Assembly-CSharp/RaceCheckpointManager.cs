using System;
using UnityEngine;

// Token: 0x020001E2 RID: 482
public class RaceCheckpointManager : MonoBehaviour
{
	// Token: 0x06000B41 RID: 2881 RVA: 0x0009A56C File Offset: 0x0009876C
	private void Start()
	{
		this.visual = base.GetComponent<RaceVisual>();
		for (int i = 0; i < this.checkpoints.Length; i++)
		{
			this.checkpoints[i].Init(this, i);
		}
		this.OnRaceEnd();
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x0009A5B0 File Offset: 0x000987B0
	public void OnRaceStart()
	{
		for (int i = 0; i < this.checkpoints.Length; i++)
		{
			this.checkpoints[i].SetIsCorrectCheckpoint(i == 0);
		}
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x0009A5E4 File Offset: 0x000987E4
	public void OnRaceEnd()
	{
		for (int i = 0; i < this.checkpoints.Length; i++)
		{
			this.checkpoints[i].SetIsCorrectCheckpoint(false);
		}
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x00037E2B File Offset: 0x0003602B
	public void OnCheckpointReached(int index, SoundBankPlayer checkpointSound)
	{
		this.checkpoints[index].SetIsCorrectCheckpoint(false);
		this.checkpoints[(index + 1) % this.checkpoints.Length].SetIsCorrectCheckpoint(true);
		this.visual.OnCheckpointPassed(index, checkpointSound);
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x00037E61 File Offset: 0x00036061
	public bool IsPlayerNearCheckpoint(VRRig player, int checkpointIdx)
	{
		return checkpointIdx >= 0 && checkpointIdx < this.checkpoints.Length && player.IsPositionInRange(this.checkpoints[checkpointIdx].transform.position, 6f);
	}

	// Token: 0x04000DA4 RID: 3492
	[SerializeField]
	private RaceCheckpoint[] checkpoints;

	// Token: 0x04000DA5 RID: 3493
	private RaceVisual visual;
}
