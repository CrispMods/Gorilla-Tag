using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class PlaySoundOnEnable : MonoBehaviour
{
	// Token: 0x0600087B RID: 2171 RVA: 0x00035FB5 File Offset: 0x000341B5
	private void Reset()
	{
		this._source = base.GetComponent<AudioSource>();
		if (this._source)
		{
			this._source.playOnAwake = false;
		}
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00035FDC File Offset: 0x000341DC
	private void OnEnable()
	{
		this.Play();
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00035FE4 File Offset: 0x000341E4
	private void OnDisable()
	{
		this.Stop();
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x0008E3BC File Offset: 0x0008C5BC
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
		this._source.clip = this._clips[UnityEngine.Random.Range(0, this._clips.Length)];
		this._source.GTPlay();
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00035FEC File Offset: 0x000341EC
	private IEnumerator DoLoop()
	{
		while (base.enabled)
		{
			this._source.clip = this._clips[UnityEngine.Random.Range(0, this._clips.Length)];
			this._source.GTPlay();
			while (this._source.isPlaying)
			{
				yield return null;
			}
			float num = UnityEngine.Random.Range(this._loopDelay.x, this._loopDelay.y);
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

	// Token: 0x06000880 RID: 2176 RVA: 0x00035FFB File Offset: 0x000341FB
	public void Stop()
	{
		this._source.GTStop();
		this._source.loop = false;
	}

	// Token: 0x040009E7 RID: 2535
	[SerializeField]
	private AudioSource _source;

	// Token: 0x040009E8 RID: 2536
	[SerializeField]
	private AudioClip[] _clips;

	// Token: 0x040009E9 RID: 2537
	[SerializeField]
	private bool _loop;

	// Token: 0x040009EA RID: 2538
	[SerializeField]
	private Vector2 _loopDelay;
}
