using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class RaceCheckpoint : MonoBehaviour
{
	// Token: 0x06000AF3 RID: 2803 RVA: 0x0003B46B File Offset: 0x0003966B
	public void Init(RaceCheckpointManager manager, int index)
	{
		this.manager = manager;
		this.checkpointIndex = index;
		this.SetIsCorrectCheckpoint(index == 0);
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x0003B485 File Offset: 0x00039685
	public void SetIsCorrectCheckpoint(bool isCorrect)
	{
		this.isCorrect = isCorrect;
		this.banner.sharedMaterial = (isCorrect ? this.activeCheckpointMat : this.wrongCheckpointMat);
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x0003B4AA File Offset: 0x000396AA
	private void OnTriggerEnter(Collider other)
	{
		if (other != GTPlayer.Instance.headCollider)
		{
			return;
		}
		if (this.isCorrect)
		{
			this.manager.OnCheckpointReached(this.checkpointIndex, this.checkpointSound);
			return;
		}
		this.wrongCheckpointSound.Play();
	}

	// Token: 0x04000D57 RID: 3415
	[SerializeField]
	private MeshRenderer banner;

	// Token: 0x04000D58 RID: 3416
	[SerializeField]
	private Material activeCheckpointMat;

	// Token: 0x04000D59 RID: 3417
	[SerializeField]
	private Material wrongCheckpointMat;

	// Token: 0x04000D5A RID: 3418
	[SerializeField]
	private SoundBankPlayer checkpointSound;

	// Token: 0x04000D5B RID: 3419
	[SerializeField]
	private SoundBankPlayer wrongCheckpointSound;

	// Token: 0x04000D5C RID: 3420
	private RaceCheckpointManager manager;

	// Token: 0x04000D5D RID: 3421
	private int checkpointIndex;

	// Token: 0x04000D5E RID: 3422
	private bool isCorrect;
}
