using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class HorseStickNoiseMaker : MonoBehaviour
{
	// Token: 0x060005D1 RID: 1489 RVA: 0x00083918 File Offset: 0x00081B18
	protected void OnEnable()
	{
		if (!this.gorillaPlayerXform && !base.transform.TryFindByPath(this.gorillaPlayerXform_path, out this.gorillaPlayerXform, false))
		{
			Debug.LogError(string.Concat(new string[]
			{
				"HorseStickNoiseMaker: DEACTIVATING! Could not find gorillaPlayerXform using path: \"",
				this.gorillaPlayerXform_path,
				"\"\nThis component's transform path: \"",
				base.transform.GetPath(),
				"\""
			}));
			base.gameObject.SetActive(false);
			return;
		}
		this.oldPos = this.gorillaPlayerXform.position;
		this.distElapsed = 0f;
		this.timeSincePlay = 0f;
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x000839C0 File Offset: 0x00081BC0
	protected void LateUpdate()
	{
		Vector3 position = this.gorillaPlayerXform.position;
		Vector3 vector = position - this.oldPos;
		this.distElapsed += vector.magnitude;
		this.timeSincePlay += Time.deltaTime;
		this.oldPos = position;
		if (this.distElapsed >= this.metersPerClip && this.timeSincePlay >= this.minSecBetweenClips)
		{
			this.soundBankPlayer.Play();
			this.distElapsed = 0f;
			this.timeSincePlay = 0f;
			if (this.particleFX != null)
			{
				this.particleFX.Play();
			}
		}
	}

	// Token: 0x040006CD RID: 1741
	[Tooltip("Meters the object should traverse between playing a provided audio clip.")]
	public float metersPerClip = 4f;

	// Token: 0x040006CE RID: 1742
	[Tooltip("Number of seconds that must elapse before playing another audio clip.")]
	public float minSecBetweenClips = 1.5f;

	// Token: 0x040006CF RID: 1743
	public SoundBankPlayer soundBankPlayer;

	// Token: 0x040006D0 RID: 1744
	[Tooltip("Transform assigned in Gorilla Player Networked Prefab to the Gorilla Player Networked parent to keep track of distance traveled.")]
	public Transform gorillaPlayerXform;

	// Token: 0x040006D1 RID: 1745
	[Delayed]
	public string gorillaPlayerXform_path;

	// Token: 0x040006D2 RID: 1746
	[Tooltip("Optional particle FX to spawn when sound plays")]
	public ParticleSystem particleFX;

	// Token: 0x040006D3 RID: 1747
	private Vector3 oldPos;

	// Token: 0x040006D4 RID: 1748
	private float timeSincePlay;

	// Token: 0x040006D5 RID: 1749
	private float distElapsed;
}
