using System;
using UnityEngine;

// Token: 0x02000828 RID: 2088
public class AmbientSoundRandomizer : MonoBehaviour
{
	// Token: 0x0600330B RID: 13067 RVA: 0x00050C21 File Offset: 0x0004EE21
	private void Button_Cache()
	{
		this.audioSources = base.GetComponentsInChildren<AudioSource>();
	}

	// Token: 0x0600330C RID: 13068 RVA: 0x00050C2F File Offset: 0x0004EE2F
	private void Awake()
	{
		this.SetTarget();
	}

	// Token: 0x0600330D RID: 13069 RVA: 0x00136AE8 File Offset: 0x00134CE8
	private void Update()
	{
		if (this.timer >= this.timerTarget)
		{
			int num = UnityEngine.Random.Range(0, this.audioSources.Length);
			int num2 = UnityEngine.Random.Range(0, this.audioClips.Length);
			this.audioSources[num].clip = this.audioClips[num2];
			this.audioSources[num].GTPlay();
			this.SetTarget();
			return;
		}
		this.timer += Time.deltaTime;
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x00050C37 File Offset: 0x0004EE37
	private void SetTarget()
	{
		this.timerTarget = this.baseTime + UnityEngine.Random.Range(0f, this.randomModifier);
		this.timer = 0f;
	}

	// Token: 0x04003689 RID: 13961
	[SerializeField]
	private AudioSource[] audioSources;

	// Token: 0x0400368A RID: 13962
	[SerializeField]
	private AudioClip[] audioClips;

	// Token: 0x0400368B RID: 13963
	[SerializeField]
	private float baseTime = 15f;

	// Token: 0x0400368C RID: 13964
	[SerializeField]
	private float randomModifier = 5f;

	// Token: 0x0400368D RID: 13965
	private float timer;

	// Token: 0x0400368E RID: 13966
	private float timerTarget;
}
