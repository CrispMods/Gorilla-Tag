using System;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class DoorSlidingOpenAudio : MonoBehaviour, IBuildValidation, ITickSystemTick
{
	// Token: 0x1700004E RID: 78
	// (get) Token: 0x0600044D RID: 1101 RVA: 0x00019AFA File Offset: 0x00017CFA
	// (set) Token: 0x0600044E RID: 1102 RVA: 0x00019B02 File Offset: 0x00017D02
	bool ITickSystemTick.TickRunning { get; set; }

	// Token: 0x0600044F RID: 1103 RVA: 0x00019B0B File Offset: 0x00017D0B
	private void OnEnable()
	{
		TickSystem<object>.AddCallbackTarget(this);
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x00019B13 File Offset: 0x00017D13
	private void OnDisable()
	{
		TickSystem<object>.RemoveCallbackTarget(this);
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00019B1C File Offset: 0x00017D1C
	public bool BuildValidationCheck()
	{
		if (this.button == null)
		{
			Debug.LogError("reference button missing for doorslidingopenaudio", base.gameObject);
			return false;
		}
		if (this.audioSource == null)
		{
			Debug.LogError("missing audio source on doorslidingopenaudio", base.gameObject);
			return false;
		}
		return true;
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00019B6C File Offset: 0x00017D6C
	void ITickSystemTick.Tick()
	{
		if (this.button.ghostLab.IsDoorMoving(this.button.forSingleDoor, this.button.buttonIndex))
		{
			if (!this.audioSource.isPlaying)
			{
				this.audioSource.time = 0f;
				this.audioSource.GTPlay();
				return;
			}
		}
		else if (this.audioSource.isPlaying)
		{
			this.audioSource.time = 0f;
			this.audioSource.GTStop();
		}
	}

	// Token: 0x04000503 RID: 1283
	public GhostLabButton button;

	// Token: 0x04000504 RID: 1284
	public AudioSource audioSource;
}
