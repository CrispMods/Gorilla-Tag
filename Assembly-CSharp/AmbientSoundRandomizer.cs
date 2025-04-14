using System;
using UnityEngine;

// Token: 0x02000825 RID: 2085
public class AmbientSoundRandomizer : MonoBehaviour
{
	// Token: 0x060032FF RID: 13055 RVA: 0x000F4129 File Offset: 0x000F2329
	private void Button_Cache()
	{
		this.audioSources = base.GetComponentsInChildren<AudioSource>();
	}

	// Token: 0x06003300 RID: 13056 RVA: 0x000F4137 File Offset: 0x000F2337
	private void Awake()
	{
		this.SetTarget();
	}

	// Token: 0x06003301 RID: 13057 RVA: 0x000F4140 File Offset: 0x000F2340
	private void Update()
	{
		if (this.timer >= this.timerTarget)
		{
			int num = Random.Range(0, this.audioSources.Length);
			int num2 = Random.Range(0, this.audioClips.Length);
			this.audioSources[num].clip = this.audioClips[num2];
			this.audioSources[num].GTPlay();
			this.SetTarget();
			return;
		}
		this.timer += Time.deltaTime;
	}

	// Token: 0x06003302 RID: 13058 RVA: 0x000F41B4 File Offset: 0x000F23B4
	private void SetTarget()
	{
		this.timerTarget = this.baseTime + Random.Range(0f, this.randomModifier);
		this.timer = 0f;
	}

	// Token: 0x04003677 RID: 13943
	[SerializeField]
	private AudioSource[] audioSources;

	// Token: 0x04003678 RID: 13944
	[SerializeField]
	private AudioClip[] audioClips;

	// Token: 0x04003679 RID: 13945
	[SerializeField]
	private float baseTime = 15f;

	// Token: 0x0400367A RID: 13946
	[SerializeField]
	private float randomModifier = 5f;

	// Token: 0x0400367B RID: 13947
	private float timer;

	// Token: 0x0400367C RID: 13948
	private float timerTarget;
}
