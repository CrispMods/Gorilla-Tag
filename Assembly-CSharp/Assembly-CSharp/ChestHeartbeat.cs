using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000506 RID: 1286
public class ChestHeartbeat : MonoBehaviour
{
	// Token: 0x06001F41 RID: 8001 RVA: 0x0009E2EC File Offset: 0x0009C4EC
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

	// Token: 0x06001F42 RID: 8002 RVA: 0x0009E3FA File Offset: 0x0009C5FA
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

	// Token: 0x0400231A RID: 8986
	public int millisToWait;

	// Token: 0x0400231B RID: 8987
	public int millisMin = 300;

	// Token: 0x0400231C RID: 8988
	public int lastShot;

	// Token: 0x0400231D RID: 8989
	public AudioSource audioSource;

	// Token: 0x0400231E RID: 8990
	public Transform scaleTransform;

	// Token: 0x0400231F RID: 8991
	private float deltaTime;

	// Token: 0x04002320 RID: 8992
	private float heartMinSize = 0.9f;

	// Token: 0x04002321 RID: 8993
	private float heartMaxSize = 1.2f;

	// Token: 0x04002322 RID: 8994
	private float minTime = 0.05f;

	// Token: 0x04002323 RID: 8995
	private float maxTime = 0.1f;

	// Token: 0x04002324 RID: 8996
	private float endtime = 0.25f;

	// Token: 0x04002325 RID: 8997
	private float currentTime;
}
