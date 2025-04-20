using System;
using System.Collections;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005FC RID: 1532
public class MusicFadeArea : MonoBehaviour
{
	// Token: 0x0600262A RID: 9770 RVA: 0x00108050 File Offset: 0x00106250
	private void Awake()
	{
		for (int i = 0; i < this.sourcesToFadeIn.Count; i++)
		{
			this.sourcesToFadeIn[i].audioSource.Stop();
			this.sourcesToFadeIn[i].audioSource.volume = 0f;
		}
	}

	// Token: 0x0600262B RID: 9771 RVA: 0x001080A4 File Offset: 0x001062A4
	private void OnTriggerEnter(Collider other)
	{
		if (other == GTPlayer.Instance.headCollider)
		{
			MusicManager.Instance.FadeOutMusic(this.fadeDuration);
			if (this.fadeCoroutine != null)
			{
				base.StopCoroutine(this.fadeCoroutine);
			}
			if (this.sourcesToFadeIn.Count > 0)
			{
				this.fadeCoroutine = base.StartCoroutine(this.FadeInSources());
			}
		}
	}

	// Token: 0x0600262C RID: 9772 RVA: 0x0010810C File Offset: 0x0010630C
	private void OnTriggerExit(Collider other)
	{
		if (other == GTPlayer.Instance.headCollider)
		{
			MusicManager.Instance.FadeInMusic(this.fadeDuration);
			if (this.fadeCoroutine != null)
			{
				base.StopCoroutine(this.fadeCoroutine);
			}
			if (this.sourcesToFadeIn.Count > 0)
			{
				this.fadeCoroutine = base.StartCoroutine(this.FadeOutSources());
			}
		}
	}

	// Token: 0x0600262D RID: 9773 RVA: 0x00049F66 File Offset: 0x00048166
	private IEnumerator FadeInSources()
	{
		for (int i = 0; i < this.sourcesToFadeIn.Count; i++)
		{
			this.sourcesToFadeIn[i].audioSource.Play();
			this.sourcesToFadeIn[i].audioSource.volume = this.sourcesToFadeIn[i].maxVolume * this.fadeProgress;
		}
		while (this.fadeProgress < 1f)
		{
			for (int j = 0; j < this.sourcesToFadeIn.Count; j++)
			{
				this.sourcesToFadeIn[j].audioSource.volume = this.sourcesToFadeIn[j].maxVolume * this.fadeProgress;
			}
			yield return null;
			this.fadeProgress = Mathf.MoveTowards(this.fadeProgress, 1f, Time.deltaTime / this.fadeDuration);
		}
		for (int k = 0; k < this.sourcesToFadeIn.Count; k++)
		{
			this.sourcesToFadeIn[k].audioSource.volume = this.sourcesToFadeIn[k].maxVolume;
		}
		yield break;
	}

	// Token: 0x0600262E RID: 9774 RVA: 0x00049F75 File Offset: 0x00048175
	private IEnumerator FadeOutSources()
	{
		for (int i = 0; i < this.sourcesToFadeIn.Count; i++)
		{
			this.sourcesToFadeIn[i].audioSource.volume = this.sourcesToFadeIn[i].maxVolume * this.fadeProgress;
		}
		while (this.fadeProgress > 0f)
		{
			for (int j = 0; j < this.sourcesToFadeIn.Count; j++)
			{
				this.sourcesToFadeIn[j].audioSource.volume = this.sourcesToFadeIn[j].maxVolume * this.fadeProgress;
			}
			yield return null;
			this.fadeProgress = Mathf.MoveTowards(this.fadeProgress, 0f, Time.deltaTime / this.fadeDuration);
		}
		for (int k = 0; k < this.sourcesToFadeIn.Count; k++)
		{
			this.sourcesToFadeIn[k].audioSource.Stop();
			this.sourcesToFadeIn[k].audioSource.volume = 0f;
		}
		yield break;
	}

	// Token: 0x04002A40 RID: 10816
	[SerializeField]
	private List<MusicFadeArea.AudioSourceEntry> sourcesToFadeIn = new List<MusicFadeArea.AudioSourceEntry>();

	// Token: 0x04002A41 RID: 10817
	[SerializeField]
	private float fadeDuration = 3f;

	// Token: 0x04002A42 RID: 10818
	private float fadeProgress;

	// Token: 0x04002A43 RID: 10819
	private Coroutine fadeCoroutine;

	// Token: 0x020005FD RID: 1533
	[Serializable]
	public struct AudioSourceEntry
	{
		// Token: 0x04002A44 RID: 10820
		public AudioSource audioSource;

		// Token: 0x04002A45 RID: 10821
		public float maxVolume;
	}
}
