using System;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class DoorSlidingOpenAudio : MonoBehaviour, IBuildValidation, ITickSystemTick
{
	// Token: 0x1700004E RID: 78
	// (get) Token: 0x0600044F RID: 1103 RVA: 0x00019E1E File Offset: 0x0001801E
	// (set) Token: 0x06000450 RID: 1104 RVA: 0x00019E26 File Offset: 0x00018026
	bool ITickSystemTick.TickRunning { get; set; }

	// Token: 0x06000451 RID: 1105 RVA: 0x00019E2F File Offset: 0x0001802F
	private void OnEnable()
	{
		TickSystem<object>.AddCallbackTarget(this);
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00019E37 File Offset: 0x00018037
	private void OnDisable()
	{
		TickSystem<object>.RemoveCallbackTarget(this);
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00019E40 File Offset: 0x00018040
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

	// Token: 0x06000454 RID: 1108 RVA: 0x00019E90 File Offset: 0x00018090
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

	// Token: 0x04000504 RID: 1284
	public GhostLabButton button;

	// Token: 0x04000505 RID: 1285
	public AudioSource audioSource;
}
