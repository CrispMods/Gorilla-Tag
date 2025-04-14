using System;
using System.Collections;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200069E RID: 1694
public class HalloweenWatcherEyes : MonoBehaviour
{
	// Token: 0x06002A10 RID: 10768 RVA: 0x000D1234 File Offset: 0x000CF434
	private void Start()
	{
		this.playersViewCenterCosAngle = Mathf.Cos(this.playersViewCenterAngle * 0.017453292f);
		this.watchMinCosAngle = Mathf.Cos(this.watchMaxAngle * 0.017453292f);
		base.StartCoroutine(this.CheckIfNearPlayer(Random.Range(0f, this.timeBetweenUpdates)));
		base.enabled = false;
	}

	// Token: 0x06002A11 RID: 10769 RVA: 0x000D1293 File Offset: 0x000CF493
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

	// Token: 0x06002A12 RID: 10770 RVA: 0x000D12AC File Offset: 0x000CF4AC
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

	// Token: 0x06002A13 RID: 10771 RVA: 0x000D13DA File Offset: 0x000CF5DA
	private void LookNormal()
	{
		this.leftEye.transform.localRotation = Quaternion.identity;
		this.rightEye.transform.localRotation = Quaternion.identity;
		this.lerpValue = 0f;
	}

	// Token: 0x04002F92 RID: 12178
	public float timeBetweenUpdates = 5f;

	// Token: 0x04002F93 RID: 12179
	public float watchRange;

	// Token: 0x04002F94 RID: 12180
	public float watchMaxAngle;

	// Token: 0x04002F95 RID: 12181
	public float lerpDuration = 1f;

	// Token: 0x04002F96 RID: 12182
	public float playersViewCenterAngle = 30f;

	// Token: 0x04002F97 RID: 12183
	public float durationToBeNormalWhenPlayerLooks = 3f;

	// Token: 0x04002F98 RID: 12184
	public GameObject leftEye;

	// Token: 0x04002F99 RID: 12185
	public GameObject rightEye;

	// Token: 0x04002F9A RID: 12186
	private float playersViewCenterCosAngle;

	// Token: 0x04002F9B RID: 12187
	private float watchMinCosAngle;

	// Token: 0x04002F9C RID: 12188
	private float pretendingToBeNormalUntilTimestamp;

	// Token: 0x04002F9D RID: 12189
	private float lerpValue;
}
