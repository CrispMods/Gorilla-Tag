using System;
using System.Collections;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020006B2 RID: 1714
public class HalloweenWatcherEyes : MonoBehaviour
{
	// Token: 0x06002A9E RID: 10910 RVA: 0x0011D4A0 File Offset: 0x0011B6A0
	private void Start()
	{
		this.playersViewCenterCosAngle = Mathf.Cos(this.playersViewCenterAngle * 0.017453292f);
		this.watchMinCosAngle = Mathf.Cos(this.watchMaxAngle * 0.017453292f);
		base.StartCoroutine(this.CheckIfNearPlayer(UnityEngine.Random.Range(0f, this.timeBetweenUpdates)));
		base.enabled = false;
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x0004CD36 File Offset: 0x0004AF36
	private IEnumerator CheckIfNearPlayer(float initialSleep)
	{
		yield return new WaitForSeconds(initialSleep);
		for (;;)
		{
			base.enabled = ((base.transform.position - GTPlayer.Instance.transform.position).sqrMagnitude < this.watchRange * this.watchRange);
			if (!base.enabled)
			{
				this.LookNormal();
			}
			yield return new WaitForSeconds(this.timeBetweenUpdates);
		}
		yield break;
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x0011D500 File Offset: 0x0011B700
	private void Update()
	{
		Vector3 normalized = (GTPlayer.Instance.headCollider.transform.position - base.transform.position).normalized;
		if (Vector3.Dot(GTPlayer.Instance.headCollider.transform.forward, -normalized) > this.playersViewCenterCosAngle)
		{
			this.LookNormal();
			this.pretendingToBeNormalUntilTimestamp = Time.time + this.durationToBeNormalWhenPlayerLooks;
		}
		if (this.pretendingToBeNormalUntilTimestamp > Time.time)
		{
			return;
		}
		if (Vector3.Dot(base.transform.forward, normalized) < this.watchMinCosAngle)
		{
			this.LookNormal();
			return;
		}
		Quaternion b = Quaternion.LookRotation(normalized, base.transform.up);
		Quaternion rotation = Quaternion.Lerp(base.transform.rotation, b, this.lerpValue);
		this.leftEye.transform.rotation = rotation;
		this.rightEye.transform.rotation = rotation;
		if (this.lerpDuration > 0f)
		{
			this.lerpValue = Mathf.MoveTowards(this.lerpValue, 1f, Time.deltaTime / this.lerpDuration);
			return;
		}
		this.lerpValue = 1f;
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x0004CD4C File Offset: 0x0004AF4C
	private void LookNormal()
	{
		this.leftEye.transform.localRotation = Quaternion.identity;
		this.rightEye.transform.localRotation = Quaternion.identity;
		this.lerpValue = 0f;
	}

	// Token: 0x04003029 RID: 12329
	public float timeBetweenUpdates = 5f;

	// Token: 0x0400302A RID: 12330
	public float watchRange;

	// Token: 0x0400302B RID: 12331
	public float watchMaxAngle;

	// Token: 0x0400302C RID: 12332
	public float lerpDuration = 1f;

	// Token: 0x0400302D RID: 12333
	public float playersViewCenterAngle = 30f;

	// Token: 0x0400302E RID: 12334
	public float durationToBeNormalWhenPlayerLooks = 3f;

	// Token: 0x0400302F RID: 12335
	public GameObject leftEye;

	// Token: 0x04003030 RID: 12336
	public GameObject rightEye;

	// Token: 0x04003031 RID: 12337
	private float playersViewCenterCosAngle;

	// Token: 0x04003032 RID: 12338
	private float watchMinCosAngle;

	// Token: 0x04003033 RID: 12339
	private float pretendingToBeNormalUntilTimestamp;

	// Token: 0x04003034 RID: 12340
	private float lerpValue;
}
