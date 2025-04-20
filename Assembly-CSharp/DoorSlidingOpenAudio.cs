using System;
using UnityEngine;

// Token: 0x020000B3 RID: 179
public class DoorSlidingOpenAudio : MonoBehaviour, IBuildValidation, ITickSystemTick
{
	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06000489 RID: 1161 RVA: 0x00033672 File Offset: 0x00031872
	// (set) Token: 0x0600048A RID: 1162 RVA: 0x0003367A File Offset: 0x0003187A
	bool ITickSystemTick.TickRunning { get; set; }

	// Token: 0x0600048B RID: 1163 RVA: 0x00033683 File Offset: 0x00031883
	private void OnEnable()
	{
		TickSystem<object>.AddCallbackTarget(this);
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x0003368B File Offset: 0x0003188B
	private void OnDisable()
	{
		TickSystem<object>.RemoveCallbackTarget(this);
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x0007D524 File Offset: 0x0007B724
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

	// Token: 0x0600048E RID: 1166 RVA: 0x0007D574 File Offset: 0x0007B774
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

	// Token: 0x04000543 RID: 1347
	public GhostLabButton button;

	// Token: 0x04000544 RID: 1348
	public AudioSource audioSource;
}
