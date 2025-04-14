using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020000DA RID: 218
public class HorseStickNoiseMaker : MonoBehaviour
{
	// Token: 0x06000590 RID: 1424 RVA: 0x00020CA0 File Offset: 0x0001EEA0
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

	// Token: 0x06000591 RID: 1425 RVA: 0x00020D48 File Offset: 0x0001EF48
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

	// Token: 0x0400068C RID: 1676
	[Tooltip("Meters the object should traverse between playing a provided audio clip.")]
	public float metersPerClip = 4f;

	// Token: 0x0400068D RID: 1677
	[Tooltip("Number of seconds that must elapse before playing another audio clip.")]
	public float minSecBetweenClips = 1.5f;

	// Token: 0x0400068E RID: 1678
	public SoundBankPlayer soundBankPlayer;

	// Token: 0x0400068F RID: 1679
	[Tooltip("Transform assigned in Gorilla Player Networked Prefab to the Gorilla Player Networked parent to keep track of distance traveled.")]
	public Transform gorillaPlayerXform;

	// Token: 0x04000690 RID: 1680
	[Delayed]
	public string gorillaPlayerXform_path;

	// Token: 0x04000691 RID: 1681
	[Tooltip("Optional particle FX to spawn when sound plays")]
	public ParticleSystem particleFX;

	// Token: 0x04000692 RID: 1682
	private Vector3 oldPos;

	// Token: 0x04000693 RID: 1683
	private float timeSincePlay;

	// Token: 0x04000694 RID: 1684
	private float distElapsed;
}
