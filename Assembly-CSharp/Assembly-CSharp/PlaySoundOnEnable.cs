using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013F RID: 319
public class PlaySoundOnEnable : MonoBehaviour
{
	// Token: 0x06000839 RID: 2105 RVA: 0x0002D479 File Offset: 0x0002B679
	private void Reset()
	{
		this._source = base.GetComponent<AudioSource>();
		if (this._source)
		{
			this._source.playOnAwake = false;
		}
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x0002D4A0 File Offset: 0x0002B6A0
	private void OnEnable()
	{
		this.Play();
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x0002D4A8 File Offset: 0x0002B6A8
	private void OnDisable()
	{
		this.Stop();
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x0002D4B0 File Offset: 0x0002B6B0
	public void Play()
	{
		if (this._loop && this._clips.Length == 1 && this._loopDelay == Vector2.zero)
		{
			this._source.clip = this._clips[0];
			this._source.loop = true;
			this._source.GTPlay();
			return;
		}
		this._source.loop = false;
		if (this._loop)
		{
			base.StartCoroutine(this.DoLoop());
			return;
		}
		this._source.clip = this._clips[Random.Range(0, this._clips.Length)];
		this._source.GTPlay();
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x0002D55A File Offset: 0x0002B75A
	private IEnumerator DoLoop()
	{
		while (base.enabled)
		{
			this._source.clip = this._clips[Random.Range(0, this._clips.Length)];
			this._source.GTPlay();
			while (this._source.isPlaying)
			{
				yield return null;
			}
			float num = Random.Range(this._loopDelay.x, this._loopDelay.y);
			if (num > 0f)
			{
				float waitEndTime = Time.time + num;
				while (Time.time < waitEndTime)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x0002D569 File Offset: 0x0002B769
	public void Stop()
	{
		this._source.GTStop();
		this._source.loop = false;
	}

	// Token: 0x040009A5 RID: 2469
	[SerializeField]
	private AudioSource _source;

	// Token: 0x040009A6 RID: 2470
	[SerializeField]
	private AudioClip[] _clips;

	// Token: 0x040009A7 RID: 2471
	[SerializeField]
	private bool _loop;

	// Token: 0x040009A8 RID: 2472
	[SerializeField]
	private Vector2 _loopDelay;
}
