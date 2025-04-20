using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020001E1 RID: 481
public class RaceCheckpoint : MonoBehaviour
{
	// Token: 0x06000B3D RID: 2877 RVA: 0x00037DAC File Offset: 0x00035FAC
	public void Init(RaceCheckpointManager manager, int index)
	{
		this.manager = manager;
		this.checkpointIndex = index;
		this.SetIsCorrectCheckpoint(index == 0);
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x00037DC6 File Offset: 0x00035FC6
	public void SetIsCorrectCheckpoint(bool isCorrect)
	{
		this.isCorrect = isCorrect;
		this.banner.sharedMaterial = (isCorrect ? this.activeCheckpointMat : this.wrongCheckpointMat);
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x00037DEB File Offset: 0x00035FEB
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

	// Token: 0x04000D9C RID: 3484
	[SerializeField]
	private MeshRenderer banner;

	// Token: 0x04000D9D RID: 3485
	[SerializeField]
	private Material activeCheckpointMat;

	// Token: 0x04000D9E RID: 3486
	[SerializeField]
	private Material wrongCheckpointMat;

	// Token: 0x04000D9F RID: 3487
	[SerializeField]
	private SoundBankPlayer checkpointSound;

	// Token: 0x04000DA0 RID: 3488
	[SerializeField]
	private SoundBankPlayer wrongCheckpointSound;

	// Token: 0x04000DA1 RID: 3489
	private RaceCheckpointManager manager;

	// Token: 0x04000DA2 RID: 3490
	private int checkpointIndex;

	// Token: 0x04000DA3 RID: 3491
	private bool isCorrect;
}
