using System;
using UnityEngine;

// Token: 0x0200083F RID: 2111
public class AmbientSoundRandomizer : MonoBehaviour
{
	// Token: 0x060033BA RID: 13242 RVA: 0x0005202F File Offset: 0x0005022F
	private void Button_Cache()
	{
		this.audioSources = base.GetComponentsInChildren<AudioSource>();
	}

	// Token: 0x060033BB RID: 13243 RVA: 0x0005203D File Offset: 0x0005023D
	private void Awake()
	{
		this.SetTarget();
	}

	// Token: 0x060033BC RID: 13244 RVA: 0x0013C040 File Offset: 0x0013A240
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

	// Token: 0x060033BD RID: 13245 RVA: 0x00052045 File Offset: 0x00050245
	private void SetTarget()
	{
		this.timerTarget = this.baseTime + UnityEngine.Random.Range(0f, this.randomModifier);
		this.timer = 0f;
	}

	// Token: 0x04003733 RID: 14131
	[SerializeField]
	private AudioSource[] audioSources;

	// Token: 0x04003734 RID: 14132
	[SerializeField]
	private AudioClip[] audioClips;

	// Token: 0x04003735 RID: 14133
	[SerializeField]
	private float baseTime = 15f;

	// Token: 0x04003736 RID: 14134
	[SerializeField]
	private float randomModifier = 5f;

	// Token: 0x04003737 RID: 14135
	private float timer;

	// Token: 0x04003738 RID: 14136
	private float timerTarget;
}
