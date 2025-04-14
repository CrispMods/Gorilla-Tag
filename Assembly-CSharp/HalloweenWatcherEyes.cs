using System;
using System.Collections;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200069D RID: 1693
public class HalloweenWatcherEyes : MonoBehaviour
{
	// Token: 0x06002A08 RID: 10760 RVA: 0x000D0DB4 File Offset: 0x000CEFB4
	private void Start()
	{
		this.playersViewCenterCosAngle = Mathf.Cos(this.playersViewCenterAngle * 0.017453292f);
		this.watchMinCosAngle = Mathf.Cos(this.watchMaxAngle * 0.017453292f);
		base.StartCoroutine(this.CheckIfNearPlayer(Random.Range(0f, this.timeBetweenUpdates)));
		base.enabled = false;
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x000D0E13 File Offset: 0x000CF013
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

	// Token: 0x06002A0A RID: 10762 RVA: 0x000D0E2C File Offset: 0x000CF02C
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

	// Token: 0x06002A0B RID: 10763 RVA: 0x000D0F5A File Offset: 0x000CF15A
	private void LookNormal()
	{
		this.leftEye.transform.localRotation = Quaternion.identity;
		this.rightEye.transform.localRotation = Quaternion.identity;
		this.lerpValue = 0f;
	}

	// Token: 0x04002F8C RID: 12172
	public float timeBetweenUpdates = 5f;

	// Token: 0x04002F8D RID: 12173
	public float watchRange;

	// Token: 0x04002F8E RID: 12174
	public float watchMaxAngle;

	// Token: 0x04002F8F RID: 12175
	public float lerpDuration = 1f;

	// Token: 0x04002F90 RID: 12176
	public float playersViewCenterAngle = 30f;

	// Token: 0x04002F91 RID: 12177
	public float durationToBeNormalWhenPlayerLooks = 3f;

	// Token: 0x04002F92 RID: 12178
	public GameObject leftEye;

	// Token: 0x04002F93 RID: 12179
	public GameObject rightEye;

	// Token: 0x04002F94 RID: 12180
	private float playersViewCenterCosAngle;

	// Token: 0x04002F95 RID: 12181
	private float watchMinCosAngle;

	// Token: 0x04002F96 RID: 12182
	private float pretendingToBeNormalUntilTimestamp;

	// Token: 0x04002F97 RID: 12183
	private float lerpValue;
}
