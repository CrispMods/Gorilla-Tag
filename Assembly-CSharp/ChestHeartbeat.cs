using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000513 RID: 1299
public class ChestHeartbeat : MonoBehaviour
{
	// Token: 0x06001F97 RID: 8087 RVA: 0x000EFD80 File Offset: 0x000EDF80
	public void Update()
	{
		if (PhotonNetwork.InRoom)
		{
			if ((PhotonNetwork.ServerTimestamp > this.lastShot + this.millisMin || Mathf.Abs(PhotonNetwork.ServerTimestamp - this.lastShot) > 10000) && PhotonNetwork.ServerTimestamp % 1500 <= 10)
			{
				this.lastShot = PhotonNetwork.ServerTimestamp;
				this.audioSource.GTPlayOneShot(this.audioSource.clip, 1f);
				base.StartCoroutine(this.HeartBeat());
				return;
			}
		}
		else if ((Time.time * 1000f > (float)(this.lastShot + this.millisMin) || Mathf.Abs(Time.time * 1000f - (float)this.lastShot) > 10000f) && Time.time * 1000f % 1500f <= 10f)
		{
			this.lastShot = PhotonNetwork.ServerTimestamp;
			this.audioSource.GTPlayOneShot(this.audioSource.clip, 1f);
			base.StartCoroutine(this.HeartBeat());
		}
	}

	// Token: 0x06001F98 RID: 8088 RVA: 0x00045740 File Offset: 0x00043940
	private IEnumerator HeartBeat()
	{
		float startTime = Time.time;
		while (Time.time < startTime + this.endtime)
		{
			if (Time.time < startTime + this.minTime)
			{
				this.deltaTime = Time.time - startTime;
				this.scaleTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * this.heartMinSize, this.deltaTime / this.minTime);
			}
			else if (Time.time < startTime + this.maxTime)
			{
				this.deltaTime = Time.time - startTime - this.minTime;
				this.scaleTransform.localScale = Vector3.Lerp(Vector3.one * this.heartMinSize, Vector3.one * this.heartMaxSize, this.deltaTime / (this.maxTime - this.minTime));
			}
			else if (Time.time < startTime + this.endtime)
			{
				this.deltaTime = Time.time - startTime - this.maxTime;
				this.scaleTransform.localScale = Vector3.Lerp(Vector3.one * this.heartMaxSize, Vector3.one, this.deltaTime / (this.endtime - this.maxTime));
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x0400236C RID: 9068
	public int millisToWait;

	// Token: 0x0400236D RID: 9069
	public int millisMin = 300;

	// Token: 0x0400236E RID: 9070
	public int lastShot;

	// Token: 0x0400236F RID: 9071
	public AudioSource audioSource;

	// Token: 0x04002370 RID: 9072
	public Transform scaleTransform;

	// Token: 0x04002371 RID: 9073
	private float deltaTime;

	// Token: 0x04002372 RID: 9074
	private float heartMinSize = 0.9f;

	// Token: 0x04002373 RID: 9075
	private float heartMaxSize = 1.2f;

	// Token: 0x04002374 RID: 9076
	private float minTime = 0.05f;

	// Token: 0x04002375 RID: 9077
	private float maxTime = 0.1f;

	// Token: 0x04002376 RID: 9078
	private float endtime = 0.25f;

	// Token: 0x04002377 RID: 9079
	private float currentTime;
}
