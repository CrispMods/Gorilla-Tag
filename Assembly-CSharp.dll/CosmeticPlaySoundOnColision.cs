using System;
using System.Collections;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000029 RID: 41
public class CosmeticPlaySoundOnColision : MonoBehaviour
{
	// Token: 0x06000094 RID: 148 RVA: 0x0006858C File Offset: 0x0006678C
	private void Awake()
	{
		this.transferrableObject = base.GetComponentInParent<TransferrableObject>();
		this.soundLookup = new Dictionary<int, int>();
		this.audioSource = base.GetComponent<AudioSource>();
		for (int i = 0; i < this.soundIdRemappings.Length; i++)
		{
			this.soundLookup.Add(this.soundIdRemappings[i].SoundIn, this.soundIdRemappings[i].SoundOut);
		}
	}

	// Token: 0x06000095 RID: 149 RVA: 0x000685F4 File Offset: 0x000667F4
	private void OnTriggerEnter(Collider other)
	{
		GorillaSurfaceOverride gorillaSurfaceOverride;
		if (this.speed >= this.minSpeed && other.TryGetComponent<GorillaSurfaceOverride>(out gorillaSurfaceOverride))
		{
			int soundIndex;
			if (this.soundLookup.TryGetValue(gorillaSurfaceOverride.overrideIndex, out soundIndex))
			{
				this.playSound(soundIndex, this.invokeEventOnOverideSound);
				return;
			}
			this.playSound(this.defaultSound, this.invokeEventOnDefaultSound);
		}
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00068650 File Offset: 0x00066850
	private void playSound(int soundIndex, bool invokeEvent)
	{
		if (soundIndex > -1 && soundIndex < GTPlayer.Instance.materialData.Count)
		{
			if (this.audioSource.isPlaying)
			{
				this.audioSource.GTStop();
				if (this.invokeEventsOnAllClients || this.transferrableObject.IsMyItem())
				{
					this.OnStopPlayback.Invoke();
				}
				if (this.crWaitForStopPlayback != null)
				{
					base.StopCoroutine(this.crWaitForStopPlayback);
					this.crWaitForStopPlayback = null;
				}
			}
			this.audioSource.clip = GTPlayer.Instance.materialData[soundIndex].audio;
			this.audioSource.GTPlay();
			if (invokeEvent && (this.invokeEventsOnAllClients || this.transferrableObject.IsMyItem()))
			{
				this.OnStartPlayback.Invoke();
				this.crWaitForStopPlayback = base.StartCoroutine(this.waitForStopPlayback());
			}
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x0002FBB4 File Offset: 0x0002DDB4
	private IEnumerator waitForStopPlayback()
	{
		while (this.audioSource.isPlaying)
		{
			yield return null;
		}
		if (this.invokeEventsOnAllClients || this.transferrableObject.IsMyItem())
		{
			this.OnStopPlayback.Invoke();
		}
		this.crWaitForStopPlayback = null;
		yield break;
	}

	// Token: 0x06000098 RID: 152 RVA: 0x0002FBC3 File Offset: 0x0002DDC3
	private void FixedUpdate()
	{
		this.speed = Vector3.Distance(base.transform.position, this.previousFramePosition) * Time.fixedDeltaTime * 100f;
		this.previousFramePosition = base.transform.position;
	}

	// Token: 0x040000B2 RID: 178
	[GorillaSoundLookup]
	[SerializeField]
	private int defaultSound = 1;

	// Token: 0x040000B3 RID: 179
	[SerializeField]
	private SoundIdRemapping[] soundIdRemappings;

	// Token: 0x040000B4 RID: 180
	[SerializeField]
	private UnityEvent OnStartPlayback;

	// Token: 0x040000B5 RID: 181
	[SerializeField]
	private UnityEvent OnStopPlayback;

	// Token: 0x040000B6 RID: 182
	[SerializeField]
	private float minSpeed = 0.1f;

	// Token: 0x040000B7 RID: 183
	private TransferrableObject transferrableObject;

	// Token: 0x040000B8 RID: 184
	private Dictionary<int, int> soundLookup;

	// Token: 0x040000B9 RID: 185
	private AudioSource audioSource;

	// Token: 0x040000BA RID: 186
	private Coroutine crWaitForStopPlayback;

	// Token: 0x040000BB RID: 187
	private float speed;

	// Token: 0x040000BC RID: 188
	private Vector3 previousFramePosition;

	// Token: 0x040000BD RID: 189
	[SerializeField]
	private bool invokeEventsOnAllClients;

	// Token: 0x040000BE RID: 190
	[SerializeField]
	private bool invokeEventOnOverideSound = true;

	// Token: 0x040000BF RID: 191
	[SerializeField]
	private bool invokeEventOnDefaultSound;
}
