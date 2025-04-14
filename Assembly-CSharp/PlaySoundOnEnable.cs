using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013F RID: 319
public class PlaySoundOnEnable : MonoBehaviour
{
	// Token: 0x06000837 RID: 2103 RVA: 0x0002D155 File Offset: 0x0002B355
	private void Reset()
	{
		this._source = base.GetComponent<AudioSource>();
		if (this._source)
		{
			this._source.playOnAwake = false;
		}
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0002D17C File Offset: 0x0002B37C
	private void OnEnable()
	{
		this.Play();
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x0002D184 File Offset: 0x0002B384
	private void OnDisable()
	{
		this.Stop();
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x0002D18C File Offset: 0x0002B38C
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

	// Token: 0x0600083B RID: 2107 RVA: 0x0002D236 File Offset: 0x0002B436
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

	// Token: 0x0600083C RID: 2108 RVA: 0x0002D245 File Offset: 0x0002B445
	public void Stop()
	{
		this._source.GTStop();
		this._source.loop = false;
	}

	// Token: 0x040009A4 RID: 2468
	[SerializeField]
	private AudioSource _source;

	// Token: 0x040009A5 RID: 2469
	[SerializeField]
	private AudioClip[] _clips;

	// Token: 0x040009A6 RID: 2470
	[SerializeField]
	private bool _loop;

	// Token: 0x040009A7 RID: 2471
	[SerializeField]
	private Vector2 _loopDelay;
}
