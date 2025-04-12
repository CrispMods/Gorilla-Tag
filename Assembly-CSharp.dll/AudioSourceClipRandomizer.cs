using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200000D RID: 13
[RequireComponent(typeof(AudioSource))]
public class AudioSourceClipRandomizer : MonoBehaviour
{
	// Token: 0x06000038 RID: 56 RVA: 0x0002F875 File Offset: 0x0002DA75
	private void Awake()
	{
		this.source = base.GetComponent<AudioSource>();
		this.playOnAwake = this.source.playOnAwake;
		this.source.playOnAwake = false;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x0006611C File Offset: 0x0006431C
	public void Play()
	{
		int num = UnityEngine.Random.Range(0, 60);
		if (GorillaComputer.instance != null)
		{
			num = GorillaComputer.instance.GetServerTime().Second;
		}
		this.source.clip = this.clips[num % this.clips.Length];
		this.source.GTPlay();
	}

	// Token: 0x0600003A RID: 58 RVA: 0x0002F8A0 File Offset: 0x0002DAA0
	private void OnEnable()
	{
		if (this.playOnAwake)
		{
			this.Play();
		}
	}

	// Token: 0x04000019 RID: 25
	[SerializeField]
	private AudioClip[] clips;

	// Token: 0x0400001A RID: 26
	private AudioSource source;

	// Token: 0x0400001B RID: 27
	private bool playOnAwake;
}
